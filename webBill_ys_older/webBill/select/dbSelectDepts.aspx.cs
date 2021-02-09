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
using System.Web.Script.Serialization;
public partial class webBill_select_dbSelectDepts : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string showflg = "2";//2代表只显示2级部门  all代表显示所有的
    protected void Page_Load(object sender, EventArgs e)
    {
        object objshowflg = Request["showflg"];
        if (objshowflg != null)
        {
            showflg = objshowflg.ToString();
        }
        if (!IsPostBack)
        {
            BindDept();
            string r = Request["r"];
            if (!string.IsNullOrEmpty(r))
            {
                string[] ret = r.Split(',');
                for (int i = 0; i < ret.Length; i++)
                {
                    for (int j = 0; j < rptYsgc.Items.Count; j++)
                    {
                        HiddenField hf = (HiddenField)rptYsgc.Items[j].FindControl("hfDept");
                        if (ret[i] == hf.Value)
                        {
                            CheckBox check = (CheckBox)this.rptYsgc.Items[j].FindControl("ckDept");
                            check.Checked = true;
                        }
                    }
                }
            }
        }
    }

    private void BindDept()
    {
        string sql = "select deptCode,('['+deptCode+']'+deptName) as deptName  from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') and deptcode!='000001' ";
        if (showflg.Equals("all"))
        {
            sql = "select deptCode,('['+deptCode+']'+deptName) as deptName  from bill_departments where 1=1 and deptcode!='000001' ";
        }
        if (!string.IsNullOrEmpty(txt_search.Text.Trim()))
        {
            sql += "  and ( deptCode like '%" + txt_search.Text.Trim() + "%' or deptName like '%" + txt_search.Text.Trim() + "%') ";
        }
        sql += " order by deptcode ";
        DataSet temp = server.GetDataSet(sql);
        rptYsgc.DataSource = temp;
        rptYsgc.DataBind();
    }

    protected void btn_select_Click(object sender, EventArgs e)
    {
        IList<MyClass> list = new List<MyClass>();
        for (int i = 0; i < rptYsgc.Items.Count; i++)
        {
            CheckBox cb = this.rptYsgc.Items[i].FindControl("ckDept") as CheckBox;
            HiddenField hf = this.rptYsgc.Items[i].FindControl("hfDept") as HiddenField;
            if (cb != null && cb.Checked && hf != null)
            {
                Literal ltl = this.rptYsgc.Items[i].FindControl("ltlDept") as Literal;
                MyClass temp = new MyClass();
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

    protected void btn_searc_Click(object sender, EventArgs e)
    {
        BindDept();
    }

    protected void rptYsgc_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
             e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string hfDept = ((HiddenField)e.Item.FindControl("hfDept")).Value;
            for (int i = 0; i < hfDept.Length - 2; i++)
            {
                string kongge = ((Label)e.Item.FindControl("kongge")).Text;
                ((Label)e.Item.FindControl("kongge")).Text = "&nbsp;&nbsp;" + kongge;
            }
        }

    }
}

public class MyClass
{
    public string code { get; set; }
    public string name { get; set; }
}
