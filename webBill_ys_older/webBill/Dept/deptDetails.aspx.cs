using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Bll;

public partial class Dept_deptDetails : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ConfigBLL bllConfig = new ConfigBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            //选择上级单位按钮绑定
            btn_seledept.Attributes.Add("onclick", "javascript:selectdept('../select/deptFrame.aspx','txb_sjdept');return false;");
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
        if (bllConfig.GetValueByKey("HasSaleRebate").Equals("1"))
        {
            this.trisSale.Visible = true;
        }
        else
        {
            this.trisSale.Visible = false;
        }

        if (type == "add")
        {
            this.RadioButton1.Checked = true;
            this.isIsSellN.Checked = true;
            this.isGkN.Checked = true;
            if (Page.Request.QueryString["pCode"].ToString().Trim() != "")
            {
                this.txb_sjdept.Value = server.GetCellValue("select  '['+deptcode+']'+deptname  from bill_departments where deptCode='" + Page.Request.QueryString["pCode"].ToString().Trim() + "'");
            }

            this.CreateDeptCode();
        }
        else
        {
            this.txb_deptcode.ReadOnly = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("select deptcode,forU8id,forTianJian,deptname,(select  '['+deptcode+']'+deptname  from bill_departments where a.sjdeptcode=deptcode) sjdeptcode,deptstatus,Isgk,IsSell,deptJianma,iskzys from bill_departments a where deptcode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");
            DataSet ds = server.GetDataSet(sb.ToString());
            //查询出一条数据
            if (ds.Tables[0].Rows.Count == 1)
            {
                txb_deptcode.Text = ds.Tables[0].Rows[0]["deptcode"].ToString();
                txb_deptname.Text = ds.Tables[0].Rows[0]["deptname"].ToString();

                txb_sjdept.Value = ds.Tables[0].Rows[0]["sjdeptcode"].ToString();
                this.txtJianma.Text = ds.Tables[0].Rows[0]["deptJianma"].ToString();
                string status = ds.Tables[0].Rows[0]["deptstatus"].ToString();
                string strIsSell = ds.Tables[0].Rows[0]["IsSell"].ToString();
                string strIsgk = ds.Tables[0].Rows[0]["Isgk"].ToString();
                string striskzys = ds.Tables[0].Rows[0]["iskzys"].ToString();
                if (status == "1") this.RadioButton1.Checked = true;
                else this.RadioButton2.Checked = true;
                if (striskzys == "N")
                {
                    this.Niskzys.Checked = true;
                }
                else
                {
                    this.Yiskzys.Checked = true;
                }
                if (strIsgk == "Y")
                {
                    this.isGkY.Checked = true;
                }
                else
                {
                    this.isGkN.Checked = true;
                }
                if (strIsSell == "Y")
                {
                    this.isIsSellY.Checked = true;
                }
                else
                {
                    this.isIsSellN.Checked = true;
                }
                txtForU8id.Text = ds.Tables[0].Rows[0]["forU8id"].ToString();
                txtForTianJian.Text = ds.Tables[0].Rows[0]["forTianJian"].ToString();
            }
        }
    }
    #endregion

    #region 保存按钮事件
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string str_sjdept = "";//判断是否选择了上级,不允许为空
        string str_deptCode = txb_deptcode.Text.Trim();//部门编号
        if (txb_sjdept.Value.ToString().Trim() == "" && str_deptCode != "000001")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('上级部门不允许为空，请选择上级部门！');", true);
            return;
        }
        else if (txb_sjdept.Value.ToString().Trim() == "" && str_deptCode == "000001")
        {
            str_sjdept = "";
        }
        else
        {
            str_sjdept = txb_sjdept.Value.Substring(txb_sjdept.Value.IndexOf("[") + 1, txb_sjdept.Value.IndexOf("]") - 1);
        }
        string deptStatus = "1";
        if (this.RadioButton2.Checked == true)
        {
            deptStatus = "D";
        }
        string strIsGk = "N";
        if (this.isGkY.Checked == true)
        {
            strIsGk = "Y";
        }
        else
        {
            strIsGk = "N";
        }

        string striskzys = "Y";
        if (this.Niskzys.Checked == true)
        {
            striskzys = "N";
        }
        else
        {
            striskzys = "Y";
        }
        string strIsSell = "N";
        if (this.isIsSellY.Checked == true)
        {
            strIsSell = "Y";
        }
        else
        {
            strIsSell = "N";
        }
        string strJianma = this.txtJianma.Text.Trim();//部门简码
        string strforu8id = this.txtForU8id.Text.Trim();//对应u8id
        string strForTianJian = this.txtForTianJian.Text.Trim();//对应天健系统id
        string type = Page.Request.QueryString["type"].ToString().Trim();
        List<string> list = new List<string>();
        if (type == "add")
        {
            DataSet temp = server.GetDataSet("select deptCode from bill_departments where deptCode='" + this.txb_deptcode.Text.ToString().Trim() + "'");
            if (temp.Tables[0].Rows.Count != 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的部门已存在,请重新生成编号！');", true);
                this.btnAgain.Visible = true;
                return;
            }
            list.Add("insert into bill_departments (deptcode,deptname,sjdeptcode,deptstatus,Isgk,IsSell,deptJianma,forU8id,forTianJian,iskzys) values('" + this.txb_deptcode.Text.ToString().Trim() + "','" + this.txb_deptname.Text.ToString().Trim() + "','" + str_sjdept + "','" + deptStatus + "','" + strIsGk + "','" + strIsSell + "','" + strJianma + "','" + strforu8id + "','" + strForTianJian + "','" + striskzys + "') ");
        }
        else //编辑
        {
            //是否选了本单位或下级作为上级
            string xjDeptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", true);//获取所有下级单位
            xjDeptCodes = "'" + xjDeptCodes.Replace(",", "','") + "'";
            if (xjDeptCodes.IndexOf("'" + str_sjdept + "'") >= 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('不能选择本部门或下级 作为上级部门！');", true);
                return;
            }
            list.Add("update bill_departments set deptname='" + this.txb_deptname.Text.ToString().Trim() + "',sjdeptcode='" + str_sjdept + "',deptstatus='" + deptStatus + "',Isgk='" + strIsGk + "',IsSell='" + strIsSell + "',deptJianma='" + strJianma + "',forU8id='" + strforu8id + "',forTianJian='" + strForTianJian + "',iskzys='"+striskzys+"' where deptcode ='" + this.txb_deptcode.Text.ToString().Trim() + "'");
            if (deptStatus == "0")
            {
                string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", true);
                list.Add("update bill_departments set deptstatus='0' where deptCode in(" + deptCodes + ")");//注意如何更新下级！
            }
        }
        upDuiYing(this.txb_deptcode.Text.ToString().Trim(), strForTianJian);
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
        string deptCode = (new billCoding()).getDeptCode();
        if (deptCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成部门编号错误,请与开发商联系！');", true);
            this.btn_save.Visible = false;
        }
        else
        {
            this.txb_deptcode.Text = deptCode;
        }
    }
    /// <summary>
    /// 更新对应表
    /// </summary>
    private void upDuiYing(string strdeptcode, string strTianJianDept)
    {
        string[] tjs = strTianJianDept.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        List<string> lstsqls = new List<string>();
        lstsqls.Add("delete from sr_dept_tianjian where ysdeptcode='" + strdeptcode + "'");
        foreach (var tjdept in tjs)
        {
            lstsqls.Add("insert into sr_dept_tianjian(tianjiancode,ysdeptcode) values('" + tjdept + "','" + strdeptcode + "')");
        }
        server.ExecuteNonQuerysArray(lstsqls);
    }
}
