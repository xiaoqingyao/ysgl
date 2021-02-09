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
using GoldMantis.Web.UI.WebControls;
using Bll.Zichan;
using System.IO;

public partial class ZiChan_ZiChanGuanLi_ZiChanLeiBieList : System.Web.UI.Page
{
    ZiChan_LeibieBll billLeibie = new ZiChan_LeibieBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            bindData();
        }
    }
    private void bindData()
    {
        TreeListView.TreeImageFolder = "../../webBill/Resources/Images/TreeListView/";
        this.tlm.RootNodeFlag = "0";
        this.tlm.DataSource = billLeibie.GetAll();
        this.tlm.DataBind();
    }
    protected void btn_delete_Click(object serder, EventArgs e)
    {
        string strCode = this.hdDelCode.Value;
        string msg = "";
        int iRel = new ZiChan_LeibieBll().Delete(strCode, out msg);
        if (iRel > 0)
        {
            showMessage("删除成功！", false, "");
        }
        else
        {
            showMessage("删除失败：原因：" + msg, false, "");
        }
        bindData();
    }
    /// <summary>
    /// 导出表格
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void Button1_Click(object sender, EventArgs e)
    {
      
        DataTable dtExport = new DataTable();
        dtExport = billLeibie.GetAlldt();
        DataTableToExcel(dtExport,this.tlm);
    }

    public delegate void MyDelegate(DataGrid gv);
    protected void DataTableToExcel(DataTable dtData, TreeListView stylegv)
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

            TreeListView gvExport = new TreeListView();


            gvExport.AutoGenerateColumns = false;
            BoundField bndColumn = new BoundField();
            for (int j = 0; j < stylegv.Columns.Count - 1; j++)
            {
                bndColumn = new BoundField();
                if (stylegv.Columns[j] is BoundField)
                {
                    bndColumn.DataField = ((BoundField)stylegv.Columns[j]).DataField.ToString();
                    bndColumn.HeaderText = ((BoundField)stylegv.Columns[j]).HeaderText.ToString();

                    //添加一列
                    gvExport.Columns.Add(bndColumn);
                }
            }
            gvExport.DataSource = billLeibie.GetAll();
            gvExport.AllowPaging = false;
            gvExport.DataBind();

            // 返回客户端
            gvExport.RenderControl(htmlWriter);
            Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\">");
            Response.Write(strWriter.ToString());
            Response.Write("</body></html>");
            Response.End();
        }
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
}
