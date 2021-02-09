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
using Bll;
using Bll.Bills;
using Models;
using System.Collections.Generic;
using System.Text;
using Bll.Sepecial;
using Bll.SaleBill;

public partial class SaleBill_TSFLSZ_TsflDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    Bll.Sepecial.SpecialRebStandBLL spebill = new Bll.Sepecial.SpecialRebStandBLL();
    ViewBLL viewbill = new ViewBLL();
    string strCtrl = "";
    string strSqCode = "";//申请单号
    string StrDeptCode = "";//申请部门
    string strybfee = "";//正常返利标准
    string strvesp = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            object objCtrl = Request["Ctrl"];
            if (objCtrl != null)
            {
                strCtrl = objCtrl.ToString();
            }
            if (Request["SqCode"].ToString() != "" && Request["SqCode"] != null)
            {
                strSqCode = Request["SqCode"].ToString();
            }
        }
        if (!IsPostBack)
        {
            bindData();
        }
    }
    private void bindData()
    {
        if (strSqCode != "" && strSqCode != null)
        {
            this.lbdjmc.Text = strSqCode;
        }else{
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请单号为空！');", true);
            return;
        }
        IList<T_SpecialRebatesAppmode> lstSpecialRebateAppModel = new SpecialRebatesAppBLL().GetListByBillCode(strSqCode);
        if (lstSpecialRebateAppModel.Count<=0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('车辆明细丢失！');", true);
            return;
        }
        StrDeptCode = lstSpecialRebateAppModel[0].SaleDeptCode;//开票部门
        //同时也有返利的部门
        DataTable dtOtherDeptCodes = new ConfigBLL().GetDtByKey("SaleRebateDepts");
        string[] arrAllDeptCodes = new string[dtOtherDeptCodes.Rows.Count+1];
        for (int i = 0; i < dtOtherDeptCodes.Rows.Count; i++)
        {
            arrAllDeptCodes[i] = dtOtherDeptCodes.Rows[i][0].ToString();
        }
        arrAllDeptCodes[dtOtherDeptCodes.Rows.Count] = StrDeptCode;
        DataTable dtspecil=new DataTable();
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
        
        this.mygrid.DataSource = dtspecil;
        this.mygrid.DataBind();
        int iCount = this.mygrid.Items.Count;
        //配置项
        for (int i = 0; i < iCount; i++)
        {
            TextBox txtfee = this.mygrid.Items[i].Cells[6].FindControl("txtpfje") as TextBox;
            txtfee.Text = dtspecil.Rows[i]["specialFee"].ToString();
        }
        if (StrDeptCode != "" && StrDeptCode != null)
        {
            //根据部门号读取名称
            string strbm = GetdetpAll();
            this.lblbm.Text = strbm.Substring(1, strbm.Length - 2);
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
        return script;
    }

    public IList<T_SpecialRebatesStandardmodel> getmodel(out bool isp)
    {
        IList<T_SpecialRebatesStandardmodel> list = new List<T_SpecialRebatesStandardmodel>();
        T_SpecialRebatesStandardmodel model = new T_SpecialRebatesStandardmodel();
        if (this.lblbm.Text.Trim() != "" && this.lblbm.Text.Trim() != null)
        {
            string strdepcode = this.lblbm.Text.Trim();
            isp = true;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门不能为空！');", true);
            isp = false;
        }
        int dtcont = this.mygrid.Items.Count;
        if (dtcont==0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('没有记录可保存！');", true);
            isp = false;
        }
        for (int i = 0; i < dtcont; i++)
        {
            model = new T_SpecialRebatesStandardmodel();
            model.AppBillCode = strSqCode;
            string truckTypeName=this.mygrid.Items[i].Cells[2].Text;
            model.TruckTypeCode = truckTypeName.Substring(1, truckTypeName.IndexOf(']')-1);
            model.TruckCode = this.mygrid.Items[i].Cells[1].Text;
            model.SysUserCode = Session["userCode"].ToString().Trim();
            model.SysTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            model.Status = "0";
            model.MarkerCode = Session["userCode"].ToString().Trim();
            string strDeptCode = this.mygrid.Items[i].Cells[0].Text;
            model.DeptCode = strDeptCode.Substring(1, strDeptCode.IndexOf(']') - 1);
            string controlCode=this.mygrid.Items[i].Cells[4].Text;
            model.ControlItemCode = controlCode.Substring(1, controlCode.IndexOf(']') - 1);
            //费用类别 如果tCode的前缀是SP是销售提成则为1如果是CF则是配置项 值则为2
            string strtype = controlCode.Substring(0, 2);
            if (strtype == "SP")
            {
                model.Type = "1";
            }
            else
            {
                model.Type = "2";
            }
            string strFeeTypeCode = this.mygrid.Items[i].Cells[3].Text;
            model.SaleFeeTypeCode = strFeeTypeCode.Substring(1, strFeeTypeCode.IndexOf(']') - 1);

            TextBox txtFee = this.mygrid.Items[i].Cells[4].FindControl("txtpfje") as TextBox;
            if (txtFee.Text != "" && txtFee.Text != null)
            {
                model.Fee = decimal.Parse(txtFee.Text.Trim());
            }
            else
            {
                model.Fee = 0;
            }
            list.Add(model);
        }
        return list;


    }
    //添加
    protected void btn_test_Click(object sender, EventArgs e)
    {
        string msg = "";
        bool flg = true;
        SpecialRebStandBLL spebll = new SpecialRebStandBLL();
        IList<T_SpecialRebatesStandardmodel> Ilistmode = getmodel(out flg);
        if (flg == true)
        {
            int iRel = spebll.Insert(Ilistmode, out msg);
            if (iRel > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);

                return;

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败！');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请检查所填项！');", true);
        }

    }

    public void clear()
    {

        //this.txtfylb.Text = "";

        int iCount = this.mygrid.Items.Count;
        //配置项
        for (int i = 0; i < iCount; i++)
        {
            TextBox txtFee = this.mygrid.Items[i].Cells[6].FindControl("txtpfje") as TextBox;
            txtFee.Text = "";
        }

    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
    }
}
