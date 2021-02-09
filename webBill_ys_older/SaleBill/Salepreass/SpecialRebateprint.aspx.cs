using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Bll.Bills;
using Bll;
using Models;
using System.Collections.Generic;

public partial class SaleBill_Salepreass_SpecialRebateprint : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    SpecialRebatesAppBLL spebll = new SpecialRebatesAppBLL();
    BillMainBLL bllBillMain = new BillMainBLL();
    string strCtrl = "";
    string strBillCode = "";
    string strUserCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            strUserCode = Session["userCode"].ToString().Trim();
            object objCtrl = Request["Ctrl"];
            if (objCtrl != null)
            {
                strCtrl = objCtrl.ToString();
            }
            object objCode = Request["Code"];
            if (objCode != null)
            {
                strBillCode = objCode.ToString();
            }
            if (!IsPostBack)
            {
                this.bindData();

            }
        }
    }
    private void bindData()
    {
        if (strCtrl=="print")
        {
            getmodel();
        }
      
    }
    /// <summary>
    /// 获取model
    /// </summary>
    public void getmodel()
    {
        IList<T_SpecialRebatesAppmode> spemode = spebll.GetListByBillCode(strBillCode);
        Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
        string deptcode = spemode[0].SaleDeptCode.ToString();
        string strsql = "select '['+deptCode+']'+deptName from bill_departments where deptCode='" + deptcode + "'";
        string strdeptname = server.GetCellValue(strsql);
        if (strdeptname!=null&&strdeptname!="")
        {
            this.lbdept.Text = strdeptname;

        }

        this.txtAppDate.Text = spemode[0].AppDate.ToString();
        this.lbeBillCode.Text = spemode[0].Code.ToString();
        //this.TextBox1.Text = spemode[0].Explain.ToString();
       // this.txtReasion.Text = spemode[0].Note1.ToString();//订单号
        this.txtWorkPlan.Text = spemode[0].TruckCount.ToString();
        //this.txtFeePlan.Text = spemode[0].TruckCode.ToString();//底牌号=车架号
        //this.txtTransport.Text = spemode[0].StandardSaleAmount.ToString();
       // this.TextBox2.Text = spemode[0].ExceedStandardPoint.ToString();
        this.txtbgtime.Text = spemode[0].EffectiveDateFrm.ToString();
        this.txtendtime.Text = spemode[0].EffectiveDateTo.ToString();

        List<T_SpecialRebatesAppmode> cxblist = new List<T_SpecialRebatesAppmode>();

        if (spemode.Count == 0)
        {
            cxblist.Add(new T_SpecialRebatesAppmode());
        }
        else
        {
            foreach (var i in spemode)
            {
                T_SpecialRebatesAppmode cx = new T_SpecialRebatesAppmode();
                cx.Note1 = i.Note1;
                cx.TruckCode = i.TruckCode;
                cx.Explain = i.Explain;
                cx.ExceedStandardPoint = i.ExceedStandardPoint;
                cx.StandardSaleAmount = i.StandardSaleAmount;
                cxblist.Add(cx);
            }
        }
        GridView1.DataSource = cxblist;
        GridView1.DataBind();
    }
}
