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

public partial class webBill_tjbb_CaiGouZiJinTongJi : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../webBill.aspx','_top');", true);
        }
        if (!IsPostBack)
        {
            DataTable dtRel = server.GetDataTable("select distinct bh,mc from  zqxserp_jk.dbo.info_dy_ven",null);
            ddlGongyingshang.DataSource = dtRel;
            ddlGongyingshang.DataTextField = "mc";
            ddlGongyingshang.DataValueField = "bh";
            ddlGongyingshang.DataBind();
            ddlGongyingshang.Items.Insert(0,new ListItem("所有供应商","所有供应商"));
            this.txtDateFrm.Attributes.Add("onfocus", "javascript:setday(this);");
            this.txtDateTo.Attributes.Add("onfocus", "javascript:setday(this);");
            DateTime dt = DateTime.Now;
            this.txtDateFrm.Text = dt.Date.Year.ToString() + "-" + dt.Date.Month.ToString() + "-" + "01";
            this.txtDateTo.Text = dt.ToString("yyyy-MM-dd");
        }
    }
    protected void Button1_Click(object sender, EventArgs e) {
        string strDateFrm = this.txtDateFrm.Text.Trim();
        string strDateTo = this.txtDateTo.Text.Trim();
        string strGongYingShang = "";
        string[] temp = hf_Gys.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        if (temp[0] != "所有供应商")
        {
            strGongYingShang = hf_Gys.Value;
        }else {
            int iCount = this.ddlGongyingshang.Items.Count;
            for (int i = 0; i < iCount; i++)
            {
                strGongYingShang += ddlGongyingshang.Items[i].Value;
                strGongYingShang += "|";
            }
            strGongYingShang.Substring(0, strGongYingShang.Length - 1);
        }
        strGongYingShang = strGongYingShang.Replace("|", "','");
        strGongYingShang = "'" + strGongYingShang + "'";
        
        if (temp[0] != "所有供应商" && temp.Length == 1) 
        {
            Response.Redirect(string.Format("../cgzj/cgzjbxList.aspx?datefrom={0}&dateto={1}&gysbh={2}", strDateFrm, strDateTo, temp[0]));
        }
        else {
            Server.Transfer(string.Format("CaiGouZiJinTongJiJieGuo.aspx?datefrom={0}&dateto={1}&gysbh={2}", strDateFrm, strDateTo, strGongYingShang));
        }
    }
}
