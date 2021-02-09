<%@ WebHandler Language="C#" Class="GetYskmHandler" %>

using System;
using System.Web;
using Bll.UserProperty;

public class GetYskmHandler : IHttpHandler
{

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string deptCode = context.Request.Params["deptCode"];
        string km = context.Request.Params["kmcode"];
        string isgk = context.Request.Params["isgk"];
        string billDate = context.Request.Params["billDate"];
        string dydj = context.Request.Params["dydj"];
        
        string result="";
        DateTime dt = DateTime.Now;
        if (!string.IsNullOrEmpty(billDate))
        {
            dt = Convert.ToDateTime(billDate);
        }
        
         //根据配置查看是否开启月度预算。
                string nd = dt.ToString("yyyy-MM-dd").Substring(0, 4);
                string config = (new SysManager()).GetsysConfigBynd(nd)["MonthOrQuarter"];


                JsonRet temp = new JsonRet();
                temp.Yscode =server.GetCellValue("select isnull('['+yskmCode+']'+yskmMc,yskmCode) from bill_yskm where yskmCode='"+km+"'");

                YsManager ysmgr = new YsManager();
                string kmCode = km;
                //dt.Year.ToString(), dt.Month
                string gcbh = ysmgr.GetYsgcCode(dt);
                
                //是否启用销售提成模块
                bool hasSaleRebate = new Bll.ConfigBLL().GetValueByKey("HasSaleRebate").Equals("1");
                decimal hfje = ysmgr.GetYueHf(gcbh, deptCode, kmCode);
                decimal ysje = ysmgr.GetYueYs(gcbh, deptCode, kmCode);
                decimal syje = ysje - hfje;
                decimal tcje = 0;
                if (hasSaleRebate)
                {
                    tcje = ysmgr.getEffectiveSaleRebateAmount(deptCode, kmCode);
                }

                decimal kyje = syje + tcje;
                //sb.Append(" 预算:"+ysje.ToString()+" 剩余:"+syje.ToString());

                #region 2014-03-17回滚预算控功能添加  --2014-03-24注掉
                //bool IsRollCtrl = new ConfigBLL().GetValueByKey("IsRollCtrl").Equals("1");
                //decimal rollje = 0;
                //if (IsRollCtrl)
                //{
                //    rollje= ysmgr.GetRollSy(gcbh, deptCode, kmCode);
                //}

                //syje += rollje;
                //kyje += rollje;
                #endregion


                //sb.Append("|");
                temp.Ysje = ysje;
                temp.Syje = syje;
                temp.Tcje = tcje;
                temp.Kyje = kyje;
                //是否项目核算
                temp.XiangMuHeSuan = new Dal.SysDictionary.YskmDal().GetYskmByCode(kmCode).XmHs.Equals("1") ? "是" : "否";
                temp.Bmhs = new Dal.SysDictionary.YskmDal().GetYskmByCode(kmCode).BmHs.Equals("1") ? "是" : "否";
                //部门
                temp.dept = new DepartmentBLL().GetShowNameByCode(deptCode);
                //预算使用比例
                string strbl = ysje == 0 ? "0%" : Math.Round(((ysje - syje) / ysje * 100), 2).ToString() + "%";
                temp.sybl = strbl;

            //System.Web.Script.Serialization.JavaScriptSerializer jserializer = new JavaScriptSerializer();
            //string script = jserializer.Serialize(retlist);
         result=JsonHelper.ObjectToJson(temp);
        context.Response.Write(result);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
        
        
         class JsonRet
    {
        public string Yscode { get; set; }
        public decimal Ysje { get; set; }
        public decimal Syje { get; set; }
        /// <summary>
        /// 销售提成金额  如果T_config没有配置系统使用该功能，则该字段无用
        /// </summary>
        public decimal Tcje { get; set; }
        /// <summary>
        /// 可用金额 预算剩余金额+费用提成金额  如果T_config没有配置系统使用该功能，则该字段无用
        /// </summary>
        public decimal Kyje { get; set; }
        /// <summary>
        /// 部门核算
        /// </summary>
        public string Bmhs{get;set;}
        /// <summary>
        /// 项目核算
        /// </summary>
        public string XiangMuHeSuan { get; set; }
        public string dept { get; set; }
        /// <summary>
        /// 预算使用比例
        /// </summary>
        public string sybl { get; set; }
    }
}