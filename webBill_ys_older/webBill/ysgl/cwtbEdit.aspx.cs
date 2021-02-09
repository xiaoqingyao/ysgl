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
using System.Collections.Generic;
using Ajax;
using Bll.UserProperty;
using System.Drawing;

public partial class webBill_ysgl_cwtbEdit : System.Web.UI.Page
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
            Ajax.Utility.RegisterTypeForAjax(typeof(webBill_ysgl_cwtbEdit));
            if (!IsPostBack)
            {
                this.lblBillCode.Text = Page.Request.QueryString["billCode"].ToString().Trim();
                DataSet temp = server.GetDataSet("select * from bill_main where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
                this.Label1.Text = "预算过程：" + server.GetCellValue("select xmmc from bill_ysgc where gcbh='" + temp.Tables[0].Rows[0]["billName"].ToString().Trim() + "'") + " 填报单位：" + server.GetCellValue("select deptname from bill_departments where deptcode='" + temp.Tables[0].Rows[0]["billDept"].ToString().Trim() + "'");

                this.bindData();
            }
        }
    }

    void bindData()
    {
        DataSet temp = server.GetDataSet("select * from bill_main where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        //去年对应过程编号
        DataSet gcInfo = server.GetDataSet("select * from bill_ysgc where gcbh='" + temp.Tables[0].Rows[0]["billName"].ToString().Trim() + "'");
        string qnGcbh = server.GetCellValue("select gcbh from bill_ysgc where nian='" + (int.Parse(gcInfo.Tables[0].Rows[0]["nian"].ToString()) - 1) + "' and yue='" + gcInfo.Tables[0].Rows[0]["yue"].ToString() + "' and ystype='" + gcInfo.Tables[0].Rows[0]["ystype"].ToString() + "'");
        DataSet temp1 = server.GetDataSet("exec bill_pro_cwtb_yskm_edit '" + temp.Tables[0].Rows[0]["billDept"].ToString().Trim() + "','" + temp.Tables[0].Rows[0]["billName"].ToString().Trim() + "','" + qnGcbh + "','02'");

        (new ysHistory()).bindHistory(this.myGrid, temp1, temp.Tables[0].Rows[0]["billDept"].ToString().Trim(), temp.Tables[0].Rows[0]["billName"].ToString().Trim()); 
        
        this.myGrid.DataSource = temp1;
        this.myGrid.DataBind();

        //检测科目是否末级，如果是，把单元格置为不可用
        SysManager sysmanager = new SysManager();
        for (int i = 0; i < this.myGrid.Items.Count; i++)
        {
            string kmcode = this.myGrid.Items[i].Cells[0].Text.ToString().Trim();
            if (sysmanager.GetYskmIsmj(kmcode) != "0")
            {

                ((TextBox)this.myGrid.Items[i].FindControl("TextBox2")).Enabled = false;
                ((TextBox)this.myGrid.Items[i].FindControl("TextBox1")).Enabled = false;
                ((HtmlInputButton)this.myGrid.Items[i].FindControl("btnUpLoad")).Disabled = true;
                myGrid.Items[i].BackColor = Color.Silver;
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataSet temp = server.GetDataSet("select * from bill_main where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        string gcbh = temp.Tables[0].Rows[0]["billName"].ToString().Trim();
        string deptCode = temp.Tables[0].Rows[0]["billDept"].ToString().Trim();
        List<string> list = new List<string>();
        string billCode = server.GetCellValue("select billCode from bill_main where flowID='ys' and billDept='" + deptCode + "' and billName='" + gcbh+ "'");


        //2012-04-06 mxl 检测预算金额是否超出上级金额
       for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            string kmcode = this.myGrid.Items[i].Cells[0].Text.ToString().Trim();
            decimal je = 0;
            try
            {
                TextBox txt = (TextBox)this.myGrid.Items[i].FindControl("TextBox2");
                je = Convert.ToDecimal(txt.Text.ToString().Trim());
            }
            catch { };

            //string gcbh2 = Page.Request.QueryString["gcbh"].ToString().Trim();
            string deptcode = Page.Request.QueryString["deptCode"].ToString().Trim();
            decimal[] yysje = new BudgetManager().GetYsMaxJe(gcbh, deptcode, kmcode);

            if (yysje[0] != -1)
            {
                decimal tempje = 0;
                decimal.TryParse(this.myGrid.Items[i].Cells[16].Text,out tempje);
                if (yysje[0] < yysje[1] + je - tempje)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('科目[" + kmcode + "]填报预算金额大于上级预算金额！');", true);
                    return;
                }
            }
        }
        //
 
        
        if (billCode == "")
        {
            billCode = (new GuidHelper()).getNewGuid();
            list.Add("insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType) values('" + billCode + "','" + gcbh + "','ys','-1','','" + System.DateTime.Now.ToString() + "','" + deptCode + "',0,1,1)");
        }
        else
        {
            //清楚原来的数据
            list.Add("delete from bill_ysmxb where billCode='" + billCode + "' and yskm in (select yskmCode from bill_yskm where tblx='02')");
        }

        



        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            string yskm = this.myGrid.Items[i].Cells[0].Text.ToString().Trim();
            double je = 0;
            string sm = "";
            try
            {
                TextBox txt = (TextBox)this.myGrid.Items[i].FindControl("TextBox2");
                TextBox txt2 = (TextBox)this.myGrid.Items[i].FindControl("TextBox1");
                je = double.Parse(txt.Text.ToString().Trim());
                sm = txt2.Text.ToString().Trim();
            }
            catch { }
            list.Add("insert into bill_ysmxb values('" + gcbh + "','" + billCode + "','" + yskm + "'," + je.ToString("0.00") + ",'" + deptCode + "','1')");
            list.Add("update bill_ysmxb_smfj set sm='" + sm + "' where billCode='" + billCode + "' and yskm='" + yskm + "'");

        }
        //更新主表的总额
        list.Add("update bill_main set billJe=(select isnull(sum(isnull(ysje,0)),0) from bill_ysmxb where billcode='" + billCode + "') where billCode='" + billCode + "'");

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
            this.bindData();
            //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('cwtbList.aspx?deptCode=" + deptCode + "','_self');", true);
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("cwtbList.aspx?deptCode=" + Page.Request.QueryString["deptCode"].ToString().Trim());
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header)
        {
            DataSet temp = server.GetDataSet("select * from bill_ysmxb_smfj where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and yskm='" + e.Item.Cells[1].Text.ToString().Trim() + "'");
            if (temp.Tables[0].Rows.Count == 0)
            { }
            else
            {
                TextBox txt = (TextBox)e.Item.FindControl("TextBox1");
                txt.Text = temp.Tables[0].Rows[0]["sm"].ToString().Trim();

                if (temp.Tables[0].Rows[0]["fj"].ToString().Trim() == "")
                {
                }
                else
                {
                    string tempStr = "&nbsp;<a href=# onclick=\"deleteFj(this);\" >删 除</a>&nbsp;&nbsp;";
                    tempStr += "<a href=\"files/" + temp.Tables[0].Rows[0]["fj"].ToString().Trim() + "\" target=_blank>下 载</a>&nbsp;";
                    e.Item.Cells[8].Text = tempStr;
                }
            }
        }
    }
    [Ajax.AjaxMethod()]
    public string getFj(string billCode, string yskm)
    {
        DataSet temp = server.GetDataSet("select * from bill_ysmxb_smfj where billCode='" + billCode + "' and yskm='" + yskm + "'");
        if (temp.Tables[0].Rows.Count == 0||temp.Tables[0].Rows[0]["fj"].ToString().Trim()=="")
        {
            return "";
        }
        else
        {
            string tempStr = "&nbsp;<a href=# onclick=\"deleteFj(this);\" >删 除</a>&nbsp;&nbsp;";
            tempStr += "<a href=\"files/" + temp.Tables[0].Rows[0]["fj"].ToString().Trim() + "\" target=_blank>下 载</a>&nbsp;";

            return tempStr;
        }
    }

    [Ajax.AjaxMethod()]
    public bool deleteFj(string billCode, string yskm)
    {
        if (server.ExecuteNonQuery("update bill_ysmxb_smfj set fj='' where billCode='" + billCode + "' and yskm='" + yskm + "'") == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
