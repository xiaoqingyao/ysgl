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
using Bll.Zichan;
using Models;
using System.Collections.Generic;
using Bll.SaleBill;
using Dal.SysDictionary;
using System.Text;

public partial class ZiChan_ZiChanGuanLi_CaiGouShenQingDetail : System.Web.UI.Page
{
    ZiChan_CaiGouShenQingBll bll = new ZiChan_CaiGouShenQingBll();
    Bll.Bills.BillMainBLL bllmain = new Bll.Bills.BillMainBLL();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ViewBLL viewbll = new ViewBLL();
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
                Response.Cache.SetSlidingExpiration(true);
                Response.Cache.SetNoStore();
                Bind();
            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetuserAll());
        }
    }
    //页面一进入时的绑定
    private void Bind()
    {

        if (Request.QueryString["type"] != null)
        {
            string type = Request.QueryString["type"].ToString();
            if (type == "Add")
            {
                ZiChan_CaiGouShenQing model = new ZiChan_CaiGouShenQing();


                txtsqrq.Text = DateTime.Now.ToString("yyyy-MM-dd");//申请编号
                model.CaiGouShuoMing = this.txtExplina.Text.ToString();
                model.GuiGe = this.txtggxh.Text.ToString().Trim();
                string strusercodename = server.GetCellValue("select '['+userCode+']'+userName  from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
                this.txtsqname.Text = strusercodename;
                this.txtjbname.Text = strusercodename;
                btn_Shtg.Visible = false;
                btn_Shbh.Visible = false;

                if (isTopDept("y", Session["userCode"].ToString().Trim()))
                {
                    string dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "') and IsSell='Y'");
                    this.lasqbm.Text = dept;

                }
                else
                {
                    //所在部门
                    string Dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "') and IsSell='Y'");
                    //上级部门
                    string sjDept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "')) and IsSell='Y'");
                    this.lasqbm.Text = Dept;
                }


                //生成编号
                string lscgCode = new DataDicDal().GetYbbxBillName("zccgsq", DateTime.Now.ToString("yyyyMMdd"), 1);
                if (lscgCode == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
                    this.btn_bc.Visible = false;
                }
                else
                {
                    this.laSqbh.Text = lscgCode;
                }
            }
            if (type == "Edit")
            {
                if (Request.QueryString["bh"] != null)
                {
                    string bh = Request.QueryString["bh"].ToString();
                    Bill_Main billmain = bllmain.GetModel(bh);
                    ZiChan_CaiGouShenQing zcmodel = new ZiChan_CaiGouShenQing();
                    zcmodel = bll.getmodel(bh);

                    laSqbh.Text = billmain.BillCode;
                    txtsqrq.Text = Convert.ToDateTime(billmain.BillDate).ToString("yyyy-MM-dd");
                    if (billmain.BillDept != null && billmain.BillDept != "")
                    {
                        string strsql = "select '['+deptcode+']'+deptName from bill_departments where deptcode='" + billmain.BillDept + "'";
                        string strsqname = server.GetCellValue(strsql);
                        lasqbm.Text = strsqname;
                    }
                    if (zcmodel.ShenQingRenCode != null && zcmodel.ShenQingRenCode != "")
                    {

                        string strsql = "select '['+userCode+']'+userName as usernamecode from bill_users where userCode='" + zcmodel.ShenQingRenCode + "'";
                        string strsqname = server.GetCellValue(strsql);
                        txtsqname.Text = strsqname;
                    }
                    if (zcmodel.JingBanRenCode != null && zcmodel.JingBanRenCode != "")
                    {

                        string strsql = "select '['+userCode+']'+userName as usernamecode from bill_users where userCode='" + zcmodel.JingBanRenCode + "'";
                        string strjbname = server.GetCellValue(strsql);
                        txtjbname.Text = strjbname;
                    }
                    this.txtcgzcname.Text = zcmodel.ZaiChanName.ToString();
                    this.txtprice.Text = zcmodel.JiaGe.ToString();
                    this.txtcount.Text = zcmodel.ShuLiang.ToString();
                    this.txtggxh.Text = zcmodel.GuiGe.ToString();
                    this.txtExplina.Text = zcmodel.CaiGouShuoMing.ToString();

                    btn_Shbh.Visible = false;
                    btn_Shtg.Visible = false;



                }
            }
            if (type == "look" || type == "addInvoice")
            {
                if (Request.QueryString["bh"] != null)
                {
                    string bh = Request.QueryString["bh"].ToString();
                    Bill_Main billmain = bllmain.GetModel(bh);
                    ZiChan_CaiGouShenQing zcmodel = new ZiChan_CaiGouShenQing();
                    zcmodel = bll.getmodel(bh);

                    laSqbh.Text = billmain.BillCode;
                    txtsqrq.Text = Convert.ToDateTime(billmain.BillDate).ToString("yyyy-MM-dd");
                    if (billmain.BillDept != null && billmain.BillDept != "")
                    {
                        string strsql = "select '['+deptcode+']'+deptName from bill_departments where deptcode='" + billmain.BillDept + "'";
                        string strsqname = server.GetCellValue(strsql);
                        lasqbm.Text = strsqname;
                    }
                    if (zcmodel.ShenQingRenCode != "" && zcmodel.ShenQingRenCode != null)
                    {

                        string strsql = "select '['+userCode+']'+userName as usernamecode from bill_users where userCode='" + zcmodel.ShenQingRenCode + "'";
                        string strsqname = server.GetCellValue(strsql);
                        txtsqname.Text = strsqname;
                    }
                    if (zcmodel.JingBanRenCode != null && zcmodel.JingBanRenCode != "")
                    {

                        string strsql = "select '['+userCode+']'+userName as usernamecode from bill_users where userCode='" + zcmodel.JingBanRenCode + "'";
                        string strjbname = server.GetCellValue(strsql);
                        txtjbname.Text = strjbname;
                    }
                    this.txtcgzcname.Text = zcmodel.ZaiChanName.ToString();
                    this.txtprice.Text = zcmodel.JiaGe.ToString();
                    this.txtcount.Text = zcmodel.ShuLiang.ToString();
                    this.txtggxh.Text = zcmodel.GuiGe.ToString();
                    this.txtExplina.Text = zcmodel.CaiGouShuoMing.ToString();
                    laSqbh.Text = billmain.BillCode;
                    txtsqrq.Text = Convert.ToDateTime(billmain.BillDate).ToString("yyyy-MM-dd");

                }

                this.txtsqrq.Enabled = false;
                this.txtExplina.Enabled = false;
                btn_bc.Visible = false;
                this.btn_Shtg.Visible = false;
                this.btn_Shbh.Visible = false;

            }
            if (type == "audit")
            {
                if (Request.QueryString["bh"] != null)
                {
                    string bh = Request.QueryString["bh"].ToString();
                    Bill_Main billmain = bllmain.GetModel(bh);
                    IList<ZiChan_CaiGouShenQing> kpsqList = bll.GetListByCode(bh);
                    laSqbh.Text = billmain.BillCode;
                    txtsqrq.Text = Convert.ToDateTime(billmain.BillDate).ToString("yyyy-MM-dd");

                    ZiChan_CaiGouShenQing zcmodel = new ZiChan_CaiGouShenQing();
                    zcmodel = bll.getmodel(bh);

                    laSqbh.Text = billmain.BillCode;
                    txtsqrq.Text = Convert.ToDateTime(billmain.BillDate).ToString("yyyy-MM-dd");
                    if (billmain.BillDept != null && billmain.BillDept != "")
                    {
                        string strsql = "select '['+deptcode+']'+deptName from bill_departments where deptcode='" + billmain.BillDept + "'";
                        string strsqname = server.GetCellValue(strsql);
                        lasqbm.Text = strsqname;
                    }
                    if (zcmodel.ShenQingRenCode != "" && zcmodel.ShenQingRenCode != null)
                    {

                        string strsql = "select '['+userCode+']'+userName as usernamecode from bill_users where userCode='" + zcmodel.ShenQingRenCode + "'";
                        string strsqname = server.GetCellValue(strsql);
                        txtsqname.Text = strsqname;
                    }
                    if (zcmodel.JingBanRenCode != null && zcmodel.JingBanRenCode != "")
                    {

                        string strsql = "select '['+userCode+']'+userName as usernamecode from bill_users where userCode='" + zcmodel.JingBanRenCode + "'";
                        string strjbname = server.GetCellValue(strsql);
                        txtjbname.Text = strjbname;
                    }
                    this.txtcgzcname.Text = zcmodel.ZaiChanName.ToString();
                    this.txtprice.Text = zcmodel.JiaGe.ToString();
                    this.txtcount.Text = zcmodel.ShuLiang.ToString();
                    this.txtggxh.Text = zcmodel.GuiGe.ToString();
                    this.txtExplina.Text = zcmodel.CaiGouShuoMing.ToString();

                }

                this.txtsqrq.Enabled = false;
                this.txtExplina.Enabled = false;

                this.btn_Shbh.Visible = true;
                this.btn_Shtg.Visible = true;
                this.btn_bc.Visible = false;
            }

        }
        //this.txtsqrq.Attributes.Add("onfocus", "javascript:setday(this);");
    }

    private string GetuserAll()
    {
        DataSet ds = server.GetDataSet("select '['+userCode+']'+userName as usernamecode from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["usernamecode"]));
            arry.Append("',");
        }
        string script;

        try
        {
            script = arry.ToString().Substring(0, arry.Length - 1);
        }
        catch (Exception)
        {
            script = "";
        }


        return script;
    }
    /// <summary>
    /// 查询是不是二级单位
    /// </summary>
    /// <param name="strus">是人员CODE？y:n</param>
    /// <param name="usercode">人员CODE</param>
    /// <returns></returns>
    public bool isTopDept(string strus, string usercode)
    {
        string sql = "";
        if (strus == "y")
        {
            sql = " select count(1) from (select deptCode from bill_departments where IsSell='Y'and sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "') ";
        }
        else
        {
            sql = " select count(1) from (select deptCode from bill_departments where IsSell='Y'and sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
        }
        if (server.GetCellValue(sql) == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e == null)
        //{
        //    return;
        //}
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    string strIsSpecialApp = e.Row.Cells[5].Text;
        //    string strTruckCode = e.Row.Cells[2].Text;
        //    string strSpecialCode = "";

        //}
    }
    //保存
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["type"] != null)
        {
            string type = Request.QueryString["type"].ToString();
            if (type == "Add")
            {
                Bill_Main main = new Bill_Main();
                main.BillCode = laSqbh.Text;
                main.BillName = "资产采购申请";
                main.FlowId = "zccgsq";
                main.StepId = "-1";
                main.BillUser = Session["userCode"].ToString().Trim();

                main.BillDate = Convert.ToDateTime(txtsqrq.Text);

                string strprice = this.txtprice.Text;
                if (!strprice.Equals(""))
                {
                    decimal decprice = 0;
                    if (decimal.TryParse(strprice, out decprice))
                    {
                        main.BillJe = decprice;
                    }
                    else
                    {
                        showMessage("单价输入不合法！", false, ""); return;
                    }
                }
                else
                {
                    main.BillJe = 0;
                }
                string strjbdeptname = lasqbm.Text.ToString();
                try
                {
                    strjbdeptname = strjbdeptname.Substring(1, strjbdeptname.IndexOf("]") - 1);

                }
                catch (Exception)
                {

                    strjbdeptname="";
                }

               
                    main.BillDept = strjbdeptname;
                

                ZiChan_CaiGouShenQing model = new ZiChan_CaiGouShenQing();
                model.MainCode = laSqbh.Text.Trim();
                txtsqrq.Text = DateTime.Now.ToString("yyyy-MM-dd");//申请编号
                string strjbname = txtjbname.Text.ToString();
                try
                {
                    strjbname = strjbname.Substring(1, strjbname.IndexOf("]") - 1);
                }
                catch (Exception)
                {

                    strjbname = "";
                }

                if (strjbname.Equals(""))//经办人
                {
                    showMessage("经办人不能为空！", false, "");
                    return;

                }
                model.JingBanRenCode = strjbname;
                string strsqname = txtsqname.Text.ToString();
                try
                {
                    strsqname = strsqname.Substring(1, strsqname.IndexOf("]") - 1);
                }
                catch (Exception)
                {
                    strsqname = "";
                }

                if (strsqname.Equals(""))//申请人
                {
                    showMessage("申请人不能为空！", false, "");
                    return;

                }
                model.ShenQingRenCode = strsqname;
                string strzcname = this.txtcgzcname.Text;
                if (strzcname.Equals(""))
                {
                    showMessage("资产名称不能为空！", false, "");
                    return;
                }
                model.ZaiChanName = strzcname;//名称
                string strggxh = this.txtggxh.Text.ToString().Trim();
                if (strggxh.Equals(""))
                {
                    showMessage("规格型号不能为空！", false, "");
                    return;
                }
                model.GuiGe = strggxh;




                string stramout = this.txtcount.Text.Trim();
                if (!stramout.Equals(""))
                {
                    int iamout = 0;
                    if (int.TryParse(stramout, out iamout))
                    {
                        model.ShuLiang = iamout;
                    }
                    else
                    {
                        showMessage("数量输入不合法！", false, ""); return;
                    }
                }
                else
                {
                    model.ShuLiang = 0;
                }




                string strpric = this.txtprice.Text.Trim();

                if (!strpric.Equals(""))
                {
                    decimal isdecprice = 0;
                    if (decimal.TryParse(strpric, out isdecprice))
                    {
                        model.JiaGe = isdecprice;
                    }
                    else
                    {
                        showMessage("价格输入不合法！", false, "");
                        return;
                    }


                }
                else
                {
                    model.JiaGe = 0;
                }



                string strcgsm = this.txtExplina.Text.ToString().Trim();
                if (strcgsm.Equals(""))
                {
                    showMessage("采购说明不能为空！", false, "");
                    return;
                }
                model.CaiGouShuoMing = this.txtExplina.Text.ToString();
                model.Note1 = "";
                model.Note2 = "";
                model.Note3 = "";
                model.Note4 = "";
                model.Note5 = "";
                model.Note6 = "";
                model.Note7 = "";
                model.Note8 = "";
                model.Note9 = "";
                model.Note10 = "";

                if (bll.Edit(main, model))
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue='1';window.close();", true);


                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);

                }

            }
            if (type == "Edit" || type == "addInvoice")
            {

                Bill_Main main = new Bill_Main();
                main.BillCode = laSqbh.Text;
                main.BillName = "资产采购申请";
                main.FlowId = "zccgsq";
                main.StepId = "-1";
                main.BillUser = Session["userCode"].ToString().Trim();
                main.BillDate = Convert.ToDateTime(txtsqrq.Text);
                string strprice = this.txtprice.Text;
                if (!strprice.Equals(""))
                {
                    decimal decprice = 0;
                    if (decimal.TryParse(strprice, out decprice))
                    {
                        main.BillJe = decprice;
                    }
                    else
                    {
                        showMessage("单价输入不合法！", false, ""); return;
                    }
                }
                else
                {
                    main.BillJe = 0;
                }
                string strjbdeptname = lasqbm.Text.ToString();
                try
                {
                    strjbdeptname = strjbdeptname.Substring(1, strjbdeptname.IndexOf("]") - 1);
                }
                catch (Exception)
                {

                    strjbdeptname = "";
                }


                if (!strjbdeptname.Equals(""))//申请部门
                {

                    main.BillDept = strjbdeptname;
                }
                ZiChan_CaiGouShenQing model = new ZiChan_CaiGouShenQing();
                if (Request.QueryString["bh"].ToString() != null && Request.QueryString["bh"].ToString() != "")
                {
                    model.MainCode = laSqbh.Text;
                    if (type == "Edit")
                    {
                        int row = bll.Delete(main, Request.QueryString["bh"].ToString());

                    }
                    txtsqrq.Text = DateTime.Now.ToString("yyyy-MM-dd");//申请编号
                    string strjbname = txtjbname.Text.ToString();
                    try
                    {
                        strjbname = strjbname.Substring(1, strjbname.IndexOf("]") - 1);
                    }
                    catch (Exception)
                    {

                        strjbname = "";
                    }

                    if (strjbname.Equals(""))//经办人
                    {
                        showMessage("经办人不能为空！", false, "");
                        return;

                    }
                    model.JingBanRenCode = strjbname;
                    string strsqname = txtsqname.Text.ToString();
                    try
                    {
                        strsqname = strsqname.Substring(1, strsqname.IndexOf("]") - 1);

                    }
                    catch (Exception)
                    {

                        strsqname = "";
                    }

                    if (strsqname.Equals(""))//申请人
                    {
                        showMessage("申请人不能为空！", false, "");
                        return;

                    }
                    model.ShenQingRenCode = strsqname;
                    string strzcname = this.txtcgzcname.Text;
                    if (strzcname.Equals(""))
                    {
                        showMessage("资产名称不能为空！", false, "");
                        return;
                    }
                    model.ZaiChanName = strzcname;//名称
                    string strggxh = this.txtggxh.Text.ToString().Trim();
                    if (strggxh.Equals(""))
                    {
                        showMessage("规格型号不能为空！", false, "");
                        return;
                    }
                    model.GuiGe = strggxh;




                    string stramout = this.txtcount.Text.Trim();
                    if (!stramout.Equals(""))
                    {
                        int iamout = 0;
                        if (int.TryParse(stramout, out iamout))
                        {
                            model.ShuLiang = iamout;
                        }
                        else
                        {
                            showMessage("数量输入不合法！", false, ""); return;
                        }
                    }
                    else
                    {
                        model.ShuLiang = 0;
                    }

                    string strpric = this.txtprice.Text.Trim();

                    if (!strpric.Equals(""))
                    {
                        decimal isdecprice = 0;
                        if (decimal.TryParse(strpric, out isdecprice))
                        {
                            model.JiaGe = isdecprice;
                        }
                        else
                        {
                            showMessage("价格输入不合法！", false, "");
                            return;
                        }
                    }
                    else
                    {
                        model.JiaGe = 0;
                    }



                    string strcgsm = this.txtExplina.Text.ToString().Trim();
                    if (strcgsm.Equals(""))
                    {
                        showMessage("采购说明不能为空！", false, "");
                        return;
                    }
                    model.CaiGouShuoMing = this.txtExplina.Text.ToString();
                    model.Note1 = "";
                    model.Note2 = "";
                    model.Note3 = "";
                    model.Note4 = "";
                    model.Note5 = "";
                    model.Note6 = "";
                    model.Note7 = "";
                    model.Note8 = "";
                    model.Note9 = "";
                    model.Note10 = "";
                }


                if (bll.Edit(main, model))
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue='1';window.close();", true);


                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);

                }
            }
        }
    }
    //判断是不是空格
    protected string GetGwStr(string str)
    {
        if (str == "&nbsp;")
        {
            return "";
        }
        else
        {
            return str;
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