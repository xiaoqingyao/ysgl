using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Bll.UserProperty;
using Models;
using System.Data;
using WorkFlowLibrary.WorkFlowBll;

public partial class webBill_main_MainNew : System.Web.UI.Page
{

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            checkEdition();
            if (!IsPostBack)
            {
                left.InnerHtml = LeftInnerMaker();

                string userCode = Convert.ToString(Session["userCode"]);
                string username = Convert.ToString(Session["userName"]);
                if (this.Td1.InnerHtml != "" && this.Td1.InnerHtml != null)
                {
                    this.magdiv.Visible = true;
                }
                else
                {
                    this.magdiv.Visible = false;
                }
                //我的单据绑定
                string strposql = @"select  top 7 billName,flowID,billuser,isGk,gkDept,(select bxzy from bill_ybbxmxb 
where bill_ybbxmxb.billCode=bill_main.billCode) as bxzy,stepid,billDept,billCode,
(select xmmc from bill_ysgc where gcbh=billName) as billName,
(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName,
(select top 1 djmc from bill_djlx where djbh=bill_main.flowid) as billType,
convert(varchar(10),billdate,20) as billdate ,billje from bill_main
where (billUser='" + Session["userCode"].ToString().Trim() + "' or billCode in (select billCode from bill_ybbxmxb where bxr='" + Session["userCode"].ToString().Trim() + "')) and stepid<>'end' and flowid not in('ys','xmys','xmyshz','zjys','srys','zcys','chys','wlys') order by billdate desc";

                DataSet temp = server.GetDataSet(strposql);

                this.myGrid.DataSource = temp;

                this.myGrid.DataBind();

                if (this.myGrid.Rows.Count <= 0)
                {
                    this.podiv.Visible = false;

                }
                else
                {
                    this.podiv.Visible = true;
                }
            }
            //enter快速打开报销单的地址
            string strurl = "../bxgl/bxDetailFinal.aspx?type=add&par=";
            string peizhiurl = new Bll.ConfigBLL().GetValueByKey("EnterForBxURL");
            strurl = peizhiurl.Equals("") ? strurl : peizhiurl;
            hdEnterURL.Value = strurl;
        }

    }
    private string LeftInnerMaker()
    {
        StringBuilder sb = new StringBuilder();
        SysManager sysMgr = new SysManager();
        string userCode = Convert.ToString(Session["userCode"]);
        string roleCode = Convert.ToString(Session["userGroup"]);
        //IList<Bill_SysMenu> userList = sysMgr.GetMenuByUserAll(userCode);
        UserMessage usMgr = new UserMessage(userCode);
        IList<Bill_SysMenu> userList = new List<Bill_SysMenu>();


        //edit by lvcc 因为未登录 usMgr.GetMenu();会报异常
        try
        {
            userList = usMgr.GetMenu();
        }
        catch (Exception)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }


        IList<Bill_SysMenu> rootList = sysMgr.GetMenuRoot();
        foreach (Bill_SysMenu rootMenu in rootList)
        {
            var tempList = from linqtemp in userList
                           where linqtemp.MenuId.Substring(0, 2) == rootMenu.MenuId
                           orderby linqtemp.MenuOrder
                           select linqtemp;

            if (tempList.Count() > 0)
            {
                sb.Append("<h3><a>");
                sb.Append(rootMenu.ShowName);
                sb.Append("</a></h3>");
                sb.Append("<div><ul>");

                foreach (Bill_SysMenu userMenu in tempList)
                {
                    sb.Append("<li>");
                    sb.Append("<img src=\"../Resources/Images/菜单标记.JPG\" alt=\"\"/>");
                    sb.Append("<span datalink='");
                    sb.Append(userMenu.MenuUrl);
                    sb.Append("' linkname='");
                    sb.Append(userMenu.ShowName);
                    sb.Append("' class='addTabs' >");
                    sb.Append(userMenu.ShowName);
                    sb.Append("</span>");
                    sb.Append("</li>");
                }
                sb.Append("</ul></div>");
            }
        }
        return sb.ToString();

    }



    protected void myGrid_ItemDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Header)
        {
            string strbillcode = e.Row.Cells[0].Text.ToString().Trim();
            string strbillname = e.Row.Cells[6].Text.Trim();
            string strurl = @"";
            SysManager sysMgr = new SysManager();
            // e.Row.Cells[4].Text = sysMgr.GetDeptCodeName(e.Row.Cells[4].Text);
            string strpotype = e.Row.Cells[3].Text.ToString().Trim();//单据类型
            string zt = e.Row.Cells[2].Text;

            if (zt == "end")
            {
                e.Row.Cells[2].Text = "审批通过";
            }
            else
            {
                if (strpotype.Equals("gkbx"))
                {
                    WorkFlowRecordManager bll = new WorkFlowRecordManager();
                    string state = bll.WFState(strbillname);
                    e.Row.Cells[2].Text = state;

                }
                else if (strpotype.Equals("yszj") || strpotype.Equals("ystz"))
                {
                    string billcode = e.Row.Cells[0].Text.Trim();
                    string state = new WorkFlowRecordManager().WFState(billcode);
                    e.Row.Cells[2].Text = state;
                }
                else
                {
                    WorkFlowRecordManager bll = new WorkFlowRecordManager();
                    string state = bll.WFState(strbillname);
                    e.Row.Cells[2].Text = state;
                }
            }
            if (strpotype != "")
            {
                if (strpotype == "ybbx" || strpotype == "qtbx" || strpotype == "cgzf") //费用报销,其他报销 采购支付 收入报告 资产购置 存货领用 往来付款
                {
                    strurl = "../bxgl/bxDetailFinal.aspx?type=look&billCode=" + strbillcode + "&dydj=02";
                }
                else if (strpotype == "srd")
                {
                    strurl = "../bxgl/bxDetailFinal.aspx?type=look&billCode=" + strbillcode + "&dydj=01";

                }

                else if (strpotype == "zcgzbx")
                {
                    strurl = "../bxgl/bxDetailFinal.aspx?type=look&billCode=" + strbillcode + "&dydj=03";

                }
                else if (strpotype == "chly")
                {
                    strurl = "../bxgl/bxDetailFinal.aspx?type=look&billCode=" + strbillcode + "&dydj=04";

                }
                else if (strpotype == "wlfk")
                {
                    strurl = "../bxgl/bxDetailFinal.aspx?type=look&billCode=" + strbillcode + "&dydj=05";

                }
                else if (strpotype == "yksq_dz")//大智用款申请单
                {
                    strurl = "../bxgl/bxDetailFinal.aspx?type=look&billCode=" + strbillcode + "&dydj=06";
                }

                else if (strpotype == "gkbx")//归口报销
                {
                    strurl = "../bxgl/bxDetailForGK.aspx?type=look&billCode=" + strbillname;
                }
                else if (strpotype == "yzsq")//借款申请单
                {
                    strurl = "../../SaleBill/BorrowMoney/LoanListDetails.aspx?Ctrl=View&Code=" + strbillcode;
                }
                else if (strpotype == "yszj")//预算追加
                {
                    strurl = "../ysgl/yszjEdit.aspx?type=look&billCode=" + strbillcode;
                }
                else if (strpotype == "ystz")//预算调整
                {
                    strurl = "../ysgl/YstzDetailNew.aspx?type=look&billCode=" + strbillcode;
                }
                else if (strpotype == "kmystz")//科目预算调整
                {
                    strurl = "../ysgl/KmYstzDetail.aspx?Ctrl=View&billCode=" + strbillcode;
                }
                else if (strpotype == "cksj")//车款上缴报告
                {
                    strurl = "../../SaleBill/RemitTance/RemitTanceDetails.aspx?Ctrl=look&Code=" + strbillcode;
                }
                else if (strpotype == "qtbx")//其他报销
                {
                    strurl = "../bxgl/bxDetailFinal.aspx?type=look&djtype=qtbx&billCode=" + strbillcode;
                }
                else if (strpotype == "ccbg")//出差报告单
                {
                    strurl = "../fysq/travelReportDetail.aspx?Ctrl=View&Code=" + strbillcode;
                }
                else if (strpotype == "ccsq")//出差申请单
                {
                    strurl = "../fysq/travelApplicationDetails.aspx?Ctrl=View&Code=" + strbillcode;

                }
                else if (strpotype == "cgsp")//采购审批单
                {
                    strurl = "../fysq/cgspDetail.aspx?type=look&cgbh=" + strbillcode;
                }
                else if (strpotype == "yzsq")//借款申请单
                {
                    strurl = "../fysq/cgspDetail.aspx?type=look&cgbh=" + strbillcode;
                }
                else if (strpotype == "kmystz")//科目预算调整单
                {
                    strurl = "../ysgl/KmYstzDetail.aspx?Ctrl=View&billCode=" + strbillcode;
                }
                else if (strpotype == "kpsq")//开票申请 
                {
                    strurl = "../../SaleBill/kpsq/KpsqDetails.aspx?type=look&&bh=" + strbillcode;
                }
                else if (strpotype == "lscg")//临时采购  
                {
                    strurl = "../fysq/lscgDetail.aspx?type=audit&cgbh=" + strbillcode;
                }
                else if (strpotype == "cgzjjh")//采购资金计划单
                {
                    strurl = "../cgzj/cgzjDetail.aspx?type=look&par=" + strbillcode;
                }
                else if (strpotype == "ys")//预算填报
                {
                    strurl = "../ysglnew/cwtbDetail.aspx?from=lookDialog&billCode=" + strbillcode + "&yskmtype =02";
                }
                else if (strpotype == "srys")//收入预算填报
                {
                    strurl = "../ysglnew/cwtbDetail.aspx?from=lookDialog&billCode=" + strbillcode + "&yskmtype =01 ";
                }
                else if (strpotype == "zcys")//固定资产预算填报
                {
                    strurl = "../ysglnew/cwtbDetail.aspx?from=lookDialog&billCode=" + strbillcode + "&yskmtype =03 ";
                }
                else if (strpotype == "chys")//存货预算填报
                {
                    strurl = "../ysglnew/cwtbDetail.aspx?from=lookDialog&billCode=" + strbillcode + "&yskmtype =04 ";
                }
                else if (strpotype == "wlys")//往来预算填报
                {
                    strurl = "../ysglnew/cwtbDetail.aspx?from=lookDialog&billCode=" + strbillcode + "&yskmtype =05 ";
                }
                else if (strpotype == "xmzf")//项目支付 
                {
                    strurl = "../xmzf/xmzfsqDetail.aspx?type=look&billCode=" + strbillcode;
                }
                else if (strpotype == "cgzjfk")//采购资金付款单
                {

                }
                else if (strpotype == "xmys")//项目预算单
                {

                }
                else if (strpotype == "zccgsq")//资产采购申请 
                {
                    strurl = "../ZiChanGuanLi/CaiGouShenQingDetail.aspx?type=look&&bh=" + strbillcode;
                }

                else if (strpotype == "zcczd")//资产处置
                {
                    strurl = "../ZiChanGuanLi/ChuZhiDanDetail.aspx?type=look&&bh=" + strbillcode;
                }
                else if (strpotype == "wxsq")//维修申请  ZiChanGuanLi/WeiXiuShenQingDetail.aspx?Ctrl=View&Code
                {
                    strurl = "../ZiChanGuanLi/WeiXiuShenQingDetail.aspx?Ctrl=View&Code=" + strbillcode;
                }
                else if (strpotype == "zcgz")//固定资产申购单
                {

                    strurl = "../ /fysq/ZcgzsqDetail.aspx?Ctrl=View&Code=" + strbillcode + "&flg=g";
                }
                else if (strpotype == "fzcgz")//物品申购单
                {

                    strurl = "../ /fysq/ZcgzsqDetail.aspx?Ctrl=View&Code=" + strbillcode + "&flg=f";
                }
                else if (strpotype == "zfzxsqd")//跨区域转费转校申请单
                {

                    strurl = "../bxgl/zfzxsqd_dzDetails.aspx?ctrl=view&billcode=" + strbillcode + "&flg=k";
                }
                else if (strpotype == "nzfzxsqd")//区域内转费转校申请单
                {

                    strurl = "../bxgl/zfzxsqd_dzDetails.aspx?ctrl=view&billcode=" + strbillcode + "&flg=n";
                }


                string strtext = e.Row.Cells[1].Text;
                string strZhaiYao = e.Row.Cells[5].Text.Trim();
                strZhaiYao = strZhaiYao.Replace("&nbsp;", "");
                string strZhaiYaoShow = getString(strZhaiYao, 9);
                if (strZhaiYao.Length > 0)
                {
                    strZhaiYaoShow = "_" + strZhaiYaoShow;
                }
                e.Row.Cells[1].Text = strtext + strZhaiYaoShow;
            }
        }
    }

    /// <summary>
    /// 截取字符串
    /// </summary>
    /// <param name="RawString"></param>
    /// <param name="Length"></param>
    /// <returns></returns>
    public string getString(string RawString, Int32 Length)
    {
        return RawString.Length >= Length ? RawString.Substring(0, Length) + "..." : RawString;
    }
    /// <summary>
    /// 检查版本 通过加密狗
    /// </summary>
    private void checkeditionbyDog()
    {
        string strSql = "select top 1 sj from t_zcxx2";
        object objDate = server.ExecuteScalar(strSql);
        string strDate = objDate == null ? "" : objDate.ToString();
        if (strDate.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请检查加密狗是否插入！');window.open('../../webBill.aspx','_top');", true);
            return;
        }
        DateTime dtReg;
        if (!DateTime.TryParse(AES.Decrypt(strDate, "vsoft"), out dtReg))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('加密程序出现错误，请联系开发商解决！');window.open('../../webBill.aspx','_top');", true);
            return;
        }
        DateTime dtNow = DateTime.Now;
        if (dtReg.AddMinutes(5) < dtNow)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('注册时间过期，请联系开发商延续使用期限！');window.open('../../webBill.aspx','_top');", true);
        }
    }

    private void checkEdition()
    {
        //注册
        object objregistermark_date = System.Configuration.ConfigurationManager.AppSettings["RegistDate"];
        DateTime dtReg;
        if (objregistermark_date != null)
        {
            dtReg = DateTime.Parse(objregistermark_date.ToString());
            DateTime strnowdate = DateTime.Now;
            if (strnowdate > dtReg)
            {
                TimeSpan aa = DateTime.Parse("2013-05-26") - DateTime.Now;
                int iDays = aa.Days + 1;
                if (iDays > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('您好，试用版本已经到期，还有" + iDays + "天系统将锁定，请联系软件开发商！');", true);
                }
                else
                {
                    checkeditionbyDog();
                }
            }
        }
    }
}

