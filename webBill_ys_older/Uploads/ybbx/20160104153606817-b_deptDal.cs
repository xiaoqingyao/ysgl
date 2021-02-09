using System;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Models;


namespace Dal.Basedata
{
    //b_dept
    public partial class b_deptDal : IDal.DalBase
    {
        public void Init()
        {

            _sql = "select *, Row_Number()over(order by " + _order + ") as crow from b_dept ";
            _sqlCount = "select count(*) from b_dept ";
            _sqlDelete = "delete b_dept";
        }


        public b_deptDal()
        {
            Init();
        }

        public b_deptDal(dynamic obj, params string[] str)
        {
            Init();
            getModel(obj, str);
        }

        public b_deptDal(Utils.ListData obj)
        {
            _order = obj.sort + obj.order;
            Init();
            getData(obj);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public override void Add( dynamic obj, SqlTransaction tran = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into b_dept(");
            strSql.Append("ts,cdef1,cdef2,cdef3,cdef4,cdef5,cdef6,cdef7,cdef8,cdef9,code,name,createDate,createPerson,address,flag,memo,toplevel");
            strSql.Append(") values (");
            strSql.Append("@ts,@cdef1,@cdef2,@cdef3,@cdef4,@cdef5,@cdef6,@cdef7,@cdef8,@cdef9,@code,@name,@createDate,@createPerson,@address,@flag,@memo,@toplevel");
            strSql.Append(") ");

            SqlParameter[] parameters = {
			            new SqlParameter("@ts", SqlDbType.Char,19) ,            
                        new SqlParameter("@cdef1", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cdef2", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cdef3", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cdef4", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cdef5", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cdef6", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cdef7", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cdef8", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cdef9", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@code", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@name", SqlDbType.VarChar,300) ,            
                        new SqlParameter("@createDate", SqlDbType.Char,19) ,            
                        new SqlParameter("@createPerson", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@address", SqlDbType.VarChar,500) ,            
                        new SqlParameter("@flag", SqlDbType.VarChar,20) ,            
                        new SqlParameter("@memo", SqlDbType.VarChar,300) ,            
                        new SqlParameter("@toplevel", SqlDbType.VarChar,50)             
              
            };
            b_dept model = (b_dept)obj;

            parameters[0].Value = SqlNull(model.ts);
            parameters[1].Value = SqlNull(model.cdef1);
            parameters[2].Value = SqlNull(model.cdef2);
            parameters[3].Value = SqlNull(model.cdef3);
            parameters[4].Value = SqlNull(model.cdef4);
            parameters[5].Value = SqlNull(model.cdef5);
            parameters[6].Value = SqlNull(model.cdef6);
            parameters[7].Value = SqlNull(model.cdef7);
            parameters[8].Value = SqlNull(model.cdef8);
            parameters[9].Value = SqlNull(model.cdef9);
            parameters[10].Value = SqlNull(model.code);
            parameters[11].Value = SqlNull(model.name);
            parameters[12].Value = SqlNull(model.createDate);
            parameters[13].Value = SqlNull(model.createPerson);
            parameters[14].Value = SqlNull(model.address);
            parameters[15].Value = SqlNull(model.flag);
            parameters[16].Value = SqlNull(model.memo);
            parameters[17].Value = SqlNull(model.toplevel);
            if (tran == null)
            {
                DataHelper.ExcuteNonQuery(strSql.ToString(), parameters, false);
            }
            else
            {
                DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
            }
        }

        //Save,保存方法，先删除，再增加
         public void Save(b_dept model)
        {
            
            SqlConnection conn = new SqlConnection(Dal.DataHelper.constr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            try {

                Delete(model, tran);
                Add(model, tran);
                tran.Commit();
                conn.Close();
            }catch(Exception e){
                tran.Rollback();
                conn.Close();
                throw e;
            }
        }

        public List<b_dept> ToList()
        {
            return List<b_dept>();
        }


    }
}

