using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace Dal.SysDictionary
{
    public class NoteDal
    {
        public IList<Bill_NotePad> GetNoteByUser(string userCode)
        {
            string sql = @"select * from bill_notePad where userCode=@userCode";
            SqlParameter[] sps = {
                                     new SqlParameter("@userCode",userCode)
                                 };
            return ListMaker(sql, sps);
        }

        public IList<Bill_NotePad> GetNoteByUserDate(string userCode, DateTime notDate)
        {
            string sql = @"select * from bill_notePad where userCode=@userCode and noteDate=@noteDate";
            SqlParameter[] sps = {
                                     new SqlParameter("@userCode",userCode),
                                     new SqlParameter("@noteDate",notDate)
                                 };
            return ListMaker(sql, sps);
        }

        
        public int TodayNoteCount(string userCode, DateTime noteDate)
        {
            string date = noteDate.ToString("yyyy-MM-dd");
            string sql = @"select count(*) from bill_notePad 
                            where userCode=@userCode 
                            and convert(varchar(10),noteDate,121)=@noteDate";
            SqlParameter[] sps = {
                                     new SqlParameter("@userCode",userCode),
                                     new SqlParameter("@noteDate",date)
                                 };
            return Convert.ToInt32(DataHelper.ExecuteScalar(sql, sps, false));
        }

        public void InsertNote(Bill_NotePad note,SqlTransaction tran)
        {
            string sql = @"insert into bill_notePad ( noteDate, userCode, context, noteType) values(
                            @noteDate, @userCode, @context, @noteType)";
            SqlParameter[] sps = {
                                     new SqlParameter("@noteDate",note.NoteDate),
                                     new SqlParameter("@userCode",note.UserCode),
                                     new SqlParameter("@context",note.Context),
                                     new SqlParameter("@noteType",note.NoteType)
                                 };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }

        public void DeleteNote(DateTime noteDate,string userCode, SqlTransaction tran)
        {
            string date = noteDate.ToString("yyyy-MM-dd");
            string sql = @"delete bill_notePad 
                            where userCode=@userCode 
                            and convert(varchar(10),noteDate,121)=@noteDate";
            SqlParameter[] sps = {
                                     new SqlParameter("@userCode",userCode),
                                     new SqlParameter("@noteDate",date)
                                 };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }


        private IList<Bill_NotePad> ListMaker(string sql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_NotePad> list = new List<Bill_NotePad>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_NotePad pad = new Bill_NotePad();
                pad.Context = Convert.ToString(dr["Context"]);
                pad.NoteDate = Convert.ToDateTime(dr["NoteDate"]);
                pad.NoteType = Convert.ToString(dr["NoteType"]);
                pad.UserCode = Convert.ToString(dr["UserCode"]);
                list.Add(pad);
            }
            if (list!=null)
            {
                return list;
            }
            else
            {
                return null;
            }
          
        }
    }
}
