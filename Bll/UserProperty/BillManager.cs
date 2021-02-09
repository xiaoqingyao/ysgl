using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Bills;
using Models;
using Bll.FeeApplication;
using Bll.Bills;
using Bll.Sepecial;
using Bll.ReportAppBLL;
using System.Data;
using Dal;
using System.Data.SqlClient;
using Dal.FeeApplication;
using Dal.FeiYong_DZ;


namespace Bll.UserProperty
{
    public class BillManager
    {
        YbbxDal ybbxDal = new YbbxDal();
        MainDal mainDal = new MainDal();
        LscgDal lscgDal = new LscgDal();
        YsglDal ysglDal = new YsglDal();
        CgzjjhDal cgzjjhDal = new CgzjjhDal();
        CgzjfkDal cgzjfkDal = new CgzjfkDal();
        CgspDal cgspDal = new CgspDal();

        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        public void InsertYbbx(Bill_Main main, IList<Bill_Ybbxmxb> mxList)
        {
            ybbxDal.InsertYbbx(main, mxList);
        }
        /// <summary>
        /// 归口费用报销单
        /// </summary>
        /// <param name="main"></param>
        /// <param name="ybbxList"></param>
        public void insertYbbxForGkfj(Bill_Main main, IList<Bill_Ybbxmxb> ybbxList)
        {
            ybbxDal.insertYbbxForGkfj(main, ybbxList);
        }

        public Bill_Main GetMainByCode(string billCode)
        {
            return mainDal.GetMainByCode(billCode);
        }
        /// <summary>
        /// 通过billname获取bill_main的多条数据
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public IList<Bill_Main> GetMainsByBillName(string strName)
        {
            return mainDal.GetMainsByBillName(strName);
        }
        public Bill_Ybbxmxb GetYbbx(string billCode)
        {
            return ybbxDal.GetYbbx(billCode);
        }

        public void DeleteYbbx(string billCode)
        {
            ybbxDal.DeleteYbbx(billCode);
        }
        /// <summary>
        /// 根据billname删除多个bill_main
        /// </summary>
        /// <param name="billCode">billname</param>
        public void DeleteYbbxsByName(string billname)
        {
            //ybbxDal.DeleteYbbxsByName(billname);
            string strsql = "select billcode,note5 from bill_main where billname=@billname";
            DataTable dtRel = DataHelper.GetDataTable(strsql, new SqlParameter[] { new SqlParameter("@billname", billname) }, false);
            if (dtRel == null)
            {
                return;
            }
            int irowscount = dtRel.Rows.Count;
            for (int i = 0; i < irowscount; i++)
            {
                ybbxDal.DeleteYbbx(dtRel.Rows[i]["billcode"].ToString());
            }
            string note5 =dtRel.Rows[0]["note5"].ToString();
            string ntsql = @"update bill_main set note5='0' where flowID='ybbx' and billName='" + note5 + "'";
            DataHelper.ExcuteNonQuery(ntsql,null,false);
        }

        public void DeleteLscg(string billCode)
        {
            lscgDal.DeleteLscg(billCode);
        }

        public void DeleteCwtb(string billCode)
        {
            ysglDal.DeleteYsmx(billCode, "02");
        }

        public void DeleteZjys(string billCode)
        {
            ysglDal.DeleteYsmx(billCode);
        }

        public void DeleteYstb(string billCode)
        {
            new MainDal().DeleteMain(billCode);
            ysglDal.DeleteYsmx(billCode, "01");
        }

        public void DeleteCgzjjh(string billCode)
        {
            cgzjjhDal.DeleteCgzjjh(billCode);
        }

        public void DeleteCgzjfk(string billCode)
        {
            cgzjfkDal.DeleteCgzjfk(billCode);
        }

        public void DeleteCgsp(string billCode)
        {
            cgspDal.DeleteCgsp(billCode);
        }

        public void DeleteYstz(string billCode)
        {
            ysglDal.DeleteYsmx(billCode);
        }
        /// <summary>
        /// 删除出差申请单
        /// </summary>
        public void DeleteTravelApplicationBill(string strBillCode)
        {
            //删除bill_main表
            MainDal mdal = new MainDal();
            mdal.DeleteMain(strBillCode);
            new bill_travelApplicationBLL().Delete(strBillCode);
        }
        /// <summary>
        /// 删除资金申请记录
        /// </summary>
        /// <param name="billcode"></param>
        public void DeleteZjsq(string billcode) {
            //删除bill_main表
            MainDal mdal = new MainDal();
            mdal.DeleteMain(billcode);
        }
        public void DeleteDzfp(string billcode)
        {
            //删除bill_main表
           List<string> list = new System.Collections.Generic.List<string>();

           list.Add("delete bill_main where billcode='" + billcode + "'");
           list.Add("delete bill_fpfj where billcode='" + billcode + "'");
           list.Add("delete bill_fpfjs where billcode='" + billcode + "'");
           server.ExecuteNonQuerysArray(list);
          
        }
        public void DeleteTravelReportBill(string billCode)
        {
            //删除bill_main表
            MainDal mdal = new MainDal();
            mdal.DeleteMain(billCode);
            new Bill_TravelReportBLL().Delete(billCode);
        }
        //删除特种车销售返利单
        public void DeleSpecialRebatesApp(string billCode)
        {
            new SpecialRebatesAppBLL().Delete(billCode);
        }
        /// <summary>
        /// 删除车款上缴表
        /// </summary>
        /// <param name="billCode"></param>
        public void DeleCarmoney(string billCode)
        {

            new RemittanceBll().Delete(billCode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="billCode"></param>
        public void Delebgsqd(string billCode)
        {

            new ReportApplicationBLL().Delemode(billCode);
        }
        /// <summary>
        /// 删除借款申请单
        /// </summary>
        /// <param name="billCode"></param>
        public void Delejksqpo(string billCode)
        {
            new LoanListBLL().Delete(billCode);
        }
        /// <summary>
        /// 删除预算明细
        /// </summary>
        /// <param name="billCode"></param>
        public void DeleteKmystz(string billCode)
        {
            new YsglDal().DeleteYsmx(billCode);
        }

        public void DelteWeiXiuSqBill(string billCode)
        {
            new Bll.Zichan.ZiChan_WeiXiuShenQingBLL().Delete(billCode);
        }

        public void DeleteZiChanChuZhi(string billCode)
        {
            new Bll.Zichan.ChuZhiDanBll().Delete(billCode);
        }

        public void DeleteHksq(string id)
        {
            new LoanListBLL().DeleteI(id);
        }


        public void DeleteLyd(string guid)
        {

            new bill_lydBll().Delete(guid);
        }

        public void DeleteYksq(string guid)
        {

            new bill_yksqBll().Delete(guid);
        }
        /// <summary>
        /// 删除资产购置单大智
        /// </summary>
        /// <param name="billCode"></param>

        public void Deletezcgz(string billCode)
        {
            new Dal.FeiYong_DZ.dz_zcgzsqdDal().Delete(billCode);
        }
        /// <summary>
        /// 删除大智用款申请单
        /// </summary>
        /// <param name="billCode"></param>

        public void Delebyksqdz(string billCode)
        {
            new Dal.FeiYong_DZ.dz_yksqdDal().Delete(billCode);
        }
        /// <summary>
        /// 删除预算汇总
        /// </summary>
        /// <param name="billcode"></param>
        public void Deleyshz(string billcode) 
        {
            new Dal.Bills.MainDal().DeleteMain(billcode);
        }

        //删除转校转费申请表
        public void Deletezxzfsqd(string billCode)
        {
            new bill_zfzxsqd_dzDal().Delete(billCode);
        }
    }
}
