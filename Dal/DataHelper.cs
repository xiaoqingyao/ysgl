using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace Dal
{
    public class DataHelper
    {
        public static  string constr =  ConfigurationManager.AppSettings["ConnectionStringvUnionDataBase"].ToString().Trim();

        public static int ExcuteNonQuery(string sql, SqlParameter[] parameter,bool ispro)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand cmd = conn.CreateCommand();
                PrepareCommand(cmd, conn, null, ispro, sql, parameter);
                int cont = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return cont;
            }
        }

        public static int ExcuteNonQuery(string sql, SqlTransaction trans, SqlParameter[] sps, bool ispro)
        {
            SqlCommand cmd = trans.Connection.CreateCommand();
            PrepareCommand(cmd, trans.Connection, trans, false, sql,sps);
            int cont = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return cont;
        }

        public static object ExecuteScalar(string sql, SqlParameter[] parameter, bool ispro)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand cmd = conn.CreateCommand();
                PrepareCommand(cmd, conn, null, ispro, sql, parameter);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static object ExecuteScalar(string sql)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand cmd = conn.CreateCommand();
                PrepareCommand(cmd, conn, null, false, sql, null);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static DataTable GetDataTable(string sql, SqlParameter[] parameter,bool ispro)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand cmd = conn.CreateCommand();
                PrepareCommand(cmd, conn, null, ispro, sql, parameter);
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cmd.Parameters.Clear();
                return dt;
            }
        }

        public static SqlDataReader GetDataReader(string sql, SqlParameter[] parameter)
        {
            SqlConnection conn = new SqlConnection(constr);
            try
            {    
                SqlCommand cmd = conn.CreateCommand();
                PrepareCommand(cmd, conn, null, false, sql, parameter);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return dr;
            }
            catch
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
                throw ;
            }
        }

        //准备
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, bool ispro, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            if (ispro)
                cmd.CommandType = CommandType.StoredProcedure;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        
    }
}
