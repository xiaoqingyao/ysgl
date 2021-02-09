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
using System.Text;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public partial class webBill_select_dbSelectYskm : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterArrayDeclaration("availableTags", GetYskmAll());
        if (!IsPostBack)
        {
            BindYskm();
            string r = Request["r"];
            if (!string.IsNullOrEmpty(r))
            {
                string[] ret = r.Split(',');
                for (int i = 0; i < ret.Length; i++)
                {
                    for (int j = 0; j < rptYsgc.Items.Count; j++)
                    {
                        HiddenField hf = (HiddenField)rptYsgc.Items[j].FindControl("hfyskm");
                        if (ret[i] == hf.Value)
                        {
                            CheckBox check = (CheckBox)this.rptYsgc.Items[j].FindControl("ckYskm");
                            check.Checked = true;
                        }
                    }
                }
            }
        }
 
    }

    private string GetYskmAll()
    {
        DataSet ds = server.GetDataSet("select '['+yskmcode+']'+yskmMc as yskmMc  from bill_yskm  where  kmStatus ='1' ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["yskmMc"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        return script;
    }

    private void BindYskm()
    {
        string sql = "select * from (	select yskmcode,'['+yskmcode+']'+yskmMc as yskmMc,(select count(*) from bill_yskm where len(yskmcode)>len(a.yskmcode) and yskmcode like a.yskmcode+'%') as childcount from bill_yskm a where  kmStatus ='1') b where b.childcount=0 order by yskmcode";
        DataSet temp = server.GetDataSet(sql);
        rptYsgc.DataSource = temp;
        rptYsgc.DataBind();
    }
    
      protected void btn_select_Click(object sender, EventArgs e)
    {
        IList<MyClass2> list = new List<MyClass2>();
        for (int i = 0; i < rptYsgc.Items.Count; i++)
        {
            CheckBox cb = this.rptYsgc.Items[i].FindControl("ckYskm") as CheckBox;
            HiddenField hf = this.rptYsgc.Items[i].FindControl("hfYskm") as HiddenField;
            if (cb != null && cb.Checked && hf != null)
            {
                Literal ltl = this.rptYsgc.Items[i].FindControl("ltlYskm") as Literal;
                MyClass2 temp = new MyClass2();
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
}
    public class MyClass2
    {
        public string code { get; set; }
        public string name { get; set; }
    }


