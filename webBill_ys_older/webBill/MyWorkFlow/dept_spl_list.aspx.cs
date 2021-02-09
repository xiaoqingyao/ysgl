using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_MyWorkFlow_dept_spl_list : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_fz_Click(object sender, EventArgs e)
    {
        string fzdeptcode = "";
        if (!string.IsNullOrEmpty(txt_dept.Text))
        {
            fzdeptcode = txt_dept.Text;
        }
        if (!string.IsNullOrEmpty(fzdeptcode))
        {
            fzdeptcode = fzdeptcode.Substring(1, fzdeptcode.IndexOf("]") - 1);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择部门。')", true);
            return;
        }
        string strdeptcode = "";
        if (!string.IsNullOrEmpty(Request["deptcode"]))
        {
            strdeptcode = Request["deptcode"].ToString();
        }
        string strdjlx = "";
        if (!string.IsNullOrEmpty(Request["djlx"]))
        {
            strdjlx = Request["djlx"].ToString();
        }

        if (Convert.ToInt32(server.GetCellValue("select count(*) from bill_departments where deptcode='" + fzdeptcode + "'")) == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('要复制的部门不存在，请重新输入');", true);
            return;
        }
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        if (!string.IsNullOrEmpty(strdeptcode) && !string.IsNullOrEmpty(strdeptcode))
        {
            //判断选择的部门已经设置过审批流

            string strcountsql = "select count(*) from workflowstep where memo='" + fzdeptcode + "' and flowid='" + strdjlx + "'";
            string strcount = server.GetCellValue(strcountsql);
            if (strcount == "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择的部门没有设置审批流，请重新选择部门');", true);
                return;
            }

            list.Add("delete from workflowstep where memo='" + strdeptcode + "' and flowid='" + strdjlx + "'");
            list.Add(@"insert into workflowstep (stepid,steptype,steptext,checkcode,checktype,filterkemumanager ,memo,flowid)
select stepid,steptype,steptext,checkcode,checktype,filterkemumanager ,'" + strdeptcode + "','" + strdjlx + "'from workflowstep where memo='" + fzdeptcode + "' and flowid='" + strdjlx + "'");
        }
        int intRow = server.ExecuteNonQuerysArray(list);
        if (intRow > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);

            return;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }


    }
    private string GetDeoptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments where deptcode !='" + Request["deptcode"] + "'");
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