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
using Models;
using Bll.Bills;
using Bll.UserProperty;
using System.Collections.Generic;
using System.Text;

public partial class webBill_bxgl_LingYongDanDetail :BasePage
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
            ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
            ClientScript.RegisterArrayDeclaration("availableTagsDept", GetDeoptAll());
            if (!IsPostBack)
            {

                this.bindData();
            }
        }
    }
    private void bindData()
    {
        if (Request["type"] == "add")
        {
            string usercode = Convert.ToString(Session["userCode"]);
            UserMessage um = new UserMessage(usercode);
            txtJbr.Text = "[" + um.Users.UserCode + "]" + um.Users.UserName;
            txtBxr.Text = "[" + um.Users.UserCode + "]" + um.Users.UserName;
            Bill_Departments dept = um.GetRootDept();
            txtDept.Text = "[" + dept.DeptCode + "]" + dept.DeptName;
            txtLysj.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        else
        {
            string billCode = Request["billCode"];
            bill_lyd model = new bill_lydBll().GetModel(billCode);
            txtJbr.Text = model.zdr;
            txtBxr.Text = model.lyr;
            txtDept.Text = new SysManager().GetDeptCodeName(model.lyDept);
            txtLysj.Text = model.lyDate;
            txtBxsm.Text = model.sm;
            txtBz.Text = model.bz;
            txtHjjeXx.Value=Convert.ToString( model.je);

            DataTable dt = server.GetDataTable("select (select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=fykm ) as yskm ,je from bill_lyds where guid ='" + billCode + "'", null);
            string ret = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ret += "<tr >";
                ret += "<td>" + dt.Rows[i]["yskm"] + "</td>";
                ret += "<td><input type='text' class='baseText ysje' onblur='htjeChange();' value='" + dt.Rows[i]["je"] + "'  onkeyup='CheckNAN(this)' /></td>";
                ret += "</tr>";
            }
            body_fykm.InnerHtml = ret;
        }

    }
    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users where userStatus='1' ");
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
    private string GetDeoptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }



    }
    protected void btn_save_click(object sender, object e)
    {
        string hyskms = hfyskms.Value;

        string[] yskms = hyskms.Split(',');
        if (string.IsNullOrEmpty(hyskms) || yskms.Length == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择费用科目！');", true);
            return;
        }

        bill_lyd model = new bill_lyd();
        IList<bill_lyds> items = new List<bill_lyds>();

        bill_lydBll bll = new bill_lydBll();
        if (Request["type"] == "add")
        {
            model.guid = new billCoding().getLydCode();
        }
        else
        {
            model = bll.GetModel(Request["billCode"]);
        }

        model.zdr = SubString(txtJbr.Text);
        model.lyr = SubString(txtBxr.Text);
        model.lyDept = SubString(txtDept.Text);
        model.lyDate = txtLysj.Text;
        model.sm = txtBxsm.Text;
        model.bz = txtBz.Text;
        model.zt="1";

        decimal zje = 0;
        for (int i = 0; i < yskms.Length; i++)
        {
            bill_lyds temp = new bill_lyds();
            string[] arr = yskms[i].Split('|');
            if (arr.Length == 2)
            {
                temp.fykm = arr[0];
                temp.je = Convert.ToDecimal(arr[1]);
                temp.guid = model.guid;
                temp.myGuid = Guid.NewGuid().ToString("D");
                items.Add(temp);
                zje += Convert.ToDecimal(temp.je);
            }
        }
        model.je = zje;
        if (items.Count > 0)
        {
            if (bll.Add(model, items))
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败，原因，参数不合法！');", true);



    }
}
