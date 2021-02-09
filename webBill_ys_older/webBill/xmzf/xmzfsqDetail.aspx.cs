using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Models;
using Bll.UserProperty;


public partial class webBill_xmzf_xmzfsqDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                string userCode = Convert.ToString(Session["userCode"]);
                string type = Page.Request.QueryString["type"].ToString().Trim();
                 if (type == "add")
                 {
                     this.txtCgrq.Attributes.Add("onfocus", "javascript:setday(this);");
                     txtCgrq.Text = DateTime.Now.ToString("yyyy-MM-dd");
                     UserMessage userMgr = new UserMessage(userCode);
                     Bill_Departments dept = userMgr.GetRootDept();
                     lblDept.Text = dept.DeptCode;
                     lblxsDept.Text = "[" + dept.DeptCode + "]" + dept.DeptName;
                     lblCbr.Text = "[" + userMgr.Users.UserCode + "]" + userMgr.Users.UserName;

                     lblCgbh.Text = (new SysManager()).GetYbbxBillName("XMZF", Convert.ToDateTime(this.txtCgrq.Text.ToString().Trim()).ToString("yyyyMMdd"), 0);
                     IList<Bill_Xm> list = (new SysManager()).GetmjXmByDep(dept.DeptCode);
                   
                     this.ddl_cglb.DataTextField = "XmName";
                     this.ddl_cglb.DataValueField = "XmCode";
                     this.ddl_cglb.DataSource = list;
                     this.ddl_cglb.DataBind();
                 }
                 else if (type == "look" || type == "search")
                 {
                     string billCode = Request.QueryString["billCode"];
                     XmzfManger xm = new XmzfManger();
                     DataTable dt = xm.GetXmzfsqd(billCode, "", "", "", "", "", "");
                     if (dt.Rows.Count > 0)
                     {
                         this.lbl_BillCode.Text = billCode;
                         this.lblCgbh.Text = billCode;
                         this.txtCgrq.Text = Convert.ToDateTime(dt.Rows[0]["billdate"]).ToString("yyyy-MM-dd");
                         txtCgrq.Enabled = false;
                         this.lblxsDept.Text = "[" + Convert.ToString(dt.Rows[0]["billdept"]) + "]" + Convert.ToString(dt.Rows[0]["deptname"]);
                         this.ddl_cglb.ClearSelection();
                         this.ddl_cglb.Items.Add(new ListItem(Convert.ToString(dt.Rows[0]["xmname"]), Convert.ToString(dt.Rows[0]["zfxm"])));
                         this.ddl_cglb.Items[1].Selected = true;
                         this.ddl_cglb.Enabled = false;
                         this.txtZynr.Text = Convert.ToString(dt.Rows[0]["zynr"]);
                         txtZynr.Enabled = false;
                         this.txtSm.Text = Convert.ToString(dt.Rows[0]["sm"]);
                         txtSm.Enabled = false;
                         this.lblCbr.Text = "[" + Convert.ToString(dt.Rows[0]["billuser"]) + "]" + Convert.ToString(dt.Rows[0]["username"]);
                         this.txtYjfy.Text = Convert.ToString(dt.Rows[0]["billje"]);
                         txtYjfy.Enabled = false;
                         btn_bc.Visible = false;
                     }
                     else
                     {
                         Response.Redirect("xmzfsqd.aspx");
                     }
                 }
                 else if (type == "edit")
                 {
                     string billCode = Request.QueryString["billCode"];
                     XmzfManger xm = new XmzfManger();
                     DataTable dt = xm.GetXmzfsqd(billCode, "", "", "", "", "", "");
                     if (dt.Rows.Count > 0)
                     {
                         this.lbl_BillCode.Text = billCode;
                         this.lblCgbh.Text = billCode;
                         this.txtCgrq.Text = Convert.ToDateTime(dt.Rows[0]["billdate"]).ToString("yyyy-MM-dd");
                         this.lblxsDept.Text = "[" + Convert.ToString(dt.Rows[0]["billdept"]) + "]" + Convert.ToString(dt.Rows[0]["deptname"]);
                         this.lblDept.Text = Convert.ToString(dt.Rows[0]["billdept"]);
                         this.ddl_cglb.ClearSelection();
                         IList<Bill_Xm> list = (new SysManager()).GetmjXmByDep(Convert.ToString(dt.Rows[0]["billdept"]));

                         this.ddl_cglb.DataTextField = "XmName";
                         this.ddl_cglb.DataValueField = "XmCode";
                         this.ddl_cglb.DataSource = list;
                         this.ddl_cglb.DataBind();
                         this.ddl_cglb.Items.FindByValue(Convert.ToString(dt.Rows[0]["zfxm"])).Selected = true;
                         this.txtZynr.Text = Convert.ToString(dt.Rows[0]["zynr"]);
                         this.txtSm.Text = Convert.ToString(dt.Rows[0]["sm"]);
                         this.lblCbr.Text = "[" + Convert.ToString(dt.Rows[0]["billuser"]) + "]" + Convert.ToString(dt.Rows[0]["username"]);
                         this.txtYjfy.Text = Convert.ToString(dt.Rows[0]["billje"]);
                     }
                     else
                     {
                         Response.Redirect("xmzfsqd.aspx");
                     }
                 }
            }
        }
    }

    protected void btn_bc_Click(object sender, EventArgs e)
    {
        Bill_Main main = new Bill_Main();
        string type = Page.Request.QueryString["type"].ToString().Trim();
        string billcode = this.lbl_BillCode.Text.Trim();
        if (type == "add")
        {
            billcode = (new SysManager()).GetYbbxBillName("XMZF", Convert.ToDateTime(this.txtCgrq.Text.ToString().Trim()).ToString("yyyyMMdd"), 1);
        }
        main.BillCode = billcode;
        main.BillDate = Convert.ToDateTime(this.txtCgrq.Text.ToString().Trim());
        main.BillDept = this.lblDept.Text.ToString().Trim();
        main.BillJe = Convert.ToDecimal(this.txtYjfy.Text.ToString().Trim());
        main.BillName = billcode;
        main.BillName2 = billcode;
        main.BillUser = Convert.ToString(Session["userCode"]); //this.lblCbr.Text.ToString().Trim();
        main.FlowId = "xmzf";
        main.StepId = "-1";

        Bill_xmzfd model = new Bill_xmzfd();
        model.Billcode = billcode;
        model.Cbr = Convert.ToString(Session["userCode"]); //this.lblCbr.Text.ToString().Trim();
        model.Sj = Convert.ToDateTime(this.txtCgrq.Text.ToString().Trim());
        model.Sm = this.txtSm.Text.ToString().Trim();
        model.ZfDept = this.lblDept.Text.ToString().Trim();
        model.Zfje = Convert.ToDecimal(this.txtYjfy.Text.ToString().Trim());
        model.Zfxm = this.ddl_cglb.Text.ToString().Trim();
        model.Zynr = this.txtZynr.Text.ToString().Trim();

        XmzfManger mgr = new XmzfManger();
        mgr.InsertXmzfdDal(model, main);
       
        Response.Redirect("xmzfsqd.aspx");
    }
    protected void btn_fh_Click(object sender, EventArgs e)
    {
        if (Page.Request.QueryString["type"].ToString().Trim() == "search")
        {
            Response.Write(" <script> window.opener=null;window.close(); </script> "); 
        }
        else
        {
            Response.Redirect("xmzfsqd.aspx");
        }
        //Response.Write("<script> history.go(-3) </script>");
    }
}
