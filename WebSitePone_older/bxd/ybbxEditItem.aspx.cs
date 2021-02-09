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
using System.Data.SqlClient;
using System.Text;

public partial class bxd_ybbxEditItem : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable main;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Login.aspx','_self';", true);
            return;
        }
        string billCode = Request["billCode"];

        if (string.IsNullOrEmpty(billCode))
        {
            Response.Redirect("ybbxEditMain.aspx");

        }
        string mainStr = "";

        if (!string.IsNullOrEmpty(Request["type"]) && Request["type"] == "addmx")
        {
            btn_save.Visible = false;
            mainStr = "select a.billCode,a.billName,a.flowID,a.stepID,a.billUser,b.bxr,convert(varchar(10),a.billDate,121) as billDate,a.billDept,a.loopTimes,a.isgk,a.gkdept,b.bxmxlx,b.bxzy,b.bxsm  from bill_main a  inner join bill_ybbxmxb b on a.billCode=b.billCode and a.billCode=@billCode";
        }
        else
        {
            btn_saveAndMx.Value = "保存并添加明细";
            mainStr = "select * from ph_main where billCode=@billCode";

            // btn_saveAndMx.Visible=false;
        }

        main = server.GetDataTable(mainStr, new SqlParameter[] { new SqlParameter("@billCode", billCode) });
        //if (main.Rows.Count==0)
        //{
        //    main=server.GetDataTable("select * from ph_main where billCode='"+billCode+"'",null);
        //}
        //if(main.Rows.Count==0)
        //{
        //    main = server.GetDataTable("select a.billCode,a.billName,a.flowID,a.stepID,a.billUser,b.bxr,convert(varchar(10),a.billDate,121) as billDate,a.billDept,a.loopTimes,a.isgk,a.gkdept,b.bxmxlx,b.bxzy,b.bxsm  from bill_main a  inner join bill_ybbxmxb b on a.billCode=b.billCode and a.billCode='"+billCode+"'", null);
        //}
        Label1.Text = main.Rows[0]["billName"].ToString();
        if (main.Rows.Count == 0)
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            string sql = ""; ;
            if (Convert.ToString(main.Rows[0]["isgk"]) == "1")
            {
                sql += "select yskmCode,('['+yskmcode+']'+yskmMc)as yskmMc ,isnull(left(yskmCode,len(yskmCode)-2),'') as parentID,(case (select count(*) from bill_yskm where yskmcode like a.yskmcode+'%') when 1 then 1 else 0 end ) as islast  from Bill_Yskm a where a.yskmcode in (select yskmcode from bill_yskm_dept where deptcode='" + Convert.ToString(main.Rows[0]["gkDept"]) + "') and kmStatus='1' and dydj='02' and gkfy='1' ";
            }
            else
            {
                sql += "select yskmCode, ('['+yskmcode+']'+yskmMc)as yskmMc ,isnull(left(yskmCode,len(yskmCode)-2),'') as parentID ,(case (select count(*) from bill_yskm where yskmcode like a.yskmcode+'%') when 1 then 1 else 0 end ) as islast from Bill_Yskm a where a.yskmcode in (select yskmcode from bill_yskm_dept where deptcode='" + Convert.ToString(main.Rows[0]["billDept"]) + "') and kmStatus='1' and dydj='02' ";
            }
            if (!string.IsNullOrEmpty(Request["type"]) && Request["type"] == "addmx")
            {
            sql +=" and yskmCode not in(select  fykm from bill_main  a inner join bill_ybbxmxb_fykm  b on  a.billCode =b.billCode and a.billCode='"+billCode+"')";
            }
            DataTable dt = server.GetDataTable(sql, null);
            DropDownListHelp ddlHelper = new DropDownListHelp();
            ddlHelper.createDropDownTree(dt, "parentID", "", "yskmCode", "yskmMc", "yskmCode asc", DropDownList1);
            hfDept.Value = Convert.ToString(main.Rows[0]["billDept"]);
            hfIsgk.Value = Convert.ToString(main.Rows[0]["isgk"]);
            hfBillDate.Value = Convert.ToString(main.Rows[0]["billDate"]);
            hfDydj.Value = "02";
            hfBxlx.Value = Convert.ToString(main.Rows[0]["bxmxlx"]);
            DropDownList1.Items[0].Text = "请选择报销费用";
        }
    }

    private string GetDept()
    {
        DataSet ds = server.GetDataSet("select '['+deptCode+']'+deptName as deptName from bill_departments where deptStatus='1' ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["deptName"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    /*
 
      <%-- <select name="newsclass" id="newsclass">
                        <%this.ResponseTypeTree(); %>
                    </select>--%>
    DataView dv = new DataView();
    string str = "";
    public void ResponseTypeTree()
    {

        string sel = "";
        DataSet ds = new DataSet();
        ds = server.GetDataSet("select yskmCode,'['+yskmCode+']'+yskmMc as yskmMc from bill_yskm where yskmCode in ( select yskmCode from bill_yskm where kmStatus !='0' and  isnull(left(yskmCode,len(yskmCode)-2),'')='')");
        dv = server.GetDataSet("select yskmCode,'['+yskmCode+']'+yskmMc as yskmMc,left(yskmCode,len(yskmCode)-2) as parentCode from bill_yskm   where kmStatus !='0'").Tables[0].DefaultView;


        Response.Write("<option value=0 selected>选择类别</option>");



        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            Response.Write("<option value=" + ds.Tables[0].Rows[i]["yskmCode"].ToString() + ">├" + ds.Tables[0].Rows[i]["yskmMc"].ToString() + "</option>");
            this.GetChildType(ds.Tables[0].Rows[i]["yskmCode"].ToString(), "");
            if (str != "")
            {
                Response.Write(str);
                str = "";
            }
        }
        if (str != "")
        {
            Response.Write(str);
        }

    }
    private void GetChildType(string parentcode, string str1)
    {
        dv.RowFilter = "parentCode='" + parentcode + "'";
        dv.Sort = "yskmCode asc";
        int a = dv.Count;
        string imgstr = "";
        string sel = "";
        imgstr = str1 + " ";
        if (dv.Count == 0)
            return;
        for (int i = 0; i < a; i++)
        {

            str += "<option value=" + dv[i]["yskmCode"].ToString() + " >│" + imgstr + "└" + dv[i]["yskmMc"].ToString() + "</option> ";
            this.GetChildType(dv[i]["yskmCode"].ToString(), imgstr);
            dv.RowFilter = "parentCode='" + parentcode + "'";
            dv.Sort = "yskmCode asc";
        }
    }
*/

}
