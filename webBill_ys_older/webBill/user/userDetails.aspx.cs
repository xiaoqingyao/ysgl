using System;
using System.Collections.Generic; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Bll.UserProperty;
using Models;
using Bll;
using Dal.SysDictionary;

public partial class user_userDetails : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string stroderpws = "";
    my_fzl.bindClss bindCl = new my_fzl.bindClss(); 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            btn_seldept.Attributes.Add("onclick", "javascript:selectDept('../select/deptFrame.aspx');return false;");
            if (!IsPostBack)
            {
                DataSet temp = server.GetDataSet("select groupid,'['+convert(varchar(6), groupid)+']'+groupname as groupname from bill_usergroup  order by groupid");
                this.ddl_group.DataTextField = "groupName";
                this.ddl_group.DataValueField = "groupID";
                this.ddl_group.DataSource = temp;
                this.ddl_group.DataBind();
                BindData();
            }
        }
    }


    #region 绑定数据
    private void BindData()
    {
        string type = Page.Request.QueryString["type"].ToString().Trim();
        if (type == "add")
        {
            if (Page.Request.QueryString["deptCode"].ToString().Trim() != "")
            {
                SysManager sysMgr = new SysManager();
                this.txtDept.Value = sysMgr.GetDeptCodeName(Page.Request.QueryString["deptCode"].ToString().Trim());
            }
            this.rdoIsSystem0.Checked = true;
            this.rdoUserStatus1.Checked = true;
            this.CreateUserCode();
        }
        else{
            this.txb_usercode.ReadOnly = true;
            //修改
            UserMessage user = new UserMessage(Page.Request.QueryString["userCode"].ToString().Trim());
            SysManager sysMgr = new SysManager();
            txb_usercode.Text = user.Users.UserCode;
            txb_username.Text = user.Users.UserName;
            this.txtDept.Value = sysMgr.GetDeptCodeName(user.Users.UserDept);
            ddl_group.SelectedValue = user.Users.UserGroup;
            
            if (user.Users.UserStatus == "1")
            {
                this.rdoUserStatus1.Checked = true;
            }
            else {
                this.rdoUserStatus2.Checked = true;
            }
            if (user.Users.IsSystem == "1")
            {
                this.rdoIsSystem1.Checked = true;
            }
            else {
                this.rdoIsSystem0.Checked = true;
            }
            //职务
            Bill_DataDic modelDataDic = new DataDicDal().GetDicByTypeCode("05", user.Users.UserPosition);
            if (modelDataDic!=null)
            {
                this.txtPosition.Value = "[" + modelDataDic.DicCode + "]" + modelDataDic.DicName;
            }
        }
    }        
    #endregion

    #region 保存
    protected void btn_save_Click(object sender, EventArgs e)
    {
        Bill_Users user = new Bill_Users();
        user.UserCode = txb_usercode.Text;
        user.UserName = txb_username.Text;
        user.UserGroup = this.ddl_group.SelectedValue;
       

        string userDept = this.txtDept.Value.ToString().Trim();
        if (userDept != "")
        {
            user.UserDept = userDept.Split(']')[0].Trim('[');
        }
        else
        {
            user.UserDept = userDept;
        }

        
        if (this.rdoIsSystem1.Checked == true)
        {
            user.IsSystem = "1";
        }
        else
        {
            user.IsSystem = "0";
        }

        if (this.rdoUserStatus2.Checked == true)
        {
            user.UserStatus = "0";
        }
        else
        {
            user.UserStatus = "1";
        }
        //职务
        user.UserPosition = new PublicServiceBLL().SubCode(this.txtPosition.Value.Trim());
        string type = Page.Request.QueryString["type"].ToString().Trim();
        UserMessage mgr = new UserMessage(user);
        if (type == "add")
        {
            user.UserPwd = "123456";//txb_usercode.Text;
            if (mgr.CheckUserCode())
            {
                mgr.InserUser();
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！用户初始密码为:123456！');window.returnValue=\"sucess\";self.close();", true);
            }
            else
            {
                this.CreateUserCode();
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的人员已存在,系统已重新生成,请保存！');", true);
                this.btnAgain.Visible = true;
                return;
            }
        }
        else {
         
            UserMessage userpws = new UserMessage(Page.Request.QueryString["userCode"].ToString().Trim());

            user.UserPwd = userpws.Users.UserPwd; ;
            mgr.InserUser();
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }

      
    }
    #endregion



    void CreateUserCode()
    {
        string userCode = (new billCoding()).getUserCode();
        if (userCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成人员编号错误,请与开发商联系！');", true);
            this.btn_save.Visible = false;
        }
        else
        {
            this.txb_usercode.Text = userCode;
        }
    }
    protected void btnAgain_Click(object sender, EventArgs e)
    {
        this.CreateUserCode();
    }
}
