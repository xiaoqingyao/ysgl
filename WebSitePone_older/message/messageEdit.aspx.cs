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
using Dal.NewsDictionary;
using Models;

public partial class message_messageEdit : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Login.aspx','_self';", true);
            return;
        }
        if (string.IsNullOrEmpty(Request["type"]))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Index.aspx','_self';", true);
            return;
        }
        if (!IsPostBack)
        {
            if (Request["type"] == "add")
            {
                btnDelete.Visible = false;
                txt_writer.Text = Session["userName"].ToString();
                txt_addTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                txt_endTime.Text = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
            }
            else if (Request["type"] == "edit" && !string.IsNullOrEmpty(Request["id"]))
            {
                BindData();
            }
        }
    }

    private void BindData()
    {
        string id = Request["id"];
        DataTable dt = server.GetDataTable("select * from bill_msg where id=" + Request["id"], null);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txt_title.Text = ObjectToStr(dr["title"]);
            txt_writer.Text = ObjectToStr(dr["writer"]);
            lbType.Text=ObjectToStr(dr["mstype"]);
            //ddlType.SelectedValue = ObjectToStr(dr["mstype"]);
            txt_addTime.Text = Convert.ToDateTime(dr["date"]).ToString("yyyy-MM-dd");
            txt_endTime.Text = Convert.ToDateTime(dr["endtime"]).ToString("yyyy-MM-dd");
            txt_content.Text = ObjectToStr(dr["contents"]);
            hffj.Value = ObjectToStr(dr["Accessories"]);
            hftzr.Value = ObjectToStr(dr["notifiername"]);
            txt_addTime.ReadOnly=false;
            txt_addTime.BackColor = System.Drawing.Color.FromName("#D9D9D9");

        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        MsgDal dal = new MsgDal();
        Bill_Msg model = new Bill_Msg();
        if (Request["type"] == "add")
        {
            model.Title = txt_title.Text;
            model.Contents = txt_content.Text;
            model.Writer = Session["userCode"].ToString();
            //beg发布时间
            if (this.txt_addTime.Text.Trim() != "")
            {
                DateTime date = DateTime.MinValue;
                bool flag = DateTime.TryParse(this.txt_addTime.Text.ToString(), out date);
                if (flag)
                {
                    model.Date = date;
                }
                else
                {
                    Response.Write("<script>alert('日期格式错误！');</script>");
                    return;
                }
            }
            else
            {
                Response.Write("<script>alert('请填写发布日期！');</script>");
                txt_addTime.Focus();
                return;
            }
           //end发布时间

            //beg截止时间
            if (this.txt_endTime.Text.Trim() != "")
            {
                DateTime date = DateTime.MinValue;
                bool flag = DateTime.TryParse(this.txt_endTime.Text.ToString(), out date);
                if (flag)
                {
                    model.Endtime = date.ToString();
                }
                else
                {
                    Response.Write("<script>alert('日期格式错误！');</script>");;
                    return;
                }
            }
            else
            {
                Response.Write("<script>alert('请填写有效期限！');</script>");
                txt_endTime.Focus();
                return;
            }
            //end截止时间
            
            
          
            model.ReadTimes = "0";
            model.Mstype = lbType.Text.Trim();//ddlType.SelectedValue;
            model.Accessories = "";
            model.Notifierid = "";
            model.Notifiername = "";

            int row = dal.InsertModel(model);
            if (row > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.location.href='messageList.aspx';", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            }
        }
        else
        {
            model.Title = txt_title.Text;
            model.Contents = txt_content.Text;
            model.Mstype = lbType.Text.Trim();//ddlType.SelectedValue;
            //beg截止时间
            if (this.txt_endTime.Text.Trim() != "")
            {
                DateTime date = DateTime.MinValue;
                bool flag = DateTime.TryParse(this.txt_endTime.Text.ToString(), out date);
                if (flag)
                {
                    model.Endtime = date.ToString();
                }
                else
                {
                    Response.Write("<script>alert('日期格式错误！');</script>");
                    return;
                }
            }
            else
            {
                Response.Write("<script>alert('请填写有效期限！');</script>");
                txt_endTime.Focus();
                return;
            }
            //end截止时间
            
            model.Notifiername = hftzr.Value;
            model.Accessories = hffj.Value;
            int row = dal.updateModel(model, Session["userCode"].ToString(), Request["id"]);
            if (row > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.location.href='messageList.aspx';", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            }
        }
    }

    private string ObjectToStr(object obj)
    {
        if (obj == null || Convert.ToString(obj) == string.Empty)
        {
            return "";
        }
        else
        {
            return obj.ToString();
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
       string sql="delete from bill_Msg where id="+Request["id"];
       int row=server.ExecuteNonQuery(sql);
       if (row > 0)
       {
           ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');window.location.href='messageList.aspx';", true);

       }
       else
       {
           ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
       }
    }
}
