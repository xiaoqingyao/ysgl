using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Bll.UserProperty;
using Models;
using System.Collections.Generic;

public partial class webBill_bxgl_qtbxDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }

        ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());

        if (!IsPostBack)
        {
            IList<Bill_DataDic> list = (new SysManager()).GetDicByType("02");
            drpBxmxlx.DataTextField = "DicName";
            drpBxmxlx.DataValueField = "DicCode";
            drpBxmxlx.DataSource = list;
            drpBxmxlx.DataBind();
            if (Page.Request.QueryString["type"] == null || Page.Request.QueryString["type"].ToString().Trim() == "add")
            {
                string usercode = Convert.ToString(Session["userCode"]);
                UserMessage um = new UserMessage(usercode);
                txtJbr.Text = "[" + um.Users.UserCode + "]" + um.Users.UserName;
                txtSqrq.Text = DateTime.Now.ToString("yyyy-MM-dd");
                HiddenField1.Value = (new GuidHelper()).getNewGuid();
                SysManager sysMgr = new SysManager();
                Label1.Text = sysMgr.GetYbbxBillName("", DateTime.Now.ToString("yyyMMdd"), 0);
            }
            else if (Request.QueryString["type"] == "edit" || Request.QueryString["type"] == "look")
            {
                //this.txtSqrq.ReadOnly = true;
                string billCode = Request.QueryString["billCode"];
                HiddenField1.Value = billCode;
                BillManager mgr = new BillManager();
                Bill_Ybbxmxb ybbx = mgr.GetYbbx(billCode);
                Bill_Main main = mgr.GetMainByCode(billCode);
                Label1.Text = main.BillName;
                UserMessage billuser = new UserMessage(main.BillUser);
                txtBxr.Value = billuser.GetName();
                UserMessage bxr = new UserMessage(ybbx.Bxr);
                txtJbr.Text = bxr.GetName();
                Bill_Departments depts = billuser.GetRootDept();
                txtDept.Value = "[" + depts.DeptCode + "]" + depts.DeptName;
                txtSqrq.Text = main.BillDate.Value.ToString("yyyy-MM-dd");
                drpBxmxlx.SelectedValue = ybbx.Bxmxlx;
                txtBxzy.Text = ybbx.Bxzy;
                txtBxsm.Text = ybbx.Bxsm;
                txt_djs.Text = Convert.ToString(ybbx.Bxdjs);

                string isgk = main.IsGk;
                if (string.IsNullOrEmpty(isgk) || isgk == "0")
                {
                    rb_can.Checked = true;
                    rb_ok.Checked = false;
                }
                else
                {
                    rb_can.Checked = false;
                    rb_ok.Checked = true;
                    Bill_Departments gkDept = (new SysManager()).GetDeptByCode(main.GkDept);
                    txt_gk.Value = "[" + gkDept.DeptCode + "]" + gkDept.DeptName;
                }

                StringBuilder sb = new StringBuilder();
                SysManager sysMgr = new SysManager();
                int i = 0;
                foreach (Bill_Ybbxmxb_Fykm fykm in ybbx.KmList)
                {
                    YsManager ysmgr = new YsManager();
                    DateTime dt = Convert.ToDateTime(txtSqrq.Text);

                    string deptCode = "";
                    if (main.IsGk == "1")
                    {
                        deptCode = main.GkDept;
                    }
                    else
                    {
                        deptCode = billuser.GetRootDept().DeptCode;
                    }
                    string kmCode = fykm.Fykm;

                    //根据配置查看是否开启月度预算。
                    string nd = txtSqrq.Text.Substring(0, 4);

                    //根据备注文件取得预算过程号
                    string gcbh= ysmgr.GetYsgcCode(dt);
                   

                    decimal hfje = ysmgr.GetYueHf(gcbh, deptCode, kmCode);
                    decimal ysje = ysmgr.GetYueYs(gcbh, deptCode, kmCode);
                    decimal syje = ysje - hfje;

                    string kmname = fykm.Fykm;
                    kmname = sysMgr.GetYskmNameCode(kmname);
                    string je = fykm.Je.ToString("F02");
                    string se = fykm.Se.ToString("F02");
                    sb.Append("<h3><a href='#'>");
                    sb.Append(kmname);

                    sb.Append(" 预算:");
                    sb.Append(ysje.ToString());

                    sb.Append(" 剩余:");
                    sb.Append(syje.ToString());

                    sb.Append("</a></h3><div><ul><li style='height:36px'>[金额] <input type='text' value='");
                    sb.Append(je);
                    sb.Append("' class='ysje' onblur='htjeChange()' id='je" + i + "' />");
                    sb.Append("<input type='button' value='部门选择' id='bmch" + i + "' onclick='bmChoose(this)' />");
                    sb.Append("<input type='button' value='项目选择' id='xmch" + i + "' onclick='xmChoose(this)' />");
                    sb.Append(" </li>");
                    sb.Append("<li style='height:36px'>[税额] ");
                    sb.Append("<input type='text' onblur='htjeChange()' class='ysse' value='" + se + "' id='se" + i + "' />");
                    sb.Append("</li>");

                    sb.Append("<li><div id='dv_bm" + i + "'>[使用部门]");
                    sb.Append("<ul>");
                    foreach (Bill_Ybbxmxb_Fykm_Dept dept in fykm.DeptList)
                    {
                        string deptcode = dept.DeptCode;
                        deptcode = sysMgr.GetDeptCodeName(deptcode);
                        string depje = dept.Je.ToString();
                        sb.Append("<li style='height:36px'><span>" + deptcode + ":</span><input type='text' value='" + depje + "' /></li>");
                    }
                    sb.Append("</ul>");


                    sb.Append("</div></li><li><div id='dv_xm" + i + "'>[科目项目]");
                    sb.Append("<ul>");
                    foreach (Bill_Ybbxmxb_Hsxm xm in fykm.XmList)
                    {
                        string xmcode = xm.XmCode;
                        xmcode = sysMgr.GetXmCodeName(xmcode);
                        string xmje = xm.Je.ToString();
                        sb.Append("<li style='height:36px'><span>" + xmcode + ":</span><input type='text' value='" + xmje + "' /></li>");
                    }
                    sb.Append("</ul>");
                    sb.Append("</div></li></ul></div>");
                    i++;
                }

                StringBuilder fysqSb = new StringBuilder();
                string sql = "select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,b.cgze,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,'审批通过' as spzt from bill_main a,bill_cgsp b where a.flowid='cgsp' and a.billCode=b.cgbh ";
                string sql2 = "select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,b.yjfy as cgze,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,'审批通过' as spzt from bill_main a,bill_lscg b where a.flowid='lscg' and a.billCode=b.cgbh ";

                foreach (Bill_Ybbx_Fysq fysq in ybbx.FysqList)
                {
                    if (fysq.SqCode.Substring(0, 2) == "cg")
                    {
                        DataSet ds = server.GetDataSet(sql + "and billcode='" + fysq.SqCode + "'");
                        fysqSb.Append("<tr>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["billCode"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cgDept"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cbr"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["sj"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cglb"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cgze"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["sm"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["spzt"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("</tr>");
                    }


                    if (fysq.SqCode.Substring(0, 2) == "ls")
                    {
                        DataSet ds = server.GetDataSet(sql2 + "and billcode='" + fysq.SqCode + "'");
                        fysqSb.Append("<tr>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["billCode"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cgDept"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cbr"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["sj"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cglb"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cgze"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["sm"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["spzt"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("</tr>");
                    }
                }



                this.fykm.InnerHtml = sb.ToString();
                tb_fysq.InnerHtml = fysqSb.ToString();
                if (Request.QueryString["type"] == "look")
                {
                    btn_test.Visible = false;
                    btnAddFykm.Visible = false;
                }
            }
        }
    }

    protected void btnScdj_Click(object sender, EventArgs e)
    {

    }


    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["username"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }
    protected void btn_kmselect_Click(object sender, EventArgs e)
    {
        this.fykm.InnerHtml = "";
        string str = hf_kmselect.Value;
        string[] km = str.Split('|');

        StringBuilder sb = new StringBuilder();
        for (var i = 0; i < km.Length; i++)
        {
            sb.Append("<h3><a href='#'>" + km[i] + "</a></h3><div><ul><li style='height:36px'>[金额] <input type='text' value='0.00' class='ysje' onblur='htjeChange()' id='je" + i + "' /><input type='button' value='部门选择' id='bmch" + i + "' onclick='bmChoose(this)' /><input type='button' value='项目选择' id='xmch" + i + "' onclick='xmChoose(this)' /> </li><li style='height:36px'>[税额] <input type='text' onblur='htjeChange()' class='ysse' value='0.00' id='se" + i + "' /></li><li><div id='dv_bm" + i + "'>[使用部门]</div></li><li><div id='dv_xm" + i + "'>[科目项目]</div></li></ul></div>");
        }
        //$("#fykm").append(innerval);
        fykm.InnerHtml = sb.ToString();

        //txtSqrq.ReadOnly = true;
    }
}