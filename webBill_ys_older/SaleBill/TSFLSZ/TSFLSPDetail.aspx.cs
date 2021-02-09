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

public partial class SaleBill_TSFLSZ_TSFLSPDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ViewBLL viewbill = new ViewBLL();
    string strCtrl = "";
    string strSqCode = "";//申请单号
    string StrCarcode = "";//车架号
    string StrDeptCode = "";//申请部门
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            if (Request["Carcode"].ToString() != null)
            {
                StrCarcode = Request["Carcode"].ToString();
            }
            if (Request["deptcode"].ToString() != null)
            {
                StrDeptCode = Request["deptcode"].ToString();
            }
            object objCtrl = Request["Ctrl"];
            if (objCtrl != null)
            {
                strCtrl = objCtrl.ToString();
            }
            if (Request["SqCode"].ToString()!=""&&Request["SqCode"]!=null)
            {
                strSqCode = Request["SqCode"].ToString();
            }
        }
        if (!IsPostBack)
        {
            //this.txtSqrq.Attributes.Add("onfocus", "javascript:setday(this);");
            //this.txtSqrq1.Attributes.Add("onfocus", "javascript:setday(this);");
            DataTable dt = server.RunQueryCmdToTable("select Code, CName from  T_ControlItem");
            Repeater1.DataSource = dt;
            Repeater1.DataBind();

            DataTable dtsale = server.RunQueryCmdToTable("select Code,PName from  dbo.T_SaleProcess where Status='1'");
            DataGrid1.DataSource = dtsale;
            DataGrid1.DataBind();

            bindData();

        }
        ClientScript.RegisterArrayDeclaration("availableTags", GetcarAll());
        ClientScript.RegisterArrayDeclaration("availableTagsdt", GetdetpAll());
        ClientScript.RegisterArrayDeclaration("availableTagsfy", GetdefyAll(StrDeptCode));
    }


    private void bindData()
    {
        DataTable dtis = server.RunQueryCmdToTable("select * from dbo.T_SpecialRebatesStandard where AppBillCode='" + strSqCode+"'");
        if (strSqCode != "" && strSqCode != null)
        {
            this.lbdjmc.Text = "申请单号" + strSqCode;
        }
        if (StrCarcode != "" && StrCarcode != null)
        {
            string strcartypename = viewbill.GetTruckTypeByCode(StrCarcode).Trim();
            if (strcartypename!=""&&strcartypename!=null)
            {
                this.lbmcar.Text = "车辆类型：" + strcartypename;

                this.txtCartype.Text = strcartypename;
                
            }
         
        }
        if (StrDeptCode != "" && StrDeptCode != null)
        {
            //根据部门号读取名称
            string strbm = GetdetpAll();
            this.lblbm.Text = "部门：" + strbm.Substring(1, strbm.Length - 2);

            string strdeptcode =GetdetpAll();
            this.txtdept.Text = strdeptcode.Substring(1, strdeptcode.Length - 2);
        }

        if (strCtrl == "View" && dtis.Rows.Count > 0)
        {
            this.tdpzadd.Visible = false;
            this.tdxsgadd.Visible = false;
            this.btn_test.Visible = false;
            this.txtfylb.Enabled = false;
            this.tdfylb.Visible = false;
           
            //查询销售过程的值
            DataTable dtspesq = server.RunQueryCmdToTable("select * from dbo.T_SpecialRebatesStandard where AppBillCode='" + strSqCode + "' and SaleProcessCode<>''");
            this.DataGrid2.DataSource = dtspesq;
            this.DataGrid2.DataBind();
            this.txtfylb.Text = dtis.Rows[0]["SaleFeeTypeCode"].ToString();
            
            //查询配置项的值
            DataTable dtpz = server.RunQueryCmdToTable("select * from dbo.T_SpecialRebatesStandard where AppBillCode='" + strSqCode + "' and ControlItemCode<>''");

            this.DataGrid3.DataSource = dtpz;
            this.DataGrid3.DataBind();
          
            
        }
        if (strCtrl == "Edit" )
        {
            this.tdpzview.Visible = false;
            this.tdxsgcview.Visible = false;
        }
        else {
        
        }
    }
    private string GetcarAll()
    {
        string script = "";
        DataSet ds = server.GetDataSet("select '['+CAST(typeCode AS varchar(100)) +']'+typeName as kemu from  T_truckType where IsLastNode='1'");
        StringBuilder arry = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                arry.Append("'");
                arry.Append(Convert.ToString(dr["kemu"]));
                arry.Append("',");
            }

            script = arry.ToString().Substring(0, arry.Length - 1);

        }

        return script;

    }

    private string GetdetpAll()
    {
        string strsql = "select deptCode, '['+deptCode+']'+deptName as dtname from  bill_departments where 1=1";
        if (StrDeptCode!=null&&StrDeptCode!="")
        {
            strsql+=" and deptCode="+StrDeptCode;
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
        //script = script.Substring(1, script.Length - 2);
        return script;
    }
    ///// <summary>
    ///// 根据车架号获取车辆类型
    ///// </summary>
    ///// <returns></returns>
    //private string GetdcartypeAll()
    //{

    //    string strsql = @"select top 1 cxh as cartypename  from V_TruckMsg where cjh='" + StrCarcode + "'";
      
    //    DataSet ds = server.GetDataSet(strsql);
    //    StringBuilder arry = new StringBuilder();
    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {
    //        arry.Append("'");
    //        arry.Append(Convert.ToString(dr["cartypename"]));
    //        arry.Append("',");
    //    }
    //    if (arry.Length > 1)
    //    {
    //        string script = arry.ToString().Substring(0, arry.Length - 1);
    //        return script;
    //    }
    //    else
    //    {
    //        return "";
    //    }
    //}
    private string GetdefyAll(string strdepcode)
    {
        string strSql = "select '['+yskmCode+']'+yskmMC as kmMc from Bill_Yskm where yskmcode in(select yskmcode from bill_yskm_dept where deptCode='" + strdepcode + "')";
       
       
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

    public IList<T_SpecialRebatesStandardmodel> getmodel(out bool isp)
    {
        IList<T_SpecialRebatesStandardmodel> list = new List<T_SpecialRebatesStandardmodel>();
        T_SpecialRebatesStandardmodel model = new T_SpecialRebatesStandardmodel();
      

        string strDeptCode = "";
        string strTruckTypeCode = "";
        string strSaleFeeTypeCode = "";
       
        if (this.txtdept.Text.Trim() != "" && this.txtdept.Text.Trim() != null)
        {
           string  strdepcode = this.txtdept.Text.Trim();
           strDeptCode = strdepcode.Substring(1, strdepcode.IndexOf("]") - 1);
            isp = true;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门不能为空！');", true);
            isp = false;
        }
        //车辆类型
        if (this.btcartype.Value.Trim() != null && this.btcartype.Value.Trim()!="")
        {
            string strc = this.txtCartype.Text.Trim();
            strTruckTypeCode = strc;
        }

        if (this.txtfylb.Text != "" && this.txtfylb.Text != null)
        {
            string strfeelcode = this.txtfylb.Text;
            strSaleFeeTypeCode = strfeelcode.Substring(1, strfeelcode.IndexOf("]") - 1);
            isp = true;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('消费类型不能为空')", true);
            isp = false;
        }
        //销售提成
        int SaleCount = this.DataGrid1.Items.Count;
        DataTable dtsale = server.RunQueryCmdToTable("select Code,PName from  dbo.T_SaleProcess where Status='1'");
        for (int i = 0; i < SaleCount; i++)
        {

            model = new T_SpecialRebatesStandardmodel();
            model.AppBillCode = strSqCode;
            model.TruckCode = StrCarcode;
            model.TruckTypeCode = strTruckTypeCode;
            model.SysUserCode = Session["userCode"].ToString().Trim();
            model.SysTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            model.Status = "0";
            model.MarkerCode = Session["userName"].ToString().Trim();
            model.DeptCode = strDeptCode;
            model.Type = "1";
            model.SaleFeeTypeCode = strSaleFeeTypeCode;
            model.ControlItemCode =  dtsale.Rows[i]["Code"].ToString().Trim();
            TextBox txtFee = this.DataGrid1.Items[i].Cells[1].FindControl("txtTc") as TextBox;
            if (txtFee.Text != "" && txtFee.Text != null)
            {
                model.Fee = decimal.Parse(txtFee.Text.Trim());
            }
            else
            {
                model.Fee = decimal.Parse("0");
            }


            list.Add(model);

        }


        DataTable dt = server.RunQueryCmdToTable("select Code, CName from  T_ControlItem");
        int iCount = this.Repeater1.Items.Count;
        //配置项
        for (int i = 0; i < iCount; i++)
        {
            model = new T_SpecialRebatesStandardmodel();
            model.AppBillCode = strSqCode;
            model.TruckCode = StrCarcode;
            model.TruckTypeCode = strTruckTypeCode;
            model.SysUserCode = Session["userCode"].ToString().Trim();
            model.SysTime = DateTime.Now.ToString("yy-MM-dd:hh:mm:ss");
            model.Status = "0";
            model.MarkerCode = Session["userName"].ToString().Trim();
            model.DeptCode = strDeptCode;
            model.Type = "2";
            model.SaleFeeTypeCode = strSaleFeeTypeCode;
            model.ControlItemCode =dt.Rows[i]["Code"].ToString().Trim();
         
            TextBox txtFee = this.Repeater1.Items[i].Cells[1].FindControl("txtpz") as TextBox;
            if (txtFee.Text != "" && txtFee.Text != null)
            {
                model.Fee = decimal.Parse(txtFee.Text.Trim());
            }
            else
            {
                model.Fee = decimal.Parse("0");
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
                clear();
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
    
        this.txtfylb.Text = "";
       
     
        int iCount = this.Repeater1.Items.Count;
        //配置项
        for (int i = 0; i < iCount; i++)
        {
            TextBox txtFee = this.Repeater1.Items[i].Cells[1].FindControl("txtpz") as TextBox;
            txtFee.Text = "";
        }

        int salecount = this.DataGrid1.Items.Count;
        for (int i = 0; i < salecount; i++)
        {
            TextBox txtsale = this.DataGrid1.Items[i].Cells[1].FindControl("txtTc") as TextBox;
            txtsale.Text = "";
        }

    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
    }
}
