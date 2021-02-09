using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Bll.UserProperty;
using Models;
using System.Collections.Generic;
using Bll;

public partial class webBill_ysglnew_BenefitproDetails : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    Dal.newysgl.Xmlr Xmlrdal = new Dal.newysgl.Xmlr();
    ConfigBLL bllConfig = new ConfigBLL();
    string strisY = "";
    string xmType = string.Empty;//项目填报类型  用于控制 预算类型下拉列表的数据源  如果传入了01 则下拉框只有01  如果没有传入参数 则显示所有的预算类型 add by lvcc
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            object objXmType = Request["xmtype"];
            if (objXmType != null)
            {
                xmType = objXmType.ToString();
            }

            if (!IsPostBack)
            {
                string selectndsql = @" select nian,xmmc from bill_ysgc where   yue='' and nian in (select distinct nd  from bill_SysConfig where configname = 'ystbfs' and configvalue='1' ) order by nian desc";
                DataTable selectdt = server.GetDataTable(selectndsql, null);
                drpNd.DataSource = selectdt;
                drpNd.DataTextField = "xmmc";
                drpNd.DataValueField = "nian";
                drpNd.DataBind();
                //List<string> ndlist = Xmlrdal.GetNdByxmLrb("1");
                //if (ndlist.Count > 0)
                //{
                //    drpNd.DataSource = ndlist;
                //    drpNd.DataBind();
                //}
                this.showData();
            }
        }
    }

    void showData()
    {

        DataTable dtbill = server.GetDataTable("select diccode,dicname from bill_datadic where dictype='18'", null);
        if (dtbill != null && dtbill.Rows.Count > 0)
        {
            this.ddl_yslx.DataSource = dtbill;
            this.ddl_yslx.DataTextField = "dicname";
            this.ddl_yslx.DataValueField = "diccode";
            this.ddl_yslx.DataBind();
            //this.ddl_yslx.Items.Insert(0, new ListItem("--请选择--", ""));
            if (!string.IsNullOrEmpty(xmType))
            {
                this.ddl_yslx.SelectedValue = xmType;
            }

        }


        //1.判断是不是简单界面

        strisY = bllConfig.GetValueByKey("Isjdxmszjm");
        if (!string.IsNullOrEmpty(strisY) && strisY == "Y")//如果是Isjdxmszjm
        {
            this.tr_xmbh.Visible = this.tr_xmmc.Visible = this.tr_nd.Visible = this.tr_xmzt.Visible = true;
            this.tr_jsfs.Visible = tr_pxh.Visible = tr_zjr.Visible = tr_zjsj.Visible = tr_xgr.Visible = tr_xgsj.Visible = this.tr_yslx.Visible = tr_lufs.Visible = false;
            this.lbl_mc.Text = "收&nbsp;入&nbsp;项&nbsp;目&nbsp;档&nbsp;案";
        }
        else
        {
            this.tr_xmbh.Visible = this.tr_xmmc.Visible = this.tr_nd.Visible = this.tr_xmzt.Visible = this.tr_yslx.Visible = this.tr_jsfs.Visible = tr_lufs.Visible = tr_pxh.Visible = tr_zjr.Visible = tr_zjsj.Visible = tr_xgr.Visible = tr_xgsj.Visible = tr_lufs.Visible = true;
            this.lbl_mc.Text = "利&nbsp;润&nbsp;项&nbsp;目&nbsp;档&nbsp;案";
        }

        string type = Page.Request.QueryString["type"].ToString().Trim();
        if (type == "add")
        {
            //this.getYskmCode();
            string stradduser = server.GetCellValue("select '['+userCode+']'+userName from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
            this.adduser.Text = stradduser;
            this.adddate.Text = DateTime.Now.ToShortDateString();
        }
        else
        {
            string code = Page.Request.QueryString["pCode"].ToString().Trim();
            DataSet temp = server.GetDataSet(@"select procode,proname,calculatype,fillintype,sortcode,status,
(select '['+userCode+']'+userName from bill_users where userCode=a.adduser)as adduser,CONVERT(varchar(10),adddate,121) 
as adddate,(select '['+userCode+']'+userName from bill_users where userCode=a.modifyuser)as
modifyuser,CONVERT(varchar(10),modifydate,121) as modifydate,annual,yslb from bill_ys_benefitpro a   where procode='" + code + "'");
            if (temp.Tables[0].Rows.Count == 1)
            {
                this.txb_kmcode.Text = temp.Tables[0].Rows[0]["procode"].ToString().Trim();
                this.txb_kmmc.Value = temp.Tables[0].Rows[0]["proname"].ToString().Trim();
                this.DropDownList1.SelectedValue = temp.Tables[0].Rows[0]["calculatype"].ToString().Trim();
                this.DropDownList2.SelectedValue = temp.Tables[0].Rows[0]["Status"].ToString().Trim();
                this.DropDownList3.SelectedValue = temp.Tables[0].Rows[0]["fillintype"].ToString().Trim();
                this.ddl_yslx.SelectedValue = temp.Tables[0].Rows[0]["yslb"].ToString().Trim();
                this.sortcode.Value = temp.Tables[0].Rows[0]["sortcode"].ToString().Trim();
                this.adduser.Text = temp.Tables[0].Rows[0]["adduser"].ToString().Trim();
                this.adddate.Text = temp.Tables[0].Rows[0]["adddate"].ToString().Trim();
                this.modifyuser.Text = temp.Tables[0].Rows[0]["modifyuser"].ToString().Trim();
                this.modifydate.Text = temp.Tables[0].Rows[0]["modifydate"].ToString().Trim();
                //this.txt_annual.Text = temp.Tables[0].Rows[0]["annual"].ToString().Trim();
                if (!string.IsNullOrEmpty(temp.Tables[0].Rows[0]["annual"].ToString().Trim()))
                {
                    drpNd.SelectedValue = temp.Tables[0].Rows[0]["annual"].ToString().Trim();
                }
                this.txb_kmcode.Attributes.Add("ReadOnly", "ReadOnly");
                this.drpNd.Attributes.Add("ReadOnly", "ReadOnly");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败,请管理员联系！');", true);
                this.btn_save.Visible = false;
            }
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string sql = "";
        string type = Page.Request.QueryString["type"].ToString().Trim();
        string procode = this.txb_kmcode.Text.ToString().Trim();
        if (procode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该项目的编号出现错误，请重新操作！');", true);
            return;
        }
        if (type == "add")
        {
            DataSet temp = server.GetDataSet("select * from bill_ys_benefitpro where procode='" + procode + "'");

            if (temp.Tables[0].Rows.Count != 0)
            {
                //this.getYskmCode();
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的项目已存在,请检查后添加！');", true);
                //this.btnAgain.Visible = true;
                return;
            }
            string straddusername = "";
            if (this.adduser.Text.Trim() != "")
            {
                straddusername = this.adduser.Text.Trim();
                straddusername = straddusername.Substring(1, straddusername.IndexOf("]") - 1).Trim();
            }
            sql = "insert into bill_ys_benefitpro(procode,proname,fillintype,status,annual,yslb) values('" + procode + "','" + this.txb_kmmc.Value.ToString().Trim() + "','明细汇总','" + this.DropDownList2.SelectedItem.Value + "','" + drpNd.SelectedValue + "','" + ddl_yslx.SelectedValue + "')";
            sql = "insert into bill_ys_benefitpro(procode,proname,calculatype,fillintype,sortcode,status,adduser,adddate,annual,yslb) values('" + procode + "','" + this.txb_kmmc.Value.ToString().Trim() + "','" + this.DropDownList1.SelectedItem.Value + "','" + this.DropDownList3.SelectedItem.Value + "','" + this.sortcode.Value.ToString().Trim() + "','" + this.DropDownList2.SelectedItem.Value + "','" + straddusername + "','" + DateTime.Now.ToShortDateString() + "','" + drpNd.SelectedValue + "','" + ddl_yslx.SelectedValue + "')";
        }
        else
        {

            //sql = "update bill_ys_benefitpro set proname='" + this.txb_kmmc.Value.ToString().Trim() + "',status='" + this.DropDownList2.SelectedItem.Value + "',annual='" + drpNd.SelectedValue + "' where procode='" + procode + "'";
            sql = "update bill_ys_benefitpro set proname='" + this.txb_kmmc.Value.ToString().Trim() + "',calculatype='" + this.DropDownList1.SelectedItem.Value + "',fillintype='" + this.DropDownList3.SelectedItem.Value + "',sortcode='" + this.sortcode.Value.ToString().Trim() + "',status='" + this.DropDownList2.SelectedItem.Value + "',modifyuser='" + Session["userCode"].ToString().Trim() + "',modifydate='" + DateTime.Now.ToShortDateString() + "',annual='" + drpNd.SelectedValue + "', yslb='" + ddl_yslx.SelectedValue + "' where procode='" + procode + "'";
        }

        if (server.ExecuteNonQuery(sql) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');window.returnValue=\"sucess\";self.close();", true);
        }
        else
        {
            if (type == "edit")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";window.close();", true);
            }
            else
            {
                //this.showData();
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";", true);
                this.txb_kmmc.Value = "";
                this.sortcode.Value = "";
            }
        }
    }
}
