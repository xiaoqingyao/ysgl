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

public partial class ysgl_yszjAddhz : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    string strNowDeptCode = "";
    string strNowDeptName = "";
    string isxmhz = "";
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
            Ajax.Utility.RegisterTypeForAjax(typeof(ysgl_yszjAddhz));


            string usercode = Session["userCode"].ToString().Trim();
            strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");
            if (!IsPostBack)
            {
                this.bindData();
                if (!string.IsNullOrEmpty(Page.Request.QueryString["billCode"]))
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
                    tr_shyj_history.Visible = false;

                }

            }
        }
    }
    /// <summary>
    /// 显示明细
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_xsmx_Click(object sender, EventArgs e)
    {
        string billcode = "";

        string strcodes = hidcodes.Value;
        if (!string.IsNullOrEmpty(strcodes))
        {
            strcodes = strcodes.Substring(1, strcodes.Length - 2);
            string[] strs = strcodes.Split(';');
            for (int i = 0; i < strs.Length; i++)
            {
                if (!string.IsNullOrEmpty(strs[i]))
                {
                    billcode += "'" + strs[i].Trim() + "',";
                }

            }
            if (!string.IsNullOrEmpty(billcode))
            {
                billcode = billcode.Substring(0, billcode.Length - 1);
            }
            else
            {
                Response.Write("<script>alert('请选择检查选择的追加单是否有误！')</script>");
                return;
            }


            string strsql = @" select (select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=ysmxb.yskm)as kmname,(select '['+xmCode+']'+xmName from bill_xm where xmCode=main.note3 ) as xm,* from bill_main main,bill_ysmxb ysmxb 
	  where main.billCode=ysmxb.billCode and ysType='2' and stepID='end'   and main.billCode in(" + billcode + ")";//and gcbh ='" + Page.Request.QueryString["gcbh"].ToString().Trim() + "'
            if (!string.IsNullOrEmpty(Request["isxm"]))
            {
                strsql += " and isnull(main.note3,'')!=''";
            }
            else
            {
                strsql += " and isnull(main.note3,'')=''";
            }
            //   Response.Write(strsql);
            DataSet temp = server.GetDataSet(strsql);
            this.myGrid.DataSource = temp;
            this.myGrid.DataBind();
        }


    }
    void bindData()
    {


        if (!string.IsNullOrEmpty(Request["billCode"]))
        {

            string strsql = @" select (select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=ysmxb.yskm)as kmname,(select '['+xmCode+']'+xmName from bill_xm where xmCode=main.note3 ) as xm,* from bill_main main,bill_ysmxb ysmxb 
	  where main.billCode=ysmxb.billCode    and main.billCode ='" + Request["billCode"] + "'";//and gcbh ='" + Page.Request.QueryString["gcbh"].ToString().Trim() + "'
            if (Request["type"] == "look")
            {
                if (!string.IsNullOrEmpty(Request["xmcode"]))
                {
                    strsql += " and isnull(main.note3,'')='" + Request["xmcode"].ToString() + "'";
                }
                else
                {
                    strsql += " and isnull(main.note3,'')=''";
                }
            }
            

            DataSet temp = server.GetDataSet(strsql);
            this.myGrid.DataSource = temp;
            this.myGrid.DataBind();
            btn_selectyszj.Visible = false;
            btn_xsmx.Visible = false;
            tr_zjmx.Visible = tr_hzje.Visible = btn_hz.Visible = myGridhz.Visible = Button2.Visible = Button1.Visible = false;
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
            else
            {
                this.txt_shxx_history.Visible = false;
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
    /// <summary>
    /// 返回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        string strdeptcode = Request["deptcode"].ToString();
        if (!string.IsNullOrEmpty(Request["ishz"]))
        {
            if (!string.IsNullOrEmpty(Request["xmcode"]))
            {
                Response.Redirect("yszjFrame.aspx?deptcode=" + strdeptcode + "&page=yszj&xmcode=" + Request["xmcode"] + "&isdz=1&isxm=1&ishz=1");
            }
            else
            {
                Response.Redirect("yszjFrame.aspx?deptcode=" + strdeptcode + "&page=yszj&ishz=1");
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(Request["xmcode"]))
            {
                Response.Redirect("yszjFrame.aspx?deptcode=" + strdeptcode + "&page=yszj&xmcode=" + Request["xmcode"] + "&isdz=1&xmzj=1");
            }
            else
            {
                Response.Redirect("yszjFrame.aspx?deptcode=" + strdeptcode + "&page=yszj");
            }

        }

    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        //获取当前人员的部门编号
        string deptGuid = Request["deptcode"].ToString();

        string ysGuid = (new GuidHelper()).getNewGuid();
        string loopTimes = "1";
        List<string> list = new List<string>();
        double yszje = 0;
        string errorMsg = "";
        if (myGridhz.Items.Count == 0)
        {
            Response.Write("<script>alert('请先汇总明细再保存！')</script>");
            return;
        }
        for (int i = 0; i <= this.myGridhz.Items.Count - 1; i++)
        {
            string yskm = this.myGridhz.Items[i].Cells[0].Text.ToString().Trim();
            if (!string.IsNullOrEmpty(yskm))
            {
                yskm = yskm.Substring(1, yskm.IndexOf("]") - 1);
            }

            try
            {
                string strje = this.myGridhz.Items[i].Cells[1].Text.ToString().Trim();
                double ysje = double.Parse(strje);
                string shuoming = this.myGridhz.Items[i].Cells[2].Text.ToString().Trim();
                if (ysje == 0)
                { }
                else
                {

                    yszje += ysje;
                    list.Add(@"insert into bill_ysmxb(gcbh,billCode,yskm,ysje,ysDept,ysType,sm)
                    values('" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','" + ysGuid + "','" + yskm + "'," + ysje.ToString() + ",'" + deptGuid + "','2','" + shuoming + "')");
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
        list.Add("insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType,billName2,dydj,note2,note3) values('" + ysGuid + "','" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','yszjhz','-1','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString() + "','" + deptGuid + "'," + yszje.ToString() + ",'" + loopTimes + "','2','" + txt_sm.Text.Trim() + "','02','" + fujian + "','" + strxmcode + "')");
        list.Add("update bill_main set billJe=(select isnull(sum(isnull(ysje,0)),0) from bill_ysmxb where billCode='" + ysGuid + "') where billcode='" + ysGuid + "'");

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            return;
        }
        else
        {
            List<string> listdy = new List<string>();

            string strcodes = hidcodes.Value;
            if (!string.IsNullOrEmpty(strcodes))
            {
                strcodes = strcodes.Substring(1, strcodes.Length - 2);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(),"","alert('请先选中需要汇总的追加单')",true);
                return;
            }
        
            string[] strs = strcodes.Split(';');
            for (int i = 0; i < strs.Length; i++)
            {
                if (!string.IsNullOrEmpty(strs[i]))
                {
                    listdy.Add("insert into dz_yksq_bxd(yksq_code,bxd_code,note1)values('" + strs[i] + "','" + ysGuid + "','zjhz')");

                }
            }
            if (listdy.Count > 0)
            {
                if (server.ExecuteNonQuerysArray(listdy) == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
                    return;
                }

            }

            string strdeptcode = Request["deptcode"].ToString();
            if (!string.IsNullOrEmpty(strxmcode))
            {

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('yszjListhz.aspx?isdz=1&isxm=1&deptcode=" + strdeptcode + "','_self');", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('yszjListhz.aspx?deptcode=" + strdeptcode + "','_self');", true);

            }
        }
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (string.IsNullOrEmpty(Request["isxm"]))
        {
            e.Item.Cells[3].CssClass = "hiddenbill";
        }
    }

    /// <summary>
    /// 上传附件
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
    /// <summary>
    /// 汇总明细
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_hz_Click(object sender, EventArgs e)
    {
        string billcode = "";

      

        string strcodes = hidcodes.Value;
        if (!string.IsNullOrEmpty(strcodes))
        {
            strcodes = strcodes.Substring(1, strcodes.Length - 2);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请先选中需要汇总的追加单')", true);
            return;
        }
        string[] strs = strcodes.Split(';');
        for (int i = 0; i < strs.Length; i++)
        {
            if (!string.IsNullOrEmpty(strs[i]))
            {
                billcode += "'" + strs[i].Trim() + "',";
            }

        }
        if (!string.IsNullOrEmpty(billcode))
        {
            billcode = billcode.Substring(0, billcode.Length - 1);
        }
        else
        {
            Response.Write("<script>alert('请选择需要汇总的追加单后汇总！')</script>");
            return;
        }

        string strsql = @" select (select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=ysmxb.yskm)as kmname,(select '['+xmCode+']'+xmName from bill_xm where xmCode=main.note3 ) as xm,sum(ysmxb.ysje) as ysje,sm  from bill_main main,bill_ysmxb ysmxb 
	  where main.billCode=ysmxb.billCode   and main.billCode in(" + billcode + ") group by yskm,sm,note3";//and gcbh ='" + Page.Request.QueryString["gcbh"].ToString().Trim() + "'


        DataSet temp = server.GetDataSet(strsql);
        this.myGridhz.DataSource = temp;
        this.myGridhz.DataBind();
    }
}
