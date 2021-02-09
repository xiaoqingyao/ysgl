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
using Bll.ReportAppBLL;

public partial class SaleBill_ReportApplication_ReportApplication : System.Web.UI.Page
{
    string strbillcode = "";
    string strtype = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ReportApplicationBLL reportbll = new ReportApplicationBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            object objRequest = Request["Code"];
            object objRequsttype = Request["Ctrl"];
            if (objRequest != null && !string.IsNullOrEmpty(objRequest.ToString()))
            {
                strbillcode = objRequest.ToString();
            }
            if (objRequsttype != null && !string.IsNullOrEmpty(objRequsttype.ToString()))
            {
                strtype = objRequsttype.ToString();
            }

            if (!IsPostBack)
            {
                bindData();
            }
            ClientScript.RegisterArrayDeclaration("availableTagsdt", GetdetpAll());
            ClientScript.RegisterArrayDeclaration("availableTagsuser", GetuserAll());
            this.txtreportdate.Attributes.Add("onfocus", "javascript:setday(this);");

        }
       
    }

    private void bindData() 
    {
        if (strtype=="Add")
        {
            Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            string strneed = DateTime.Now.ToString("yyyyMMdd");
            string strcode = pusbll.GetBillCode("bgsq", strneed, 1, 6);
            this.lblcode.Text = strcode;
        }
        if (strtype=="Edit"&&strbillcode!=null&&strbillcode!="")
        {
            getmodel();
        }
       
    
    }
    /// <summary>
    /// 部门
    /// </summary>
    /// <returns></returns>
    private string GetdetpAll()
    {
        DataSet ds = server.GetDataSet("select deptCode, '['+deptCode+']'+deptName as dtname from  bill_departments");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dtname"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        return script;
    }

    /// <summary>
    /// 部门
    /// </summary>
    /// <returns></returns>
    private string GetuserAll()
    {
        DataSet ds = server.GetDataSet("select userCode, '['+userCode+']'+userName as usernames from  bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["usernames"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        return script;
    }
    public void getmodel()
    {
        Models.T_ReportApplication model = new Models.T_ReportApplication();
        if (strbillcode!=""&&strbillcode!=null)
        {
            model = reportbll.getmode(strbillcode);
            this.lblcode.Text = strbillcode;
            this.txtreportname.Text="["+model.ReportNameCode+"]"+model.ReportName;
            this.txtreportdept.Text="["+model.ReportDeptCode+"]"+model.ReportDeptName;
            this.txtreportdate.Text = model.ReportDate;
            this.txtreportexplain.Text = model.ReportExplain;
        }
    
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {

            Models.T_ReportApplication model = new Models.T_ReportApplication();
            model.ReportAppCode = this.lblcode.Text.Trim();
            model.ReportDate = this.txtreportdate.Text.Trim();
            string strdept = this.txtreportdept.Text.Trim();
            model.ReportDeptCode = strdept.Substring(1, strdept.IndexOf("]") - 1).Trim();
            model.ReportDeptName = strdept.Substring(strdept.IndexOf("]") + 1).Trim();
            model.ReportExplain = this.txtreportexplain.Text;
            string strusername = this.txtreportname.Text.Trim();
            model.ReportNameCode = strusername.Substring(1, strusername.IndexOf("]") - 1).Trim();
            model.ReportName = strusername.Substring(strusername.IndexOf("]") + 1).Trim();
           int row= reportbll.Addmodel(model);
           if (strtype=="Add")
           {
               if (row > -1)
               {
                   ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加成功！');window.returnValue=\"sucess\";self.close();", true);

               }
               else
               {
                   ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败！');window.returnValue=\"sucess\";self.close();", true);

               }

           }
           else
           {
               if (row > -1)
               {
                   ClientScript.RegisterStartupScript(this.GetType(), "", "alert('修改成功！');window.returnValue=\"sucess\";self.close();", true);

               }
               else
               {
                   ClientScript.RegisterStartupScript(this.GetType(), "", "alert('修改失败！');window.returnValue=\"sucess\";self.close();", true);

               }
           }
          
       // this.txtnid.Text = strcode;
    }
}
