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
using WorkFlowLibrary.WorkFlowDal;
using Models;
using System.Collections.Generic;

public partial class SaleBill_SaleConfig_SaleConfigDail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    Models.T_ControlItemMode conmode = new T_ControlItemMode();
    Dal.SaleProcess.ControlItemDal conitemdal = new Dal.SaleProcess.ControlItemDal();
  
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                this.showData();
            }
        }
    }

    void showData()
    {
        string type = Page.Request.QueryString["ctrl"].ToString().Trim();
       Dal.SaleProcess.SaleProcessDal salep = new Dal.SaleProcess.SaleProcessDal();
            IList<SaleProcessMode> configlist =salep.GetAllList1();

            //this.DropDownList1.DataSource = configlist;
            //DropDownList1.DataTextField = "PName";
            //DropDownList1.DataValueField = "Code";
            //DropDownList1.DataBind();
            //DropDownList1.Items.Add(new ListItem("请选择", ""));
            //DropDownList1.SelectedValue = "";

            DropDownList2.DataSource = configlist;
            DropDownList2.DataTextField = "PName";
            DropDownList2.DataValueField = "Code";
            DropDownList2.DataBind();
            DropDownList2.Items.Add(new ListItem("请选择",""));
            DropDownList2.SelectedValue = "";
            this.lblkzbg.Text = "生产入库";
           
        if (type == "add")
        {
            
        }
        if (type=="edit")
        {
            string code = Page.Request.QueryString["Code"].ToString().Trim();
            DataSet temp = server.GetDataSet(" select (case Status when '1' then '正常' when '0' then '禁用' end) as status, CName,ControlCodeFirst,ControlNameFirst,ControlCodeSecond,ControlNameSecond,Months,Remark from T_ControlItem where Code='" + code + "'  ");
          
            if (temp.Tables[0].Rows.Count == 1)
            {
                this.txtTitle.Text = temp.Tables[0].Rows[0]["CName"].ToString();

                if (temp.Tables[0].Rows[0]["status"].ToString() == "正常")
                {
                    this.redY.Checked = true;
                    this.redN.Checked = false;
                }
                else
                {
                    this.redN.Checked = true;
                    this.redY.Checked = false;
                }
                //this.DropDownList1.SelectedValue = temp.Tables[0].Rows[0]["ControlCodeFirst"].ToString();
                this.DropDownList2.SelectedValue = temp.Tables[0].Rows[0]["ControlCodeSecond"].ToString();
                this.txtRemark.Text = temp.Tables[0].Rows[0]["Remark"].ToString();
                this.DropDownList3.SelectedIndex = Convert.ToInt32(temp.Tables[0].Rows[0]["Months"].ToString())-1;

               
            }
           
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败,请与开发商联系！');", true);
                this.btn_save.Visible = false;
            }
        }
        if (type == "view")
        {
            string code = Page.Request.QueryString["Code"].ToString().Trim();
            DataSet temp = server.GetDataSet(" select (case Status when '1' then '正常' when '0' then '禁用' end) as status, CName,ControlCodeFirst,ControlNameFirst,ControlCodeSecond,ControlNameSecond,Months,Remark from T_ControlItem where Code='" + code + "'  ");
            if (temp.Tables[0].Rows.Count == 1)
            {
                this.txtTitle.Text = temp.Tables[0].Rows[0]["CName"].ToString();
                this.txtTitle.Enabled = false;
                this.redN.Enabled = false;
                this.redY.Enabled = false;
                //this.DropDownList1.Enabled = false;
                this.DropDownList2.Enabled = false;
                this.DropDownList3.Enabled = false;
                this.txtRemark.Enabled = false;
                this.btn_save.Visible = false;
                this.tbmsg.Visible = false;
                if (temp.Tables[0].Rows[0]["status"].ToString() == "正常")
                {
                    this.redY.Checked = true;
                    this.redN.Checked = false;
                }
                else
                {
                    this.redN.Checked = true;
                    this.redY.Checked = false;
                    
                }
                //this.DropDownList1.SelectedValue = temp.Tables[0].Rows[0]["ControlCodeFirst"].ToString();
                this.DropDownList2.SelectedValue = temp.Tables[0].Rows[0]["ControlCodeSecond"].ToString();
                this.txtRemark.Text = temp.Tables[0].Rows[0]["Remark"].ToString();
                this.DropDownList3.SelectedIndex = Convert.ToInt32(temp.Tables[0].Rows[0]["Months"].ToString()) - 1;


            }

            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败,请与开发商联系！');", true);
                this.btn_save.Visible = false;
            }
        }
       
    }



    public void CreateCwkmCode()
    {
       
    }
    protected void btnAgain_Click(object sender, EventArgs e)
    {
        //this.CreateCwkmCode();
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {

        string type = Page.Request.QueryString["ctrl"].ToString().Trim();
        

        if (type == "add")
        {
            Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            string strneed = DateTime.Now.ToString("yyyyMMdd");

            string strcode = pusbll.GetBillCode("CF", strneed, 1, 6);



            try
            {
                conmode.Code = strcode;
                if (this.txtTitle.Text!=null&&this.txtTitle.Text!="")
                {
                    conmode.CName = this.txtTitle.Text;
                }
                else
                {
                    lblMsg.Text = "名称不能为空";
                    return;
                }
                conmode.ControlCodeFirst = "";
                conmode.ControlNameFirst = this.lblkzbg.Text.Trim();

                //if (this.DropDownList1.SelectedValue.ToString()!=null&&this.DropDownList1.SelectedValue.ToString()!="")
                //{
                //    conmode.ControlCodeFirst = this.DropDownList1.SelectedValue.ToString();
                //}
                //else
                //{
                //    lblMsg.Text = "请选择控制点1";
                //    return;
                //}
                //if (this.DropDownList1.SelectedItem.ToString()!=null&&this.DropDownList1.SelectedItem.ToString()!="")
                //{
                //    conmode.ControlNameFirst = this.DropDownList1.SelectedItem.ToString();
                //}
                //else
                //{
                //    lblMsg.Text = "请选择控制点1";
                //    return;
                //}
                if (this.DropDownList2.SelectedValue.ToString()!=null&&this.DropDownList2.SelectedValue.ToString()!="")
                {
                   
                        conmode.ControlCodeSecond = this.DropDownList2.SelectedValue.ToString();
                   
                   
                }
                else
                {
                    lblMsg.Text = "请选择控制点2";
                    return;
                }
                if (this.DropDownList2.SelectedItem.ToString()!=null&&this.DropDownList2.SelectedItem.ToString()!="")
                {
                   
                        conmode.ControlNameSecond = this.DropDownList2.SelectedItem.ToString();
                }
                else
                {
                    lblMsg.Text = "请选择控制点2";
                    return;
                }
              
         
                if (this.redY.Checked)
                {
                    conmode.Status = "1";
                }
                else
                {
                    conmode.Status = "0";
                }
                    conmode.Months = this.DropDownList3.SelectedItem.ToString();
                    conmode.Remark = txtRemark.Text;
                    conitemdal.Add(conmode);
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);

               

            }
            catch (Exception ex)
            {
                
            }
        }
        else
        {
            conmode.Code =  Page.Request.QueryString["Code"].ToString().Trim();
            conmode.CName = this.txtTitle.Text;
            if (this.txtTitle.Text != null && this.txtTitle.Text != "")
            {
                conmode.CName = this.txtTitle.Text;
            }
            else
            {
                lblMsg.Text = "名称不能为空";
                return;
            }
            conmode.ControlCodeFirst = "";
            conmode.ControlNameFirst = this.lblkzbg.Text.Trim();
            conmode.ControlCodeSecond = this.DropDownList2.SelectedValue.ToString();
            conmode.ControlNameSecond = this.DropDownList2.SelectedItem.ToString();
            if (this.redY.Checked)
            {
                conmode.Status = "1";
            }
            else
            {
                conmode.Status = "0";
            }
            
            conmode.Months =this.DropDownList3.SelectedItem.ToString();
            conmode.Remark = txtRemark.Text;

            if (conitemdal.Updetesalep(conmode) > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);

            }

            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('修改失败！');window.returnValue=\"sucess\";self.close();", true);
            }
        }


       
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
    }


   
}
