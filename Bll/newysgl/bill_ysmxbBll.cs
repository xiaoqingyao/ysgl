using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Dal;

namespace Bll.newysgl
{
    public class bill_ysmxbBll
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        Dal.newysgl.bill_ysmxbDal dal = new Dal.newysgl.bill_ysmxbDal();
        public IList<Models.Bill_Ysmxb> GetYsmxByNian(string nd, string deptcode, string xmbh, string stepid)
        {
            return dal.GetYsmxByNian(nd, deptcode, xmbh, stepid);
        }
        public string getmoney(string strdeptCode, string strkmCode, string strgcCode)
        {
            string strsql = "select isnull(sum(ysje),0) from bill_ysmxb where ysdept='" + strdeptCode + "'  and yskm='" + strkmCode + "' and gcbh='" + strgcCode + "'  and (ysType='1' or ysType='5')";
            string strRet = server.GetCellValue(strsql);
            return strRet;

        }
        /// <summary>
        /// 获取报销金额
        /// </summary>
        /// <param name="year"></param>
        /// <param name="moth"></param>
        /// <param name="kmbh"></param>
        /// <returns></returns>
        public string getbxje(string year, string moth, string kmbh, string deptcode)
        {
            string strbeg = "";
            string strend = "";
            string strbx = "0";
            string strtimesql = @" select * from bill_Cnpz   where year_CN='" + year + "' and year_moth='" + moth + "' ";
            DataTable dt = server.GetDataTable(strtimesql, null);
            if (dt != null && dt.Rows.Count > 0)
            {
                strbeg = dt.Rows[0]["beg_time"].ToString();
                strend = dt.Rows[0]["end_time"].ToString();
            }
            string strsql = @" exec dz_hfje '" + strbeg + "','" + strend + "','" + deptcode + "','" + kmbh + "'";

            strbx = server.GetCellValue(strsql);

            return strbx;
        }
        /// <summary>
        /// 为报销单添加报销单号
        /// </summary>
        /// <param name="strCodes">单据号，处理成'aa','bb'</param>
        /// <param name="strPzCode"></param>
        /// <param name="strZhangTaoCode">帐套号</param>
        /// <param name="strPzDate"></param>
        /// <returns></returns>
        public int SetPingZheng(string strCodes, string strPzCode, string strZhangTaoCode, string strPzDate)
        {
            if (string.IsNullOrEmpty(strCodes))
            {
                throw new Exception("报销单不能为空！");
            }
            else if (string.IsNullOrEmpty(strPzCode))
            {
                throw new Exception("凭证号不能为空");
            }
            else if (string.IsNullOrEmpty(strPzDate))
            {
                throw new Exception("时间不能为空");
            }
            else
            {
                //修改一般报销明细表
                string strSql = "update bill_ybbxmxb set pzcode=@pzcode,pzdate=@pzdate,zhangtao=@zhangtao,pzbldate=getdate() where billcode in(" + strCodes + ")";
                SqlParameter[] arrSp = { 
                                   new SqlParameter("@pzcode",strPzCode),
                                   new SqlParameter("@pzdate",strPzDate),
                                   new SqlParameter("@zhangtao",strZhangTaoCode)
                                   };
                int irel = server.ExecuteNonQuery(strSql, arrSp);
                if (irel > 0)
                {
                    //为还款单的还款明细  记录凭证号
                    string strsql = "update T_ReturnNote set note3=@pzcode where note4 in (" + strCodes + ") ";
                    SqlParameter[] arrSp2 = { 
                                   new SqlParameter("@pzcode",strPzCode)
                                   };
                    server.ExecuteNonQuery(strsql, arrSp2);
                }
                return irel;
            }
        }
        /// <summary>
        /// 根据编号给明细表更新凭证信息
        /// </summary>
        /// <param name="strInBillCode">billcode 处理成'aa','bb'</param>
        /// <param name="strpingzhenghao"></param>
        /// <param name="strZhangTaodb">帐套号</param>
        /// <param name="strBillDate"></param>
        /// <returns></returns>
        public int SetPingZhengByBillName(string strInBillCode, string strpingzhenghao, string strZhangTaodb, string strBillDate)
        {
            if (string.IsNullOrEmpty(strInBillCode))
            {
                throw new Exception("单据编号不能为空！");
            }
            else if (string.IsNullOrEmpty(strpingzhenghao))
            {
                throw new Exception("凭证号不能为空");
            }
            else if (string.IsNullOrEmpty(strBillDate))
            {
                throw new Exception("时间不能为空");
            }
            else
            {
                string strSql = "update bill_ybbxmxb set pzcode=@pzcode,pzdate=@pzdate,zhangtao=@zhangtao where billcode in (select billcode from bill_main where billname in (select billname from bill_main where billcode in (" + strInBillCode + ")))";
                SqlParameter[] arrSp = { 
                                   new SqlParameter("@pzcode",strpingzhenghao),
                                   new SqlParameter("@pzdate",strBillDate),
                                   new SqlParameter("@zhangtao",strZhangTaodb)
                                   };
                int irel = server.ExecuteNonQuery(strSql, arrSp);
                return irel;
            }
        }

        /// <summary>
        /// 大智专用 计算报销金额
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="nd"></param>
        /// <returns></returns>
        public IList<yskmhf> getkmje(string deptcode, string nd)
        {
            string strsql = @"exec [dz_hfje_new] '" + deptcode + "','" + nd + "'";
            DataTable dt = DataHelper.GetDataTable(strsql, null, false);

            IList<yskmhf> list = new List<yskmhf>();
            foreach (DataRow dr in dt.Rows)
            {
                yskmhf yskm = new yskmhf();
                yskm.cnyf = Convert.ToString(dr["cnyf"]);
                yskm.fykm = Convert.ToString(dr["fykm"]);
                yskm.fykmname = Convert.ToString(dr["fykmname"]);
                string strje = Convert.ToString(dr["je"]);
                yskm.je = strje.Equals("") ? 0 : decimal.Parse(strje) ;
                list.Add(yskm);
            }
            return list;
        }
    }


    public class yskmhf
    {
        /// <summary>
        /// 财年
        /// </summary>
        private string _cnyf;
        public string cnyf
        {
            get { return _cnyf; }
            set { _cnyf = value; }
        }
        /// <summary>
        /// 费用科目编号
        /// </summary>
        private string _fykm;
        public string fykm
        {
            get { return _fykm; }
            set { _fykm = value; }
        }
        /// <summary>
        /// 费用科目名称
        /// </summary>
        private string _fykmname;
        public string fykmname
        {
            get { return _fykmname; }
            set { _fykmname = value; }
        }
        /// <summary>
        /// 金额
        /// </summary>
        private decimal _je;
        public decimal je
        {
            get { return _je; }
            set { _je = value; }
        }

    }


}
