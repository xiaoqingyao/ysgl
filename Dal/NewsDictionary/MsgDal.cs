using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Models;

namespace Dal.NewsDictionary
{
    public class MsgDal
    {
        string selsql = "select id, title, contents, writer, date, readTimes, mstype, notifierid,notifiername,endtime,Accessories,Row_Number()over(order by date desc) crow  from bill_msg ";
        /// <summary>
        /// 得到所有消息
        /// </summary>
        public IList<Bill_Msg> GetNews()
        {
            string strsql = @"select top 5 id, title, contents, writer, date, readTimes, mstype, notifierid,notifiername,endtime,Accessories,Row_Number()over(order by id desc) crow  from bill_msg ";
            string strtime = DateTime.Now.ToString("yyyy-MM-dd");
            StringBuilder strSql = new StringBuilder();
            strSql.Append(strsql);
            strSql.Append(" where mstype='新闻'and endtime>='" + strtime + "'");

            return ListMaker(strSql.ToString(), null);
        }

        /// <summary>
        /// 根据发布人得到通知
        /// </summary>
        public IList<Bill_Msg> GetMessageByMaker(string userCode)
        {
            string strtime=DateTime.Now.ToString("yyyy-MM-dd");
            StringBuilder strSql = new StringBuilder();
            strSql.Append(selsql);
            strSql.Append(" where mstype='通知' and notifiername=@userCode and endtime>='" + strtime + "'");
            SqlParameter[] sps = { 
                                     new SqlParameter("@userCode",userCode)
                                 };

            return ListMaker(strSql.ToString(), sps);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tran"></param>
        public int InsertModel(Bill_Msg model)
        {
            string sql = @"insert into bill_msg(title,contents,writer,date,readtimes,mstype,notifierid,notifiername,endtime,Accessories) values(
                            @title, @contents, @writer, @date,@readtimes,@mstype,@notifierid,@notifiername,@endtime,@Accessories)";
            SqlParameter[] sps = {
                                     new SqlParameter("@title",model.Title),
                                     new SqlParameter("@contents",model.Contents),
                                     new SqlParameter("@writer",model.Writer),
                                     new SqlParameter("@date",model.Date),
                                     new SqlParameter("@readtimes",model.ReadTimes),
                                     new SqlParameter("@mstype",model.Mstype),
                                     new SqlParameter("@notifierid",model.Notifierid),
                                      new SqlParameter("@notifiername",model.Notifiername),
                                       new SqlParameter("@endtime",model.Endtime),
                                       new SqlParameter("@Accessories",model.Accessories)
                                 };
           return DataHelper.ExcuteNonQuery(sql, sps, false);
        }


        public int updateModel(Bill_Msg model,string strusername, string strnid) 
        {
            string sql = @"update bill_msg set title=@title,contents=@contents,mstype=@mstype,notifiername=@notifiername,endtime=@endtime,Accessories=@Accessories where writer='" + strusername + "' and id='" + strnid + "'";
            SqlParameter[] sps = {
                                     new SqlParameter("@title",model.Title),
                                     new SqlParameter("@contents",model.Contents),
                                     new SqlParameter("@mstype",model.Mstype),
                                      new SqlParameter("@notifiername",model.Notifiername),
                                       new SqlParameter("@endtime",model.Endtime),
                                       new SqlParameter ("@Accessories",model.Accessories)
                                 };
           return DataHelper.ExcuteNonQuery(sql, sps, false);
        }
        private IList<Bill_Msg> ListMaker(string sql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Msg> list = new List<Bill_Msg>();

            foreach (DataRow dr in dt.Rows)
            {

                Bill_Msg model = new Bill_Msg();
                model.ID =int.Parse(dr["id"].ToString());
                model.Title = dr["title"].ToString();
                model.Contents = dr["contents"].ToString();
                model.Date =Convert.ToDateTime(dr["date"].ToString());
                model.Writer = dr["writer"].ToString();
                model.Mstype = dr["mstype"].ToString();
                model.ReadTimes = dr["readTimes"].ToString();
                model.Notifierid = dr["notifierid"].ToString();
                model.Notifiername = dr["notifiername"].ToString();
                model.Endtime = dr["endtime"].ToString();
                model.Accessories = dr["Accessories"].ToString();
               
                list.Add(model);
            }
            return list;
        }
    }
}
