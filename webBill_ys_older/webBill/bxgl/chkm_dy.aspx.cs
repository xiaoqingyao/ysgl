using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_bxgl_chkm_dy : BasePage
{
    string strNowDeptCode = "";
    string strNowDeptName = "";
    DataTable dtuserRightDept = new DataTable();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        ClientScript.RegisterArrayDeclaration("availablekm", getkm());
        if (!IsPostBack)
        {

            bindZhangTao();

            hfDept.Value = server.GetCellValue("select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
           
               
            //string strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");
            //string strsql = @"select deptCode,deptName from bill_departments where  sjdeptCode!='000001' and deptCode in (" + strDeptCodes + ")  order by deptcode";
            //dtuserRightDept = server.GetDataTable(strsql, null);

          
            //    //获取人员管理下的部门
            //    if (strDeptCodes != "")
            //    {
            //        if (dtuserRightDept.Rows.Count > 0)
            //        {
            //            for (int i = 0; i < dtuserRightDept.Rows.Count; i++)
            //            {
            //                ListItem li = new ListItem();
            //                li.Text = "[" + dtuserRightDept.Rows[i]["deptCode"].ToString().Trim() + "]" + dtuserRightDept.Rows[i]["deptName"].ToString().Trim();
            //                li.Value = dtuserRightDept.Rows[i]["deptCode"].ToString().Trim();
            //                this.ddl_dept.Items.Add(li);
            //            }

            //        }

            //    }

          
            bindData();
        }
    }
    private string getkm()
    {
        string script = "";
        DataSet ds = server.GetDataSet(@"select '['+yskmcode+']'+yskmmc showname from (
                                        select *,(select count(*) from bill_yskm where yskmcode like yskm.yskmcode+'%' and len(yskmcode)>len(yskm.yskmcode)) childcount from bill_yskm yskm  
                                        ) b where childcount=0 ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow item in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(item["showname"]));
            arry.Append("',");
        }
        if (!string.IsNullOrEmpty(arry.ToString()))
        {
            script = arry.ToString().Substring(0, arry.Length - 1);
        }

        return script;
    }

    private bool IsAdded(string outCode, string ysCode)
    {
        //string sql = "select count(*) from sr_kmdy where outcode='" + outCode + "' and yskmcode='" + ysCode + "' ";
        //int row = Convert.ToInt32(server.GetCellValue(sql));

        //if (row > 0)
        //    return true;
        //else
        return false;
    }

    protected void btnSaveAll_Click(object sender, EventArgs e)
    {
        string strufdata = ddlZhangTao.SelectedValue;

        //string strdeptcode = ddl_dept.SelectedValue;
        if (string.IsNullOrEmpty(strufdata))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择帐套');", true);
            return;
        }
        IList<string> sqlList = new List<string>();
        List<string> tempList = new List<string>();

        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            string chcode = myGrid.Items[i].Cells[0].Text.Trim();
            string chname = myGrid.Items[i].Cells[1].Text.Trim();
            string strdept = myGrid.Items[i].Cells[2].Text.Trim();
            TextBox tb = myGrid.Items[i].Cells[3].FindControl("txtYskm") as TextBox;
            string strKm = tb.Text.Trim();
            string yskmCode = CutVal(strKm);
            string yskmName = GetName(strKm);

            if (string.IsNullOrEmpty(strKm))
            {

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择对应科目。');", true);
                return;
            }

            sqlList.Add(@" update bill_yskmchdy set kmcode='" + yskmCode + "',kmname='" + yskmName + "' where chcode='" + chcode + "' and yslx='04' and  ufdata='" + strufdata + "' and note1='" + strdept + "'");// 
        }
        int row = server.ExecuteNonQuerysArray(sqlList.ToList());

        if (row > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功');", true);
            bindData();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败');", true);
            return;
        }
    }
    /// <summary>
    /// 绑定帐套
    /// </summary>
    private void bindZhangTao()
    {
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");

        string strselectsql = @"select dsname as db_data,iYear,
                        cast(cAcc_Num as varchar(50))+'|*|'+dsname as tval,
                        * from [{0}].UFTSystem.dbo.EAP_Account where iYear>='2014' order by iYear desc";
        strselectsql = string.Format(strselectsql, strlinkdbname);
        this.ddlZhangTao.DataSource = server.GetDataTable(strselectsql, null);
        this.ddlZhangTao.DataTextField = "companyname";
        this.ddlZhangTao.DataValueField = "db_data";
        this.ddlZhangTao.DataBind();
    }
    protected void OnddlZhangTao_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.bindData();
    }
    private void bindData()
    {

        string strufdata = ddlZhangTao.SelectedValue;
        string sql = @"select chcode,chname,note1,  case '['+kmcode+']'+kmname  when '[]' then '' else '['+kmcode+']'+kmname end  as   yskmcode  
                    from	 bill_yskmchdy where 1=1 ";

        string temp = ddlType.SelectedValue;
        if (!string.IsNullOrEmpty(strufdata))
        {
            sql += " and ufdata='" + strufdata + "'";
        }
        if (!string.IsNullOrEmpty(temp))
        {
            if (temp == "1")
            {
                sql += " and kmcode is not null";
            }
            else if (temp == "0")
            {
                sql += " and kmcode is  null";
            }
        }
      
        DataTable dt = server.GetDataTable(sql, null);
        myGrid.DataSource = dt;
        myGrid.DataBind();
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
    }

}