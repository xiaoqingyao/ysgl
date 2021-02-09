using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Dal.SysDictionary;
using System.Configuration;
using System.Data.SqlClient;

namespace Bll.UserProperty
{
    public class NotePad
    {
        private Bill_NotePad note;
        private NoteDal noteDal = new NoteDal();

        public Bill_NotePad Note
        {
            get { return note; }
            set { note = value; }
        }

        /*
        public NotePad(string userCode, DateTime date)
        {
            note = noteDal.GetNoteByUserDate(userCode, date);
        }
         */

        public NotePad(Bill_NotePad tempNote)
        {
            note = tempNote;
        }

        public void Edit()
        {
            using (SqlConnection conn = new SqlConnection(GetConStr()))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    //noteDal.DeleteNote(note.NoteDate, note.UserCode, tran);
                    if (!string.IsNullOrEmpty(note.Context))
                    {
                        noteDal.InsertNote(note, tran);
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        private string GetConStr()
        {
           return ConfigurationManager.AppSettings["ConnectionStringvUnionDataBase"].ToString().Trim();
        }
    }
}
