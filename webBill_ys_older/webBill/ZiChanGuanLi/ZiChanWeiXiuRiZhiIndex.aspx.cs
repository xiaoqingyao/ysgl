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
using Bll.Zichan;

public partial class webBill_ZiChanGuanLi_ZiChanWeiXiuRiZhiIndex : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ZiChan_WeiXiuRiZhiBll wxrzbll = new ZiChan_WeiXiuRiZhiBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
           
            ClientScript.RegisterArrayDeclaration("zcTags", GetzcAll());
            ClientScript.RegisterArrayDeclaration("userTags", GetUsersAll());
            if (!IsPostBack)
            {
                this.BindDataGrid();
            }
        }

    }
   
    void BindDataGrid()
    {

        Models.ZiChan_WeiXiuRiZhi model = new Models.ZiChan_WeiXiuRiZhi();

        //资产编号
        string strzccode = this.txtCode.Text;
        try
        {
            strzccode = strzccode.Substring(1, strzccode.IndexOf("]") - 1);
        }
        catch (Exception)
        {
            this.txtCode.Text = "";
            strzccode="";
        }
        if (!strzccode.Equals(""))
        {
            model.ZiChanCode = strzccode;
        }

        
        //维修人
        string strusername = this.txtname.Text;
        try
        {
            strusername = strusername.Substring(1, strusername.IndexOf("]") - 1);
        }
        catch (Exception)
        {
            this.txtname.Text = "";
            strusername="";
        }

        if (!strusername.Equals(""))
        {
             model.WeiXiuRenCode = strusername;
        }

        string strdeptcode = this.txtPaymentDeptCode.Text;
        try
        {
            strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);

        }
        catch (Exception)
        {
            this.txtPaymentDeptCode.Text = "";
            strdeptcode="";
        }

        if (!strdeptcode.Equals(""))
        {
           
            model.WeiXiuBuMenCode = strdeptcode;
        }
        //增减方式
        if (DropDownList1.SelectedValue != null && DropDownList1.SelectedValue != "")
        {

            model.ShiFouShenPi = DropDownList1.SelectedValue;

        }

        DataTable temp = wxrzbll.GetAllListBySql1(model);


        #region 计算分页相关数据

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
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
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
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        string strCode = this.hdDelCode.Value;
        string msg = "";
        int iRel = new ZiChan_WeiXiuRiZhiBll().Delete(int.Parse(strCode));
        if (iRel > 0)
        {
            showMessage("删除成功！", false, "");
        }
        else
        {
            showMessage("删除失败：原因：" + msg, false, "");
        }
        this.BindDataGrid();
    }
    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    /// <param name="isExit">提示后是否退出</param>
    /// <param name="strReturnVal">返回值</param>
    private void showMessage(string strMsg, bool isExit, string strReturnVal)
    {
        string strScript = "alert('" + strMsg + "');";
        if (!strReturnVal.Equals(""))
        {
            strScript += "window.returnValue=\"" + strReturnVal + "\";";
        }
        if (isExit)
        {
            strScript += "self.close();";
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
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
    /// <summary>
    /// 部门
    /// </summary>
    /// <returns></returns>
    private string GetDeoptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments where IsSell='Y'");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }

    public delegate void MyDelegate(DataGrid gv);
    /// <summary>
    /// 人员
    /// </summary>
    /// <returns></returns>
    private string GetUsersAll()
    {
        DataSet ds = server.GetDataSet("select '['+userCode+']'+userName as yhnames from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["yhnames"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }

    /// <summary>
    /// 资产
    /// </summary>
    /// <returns></returns>
    private string GetzcAll()
    {
        DataSet ds = server.GetDataSet("select '['+ZiChanCode+']'+ZiChanName as zcnames  from ZiChan_Jilu");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["zcnames"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
}
