using System;
using System.Collections.Generic; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

public partial class Dept_deptDetails : System.Web.UI.Page
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
            //选择上级单位按钮绑定
            this.btn_seledept.Attributes.Add("onclick", "javascript:selectdept('../select/deptFrame.aspx','txb_sjdept');return false;");
            this.btn_selectXm.Attributes.Add("onclick", "javascript:selectdept('../select/xmFrame.aspx?deptCode=" + Page.Request.QueryString["deptCode"].ToString().Trim() + "','txtSjxm');return false;");
           
            if (!IsPostBack)
            {
                bindData();
            }
        }
    }

    #region 页面数据绑定
    private void bindData()
    {
        string type = Page.Request.QueryString["type"].ToString().Trim();
        if (type == "add")
        {
            if (Page.Request.QueryString["deptCode"].ToString().Trim() != "")
            {
                this.txb_sjdept.Value = server.GetCellValue("select  '['+deptcode+']'+deptname  from bill_departments where deptCode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");
            }

            this.CreateDeptCode();
        }
        else
        {
            this.txb_deptcode.ReadOnly = true;
            string strdeptcode = "";
            string strxmcode = "";
            StringBuilder sb = new StringBuilder();
            if (Page.Request.QueryString["deptCode"].ToString().Trim() != "")
            {
                strdeptcode = Page.Request.QueryString["deptCode"].ToString().Trim();
            }
            if (Page.Request.QueryString["xmCode"].ToString().Trim()!="")
            {
                strxmcode = Page.Request.QueryString["xmCode"].ToString().Trim();
            }

            sb.Append("select xmstatus,xmcode,xmname,sjxm,xmdept,(select top 1 '['+xmcode+']'+xmname from bill_xm b where xmcode=(select sjXm from bill_xm b where xmcode='" + strxmcode + "'and xmdept='" + strdeptcode + "')) as sjxmname,(select '['+deptcode+']'+deptname from bill_departments where deptcode=a.xmdept) as xmdeptname from bill_xm a where xmcode='" + strxmcode + "' and  xmdept='" + strdeptcode + "'");
            DataSet ds = server.GetDataSet(sb.ToString());
            //查询出一条数据
            if (ds.Tables[0].Rows.Count == 1)
            {
                txb_deptcode.Text = ds.Tables[0].Rows[0]["xmCode"].ToString();
                txb_deptname.Text = ds.Tables[0].Rows[0]["xmname"].ToString();
                txb_sjdept.Value = ds.Tables[0].Rows[0]["xmdeptname"].ToString();
                txtSjxm.Value = ds.Tables[0].Rows[0]["sjxmname"].ToString();
                this.DropDownList1.SelectedValue = ds.Tables[0].Rows[0]["xmstatus"].ToString();
               
              
               
            }
        }
    }
    #endregion

    #region 保存按钮事件
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string str_sjdept = "";//判断是否选择了上级？
        if (txb_sjdept.Value.ToString().Trim() != "")
        {
            str_sjdept = txb_sjdept.Value.Substring(txb_sjdept.Value.IndexOf("[") + 1, txb_sjdept.Value.IndexOf("]") - 1);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('项目所属部门不允许为空，请添加！');", true);
            return;
        }

        string str_sjxm = "";//判断是否选择了上级？
        if (txtSjxm.Value.ToString().Trim() != "")
        {
            str_sjxm = txtSjxm.Value.Substring(txtSjxm.Value.IndexOf("[") + 1, txtSjxm.Value.IndexOf("]") - 1);
        }

        string type = Page.Request.QueryString["type"].ToString().Trim();
        List<string> list = new List<string>();
     
      
        if (type == "add")
        {
            DataSet temp = server.GetDataSet("select xmcode from bill_xm where xmcode='" + this.txb_deptcode.Text.ToString().Trim() + "'");
            if (temp.Tables[0].Rows.Count != 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的项目已存在,请重新生成编号！');", true);
                this.btnAgain.Visible = true;
                return;
            }
            list.Add("insert into bill_xm (xmcode,xmname,sjxm,xmdept,xmStatus) values('" + this.txb_deptcode.Text.ToString().Trim() + "','" + this.txb_deptname.Text.ToString().Trim() + "','" + str_sjxm + "','" + str_sjdept + "','"+this.DropDownList1.SelectedItem.Value+"') ");
        }
        else //编辑
        {
            ////是否选了本单位或下级作为上级
            //string xjDeptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", true);//获取所有下级单位
            //xjDeptCodes = "'" + xjDeptCodes.Replace(",","','") + "'";
            //if (xjDeptCodes.IndexOf("'" + str_sjdept + "'") >= 0)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('不能选择本部门或下级 作为上级部门！');", true);
            //    return;
            //}
            list.Add("update bill_xm set xmstatus='" + this.DropDownList1.SelectedItem.Value + "',xmname='" + this.txb_deptname.Text.ToString().Trim() + "',xmdept='" + str_sjdept + "',sjxm='" + str_sjxm + "'  where xmcode ='" + this.txb_deptcode.Text.ToString().Trim() + "'");
            //if (deptStatus == "0")
            //{
            //    string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", true);
            //    list.Add("update bill_departments set deptstatus='0' where deptCode in(" + deptCodes + ")");//注意如何更新下级！
            //}
        }

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
    }
    #endregion

    #region 返回按钮事件
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"\";self.close();", true);
    } 
    #endregion

    protected void btnAgain_Click(object sender, EventArgs e)
    {
        this.CreateDeptCode();
    }

    public void CreateDeptCode()
    {
        string deptCode = (new billCoding()).getXmCode();
        if (deptCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成项目编号错误,请与开发商联系！');", true);
            this.btn_save.Visible = false;
        }
        else
        {
            this.txb_deptcode.Text = deptCode;
        }
    }
    protected void Button1_ServerClick(object sender, EventArgs e)
    {
        this.txtSjxm.Value = "";
    }
}
