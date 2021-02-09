using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;
namespace Dal.Bills
{
    public class XmDeptNdDal
    {
        string sql = "select xmCode,xmDept,note4,note5,note6,note7,je,isCtrl,nd,status,note0,note1,note2,note3,Row_Number()over(order by xmCode desc) as crow from bill_xm_dept_nd";
        string sqlCont = "select count(*) from bill_xm_dept_nd";

        public bool Exists(string xmDept ,string nd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" nd = @nd  ");
            strSql.Append(" and  xmDept = @xmDept  ");
            SqlParameter[] parameters = {
					new SqlParameter("@nd", SqlDbType.VarChar,4),
                    new SqlParameter("@xmDept", SqlDbType.VarChar,50)     };
            parameters[0].Value = xmDept;
            parameters[1].Value = nd;
            int cont = Convert.ToInt32(DataHelper.ExecuteScalar(strSql.ToString(), parameters, false));
            if (cont > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool Update(IList<bill_xm_dept_nd> yxxmfjb)
        {
            string upsql = "update bill_xm_dept_nd set je=@je,isCtrl=@isCtrl , status=@status  where   xmCode=@xmCode and xmDept=@xmDept and nd=@nd ";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                //try
                //{
                foreach (bill_xm_dept_nd i in yxxmfjb)
                {                    
                        SqlParameter[] paramter = { new SqlParameter("@je",i.je),
                                                    new SqlParameter("@isCtrl",i.isCtrl) ,
                                                    new SqlParameter("@status",i.status),
                                                    new SqlParameter("@xmCode",i.xmCode),
                                                    new SqlParameter("@xmDept",i.xmDept),
                                                    new SqlParameter("@nd",i.nd)
                                                   };
                        DataHelper.ExcuteNonQuery(upsql, paramter, false);
                }
                tran.Commit();
                return true;
                //}
                //catch
                //{
                //    tran.Rollback();
                //    return false;
                //    throw;
                //}
            }
        }
        public bool InsertTb(IList<bill_xm_dept_nd> yxxmfjb)
        {
            string upsql = "update bill_xm_dept_nd set je=@je,isCtrl=@isCtrl where   xmCode=@xmCode and xmDept=@xmDept and nd=@nd";
            string cxsql = "select count(*) from  bill_xm_dept_nd where xmDept=@xmDept and nd =@nd ";
            string insql = " insert into bill_xm_dept_nd(xmCode,xmDept,je,isCtrl, nd,status) values (@xmCode,@xmDept,@je,@isCtrl, @nd,@status)";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                //try
                //{
                foreach (bill_xm_dept_nd i in yxxmfjb)
                    {
                        SqlParameter[] cxparamter = { new SqlParameter("@xmDept", i.xmDept), 
                                                      new SqlParameter("@nd", i.nd) };
                        int s = Convert.ToInt32(DataHelper.ExecuteScalar(cxsql, cxparamter, false));
                        if (!Exists(i.xmDept,i.xmDept))
                        {
                            SqlParameter[] inparamter = { new SqlParameter("@xmCode",i.xmCode),
                                                          new SqlParameter("@xmDept",i.xmDept),
                                                          new SqlParameter("@je",i.je),
                                                          new SqlParameter("@isCtrl",i.isCtrl),
                                                          new SqlParameter("@nd",i.nd),
                                                           new SqlParameter("@status",i.status)};
                            DataHelper.ExcuteNonQuery(insql, inparamter, false);
                        }
                        else
                        {

                            //throw new Exception("dept:" + i.xmDept+"&je:"+Convert.ToString(i.je));
                      
                            SqlParameter[] paramter = { new SqlParameter("@je",i.je),
                                                           new SqlParameter("@isCtrl",i.isCtrl) ,
                                                    new SqlParameter("@xmCode",i.xmCode),
                                                    new SqlParameter("@xmDept",i.xmDept),
                                                      new SqlParameter("@nd",i.nd)};
                            DataHelper.ExcuteNonQuery(upsql, paramter, false);
                        }
                    }
                    tran.Commit();
                    return true;
                //}
                //catch
                //{
                //    tran.Rollback();
                //    return false;
                //    throw;
                //}
            }
        }

        /// <summary>
        /// 检测项目基表和项目部门年度表数据同步根据部门和年度
        /// </summary>
        /// <returns></returns>
        public bool IsNewXm(string nd, string deptcode)
        {

            string strSql = "select count(*) from  bill_xm where 1=1 and xmStatus='1' and  xmDept=@dept  and xmCode not in (select xmCode from bill_xm_dept_nd  where 1=1 and nd=@nd  and xmDept=@dept ) ";
            SqlParameter[] parameters = {
					new SqlParameter("@dept",SqlDbType.VarChar,50 ),
                    new SqlParameter("@nd", SqlDbType.VarChar,4) };
            parameters[0].Value = deptcode;
            parameters[1].Value = nd;
            int cont = Convert.ToInt32(DataHelper.ExecuteScalar(strSql.ToString(), parameters, false));
            if (cont > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取未同步的项目
        /// </summary>
        /// <param name="nd">年度</param>
        /// <param name="deptcode">部门</param>
        /// <returns>未同步的项目编号 用“|”隔开</returns>
        public string GetNewXm(string nd, string deptcode)
        {
            string sql = "select  xmCode from  bill_xm  where xmStatus ='1'  and xmDept='"+deptcode+"' and xmCode not in(select xmCode from bill_xm_dept_nd where nd='"+nd+"' and xmDept='"+deptcode+"')";
            DataTable dt = DataHelper.GetDataTable(sql,null,false);
            string Result = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Result += Convert.ToString(dt.Rows[i]["xmCode"])+"|";
            }

            if (Result.Length>1)
            {
                Result = Result.Substring(0,(Result.Length)-1);
            }
            return Result;
        }

        /// <summary>
        /// 如果年度的数据不存在2 就先把数据插入到数据库
        /// </summary>
        /// <param name="nd"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public void NullListMaker(string nd, string deptcode)
        {


            string csql = "select *  from  bill_xm where xmDept = '" + deptcode + "' and xmStatus='1'";

            IList<bill_xm_dept_nd> list = new List<bill_xm_dept_nd>();
            //获取部门的项目
            DataTable sdt = DataHelper.GetDataTable(csql, null, false);
            //项目数据插入到表 在查询出结果返回
            for (int i = 0; i < sdt.Rows.Count; i++)
            {
                bill_xm_dept_nd temp = new bill_xm_dept_nd();
                temp.nd = nd;
                temp.xmDept = deptcode;
                temp.xmCode = Convert.ToString(sdt.Rows[i]["xmCode"]);
                temp.je = Convert.ToDecimal(0.00);
                temp.status = "0";
                temp.isCtrl = "0";
                list.Add(temp);
            }
            //return null;
            this.InsertList(list);
        }

        public DataTable GetDataAll(string nd, string deptcode, int pagefrm, int pageto, out int count)
        {
            string sql = "select Row_Number()over(order by xmCode) as crow, (case isnull(status,'1') when '1' then '正常' when '0' then '停用' end) as status,nd,(select top 1 '['+xmCode+']'+xmName from bill_xm where xmCode = a.xmCode) as xmCode,(select  top 1 '['+deptcode+']'+deptname from bill_departments where deptcode=a.xmdept) as xmDept ,je,(case isnull(isCtrl,'0') when '1' then '是' when '0' then '否' end) as isCtrl  from bill_xm_dept_nd  as a  where 1=1 and  nd='" + nd + "'  and  xmDept = '" + deptcode + "' and xmCode not in (select xmCode from bill_xm where xmDept='" + deptcode + "' and xmStatus='0')";


            string strsqlcount = "select count(*) from ( {0} ) t";
            strsqlcount = string.Format(strsqlcount, sql);
            count = Convert.ToInt32(DataHelper.ExecuteScalar(strsqlcount));

            string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
            strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
            return DataHelper.GetDataTable(strsqlframe, null, false);
        }

        /// <summary>
        /// 把在列表页新添加的数据添加到表
        /// </summary>
        /// <param name="nd"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public void NullListMakerNew(string nd, string deptcode)
        {
            //获取在基表中新插入的数据
            string xmcodes = GetNewXm(nd, deptcode);
            //把数据插入到项目部门年度表
            string[] xmcode = xmcodes.Split('|');
            for (int i = 0; i < xmcode.Length; i++)
            {
                Insert(nd,deptcode, xmcode[i]);
            }
        }

        /// <summary>
        /// 如果年度的数据不存在2 就先把数据插入到数据库
        /// </summary>
        /// <param name="nd"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public void Insert(string nd, string deptcode,string xm)
        {
            IList<bill_xm_dept_nd> list = new List<bill_xm_dept_nd>();
                bill_xm_dept_nd temp = new bill_xm_dept_nd();
                temp.nd = nd;
                temp.xmDept = deptcode;
                temp.xmCode = Convert.ToString(xm);
                temp.je = Convert.ToDecimal(0.00);
                temp.status = "0";
                temp.isCtrl = "0";
                list.Add(temp);
           
            this.InsertList(list);

        }

     
        public bool InsertList(IList<bill_xm_dept_nd> yxxmfjb)
        {
            string insql = " insert into bill_xm_dept_nd(xmCode,xmDept,je,isCtrl, nd,status) values (@xmCode,@xmDept,@je,@isCtrl, @nd,@status)";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    foreach (var i in yxxmfjb)
                    {
                        SqlParameter[] inparamter = { new SqlParameter("@xmCode",i.xmCode),
                                                          new SqlParameter("@xmDept",i.xmDept),
                                                          new SqlParameter("@je",i.je),
                                                          new SqlParameter("@isCtrl",i.isCtrl),
                                                          new SqlParameter("@nd",i.nd),
                                                           new SqlParameter("@status",i.status)};
                        DataHelper.ExcuteNonQuery(insql, inparamter, false);
                    }
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                    throw;
                }
            }
        }

        public IList<bill_xm_dept_nd> GetXmfj(string nd)
        {
            string cxsql = sql + " where nd=@nd ";
            SqlParameter[] paramter = { new SqlParameter("@nd", nd) };
            return ListMaker(cxsql, paramter);
        }

        /// <summary>
        /// 通过查询语句和参数返回对象List 由于不允许直接传入语句查询  所以要求该方法一定为private
        /// </summary>
        private IList<bill_xm_dept_nd> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<bill_xm_dept_nd> list = new List<bill_xm_dept_nd>();
            foreach (DataRow dr in dt.Rows)
            {
                bill_xm_dept_nd model = new bill_xm_dept_nd();
                model.xmCode = dr["xmCode"].ToString();
                model.note3 = dr["note3"].ToString();
                model.note4 = dr["note4"].ToString();
                model.note5 = dr["note5"].ToString();
                model.note6 = dr["note6"].ToString();
                model.note7 = dr["note7"].ToString();
                model.xmDept = dr["xmDept"].ToString();
                if (!DBNull.Value.Equals(dr["je"]))
                {
                    model.je = decimal.Parse(dr["je"].ToString());
                }
                model.isCtrl = dr["isCtrl"].ToString();
                model.nd = dr["nd"].ToString();
                model.status = dr["status"].ToString();
                model.note0 = dr["note0"].ToString();
                model.note1 = dr["note1"].ToString();
                model.note2 = dr["note2"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 项目是否存在
        /// </summary>
        /// <param name="XmCode"></param>
        /// <returns></returns>
        public bool IsExsitXmCode(string xmCode)
        {

            //StringBuilder strSql = new StringBuilder();
            //strSql.Append(sqlCont);

            //strSql.Append(" where ");

            //strSql.Append("   xmCode = @xmCode  ");
            //SqlParameter[] parameters = {
					
            //        new SqlParameter("@xmCode", SqlDbType.VarChar,50)     };
            //parameters[0].Value = xmCode;
         
            //int cont = Convert.ToInt32(DataHelper.ExecuteScalar(strSql.ToString(), parameters, false));
            //throw new Exception(Convert.ToString(xmCode));
            //if (cont > 0)
            //{
                
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            string sql = "select SUM(a.con) as con  from (select ISNULL(count(*),'0') as con from bill_xm_dept_nd where xmCode in("+xmCode+")) a";
            SqlParameter[] parms = { new SqlParameter("@xmCode", xmCode) };
            DataTable dt = DataHelper.GetDataTable(sql, null, false);
           
            if (Convert.ToInt32(dt.Rows[0]["con"]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }        
            
    }
    }
}
