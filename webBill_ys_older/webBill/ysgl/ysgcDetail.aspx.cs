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

public partial class ysgl_ysgcDetail : System.Web.UI.Page
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
                this.showData();
            }
        }
    }

    void showData()
    {
        string type = Page.Request.QueryString["type"].ToString().Trim();
        if (type == "add")
        {
            this.txtGcbh.Text = (new billCoding()).getYsgcCode();
        }
        else if (type == "edit")
        {
            DataSet temp = server.GetDataSet("select * from bill_ysgc where gcbh='" + Page.Request.QueryString["gcbh"].ToString().Trim() + "'");

            if (temp.Tables[0].Rows.Count == 0)//没有获取到数据
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败！');", true);
            }
            else
            {
                this.txtGcbh.ReadOnly = true;
                this.txtNian.ReadOnly = true;
                this.drpYueFen.Enabled = false;
                this.drpJd.Enabled = false;
                this.drp_ysType.Enabled = false;
                try
                {
                    this.txtGcbh.Text = temp.Tables[0].Rows[0]["gcbh"].ToString().Trim();
                    this.txtXmmc.Text = temp.Tables[0].Rows[0]["xmmc"].ToString().Trim();
                    this.txtKssj.Value = DateTime.Parse(temp.Tables[0].Rows[0]["kssj"].ToString().Trim()).ToShortDateString();
                    this.txtJzsj.Value = DateTime.Parse(temp.Tables[0].Rows[0]["jzsj"].ToString().Trim()).ToShortDateString();
                    this.DropDownList1.SelectedValue = temp.Tables[0].Rows[0]["status"].ToString().Trim();
                    this.txtNian.Text = temp.Tables[0].Rows[0]["nian"].ToString().Trim();
                    this.drp_ysType.SelectedValue = temp.Tables[0].Rows[0]["ysType"].ToString().Trim();
                    if (temp.Tables[0].Rows[0]["ysType"].ToString().Trim() == "0")//年度
                    {
                        this.ysTypeDiv.Style["display"] = "none";
                        this.ysLxDiv.Style["display"] = "none";
                    }
                    else
                    {
                        this.ysLxDiv.Style["display"] = "";
                        this.ysTypeDiv.Style["display"] = "";
                        if (temp.Tables[0].Rows[0]["ysType"].ToString().Trim() == "1")
                        {
                            this.drpJd.Visible = true;
                            this.drpYueFen.Visible = false;
                            this.drpJd.SelectedValue = temp.Tables[0].Rows[0]["yue"].ToString().Trim();
                        }
                        else
                        {
                            this.drpJd.Visible = false;
                            this.drpYueFen.Visible = true;
                            this.drpYueFen.SelectedValue = temp.Tables[0].Rows[0]["yue"].ToString().Trim();

                        }
                    }
                }
                catch
                {
                    //展示数据出错
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败！');", true);
                }
            }
        }
    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        string ysType = this.drp_ysType.SelectedItem.Value;
        string nian = this.txtNian.Text.ToString().Trim();
        string yue = "";
        if (ysType == "0")//年度预算
        {
           yue = "";
        }
        else if (ysType == "1")//季度预算
        {
            yue = this.drpJd.SelectedItem.Value;
        }
        else {
            yue = this.drpYueFen.SelectedItem.Value;
        }

        string type = Page.Request.QueryString["type"].ToString().Trim();
        if (type == "add")
        {
            DataSet temp = server.GetDataSet("select count(1) from bill_ysgc where gcbh='" + this.txtGcbh.Text.ToString().Trim() + "'");
            if (temp.Tables[0].Rows[0][0].ToString().Trim() != "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('过程编号已存在！');", true);
                return;
            }
            //是否存在预算过程
            temp = server.GetDataSet("select * from bill_ysgc where ysType='" + ysType + "' and nian='" + nian + "' and yue='" + yue + "'");
            if (temp.Tables[0].Rows.Count!=0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('对应时间的预算过程已存在！');", true);
                return;
            }


            string sql = "insert into bill_ysgc(gcbh,xmmc,kssj,jzsj,status,fqr,fqsj,nian,yue,ysType)";
            sql += " values('" + this.txtGcbh.Text.ToString().Trim() + "','" + this.txtXmmc.Text.ToString().Trim() + "','" + this.txtKssj.Value.ToString().Trim() + "','" + this.txtJzsj.Value.ToString().Trim() + "','" + this.DropDownList1.SelectedItem.Value + "','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString() + "','" + nian + "','" + yue + "','" + ysType + "')";

            if (server.ExecuteNonQuery(sql) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('新建过程失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('新建过程完成！');window.returnValue=\"sucess\";self.close();", true);
            }
        }
        else
        {
            //是否存在预算过程
            string sql = "update bill_ysgc  ";
            sql += "  set xmmc='" + this.txtXmmc.Text.ToString().Trim() + "',kssj='" + this.txtKssj.Value.ToString().Trim() + "',jzsj='" + this.txtJzsj.Value.ToString().Trim() + "',status='" + this.DropDownList1.SelectedItem.Value + "' where gcbh='" + Page.Request.QueryString["gcbh"].ToString().Trim() + "' ";

            if (server.ExecuteNonQuery(sql) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('修改过程失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('修改过程完成！');window.returnValue=\"sucess\";self.close();", true);
            }
        }
    }
    protected void btn_Cancle_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),"","window.returnValue=\"\";self.close();",true);
    }
    protected void drp_ysType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drp_ysType.SelectedIndex == 0)
        {
            this.ysTypeDiv.Style["display"] = "none";
        }
        else
        {
            this.ysTypeDiv.Style["display"] = "";
            if (this.drp_ysType.SelectedIndex == 1)//季度
            {
                this.drpJd.Visible = true;
                this.drpYueFen.Visible = false;
            }
            else
            {
                this.drpJd.Visible = false;
                this.drpYueFen.Visible = true;
            }
        }
    }
}
