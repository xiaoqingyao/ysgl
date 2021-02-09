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
using Bll.UserProperty;
using System.Collections.Generic;

public partial class webBill_xtsz_jsczqxList2 : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                if (Page.Request.QueryString["groupID"].ToString().Trim() == "")
                {
                    this.Button1.Visible = false;
                    this.Label1.Text = "请选择相关角色后进行操作权限设置...";
                }
                else
                {
                    this.Label1.Text = "当前角色：[" + Page.Request.QueryString["groupID"].ToString().Trim() + "]" + server.GetCellValue("select groupname from bill_userGroup where groupID='" + Page.Request.QueryString["groupID"].ToString().Trim() + "'");
                }
                if (Page.Request.QueryString["groupID"].ToString().Trim() != "")
                {
                    this.Bind();
                }
            }
        }
    }


    //设置pnal的滚动条不懂动    勿删  留着
    private void HandlePanelScrolBar()
    {
        //    //定义两个HiddenField，分别纪录Panel的ScrollBar的X与Y位置
        //  HiddenField HF_ScrollPosX = new HiddenField();
        //  HiddenField HF_ScrollPosY = new HiddenField();

        //  HF_ScrollPosX.ID = "ScrollPosX";
        //  HF_ScrollPosY.ID = "ScrollPosY";
        //  form1.Controls.Add(HF_ScrollPosX);
        //  form1.Controls.Add(HF_ScrollPosY);

        //  //生成JS：将Panel的ScrollBar的X,Y位置设置给两个HiddenField
        //  string script;
        //  script = "window.document.getElementById('" + HF_ScrollPosX.ClientID + "').value = "
        //            + "window.document.getElementById('" + Panel1.ClientID + "').scrollLeft;"
        //            + "window.document.getElementById('" + HF_ScrollPosY.ClientID + "').value = "
        //            + "window.document.getElementById('" + Panel1.ClientID + "').scrollTop;";

        //  this.ClientScript.RegisterOnSubmitStatement(this.GetType(), "SavePanelScroll", script);

        //  if (IsPostBack) //如果是PostBack，将保存在HiddenField的ScrollBar的X,Y值重设回给Panel的ScrollBar
        //{
        //    script = "window.document.getElementById('" + Panel1.ClientID + "').scrollLeft = "
        //              + "window.document.getElementById('" + HF_ScrollPosX.ClientID + "').value;"
        //              + "window.document.getElementById('" + Panel1.ClientID + "').scrollTop = "
        //              + "window.document.getElementById('" + HF_ScrollPosY.ClientID + "').value;";

        //      this.ClientScript.RegisterStartupScript(this.GetType(), "SetPanelScroll", script, true);
        //}

    }




    private void Bind()
    {
        string sql = "select menuid,menuname,menuurl,menuorder,menusm,menustate from bill_sysmenu  where isnull(menustate,0)!='D' order BY menuid";
        DataSet dt = server.GetDataSet(sql);
        this.GridView1.DataSource = dt.Tables[0];
        this.GridView1.DataBind();

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        list.Add("delete from bill_userright where usercode='" + Page.Request.QueryString["groupID"].ToString().Trim() + "' and righttype='3'");

        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox cbox = (CheckBox)row.FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                if (row.Cells[1].Text.Length > 2)
                {
                    list.Add("insert into bill_userright(usercode,objectid,righttype) values('" + Page.Request.QueryString["groupID"].ToString().Trim() + "','" + row.Cells[1].Text + "','3')");
                }
            }
        }


        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
        }

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Pager)
        {

            if (Page.Request.QueryString["groupID"] == null)
            {

            }
            else
            {

                if (e.Row.Cells[1].Text.Length < 3)
                {
                    string sql1 = "select * from bill_userright where usercode='" + Page.Request.QueryString["groupID"].ToString().Trim() + "' and objectid like '" + e.Row.Cells[1].Text + "%'";
                    if (server.GetDataSet(sql1).Tables[0].Rows.Count > 0)
                    {
                        CheckBox cbox = (CheckBox)e.Row.FindControl("CheckBox1");
                        cbox.Checked = true;
                    }
                }
                string sql2 = "select usercode,objectid,rightType from bill_userright where bill_userright.usercode='" + Page.Request.QueryString["groupID"].ToString().Trim() + "'";
                DataSet ds = server.GetDataSet(sql2);
                DataTable dt1 = ds.Tables[0];

                foreach (GridViewRow row in GridView1.Rows)
                {
                    foreach (DataRow dtrows in dt1.Rows)
                    {
                        if (Convert.ToString(dtrows["objectid"]) == Convert.ToString(row.Cells[1].Text))
                        {
                            CheckBox cbox = (CheckBox)row.FindControl("CheckBox1");
                            cbox.Checked = true;
                            break;
                        }
                    }

                }


                if (e.Row.Cells[1].Text.Length < 3)
                {
                    string text = "&nbsp &nbsp " + e.Row.Cells[2].Text;
                    e.Row.Cells[2].Text = text;
                    e.Row.Cells[2].Font.Bold = true;
                }
                if (e.Row.Cells[1].Text.Length > 2)
                {
                    string text = "&nbsp &nbsp &nbsp &nbsp &nbsp " + e.Row.Cells[2].Text;
                    e.Row.Cells[2].Text = text;
                }
            }
        }
    }

}

