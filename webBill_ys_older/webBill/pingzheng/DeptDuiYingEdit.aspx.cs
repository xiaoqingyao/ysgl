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
using System.Web.UI.MobileControls;
using System.Collections.Generic;

public partial class webBill_pingzheng_DeptDuiYingEdit : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strcurrentdeptname = "";
    string strDeptName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        object objcurrentdeptname = Request["currentdeptname"];
        if (objcurrentdeptname != null)
        {
            strcurrentdeptname = objcurrentdeptname.ToString();
        }
        object objDeptName = Request["osdeptname"];
        if (objDeptName!=null)
        {
            strDeptName = objDeptName.ToString();
        }
        if (!IsPostBack)
        {
            binddata();
        }
    }
    private void binddata() {
        this.lblOlderName.Text = strcurrentdeptname;
        this.txtDeptName.Text = strDeptName;
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        if (strcurrentdeptname.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('重要参数丢失，请刷新后重试！');", true);
            return;
        }
        string strDeptName = this.txtDeptName.Text.Trim();
        if (strDeptName.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门名称不能为空！');", true);
            return;
        }
        string strsql1 = "delete from bill_pingzheng_bumenduiying where Note1='" + strcurrentdeptname + "'";
        string strsql2 = "insert into bill_pingzheng_bumenduiying (OSDeptCode,OSDeptName,Note1) values('','" + strDeptName + "','"+strcurrentdeptname+"')";
        List<string> listSql=new List<string>();
        listSql.Add(strsql1);
        listSql.Add(strsql2);
        if (server.ExecuteNonQuerysArray(listSql) > -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');self.close();", true);
        }
        else {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');window.returnValue='success';self.close();", true);
        }
    }
}
