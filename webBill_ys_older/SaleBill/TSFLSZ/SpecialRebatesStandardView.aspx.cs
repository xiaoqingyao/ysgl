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
using Bll.SaleBill;
using Models;
using System.Collections.Generic;
using Bll;

public partial class SaleBill_TSFLSZ_SpecialRebatesStandardView : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    Bll.Sepecial.SpecialRebStandBLL spebill = new Bll.Sepecial.SpecialRebStandBLL();
    ViewBLL viewbill = new ViewBLL();

    string strCtrl = "";
    string strSqCode = "";//申请单号
    //string StrCarcode = "";//车架号
    string StrDeptCode = "";//申请部门
    string Strstuta = "";//批复状态
   // string strybfee = "";//正常返利标准
    //string strvesp = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request["Carcode"].ToString() != null)
        //{
        //    StrCarcode = Request["Carcode"].ToString();
        //}
        //if (Request["deptcode"].ToString() != null)
        //{
        //    StrDeptCode = Request["deptcode"].ToString();
        //}

        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        object objCtrl = Request["Ctrl"];
        if (objCtrl != null)
        {
            strCtrl = objCtrl.ToString();
        }
        if (Request["SqCode"].ToString() != "" && Request["SqCode"] != null)
        {
            strSqCode = Request["SqCode"].ToString();
        }
        if (Request["stu"].ToString() != null && Request["stu"].ToString() != "")
        {
            Strstuta = Request["stu"].ToString();
        }
        //if (Request["ybfee"] != null && Request["ybfee"].ToString() != "")
        //{
        //    strybfee = Request["ybfee"].ToString();
        //} 
        //if (Request["vesp"] != null && Request["vesp"].ToString() != "")
        //{
        //    strvesp = Request["vesp"].ToString();
        //}
        if (!IsPostBack)
        {
            bindData();
        }
    }

    private void bindData()
    {
        //根据部门查询费用类别

        //查询配置明细

        if (strSqCode != "" && strSqCode != null)
        {
           
            this.lbsqcode.Text = strSqCode;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请单号为空！');", true);
            return;
        }
        IList<T_SpecialRebatesAppmode> lstSpecialRebateAppModel = new SpecialRebatesAppBLL().GetListByBillCode(strSqCode);
        if (lstSpecialRebateAppModel.Count <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('车辆明细丢失！');", true);
            return;
        }
        StrDeptCode = lstSpecialRebateAppModel[0].SaleDeptCode;//开票部门
        DataTable dtOtherDeptCodes = new ConfigBLL().GetDtByKey("SaleRebateDepts");
        string[] arrAllDeptCodes = new string[dtOtherDeptCodes.Rows.Count+1];
        for (int i = 0; i < dtOtherDeptCodes.Rows.Count; i++)
        {
            arrAllDeptCodes[i] = dtOtherDeptCodes.Rows[i][0].ToString();
        }
        arrAllDeptCodes[dtOtherDeptCodes.Rows.Count] = StrDeptCode;
        DataTable dtspecil = new DataTable();
        for (int j = 0; j < arrAllDeptCodes.Length; j++)
        {
            for (int i = 0; i < lstSpecialRebateAppModel.Count; i++)
            {
                DataTable dtEveNote = spebill.getTablepro(arrAllDeptCodes[j], strSqCode, lstSpecialRebateAppModel[i].TruckCode);
                dtEveNote.Columns.Add("deptName");
                string strDeptName = new DepartmentBLL().GetShowNameByCode(arrAllDeptCodes[j]);
                for (int k = 0; k < dtEveNote.Rows.Count; k++)
                {
                    dtEveNote.Rows[k]["deptName"] = strDeptName;
                }
                if (j == 0 && i == 0)
                {
                    dtspecil = dtEveNote.Clone();
                }
                dtspecil.Merge(dtEveNote);
            }
        }
        this.DataGrid2.DataSource = dtspecil;
        this.DataGrid2.DataBind();
        
        if (StrDeptCode != "" && StrDeptCode != null)
        {
            this.lbdeptcode.Text = GetdetpAll();
        }
        if (Strstuta == "已批复")
        {
            this.trpf.Visible = false;
        }
    }
    private string GetdetpAll()
    {
        string strsql = "select deptCode, '['+deptCode+']'+deptName as dtname from  bill_departments where 1=1";
        if (StrDeptCode != null && StrDeptCode != "")
        {
            strsql += " and deptCode=" + StrDeptCode;
        }
        DataSet ds = server.GetDataSet(strsql);
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dtname"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        script = script.Substring(1, script.Length - 2);
        return script;
    }

    protected void btpf_Click(object sender, EventArgs e)
    {
        string strsql = "update T_SpecialRebatesStandard set Status='1' where AppBillCode='" + strSqCode + "'";
        int row = server.ExecuteNonQuery(strsql);
        if (row > -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('批复成功！');window.returnValue='1';self.close();", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('批复失败！')", true);
            return;
        }
    }
}
