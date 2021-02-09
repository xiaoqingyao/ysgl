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
using Bll;

public partial class ysgl_yszjAdd : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    YsManager ysmgr = new YsManager();

    string strNowDeptCode = "";
    string strNowDeptName = "";
    string strCsDeptCode;
    string strDeptCodes;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["gcbh"])))
            {
                Label2.Text = "当前预算过程为：" + server.GetCellValue("select xmmc  from biLL_ysgc where gcbh='" + Convert.ToString(Request.QueryString["gcbh"]) + "'");
            }
            if (!string.IsNullOrEmpty(Request["xmcode"]))
            {
                Label2.Text += "  ；当前选择项目：" + server.GetCellValue("  select '['+xmCode+']'+xmName from bill_xm where xmCode='" + Request["xmcode"].ToString() + "'");
            }
            if (Request["deptcode"] != null)
            {
                strCsDeptCode = Request["deptcode"].ToString().Trim();
            }
            Ajax.Utility.RegisterTypeForAjax(typeof(ysgl_yszjAdd));


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
            else
            {
                if (isTopDept("y", usercode))
                {
                    if (strCsDeptCode != "")
                    {
                        strCsDeptCode = Request["deptcode"].ToString().Trim();
                        strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode='" + strCsDeptCode + "'");
                        strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode='" + strCsDeptCode + "'");

                        dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strDeptCodes + ") and deptCode not in ('" + strCsDeptCode + "')", null);

                    }
                    else
                    {
                        //获取当前用户所在的部门编号及其部门名称
                        strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                        strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                        dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strDeptCodes + ") and deptCode not in (" + strNowDeptCode + ")", null);

                    }


                }
                else
                {
                    //上级部门
                    strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
                    strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
                }
            }

            if (!IsPostBack)
            {
                #region 绑定人员管理下的部门
                //if (!strNowDeptCode.Equals(""))
                //{
                //获取人员管理下的部门
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
                    }
                    if (strNowDeptCode != "")
                    {
                        this.LaDept.Items.Insert(0, new ListItem("[" + strNowDeptCode + "]" + strNowDeptName, strNowDeptCode));
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
                //string gcbh = Page.Request.QueryString["gcbh"].ToString().Trim();
                //DataSet temp = server.GetDataSet("select * from bill_ysgc where gcbh='" + gcbh + "'");
                //this.Label1.Text = temp.Tables[0].Rows[0]["xmmc"].ToString().Trim();
                //绑定预算类型
                this.ddlYsType.DataSource = new sqlHelper.sqlHelper().GetDataTable("select * from bill_dataDic where dictype='18'", null);
                this.ddlYsType.DataTextField = "dicName";
                this.ddlYsType.DataValueField = "dicCode";
                this.ddlYsType.DataBind();
                this.ddlYsType.SelectedValue = "02";//默认费用预算
                this.bindData();
            }
        }
    }

    void bindData()
    {
        //string deptGuid = server.GetCellValue("select userdept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
        if (LaDept.SelectedValue != null)
        {
            string dydj = "02";
            if (!string.IsNullOrEmpty( this.ddlYsType.SelectedValue ))
            {
                dydj = this.ddlYsType.SelectedValue.Trim();
            }
            string deptGuid = LaDept.SelectedValue.Trim();
            DataSet temp = server.GetDataSet("select yskmCode,yskmBm,replicate('　　',len(yskmCode)-2)+yskmmc as yskmMc,(case tblx when '01' then '单位填报' when '02' then '<font color=red>财务填报</font>' end) as tblx from bill_yskm where yskmcode in (select yskmcode from bill_yskm_dept where deptCode='" + deptGuid + "' and dydj='" + dydj + "' ) or tblx='02' order by yskmCode");
            this.myGrid.DataSource = temp;
            this.myGrid.DataBind();
        }

        if (!string.IsNullOrEmpty(Request["billCode"]))
        {
            string fujian = server.GetCellValue("select top 1 note2 from bill_main where billcode='" + Request["billCode"] + "'");
            if (!string.IsNullOrEmpty(fujian))
            {
                string[] arrTemp = fujian.Split('|');
                Lafilename.Text = "我的附件";
                upLoadFiles.Visible = false;
                btn_sc.Text = "新增附件";
                string[] arrname = arrTemp[0].Split(';');
                string[] arrfile = arrTemp[1].Split(';');
                for (int i = 0; i < arrname.Length - 1; i++)
                {
                    Literal1.Text += "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>附件" + (i + 1) + "：</span><a href='../../AFrame/download.aspx?filename=" + Server.UrlEncode(arrname[i]) + "&filepath=" + Server.UrlEncode(arrfile[i]) + "' target='_blank'>" + arrname[i] + "下载;</a></div>";
                }
                Lafilename.Text = arrTemp[0];//显示名
                hidfilnename.Value = arrTemp[0];
                hiddFileDz.Value = arrTemp[1];
            }
            else
            {
                //如果没有附件的话
                btn_sc.Text = "上 传";
                Lafilename.Text = "";
                hidfilnename.Value = "";
                upLoadFiles.Visible = true;
                hiddFileDz.Value = "";
            }
        }
        //  string deptGuid = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string strdeptcode = this.LaDept.SelectedValue.ToString().Trim();
        if (!string.IsNullOrEmpty(Request["xmcode"]))
        {
            Response.Redirect("yszjFrame.aspx?deptcode=" + strdeptcode + "&page=yszj&xmcode=" + Request["xmcode"] + "&isdz=1&xmzj=1");
        }
        else
        {
            Response.Redirect("yszjFrame.aspx?deptcode=" + strdeptcode + "&page=yszj");
        }

        //Response.Redirect("yszjList.aspx?gcbh=" + Page.Request.QueryString["gcbh"].ToString().Trim());
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
        string deptGuid = this.LaDept.SelectedValue.Trim();

        string ysGuid = (new GuidHelper()).getNewGuid();
        string loopTimes = "1";
        List<string> list = new List<string>();
        decimal yszje = 0;
        string errorMsg = "";


        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            string yskm = this.myGrid.Items[i].Cells[0].Text.ToString().Trim();
            TextBox txt = (TextBox)this.myGrid.Items[i].FindControl("TextBox2");
            string shuoming = ((TextBox)this.myGrid.Items[i].FindControl("txtShuoming")).Text.Trim();
            try
            {


                decimal ysje = decimal.Parse(txt.Text.ToString().Trim());



                if (ysje == 0)
                { }
                else
                {

                    if (shuoming.Equals(""))
                    {
                        string yskmmc = this.myGrid.Items[i].Cells[2].Text.ToString().Trim();
                        errorMsg = "请输入第" + (i + 1) + "行" + yskmmc + "的追加说明";
                    }

                    #region 大智专用

                    decimal kyje = getkyys(yskm);//可用金额
                    if (ysje < 0)
                    {
                        decimal jdzzjje = System.Math.Abs(ysje);//追加绝对值

                        if (kyje < jdzzjje)
                        {
                            errorMsg = "请输入第" + (i + 1) + "行追加金额的绝对值不能小于剩余预算";
                        }
                    }

                    #endregion



                    //if (yskm.Length == 2)
                    //{
                    yszje += ysje;
                    //}
                    //写入预算明细
                    list.Add("insert into bill_ysmxb(gcbh,billCode,yskm,ysje,ysDept,ysType,sm) values('" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','" + ysGuid + "','" + yskm + "'," + ysje.ToString() + ",'" + deptGuid + "','2','" + shuoming + "')");
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

        //判断说明是不是为空

        //if (string.IsNullOrEmpty(txt_sm.Text) || txt_sm.Text.Length > 25)
        //{
        //    Response.Write("<script>alert('追加说明不能为空，字数不能多于25个字，保存失败！')</script>");
        //    return;
        //}


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
        //  fujian = escape(fjname + "|" + fjurl);
        //写入预算总表 记录
        list.Add("insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType,billName2,dydj,note2,note3) values('" + ysGuid + "','" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','" + flowid + "','-1','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + deptGuid + "'," + yszje.ToString() + ",'" + loopTimes + "','2','" + txt_sm.Text.Trim() + "','" + this.ddlYsType.SelectedValue + "','" + fujian + "','" + strxmcode + "')");
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
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('yszjList.aspx?isdz=1&xmzj=1&deptcode=" + strdeptcode + "','_self');", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('yszjList.aspx?deptcode=" + strdeptcode + "','_self');", true);

            }
            //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('ystbDetail.aspx?gcbh=" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','_self');", true);
        }
    }



    /// <summary>
    /// 大智专用 求剩余预算
    /// </summary>
    /// <param name="yskm"></param>
    /// <returns></returns>
    public decimal getkyys(string yskm)
    {
        decimal kyje = 0;
        string gcbh = Request.QueryString["gcbh"];
        string deptCode = this.LaDept.SelectedValue.ToString().Trim();
        //1.计算剩余预算
        decimal yysje = yysje = ysmgr.GetYueYs(gcbh, deptCode, yskm);//预算金额
        decimal hfje = 0;
        if (yskm.IndexOf("0234") > 0)//退费
        {
            hfje = ysmgr.GetYueHf_tf(gcbh, deptCode, yskm, "02");//花费金额    
        }
        else
        {
            hfje = ysmgr.GetYueHf(gcbh, deptCode, yskm, "02");//花费金额    
        }
        decimal syje = yysje - hfje;
        decimal tcje = 0;
        bool hasSaleRebate = new ConfigBLL().GetValueByKey("HasSaleRebate").Equals("1");

        if (hasSaleRebate)
        {
            tcje = ysmgr.getEffectiveSaleRebateAmount(deptCode, yskm);//提成金额
        }
        kyje = syje + tcje;//可用金额

        return kyje;


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
    /// <param name="currentCode"></param>
    /// <param name="arrIndex"></param>
    /// <param name="arrCode"></param>
    /// <param name="arrVal"></param>
    /// <returns></returns>

    [Ajax.AjaxMethod(HttpSessionStateRequirement.Read)]
    public string[] getCalResult(string currentCode, int[] arrIndex, string[] arrCode, string[] arrVal)
    {
        string[] returnVal = new string[arrIndex.Length];
        int len = currentCode.Length;
        while (len >= 4)
        {
            double dValue = 0;
            for (int i = 0; i <= arrIndex.Length - 1; i++)
            {
                if (arrCode[i].Length == len && arrCode[i].Substring(0, len - 2) == currentCode.Substring(0, len - 2))//同级编号
                {
                    dValue += double.Parse(arrVal[i]);
                }
            }
            //找到上级并赋值
            for (int i = 0; i <= arrIndex.Length - 1; i++)
            {
                string cCode = currentCode.Substring(0, len - 2);
                if (arrCode[i] == cCode)//找到上级
                {
                    arrVal[i] = dValue.ToString();
                }
            }
            len = len - 2;
        }

        for (int i = 0; i <= arrIndex.Length - 1; i++)
        {
            returnVal[i] = arrIndex[i].ToString().Trim() + "," + double.Parse(arrVal[i].ToString().Trim()).ToString("0.00");
        }
        return returnVal;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LaDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        decimal kyys = 0;
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {

            TextBox zjtext = e.Item.FindControl("TextBox2") as TextBox;
            string kmcode = Convert.ToString(e.Item.Cells[0].Text.Trim());
            if (LaDept.SelectedValue != null)
            {
                //select count(*)  from bill_yskm_dept where deptCode='" + dept + "' AND  yskmCode LIKE '" + kmcode + "%'
                string dept = LaDept.SelectedValue.Trim();
                int count = Convert.ToInt32(server.GetCellValue(@" select (select count(*) from bill_yskm where yskmcode like b.yskmcode+'%' and len(yskmcode)>len(b.yskmcode))as ss  from bill_yskm_dept b
	              where yskmcode in( select yskmcode from bill_yskm_dept where deptcode='" + dept + "')  and deptcode='" + dept + "' 	and yskmcode='" + kmcode + "'"));
                if (count != 0)
                {
                    zjtext.Enabled = false;
                }
            }
            #region 大智要求，其他客户可以不更新
            kyys = getkyys(kmcode);
            e.Item.Cells[3].Text = kyys.ToString();
            #endregion

        }
    }

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

    protected void ddlYsType_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
    }
}
