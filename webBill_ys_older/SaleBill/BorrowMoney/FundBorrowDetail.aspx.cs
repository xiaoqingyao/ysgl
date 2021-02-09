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
using Bll.Bills;
using Models;
using System.Collections.Generic;
using System.Text;
using Dal.SysDictionary;
using Bll;
using WorkFlowLibrary.WorkFlowBll;
using System.IO;

public partial class SaleBill_BorrowMoney_FundBorrowDetail : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    LoanListBLL loanbll = new LoanListBLL();
    BillMainBLL bllBillMain = new BillMainBLL();
    ConfigBLL bllConfig = new ConfigBLL();

    string strCtrl = "";
    string strBillCode = "";
    string strUserCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            strUserCode = Session["userCode"].ToString().Trim();
            object objCtrl = Request["Ctrl"];
            if (objCtrl != null)
            {
                strCtrl = objCtrl.ToString();
            }
            object objCode = Request["Code"];
            if (objCode != null)
            {
                strBillCode = objCode.ToString();
            }
            if (!IsPostBack)
            {
                this.bindData();
                BindDDL();
            }
            ClientScript.RegisterArrayDeclaration("availableTagsdt", GetdetpAll());
            ClientScript.RegisterArrayDeclaration("avaiusernamedt", GetuserAll());
        }
    }

    private void BindDDL()
    {
        DataTable dt = server.GetDataTable("select dicCode,dicName from bill_datadic where dictype='20'", null);
        ddljklb.DataSource = dt;
        ddljklb.DataTextField = "dicName";
        ddljklb.DataValueField = "dicCode";
        ddljklb.DataBind();
        ddljklb.Items.Insert(0, new ListItem("--选择--", ""));


        //通过配置项添加附加单据

        if (bllConfig.GetModuleDisabled("HasBGSQ"))
        {
            this.selectBill.Items.Add(new ListItem("报告单", "bg"));
        }
        if (bllConfig.GetModuleDisabled("HasCGSP"))
        {
            this.selectBill.Items.Add(new ListItem("采购单", "cg"));
        }
        if (bllConfig.GetModuleDisabled("HasCCSQ"))
        {
            this.selectBill.Items.Add(new ListItem("出差单", "cc"));
        }
    }
    /// <summary>
    /// 选择人员
    /// </summary>
    /// <returns></returns>
    public string GetuserAll()
    {
        DataSet ds = server.GetDataSet("select  '['+usercode+']'+ username as usercodename from bill_users ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["usercodename"]));
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

    /// <summary>
    /// 部门选择
    /// </summary>
    /// <returns></returns>
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

    private void bindData()
    {

        if (strCtrl == "Add")
        {

            //有效日期默认当天，申请日期默认当天
            this.txtaddtime.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            this.txtjksj.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
                return;
            }
            else
            {
                string userCode = Session["userCode"].ToString();
                string strsqlrespcodename = "select  '['+usercode+']'+ username from bill_users where usercode='" + userCode + "'";
                string strrespcodename = server.GetCellValue(strsqlrespcodename);
                string strlastjkr = "";
                string strCurrentOrLastForYbbx = new Bll.ConfigBLL().GetValueByKey("CurrentOrLastForJK");
                if (Session["LastJKR"] != null && Session["LastJKR"].ToString() != "" && strCurrentOrLastForYbbx.Equals("0"))
                {
                    strlastjkr = Session["LastJKR"].ToString();
                    string strlastjkrname = server.GetCellValue("select  '['+usercode+']'+ username from bill_users where usercode='" + strlastjkr + "'");

                    txtloanName.Text = strlastjkrname;//借款人
                }
                else
                {
                    txtloanName.Text = strrespcodename;//借款人
                }

                if (strrespcodename != "" && strrespcodename != null)
                {
                    this.txtResponsibleName.Text = strrespcodename;//经办人
                    string strloancode = strrespcodename.Substring(1, strrespcodename.IndexOf("]") - 1);
                    Bill_Departments modelDept = new Bll.UserProperty.UserMessage(strloancode).GetRootDept();
                    this.txtdeptname.Text = "[" + modelDept.DeptCode + "]" + modelDept.DeptName;
                    this.HiddenField1.Value = "[" + modelDept.DeptCode + "]" + modelDept.DeptName;
                }
            }
            //报告单号
            Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            string strneed = DateTime.Now.ToString("yyyyMMdd");
            string strcode = pusbll.GetBillCode("jksq", strneed, 1, 3);
            this.lbjkcode.Text = strcode.Trim();
            this.btn_ok.Visible = false;
            this.btn_cancel.Visible = false;
        }
        else
        {
            BindModel();
            if (strCtrl == "Edit" || strCtrl == "Atal")
            {
                this.trgride.Visible = false;
                this.btn_ok.Visible = false;
                this.btn_cancel.Visible = false;
            }
            else if (strCtrl == "View")
            {
                this.btn_bc.Visible = false;
              
            }
            else if (strCtrl == "look")
            {
                this.btn_sc.Visible = false;
                this.btn_ok.Visible = false;
                this.btn_cancel.Visible = false;
                this.btn_bc.Visible = false;
                this.trgride.Visible = true;
            }
            else if (strCtrl.Equals("audit"))
            {
                this.btn_bc.Visible = false;
            }
        }
    }

    /// <summary>
    /// 获取model
    /// </summary>
    public void BindModel()
    {
        T_LoanList modeljk = loanbll.GetModel(strBillCode);
        if (modeljk != null)
        {
            lbjkcode.Text = modeljk.Listid;
            txtloanName.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode ='" + modeljk.LoanCode + "'");
            txtResponsibleName.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode ='" + modeljk.ResponsibleCode + "'");
            ddljklb.SelectedValue = modeljk.NOTE6;
            txtdeptname.Text = server.GetCellValue("select '['+deptcode+']'+deptname from bill_departments where deptcode ='" + modeljk.LoanDeptCode + "'");
            txtjksj.Text = Convert.ToDateTime(modeljk.LoanDate).ToString("yyyy-MM-dd");//借款日期
            txtaddtime.Text = Convert.ToDateTime(modeljk.LoanSystime).ToString("yyyy-MM-dd");
            txtmoney.Text = modeljk.LoanMoney.ToString();
            txtjksy.Text = modeljk.LoanExplain;
            txtbz.Text = modeljk.NOTE5;
            txtjkts.Text = modeljk.NOTE4;
            string fjName = modeljk.NOTE8;
            string fjUrl = modeljk.NOTE9;
            if (!string.IsNullOrEmpty(modeljk.NOTE7))
            {
                tb_fysq.InnerHtml = GetFjdj(modeljk.NOTE7);
            }
            if (!string.IsNullOrEmpty(fjName)&&!string.IsNullOrEmpty(fjUrl))
            {
                Literal1.Text = "<a href='../../AFrame/download.aspx?filename=" + fjName + "&filepath=" + fjUrl + "' target='_blank'>" + fjName + "下载</a>";

            }
            else
            {
                Literal1.Text = "无附件";

            }
        }

        GridView1.DataSource = getbycodemodel(strBillCode);
        GridView1.DataBind();
        trgride.Visible = true;
        for (int i = 0; i < this.GridView1.Rows.Count; i++)
        {
            this.GridView1.Rows[i].Cells[2].Text = "<a href=# onclick=\"openDetailbx('" + this.GridView1.Rows[i].Cells[1].Text.ToString().Trim() + "');\">" + this.GridView1.Rows[i].Cells[2].Text.ToString().Trim() + "</a>";
        }

    }



    private object getbycodemodel(string strBillCode)
    {
        string strLoaner = SubString(txtloanName.Text.Trim());
        string strsql = "";

        int count = Convert.ToInt32(server.GetCellValue("select count(*) from  T_ReturnNote where  loancode ='" + strBillCode + "'"));
        if (count == 0)
        {
            return null;
        }
        strsql = "select  loancode,je ,case ltype when '2' then '现金' when '1' then '单据冲减'  end as ltype,billcode,ldate,note1,(select billName from bill_main where billcode=T_ReturnNote.billcode)  as billname ,(select flowid from bill_main where billcode=T_ReturnNote.billcode)  as flowid ,listid,note3 from T_ReturnNote where  loancode ='" + strBillCode + "' ";
        return server.RunQueryCmdToTable(strsql);
    }
    private string GetFjdj(string djs)
    {

        StringBuilder fysqSb = new StringBuilder();
        string sql = "select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,b.cgze,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,'审批通过' as spzt from bill_main a,bill_cgsp b where a.flowid='cgsp' and a.billCode=b.cgbh ";
        string sql2 = "select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,b.yjfy as cgze,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,'审批通过' as spzt from bill_main a,bill_lscg b where a.flowid='lscg' and a.billCode=b.cgbh ";
        string sqlCCSQ = @"select(select dicname from bill_datadic where diccode=b.typecode and dictype='06') 
                as cclb,a.billDate,(select deptName from bill_departments where deptCode=a.billDept)
                 as Dept,(select userName from bill_users where userCode=a.billUser)
                 as  billUser ,a.billJe,'审批通过' as spzt,b.reasion,a.billCode
               from bill_main a,bill_travelApplication b where a.billCode=b.maincode";

        string[] arr = djs.Split(',');
        for (int i = 1; i <= arr.Length; i++)
        {
            if (arr[i - 1].Substring(0, 4) == "ccsq")//出差申请
            {
                DataSet ds = server.GetDataSet(sqlCCSQ + " and a.billcode='" + arr[i - 1] + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fysqSb.Append("<tr>");
                    fysqSb.Append("<td><input id='radio" + i + "' type='radio' name='myrad' onclick='radCheck(this);' /></td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["billCode"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["Dept"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["billUser"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(new PublicServiceBLL().cutDt(ds.Tables[0].Rows[0]["billDate"].ToString()));
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cclb"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["billJe"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["reasion"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["spzt"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("</tr>");
                }
            }
            else if (arr[i - 1].Substring(0, 2) == "cg")
            {
                DataSet ds = server.GetDataSet(sql + "and billcode='" + arr[i - 1] + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fysqSb.Append("<tr>");
                    fysqSb.Append("<td><input id='radio" + i + "' type='radio' name='myrad' onclick='radCheck(this);'/></td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["billCode"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cgDept"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cbr"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(new PublicServiceBLL().cutDt((ds.Tables[0].Rows[0]["sj"].ToString())));
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cglb"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cgze"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["sm"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["spzt"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("</tr>");
                }
            }
            else if (arr[i - 1].Substring(0, 2) == "ls")
            {
                DataSet ds = server.GetDataSet(sql2 + "and billcode='" + arr[i - 1] + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fysqSb.Append("<tr>");
                    fysqSb.Append("<td><input id='radio" + i + "' type='radio' name='myrad'onclick='radCheck(this);' /></td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["billCode"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cgDept"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cbr"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["sj"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cglb"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cgze"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["sm"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["spzt"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("</tr>");
                }
            }
        }

        return fysqSb.ToString();
    }


    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        T_LoanList modeljk = new T_LoanList();
        Bill_Main modelmain = new Bill_Main();


        string fjName = Lafilename.Text.Trim();
        string fjUrl = hiddFileDz.Value.Trim();

        modeljk.Listid = lbjkcode.Text.Trim();
        modeljk.LoanCode = SubString(txtloanName.Text.Trim());
        modeljk.ResponsibleCode = SubString(txtResponsibleName.Text.Trim());
        if (!string.IsNullOrEmpty(ddljklb.SelectedValue))
        {
            modeljk.NOTE6 = ddljklb.SelectedValue;
        }
        modeljk.LoanDeptCode = SubString(txtdeptname.Text.Trim());
        modeljk.LoanDate = txtjksj.Text.Trim();//借款日期
        modeljk.LoanSystime = txtaddtime.Text.Trim();
        modeljk.LoanMoney = getDecimal(txtmoney.Text.Trim());
        modeljk.LoanExplain = txtjksy.Text.Trim();
        modeljk.NOTE5 = txtbz.Text.Trim();
        modeljk.NOTE4 = txtjkts.Text.Trim();
        modeljk.Status = "1";
        modeljk.NOTE7 = hffjdj.Value;
        modeljk.NOTE3 = "0";
        modeljk.NOTE8 = fjName;
        modeljk.NOTE9 = fjUrl;

        modelmain.BillCode = lbjkcode.Text.Trim();
        modelmain.BillName = "借款申请单";
        modelmain.FlowId = "jksq";
        modelmain.StepId = "-1";
        modelmain.BillUser = SubString(txtloanName.Text.Trim());
        modelmain.BillDate = Convert.ToDateTime(txtaddtime.Text.Trim());
        modelmain.BillDept = SubString(txtdeptname.Text.Trim());
        modelmain.BillJe = getDecimal(txtmoney.Text.Trim());
        modelmain.LoopTimes = 0;

        if (loanbll.AddModel(modeljk, modelmain))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败！');", true);
            return;
        }
    }


    private decimal getDecimal(string val)
    {
        if (!string.IsNullOrEmpty(val))
        {
            try
            {
                return Convert.ToDecimal(val);
            }
            catch (Exception)
            {

                throw;
            }

        }
        else
        {
            return 0;
        }
    }

    decimal deAmount = 0;
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string zt = e.Row.Cells[5].Text;
            string state = "";
            string billcode = e.Row.Cells[4].Text;
            string billname = e.Row.Cells[6].Text;
            string strpotype = e.Row.Cells[7].Text;
            string strurl = "";
            string billType = "";
            if (zt == "1")
            {
                state = "审批通过";
            }
            else
            {
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                state = bll.WFState(billcode);
            }
            e.Row.Cells[5].Text = state;

            if (strpotype != "")
            {
                if (strpotype == "ybbx")//一般报销单
                {
                    billType = "一般报销单";
                    strurl = "../../webBill/bxgl/bxDetailFinal.aspx?type=look&billCode=" + billcode;
                }
                else if (strpotype == "gkbx")//归口报销
                {
                    billType = "归口报销单";
                    strurl = "../../webBill/bxgl/bxDetailForGK.aspx?type=look&billCode=" + billname;
                }
                else if (strpotype == "hksq")//还款申请单
                {
                    billType = "还款申请单";
                    string id = strpotype = e.Row.Cells[8].Text;
                    strurl = "FundHKDetail.aspx?Ctrl=look&Code=" + billcode + "&id=" + id;
                }

                e.Row.Cells[4].Text = "<a href=\"#\" onclick=\"openDetail('" + strurl + "')\">" + billType + "</a>";
            }

            //计算合计行
            string streveJe = e.Row.Cells[1].Text.Trim();
            decimal deeveJe = 0;
            if (decimal.TryParse(streveJe, out deeveJe))
            {
                deAmount += deeveJe;
            }

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = deAmount.ToString("N02");
        }
    }
    protected void btn_sc_Click(object sender, EventArgs e)
    {
        if (uploadFiles.Visible == true)
        {
            string script;
            if (uploadFiles.PostedFile.FileName == "")
            {
                laFilexx.Text = "请选择文件";
                return;
            }
            else
            {
                //
                try
                {
                    string filePath = uploadFiles.PostedFile.FileName;
                    string Name = this.uploadFiles.PostedFile.FileName;
                    string name = System.IO.Path.GetFileName(Name).Split('.')[0];
                    string exname = System.IO.Path.GetExtension(Name);
                    if (isOK(exname))
                    {
                        string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                        string fileSn = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        ////转换成绝对地址,
                        string serverpath = Server.MapPath(@"~\Uploads\jkd\") + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////转换成与相对地址,相对地址为将来访问图片提供
                        string relativepath = @"~\Uploads\jkd\" + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////绝对地址用来将上传文件夹保存到服务器的具体路下。
                        if (!Directory.Exists(Server.MapPath(@"~\Uploads\jkd\")))
                        {
                            Directory.CreateDirectory(Server.MapPath(@"~\Uploads\jkd\"));
                        }

                        uploadFiles.PostedFile.SaveAs(serverpath);
                        ////把相对路径的地址保存到页面hdImageUrl的value值上以供保存值时使用。
                        hiddFileDz.Value = relativepath;
                        Lafilename.Text = filename;
                        laFilexx.Text = "上传成功";
                        btn_sc.Text = "修改附件";
                        uploadFiles.Visible = false;
                    }
                    else
                    {
                        Response.Write("<script>alert('文件类型不合法');</script>");
                    }
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
            uploadFiles.Visible = true;
            Lafilename.Text = "";
        }
    }


    bool isOK(string exname)
    {
        if (exname.ToLower() == ".doc" || exname.ToLower() == ".docx" || exname.ToLower() == ".jpg" || exname.ToLower() == ".png" || exname.ToLower() == ".gif" || exname.ToLower() == ".xls" || exname.ToLower() == ".xlsx" || exname.ToLower() == ".zip" || exname.ToLower() == ".txt" || exname.ToLower() == ".pdf" || exname.ToLower() == ".rar" || exname.ToLower() == ".ppt")
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}
