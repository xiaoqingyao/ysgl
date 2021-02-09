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
using System.Collections.Generic;

public partial class webBill_xtsz_phUserRightList : System.Web.UI.Page
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
        string sql = "select menuid,menuname,menuurl,menuorder,menusm,menustate from ph_sysmenu  where isnull(menustate,'0')!='D' order BY menuid";
        DataSet dt = server.GetDataSet(sql);
        this.GridView1.DataSource = dt.Tables[0];
        this.GridView1.DataBind();

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        List<string> list = new List<string>();
        list.Add("delete from ph_menuRight where objectID='" + Page.Request.QueryString["groupID"].ToString().Trim() + "' and rightType='2'");

        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox cbox = (CheckBox)row.FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                if (row.Cells[1].Text.Length > 2)
                {
                    list.Add("insert into ph_menuRight(menuid,objectid,rightType) values('" + row.Cells[1].Text + "','" + Page.Request.QueryString["groupID"].ToString().Trim() + "','2')");
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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (Page.Request.QueryString["groupID"] == null)
            {
            }
            else if (e.Row.Cells.Count == 0)
            {
            }
            else
            {
                int count = Convert.ToInt32(server.GetCellValue("select count(*) from ph_menuRight where menuid='" + e.Row.Cells[1].Text + "' and objectId='" + Request["groupID"] + "' and rightType='2'"));
                if (count > 0)
                {
                    CheckBox cbox = (CheckBox)e.Row.FindControl("CheckBox1");
                    cbox.Checked = true;
                }
                if (e.Row.Cells[1].Text.Length < 3)
                {
                    e.Row.Cells[2].Text = "&nbsp &nbsp " + e.Row.Cells[2].Text;
                    e.Row.Cells[2].Font.Bold = true;
                }
                if (e.Row.Cells[1].Text.Length > 2)
                {
                    e.Row.Cells[2].Text = "&nbsp &nbsp &nbsp &nbsp &nbsp " + e.Row.Cells[2].Text;
                }
            }
        }
    }

}