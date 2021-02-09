using Bll.UserProperty;
using Dal.Bills;
using Dal.UserProperty;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_ysglnew_ystbHzDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string ctrl = "add";
    string billcode = string.Empty;
    string checking = string.Empty;//从审核页面中传过来的状态 如果是正在执行 则显示审批区域 否则的话不显示审批区域
    string deptcodes = string.Empty;//传入的deptcodes 加这个参数的目的是为了让预算填报页面的汇总也可以用这个页面，不传入billcode 而是直接传入deptcodes
    string fromurl = string.Empty;//上游url
    string gkdept = string.Empty;//归口部门编号   如果这个页面是用于财务预算汇总 这个参数就没有   如果这个页面是用于预算填报 归口部门查看预算明细  则需要提供这个
    string djlx = "02";//预算科目的属性：单据类型也就是预算类型 01收入 02费用 默认02费用
    string xmcode = "";
    //string isxmys = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        #region 获取url参数
        object objCtrl = Request["ctrl"];
        if (objCtrl != null)
        {
            ctrl = objCtrl.ToString();
        }
        object objBillCode = Request["billcode"];
        if (objBillCode != null)
        {
            billcode = objBillCode.ToString();
        }
        object objchecking = Request["checking"];
        if (objchecking != null)
        {
            checking = objchecking.ToString();
        }
        object objdeptcodes = Request["deptcodes"];
        if (objdeptcodes != null)
        {
            deptcodes = objdeptcodes.ToString();
        }
        object objgkdept = Request["gkdept"];
        if (objgkdept != null)
        {
            gkdept = objgkdept.ToString();
        }
        object objDjlx = Request["djlx"];
        if (objDjlx != null)
        {
            djlx = objDjlx.ToString();
        }
        #endregion

        if (!IsPostBack)
        {
            //绑定年度
            string selectndsql = "select nian,xmmc from bill_ysgc where   yue='' order by nian desc";
            DataTable selectdt = server.GetDataTable(selectndsql, null);
            drpSelectNd.DataSource = selectdt;
            drpSelectNd.DataTextField = "xmmc";
            drpSelectNd.DataValueField = "nian";
            drpSelectNd.DataBind();
            if (!ctrl.Equals("bb"))
            {
                this.lbl_masge.Text = "请先单击选择按钮选择部门 然后再生成汇总表";
            }
            if (ctrl.Equals("edit") || ctrl.Equals("view") || ctrl.Equals("audit"))
            {
                if (!billcode.Equals(""))
                {
                    Bill_Main main = new MainDal().GetMainByCode(billcode);
                    this.hddepts.Value = main.BillName2;
                    this.hdshowdepts.Value = main.Note1;
                    this.drpSelectNd.SelectedValue = main.Note2;
                }
                else if (!deptcodes.Equals(""))
                {
                    List<string> lstdepts = new List<string>();
                    string[] arrdepts = deptcodes.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    StringBuilder sb = new StringBuilder();
                    foreach (string deptcode in arrdepts)
                    {
                        sb.Append(new DepartmentBLL().GetShowNameByCode(deptcode) + ",");
                    }
                    this.hdshowdepts.Value = sb.ToString();
                    this.hddepts.Value = deptcodes;
                }
                showcontent();
                //控制按钮
                if (ctrl.Equals("view") || ctrl.Equals("audit"))
                {
                    Button1.Visible = false;//生成汇总
                    select.Visible = false;//选择部门
                    btn_save.Visible = false;//保存
                }
                if (ctrl.Equals("audit") && checking.Equals("true"))//如果是审核  并且审批流正好在当前用户这
                {
                    pShenHe.Visible = true;
                    returnlist.Visible = false;
                }
                else if (ctrl.Equals("audit"))
                {
                    returnlist.Visible = false;
                }
            }
            else if (ctrl.Equals("bb"))//报表
            {
                Button1.Visible = false;//生成汇总
                select.Visible = false;//选择部门
                btn_save.Visible = false;//保存
                pShenHe.Visible = false;
                returnlist.Visible = false;
                returnlist.Visible = false;
                this.hddepts.Value = "";
                divHzbm.Visible = false;
                showcontent();
            }
        }
    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        showcontent();
    }
    private void showcontent()
    {
        this.lblDepts.Text = this.hdshowdepts.Value;
        string deptcodes = this.hddepts.Value;
        string nd = drpSelectNd.SelectedValue;
        if (!string.IsNullOrEmpty(Request["xmcode"]))
        {
            xmcode = Request["xmcode"].ToString();
        }
        if (string.IsNullOrEmpty(deptcodes) && !ctrl.Equals("bb"))
        {
            Response.Write("<script>alert('请先单击选择按钮选择部门 然后再生成汇总表')</script>");
            return;
        }
        if (dept.Checked)
        {
            string sql = "exec pro_dept_ys '" + nd + "','" + deptcodes + "','" + gkdept + "','dept','" + djlx + "','" + xmcode + "'";
            Response.Write(sql);
            DataTable dtrel = server.GetDataTable(sql, null);

            this.GridView2.DataSource = dtrel;
            this.GridView2.DataBind();
        }
        else
        {
            string strsql = @"exec pro_dept_ys '" + nd + "','" + deptcodes + "','" + gkdept + "','yskm','" + djlx + "','" + xmcode + "'";
            Response.Write(strsql);
            DataTable dtrel = server.GetDataTable(strsql, null);
            this.GridView1.DataSource = dtrel;
            this.GridView1.DataBind();
        }

    }
    decimal yi = 0, er = 0, san = 0, si = 0, wu = 0, liu = 0, qi = 0, ba = 0, jiu = 0, shi = 0, shiyi = 0, shier = 0, nian = 0;
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            string strdeptname;
            string stryskmname;
            if (yskm.Checked)
            {
                strdeptname = e.Row.Cells[2].Text.Trim();
                stryskmname = e.Row.Cells[1].Text.Trim();
            }
            else
            {
                strdeptname = e.Row.Cells[1].Text.Trim();
                stryskmname = e.Row.Cells[2].Text.Trim();
            }

            decimal nianeve = decimal.Parse(e.Row.Cells[3].Text.Trim());
            decimal yieve = decimal.Parse(e.Row.Cells[4].Text.Trim());
            decimal ereve = decimal.Parse(e.Row.Cells[5].Text.Trim());
            decimal saneve = decimal.Parse(e.Row.Cells[6].Text.Trim());
            decimal sieve = decimal.Parse(e.Row.Cells[7].Text.Trim());
            decimal wueve = decimal.Parse(e.Row.Cells[8].Text.Trim());
            decimal liueve = decimal.Parse(e.Row.Cells[9].Text.Trim());
            decimal qieve = decimal.Parse(e.Row.Cells[10].Text.Trim());
            decimal baeve = decimal.Parse(e.Row.Cells[11].Text.Trim());
            decimal jiueve = decimal.Parse(e.Row.Cells[12].Text.Trim());
            decimal shieve = decimal.Parse(e.Row.Cells[13].Text.Trim());
            decimal shiyieve = decimal.Parse(e.Row.Cells[14].Text.Trim());
            decimal shiereve = decimal.Parse(e.Row.Cells[15].Text.Trim());
            if (strdeptname.IndexOf("小计") == -1 && stryskmname.IndexOf("小计") == -1)
            {
                yi += yieve;
                er += ereve;
                san += saneve;
                si += sieve;
                wu += wueve;
                liu += liueve;
                qi += qieve;
                ba += baeve;
                jiu += jiueve;
                shi += shieve;
                shiyi += shiyieve;
                shier += shiereve;
                nian += nianeve;
            }

            //添加空格
            string yskmbh = e.Row.Cells[13].Text.Trim();

            string yskmmc = "";
            if (yskm.Checked)
            {
                yskmmc = e.Row.Cells[1].Text.Trim();
            }
            else
            {
                yskmmc = e.Row.Cells[2].Text.Trim();
            }
            int count = yskmbh.Length - 2;
            for (int i = 0; i < count; i++)
            {
                yskmmc = "&nbsp;&nbsp;" + yskmmc;
            }
            if (yskm.Checked)
            {
                e.Row.Cells[1].Text = yskmmc;
            }
            else
            {
                e.Row.Cells[2].Text = yskmmc;
            }

            #region  如果是0则显示-
            if (nianeve == 0)
            {
                e.Row.Cells[3].Text = "-";
            }
            if (yieve == 0)
            {
                e.Row.Cells[4].Text = "-";
            }
            if (ereve == 0)
            {
                e.Row.Cells[5].Text = "-";
            }
            if (saneve == 0)
            {
                e.Row.Cells[6].Text = "-";
            }
            if (sieve == 0)
            {
                e.Row.Cells[7].Text = "-";
            }
            if (wueve == 0)
            {
                e.Row.Cells[8].Text = "-";
            }
            if (liueve == 0)
            {
                e.Row.Cells[9].Text = "-";
            }
            if (qieve == 0)
            {
                e.Row.Cells[10].Text = "-";
            }
            if (baeve == 0)
            {
                e.Row.Cells[11].Text = "-";
            }
            if (jiueve == 0)
            {
                e.Row.Cells[12].Text = "-";
            }
            if (shieve == 0)
            {
                e.Row.Cells[13].Text = "-";
            }
            if (shiyieve == 0)
            {
                e.Row.Cells[14].Text = "-";
            }
            if (shiereve == 0)
            {
                e.Row.Cells[15].Text = "-";
            }
            #endregion  如果是0则显示-
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[3].Text = nian.ToString();
            e.Row.Cells[4].Text = yi.ToString();
            e.Row.Cells[5].Text = er.ToString();
            e.Row.Cells[6].Text = san.ToString();
            e.Row.Cells[7].Text = si.ToString();
            e.Row.Cells[8].Text = wu.ToString();
            e.Row.Cells[9].Text = liu.ToString();
            e.Row.Cells[10].Text = qi.ToString();
            e.Row.Cells[11].Text = ba.ToString();
            e.Row.Cells[12].Text = jiu.ToString();
            e.Row.Cells[13].Text = shi.ToString();
            e.Row.Cells[14].Text = shiyi.ToString();
            e.Row.Cells[15].Text = shier.ToString();
            e.Row.Cells[2].Text = "合计";
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_Click(object sender, EventArgs e)
    {
        SysManager sysMgr = new SysManager();
        string ysGuid = (new GuidHelper()).getNewGuid();
        if (!string.IsNullOrEmpty(Request["xmcode"]))
        {
            xmcode = Request["xmcode"].ToString();
        }
        string strflowid = "yshz";
        string strbillname = sysMgr.GetYbbxBillName(strflowid, DateTime.Now.ToString("yyyMMdd"), 1);

        DepartmentDal depDal = new DepartmentDal();
        string strdept = "";
        if (Session["userCode"] != null && Session["userCode"].ToString().Trim() != "")
        {
            strdept = depDal.GetDeptByUser(Session["userCode"].ToString());
        }
        if (!string.IsNullOrEmpty(strdept))
        {
            strdept = strdept.Substring(1, strdept.IndexOf("]") - 1);
        }
        string strcn = drpSelectNd.SelectedValue;//财年

        string strbilldepts = this.hddepts.Value;//0101，0102
        string strdeptcodename = this.hdshowdepts.Value;//部门名称
        string strsql = "";
        if (ctrl.Equals("edit"))
        {
            strsql = "delete from bill_main where billcode='" + billcode + "';";
        }


        if (!string.IsNullOrEmpty(xmcode))
        {
            strflowid = "xmyshz";
        }
        strsql = @"insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType,billname2,note1,note2,note3) 
                                      values('" + ysGuid + "','" + strbillname + "','" + strflowid + "','-1','" + Session["userCode"].ToString() + "','" + System.DateTime.Now.ToString() + "','" + strdept + "','0','1','2','" + strbilldepts + "','" + strdeptcodename + "','" + strcn + "','" + xmcode + "')";
        int introw = server.ExecuteNonQuery(strsql, null);
        if (introw > 0)
        {
            if (!string.IsNullOrEmpty(xmcode))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.location.href='ystbHzList.aspx?xmys=1';", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.location.href='ystbHzList.aspx';", true);

            }

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！')", true);
            return;
        }
    }
    protected void yskm_CheckedChanged(object sender, EventArgs e)
    {
        if (yskm.Checked)
        {
            this.GridView1.Visible = true;
            this.GridView2.Visible = false;

        }
        else
        {
            this.GridView1.Visible = false;
            this.GridView2.Visible = true;
        }
        showcontent();
    }
    /// <summary>
    /// 导出excel表格
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        this.lblDepts.Text = this.hdshowdepts.Value;
        string deptcodes = this.hddepts.Value;
        string nd = drpSelectNd.SelectedValue;
        //  ExcelHelper excl = new ExcelHelper();
        DataTable dtrel = new DataTable();
        if (!string.IsNullOrEmpty(Request["xmcode"]))
        {
            xmcode = Request["xmcode"].ToString();
        }

        if (dept.Checked)
        {
            dtrel = server.GetDataTable("exec pro_dept_ys '" + nd + "','" + deptcodes + "','" + gkdept + "','dept','" + djlx + "','" + xmcode + "'", null);
            //excl.ExpExcel(dtrel, GridView2);
        }
        else
        {
            dtrel = server.GetDataTable("exec pro_dept_ys '" + nd + "','" + deptcodes + "','" + gkdept + "','yskm','" + djlx + "','" + xmcode + "'", null);
            //excl.ExpExcelRedirect(dtrel,"11",null);
        }
        int count = 0;

        Dictionary<string, string> dic = new Dictionary<String, String>();
        //dic.Add("billname", "单据编号");
        //dic.Add("billdeptname", "部门");
        //dic.Add("billUserName", "制单人");
        //dic.Add("billdate", "单据日期");
        //dic.Add("billJe", "单据金额");
        //dic.Add("isgk", "是否归口");
        //dic.Add("bxsm", "摘要");
        new ExcelHelper().ExpExcel(dtrel, "ExportFile", dic);
    }


    public delegate void MyDelegate(DataGrid gv);
    protected void DataTableToExcel(DataTable dtData, DataGrid stylegv, MyDelegate rowbound)
    {
        if (dtData != null)
        {
            // 设置编码和附件格式
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.Charset = "utf-8";

            // 导出excel文件
            // IO用于导出并返回excel文件
            StringWriter strWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(strWriter);

            DataGrid gvExport = new DataGrid();


            gvExport.AutoGenerateColumns = false;
            BoundColumn bndColumn = new BoundColumn();
            for (int j = 0; j < stylegv.Columns.Count - 1; j++)
            {
                bndColumn = new BoundColumn();
                if (stylegv.Columns[j] is BoundColumn)
                {
                    bndColumn.DataField = ((BoundColumn)stylegv.Columns[j]).DataField.ToString();
                    bndColumn.HeaderText = ((BoundColumn)stylegv.Columns[j]).HeaderText.ToString();

                    //添加一列
                    gvExport.Columns.Add(bndColumn);
                }
            }
            gvExport.DataSource = dtData.DefaultView;
            gvExport.AllowPaging = false;
            gvExport.DataBind();
            if (rowbound != null)
            {
                rowbound(gvExport);
            }

            // 返回客户端
            gvExport.RenderControl(htmlWriter);
            Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\">");
            Response.Write(strWriter.ToString());
            Response.Write("</body></html>");
            Response.End();
        }
    }
}