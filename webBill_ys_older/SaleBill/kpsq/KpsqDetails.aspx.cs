using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dal.SysDictionary;
using Models;
using Bll.UserProperty;
using System.Data;
using System.Text;
using Bll.SaleBill;
using System.Data.SqlClient;
using System.Windows.Forms;
using Bll.TruckType;

public partial class SaleBill_kpsq_KpsqDetails : System.Web.UI.Page
{
    Bll.Bills.T_BillingApplicationBll bll = new Bll.Bills.T_BillingApplicationBll();
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
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
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
                IList<T_BillingApplication> cxb = new List<T_BillingApplication>();
                cxb.Add(new T_BillingApplication());
                GridView1.DataSource = cxb;
                GridView1.DataBind();
                txtsqrq.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
                string lscgCode = new DataDicDal().GetYbbxBillName("kpsq", DateTime.Now.ToString("yyyyMMdd"), 1);
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
                    IList<T_BillingApplication> kpsqList = bll.GetListByCode(bh);
                    laSqbh.Text = billmain.BillCode;
                    txtsqrq.Text = Convert.ToDateTime(billmain.BillDate).ToString("yyyy-MM-dd");
                    lasqbm.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + billmain.BillDept + "'");
                    List<T_BillingApplication> cxblist = new List<T_BillingApplication>();
                    btn_Shbh.Visible = false;
                    btn_Shtg.Visible = false;
                    if (kpsqList.Count == 0)
                    {
                        //如果一条也没有的话就绑定加一条空的
                        cxblist.Add(new T_BillingApplication());
                    }
                    else
                    {
                        //循环增加
                        foreach (var i in kpsqList)
                        {
                            T_BillingApplication cx = new T_BillingApplication();
                            cx.TruckCode = i.TruckCode;
                            cx.Note2 = i.Note2;
                            cx.IsJC = i.IsJC == "1" ? "是" : "否";
                            cx.IsSpApp = i.IsSpApp == "1" ? "是" : "否";
                            cx.Note3 = i.Note3 == "1" ? "是" : "否";
                            cx.DealersName = i.DealersName;
                            cxblist.Add(cx);
                        }
                        txtExplina.Text = kpsqList.First().Explain;
                        //如果有附件的话
                        if (kpsqList.First().Note1 != "")
                        {
                            Lafilename.Text = kpsqList.First().Note1;
                            FileUpload1.Visible = false;
                            btn_sc.Text = "修改附件";
                            hiddFileDz.Value = kpsqList.First().AttachmentUrl;
                            Literal1.Text = "<a href='../Upload/kpsq/" + System.IO.Path.GetFileName(hiddFileDz.Value) + @"' target='_blank'  >下载</a> ";
                        }
                        else
                        {
                            //如果没有附件的话
                            btn_sc.Text = "上 传";
                            Lafilename.Text = "";
                            FileUpload1.Visible = true;
                            hiddFileDz.Value = "";
                        }

                    }
                    GridView1.DataSource = cxblist;
                    GridView1.DataBind();

                }
            }
            if (type == "look" || type == "addInvoice")
            {
                if (Request.QueryString["bh"] != null)
                {
                    string bh = Request.QueryString["bh"].ToString();
                    Bill_Main billmain = bllmain.GetModel(bh);
                    IList<T_BillingApplication> kpsqList = bll.GetListByCode(bh);
                    laSqbh.Text = billmain.BillCode;
                    txtsqrq.Text = Convert.ToDateTime(billmain.BillDate).ToString("yyyy-MM-dd");
                    lasqbm.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + billmain.BillDept + "'");
                    List<T_BillingApplication> cxblist = new List<T_BillingApplication>();
                    //如果一条也没有的话就绑定一条空的
                    if (kpsqList.Count == 0)
                    {
                        cxblist.Add(new T_BillingApplication());
                    }
                    else
                    {
                        foreach (var i in kpsqList)
                        {
                            T_BillingApplication cx = new T_BillingApplication();
                            cx.TruckCode = i.TruckCode;
                            cx.Note2 = i.Note2;
                            cx.DealersName = i.DealersName;
                            cx.IsJC = i.IsJC == "1" ? "是" : "否";
                            cx.IsSpApp = i.IsSpApp == "1" ? "是" : "否";
                            cx.Note3 = i.Note3 == "1" ? "是" : "否";
                            cxblist.Add(cx);
                        }
                        txtExplina.Text = kpsqList.First().Explain;
                        btn_sc.Visible = false;
                        AddDiv.Style.Add("display", "none");
                        btn_Addcx.Visible = false;
                        btn_Del.Visible = false;
                        tsxx.Visible = false;
                        btn_bc.Visible = false;
                        if (type == "look")
                        {
                            btn_bc.Visible = btn_Shtg.Visible = btn_Shbh.Visible = false;
                        }
                        //如果有附件的话
                        if (kpsqList.First().Note1 != "")
                        {
                            Lafilename.Text = kpsqList.First().Note1;
                            FileUpload1.Visible = false;
                            hiddFileDz.Value = kpsqList.First().AttachmentUrl;

                            Literal1.Text = "<a href='../Upload/kpsq/" + System.IO.Path.GetFileName(hiddFileDz.Value) + @"' target='_blank'  >下载</a> ";
                            //<input  type='button' class='baseButton' value='下 载'/>
                        }
                        else
                        {
                            //如果没有附件的话
                            Lafilename.Text = "无附件";
                            FileUpload1.Visible = false;
                        }

                    }
                    GridView1.DataSource = cxblist;
                    GridView1.DataBind();

                }

                this.txtsqrq.Enabled = false;
                this.txtExplina.Enabled = false;
                this.lasqbm.Enabled = false;

            }
            if (type == "audit")
            {
                if (Request.QueryString["bh"] != null)
                {
                    string bh = Request.QueryString["bh"].ToString();
                    Bill_Main billmain = bllmain.GetModel(bh);
                    IList<T_BillingApplication> kpsqList = bll.GetListByCode(bh);
                    laSqbh.Text = billmain.BillCode;
                    txtsqrq.Text = Convert.ToDateTime(billmain.BillDate).ToString("yyyy-MM-dd");
                    lasqbm.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + billmain.BillDept + "'");
                    List<T_BillingApplication> cxblist = new List<T_BillingApplication>();
                    //如果一条也没有的话就绑定一条空的
                    if (kpsqList.Count == 0)
                    {
                        cxblist.Add(new T_BillingApplication());
                    }
                    else
                    {
                        foreach (var i in kpsqList)
                        {
                            T_BillingApplication cx = new T_BillingApplication();
                            cx.TruckCode = i.TruckCode;
                            cx.Note2 = i.Note2;
                            cx.DealersName = i.DealersName;
                            cx.IsJC = i.IsJC == "1" ? "是" : "否";
                            cx.Note3 = i.Note3 == "1" ? "是" : "否";
                            cxblist.Add(cx);
                        }
                        txtExplina.Text = kpsqList.First().Explain;
                        btn_sc.Visible = false;
                        AddDiv.Style.Add("display", "none");
                        btn_Addcx.Visible = false;
                        btn_Del.Visible = false;
                        tsxx.Visible = false;
                        btn_bc.Visible = false;
                        if (type == "addInvoice")
                        {
                            btn_bc.Visible = true;
                            btn_Shtg.Visible = btn_Shbh.Visible = false;
                        }
                        //如果有附件的话
                        if (kpsqList.First().Note1 != "")
                        {
                            Lafilename.Text = kpsqList.First().Note1;
                            FileUpload1.Visible = false;
                            hiddFileDz.Value = kpsqList.First().AttachmentUrl;

                            Literal1.Text = "<a href='../Upload/kpsq/" + System.IO.Path.GetFileName(hiddFileDz.Value) + @"' target='_blank'  >下载</a> ";
                            //<input  type='button' class='baseButton' value='下 载'/>
                        }
                        else
                        {
                            //如果没有附件的话
                            Lafilename.Text = "无附件";
                            FileUpload1.Visible = false;
                        }

                    }
                    GridView1.DataSource = cxblist;
                    GridView1.DataBind();

                }

                this.txtsqrq.Enabled = false;
                this.txtExplina.Enabled = false;
                this.lasqbm.Enabled = false;
                this.btn_Shbh.Visible = true;
                this.btn_Shtg.Visible = true;
            }

        }
        //this.txtsqrq.Attributes.Add("onfocus", "javascript:setday(this);");
    }

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
        if (e == null)
        {
            return;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strIsSpecialApp = e.Row.Cells[5].Text;
            string strTruckCode = e.Row.Cells[2].Text;
            string strSpecialCode = "";
            if (bll.isSpecialRebatesApp(strTruckCode, out strSpecialCode))
            {
                e.Row.Cells[5].Text = "是" + "<a href='#' style=\"color:Blue\"  onclick=\"specialAppCode('" + strSpecialCode + "')\">" + strSpecialCode + "</a>";
            }
        }
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
                main.BillName = "开票申请单";
                main.FlowId = "kpsq";
                main.StepId = "-1";
                main.BillUser = Session["userCode"].ToString().Trim();
                main.BillDate = Convert.ToDateTime(txtsqrq.Text);
                if (lasqbm.Text != "" && lasqbm.Text != null)
                {
                    string strDeptText = lasqbm.Text.Trim();
                    main.BillDept = strDeptText.Substring(1, strDeptText.IndexOf("]") - 1); //server.GetCellValue("select userdept from bill_users where usercode='" + main.BillUser + "'");

                }
                else
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门不能为空');", true);
                    lasqbm.Focus();
                    return;

                }
                main.BillJe = 0;
                IList<T_BillingApplication> kpsqLists = new List<T_BillingApplication>();
                for (int s = 0; s < GridView1.Rows.Count; s++)
                {
                    if (GridView1.Rows[s].Cells[1].Text != "" && GridView1.Rows[s].Cells[1].Text != "&nbsp;")
                    {
                        T_BillingApplication kpsq = new T_BillingApplication();
                        kpsq.Code = laSqbh.Text;
                        kpsq.BillMainCode = laSqbh.Text;
                        if (bll.isExist(GridView1.Rows[s].Cells[2].Text.Trim()))
                        {

                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('车架号为：" + GridView1.Rows[s].Cells[2].Text.Trim() + "的车辆已经做过开票申请！');", true);
                            return;

                        }
                        kpsq.TruckCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);

                        kpsq.Note2 = GetGwStr(GridView1.Rows[s].Cells[1].Text);

                        kpsq.SaleDeptCode = main.BillDept;
                        kpsq.AppDate = txtsqrq.Text;
                        kpsq.SysPersionCode = Session["userCode"].ToString().Trim();
                        kpsq.SysDateTime = DateTime.Now.ToString("yyyy-MM-dd");
                        kpsq.Explain = txtExplina.Text;
                        kpsq.DealersName = GetGwStr(GridView1.Rows[s].Cells[3].Text);
                        kpsq.IsJC = GridView1.Rows[s].Cells[4].Text == "是" ? "1" : "0";
                        kpsq.IsSpApp = GridView1.Rows[s].Cells[5].Text == "否" ? "0" : "1";
                        kpsq.Note3 = GridView1.Rows[s].Cells[6].Text == "是" ? "1" : "0";
                        kpsq.AttachmentUrl = hiddFileDz.Value;
                        kpsq.Note1 = Lafilename.Text;
                        kpsqLists.Add(kpsq);
                    }
                }

                if (kpsqLists.Count > 0)
                {
                    if (bll.Add(main, kpsqLists))
                    {
                        //kpsqLists.Count为明细  判断明细是不是有
                        if (kpsqLists.Count > 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue='1';window.close();", true);
                        }

                    }
                }
                else
                {
                    tsxx.InnerHtml = "明细至少要有一行";
                    return;
                }

            }
            if (type == "Edit" || type == "addInvoice")
            {

                Bill_Main main = new Bill_Main();
                main.BillCode = laSqbh.Text;
                main.BillName = "开票申请单";
                main.FlowId = "kpsq";
                main.StepId = "-1";
                main.BillUser = Session["userCode"].ToString().Trim();
                main.BillDate = Convert.ToDateTime(txtsqrq.Text);
                string strDeptText = lasqbm.Text.Trim();
                main.BillDept = strDeptText.Substring(1, strDeptText.IndexOf("]") - 1); //server.GetCellValue("select userdept from bill_users where usercode='" + main.BillUser + "'");
                main.BillJe = 0;
                IList<T_BillingApplication> kpsqLists = new List<T_BillingApplication>();

                if (Request.QueryString["bh"].ToString() != null && Request.QueryString["bh"].ToString() != "")
                {


                    for (int s = 0; s < GridView1.Rows.Count; s++)
                    {
                        if (GridView1.Rows[s].Cells[1].Text != "" && GridView1.Rows[s].Cells[1].Text != "&nbsp;")
                        {
                            T_BillingApplication kpsq = new T_BillingApplication();
                            kpsq.Code = laSqbh.Text;
                            kpsq.BillMainCode = laSqbh.Text;
                            if (type == "Edit")
                            {
                                int row = bll.Delete(Request.QueryString["bh"].ToString());
                                //if (bll.isExist(GridView1.Rows[s].Cells[2].Text.Trim()))
                                //{
                                //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + GridView1.Rows[s].Cells[2].Text.Trim() + "的车辆已经做过开票申请， 请将其删除点击保存！');", true);
                                //    return;

                                //}
                            }

                            kpsq.TruckCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);
                            kpsq.Note2 = GetGwStr(GridView1.Rows[s].Cells[1].Text);
                            kpsq.DealersName = GetGwStr(GridView1.Rows[s].Cells[3].Text);
                            kpsq.SaleDeptCode = main.BillDept;//GetGwStr(GridView1.Rows[s].Cells[2].Text);
                            kpsq.AppDate = txtsqrq.Text;
                            kpsq.SysPersionCode = Session["userCode"].ToString().Trim();
                            kpsq.SysDateTime = DateTime.Now.ToString("yyyy-MM-dd");
                            kpsq.Explain = txtExplina.Text;
                            kpsq.IsJC = GridView1.Rows[s].Cells[4].Text == "是" ? "1" : "0";
                            kpsq.IsSpApp = GridView1.Rows[s].Cells[5].Text == "是" ? "1" : "0";
                            kpsq.Note3 = GridView1.Rows[s].Cells[6].Text == "是" ? "1" : "0";
                            kpsq.AttachmentUrl = hiddFileDz.Value;
                            kpsq.Note1 = Lafilename.Text;
                            kpsqLists.Add(kpsq);
                        }
                    }
                }
                if (bll.Edit(main, kpsqLists))
                {
                    //kpsqLists.Count为明细  判断明细是不是有
                    if (kpsqLists.Count > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue='1';window.close();", true);
                    }
                    else
                    {
                        tsxx.InnerHtml = "明细至少要有一行";
                        return;
                    }
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
    //增加一行
    protected void btn_Addcx_Click(object sender, EventArgs e)
    {
        string cjh = txtcjh.Text;
        string xsdh = txtxsdh.Text;
        string isjc = checkjc.Checked ? "是" : "否";
        string isapll = checksp.Checked ? "是" : "否";
        string isdf = checkdf.Checked ? "是" : "否";
        string strDealersName = txtDealersName.Text.Trim();

        IList<T_BillingApplication> cxblist = new List<T_BillingApplication>();
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            T_BillingApplication applicmodel = new T_BillingApplication();
            if (GridView1.Rows[s].Cells[1].Text != "" && GridView1.Rows[s].Cells[1].Text != "&nbsp;")
            {
                applicmodel.Note2 = GetGwStr(GridView1.Rows[s].Cells[1].Text);
                if (cjh == GetGwStr(GridView1.Rows[s].Cells[2].Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + cjh + "车架号已经存在！');", true);
                    this.txtcjh.Text = "";
                    this.txtxsdh.Text = "";
                    this.txtDealersName.Text = "";
                    checkjc.Checked = false;
                    checksp.Checked = false;
                    return;
                }
                applicmodel.TruckCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);
                applicmodel.DealersName = GetGwStr(GridView1.Rows[s].Cells[3].Text);
                applicmodel.IsJC = GridView1.Rows[s].Cells[4].Text == "是" ? "是" : "否";
                applicmodel.IsSpApp = GridView1.Rows[s].Cells[5].Text == "是" ? "是" : "否";
                applicmodel.Note3 = GridView1.Rows[s].Cells[6].Text == "是" ? "是" : "否";
                cxblist.Add(applicmodel);
            }
        }
        if (cjh == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('车架号不能为空！');", true);
            this.txtcjh.Focus();
            return;
        }
        if (strDealersName == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('经销商不能为空！');", true);
            this.txtDealersName.Focus();
            return;
        }
        if (cjh.Trim() != null)
        {
            string strcarcode = cjh.Trim();
            string strmag = "";
            string strsql = "select cxh from dbo.V_TruckMsg where cjh='" + strcarcode + "'";
            string strTrckCode = server.GetCellValue(strsql);//通过车架号找车型号 如果找不到“无效底盘号（车架号）”
            //判断该车辆有没有做过特殊返利
            if (bll.isSpecialRebatesApp(strcarcode, out strmag)) //判断该车辆有没有做过特殊返利 如果有
            {
                //判断是否批复
                if (!bll.ispf(strcarcode))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('车架号为" + strcarcode.Trim() + "的车辆为特殊返利车辆,但是返利标准还未批复！');", true);
                    return;
                }
            }
            if (strTrckCode != "" && strTrckCode != null)
            {
                if (bll.isExist(strcarcode))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('车架号为" + strcarcode + "的车辆已经做过开票申请！');", true);
                    this.txtcjh.Text = "";
                    this.txtxsdh.Text = "";
                    this.txtDealersName.Text = "";
                    this.checksp.Checked = false;
                    this.checkjc.Checked = false;
                    this.checkdf.Checked = false;
                    return;
                }
                //因为有车辆对应 所以多加了一步  再通过车型号找是否已经设置车辆类型对应
                string strTruckTypeNum = new T_TruckTypeCorrespondBLL().GetTruckTypeNumByFactCode(strTrckCode);
                if (strTruckTypeNum != null && strTruckTypeNum != "")//如果有对应
                {
                    //根据部门
                    string strdeptcode = lasqbm.Text.Trim();
                    if (strdeptcode!=null&&strdeptcode!="")
                    {
                        strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);

                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请先填写申请单位！');", true);
                        return;
                    }

                    //根据时间
                    string strdatetime = DateTime.Now.ToString("yyyy-MM-dd");
                    //销售过程必须是开票
                    string strxsgcode = server.GetCellValue("select Code from T_SaleProcess where Code='SPKP'");
                    if (strxsgcode==null||strxsgcode=="")//如果销售过程中不存在开票给予提示结束操作
                    {
                           ClientScript.RegisterStartupScript(this.GetType(), "", "alert('销售过程中没有开票 请到基础数据 销售过程中维护好开票再添加！');", true);
                          return;
                    }

                    if (!bll.isExistrebatekp(strTruckTypeNum, strdatetime, strdeptcode, strxsgcode))//判断对应上的车辆类型号是否有返利标准
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('编号为" + strTruckTypeNum.Trim() + "的车辆类型没有制定返利标准！');", true);
                    }
                }
                else
                {
                    //将车型号添加到对应表中
                    T_TruckTypeCorrespond model = new T_TruckTypeCorrespond();
                    T_TruckTypeCorrespondBLL trckbll = new T_TruckTypeCorrespondBLL();
                    string msg = "";
                    model.factTruckType = strTrckCode;
                    //判断是否已经有改车型号了
                    if (trckbll.isExistfact(strTrckCode))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该车架号对应的内部车型号" + strTrckCode.Trim() + "未设置过车辆类型对应，系统将该车型号添加到车辆类型对应的设置界面，请到对应界面设置！');", true);
                        return;
                    }
                    else
                    {
                        int row = trckbll.Add(model, out msg);
                        if (row <= 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('系统已记录该车型号失败！');", true);
                        }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该车架号对应的内部车型号" + strTrckCode.Trim() + "未设置过车辆类型对应，系统将该车型号添加到车辆类型对应的设置界面，请到对应界面设置！');", true);
                    return;
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('无效的车架号！');", true);
                this.txtcjh.Text = "";
                this.txtxsdh.Text = "";
                this.txtDealersName.Text = "";
                checkjc.Checked = false;
                this.checksp.Checked = false;
                this.checkdf.Checked = false;
                txtcjh.Focus();
                return;
            }
        }
        T_BillingApplication thiscx = new T_BillingApplication();
        thiscx.TruckCode = cjh;
        thiscx.DealersName = strDealersName;
        thiscx.Note2 = xsdh;
        thiscx.IsJC = checkjc.Checked ? "是" : "否";
        thiscx.IsSpApp = checksp.Checked ? "是" : "否";
        thiscx.Note3 = checkdf.Checked ? "是" : "否";
        cxblist.Add(thiscx);
        GridView1.DataSource = cxblist;
        GridView1.DataBind();
        txtcjh.Text = "";
        txtxsdh.Text = "";
        txtDealersName.Text = "";
        checksp.Checked = false;
        checkjc.Checked = false;
        checkdf.Checked = false;
    }
    //删除一行
    protected void btn_Del_Click(object sender, EventArgs e)
    {

        List<T_BillingApplication> cxblist = new List<T_BillingApplication>();
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            System.Web.UI.WebControls.CheckBox check = GridView1.Rows[s].FindControl("CheckBox2") as System.Web.UI.WebControls.CheckBox;
            if (check.Checked == false)
            {
                T_BillingApplication cx = new T_BillingApplication();
                cx.Note2 = GetGwStr(GridView1.Rows[s].Cells[1].Text);
                cx.TruckCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);
                cx.DealersName = GetGwStr(GridView1.Rows[s].Cells[3].Text);
                cx.IsJC = GridView1.Rows[s].Cells[4].Text == "是" ? "是" : "否";
                cx.IsSpApp = GridView1.Rows[s].Cells[5].Text == "是" ? "是" : "否";
                cx.Note3 = GridView1.Rows[s].Cells[6].Text == "是" ? "是" : "否";
                cxblist.Add(cx);
            }
            else
            {
                if (GridView1.Rows[s].Cells[1].Text.Trim() != null && GridView1.Rows[s].Cells[1].Text != "")
                {
                    if (GridView1.Rows[s].Cells[2].Text.Trim() != null && GridView1.Rows[s].Cells[2].Text != "")
                    {
                        string strOderCode = GridView1.Rows[s].Cells[1].Text.Trim();
                        string strCarCode = GridView1.Rows[s].Cells[2].Text.Trim();
                        if (GridView1.Rows.Count > 1)
                        {
                            int row = bll.DeletebyTrckCode(strCarCode, strOderCode);
                            if (row > -1)
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
                            }

                        }
                        else if (GridView1.Rows.Count == 1)
                        {

                            if (MessageBox.Show("警告:您是否删除最后一条,如果删除则该单作废", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                int row = bll.DeletebyTrckCode(strCarCode, strOderCode);
                                if (row > -1)
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
                                }

                            }
                            else
                            {
                                // ClientScript.RegisterStartupScript(this.GetType(), "", "window.confirm('你要删除这个部门，真的要如此吗');", true);
                                return;
                            }
                        }
                    }
                }
            }
        }
        GridView1.DataSource = cxblist;
        GridView1.DataBind();
    }
    //上传按钮
    protected void btn_sc_Click(object sender, EventArgs e)
    {
        if (FileUpload1.Visible == true)
        {
            string script;
            if (FileUpload1.PostedFile.FileName == "")
            {
                laFilexx.Text = "请选择图片";
                return;
            }
            else
            {
                try
                {
                    string filePath = FileUpload1.PostedFile.FileName;
                    string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                    string fileSn = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    ////转换成绝对地址,
                    string serverpath = Server.MapPath(@"~\SaleBill\Upload\kpsq\") + fileSn + filename.Substring(filename.LastIndexOf("."));
                    ////转换成与相对地址,相对地址为将来访问图片提供
                    string relativepath = @"~\SaleBill\Upload\kpsq\" + fileSn + filename.Substring(filename.LastIndexOf("."));
                    ////绝对地址用来将上传文件夹保存到服务器的具体路下。
                    FileUpload1.PostedFile.SaveAs(serverpath);
                    ////把相对路径的地址保存到页面hdImageUrl的value值上以供保存值时使用。
                    hiddFileDz.Value = relativepath;
                    Lafilename.Text = filename;
                    laFilexx.Text = "上传成功";
                    btn_sc.Text = "修改附件";
                    FileUpload1.Visible = false;
                }
                catch (Exception ex)
                {
                    laFilexx.Text = ex.ToString();
                }
            }
        }
        else
        {
            btn_sc.Text = "上传";
            laFilexx.Text = "";
            FileUpload1.Visible = true;
            Lafilename.Text = "";
        }
    }
    ////下载按钮
    //protected void btn_xz_Click(object sender, EventArgs e)//下载按钮  现在不用这个了
    //{
    //    string url = hiddFileDz.Value;
    //    if (url != "")
    //    {
    //        string filename = Server.MapPath(url);
    //        ClientScript.RegisterStartupScript(this.GetType(), "", @"<a href='" + url + @"' target='_blank' id='dfg' ></a> <script> document.getElementById('dfg').onclick(); </script>", true);
    //    }
    //}
    /// <summary>
    /// 根据车架号得到相应信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btdr_Click(object sender, EventArgs e)
    {
        if (txtcjh.Text != null && this.txtcjh.Text != "")
        {
            DataTable temp = viewbll.getDt(txtcjh.Text.Trim());
            if (temp != null && temp.Rows.Count > 0)
            {
                this.txtxsdh.Text = temp.Rows[0]["ddh"].ToString().Trim();

                this.txtDealersName.Text = temp.Rows[0]["yhmc"].ToString().Trim();
            }
            string strSpecialCode = "";
            if (bll.isSpecialRebatesApp(txtcjh.Text.Trim(), out strSpecialCode))
            {
                checksp.Checked = true;
            }
            else
            {
                checksp.Checked = false;
            }

            if (bll.isDF(txtcjh.Text.Trim(), out strSpecialCode))
            {
                checkdf.Checked = true;
            }
            else
            {
                checkdf.Checked = false;
            }
        }
    }
}