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
using Models;
using Bll.Sepecial;
using System.Collections.Generic;
using Bll.SaleBill;
using Dal.SaleBill;

public partial class SaleBill_Flsz_SpecialRebatesStandardDail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strDeptCode = "";
    string strEffectiveDateFrm = "";
    string strEffectiveDateTo = "";
    string strTruckTypeCode = "";
    string strSaleFeeTypeCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        if (!IsPostBack)
        {
            this.txtedtime.Attributes.Add("onfocus", "javascript:setday(this);");
            this.txtbgtime.Attributes.Add("onfocus", "javascript:setday(this);");
            DataTable dt = server.RunQueryCmdToTable("select Code, CName from  T_ControlItem");
            Repeater1.DataSource = dt;
            Repeater1.DataBind();

            DataTable dtsale = server.RunQueryCmdToTable("select Code,PName from  dbo.T_SaleProcess where Status='1'");
            DataGrid1.DataSource = dtsale;
            DataGrid1.DataBind();
        }
        ClientScript.RegisterArrayDeclaration("availableTags", GetcarAll());
        ClientScript.RegisterArrayDeclaration("availableTagsdt", GetdetpAll());
       

        //ClientScript.RegisterArrayDeclaration("availableTagsfy", GetdefyAll());
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
        DataSet ds = server.GetDataSet("select deptCode, '['+deptCode+']'+deptName as dtname from  bill_departments");
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

    private string GetdefyAll()
    {
        string strDeptCode = this.txtdept.Value.ToString().Trim();
    
        string strSql = "select '['+yskmCode+']'+yskmMC as kmMc from Bill_Yskm where yskmcode in(select yskmcode from bill_yskm_dept where 1=1)";
     
        if (!strDeptCode.Equals(""))
        {
            strDeptCode = strDeptCode.Substring(1, strDeptCode.IndexOf("]") - 1);
            strSql += " and deptCode='" + strDeptCode + "'";
        }
        DataSet ds = server.GetDataSet(strSql);
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["kmMc"]));
            arry.Append("',");
        }
        if (arry.Length>1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }
        

    }

    public IList<T_RebatesStandard> getmodel(out bool isp)
    {
        IList<T_RebatesStandard> list = new List<T_RebatesStandard>();
        T_RebatesStandard mode = new T_RebatesStandard();
        string strRemark = this.txtbz.Text.Trim();

      
        if (this.txtbgtime.Text != "" && this.txtbgtime.Text != null)
        {
            strEffectiveDateFrm = this.txtbgtime.Text;
            isp = true;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('有效日期起不能为空')", true);
            isp = false;
        }



        if (this.txtedtime.Text != "" && this.txtedtime.Text != null)
        {
            DateTime dtbgtime = DateTime.Parse(this.txtbgtime.Text.ToString().Trim());
            DateTime dtedtime = DateTime.Parse(this.txtedtime.Text.ToString().Trim());
            int iCompareRel = DateTime.Compare(dtedtime, dtbgtime);
            if (iCompareRel > 0)
            {
                strEffectiveDateTo = this.txtedtime.Text;
                isp = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('有效的截止日期必须大于起始日期')", true);
                isp = false;
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请填入有效的截止日期 该日期大于起始日期')", true);
            isp = false;
        }
        if (this.txtCartype.Value != "" && this.txtCartype.Value != null)
        {
            string strcarcode = this.txtCartype.Value.ToString().Trim();
            strTruckTypeCode = strcarcode.Substring(1, strcarcode.IndexOf("]") - 1); 
            isp = true;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('车辆类型不能为空')", true);
            isp = false;
        }
        if (this.txtdept.Value != "" && this.txtdept.Value != null)
        {
            string strdepcode = this.txtdept.Value;
            strDeptCode = strdepcode.Substring(1, strdepcode.IndexOf("]") - 1);
            isp = true;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门不能为空！');", true);
            isp = false;
        }


        if (this.txtfylb.Value != "" && this.txtfylb.Value != null)
        {
            string strfeelcode = this.txtfylb.Value;
            strSaleFeeTypeCode = strfeelcode.Substring(1,strfeelcode.IndexOf("]")-1);
            isp = true;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('费用类别不能为空')", true);
            isp = false;
        }
        //销售提成
        int SaleCount = this.DataGrid1.Items.Count;
        DataTable dtsale = server.RunQueryCmdToTable("select Code,PName from  dbo.T_SaleProcess where Status='1'");
        for (int i = 0; i < SaleCount; i++)
        {
            mode = new T_RebatesStandard();
            mode.DeptCode = strDeptCode;
            mode.EffectiveDateFrm = strEffectiveDateFrm;
            mode.EffectiveDateTo = strEffectiveDateTo;
            mode.TruckTypeCode = strTruckTypeCode;
            mode.SaleFeeTypeCode = strSaleFeeTypeCode;
            mode.Status = "1";
            mode.Remark = strRemark;

            mode.ControlItemCode =  dtsale.Rows[i]["Code"].ToString().Trim();
            TextBox txtFee = this.DataGrid1.Items[i].Cells[1].FindControl("txtTc") as TextBox;
            if (txtFee.Text != "" && txtFee.Text != null)
            {
                mode.Fee = decimal.Parse(txtFee.Text.Trim());
            }
            else
            {
                mode.Fee = decimal.Parse("0");
            }

            mode.Type = "1";
            list.Add(mode);

        }


        //期初分配
        if (!this.txtncfp.Text.Trim().Equals(""))
        {
            mode = new T_RebatesStandard();
            mode.DeptCode = strDeptCode;
            mode.EffectiveDateFrm = strEffectiveDateFrm;
            mode.EffectiveDateTo = strEffectiveDateTo;
            mode.TruckTypeCode = strTruckTypeCode;
            mode.SaleFeeTypeCode = strSaleFeeTypeCode;
            mode.Status = "1";
            mode.ControlItemCode = "期初分配";
            mode.Remark = strRemark;
            if (this.txtncfp.Text.Trim() != "" && this.txtncfp.Text.Trim() != null)
            {
                mode.Fee = decimal.Parse(this.txtncfp.Text.Trim());
            }
            else
            {
                mode.Fee = decimal.Parse("0");
            }

            mode.Type = "0";
            list.Add(mode);
        }
        DataTable dt = server.RunQueryCmdToTable("select Code, CName from  T_ControlItem");
        int iCount = this.Repeater1.Items.Count;
        //配置项
        for (int i = 0; i < iCount; i++)
        {
            mode = new T_RebatesStandard();
            mode.DeptCode = strDeptCode;
            mode.EffectiveDateFrm = strEffectiveDateFrm;
            mode.EffectiveDateTo = strEffectiveDateTo;
            mode.TruckTypeCode = strTruckTypeCode;
            mode.SaleFeeTypeCode = strSaleFeeTypeCode;
            mode.Status = "1";
            mode.Remark = strRemark;
            mode.Type = "2";
            mode.ControlItemCode = dt.Rows[i]["Code"].ToString();
            TextBox txtFee = this.Repeater1.Items[i].Cells[1].FindControl("txtpz") as TextBox;
            if (txtFee.Text != "" && txtFee.Text != null)
            {
                mode.Fee = decimal.Parse(txtFee.Text.Trim());
            }
            else
            {
                mode.Fee = decimal.Parse("0");
            }

            list.Add(mode);
        }
        return list;


    }
    //添加
    protected void btn_test_Click(object sender, EventArgs e)
    {
        string msg = "";
        bool flg = true;
        RebatesStandardBLL spebll = new RebatesStandardBLL();
        IList<T_RebatesStandard> Ilistmode = getmodel(out flg);
        if (flg == true)
        {
            int iRel = spebll.Insert(Ilistmode, out msg);
            if (iRel > 0)
            {
               
                //向note表中加记录
                  ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
                  //  ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
                    clear();
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
        this.txtCartype.Value = "";
        this.txtbz.Text = "";
        this.txtdept.Value = "";
        this.txtfylb.Value = "";
        this.txtncfp.Text = "";
        this.txtedtime.Text = "";
        this.txtbgtime.Text = "";
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

    protected void btn_client_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"sucess\";self.close();", true);
      //  ClientScript.RegisterStartupScript(this.GetType(), "", "window.close();", true);

    }
}
