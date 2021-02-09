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
using Bll.SaleBill;
using WorkFlowLibrary.WorkFlowBll;
using System.Text;
using System.IO;

public partial class SaleBill_Flsz_SaleFeeSpendNoteQuery : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    T_SaleFeeSpendNoteBll specibll = new T_SaleFeeSpendNoteBll();
    string strdeptcode = "";
    string strCtrl = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            object objDateFrm = Request["DateFrm"];
            if (objDateFrm != null)
            {
                this.txtDateFrm.Text = objDateFrm.ToString();
            }
            object objDateTo = Request["DateTo"];
            if (objDateTo != null)
            {
                txtDateTo.Text = objDateTo.ToString();
            }
            object objDeptCode = Request["DeptCode"];
            if (objDeptCode != null)
            {
                txtdeptname.Text = objDeptCode.ToString();
            }
            object objkmCode = Request["kmCode"];
            if (objkmCode != null)
            {
                txtyskmname.Text = objkmCode.ToString();
            }
            object objCtrl = Request["Ctrl"];
            if (objCtrl!=null)
            {
                strCtrl = objCtrl.ToString();
            }
            if (!IsPostBack)
            {
                this.BindDataGrid();
            }
        }
        ClientScript.RegisterArrayDeclaration("availableTagsysgc", GetysgcAll());
        ClientScript.RegisterArrayDeclaration("availableTagsdt", GetdetpAll());
        ClientScript.RegisterArrayDeclaration("availableTagsfy", GetdefyAll());
    }

    void BindDataGrid()
    {
        if (strCtrl.Equals("Select"))
        {
            this.txtdeptname.Enabled = false;
        }
        //string sql = getSelectSql();
        Models.T_SaleFeeSpendNote model = new Models.T_SaleFeeSpendNote();
        //时间起
        if (!string.IsNullOrEmpty(txtDateFrm.Text))
        {
            model.dateFrm = this.txtDateFrm.Text.Trim();
        }
        //时间止
        if (!string.IsNullOrEmpty(txtDateTo.Text))
        {
            model.dateTo = this.txtDateTo.Text.Trim();
        }
        //申请单号
        if (txtCode.Text != null && txtCode.Text != "")
        {
            model.Billcode = this.txtCode.Text.Trim();
        }
        ////预算科目
        if (txtyskmname.Text != null && txtyskmname.Text != "")
        {
            string stryscode = this.txtyskmname.Text.Trim();
            stryscode = stryscode.Substring(1,stryscode.IndexOf("]")-1).Trim();
            model.Yskmcode = stryscode;
        }
        ////预算过程
        if (txtysgcname.Text != null && txtysgcname.Text != "")
        {
            string txtysgc = this.txtysgcname.Text.Trim();
            txtysgc = txtysgc.Substring(1,txtysgc.IndexOf("]")-1).Trim();
            model.YsgcCode = txtysgc;
        }
        //单位
        if (txtdeptname.Text != null && txtdeptname.Text != "")
        {
            strdeptcode = txtdeptname.Text.Trim();
            strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1).Trim();
            model.Deptcode = strdeptcode;
        }
        
        DataTable temp = specibll.GetAllListBySql(model);



        #region 计算分页相关数据1

        this.lblPageSize.Text = this.myGrid.PageSize.ToString();
        this.lblItemCount.Text = temp.Rows.Count.ToString();
        double pageCountDouble = double.Parse(this.lblItemCount.Text) / double.Parse(this.lblPageSize.Text);
        int pageCount = Convert.ToInt32(Math.Ceiling(pageCountDouble));
        this.lblPageCount.Text = pageCount.ToString();
        this.drpPageIndex.Items.Clear();
        for (int i = 0; i <= pageCount - 1; i++)
        {
            int pIndex = i + 1;
            ListItem li = new ListItem(pIndex.ToString(), pIndex.ToString());
            if (pIndex == this.myGrid.CurrentPageIndex + 1)
            {
                li.Selected = true;
            }
            this.drpPageIndex.Items.Add(li);
        }
        this.showStatus();
        #endregion
        if (temp.Rows.Count==0)
        {
            temp = null;
        }

        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }
    /// <summary>
    /// 预算过程
    /// </summary>
    /// <returns></returns>
    private string GetysgcAll() 
    {
        DataSet ds = server.GetDataSet("select '['+gcbh+']'+xmmc as ysgcname from bill_ysgc");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["ysgcname"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        return script;
    }

    /// <summary>
    /// 部门
    /// </summary>
    /// <returns></returns>
    private string GetdetpAll()
    {
        DataSet ds = server.GetDataSet("select deptCode, '['+deptCode+']'+deptName as dtname from  bill_departments  where IsSell='Y'");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dtname"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        return script;
    }

    /// <summary>
    /// 费用类别
    /// </summary>
    /// <returns></returns>
    private string GetdefyAll()
    {


        string strSql = "select '['+yskmCode+']'+yskmMC as kmMc from Bill_Yskm where yskmcode in(select yskmcode from bill_yskm_dept where 1=1)";

        //if (!strdeptcode.Equals(""))
        //{
        //    // strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);
        //    strSql += " and deptCode='" + strdeptcode + "'";
        //}
        DataSet ds = server.GetDataSet(strSql);
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["kmMc"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }


    }

    #region showStatus 分页相关
    void showStatus()
    {
        if (this.drpPageIndex.Items.Count == 0)
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
            return;
        }
        if (int.Parse(this.lblPageCount.Text) == int.Parse(this.drpPageIndex.SelectedItem.Value))//最后一页
        {
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
        }
        else
        {
            this.lBtnNextPage.Enabled = true;
            this.lBtnLastPage.Enabled = true;
        }
        if (int.Parse(this.drpPageIndex.SelectedItem.Value) == 1)//第一页
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
        }
        else
        {
            this.lBtnFirstPage.Enabled = true;
            this.lBtnPrePage.Enabled = true;
        }
    }
  

    protected void lBtnFirstPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = 0;
        this.BindDataGrid();
    }
    protected void lBtnPrePage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex - 1;
        this.BindDataGrid();
    }
    protected void lBtnNextPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex + 1;
        this.BindDataGrid();
    }
    protected void lBtnLastPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.PageCount - 1;
        this.BindDataGrid();
    }
    protected void drpPageIndex_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = int.Parse(this.drpPageIndex.SelectedItem.Value) - 1;
        this.BindDataGrid();
    }
    #endregion
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    protected void Button3_Click1(object sender, EventArgs e)
    {
        string billCode = hd_billCode.Value.ToString().Trim();
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../bxgl/bxDetailFinal.aspx?type=look&billCode=" + billCode + "');", true);
    }

    protected void Button5_Click(object sender, EventArgs e)
    {

        string billCode = hd_billCode.Value.ToString().Trim();
        ClientScript.RegisterStartupScript(this.GetType(), "", "openLookSpStep('../../workflow/steplook.aspx?billType=ybbx&billCode=" + billCode + "');", true);
    }

    protected void Button6_Click1(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }


    public delegate void MyDelegate(DataGrid gv);


    public void btnExportExcel_Click(object sender, EventArgs e) {
        Response.ClearContent();
        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        myGrid.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }
    float fSumAmount = 0;
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        //显示合计行
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            float feveAmount = 0;
            bool bofeveAmount = float.TryParse(e.Item.Cells[5].Text, out feveAmount);
            fSumAmount += feveAmount;
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            e.Item.Cells[1].Text = "合计：";
            e.Item.Cells[5].Text = fSumAmount.ToString();
        }
        else { }
    }
}
