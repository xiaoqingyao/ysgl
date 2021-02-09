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
using System.Text;
using System.Collections.Generic;

public partial class webBill_xmsz_CopyDeptXm : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strdeptCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        object objDeptCode = Request["deptCode"];
        if (objDeptCode != null)
        {
            strdeptCode = objDeptCode.ToString();
            string strsqk = "select '['+deptCode+']'+deptName from bill_departments where deptCode='"+strdeptCode+"'";
            object objdeptname = server.ExecuteScalar(strsqk);
            lblToDept.Text=objdeptname==null?"":objdeptname.ToString();
        }
        ClientScript.RegisterArrayDeclaration("availableTagsDept", GetDeptAll());
    }
    protected void btn_sure_Click(object sender, EventArgs e)
    {
        if (strdeptCode.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('重要参数丢失！');window.returnValue='no';self.close();", true);
            return;
        }
        string strFrmDept = this.txtFromDept.Text.ToString();
        string strFrmDeptCode = "";
        try
        {
            strFrmDeptCode = strFrmDept.Substring(1, strFrmDept.IndexOf("]") - 1);
        }
        catch (Exception)
        {
            strFrmDeptCode = "";
        }
        if (strFrmDeptCode.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择要复制的部门！');window.returnValue='no';self.close();", true); return;
        }
        string strSql = "delete from bill_xm where xmDept='" + strdeptCode + "'";
        string strsql2 = "insert into bill_xm select xmCode,xmName,sjXm,'" + strdeptCode + "',xmStatus from bill_xm where xmDept='" + strFrmDeptCode + "'";
        List<string> lstsql = new List<string>();
        lstsql.Add(strSql);
        lstsql.Add(strsql2);
        if (server.ExecuteNonQuerysArray(lstsql) > -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('复制成功！');window.returnValue='yes';self.close();", true);
        }
        else {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('复制失败！');window.returnValue='no'self.close();", true);
        }
    }
    private string GetDeptAll()
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
}
