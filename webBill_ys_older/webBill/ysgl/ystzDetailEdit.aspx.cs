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
using Ajax;

public partial class webBill_ysgl_ystzDetailEdit : System.Web.UI.Page
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
            Ajax.Utility.RegisterTypeForAjax(typeof(webBill_ysgl_ystzDetailEdit));
            if (!IsPostBack)
            {
                this.bindData();
            }
        }
    }

    public void bindData()
    {
        DataSet temp = server.GetDataSet("select a.sCode,b.yskmCode as yskm,b.yskmbm as kmbm,b.yskmmc as kmmc,a.sJe as je,a.sJe_Before as jeBefore from bill_ystz a,bill_yskm b where b.yskmCode=a.skm and a.billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        this.myGrid1.DataSource = temp;
        this.myGrid1.DataBind();
        this.Label3.Text = temp.Tables[0].Rows[0]["sCode"].ToString().Trim();
        this.Label1.Text = server.GetCellValue("select xmmc from bill_ysgc where gcbh='" + temp.Tables[0].Rows[0]["sCode"].ToString().Trim() + "'");

        temp = server.GetDataSet("select a.tCode,b.yskmCode as yskm,b.yskmbm as kmbm,b.yskmmc as kmmc,a.tJe as je,a.tJe_Before as jeBefore from bill_ystz a,bill_yskm b where b.yskmCode=a.tkm and a.billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        this.myGrid2.DataSource = temp;
        this.myGrid2.DataBind();
        this.Label4.Text = temp.Tables[0].Rows[0]["tCode"].ToString().Trim();
        this.Label2.Text = server.GetCellValue("select xmmc from bill_ysgc where gcbh='" + temp.Tables[0].Rows[0]["tCode"].ToString().Trim() + "'");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string deptCode = server.GetCellValue("select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
        string sCode = this.Label3.Text.ToString().Trim();
        string tCode = this.Label4.Text.ToString().Trim();
        string guid = (new GuidHelper()).getNewGuid();

        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        list.Add("delete from bill_main where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        list.Add("delete from bill_ystz where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        for (int i = 0; i <= this.myGrid1.Items.Count - 1; i++)
        {
            string sKm = this.myGrid1.Items[i].Cells[0].Text.ToString().Trim();
            string sJe = ((TextBox)this.myGrid1.Items[i].FindControl("TextBox2")).Text.ToString().Trim();
            string sJe_Old = this.myGrid1.Items[i].Cells[4].Text.ToString().Trim();
            string tKm = this.myGrid2.Items[i].Cells[0].Text.ToString().Trim();
            string tJe = ((System.Web.UI.HtmlControls.HtmlInputText)this.myGrid2.Items[i].FindControl("txtJe")).Value.ToString().Trim();
            string tJe_Old = this.myGrid2.Items[i].Cells[4].Text.ToString().Trim();
            list.Add("insert into bill_ystz values('" + guid + "','" + deptCode + "','" + sCode + "','" + sKm + "','" + sJe + "','" + sJe_Old + "','" + tCode + "','" + tKm + "','" + tJe + "','" + tJe_Old + "')");
        }
        //写入预算总表 记录
        list.Add("insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType) values('" + guid + "','预算调整','ystz','-1','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString() + "','" + deptCode + "',0,'0','1')");


        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('ystz.aspx','_self');", true);
        }
    }

    public void bindDataGrid()
    {

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        this.bindDataGrid();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("ystz.aspx");
    }

    [Ajax.AjaxMethod(HttpSessionStateRequirement.Read)]
    public string[] getCalResult(string currentCode, int[] arrIndex, string[] arrCode, string[] arrVal)
    {
        string[] returnVal = new string[arrIndex.Length];
        int len = currentCode.Length;
        while (len >= 4)
        {
            double dValue = 0;
            for (int i = 0; i <= arrIndex.Length - 1; i++)
            {
                if (arrCode[i].Length == len && arrCode[i].Substring(0, len - 2) == currentCode.Substring(0, len - 2))//同级编号
                {
                    dValue += double.Parse(arrVal[i]);
                }
            }
            //找到上级并赋值
            for (int i = 0; i <= arrIndex.Length - 1; i++)
            {
                string cCode = currentCode.Substring(0, len - 2);
                if (arrCode[i] == cCode)//找到上级
                {
                    arrVal[i] = dValue.ToString();
                }
            }
            len = len - 2;
        }

        for (int i = 0; i <= arrIndex.Length - 1; i++)
        {
            returnVal[i] = arrIndex[i].ToString().Trim() + "," + double.Parse(arrVal[i].ToString().Trim()).ToString("0.00");
        }
        return returnVal;
    }

    [Ajax.AjaxMethod(HttpSessionStateRequirement.Read)]
    public string CalNormal(string sOld, string sBef, string tOld)
    {
        try
        {
            double res = double.Parse(tOld) + double.Parse(sBef) - double.Parse(sOld);
            return res.ToString("0.00");
        }
        catch
        {
            return "0.00";
        }
    }
}
