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

public partial class BillTravelApply_travelApplyView : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Login.aspx','_self';", true);
            return;
        }
        if (string.IsNullOrEmpty(Request["billCode"]))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Index.aspx','_self';", true);
            return;
        }
        if (!IsPostBack)
        {
            BindModel();
        }
    }

    private void BindModel()
    {
        string code = Convert.ToString(Request["billCode"]);
        DataTable dt = server.GetDataTable("select distinct  maincode,typecode,arrdess,travelDate,reasion,travelplan,needAmount,transport,moreThanStandard,reportCode,jiaotongfei,zhusufei,yewuzhaodaifei,huiyifei,yinshuafei,qitafei,sendDept,b.billCode as billCode,b.billName as billName, convert(varchar(10),b.billdate,121) as billDate,b.billDept as billDept ,b.billje as billje ,b.billuser from bill_travelApplication a, bill_main b where a.mainCode=b.billCode and a.mainCode='" + code + "'", null);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            lb_bm.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + ObjectToStr(dr["sendDept"]) + "'");
            lb_billDate.Text = Convert.ToDateTime(ObjectToStr(dr["billDate"])).ToString("yyyy-MM-dd");
            lb_billUser.Text = server.GetCellValue("select '['+usercode+']'+userName from bill_users where usercode='" + ObjectToStr(dr["billuser"]) + "'");
            lb_travelDate.Text = ObjectToStr(dr["travelDate"]);
            string persons = "";
            DataTable pdt=server.GetDataTable("select travelPersionCode  as p from bill_travelApplication where mainCode='"+code+"' ",null);
            if (pdt.Rows.Count>0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<h5>出差人<h5>");
                sb.Append("<table class='tab-hs' style='color: Black; font-family: 微软雅黑;'>");
                for (int i = 0; i < pdt.Rows.Count; i++)
                {
                    DataTable itemdt = server.GetDataTable("select '['+usercode+']'+userName as userCode,(select '['+deptCode+']'+deptName from bill_departments where deptCode=userDept) as deptCode  from bill_users where usercode='" + pdt.Rows[i]["p"].ToString() + "'", null);
                    //sb.Append("<tr><td  class='tdborder'>" + itemdt.Rows[0]["userCode"] + ":&nbsp;&nbsp;&nbsp;" + itemdt.Rows[0]["deptCode"] + "</td><td  class='tdborder'></tr>");
                    sb.Append("<tr><td  class='tdborder' style='min-width:120px'>" +itemdt.Rows[0]["userCode"]  + "</td><td  class='tdborder'>" +  itemdt.Rows[0]["deptCode"]+ "</td><td  class='tdborder'></tr>");
                }
                sb.Append("</table>");
                persons = sb.ToString();             
            }
            chr.InnerHtml = persons;
            
            lb_address.Text = ObjectToStr(dr["arrdess"]);
            lb_reasion.Text = ObjectToStr(dr["reasion"]);
            lb_plan.Text = ObjectToStr(dr["travelplan"]);
            lb_zje.Text = NullToNUm(dr["billje"]);
            lb_jtf.Text =NullToNUm(dr["jiaotongfei"]);
            lb_zsf.Text = NullToNUm(dr["zhusufei"]);
            lb_zdf.Text = NullToNUm(dr["yewuzhaodaifei"]);
            lb_hyf.Text = NullToNUm(dr["huiyifei"]);
            lb_ysf.Text = NullToNUm(dr["yinshuafei"]);
            lb_qt.Text = NullToNUm(dr["qitafei"]);
            lb_jtgj.Text = ObjectToStr(dr["transport"]);
            lb_isbz.Text = ObjectToStr(dr["moreThanStandard"])=="0"?"否":"是";
        }

        string type = Request["type"];
        if (!string.IsNullOrEmpty(type))
        {
            if (type == "View")
            {
                aduittr.Visible = false;
                btn_audit.Visible = false;
                btn_cancel.Visible = false;
                //判断是否已提交
                DataTable dt1 = server.GetDataTable("select * from workflowrecord where billCode='" + code + "'", null);
                if (dt1.Rows.Count > 0)
                {
                    btn_submit.Visible = false;
                    btn_delete.Visible = false;
                    if (dt1.Rows[0]["rdState"].ToString() == "3")
                    {
                        btn_revoke.Visible = true;
                    }
                }


            }
            if (type == "audit")
            {
                btn_submit.Visible = false;
                btn_delete.Visible = false;
                aduittr.Visible = true;
            }

        }
    }


    private string NullToNUm(object obj)
    {
        string num = ObjectToStr(obj);
        if (string.IsNullOrEmpty(num))
        {
            return "0.00";
        }
        else
            return Convert.ToDecimal(num).ToString("N02");
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
}
