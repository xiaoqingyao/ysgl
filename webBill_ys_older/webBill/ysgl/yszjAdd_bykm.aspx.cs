using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Ajax;
using System.IO;
using Bll.UserProperty;
using System.Text;

public partial class ysgl_yszjAdd_bykm : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    string strNowDeptCode = "";
    string strNowDeptName = "";
    string strCsDeptCode;
    string strCsdeptcode;
    string strCsdeptname;
    string strDeptCodes;
    string type = "";
    string billCode = "";
    string strbillname = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {

            if (Request["type"] != null)
            {
                type = Request["type"].ToString().Trim();
            }
            if (Request["deptcode"] != null)
            {
                strCsDeptCode = Request["deptcode"].ToString().Trim();
            }
            if (Request["billCode"] != null)
            {
                billCode = Request["billCode"].ToString().Trim();
            }

            if (type!="add"&&type!="edit")
            {
                this.Button1.Enabled=ddlnian.Enabled=LaDept.Enabled = false;
            }
            Ajax.Utility.RegisterTypeForAjax(typeof(ysgl_yszjAdd_bykm));


            string usercode = Session["userCode"].ToString().Trim();
            strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");

            //验证是否是预算到末级单位
            string strsfmj = server.GetCellValue("select avalue from t_config where akey ='deptjc'");
            if (!string.IsNullOrEmpty(strsfmj) && strsfmj == "Y")
            {
                strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strDeptCodes + ") and deptCode not in (" + strNowDeptCode + ")", null);

            }


            if (!IsPostBack)
            {


                //绑定预算类型
                this.ddlYsType.DataSource = new sqlHelper.sqlHelper().GetDataTable("select * from bill_dataDic where dictype='18'", null);
                this.ddlYsType.DataTextField = "dicName";
                this.ddlYsType.DataValueField = "dicCode";
                this.ddlYsType.DataBind();
                this.ddlYsType.SelectedValue = "02";//默认费用预算
                binddept();
                this.BindDataGrid();

            }
            string dept = "";
            if (!string.IsNullOrEmpty(LaDept.SelectedValue))
            {
                dept = LaDept.SelectedValue;
            }
            ClientScript.RegisterArrayDeclaration("availableTagsDeptkm", GetdeptkmAll(dept));
        }
    }
    public void binddept()
    {
        #region 绑定人员管理下的部门
        //if (!strNowDeptCode.Equals(""))
        //{
        //获取人员管理下的部门
        string usercode = Session["userCode"].ToString().Trim();
        strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");
        string strsfmj = new Bll.ConfigBLL().GetValueByKey("deptjc");//是否预算到末级部门
        string addWhere = " and sjdeptCode='000001' ";
        if (!string.IsNullOrEmpty(strsfmj) && strsfmj == "Y")
        {
            addWhere = "";
        }
        if (Request["deptcode"] != null)
        {
            strCsDeptCode = Request["deptcode"].ToString().Trim();
            strCsdeptcode = server.GetCellValue("select deptcode from bill_departments where deptcode='" + strCsDeptCode + "'");
            strCsdeptname = server.GetCellValue("select deptName from bill_departments where deptcode='" + strCsDeptCode + "'");

            dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  deptCode in (" + strDeptCodes + ") and deptCode not in (" + strCsdeptcode + ")" + addWhere, null);
        }
        //如果是三级部门并且不预算到三级部门
        if (!isTopDept("y", usercode) && strsfmj != "Y")
        {
            //获取上级部门
            strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
            strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
        }
        else
        {
            //获取当前用户所在的部门编号及其部门名称
            strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");

        }
        string strrightsql = "select deptCode,deptName from bill_departments where   deptCode in (" + strDeptCodes + ") and deptCode not in (" + strNowDeptCode + ")" + addWhere + " order by deptcode";
        dtuserRightDept = server.GetDataTable(strrightsql, null);

        if (strDeptCodes != "")
        {
            if (dtuserRightDept.Rows.Count > 0)
            {
                for (int i = 0; i < dtuserRightDept.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Text = "[" + dtuserRightDept.Rows[i]["deptCode"].ToString().Trim() + "]" + dtuserRightDept.Rows[i]["deptName"].ToString().Trim();
                    li.Value = dtuserRightDept.Rows[i]["deptCode"].ToString().Trim();

                    this.LaDept.Items.Add(li);

                }
                this.LaDept.Items.Insert(0, new ListItem("-全部-", ""));
            }
            if (strCsdeptcode != "" && strCsdeptcode != null)
            {
                this.LaDept.Items.Insert(0, new ListItem("[" + strCsdeptcode + "]" + strCsdeptname, strCsdeptcode));
                this.LaDept.SelectedIndex = 0;


            }
            else
            {
                this.LaDept.Items.Insert(0, new ListItem("[" + strNowDeptCode + "]" + strNowDeptName, strNowDeptCode));
                this.LaDept.SelectedIndex = 0;
            }
        }
        //}
        #endregion
    }


    protected void ddlnianselectindexchanged(object sender, EventArgs e)
    {

        string sql = "select gcbh,xmmc,kssj,jzsj,(select username from bill_users where usercode=fqr) as fqr,fqsj,(case status when '0' then '未开始' when '1' then '进行中' when '2' then '已结束' end) as statusName,status from bill_ysgc ";
        SysManager sysMgr = new SysManager();
        string nd = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 4);
        IDictionary<string, string> config = sysMgr.GetsysConfigBynd(nd);
       
        //0，年度预算，1，季度预算，2，月度预算
        if (config["MonthOrQuarter"] == "1")
        {
            //sql += " where ystype='1' and (status='2' or jzsj<'" + System.DateTime.Now.ToShortDateString() + "') ";
            sql += " where ystype='1' and and status<>'0'  ";
        }
        else if (config["MonthOrQuarter"] == "0")
        {
            //sql += " where ystype='0' and (status='2' or jzsj<'" + System.DateTime.Now.ToShortDateString() + "') ";
            sql += " where ystype='0' and status<>'0'  ";
        }
        else
        {
            //sql += " where ystype='2' and (status='2' or jzsj<'" + System.DateTime.Now.ToShortDateString() + "') ";
            sql += " where ystype='2' and status<>'0' ";
        }

        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (gcbh like '%" + this.TextBox1.Text.ToString().Trim() + "%' or  xmmc like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
        }
        if (ddlnian.SelectedValue != null)
        {
            sql += " and nian='" + ddlnian.SelectedValue + "'";
        }
        //2014-05-16 beg 预算调整时目标预算过程必须是已开启预算
        // and gcbh in (select distinct gcbh from  bill_ysmxb where left(gcbh,4)='2014' and ysdept='05' and ystype='1')
        string configVal = server.GetCellValue("select avalue from t_Config where akey='AllowTzUodoYs' ");
        if (!string.IsNullOrEmpty(configVal) && configVal == "1")
        {
            sql += "  and gcbh in (select distinct gcbh from  bill_ysmxb where left(gcbh,4)='" + ddlnian.SelectedValue + "' and ysdept='" + Request["deptcode"] + "' and ystype='1')";
        }
        //2014-05-16 end
        sql += " order by nian desc,gcbh ";
        //Response.Write(sql);
        DataSet temp = server.GetDataSet(sql);
        this.DataGrid1.DataSource = temp;
        this.DataGrid1.DataBind();
    }
    public void BindDataGrid()
    {

        string strsql2 = "select nian,xmmc from bill_ysgc where   yue='' order by nian desc";
        this.ddlnian.DataSource = server.GetDataTable(strsql2, null);
        this.ddlnian.DataTextField = "xmmc";
        this.ddlnian.DataValueField = "nian";
        this.ddlnian.DataBind();

        if (type == "add")
        {
            string sql = "select gcbh,xmmc,kssj,jzsj,(select username from bill_users where usercode=fqr) as fqr,fqsj,(case status when '0' then '未开始' when '1' then '进行中' when '2' then '已结束' end) as statusName,status from bill_ysgc  ";

            SysManager sysMgr = new SysManager();
            string nd = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 4);
            IDictionary<string, string> config = sysMgr.GetsysConfigBynd(nd);
            string strisdz = "";
            if (!string.IsNullOrEmpty(Request["isdz"]))
            {
                strisdz = Request["isdz"].ToString();
            }
            if (strisdz != "1")
            {
                //0，年度预算，1，季度预算，2，月度预算
                if (config["MonthOrQuarter"] == "1")
                {
                    //sql += " where ystype='1' and (status='2' or jzsj<'" + System.DateTime.Now.ToShortDateString() + "') ";
                    sql += " where ystype='1' and and status<>'0'  ";
                }
                else if (config["MonthOrQuarter"] == "0")
                {
                    //sql += " where ystype='0' and (status='2' or jzsj<'" + System.DateTime.Now.ToShortDateString() + "') ";
                    sql += " where ystype='0' and status<>'0'  ";
                }
                else
                {
                    //sql += " where ystype='2' and (status='2' or jzsj<'" + System.DateTime.Now.ToShortDateString() + "') ";
                    sql += " where ystype='2' and status<>'0' ";
                }
            }
            else
            {
                lblmsg.Visible = false;
                sql += " where ystype='2'";
            }

            if (this.TextBox1.Text.ToString().Trim() != "")
            {
                sql += " and (gcbh like '%" + this.TextBox1.Text.ToString().Trim() + "%' or  xmmc like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
            }
            if (ddlnian.SelectedValue != null)
            {
                sql += " and nian='" + ddlnian.SelectedValue + "'";
            }

            //2014-05-16 beg 预算调整时目标预算过程必须是已开启预算
            // and gcbh in (select distinct gcbh from  bill_ysmxb where left(gcbh,4)='2014' and ysdept='05' and ystype='1')
            string configVal = server.GetCellValue("select avalue from t_Config where akey='AllowTzUodoYs' ");
            if (!string.IsNullOrEmpty(configVal) && configVal == "1")
            {
                sql += "  and gcbh in (select distinct gcbh from  bill_ysmxb where left(gcbh,4)='" + ddlnian.SelectedValue + "' and ysdept='" + Request["deptcode"] + "' and ystype='1')";
                lblmsg.InnerText = lblmsg.InnerText + "只能选择做过年初预算的预算过程；";
            }
            //2014-05-16 end
            sql += " order by nian desc,gcbh ";
            //Response.Write(sql);
            DataSet temp = server.GetDataSet(sql);



            this.DataGrid1.DataSource = temp;
            this.DataGrid1.DataBind();
        }
        else
        {
            if (!string.IsNullOrEmpty(billCode))
            {

                string sql = @"   select  (case status when '0' then '未开始' when '1' then '进行中' when '2' then '已结束' end) 
              as statusName,
              (select billName from bill_users where userCode=main.billUser) as billname,
              ysgc.*,  mx.ysje,mx.sm,
              (select deptCode from bill_departments where deptCode=main.billdept) as showdept
              ,(select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=mx.yskm) as showyskm
              ,( select xmmc from bill_ysgc where   yue=''  and nian=LEFT(mx.gcbh,4))as ysgc
              ,(  select dicCode from bill_dataDic where dictype='18' and dicCode=main.dydj) as djlx
              from bill_main main,bill_ysmxb mx,bill_ysgc ysgc where
               main.billCode=mx.billCode  and mx.gcbh=ysgc.gcbh and main.billCode='" + billCode + "'";
                DataTable dt = server.GetDataTable(sql, null);
                if (dt != null)
                {
                    LaDept.SelectedValue = dt.Rows[0]["showdept"].ToString();
                    txt_yskm.Value = dt.Rows[0]["showyskm"].ToString();
                    ddlYsType.SelectedValue = dt.Rows[0]["djlx"].ToString();
                    hidbillname.Value = dt.Rows[0]["billname"].ToString().Trim();
                }
                this.DataGrid1.DataSource = dt;
                this.DataGrid1.DataBind();
            }

        }

    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("yszjList_bykm.aspx");
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        //获取当前人员的部门编号
        //string deptGuid = server.GetCellValue("select userdept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
        // string deptGuid = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());
        string deptGuid = "";
        string ysGuid = "";

        deptGuid = this.LaDept.SelectedValue.Trim();
        SysManager sysMgr = new SysManager();



        string loopTimes = "1";
        List<string> list = new List<string>();
        double yszje = 0;
        string errorMsg = "";
        string yskm = txt_yskm.Value;
        if (!string.IsNullOrEmpty(yskm))
        {
            yskm = yskm.Substring(1, yskm.IndexOf("]") - 1);
        }
        else
        {
            errorMsg = "请选择追加科目。";
        }

        if (!string.IsNullOrEmpty(billCode))
        {
            ysGuid = billCode;
            list.Add("delete bill_ysmxb where billcode='" + ysGuid + "'");
            list.Add("delete bill_main where billcode='" + ysGuid + "'");

        }
        else
        {
            ysGuid = (new GuidHelper()).getNewGuid();

        }
        if (string.IsNullOrEmpty(hidbillname.Value))
        {
            strbillname = sysMgr.GetYbbxBillName("pl_", DateTime.Now.ToString("yyyMMdd"), 1);
        }
        else
        {
            strbillname = hidbillname.Value;
        }

        for (int i = 0; i <= this.DataGrid1.Items.Count - 1; i++)
        {


            string gcbh = this.DataGrid1.Items[i].Cells[1].Text.ToString().Trim();
            TextBox txt = (TextBox)this.DataGrid1.Items[i].FindControl("TextBox2");
            string shuoming = ((TextBox)this.DataGrid1.Items[i].FindControl("txtShuoming")).Text.Trim();
            try
            {
                double ysje = double.Parse(txt.Text.ToString().Trim());

                if (ysje == 0)
                { }
                else
                {
                    if (shuoming.Equals(""))
                    {

                        errorMsg = "请输入第" + (i + 1) + "行" + gcbh + "的追加说明";
                    }
                    //if (yskm.Length == 2)
                    //{
                    yszje += ysje;
                    //}
                    //写入预算明细
                    list.Add("insert into bill_ysmxb(gcbh,billCode,yskm,ysje,ysDept,ysType,sm) values('" + gcbh + "','" + ysGuid + "','" + yskm + "'," + ysje.ToString() + ",'" + deptGuid + "','2','" + shuoming + "')");
                    //list.Add("insert into bill_ysmxb(gcbh,billCode,yskm,ysje,ysDept,ysType) values('" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','" + ysGuid + "','" + yskm + "'," + ysje.ToString() + ",'" + deptGuid + "','2')");
                }
            }
            catch { }
        }
        if (!string.IsNullOrEmpty(errorMsg))
        {
            Response.Write("<script>alert('" + errorMsg + "')</script>");
            return;
        }
        else if (yszje == 0)
        {
            Response.Write("<script>alert('总金额为0，保存失败！')</script>");
            return;
        }




        //添加附件
        string fjname = hidfilnename.Value;
        string fjurl = hiddFileDz.Value;
        var fujian = "";
        if (!string.IsNullOrEmpty(fjname) && !string.IsNullOrEmpty(fjurl))
        {
            fujian = Server.UrlDecode(fjname + "|" + fjurl);
        }
        string strxmcode = "";
        string flowid = "yszj";
        if (!string.IsNullOrEmpty(Request["xmcode"]))
        {
            strxmcode = Request["xmcode"].ToString();

        }


        //写入预算总表 记录
        list.Add("insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType,billName2,dydj,note2,note3) values('" + ysGuid + "','" + strbillname + "','" + flowid + "','-1','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString() + "','" + deptGuid + "'," + yszje.ToString() + ",'" + loopTimes + "','2','" + txt_sm.Text.Trim() + "','" + this.ddlYsType.SelectedValue + "','" + fujian + "','" + strxmcode + "')");
        //list.Add("insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType) values('" + ysGuid + "','" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','yszj','-1','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString() + "','" + deptGuid + "'," + yszje.ToString() + ",'" + loopTimes + "','2')");
        list.Add("update bill_main set billJe=(select isnull(sum(isnull(ysje,0)),0) from bill_ysmxb where billCode='" + ysGuid + "') where billcode='" + ysGuid + "'");

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            string strdeptcode = this.LaDept.SelectedValue.ToString().Trim();
            if (!string.IsNullOrEmpty(strxmcode))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('yszjList_bykm.aspx?isdz=1&xmzj=1&deptcode=" + strdeptcode + "','_self');", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('yszjList_bykm.aspx?deptcode=" + strdeptcode + "','_self');", true);

            }
        }
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
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "')";
        }
        else
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnScdj_Click(object sender, EventArgs e)
    {
        string filePath = "";
        string Name = "";
        string name = "";
        string exname = "";
        if (upLoadFiles.Visible == true)
        {
            if (upLoadFiles.PostedFile.FileName == "")
            {
                laFilexx.Text = "请选择文件";
                return;
            }
            else
            {
                try
                {
                    filePath = upLoadFiles.PostedFile.FileName;
                    Name = this.upLoadFiles.PostedFile.FileName;
                    name = System.IO.Path.GetFileName(Name).Split('.')[0];
                    exname = System.IO.Path.GetExtension(Name);
                    if (isOK(exname))
                    {
                        string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                        string fileSn = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        ////转换成绝对地址,
                        string serverpath = Server.MapPath(@"~\Uploads\yszj\") + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////转换成与相对地址,相对地址为将来访问图片提供
                        string relativepath = @"~\Uploads\yszj\" + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////绝对地址用来将上传文件夹保存到服务器的具体路下。
                        if (!Directory.Exists(Server.MapPath(@"~\Uploads\yszj\")))
                        {
                            Directory.CreateDirectory(Server.MapPath(@"~\Uploads\yszj\"));
                        }
                        upLoadFiles.PostedFile.SaveAs(serverpath);
                        ////把相对路径的地址保存到页面hdImageUrl的value值上以供保存值时使用。
                        hiddFileDz.Value += relativepath + ";";
                        Lafilename.Text += "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>新附件" + filename + "：</span></div>";

                        hidfilnename.Value += filename + ";";
                        laFilexx.Text = "上传成功";
                        //  btn_sc.Text = "修改附件";
                        //upLoadFiles.Visible = false;
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
            //laFilexx.Text = "";
            upLoadFiles.Visible = true;
            //Lafilename.Text = "";
            //hidfilnename.Value = "";
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

    private string GetdeptkmAll(string dept)
    {
        DataSet ds = server.GetDataSet("  select (select '['+yskmCode+']'+ yskmMc  from bill_yskm where yskmCode= a.yskmCode) as showcode, * from bill_yskm_dept a where deptCode='" + dept + "'");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["showcode"]));
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
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            TextBox zjjetext = e.Item.FindControl("TextBox2") as TextBox;
            TextBox smtext = e.Item.FindControl("txtShuoming") as TextBox;
            string gcbh = e.Item.Cells[1].Text.Trim();
            string strsql = @"  select * from bill_ysmxb where billCode='" + billCode + "' and gcbh='" + gcbh + "'";
            DataTable dt = server.GetDataTable(strsql, null);
            if (dt!=null&&dt.Rows.Count!=0)
            {
                zjjetext.Text = dt.Rows[0]["ysje"].ToString();
                smtext.Text = dt.Rows[0]["sm"].ToString();
            }
            
        }
    }
}
