using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dal.NewsDictionary;
using Models;

public partial class message_messDetails : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    MsgDal dal = new MsgDal();
    Bill_Msg model = new Bill_Msg();
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
        string type = Page.Request.QueryString["type"].ToString().Trim();
        this.txtendtime.Attributes.Add("onfocus", "javascript:setday(this);");
        this.txtDate.Attributes.Add("onfocus", "javascript:setday(this);");
        if (type == "add")
        {
          
        //    this.CreateCwkmCode();
            //txtDate.Text = System.DateTime.Now.ToShortDateString();
            txtDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            this.txtendtime.Text = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
            txtWriter.Text = Session["userName"].ToString();
            txtReadTimes.Text = "0";
        }
        else {
            string code = Page.Request.QueryString["mCode"].ToString().Trim();
            DataSet temp = server.GetDataSet(" select title,contents,writer,Accessories,(select username from bill_users where usercode = writer)as ry,CONVERT(varchar(100),date, 23) as date  ,readtimes,mstype,notifiername,endtime  from bill_msg where id='" + code + "'  ");
            if (temp.Tables[0].Rows.Count == 1)
            {
                this.txtTitle.Text = temp.Tables[0].Rows[0]["title"].ToString();
                this.txtWriter.Text = temp.Tables[0].Rows[0]["ry"].ToString().Trim();
                this.txtDate.Text = temp.Tables[0].Rows[0]["date"].ToString().Trim();
                
                this.txtReadTimes.Text = temp.Tables[0].Rows[0]["readTimes"].ToString().Trim() == "" ? "0" : temp.Tables[0].Rows[0]["readTimes"].ToString().Trim();
                FCKeditor1.Value = temp.Tables[0].Rows[0]["contents"].ToString();
                this.txt_spr.Value = temp.Tables[0].Rows[0]["notifiername"].ToString();
                this.txtendtime.Text = temp.Tables[0].Rows[0]["endtime"].ToString();
                if (temp.Tables[0].Rows[0]["mstype"].ToString()=="通知")
                {
                    this.Drtype.SelectedIndex = 1;
                }
                else
                {
                    this.Drtype.SelectedIndex = 0;
                   

                }
                if (temp.Tables[0].Rows[0]["Accessories"].ToString() != null && temp.Tables[0].Rows[0]["Accessories"].ToString() != "")
                {
                    //this.TextBox3.Text = remitemode[0].Accessories.ToString();
                    string strfilename = this.HiddenField2.Value.ToString();
                    string strAppTemp = string.Format("<a href=\"../../" + temp.Tables[0].Rows[0]["Accessories"].ToString() + " \" target='_blank'>点击查看附件</a>");
                    this.lbfj.Text = strAppTemp;

                }
                else
                {
                    this.lbfj.Text = "";
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败,请与开发商联系！');", true);
                this.btn_save.Visible = false;
            }
        }
        if (this.Drtype.SelectedIndex == 0)
        {
            this.tdnamec.Visible = false;
            this.tdnmae.Visible = false;
          
        }
        else
        {
            this.tdnmae.Visible = true;
            this.tdnamec.Visible = true;
           
        }
        txtDate.ReadOnly = true;
        txtReadTimes.ReadOnly = true;
        txtWriter.ReadOnly = true;
    }



    public void CreateCwkmCode()
    {
        //string cwkmCode = (new billCoding()).getCwkmCode(Page.Request.QueryString["pCode"].ToString().Trim());
        //if (cwkmCode == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成科目编码错误,请与开发商联系！');", true);
        //    this.btn_save.Visible = false;
        //}
        //else
        //{
        //    this.txb_kmcode.Text = cwkmCode;
        //}
    }
    protected void btnAgain_Click(object sender, EventArgs e)
    {
        //this.CreateCwkmCode();
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string sql = "";
        string type = Page.Request.QueryString["type"].ToString().Trim();
        //判断是否是消息 如果是通知添加通知人 
        //通知
        string strAccessories = "";
        if (HiddenField2.Value.Trim() != null && HiddenField2.Value.Trim() != "")
        {
            strAccessories = HiddenField2.Value.Trim();

        }

         if (type == "add")
         {
             model = new Bill_Msg();
             model.Title = this.txtTitle.Text;
             model.Contents = FCKeditor1.Value;
             model.Writer = Session["userCode"].ToString();
             model.Date = DateTime.Parse(this.txtDate.Text);
             
             model.ReadTimes = "0";
             string strendtime = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
             model.Endtime = strendtime;
             model.Mstype = Drtype.SelectedItem.ToString();
             model.Accessories = strAccessories;
              if (Drtype.SelectedIndex==1)
                {
                  
                    model.Notifierid = "";
                    model.Notifiername = txt_spr.Value;
                  

                    //sql = @"insert into bill_msg(title,contents,writer,date,readtimes,mstype,notifierid,notifiername,endtime) values(";
                    //sql += @"'" + txtTitle.Text + "','" + FCKeditor1.Value + "','" + Session["userCode"].ToString() + "','" + txtDate.Text + "','0','" + Drtype.SelectedItem.ToString() + "','','" + txt_spr.Value + "','" + strendtime + "')";

                }else//消息
	            {
                    model.Notifierid = "";
                    model.Notifiername = "";
                         //sql = @"insert into bill_msg(title,contents,writer,date,readtimes,mstype,notifierid,notifiername,endtime) values(";
                         //sql += @"'" + txtTitle.Text + "','" + FCKeditor1.Value + "','" + Session["userCode"].ToString() + "','" + txtDate.Text + "','0','" + Drtype.SelectedItem.ToString() + "','','','" + strendtime + "')";

	            }
             int row= dal.InsertModel(model);
             if (row>0)
             {
                             ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);

             }
             else
             {
                 ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
             }
           
         }
         else
         {
             model = new Bill_Msg();
             string code = Page.Request.QueryString["mCode"].ToString().Trim();
             model.Title = txtTitle.Text;
             model.Contents = FCKeditor1.Value;
             model.Mstype = Drtype.SelectedItem.ToString();
             model.Endtime = txtendtime.Text;
             model.Accessories = strAccessories;
             if (Drtype.SelectedIndex==1)
             {

                 
                 model.Notifiername = txt_spr.Value;
                
               //  sql = @"update bill_msg set title='" + txtTitle.Text + "',contents='" + FCKeditor1.Value + "',mstype='" + Drtype.SelectedItem.ToString() + "',notifiername='" + txt_spr.Value + "',endtime='"+txtendtime.Text+"' where writer='" + Session["userCode"].ToString() + "' and id='" + code + "'";

             }
             else
             {
               
                 model.Notifiername = "";
               
               //  sql = @"update bill_msg set title='" + txtTitle.Text + "',contents='" + FCKeditor1.Value + "',mstype='" + Drtype.SelectedItem.ToString() + "',endtime='" + txtendtime.Text + "' where writer='" + Session["userCode"].ToString() + "' and id='" + code + "'";
             }
             int rowup = dal.updateModel(model, Session["userCode"].ToString(), code);
            if (rowup>0)
             {
                 ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);

             }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);

            }
         }
      
       
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
    }


    protected void change(object sender, EventArgs e)
    {
        if (this.Drtype.SelectedIndex==0)
        {
            this.tdnamec.Visible = false;
            this.tdnmae.Visible = false;
           

        }
        else
        {
            this.tdnmae.Visible = true;
            this.tdnamec.Visible = true;
           
        }
    }
}
