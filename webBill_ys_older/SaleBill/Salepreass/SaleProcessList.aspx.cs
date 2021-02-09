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
using Dal.SaleProcess;
using Models;

public partial class SaleBill_Salepreass_SaleProcessList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    SaleProcessDal salprocedal = new SaleProcessDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
               // hf_km.Value = Request.QueryString["kmCode"];
                this.BindDataGrid();

            }
        }
    }

    public void BindDataGrid()
    {
        string strtext = this.TextBox1.Text;
        DataTable data=new DataTable();
        data = salprocedal.GetAllDate(strtext);
        this.myGrid.DataSource = data;
        this.myGrid.DataBind();
    }

  

  ///// <summary>
  ///// 删除
  ///// </summary>
  ///// <param name="sender"></param>
  ///// <param name="e"></param>

  //  protected void Button3_Click(object sender, EventArgs e)
  //  {
  //      string strCwkmCode = spcode.Value.ToString().Trim();
  //      if (strCwkmCode!=null&&strCwkmCode!="")
  //      {
  //          List<string> list = new List<string>();
  //          list.Add("delete from T_SaleProcess where Code='" + strCwkmCode + "' ");

  //          //判断要删除的科目是否有预算或者使用

  //          if (server.ExecuteNonQuerysArray(list) == -1)
  //          {
  //              ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
  //          }
  //          else
  //          {
  //              ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);

  //          }
  //          this.BindDataGrid();
  //      }
  //      else
  //      {
  //          ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择删除行！');", true);

  //      }
      
  //  }
    //protected void Button4_Click(object sender, EventArgs e)
    //{
    //    this.BindDataGrid();
    //}
   /// <summary>
   /// 启用
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void Button6_Click(object sender, EventArgs e)
    {
        SaleProcessMode processMode = new SaleProcessMode();
        string strCodes = spcode.Value.ToString().Trim();
        processMode = salprocedal.GetModel(strCodes);
        processMode.Code = strCodes;
        processMode.Status = "1";
        salprocedal.Updetesalep(processMode);
        this.BindDataGrid();
       
       
    }
 

    /// <summary>
    /// 禁用
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void Button5_Click(object sender, EventArgs e)
    {

        SaleProcessMode processMode = new SaleProcessMode();
        string strCodes = spcode.Value.ToString().Trim();
        processMode = salprocedal.GetModel(strCodes);
        processMode.Code = strCodes;
        processMode.Status = "0";
        salprocedal.Updetesalep(processMode);
        this.BindDataGrid();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    
}
