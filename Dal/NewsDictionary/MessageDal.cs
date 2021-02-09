using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Models;

namespace Dal.NewsDictionary
{
    public class MessageDal
    {
        string selsql = "select code, title, context, memo, userCode, messageDate, msgType, upfile,Row_Number()over(order by messageDate desc) crow  from titleMessage ";


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(TitleMessage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into titleMessage(");
            strSql.Append("code,title,context,memo,userCode,messageDate,msgType,upfile)");
            strSql.Append(" values (");
            strSql.Append("@code,@title,@context,@memo,@userCode,@messageDate,@msgType,@upfile)");
            SqlParameter[] parameters = {
					                        new SqlParameter("@code", SqlDbType.VarChar,30),
					                        new SqlParameter("@title", SqlDbType.VarChar,50),
					                        new SqlParameter("@context", SqlDbType.VarChar,300),
					                        new SqlParameter("@memo", SqlDbType.VarChar,50),
					                        new SqlParameter("@userCode", SqlDbType.VarChar,30),
					                        new SqlParameter("@messageDate", SqlDbType.DateTime),
					                        new SqlParameter("@msgType", SqlDbType.VarChar,2),
					                        new SqlParameter("@upfile", SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = model.Code;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.Context;
            parameters[3].Value = model.Memo;
            parameters[4].Value = model.UserCode;
            parameters[5].Value = model.MessageDate;
            parameters[6].Value = model.MsgType;
            parameters[7].Value = model.Upfile;

            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    Delete(model.Code,trans);
                    DataHelper.ExcuteNonQuery(strSql.ToString(), trans, parameters, false);
                    if (model.Userlist.Count > 0)
                    {
                        AddUser(model.Userlist, trans);
                    }
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 添加看见通知的用户
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tran"></param>
        public void AddUser(IList<MessageReader> list, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into MessageReader(");
            strSql.Append("usercode,code,isRead");
            strSql.Append(") values (");
            strSql.Append("@usercode,@code,@isRead");
            strSql.Append(") ");
            foreach (MessageReader user in list)
            {
                SqlParameter[] parameters = {
	                                            new SqlParameter("@usercode", SqlDbType.VarChar,30) ,            
                                                new SqlParameter("@code", SqlDbType.VarChar,30) ,            
                                                new SqlParameter("@isRead", SqlDbType.Int,4)
                                            };

                parameters[0].Value = user.Usercode;
                parameters[1].Value = user.Code;
                parameters[2].Value = user.IsRead;
                DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string code)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(code, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string code,SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" delete from titleMessage ");
            strSql.Append(" where code=@code ");

            StringBuilder delForDy = new StringBuilder();
            delForDy.Append("delete from MessageReader");
            delForDy.Append(" where code=@code");

            SqlParameter[] parameters = {
					                        new SqlParameter("@code", SqlDbType.VarChar,30)			
                                        };
            parameters[0].Value = code;
            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
            DataHelper.ExcuteNonQuery(delForDy.ToString(), tran, parameters, false);

        }

        /// <summary>
        /// 得到所有新闻
        /// </summary>
        public IList<TitleMessage> GetNews()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(selsql);
            strSql.Append(" where msgType='1'");

            return ListMaker(strSql.ToString(), null);
        }

        /// <summary>
        /// 根据发布人得到通知
        /// </summary>
        public IList<TitleMessage> GetMessageByMaker(string userCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(selsql);
            strSql.Append(" where msgType='2' and usercode=@userCode ");
            SqlParameter[] sps = { 
                                     new SqlParameter("@userCode",userCode)
                                 };

            return ListMaker(strSql.ToString(), sps);
        }



        /// <summary>
        /// 根据开始行结束行获得通知
        /// </summary>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<TitleMessage> GetMessageByMaker(string userCode,  int beg, int end)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from(");
            sql.Append(selsql);
            sql.Append(" where msgType='2' and usercode=@usercode ");
            sql.Append(")t where t.crow>");
            sql.Append(beg.ToString());
            sql.Append(" and t.crow<=");
            sql.Append(end.ToString());
            SqlParameter[] sps = { 
                                     new SqlParameter("@userCode",userCode)
                                 };
            return ListMaker(sql.ToString(), sps);
        }
        /// <summary>
        /// 根据接收者获得通知
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public IList<TitleMessage> GetMessageByReader(string userCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(selsql);
            strSql.Append(" where msgType='2' and code in(select code from MessageReader where usercode= @userCode) ");
            SqlParameter[] sps = { 
                                     new SqlParameter("@userCode",userCode)
                                 };
            return ListMaker(strSql.ToString(), sps);
        }
        /// <summary>
        /// 根据接收者获得通知
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public IList<TitleMessage> GetMessageByReader(string userCode, int beg, int end)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from(");
            strSql.Append(selsql);
            strSql.Append(" where msgType='2' and code in(select code from MessageReader where usercode= @userCode) ");
            strSql.Append(")t where t.crow>");
            strSql.Append(beg.ToString());
            strSql.Append(" and t.crow<=");
            strSql.Append(end.ToString());

            SqlParameter[] sps = { 
                                     new SqlParameter("@userCode",userCode)
                                 };
            return ListMaker(strSql.ToString(), sps);
        }
        /// <summary>
        /// 获得单据是否已读
        /// </summary>
        /// <param name="code"></param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public int GetMessageState(string code, string userCode)
        {
            string sql = "select isread from MessageReader where userCode=@userCode and code=@code";
            SqlParameter[] sps = { 
                                     new SqlParameter("@userCode",userCode),
                                     new SqlParameter("@code",code)
                                 };
            return Convert.ToInt32(DataHelper.ExecuteScalar(sql,sps,false));
        }

        /// <summary>
        /// 根据开始行结束行获得新闻
        /// </summary>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<TitleMessage> GetNews(int beg, int end)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from(");
            sql.Append(selsql);
            sql.Append(" where msgType='1' ");
            sql.Append(")t where t.crow>");
            sql.Append(beg.ToString());
            sql.Append(" and t.crow<=");
            sql.Append(end.ToString());
            return ListMaker(sql.ToString(), null);
        }
        /// <summary>
        /// 更新信息状况，0未读，1已读
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="code"></param>
        /// <param name="val"></param>
        public void UpdateMessageState(string userCode,string code, int val)
        {
            string sql = "update MessageReader set isread=@isread where code=@code and userCode=@userCode";
            SqlParameter[] sps = { 
                                     new SqlParameter("@isread",val),
                                     new SqlParameter("@userCode",userCode),
                                     new SqlParameter("@code",code)
                                 };
            DataHelper.ExcuteNonQuery(sql, sps, false);
        }

        public int GetNewsCount(string type)
        {
            string sql = "select count(*) from titleMessage where msgType=@msgType";
            SqlParameter[] sps = {
                                     new SqlParameter("@msgType",type)
                                 };
            return Convert.ToInt32(DataHelper.ExecuteScalar(sql, sps, false));
        }

        public int GetMessageCount(string userCode)
        {
            string sql = "select count(*) from titleMessage where msgType=2 and userCode=@userCode";
            SqlParameter[] sps = {
                                     new SqlParameter("@userCode",userCode)
                                 };
            return Convert.ToInt32(DataHelper.ExecuteScalar(sql, sps, false));
        }

        public int GetReaderCount(string userCode)
        {
            string sql = "select count(*) from MessageReader where userCode=@userCode";
            SqlParameter[] sps = {
                                     new SqlParameter("@userCode",userCode)
                                 };
            return Convert.ToInt32(DataHelper.ExecuteScalar(sql, sps, false));
        }

        public TitleMessage GetNewsByCode(string code)
        {
            string sql = selsql + " where code=@code ";
            SqlParameter[] sps = {
                                     new SqlParameter("@code",code)
                                 };
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {
                    TitleMessage model = new TitleMessage();
                    model.Code = dr["code"].ToString();
                    model.Title = dr["title"].ToString();
                    model.Context = dr["context"].ToString();
                    model.Memo = dr["memo"].ToString();
                    model.UserCode = dr["userCode"].ToString();
                    if (dr["messageDate"].ToString() != "")
                    {
                        model.MessageDate = Convert.ToDateTime(dr["messageDate"]);
                    }
                    model.MsgType = dr["msgType"].ToString();
                    model.Upfile = dr["upfile"].ToString();

                    string tempsql = "select * from MessageReader where code=@code";
                    SqlParameter[] tempsps = { new SqlParameter("@code", model.Code) };
                    DataTable dt = DataHelper.GetDataTable(tempsql, tempsps, false);
                    foreach (DataRow tempdr in dt.Rows)
                    {
                        MessageReader reader = new MessageReader();
                        reader.Code = model.Code;
                        reader.IsRead = Convert.ToInt32(tempdr["IsRead"]);
                        reader.Usercode = Convert.ToString(tempdr["Usercode"]);
                        model.Userlist.Add(reader);
                    }
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        private IList<TitleMessage> ListMaker(string sql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<TitleMessage> list = new List<TitleMessage>();

            foreach (DataRow dr in dt.Rows)
            {
                TitleMessage model = new TitleMessage();
                model.Code = dr["code"].ToString();
                model.Title = dr["title"].ToString();
                model.Context = dr["context"].ToString();
                model.Memo = dr["memo"].ToString();
                model.UserCode = dr["userCode"].ToString();
                if (dr["messageDate"].ToString() != "")
                {
                    model.MessageDate = Convert.ToDateTime(dr["messageDate"]);
                }
                model.MsgType = dr["msgType"].ToString();
                model.Upfile = dr["upfile"].ToString();
                list.Add(model);
            }
            return list;
        }

    }
}
