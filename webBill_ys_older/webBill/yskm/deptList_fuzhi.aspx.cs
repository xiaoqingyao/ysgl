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

public partial class webBill_yskm_deptList_fuzhi : BasePage
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
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
        }
    }

    protected void Button1_click(object sender, EventArgs e)
    {
        string dept = Request["deptCode"];
        string strdjlx = Request["djlx"];
        string sdept = CutVal(txtdept.Text.Trim());
        if (Convert.ToInt32(server.GetCellValue("select count(*) from bill_departments where deptcode='" + sdept + "'")) == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('要复制的部门不存在，请重新输入');", true);
            return;
        }
        if (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(sdept))
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            list.Add("delete from bill_yskm_dept where deptCode='" + dept + "' and djlx='" + strdjlx + "'");
            DataTable dt = server.GetDataTable("select * from bill_yskm_dept where deptCode='" + sdept + "' and djlx='" + strdjlx + "' ", null);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add("insert into bill_yskm_dept (deptCode,yskmCode,cwkmCode,jfkmcode1,dfkmcode1,jfkmcode2,dfkmcode2,djlx) values ('" + dept + "','" + Convert.ToString(dt.Rows[i]["yskmCode"]) + "','','" + Convert.ToString(dt.Rows[i]["jfkmcode1"]) + "','" + Convert.ToString(dt.Rows[i]["dfkmcode1"]) + "','" + Convert.ToString(dt.Rows[i]["jfkmcode2"]) + "','" + Convert.ToString(dt.Rows[i]["dfkmcode2"]) + "','" + strdjlx + "') ");
            }
            if (server.ExecuteNonQuerysArray(list) == -1)//执行list中的sql
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
            }
        }


    }


    private string GetDeoptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments where deptcode !='" + Request["deptCode"] + "'");
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
