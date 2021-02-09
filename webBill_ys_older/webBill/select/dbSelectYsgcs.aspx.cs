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
using System.Web.Script.Serialization;
using System.Collections.Generic;

public partial class webBill_select_dbSelectYsgcs : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDDL();
            BindYsgc();
            string r = Request["r"];
            if (!string.IsNullOrEmpty(r))
            {
                string[] ret = r.Split(',');
                for (int i = 0; i < ret.Length; i++)
                {
                    for (int j = 0; j < rptYsgc.Items.Count; j++)
                    {
                        HiddenField hf = (HiddenField)rptYsgc.Items[j].FindControl("hfYsgc");
                        if (ret[i] == hf.Value)
                        {
                            CheckBox check = (CheckBox)this.rptYsgc.Items[j].FindControl("ckYsgc");
                            check.Checked = true;
                        }
                    }
                }
            }
        }
    }

    private void BindDDL()
    {
        string sql = "select distinct nian from bill_ysgc order by nian desc ";
        ddlNd.DataSource = server.GetDataSet(sql);
        ddlNd.DataTextField = "nian";
        ddlNd.DataValueField = "nian";
        ddlNd.DataBind();
        //ddlNd.Items.Insert(0,new ListItem("全部",""));
        ddlNd.Items.Add(new ListItem("全部",""));

    }
    private void BindYsgc()
    {
        string sql = "select gcbh,replace(xmmc,'预算过程','') as xmmc from bill_ysgc  where 1=1 and ysType !='0' ";
        if (!string.IsNullOrEmpty(ddlNd.SelectedValue))
        {
             sql+=" and nian='"+ddlNd.SelectedValue+"'";
        }
        sql += "order by nian desc , gcbh asc";
        DataSet temp = server.GetDataSet(sql);
        rptYsgc.DataSource = temp;
        rptYsgc.DataBind();
    }

    protected void btn_select_Click(object sender, EventArgs e)
    {
        IList<MyClass1> list = new List<MyClass1>();
        for (int i = 0; i < rptYsgc.Items.Count; i++)
        {
            CheckBox cb = this.rptYsgc.Items[i].FindControl("ckYsgc") as CheckBox;
            HiddenField hf = this.rptYsgc.Items[i].FindControl("hfYsgc") as HiddenField;
            if (cb != null && cb.Checked && hf != null)
            {
                Literal ltl = this.rptYsgc.Items[i].FindControl("ltlYsgc") as Literal;
                MyClass1 temp = new MyClass1();
                temp.code = hf.Value;
                temp.name = ltl.Text.Trim();
                list.Add(temp);
            }
        }
        
        if (list != null)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            string strEnd = jserializer.Serialize(list);
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>window.returnValue='" + strEnd + "'; self.close();</script>");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>self.close();</script>");
        }
    }
    protected void ddlNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindYsgc();
    }
}
    public class MyClass1
    {
        public string code { get; set; }
        public string name { get; set; }
    }
