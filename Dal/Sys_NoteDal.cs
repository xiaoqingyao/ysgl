using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using Models;

namespace Dal
{
    /// <summary>
    /// 说明：日志Dal
    /// <create> by:zyl</create>
    /// <createDate>2014-05-20</createDate>
    /// <remarks>用户日志操作类</remarks>
    /// </summary>
    public class Sys_NoteDal
    {
        /// <summary>
        /// 主表sql
        /// </summary>
        string sql = "select sysCode,afterstr,opeDiscretion,note1,note2,note3,note4,note5,note6,note7,note8,billcode,note9,note10,note11,note12,note13,note14,note15,note16,                      note17,note18,billname,note19,note20,usercode,billtype,OperationType,userip,ndate,beforestr,Row_Number()over(order by sysCode) as crow from Sys_Note";
        string sqlCont = "select count(*) as crow from Sys_Note";

        /// <summary>
        /// 子表sql
        /// </summary>
        string itemSql = "select sysCode,note6,note7,note8,note9,note10,note11,note12,note13,note14,note15,beforestr,note16,note17,note18,note19,note20,afterstr,edittype,note1,            note2,note3,note4,note5,Row_Number()over(order by sysCode) as crow from Sys_Notes";
        string iteSqlCont = "select count(*) as crow from Sys_Notes";



        /// <summary>
        /// 创建日志
        /// </summary>
        /// <param name="sysnote">日志主表对象</param>
        /// <param name="objMain">单据主表</param>
        /// <param name="lstItem">单据</param>k
        /// <returns></returns>
        public int Create(Sys_Note sysnote, object objMain, List<object> lstItem)
        {
            int row = 0;
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

                    if (objMain != null)
                    {
                        sysnote.afterstr = MainStr(objMain);
                    }
                     Add(sysnote, tran);
                    if (lstItem.Count > 0)
                    {
                        List<Sys_Notes> lists = ItemListMaker(sysnote.sysCode, lstItem);
                     AddItems(lists, tran);
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
            return row;
        }


        /// <summary>
        /// 返回主表对象集合和子表对象集合
        /// </summary>
        /// <param name="sysnote">主表对象</param>
        /// <param name="items"><子表对象集合/param>
        /// <returns>子表对象集合和子表对象集合</returns>
        public List<Sys_Note> GetList(string sqls, List<SqlParameter> paramter, out List<Sys_Notes> items)
        {
            List<Sys_Note> mianList = GetList(sqls, paramter);
            List<Sys_Notes> item = null;
            for (int i = 0; i < mianList.Count; i++)
            {
                item.AddRange(GetItemList(mianList[i].billcode));
            }
            items = item;
            return mianList;
        }
        #region Mian
        /// <summary>
        /// 单据主表字符串
        /// </summary>
        /// <param name="objMain"></param>
        /// <returns></returns>
        private string MainStr(object objMain)
        {
            Type tMain = objMain.GetType();
            System.Reflection.PropertyInfo[] arr = tMain.GetProperties();
            StringBuilder sbMianStr = new StringBuilder();
            sbMianStr.Append(",");
            for (int i = 0; i < arr.Length; i++)
            {
                object value1 = arr[i].GetValue(tMain, null);//用GetValue获得值
                string name = arr[i].Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                //获得属性的类型,进行判断然后进行以后的操作,例如判断获得的属性是整数
                sbMianStr.Append(name + ":" + (string)value1 + ",");

            }
            sbMianStr.Append(")");
            string result = sbMianStr.ToString();
            if (result.Length > 1)
            {
                result = result.Substring(0, result.Length - 1);
            }
            else
            {
                result = "";
            }
            return result;
        }
        /// <summary>
        /// 增加一条主表数据
        /// </summary>
        public void Add(Sys_Note model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sys_Note(");
            strSql.Append("sysCode,afterstr,opeDiscretion,note1,note2,note3,note4,note5,note6,note7,note8,billcode,note9,note10,note11,note12,note13,note14,note15,note16,note17,note18,billname,note19,note20,usercode,billtype,OperationType,userip,ndate,beforestr");
            strSql.Append(") values (");
            strSql.Append("@sysCode,@afterstr,@opeDiscretion,@note1,@note2,@note3,@note4,@note5,@note6,@note7,@note8,@billcode,@note9,@note10,@note11,@note12,@note13,@note14,@note15,@note16,@note17,@note18,@billname,@note19,@note20,@usercode,@billtype,@OperationType,@userip,@ndate,@beforestr");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@sysCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@afterstr", SqlDbType.Text) ,            
                        new SqlParameter("@opeDiscretion", SqlDbType.Text) ,            
                        new SqlParameter("@note1", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note2", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note3", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note4", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note5", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note6", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note7", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note8", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@billcode", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@note9", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note10", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note11", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note12", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note13", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note14", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note15", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note16", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note17", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note18", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@billname", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@note19", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note20", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@usercode", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@billtype", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@OperationType", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@userip", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@ndate", SqlDbType.DateTime) ,            
                        new SqlParameter("@beforestr", SqlDbType.Text)             
              
            };

            parameters[0].Value = SqlNull(model.sysCode);

            parameters[1].Value = SqlNull(model.afterstr);

            parameters[2].Value = SqlNull(model.opeDiscretion);

            parameters[3].Value = SqlNull(model.note1);

            parameters[4].Value = SqlNull(model.note2);

            parameters[5].Value = SqlNull(model.note3);

            parameters[6].Value = SqlNull(model.note4);

            parameters[7].Value = SqlNull(model.note5);

            parameters[8].Value = SqlNull(model.note6);

            parameters[9].Value = SqlNull(model.note7);

            parameters[10].Value = SqlNull(model.note8);

            parameters[11].Value = SqlNull(model.billcode);

            parameters[12].Value = SqlNull(model.note9);

            parameters[13].Value = SqlNull(model.note10);

            parameters[14].Value = SqlNull(model.note11);

            parameters[15].Value = SqlNull(model.note12);

            parameters[16].Value = SqlNull(model.note13);

            parameters[17].Value = SqlNull(model.note14);

            parameters[18].Value = SqlNull(model.note15);

            parameters[19].Value = SqlNull(model.note16);

            parameters[20].Value = SqlNull(model.note17);

            parameters[21].Value = SqlNull(model.note18);

            parameters[22].Value = SqlNull(model.billname);

            parameters[23].Value = SqlNull(model.note19);

            parameters[24].Value = SqlNull(model.note20);

            parameters[25].Value = SqlNull(model.usercode);

            parameters[26].Value = SqlNull(model.billtype);

            parameters[27].Value = SqlNull(model.OperationType);

            parameters[28].Value = SqlNull(model.userip);

            parameters[29].Value = SqlNull(model.ndate);

            parameters[30].Value = SqlNull(model.beforestr);


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        /// <summary>
        ///根据查询获取主表对象集合
        /// </summary>
        /// <param name="tempsql">查询语句</param>
        /// <param name="sps">参数</param>
        /// <returns>主表对象集合</returns>
        public IList<Sys_Note> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<Sys_Note> list = new List<Sys_Note>();
            foreach (DataRow dr in dt.Rows)
            {
                Sys_Note model = new Sys_Note();
                model.sysCode = dr["sysCode"].ToString();
                model.afterstr = dr["afterstr"].ToString();
                model.opeDiscretion = dr["opeDiscretion"].ToString();
                model.note1 = dr["note1"].ToString();
                model.note2 = dr["note2"].ToString();
                model.note3 = dr["note3"].ToString();
                model.note4 = dr["note4"].ToString();
                model.note5 = dr["note5"].ToString();
                model.note6 = dr["note6"].ToString();
                model.note7 = dr["note7"].ToString();
                model.note8 = dr["note8"].ToString();
                model.billcode = dr["billcode"].ToString();
                model.note9 = dr["note9"].ToString();
                model.note10 = dr["note10"].ToString();
                model.note11 = dr["note11"].ToString();
                model.note12 = dr["note12"].ToString();
                model.note13 = dr["note13"].ToString();
                model.note14 = dr["note14"].ToString();
                model.note15 = dr["note15"].ToString();
                model.note16 = dr["note16"].ToString();
                model.note17 = dr["note17"].ToString();
                model.note18 = dr["note18"].ToString();
                model.billname = dr["billname"].ToString();
                model.note19 = dr["note19"].ToString();
                model.note20 = dr["note20"].ToString();
                model.usercode = dr["usercode"].ToString();
                model.billtype = dr["billtype"].ToString();
                model.OperationType = dr["OperationType"].ToString();
                model.userip = dr["userip"].ToString();
                if (!DBNull.Value.Equals(dr["ndate"]))
                {
                    model.ndate = DateTime.Parse(dr["ndate"].ToString());
                }
                model.beforestr = dr["beforestr"].ToString();

                list.Add(model);
            }
            return list;
        }
        #endregion



        #region Item
        /// <summary>
        /// 获取单据子表对象字符串
        /// </summary>
        /// <param name="mainCode">日志主表主键 guid</param>
        /// <param name="lstItem">单据子表集合</param>
        /// <returns>子表对象键值对字符串</returns>
        private List<Sys_Notes> ItemListMaker(string mainCode, List<object> lstItem)
        {
            List<Sys_Notes> resultList = new List<Sys_Notes>();
            foreach (List<object> item in lstItem)
            {
                foreach (object i in item)
                {
                    Type tItem = i.GetType();
                    System.Reflection.PropertyInfo[] arrTemp = tItem.GetProperties();
                    Sys_Notes temp = new Sys_Notes();
                    temp.sysCode = mainCode;

                    StringBuilder sbItemTemp = new StringBuilder();
                    sbItemTemp.Append(",");
                    for (int m = 0; m < arrTemp.Length; m++)
                    {
                        string key = Convert.ToString(arrTemp[m].Name);
                        object value = arrTemp[m].GetValue(tItem, null);
                        sbItemTemp.Append(key + ":" + (string)value + ",");
                    }
                    string result = sbItemTemp.ToString();
                    if (result.Length > 1)
                        result = result.Substring(0, result.Length - 1);
                    else
                        result = "";
                    temp.afterstr = result;
                    resultList.Add(temp);
                }
            }
            return resultList;
        }


        /// <summary>
        /// 增加子表列表集合
        /// </summary>
        public void AddItems(List<Sys_Notes> lists, SqlTransaction tran)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                AddItem(lists[i], tran);
            }
        }

        /// <summary>
        /// 增加子表
        /// </summary>
        public void AddItem(Sys_Notes model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sys_Notes(");
            strSql.Append("sysCode,note6,note7,note8,note9,note10,note11,note12,note13,note14,note15,beforestr,note16,note17,note18,note19,note20,afterstr,edittype,note1,note2,note3,note4,note5");
            strSql.Append(") values (");
            strSql.Append("@sysCode,@note6,@note7,@note8,@note9,@note10,@note11,@note12,@note13,@note14,@note15,@beforestr,@note16,@note17,@note18,@note19,@note20,@afterstr,@edittype,@note1,@note2,@note3,@note4,@note5");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@sysCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note6", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note7", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note8", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note9", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note10", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note11", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note12", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note13", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note14", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note15", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@beforestr", SqlDbType.Text) ,            
                        new SqlParameter("@note16", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note17", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note18", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note19", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note20", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@afterstr", SqlDbType.Text) ,            
                        new SqlParameter("@edittype", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note1", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note2", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note3", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note4", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@note5", SqlDbType.VarChar,100)             
              
            };

            parameters[0].Value = SqlNull(model.sysCode);

            parameters[1].Value = SqlNull(model.note6);

            parameters[2].Value = SqlNull(model.note7);

            parameters[3].Value = SqlNull(model.note8);

            parameters[4].Value = SqlNull(model.note9);

            parameters[5].Value = SqlNull(model.note10);

            parameters[6].Value = SqlNull(model.note11);

            parameters[7].Value = SqlNull(model.note12);

            parameters[8].Value = SqlNull(model.note13);

            parameters[9].Value = SqlNull(model.note14);

            parameters[10].Value = SqlNull(model.note15);

            parameters[11].Value = SqlNull(model.beforestr);

            parameters[12].Value = SqlNull(model.note16);

            parameters[13].Value = SqlNull(model.note17);

            parameters[14].Value = SqlNull(model.note18);

            parameters[15].Value = SqlNull(model.note19);

            parameters[16].Value = SqlNull(model.note20);

            parameters[17].Value = SqlNull(model.afterstr);

            parameters[18].Value = SqlNull(model.edittype);

            parameters[19].Value = SqlNull(model.note1);

            parameters[20].Value = SqlNull(model.note2);

            parameters[21].Value = SqlNull(model.note3);

            parameters[22].Value = SqlNull(model.note4);

            parameters[23].Value = SqlNull(model.note5);


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        /// <summary>
        /// 返回主表对象集合
        /// </summary>
        /// <param name="sysnote"></param>
        /// <returns></returns>
        public List<Sys_Note> GetList(string sqls, List<SqlParameter> paramter)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where 1=1 ");
            strSql.Append(sqls);
            if (paramter == null)
            {
                return ListMaker(strSql.ToString(), null).ToList<Sys_Note>();
            }
            else
            {
                return ListMaker(strSql.ToString(), paramter.ToArray()).ToList<Sys_Note>();
            }

        }


        /// <summary>
        /// 根据主表guid 返回子表
        /// </summary>
        /// <param name="mainCode"></param>
        /// <returns></returns>
        public List<Sys_Notes> GetItemList(string mainCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(itemSql);
            strSql.Append(" where 1=1 ");
            if (!string.IsNullOrEmpty(mainCode))
            {
                strSql.Append("  and sysCode=@sysCode");
                SqlParameter[] paramter = { new SqlParameter("@sysCode", mainCode) };
                return ItemsListMaker(strSql.ToString(), paramter.ToArray()).ToList<Sys_Notes>();
            }
            else
            {
                return ItemsListMaker(strSql.ToString(), null).ToList<Sys_Notes>();
            }
        }

        /// <summary>
        /// 查询结果集转化为对象
        /// </summary>
        /// <param name="tempsql"></param>
        /// <param name="sps"></param>
        /// <returns></returns>
        public IList<Sys_Notes> ItemsListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<Sys_Notes> list = new List<Sys_Notes>();
            foreach (DataRow dr in dt.Rows)
            {
                Sys_Notes model = new Sys_Notes();
                model.sysCode = dr["sysCode"].ToString();
                model.note6 = dr["note6"].ToString();
                model.note7 = dr["note7"].ToString();
                model.note8 = dr["note8"].ToString();
                model.note9 = dr["note9"].ToString();
                model.note10 = dr["note10"].ToString();
                model.note11 = dr["note11"].ToString();
                model.note12 = dr["note12"].ToString();
                model.note13 = dr["note13"].ToString();
                model.note14 = dr["note14"].ToString();
                model.note15 = dr["note15"].ToString();
                model.beforestr = dr["beforestr"].ToString();
                model.note16 = dr["note16"].ToString();
                model.note17 = dr["note17"].ToString();
                model.note18 = dr["note18"].ToString();
                model.note19 = dr["note19"].ToString();
                model.note20 = dr["note20"].ToString();
                model.afterstr = dr["afterstr"].ToString();
                model.edittype = dr["edittype"].ToString();
                model.note1 = dr["note1"].ToString();
                model.note2 = dr["note2"].ToString();
                model.note3 = dr["note3"].ToString();
                model.note4 = dr["note4"].ToString();
                model.note5 = dr["note5"].ToString();

                list.Add(model);
            }
            return list;
        }

        #endregion
        private object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }
    }
}
