using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Models;

namespace Dal.newysgl
{
    public class Xmlr
    {
        string sql = "select procode,kmcode,budgetmoney,annual,by1,by2,by3,Row_Number()over(order by procode) as crow from bill_ys_xmfjlrb";    
        public bool IsLetsAddbyXmlr(string nd)
        {
            string cxsql = "select count(*) from bill_ys_benefitpro where annual=@annual";
            SqlParameter[] paramter = { new SqlParameter("@annual", nd) };
            return Convert.ToInt32(DataHelper.ExecuteScalar(cxsql, paramter, false)) > 0;

        }

        public bool IsLetsAddbyXmysz(string nd)
        {
            string cxsql = "select count(*) from bill_ys_xmfjlrb where annual=@annual";
            SqlParameter[] paramter = { new SqlParameter("@annual", nd) };
            return Convert.ToInt32(DataHelper.ExecuteScalar(cxsql, paramter, false)) == 0;
        }

        public bool Addxmlr(string nd)
        {
            SqlParameter[] paramter = { new SqlParameter("@nd",nd) };
            string cxsql = @"insert into bill_ys_xmfjlrb(procode,kmcode,budgetmoney,annual) 
                             select procode,kmcode,0,@nd from (   
		                            select a.procode as procode ,a.proname, b.yskmcode as kmcode,b.yskmmc 
		                            from bill_ys_benefitpro a ,
			                             bill_yskm b, 
			                             bill_ys_benefits_yskm c
		                            where a.procode=c.procode 
			                              and b.yskmcode=c.yskmcode 
			                              and a.annual=@nd )t";
            return (DataHelper.ExcuteNonQuery(cxsql, paramter, false)) != -1;
       
        }

        public DataTable GetxmbBynd(string nd,string strlrxm,string strkmcode)
        {
            
            SqlParameter[] paramter = { new SqlParameter("@annual", nd)
                                         
                                    };

            string cxsql = @" select a.procode as procode ,'['+a.procode+']'+a.proname as proname,( 
		select count(*) from bill_yskm where yskmcode like b.yskmcode+'%' 
		and len(yskmcode)>len(b.yskmcode)
    ) as childcount, b.yskmcode as kmcode,'['+b.yskmbm+']'+b.yskmmc as yskmmc ,a.annual
		                      from bill_ys_benefitpro a ,
			                             bill_yskm b, 
			                             bill_ys_benefits_yskm c
		                      where a.procode=c.procode 
			                              and b.yskmcode=c.yskmcode 
			                              and a.annual=@annual
										  and c.deptcode = ''
										 "; //and len(c.yskmcode) > 2 
            if (strlrxm!="")
            {
                cxsql += "and a.proname like '%" + strlrxm + "%'";
            }
            else
            {
                cxsql += "and a.proname like '%%'";
            }
            if (strkmcode!="")
            {
                cxsql += " and b.yskmcode like '" + strkmcode + "%'";
                
            }
            cxsql += " and ( select count(*) from bill_yskm where yskmcode like b.yskmcode+'%' and len(yskmcode)>len(b.yskmcode) ) =0";
          
            return DataHelper.GetDataTable(cxsql,paramter,false);

        }
        public IList<bill_ys_xmfjlrb> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<bill_ys_xmfjlrb> list = new List<bill_ys_xmfjlrb>();
            foreach (DataRow dr in dt.Rows)
            {
                bill_ys_xmfjlrb model = new bill_ys_xmfjlrb();
                model.procode = dr["procode"].ToString();
                model.kmcode = dr["kmcode"].ToString();
                if (!DBNull.Value.Equals(dr["budgetmoney"]))
                {
                    model.budgetmoney = decimal.Parse(dr["budgetmoney"].ToString());
                }
                model.annual = dr["annual"].ToString();
                model.by1 = dr["by1"].ToString();
                model.by2 = dr["by2"].ToString();
                model.by3 = dr["by3"].ToString();

                list.Add(model);
            }
            return list;
        }

        public List<string> GetNdByxmLrb()
        {
            string cxsql = "select distinct annual from bill_ys_xmfjlrb order by annual desc";
            DataTable dt = DataHelper.GetDataTable(cxsql,null,false);
            List<string> ndlist = new List<string>();
            foreach (DataRow i in dt.Rows)
            {
                ndlist.Add(i["annual"].ToString());
            }
            return ndlist;
        }

        public bool InsertTb(IList<bill_ys_xmfjlrb> yxxmfjb)
        {
            string upsql = "update bill_ys_xmfjlrb set budgetmoney=@budgetmoney where   procode=@procode and kmcode=@kmcode";
            string cxsql = "select count(*) from  bill_ys_xmfjlrb where procode=@procode and kmcode=@kmcode ";
            string insql = " insert into bill_ys_xmfjlrb(procode,kmcode,budgetmoney, annual) values (@procode,@kmcode,@budgetmoney, @annual)";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    foreach (var i in yxxmfjb)
                    {
                        SqlParameter[] cxparamter={ new SqlParameter("@procode",i.procode),
                                                  new SqlParameter("@kmcode",i.kmcode)};
                        int s = Convert.ToInt32(DataHelper.ExecuteScalar(cxsql,cxparamter,false));
                        if (s == 0)
                        {
                            SqlParameter[] inparamter = { new SqlParameter("@procode",i.procode),
                                                          new SqlParameter("@kmcode",i.kmcode),
                                                          new SqlParameter("@budgetmoney",i.budgetmoney),
                                                          new SqlParameter("@annual",i.procode.Substring(0,4))};
                            DataHelper.ExcuteNonQuery(insql,inparamter,false);
                        }
                        else
                        {
                            SqlParameter[] paramter = { new SqlParameter("@budgetmoney",i.budgetmoney),
                                                    new SqlParameter("@procode",i.procode),
                                                    new SqlParameter("@kmcode",i.kmcode)};
                            DataHelper.ExcuteNonQuery(upsql,  paramter, false);
                        }
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
        //提交
        public bool SubMitXm(IList<bill_ys_xmfjlrb> yxxmfjb)
        {
           return  SubMit(yxxmfjb,"1");
        }

        public bool SubMitXmFlase(IList<bill_ys_xmfjlrb> yxxmfjb)
        {
            return SubMit(yxxmfjb, "0");
        }
        public bool SubMit(IList<bill_ys_xmfjlrb> yxxmfjb, string flag)
        {
            string cxsql = "update bill_ys_xmfjlrb set by1=@by1 where   procode=@procode and kmcode=@kmcode";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    foreach (var i in yxxmfjb)
                    {
                        SqlParameter[] paramter = { new SqlParameter("@by1",flag),
                                                    new SqlParameter("@procode",i.procode),
                                                    new SqlParameter("@kmcode",i.kmcode)};
                        DataHelper.ExcuteNonQuery(cxsql, tran, paramter, false);
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



        public IList<bill_ys_xmfjlrb> GetXmfj(string nd)
        {
            string cxsql = sql + " where annual=@annual ";
            SqlParameter[] paramter = { new SqlParameter("@annual", nd) };
            return ListMaker(cxsql, paramter);
        }

        public List<string> GetNdByxmLrb(string p)
        {
            string selectndsql = @" select nian,xmmc from bill_ysgc where   yue='' and nian in (select distinct nd  from bill_SysConfig where configname = 'ystbfs' and configvalue='1' ) order by nian desc";
            DataTable selectdt = DataHelper.GetDataTable(selectndsql,null,false);
            List<string> liststr = new List<string>();
            for (int s = 0; s < selectdt.Rows.Count; s++)
            {
                liststr.Add(selectdt.Rows[s][0].ToString());
            }
            return liststr;
        }
    }
}
