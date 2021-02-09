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
using System.Text;
using System.Data.SqlClient;
using Bll.UserProperty;
using Bll;

public partial class ysgl_yszjEdit : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    YsManager ysmgr = new YsManager();
    string strdeptcode = "";
    string strgcbh = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(ysgl_yszjEdit));
            if (!IsPostBack)
            {

                string ysGuid = Page.Request.QueryString["billCode"].ToString().Trim();


                strgcbh = server.GetCellValue("select billName from bill_main where billCode='" + ysGuid + "'");
                lbl_ysgc.Text = strgcbh;

                //string gcbh = Page.Request.QueryString["gcbh"].ToString().Trim();
                //DataSet temp = server.GetDataSet("select * from bill_ysgc where gcbh='" + gcbh + "'");
                //this.Label1.Text = temp.Tables[0].Rows[0]["xmmc"].ToString().Trim();
                //给部门和预算类型赋值
                DataTable dtDept = server.GetDataTable("select (select '['+xmCode+']'+xmName from bill_xm where xmCode=note3 ) as xm,billDept,(select deptname from bill_departments where deptcode=bill_main.billDept) as deptname,dydj,(select dicName from bill_dataDic where diccode=bill_main.dydj and dictype='18') as dydjname from bill_main where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'", null);
                if (dtDept != null && dtDept.Rows.Count != 0)
                {
                    this.LaDept.Items.Add(new ListItem(dtDept.Rows[0]["deptname"].ToString(), dtDept.Rows[0]["billDept"].ToString()));
                    this.ddlYsType.Items.Add(new ListItem(dtDept.Rows[0]["dydjname"].ToString(), dtDept.Rows[0]["dydj"].ToString()));
                    if (!string.IsNullOrEmpty(dtDept.Rows[0]["xm"].ToString().Trim()))
                    {
                        this.Label1.Text = "项目追加，当前选择项目：" + dtDept.Rows[0]["xm"].ToString().Trim();
                    }

                }
                //控制审批意见区域的显示与隐藏
                if (Page.Request.QueryString["type"].ToString().Trim() == "look")
                {
                    tr_shyj_history.Visible = true;
                    //显示历史驳回意见
                    DataTable dtHisMind = server.GetDataTable("select * from bill_ReturnHistory where billcode='" + Request.QueryString["billCode"] + "' order by dt desc", null);
                    if (dtHisMind.Rows.Count == 0)
                    {
                        this.txt_shyj_History.InnerHtml = "无";
                    }
                    else
                    {
                        StringBuilder sbMind = new StringBuilder();
                        for (int i = 0; i < dtHisMind.Rows.Count; i++)
                        {
                            sbMind.Append("<br/>");

                            sbMind.Append("&nbsp;&nbsp;驳回人：");
                            sbMind.Append(dtHisMind.Rows[i]["usercode"].ToString());
                            sbMind.Append("&nbsp;&nbsp;驳回时间：");
                            sbMind.Append(dtHisMind.Rows[i]["dt"].ToString());
                            sbMind.Append("<br/>");
                            sbMind.Append("&nbsp;&nbsp;驳回意见：");
                            sbMind.Append(dtHisMind.Rows[i]["mind"].ToString());
                            sbMind.Append("<br/>");
                            sbMind.Append("<hr/>");
                        }
                        this.txt_shyj_History.InnerHtml = sbMind.ToString();
                    }
                }
                else
                {
                    tr_shyj_history.Visible = tr_shyj.Visible = btn_ok.Visible = btn_cancel.Visible = false;

                }

                this.bindData();
            }
        }
    }

    void bindData()
    {
        if (Page.Request.QueryString["type"].ToString().Trim() == "look")
        {
            this.Button1.Visible = false;
            //this.myGrid.Columns[3].Visible = false;
            txt_sm.Enabled = false;
            Button2.Text = "关  闭";

        }
        string deptGuid = server.GetCellValue("select billDept from bill_main where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        strdeptcode = deptGuid;
        lbl_dept.Text = deptGuid;
        string strsm = server.GetCellValue("select billName2 from bill_main where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        txt_sm.Text = strsm;
        DataSet temp = server.GetDataSet("exec bill_pro_yszjmxb_dept '" + Page.Request.QueryString["billCode"].ToString().Trim() + "','" + deptGuid + "'");
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();

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
                    Literal1.Text += "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>附件" + (i + 1) + "：</span><a href='../../AFrame/download.aspx?filename=" + Server.UrlDecode(arrname[i]) + "&filepath=" + Server.UrlDecode(arrfile[i]) + "' target='_blank'>" + arrname[i] + "下载;</a></div>";
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

            DataTable shdt = GetData();
            if (shdt != null)
            {
                StringBuilder sbxx = new StringBuilder();
                for (int i = 0; i < shdt.Rows.Count; i++)
                {
                    sbxx.Append("<br/>");

                    sbxx.Append("&nbsp;&nbsp;审批人：");
                    sbxx.Append(shdt.Rows[i]["checkuser"].ToString());
                    sbxx.Append("&nbsp;&nbsp;审批状态：");
                    sbxx.Append(shdt.Rows[i]["wsrdstate"].ToString());
                    sbxx.Append("&nbsp;&nbsp;审批时间：");
                    sbxx.Append(shdt.Rows[i]["checkdate1"].ToString());
                    sbxx.Append("<br/>");
                    sbxx.Append("&nbsp;&nbsp;审批意见：");
                    sbxx.Append(shdt.Rows[i]["mind"].ToString());
                    sbxx.Append("<br/>");
                    sbxx.Append("<hr/>");
                }
                this.txt_shxx_history.InnerHtml = sbxx.ToString();
            }
        }
    }
    private DataTable GetData()
    {
        string strBillCode = "";
        if (!string.IsNullOrEmpty(Request["billCode"]))
        {
            strBillCode = Request["billCode"].ToString();
        }
        List<SqlParameter> lstParameter = new List<SqlParameter>();
        StringBuilder sb = new StringBuilder();
        sb.Append(@" select Row_Number()over(order by w.flowid asc,ws.stepid asc) as crow,w.billcode,billtype,w.flowid,isEdit,
                   w.rdState as wrdstate, 
                   stepid,steptext,recordtype, isnull((select '['+usercode+']'+username from bill_users where usercode=ws.checkuser),checkuser) as checkuser,
                  isnull((select '['+usercode+']'+username from bill_users where usercode=ws.finaluser),finaluser) as finaluser,
                 (case when  ws.rdstate  ='1' then '正在进行'  when  ws.rdstate  ='0' then '等待' when  ws.rdstate  ='2' then'审核通过' when  ws.rdstate  ='3' then '驳回'  end)as wsrdstate,
                  mind, convert(varchar(10),ws.checkdate,121) as checkdate, ws.checkdate as checkdate1,checktype   
                  from workflowrecord w inner join   workflowrecords ws  on w.recordid =ws.recordid  ");
        sb.Append(" and  billCode  = @billCode ");
        lstParameter.Add(new SqlParameter("@billCode ", strBillCode));
        return server.GetDataTable(sb.ToString(), lstParameter.ToArray());
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        if (Page.Request.QueryString["type"] == "look")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
        }
        else
        {
            Response.Redirect("yszjList.aspx");
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        //获取当前人员的部门编号

        string deptGuid = "";// lbl_dept.Text;//new Bll.UserProperty.UserMessage(Session["usercode"].ToString()).GetDept().DeptCode;
        string ysGuid = Page.Request.QueryString["billCode"].ToString().Trim();
        string sql = "select * from bill_main where billCode='" + ysGuid + "'";
        DataTable dtmain = server.GetDataTable(sql,null);
        string loopTimes = "";
        string strzdr = "";
        if (dtmain!=null)
        {
            loopTimes = dtmain.Rows[0]["loopTimes"].ToString();
            strzdr = dtmain.Rows[0]["billuser"].ToString();
            deptGuid = dtmain.Rows[0]["billdept"].ToString();
        }
        //loopTimes = server.GetCellValue("select loopTimes from bill_main where billCode='" + ysGuid + "'");
       // strzdr = server.GetCellValue("select billuser from bill_main where billCode='" + ysGuid + "'");
        List<string> list = new List<string>();
        decimal yszje = 0;
        string errorMsg = "";
        string gcbh = server.GetCellValue("select billName from bill_main where billCode='" + ysGuid + "'");
        list.Add("delete from bill_main where billCode='" + ysGuid + "'");
        list.Add("delete from bill_ysmxb where billCode='" + ysGuid + "'");
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
                    //list.Add("insert into bill_ysmxb(gcbh,billCode,yskm,ysje,ysDept,ysType) values('" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','" + ysGuid + "','" + yskm + "'," + ysje.ToString() + ",'" + deptGuid + "','2')");
                    //list.Add("insert into bill_ysmxb(gcbh,billCode,yskm,ysje,ysDept,ysType) values('" + gcbh + "','" + ysGuid + "','" + yskm + "'," + ysje.ToString() + ",'" + deptGuid + "','2')");
                    list.Add("insert into bill_ysmxb(gcbh,billCode,yskm,ysje,ysDept,ysType,sm) values('" + gcbh + "','" + ysGuid + "','" + yskm + "'," + ysje.ToString() + ",'" + deptGuid + "','2','" + shuoming + "')");

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

        //写入预算总表 记录
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
        list.Add("insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType,billName2,dydj,note2) values('" + ysGuid + "','" + gcbh + "','yszj','-1','" + strzdr + "','" + System.DateTime.Now.ToString() + "','" + deptGuid + "'," + yszje.ToString() + ",'" + loopTimes + "','2','" + txt_sm.Text.Trim() + "','02','" + fujian + "')");
        //list.Add("insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType) values('" + ysGuid + "','" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','yszj','-1','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString() + "','" + deptGuid + "'," + yszje.ToString() + ",'" + loopTimes + "','2')");
        list.Add("update bill_main set billJe=(select isnull(sum(isnull(ysje,0)),0) from bill_ysmxb where billCode='" + ysGuid + "') where billCode='" + ysGuid + "'");

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            Response.Redirect("yszjList.aspx");
            //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('yszjList.aspx?gcbh=" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','_self');", true);
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
        string gcbh = "";
        if (string.IsNullOrEmpty(strgcbh))
        {
            gcbh = lbl_ysgc.Text;
        }
        else
        {
            gcbh = strgcbh;
        }
        string deptCode = "";
        if (string.IsNullOrEmpty(strdeptcode))
        {
            deptCode = LaDept.SelectedValue;
        }
        else
        {
            deptCode = strdeptcode;
        }

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
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        decimal kyys = 0;
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {


            string kmcode = Convert.ToString(e.Item.Cells[0].Text.Trim());

            #region 大智要求，其他客户可以不更新
            kyys = getkyys(kmcode);
            e.Item.Cells[3].Text = kyys.ToString();
            #endregion

        }
    }
}
