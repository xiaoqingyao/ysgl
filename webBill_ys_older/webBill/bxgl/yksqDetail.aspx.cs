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
using System.Text;

public partial class webBill_bxgl_yksqDetail : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        ClientScript.RegisterArrayDeclaration("availableTagsDept", GetDeoptAll());
        string type = Request["type"];
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(type) && type != "add")
            {
                BindData();
                if (type=="view")
                {
                    this.btn_save.Visible=false;
                    this.select_rk.Disabled=true;
                }
            }
            else
            {
                txtJbr.Text = server.GetCellValue("select '['+userCode+']'+userName from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'"); 
                txtSqrq.Text =DateTime.Now.ToString("yyyy-MM-dd");
                string deptCode = server.GetCellValue("select userDept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'");
                txtDept.Text = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where deptCode='" + deptCode + "'");
            }
        }
    }

    private void BindData()
    {
        string billCode = Request["billCode"];
        if (!string.IsNullOrEmpty(billCode))
        {
            bill_yksq model = bll.GetModel(billCode);
            if (model != null)
            {
                txtJbr.Text =server.GetCellValue("select '['+userCode+']'+userName from bill_users where usercode='"+model.jbr+"'");  ;
                txtSqrq.Text = Convert.ToDateTime( model.ddefine0).ToString("yyyy-MM-dd");
                txtDept.Text = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where deptCode='" + model.billDept + "'"); 
                txtJe.Text = model.je.ToString();
                txtRkCodes.Text = model.rkCodes;
                txtyt.Text = model.yt;
            }
        }
    }

    bill_yksqBll bll = new bill_yksqBll();
    protected void btn_save_Click(object senser, EventArgs e)
    {

        bill_yksq model = new bill_yksq();

        if (Request["type"] != "add")
        {
            string billCode = Request["billCode"];
            model = bll.GetModel(billCode);
        }
        else
        {
            model.billCode = new billCoding().getYksqCode();
        }
        model.jbr = SubString(txtJbr.Text.Trim());
        model.billDept = SubString(txtDept.Text.Trim());
        model.je = MyDecimal(txtJe.Text.Trim());
        model.rkCodes = txtRkCodes.Text.Trim();
        model.yt = txtyt.Text.Trim();
        model.ddefine0 =  Convert.ToDateTime(txtSqrq.Text.Trim());
        if (bll.Add(model))
        {
             ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        } 
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败，请先“系统配置项”菜单配置用款申请配置项。');", true);
        }

    }

    private decimal MyDecimal(string str)
    {
    decimal ret=0;
    ret=string.IsNullOrEmpty(str)?0:decimal.Parse(str);
    return ret;
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
}
