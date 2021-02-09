using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_xtsz_ysbh : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    string strNowDeptCode = "";
    string strNowDeptName = "";
    string stryskmtype = "";//预算类型  01收入 02费用 ……
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {

            string usercode = Session["userCode"].ToString().Trim();

            strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");


            string strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");

            strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");


            if (!IsPostBack)
            {
                dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where   deptCode in (" + strDeptCodes + ")  order by deptcode", null);//and deptCode not in (" + strNowDeptCode + ")



                #region 绑定人员管理下的部门
                if (!strNowDeptCode.Equals(""))
                {
                    //获取人员管理下的部门
                    if (strDeptCodes != "")
                    {
                        if (dtuserRightDept.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtuserRightDept.Rows.Count; i++)
                            {
                                ListItem li = new ListItem();
                                li.Text = "[" + dtuserRightDept.Rows[i]["deptCode"].ToString().Trim() + "]" + dtuserRightDept.Rows[i]["deptName"].ToString().Trim();
                                li.Value = dtuserRightDept.Rows[i]["deptCode"].ToString().Trim();
                                this.drp_dept.Items.Add(li);
                            }

                        }

                    }

                }

                #endregion


                //string strxmsql = @"select * from bill_xm";


            }

        }

    }
    /// <summary>
    /// 审批驳回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btn_bh_Click(object sender, EventArgs e)
    {
        string strdeptcode = drp_dept.SelectedValue;
        List<string> listsql = new List<string>();
        string stryslx = drp_yslx.SelectedValue;

        if (!string.IsNullOrEmpty(strdeptcode))
        {
            listsql.Add(@"delete from workflowrecords where recordid in (	
                select recordid from workflowrecord where billcode in (	select billcode from bill_main where flowid='" + stryslx + "'  and billdept in ('" + strdeptcode + "')))");
            listsql.Add(@" delete from workflowrecord where billcode in (select billcode from bill_main where flowid='" + stryslx + "'and billdept in ('" + strdeptcode + "'))");
            listsql.Add(@" update bill_main set stepid='-1' where flowid='" + stryslx + "' and billdept in ('" + strdeptcode + "')");
            int introw = 0;
            if (listsql.Count() > 0)
            {
                introw = server.ExecuteNonQuerys(listsql.ToArray());
            }

            if (introw > 0)
            {
                Response.Write("<script>alert('驳回成功');</script>");
            }
            else
            {
                Response.Write("<script>alert('驳回失败，请联系系统管理员。');</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('请检查是否选择了部门或者预算类型');</script>");
            return;

        }

    }
    /// <summary>
    /// 审批通过
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_tg_Click(object sender, EventArgs e)
    {
        string strdeptcode = drp_dept.SelectedValue;
        List<string> listsql = new List<string>();
        string stryslx = drp_yslx.SelectedValue;

        if (!string.IsNullOrEmpty(strdeptcode) && !string.IsNullOrEmpty(stryslx))
        {
            listsql.Add(@" update bill_main set stepid='end' where flowid='" + stryslx + "' and billdept in ('" + strdeptcode + "')");
            listsql.Add(@"delete from workflowrecords where recordid in (	
                select recordid from workflowrecord where billcode in (	select billcode from bill_main where flowid='" + stryslx + "'  and billdept in ('" + strdeptcode + "')))");
            listsql.Add(@" delete from workflowrecord where billcode in (select billcode from bill_main where flowid='" + stryslx + "'and billdept in ('" + strdeptcode + "'))");
            int introw = 0;
            if (listsql.Count() > 0)
            {
                introw = server.ExecuteNonQuerys(listsql.ToArray());
            }

            if (introw > 0)
            {
                Response.Write("<script>alert('审核成功');</script>");
            }
            else
            {
                Response.Write("<script>alert('审核失败，请联系系统管理员。');</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('请检查是否选择了部门或者预算类型。');</script>");
            return;

        }

    }
    protected void btn_js_tongguo_Click(object sender, EventArgs e)
    {
        caoZuoJsBill("tg");
    }
    protected void btn_js_bohui_Click(object sender, EventArgs e)
    {
        caoZuoJsBill("bh");
    }
    private void caoZuoJsBill(string flg)
    {
        string billname = this.txtBillName.Text.Trim();
        if (billname == "")
        {
            Response.Write("<script>alert('请输入单据编号');</script>"); return;
        }
        //检查单据是否存在
        string sql = "select count(*) from bill_main where billname='" + billname + "' and flowid in ('yksq_dz','ybbx')";
        string count = server.GetCellValue(sql);
        if (count == "0")
        {
            Response.Write("<script>alert('该单据不存在，请仔细检查单号是否正确。（该操作只适用于用款申请单和费用报销单）');</script>"); return;
        }
        //执行
        if (flg == "tg")
        {
            sql = "update bill_main set stepid='end' where billname='" + billname + "'  and flowid in ('yksq_dz','ybbx'); delete from workflowrecord where billcode='" + billname + "'";
        }
        else {
            sql = "update bill_main set stepid='-1' where billname='" + billname + "'  and flowid in ('yksq_dz','ybbx'); delete from workflowrecord where billcode='" + billname + "' or billcode in (select billcode from bill_main where billname='"+billname+"')";
        }
        //Response.Write(sql);
        server.ExecuteNonQuery(sql);
        Response.Write("<script>alert('操作成功');</script>");
    }
}