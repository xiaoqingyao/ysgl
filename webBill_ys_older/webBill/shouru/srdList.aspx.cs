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
using WorkFlowLibrary.WorkFlowBll;
using Bll.UserProperty;
using Bll;
using System.IO;

public partial class webBill_shouru_srdList : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strDeptCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            object objDeptCode = Request["deptcode"];
            if (objDeptCode != null)
            {
                strDeptCode = objDeptCode.ToString();
            }
            if (!IsPostBack)
            {
                //this.txtDateFrm.Attributes.Add("onfocus", "javascript:setday(this);");
                //this.txtDateTo.Attributes.Add("onfocus", "javascript:setday(this);");
                //this.Button1.Attributes.Add("onclick", "javascript:selectry('../select/userFrame.aspx');return false;");
                this.BindDataGrid();
            }
        }
        ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        ClientScript.RegisterArrayDeclaration("subjectTags", GetYSSubjectAll(strDeptCode));
    }

    void BindDataGrid()
    {
        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 120);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>
        DataTable dtrel = GetData(arrpage[0], arrpage[1], out icount);
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount == 0 ? 1 : icount;
        //----------给gridview赋值
        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();
        //string sql = getSelectSql();
        //DataSet temp = server.GetDataSet(sql);


        //if (temp.Tables[0].Rows.Count == 0)
        //{
        //    temp = null;
        //}
        //this.myGrid.DataSource = temp;
        //this.myGrid.DataBind();
    }
    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string sql = @"select Row_Number()over(order by billdate) as crow ,*,(select bxzy from bill_ybbxmxb where bill_ybbxmxb.billCode=amain.billCode) as bxzy,
            (select username from bill_users where usercode=amain.billuser) as billUser1,
            (select (case isnull(sfgf,'0') when '0' then '未给付' when '1' then '已给付' end) as sfgf from bill_ybbxmxb where billcode=amain.billcode) as sfgf,
            (select ybje from bill_ybbxmxb where billcode=amain.billcode)as gfje,
            (select (select username from bill_users where usercode=gfr) as gfr from bill_ybbxmxb where billcode=amain.billcode) as gfr,
            (select gfsj from bill_ybbxmxb where billcode=amain.billcode) as gfsj,
            (select cxyy from bill_ybbxmxb where billcode=amain.billcode) as cxyy,
            (select (select username from bill_users where usercode=cxr) as cxr from bill_ybbxmxb  where billcode=amain.billcode) as cxr,
            (select cxsj from bill_ybbxmxb where billcode=amain.billcode) as cxsj,
            (select pzcode from bill_ybbxmxb where billcode=amain.billcode) as pzcode from (
            select 
            stepid,billDept,(select top 1 billcode from bill_main where billname=main.billname) as billCode,billName,
            billdate,gkdept,isgk,sum(billje) as billje,billuser 
              from bill_main main
            where flowID='srd' 
             group by billname,flowid,stepid,billuser,billdate,isgk,gkdept,billdept
            ) amain where 1=1
";


        //单位
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and billDept in (" + deptCodes + ")";
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
            sql += " and billDept in (" + deptCodes + ")";
        }
        //日期frm
        string strDateFrm = this.txtDateFrm.Text.Trim();
        if (!strDateFrm.Equals(""))
        {
            sql += " and convert(varchar(10),billdate,121)>='" + strDateFrm + "'";
        }
        //日期to
        string strDateTo = this.txtDateTo.Text.Trim();
        if (!strDateTo.Equals(""))
        {
            sql += " and convert(varchar(10),billdate,121)<='" + strDateTo + "'";
        }
        //报销人

        if (!string.IsNullOrEmpty(this.txtBxr.Text.Trim()))
        {
            string strBxr = (new PublicServiceBLL()).SubSting(this.txtBxr.Text.Trim());

            sql += " and billUser='" + strBxr + "'";
        }
        //string strBxr = this.txtBxr.Text.Trim();
        //if (!strBxr.Equals(""))
        //{
        //    strBxr = strBxr.Substring(1, strBxr.IndexOf("]") - 1);
        //    sql += " and billUser='" + strBxr + "'";
        //}
        //科目
        if (!string.IsNullOrEmpty(this.txtSubject.Text.Trim()))
        {
            string strSubject = (new PublicServiceBLL()).SubSting(this.txtSubject.Text.Trim());
            sql += " and billcode in (select billcode from bill_ybbxmxb_fykm where fykm = '" + strSubject + "') ";
        }
        //string strSubject = this.txtSubject.Text.Trim();
        //if (!strSubject.Equals(""))
        //{
        //    strSubject = strSubject.Substring(1, strSubject.IndexOf("]") - 1);
        //    sql += " and billcode in (select billcode from bill_ybbxmxb_fykm where fykm = '" + strSubject + "') ";
        //}
        //报销单号
        string strBillCode = this.txtBillCode.Text.Trim();
        if (!string.IsNullOrEmpty(strBillCode))
        {
            sql += " and billName like '%" + strBillCode + "%' ";
        }
        //审核状态
        string strStatus = this.ddlStatus.SelectedValue;
        if (!strStatus.Equals(""))
        {

            if (strStatus.Equals("-1"))
            {
                sql += " and billcode not in (select billcode from workflowrecord)";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='D' ";
            }
            else if (strStatus.Equals("1"))
            {
                sql += " and billcode in (select billcode from workflowrecord where rdState='1')";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='1'";
            }
            else
            {
                sql += " and billcode in (select billcode from workflowrecord where rdState='2')";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='2'";
            }
        }

        string strgfstatus = this.DropDownList1.SelectedValue;
        if (!strgfstatus.Equals(""))
        {

            sql += " and (select sfgf from bill_ybbxmxb where billcode=bill_main.billcode) ='" + strgfstatus.Trim() + "'";
        }
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    private string getSelectSql()
    {
        string sql = @"select *,(select bxzy from bill_ybbxmxb where bill_ybbxmxb.billCode=amain.billCode) as bxzy,
            (select username from bill_users where usercode=amain.billuser) as billUser,
            (select (case isnull(sfgf,'0') when '0' then '未给付' when '1' then '已给付' end) as sfgf from bill_ybbxmxb where billcode=amain.billcode) as sfgf,
            (select ybje from bill_ybbxmxb where billcode=amain.billcode)as gfje,
            (select (select username from bill_users where usercode=gfr) as gfr from bill_ybbxmxb where billcode=amain.billcode) as gfr,
            (select gfsj from bill_ybbxmxb where billcode=amain.billcode) as gfsj,
            (select cxyy from bill_ybbxmxb where billcode=amain.billcode) as cxyy,
            (select (select username from bill_users where usercode=cxr) as cxr from bill_ybbxmxb  where billcode=amain.billcode) as cxr,
            (select cxsj from bill_ybbxmxb where billcode=amain.billcode) as cxsj,
            (select pzcode from bill_ybbxmxb where billcode=amain.billcode) as pzcode from (
            select 
            stepid,billDept,(select top 1 billcode from bill_main where billname=main.billname) as billCode,billName,
            billdate,gkdept,isgk,sum(billje) as billje,billuser
              from bill_main main
            where flowID='srd' 
             group by billname,flowid,stepid,billuser,billdate,isgk,gkdept,billdept
            ) amain where 1=1
";
        //单位
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and billDept in (" + deptCodes + ")";
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
            sql += " and billDept in (" + deptCodes + ")";
        }
        //日期frm
        string strDateFrm = this.txtDateFrm.Text.Trim();
        if (!strDateFrm.Equals(""))
        {
            sql += " and convert(varchar(10),billdate,121)>='" + strDateFrm + "'";
        }
        //日期to
        string strDateTo = this.txtDateTo.Text.Trim();
        if (!strDateTo.Equals(""))
        {
            sql += " and convert(varchar(10),billdate,121)<='" + strDateTo + "'";
        }
        //报销人

        if (!string.IsNullOrEmpty(this.txtBxr.Text.Trim()))
        {
            string strBxr = (new PublicServiceBLL()).SubSting(this.txtBxr.Text.Trim());

            sql += " and billUser='" + strBxr + "'";
        }
        //string strBxr = this.txtBxr.Text.Trim();
        //if (!strBxr.Equals(""))
        //{
        //    strBxr = strBxr.Substring(1, strBxr.IndexOf("]") - 1);
        //    sql += " and billUser='" + strBxr + "'";
        //}
        //科目
        if (!string.IsNullOrEmpty(this.txtSubject.Text.Trim()))
        {
            string strSubject = (new PublicServiceBLL()).SubSting(this.txtSubject.Text.Trim());
            sql += " and billcode in (select billcode from bill_ybbxmxb_fykm where fykm = '" + strSubject + "') ";
        }
        //string strSubject = this.txtSubject.Text.Trim();
        //if (!strSubject.Equals(""))
        //{
        //    strSubject = strSubject.Substring(1, strSubject.IndexOf("]") - 1);
        //    sql += " and billcode in (select billcode from bill_ybbxmxb_fykm where fykm = '" + strSubject + "') ";
        //}
        //报销单号
        string strBillCode = this.txtBillCode.Text.Trim();
        if (!string.IsNullOrEmpty(strBillCode))
        {
            sql += " and billName like '%" + strBillCode + "%' ";
        }
        //审核状态
        string strStatus = this.ddlStatus.SelectedValue;
        if (!strStatus.Equals(""))
        {

            if (strStatus.Equals("-1"))
            {
                sql += " and billcode not in (select billcode from workflowrecord)";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='D' ";
            }
            else if (strStatus.Equals("1"))
            {
                sql += " and billcode in (select billcode from workflowrecord where rdState='1')";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='1'";
            }
            else
            {
                sql += " and billcode in (select billcode from workflowrecord where rdState='2')";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='2'";
            }
        }

        string strgfstatus = this.DropDownList1.SelectedValue;
        if (!strgfstatus.Equals(""))
        {

            sql += " and (select sfgf from bill_ybbxmxb where billcode=bill_main.billcode) ='" + strgfstatus.Trim() + "'";
        }
        sql += " order by billdate desc";
        return sql;
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    protected void Button3_Click1(object sender, EventArgs e)
    {
        string billCode = hd_billCode.Value.ToString().Trim();
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('srdDetail.aspx?type=look&billCode=" + billCode + "');", true);
    }

    protected void Button5_Click(object sender, EventArgs e)
    {

        string billCode = hd_billCode.Value.ToString().Trim();
        billCode=server.GetCellValue("select billName from bill_main where billCode='"+billCode+"'");
        ClientScript.RegisterStartupScript(this.GetType(), "", "openLookSpStep('../../workflow/steplook.aspx?billType=ybbx&billCode=" + billCode + "');", true);
    }
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button6_Click1(object sender, EventArgs e)
    {
        string billCode = hd_billCode.Value.ToString().Trim();
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../YbbxPrint/YbbxPrint.aspx?billCode=" + billCode + "');", true);
    }
    decimal deJe = 0;
    decimal decgfje = 0;
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string billcode = e.Item.Cells[0].Text;//报销单号
            e.Item.Cells[3].Text = (new billCoding()).getDeptLevel2Name(e.Item.Cells[3].Text.ToString().Trim());//所属部门
            string zt = e.Item.Cells[8].Text;//状态
            if (zt == "end")
            {
                e.Item.Cells[8].Text = "审批通过";
            }
            else
            {
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[8].Text = state;
            }
            //金额
            string strEveJe = e.Item.Cells[5].Text.Trim();//报销总额
            decimal deEveJe = 0;
            if (decimal.TryParse(strEveJe, out deEveJe))
            {
                deJe += deEveJe;
            }

            string strgfje = e.Item.Cells[10].Text.Trim();//给付金额
            decimal deGfje = 0;
            if (decimal.TryParse(strgfje, out deGfje))
            {
                decgfje += deGfje;
            }
            //添加附加单据信息
            string strFuJiaDanJu = this.getFuJiaDanJu(billcode);
            if (!strFuJiaDanJu.Equals(""))
            {
                string[] arrDanJuCode = strFuJiaDanJu.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string strEndText = "";
                for (int i = 0; i < arrDanJuCode.Length; i++)
                {
                    string strDanJuType = arrDanJuCode[i].Substring(0, 4);
                    switch (strDanJuType)
                    {
                        case "ccsq": strEndText += string.Format("<a href='#' style=\"color:Blue\" onclick=\"window.showModalDialog('../fysq/travelReportDetail.aspx?Ctrl=View&AppCode={0}', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:940px;status:no;scroll:yes');\">{0}</a>", arrDanJuCode[i]); break;
                        case "lscg": strEndText += string.Format("<a href='#' style=\"color:Blue\" onclick=\"window.showModalDialog('../fysq/lscgDetail.aspx?type=look&cgbh={0}','newwindow','center:yes;dialogHeight:560px;dialogWidth:940px;status:no;scroll:yes');\">{0}</a>", arrDanJuCode[i]); break;
                        case "cgsp": strEndText += string.Format("<a href='#' style=\"color:Blue\" onclick=\"window.showModalDialog('../fysq/cgspDetail.aspx?type=look&cgbh={0}', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:940px;status:no;scroll:yes');\">{0}</a>", arrDanJuCode[i]); break;
                        default: strEndText += strFuJiaDanJu;
                            break;
                    }
                    strEndText += ",";
                }
                strEndText = strEndText.Substring(0, strEndText.Length - 1);
                e.Item.Cells[7].Text = strEndText;//附加单据
            }
        }
        //else if (e.Item.ItemType == ListItemType.Footer)
        //{
        //    e.Item.Cells[1].Text = "合计：";
        //    e.Item.Cells[1].Attributes.Add("align", "right");
        //    e.Item.Cells[5].Text = deJe.ToString("N2");
        //    e.Item.Cells[10].Text = decgfje.ToString("N2");
        //}
    }
    /// <summary>
    /// 导出excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        string sql = getSelectSql();
        DataTable dtExport = new DataTable();
        dtExport = server.GetDataSet(sql).Tables[0];
        DataTableToExcel(dtExport, this.myGrid, null);
    }
    public delegate void MyDelegate(DataGrid gv);
    protected void DataTableToExcel(DataTable dtData, DataGrid stylegv, MyDelegate rowbound)
    {
        if (dtData != null)
        {
            // 设置编码和附件格式
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.Charset = "utf-8";

            // 导出excel文件
            // IO用于导出并返回excel文件
            StringWriter strWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(strWriter);

            DataGrid gvExport = new DataGrid();


            gvExport.AutoGenerateColumns = false;
            BoundColumn bndColumn = new BoundColumn();
            for (int j = 0; j < stylegv.Columns.Count - 1; j++)
            {
                bndColumn = new BoundColumn();
                if (stylegv.Columns[j] is BoundColumn)
                {
                    bndColumn.DataField = ((BoundColumn)stylegv.Columns[j]).DataField.ToString();
                    bndColumn.HeaderText = ((BoundColumn)stylegv.Columns[j]).HeaderText.ToString();

                    //添加一列
                    gvExport.Columns.Add(bndColumn);
                }
            }
            gvExport.DataSource = dtData.DefaultView;
            gvExport.AllowPaging = false;
            gvExport.DataBind();
            if (rowbound != null)
            {
                rowbound(gvExport);
            }

            // 返回客户端
            gvExport.RenderControl(htmlWriter);
            Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\">");
            Response.Write(strWriter.ToString());
            Response.Write("</body></html>");
            Response.End();
        }
    }

    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["username"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }
    private string GetYSSubjectAll(string strdept)
    {
        DataSet ds = server.GetDataSet("select '['+yskmCode+']'+yskmMc as kemu from  bill_yskm");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["kemu"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }

    private string getFuJiaDanJu(string strBillCode)
    {
        string strSql = "select sqCode from bill_ybbx_fysq where billCode='" + strBillCode + "'";
        DataTable dtRel = server.GetDataTable(strSql, null);
        string strReturn = "";
        if (dtRel != null && dtRel.Rows.Count > 0)
        {
            for (int i = 0; i < dtRel.Rows.Count; i++)
            {
                strReturn += dtRel.Rows[i][0].ToString() + ",";
            }
            strReturn = strReturn.Substring(0, strReturn.Length - 1);
        }
        return strReturn;
    }
}
