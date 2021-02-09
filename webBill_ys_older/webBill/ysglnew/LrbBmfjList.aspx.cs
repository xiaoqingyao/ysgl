using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Models;
using Bll.UserProperty;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;

public partial class webBill_ysglnew_LrbBmfjList : System.Web.UI.Page
{
    Dal.newysgl.Bmfj dal = new Dal.newysgl.Bmfj();

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["nd"] != null)
            {   //绑定项目
                string sql = "select procode,proname from bill_ys_benefitpro where fillintype = '明细汇总' and annual='" + Request.QueryString["nd"] + "' and status='1'  order by procode";
                DataTable dtrel = server.GetDataTable(sql, null);
                if (dtrel != null)
                {
                    this.ddlXm.Items.Clear();
                    for (int i = 0; i < dtrel.Rows.Count; i++)
                    {
                        ddlXm.Items.Add(new ListItem(dtrel.Rows[i]["proname"].ToString(), dtrel.Rows[i]["procode"].ToString()));
                    }
                    ddlXm.Items.Insert(0, new ListItem("--全部--", ""));
                }
            }

            if (Request.QueryString["yskm"] != null && Request.QueryString["nd"] != null)
            {

                SysManager sysmanager = new SysManager();
                if (sysmanager.GetYskmIsmj(Request.QueryString["yskm"].ToString()) == "0")
                {
                    btn_save.Enabled = true;
                    btn_qr.Enabled = true;
                    //  Bind();
                }
                else
                {
                    btn_save.Enabled = false;
                    btn_qr.Enabled = false;
                    Button1.Enabled = false;
                }
                Bind();
            }
            else if (Request.QueryString["nd"] != null)
            {
                btn_save.Enabled = false;
                btn_qr.Enabled = false;
                Button1.Enabled = false;
                bindAll(Request.QueryString["nd"]);
            }
            else
            {
                btn_save.Enabled = false;
                btn_qr.Enabled = false;
                Button1.Enabled = false;
            }
            if (new Bll.ConfigBLL().GetValueByKey("UseGKFJ").Equals("1"))
            {
                btn_edit.Visible = false;
            }
        }
    }

    /// <summary>
    /// 选择所有科目的时候
    /// </summary>
    /// <param name="p"></param>
    private void bindAll(string p)
    {
        DataTable dt = new DataTable();
        dt = getdt(p);
        //获取总金额
        //string strsql = "select sum(isnull(budgetmoney,0)) from bill_ys_xmfjlrb where annual=@niandu";
        //object obj = new sqlHelper.sqlHelper().GetCellValue(strsql, new SqlParameter[] { new SqlParameter("niandu", p) });
        //string strtotal = obj == null ? "0" : obj.ToString();
        //LaZje.Text = strtotal;

        //string strstatus = this.ddlStatus.SelectedValue.Trim();//状态
        //string strxmcode = this.ddlXm.SelectedValue;//利润项目

        //DataTable dt = new DataTable();
        //bool usegkfj = new Bll.ConfigBLL().GetValueByKey("UseGKFJ").Equals("1") ? true : false;
        //if (usegkfj)
        //{
        //    dt = new Dal.SysDictionary.YskmDal().GetGkDeptBynd(p, strstatus, strxmcode);
        //}
        //else
        //{
        //    dt = new Dal.SysDictionary.YskmDal().GetDeptByNd(p, strstatus, strxmcode);
        //}
        GridView1.DataSource = dt;
        GridView1.DataBind();
        RowsBound();
    }

    private DataTable getdt(string p)
    {
        //获取总金额
        string strsql = "select sum(isnull(budgetmoney,0)) from bill_ys_xmfjlrb where annual=@niandu";
        object obj = new sqlHelper.sqlHelper().GetCellValue(strsql, new SqlParameter[] { new SqlParameter("niandu", p) });
        string strtotal = obj == null ? "0" : obj.ToString();
        LaZje.Text = strtotal;

        string strstatus = this.ddlStatus.SelectedValue.Trim();//状态
        string strxmcode = this.ddlXm.SelectedValue;//利润项目

        DataTable dt = new DataTable();
        bool usegkfj = new Bll.ConfigBLL().GetValueByKey("UseGKFJ").Equals("1") ? true : false;
        if (usegkfj)
        {
            dt = new Dal.SysDictionary.YskmDal().GetGkDeptBynd(p, strstatus, strxmcode);
        }
        else
        {
            dt = new Dal.SysDictionary.YskmDal().GetDeptByNd(p, strstatus, strxmcode);
        }
        return dt;
    }

    private void Bind()
    {
        string yskm = Request.QueryString["yskm"].ToString();
        string nd = Request.QueryString["nd"].ToString();
        LaNd.Text = nd;
        LaKm.Text = new Dal.SysDictionary.YskmDal().GetYskmNameCode(yskm);
        this.hidkmcode.Value = LaKm.Text.Substring(1, LaKm.Text.IndexOf("]") - 1);
        LaZje.Text = decimal.Parse(dal.GetJebyfjb(yskm, nd)).ToString("N02");

        string strstatus = this.ddlStatus.SelectedValue.Trim();//状态
        string strxmcode = this.ddlXm.SelectedValue;//利润项目

        bool usegkfj = new Bll.ConfigBLL().GetValueByKey("UseGKFJ").Equals("1") ? true : false;
        DataTable dt = new DataTable();
        if (usegkfj)
        {
            dt = new Dal.SysDictionary.YskmDal().GetGkDeptByYskm(yskm, strstatus, strxmcode);
        }
        else
        {
            dt = new Dal.SysDictionary.YskmDal().GetDeptByYskm(yskm, strstatus, strxmcode);
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
        RowsBound();
    }
    double fTotalAmount = 0;//预算控制金额汇总
    double fSymoney = 0;// 剩余金额
    double fTotalShenbao = 0;//申报金额汇总
    double dTotalChae = 0;//总差额
    private void RowsBound()
    {
        string strstatus = this.ddlStatus.SelectedValue.Trim();//状态

        string yskm = "";
        string nd = Request.QueryString["nd"].ToString();

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            yskm = GridView1.Rows[i].Cells[8].Text.Trim();
            IList<bill_ys_xmfjbm> bmtb = dal.GetBmfjtb(nd, yskm);
            GridView1.Rows[i].Cells[0].Text = (i + 1).ToString();
            TextBox txtMoney = GridView1.Rows[i].FindControl("TextBox1") as TextBox;

            string deptcode = (GridView1.Rows[i].FindControl("hidddept") as HiddenField).Value;
            var temp = from p in bmtb
                       where p.deptcode == deptcode && p.kmcode == yskm
                       select p;
            if (temp.Count() > 0)
            {
                double dekongzhijine = double.Parse(temp.First().je.ToString());//预算控制金额
                txtMoney.Text = dekongzhijine.ToString("N02");
                #region 合计行
                double flAmount = 0;
                if (!string.IsNullOrEmpty(txtMoney.Text) && double.TryParse(txtMoney.Text, out flAmount))
                {
                    fTotalAmount += flAmount;
                }
                #endregion

                //申报金额合计
                Label lbleveShenBao = GridView1.Rows[i].Cells[4].FindControl("lbleveShenBao") as Label;

                if (temp.First().by1 == null || temp.First().by1 == "0.000")
                {
                    GridView1.Rows[i].Cells[4].Text = "";
                }
                else
                {
                    double deeveshebao = double.Parse(temp.First().by1);//建议金额
                    fTotalShenbao += deeveshebao;
                    lbleveShenBao.Text = deeveshebao == 0 ? "" : deeveshebao.ToString("N02");

                    //建议差额
                    if (deeveshebao != 0)
                    {
                        double dechae = deeveshebao - dekongzhijine;
                        GridView1.Rows[i].Cells[5].Text = dechae.ToString("N02");
                        dTotalChae += dechae;
                    }
                }
                GridView1.Rows[i].Cells[6].Text = temp.First().by2;
                string isstate = "";
                string statusflg = temp.First().by3;
                if (statusflg == "1")
                {
                    isstate = "预算确认";

                }
                else if (statusflg == "2")
                {
                    isstate = "部门确认";
                }
                else if (statusflg == "3")
                {
                    isstate = "部门异议";
                }
                else
                {
                    isstate = "未确认";
                }
                if (!strstatus.Equals(statusflg) && !strstatus.Equals(""))
                {
                    GridView1.Rows[i].Visible = false;
                }
                GridView1.Rows[i].Cells[7].Text = isstate;
                if (isstate == "部门确认")
                {
                    TextBox tb = GridView1.Rows[i].FindControl("TextBox1") as TextBox;
                    tb.ReadOnly = true;
                    tb.BackColor = System.Drawing.Color.LightGray;
                }
            }
            else
            {
                GridView1.Rows[i].Cells[7].Text = "未确认";
                if (!strstatus.Equals(""))
                {
                    GridView1.Rows[i].Visible = false;
                }
            }

            //判断各部门是否已经确认 如果状态为“部门确认”金额不允许修改

            if (GridView1.Rows[i].Cells[2].Text != "")
            {
                string strdeptcode = GridView1.Rows[i].Cells[2].Text;
                strdeptcode = strdeptcode.Replace("&nbsp;", "");
                if (strdeptcode.Equals(""))
                {
                    continue;
                }
                strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);

                string strsqlby3 = @"select by3 from  bill_ys_xmfjbm where procode='" + Request["nd"].ToString() + "' and deptcode='" + strdeptcode + "'";
                string strStuats = server.GetCellValue(strsqlby3);
                //if (strStuats == "2")
                //{
                //    txtMoney.Attributes.Add("readonly", "readonly");
                //}
            }
            if (GridView1.Rows[i].Cells[7].Text == "部门确认")
            {
                //txtMoney.Attributes.Add("readonly", "readonly");
                GridView1.Rows[i].Cells[7].Style.Add("color", "blue");
            }
            if (GridView1.Rows[i].Cells[7].Text == "部门异议")
            {
                GridView1.Rows[i].Cells[7].Style.Add("color", "red");
            }
            #region 合计行
            Table t = (Table)GridView1.Controls[0];
            GridViewRow item = (GridViewRow)t.Rows[t.Rows.Count - 1];
            Label control = item.FindControl("lbeTotalAmount") as Label;
            if (control != null)
            {
                control.Text = fTotalAmount.ToString("N02");
            }
            Label lbltotalShenBao = item.FindControl("lbltotalShenBao") as Label;
            if (lbltotalShenBao != null)
            {
                lbltotalShenBao.Text = fTotalShenbao.ToString("N02");
            }
            double fzje = 0;
            if (LaZje.Text != "" && LaZje.Text != null)
            {
                double.TryParse(LaZje.Text, out fzje);
            }
            fSymoney = fzje - fTotalAmount;
            this.Syje.Text = fSymoney.ToString("N02");
            this.hidwfmoney.Value = fSymoney.ToString("0.00");

            item.Cells[5].Text = dTotalChae.ToString("N02");//差额汇总数
            #endregion
        }

    }

    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {


        DataTable dt = new DataTable();
        string strnd = "";
        if (!string.IsNullOrEmpty(Request["nd"]))
        {
            strnd = Request["nd"].ToString();
        }
        string strkm = "";
        if (!string.IsNullOrEmpty(Request["yskm"]))
        {
            strkm = Request["yskm"].ToString();
        }

        bool usegkfj = new Bll.ConfigBLL().GetValueByKey("UseGKFJ").Equals("1") ? true : false;
        string strsql = "";
        string strstatus = this.ddlStatus.SelectedValue.Trim();//状态
        string strxmcode = this.ddlXm.SelectedValue;//利润项目
        if (usegkfj)
        {


            strsql = @"select * from (
	                select a.deptcode,yskmcode,(select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=a.yskmcode) as yskmname
	                ,(select  '['+deptcode+']'+ deptname from bill_departments where bill_departments.deptcode=a.deptcode) as deptname 
	                ,c.je as yskzje,c.deptcode as d,c.kmcode,c.procode, c.by1 as yssbje,
                    (isnull(c.by1-c.je,0) ) as ce,c.by2 as jysm,
                    ((case  by3 when '1' then '预算确认' when'2' then'部门确认' when'3' then '部门异议' else '未确认' end)) as zt
	                from bill_yskm_gkdept a,bill_ys_xmfjbm c where yskmcode='" + strkm + "' and a.deptcode<>'' and a.yskmCode=c.kmcode and a.deptCode=c.deptcode  and left(procode,4)='" + strnd + "'   ) b where b.deptname!=''";
            if (!strstatus.Equals(""))
            {
                //          and yskmcode in (select kmcode from bill_ys_xmfjbm where by3='1')	              and a.deptcode   in (select deptcode from bill_ys_xmfjbm where by3='1') and yskmcode in (select yskmcode from bill_ys_benefits_yskm 	where procode='201602')
                strsql += " and yskmcode in (select kmcode from bill_ys_xmfjbm where by3='" + strstatus + "') and deptcode in (select deptcode from bill_ys_xmfjbm where by3='" + strstatus + "')";
            }
            if (!strxmcode.Equals(""))
            {
                strsql += " and yskmcode in (select yskmcode from bill_ys_benefits_yskm where procode='" + strxmcode + "')";
            }

        }
        else
        {
            strsql = @"select * from (
                      select distinct  a.deptCode,yskmcode,(select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=a.yskmcode)  as yskmname
                    ,(select  '['+deptcode+']'+ deptname from bill_departments where bill_departments.deptcode=a.deptcode)  as deptname
                    ,c.je as yskzje,c.deptcode as d,c.kmcode,c.procode, c.by1 as yssbje,(isnull(c.by1-c.je,0) ) as ce,c.by2 as jysm,
                    ((case  by3 when '1' then '预算确认' when'2' then'部门确认' when'3' then '部门异议' else '未确认' end)) as zt
                      from dbo.bill_yskm_dept a, bill_ys_xmfjbm  c where  yskmcode  like  '" + strkm + "%'  and a.deptCode <> ''   and 1=1 and a.yskmCode=c.kmcode and a.deptCode=c.deptcode and left(procode,4)='" + strnd + "'   ) b where b.deptname!='' ";

        }

        dt = server.GetDataTable(strsql, null);


        if (dt.Rows.Count <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请分解后再进行导出！');", true);
            return;
        }
        //dt.Columns.Add(new DataColumn("你好1", typeof(string), decje.ToString()));
        Dictionary<string, string> dic = new Dictionary<String, String>();
        dic.Add("yskmname", "预算科目");
        dic.Add("deptname", "部门");
        dic.Add("yskzje", "预算控制金额");
        dic.Add("yssbje", "预算申报金额");
        dic.Add("ce", "申报差额");
        dic.Add("jysm", "建议说明");
        dic.Add("zt", "状态");

        new ExcelHelper().ExpExcel(dt, "ExportFile", dic);

        ////临时文件    
        //string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        ////使用OleDb连接  
        //OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
        //using (con)
        //{
        //    con.Open();
        //    //创建Sheet   
        //    OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([序号] VarChar,[年度] VarChar,[预算科目] VarChar,[部门] VarChar,[预算控制金额] VarChar,[申报金额] VarChar,[申报说明] text,[预算金额状态] VarChar)", con);
        //    cmdCreate.ExecuteNonQuery();
        //    //插入数据     
        //    for (int i = 0; i < this.GridView1.Rows.Count; i++)
        //    {
        //        using (OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@xuhao,@nd,@yskm,@dept,@je,@sbje,@sbsm,@status)", con))
        //        {
        //            string strxuhao = GridView1.Rows[i].Cells[0].Text.Trim();
        //            string dept = GridView1.Rows[i].Cells[2].Text.Trim();
        //            string km = GridView1.Rows[i].Cells[1].Text.Trim();
        //            string je = "0";

        //            TextBox txtje = GridView1.Rows[i].Cells[3].FindControl("TextBox1") as TextBox;
        //            if (txtje != null)
        //            {
        //                je = txtje.Text.Trim();
        //            }
        //            string strsbje = "0";
        //            Label txtsbje = GridView1.Rows[i].Cells[4].FindControl("lbleveShenBao") as Label;
        //            if (txtsbje != null)
        //            {
        //                strsbje = txtsbje.Text;
        //            }
        //            strsbje = strsbje.Replace("&nbsp;", "");
        //            strsbje = strsbje.Equals("") ? "0" : strsbje.ToString();

        //            string strsbsm = GridView1.Rows[i].Cells[6].Text.Trim();//申报说明
        //            string status = GridView1.Rows[i].Cells[7].Text.Trim();

        //            object objnd=Request["nd"];
        //            string strnd=string.Empty;
        //            if (objnd!=null)
        //            {
        //                strnd=objnd.ToString();
        //            }
        //            cmd.Parameters.AddWithValue("@xuhao", strxuhao);
        //            cmd.Parameters.AddWithValue("@nd", strnd);
        //            cmd.Parameters.AddWithValue("@yskm", km);
        //            cmd.Parameters.AddWithValue("@dept", dept);
        //            cmd.Parameters.AddWithValue("@je", je);
        //            cmd.Parameters.AddWithValue("@sbje", strsbje);
        //            cmd.Parameters.AddWithValue("@sbsm", strsbsm);
        //            cmd.Parameters.AddWithValue("@status", status);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}
        //Response.ContentType = "application/ms-excel";
        //Response.AppendHeader("Content-Disposition", "attachment;filename=ysbmfj.xls");
        //Response.BinaryWrite(File.ReadAllBytes(tempFile));
        //Response.End();
        //File.Delete(tempFile);
    }
    /// <summary>
    /// 保存
    /// </summary>
    private void save()
    {
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_Click(object sender, EventArgs e)
    {

        IList<bill_ys_xmfjbm> bm = new List<bill_ys_xmfjbm>();
        decimal MainMoney = 0;
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            string deptcode = (GridView1.Rows[i].FindControl("hidddept") as HiddenField).Value;
            string money = (GridView1.Rows[i].FindControl("TextBox1") as TextBox).Text.Trim();
            //string procoode = Request.QueryString["procode"].ToString();
            string kmcode = GridView1.Rows[i].Cells[1].Text.Trim();
            kmcode = kmcode.Substring(1, kmcode.IndexOf(']') - 1);
            bill_ys_xmfjbm bmtb = new bill_ys_xmfjbm();
            bmtb.kmcode = kmcode;
            bmtb.procode = Request.QueryString["nd"].ToString();
            bmtb.by1 = GridView1.Rows[i].Cells[4].Text;
            bmtb.by2 = GridView1.Rows[i].Cells[6].Text;
            bmtb.je = Convert.ToDecimal(money == "" ? "0" : money);

            if (GridView1.Rows[i].Cells[7].Text == "未确认")
            {
                bmtb.by3 = "0";
            }
            else if (GridView1.Rows[i].Cells[7].Text == "部门确认")
            {
                bmtb.by3 = "2";
            }
            else if (GridView1.Rows[i].Cells[7].Text == "部门异议")
            {
                bmtb.by3 = "3";
            }
            else
            {
                bmtb.by3 = "1";
            }
            bmtb.deptcode = deptcode;
            string strattachment = GridView1.Rows[i].Cells[10].Text.Trim();
            strattachment = strattachment.Replace("&nbsp;", "");
            bmtb.Attachment = strattachment;
            bm.Add(bmtb);
            MainMoney += Convert.ToDecimal(bmtb.je);
        }


        if (MainMoney > Convert.ToDecimal(LaZje.Text.Trim()))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门总金额已经超出预算！');", true);
            return;
        }
        if (dal.AddBmfj(bm))
        {
            //if (Request.QueryString["yskm"] != null && Request.QueryString["nd"] != null)
            //{
            Bind();
            //}
            //else
            //{
            //    bindAll(Request.QueryString["nd"]);
            //}
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
    }
    /// <summary>
    /// 确定并保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_qr_Click(object sender, EventArgs e)
    {
        //保存

        IList<bill_ys_xmfjbm> bm = new List<bill_ys_xmfjbm>();
        decimal MainMoney = 0;
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            string deptcode = (GridView1.Rows[i].FindControl("hidddept") as HiddenField).Value;
            string money = (GridView1.Rows[i].FindControl("TextBox1") as TextBox).Text.Trim();
            //string procoode = Request.QueryString["procode"].ToString();
            string kmcode = GridView1.Rows[i].Cells[1].Text.Trim();
            kmcode = kmcode.Substring(1, kmcode.IndexOf(']') - 1);
            bill_ys_xmfjbm bmtb = new bill_ys_xmfjbm();
            bmtb.kmcode = kmcode;
            bmtb.procode = Request.QueryString["nd"].ToString();
            bmtb.by1 = ((Label)GridView1.Rows[i].Cells[4].FindControl("lbleveShenBao")).Text.Trim();
            bmtb.by2 = GridView1.Rows[i].Cells[6].Text;
            bmtb.je = Convert.ToDecimal(money == "" ? "0" : money);
            if (GridView1.Rows[i].Cells[7].Text == "未确认")
            {
                bmtb.by3 = "0";
            }
            else if (GridView1.Rows[i].Cells[7].Text == "部门确认")
            {
                bmtb.by3 = "2";
            }
            else if (GridView1.Rows[i].Cells[7].Text == "预算确认")
            {
                bmtb.by3 = "3";

            }
            else//预算确认
            {
                bmtb.by3 = "1";
            }
            bmtb.deptcode = deptcode;
            string strattachment = GridView1.Rows[i].Cells[10].Text.Trim();
            strattachment = strattachment.Replace("&nbsp;", "");
            bmtb.Attachment = strattachment;
            bm.Add(bmtb);
            MainMoney += Convert.ToDecimal(bmtb.je);
        }


        if (MainMoney > Convert.ToDecimal(LaZje.Text.Trim()))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门总金额已经超出预算！');", true);
            return;
        }
        //保存成功后确认
        if (dal.AddBmfj(bm))
        {
            IList<bill_ys_xmfjbm> bmqr = new List<bill_ys_xmfjbm>();
            decimal MainMoneyqr = 0;
            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                string deptcode = (GridView1.Rows[j].FindControl("hidddept") as HiddenField).Value;
                string money = (GridView1.Rows[j].FindControl("TextBox1") as TextBox).Text.Trim();
                string nd = Request.QueryString["nd"].ToString();
                string kmcode = GridView1.Rows[j].Cells[1].Text.Trim();
                kmcode = kmcode.Substring(1, kmcode.IndexOf(']') - 1);
                bill_ys_xmfjbm bmtbqr = new bill_ys_xmfjbm();
                if (GridView1.Rows[j].Cells[7].Text != "部门确认")
                {
                    bmtbqr.kmcode = kmcode;
                    bmtbqr.procode = nd;
                    bmtbqr.deptcode = deptcode;
                    bmqr.Add(bmtbqr);
                }
                bmtbqr.je = Convert.ToDecimal(money == "" ? "0" : money);
                MainMoneyqr += Convert.ToDecimal(bmtbqr.je);
            }
            if (MainMoneyqr > Convert.ToDecimal(LaZje.Text.Trim()))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门总金额已经超出预算！');", true);
                return;
            }
            if (dal.AddBmfjQr(bmqr))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作成功！');", true);
                //if (Request.QueryString["yskm"] != null && Request.QueryString["nd"] != null)
                //{
                Bind();
                //}
                //else
                //{
                //    bindAll(Request.QueryString["nd"]);
                //}
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
    }

    /// <summary>
    /// 确认申报金额
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_qrsb_Click(object sender, EventArgs e)
    {
        //保存

        IList<bill_ys_xmfjbm> bm = new List<bill_ys_xmfjbm>();
        decimal MainMoney = 0;
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            string deptcode = (GridView1.Rows[i].FindControl("hidddept") as HiddenField).Value;
            string money = (GridView1.Rows[i].FindControl("TextBox1") as TextBox).Text.Trim();
            //string procoode = Request.QueryString["procode"].ToString();
            string kmcode = GridView1.Rows[i].Cells[1].Text.Trim();
            kmcode = kmcode.Substring(1, kmcode.IndexOf(']') - 1);
            bill_ys_xmfjbm bmtb = new bill_ys_xmfjbm();
            bmtb.kmcode = kmcode;
            bmtb.procode = Request.QueryString["nd"].ToString();
            bmtb.by1 = ((Label)GridView1.Rows[i].Cells[4].FindControl("lbleveShenBao")).Text.Trim();
            bmtb.by2 = GridView1.Rows[i].Cells[6].Text;

            decimal dbje = 0;
            if (decimal.TryParse(bmtb.by1, out dbje))
            {
                bmtb.je = dbje;
            }
            else
            {
                bmtb.je = Convert.ToDecimal(money == "" ? "0" : money);
            }

            if (GridView1.Rows[i].Cells[7].Text == "未确认")
            {
                bmtb.by3 = "0";
            }
            else if (GridView1.Rows[i].Cells[7].Text == "部门确认")
            {
                bmtb.by3 = "2";
            }
            else if (GridView1.Rows[i].Cells[7].Text == "预算确认")
            {
                bmtb.by3 = "3";

            }
            else//预算确认
            {
                bmtb.by3 = "1";
            }
            bmtb.deptcode = deptcode;
            string strattachment = GridView1.Rows[i].Cells[10].Text.Trim();
            strattachment = strattachment.Replace("&nbsp;", "");
            bmtb.Attachment = strattachment;
            bm.Add(bmtb);
            MainMoney += Convert.ToDecimal(bmtb.je);
        }


        if (MainMoney > Convert.ToDecimal(LaZje.Text.Trim()))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门总金额已经超出预算！');", true);
            return;
        }
        //保存成功后确认
        if (dal.AddBmfj(bm))
        {
            IList<bill_ys_xmfjbm> bmqr = new List<bill_ys_xmfjbm>();
            decimal MainMoneyqr = 0;
            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                string deptcode = (GridView1.Rows[j].FindControl("hidddept") as HiddenField).Value;
                string money = (GridView1.Rows[j].FindControl("TextBox1") as TextBox).Text.Trim();
                string nd = Request.QueryString["nd"].ToString();
                string kmcode = GridView1.Rows[j].Cells[1].Text.Trim();
                kmcode = kmcode.Substring(1, kmcode.IndexOf(']') - 1);
                bill_ys_xmfjbm bmtbqr = new bill_ys_xmfjbm();
                if (GridView1.Rows[j].Cells[7].Text != "部门确认")
                {
                    bmtbqr.kmcode = kmcode;
                    bmtbqr.procode = nd;
                    bmtbqr.deptcode = deptcode;
                    bmqr.Add(bmtbqr);
                }
                bmtbqr.je = Convert.ToDecimal(money == "" ? "0" : money);
                MainMoneyqr += Convert.ToDecimal(bmtbqr.je);
            }
            if (MainMoneyqr > Convert.ToDecimal(LaZje.Text.Trim()))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门总金额已经超出预算！');", true);
                return;
            }
            if (dal.AddBmfjQr(bmqr))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作成功！');", true);
                //if (Request.QueryString["yskm"] != null && Request.QueryString["nd"] != null)
                //{
                Bind();
                //}
                //else
                //{
                //    bindAll(Request.QueryString["nd"]);
                //}
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
    }
    protected void GridViewRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e == null)
        {
            return;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strnd = Request.QueryString["nd"];
            string stryskm = e.Row.Cells[1].Text.Trim();
            stryskm = stryskm.Substring(1, stryskm.IndexOf("]") - 1);
            string strdept = e.Row.Cells[2].Text.Trim();
            strdept = strdept.Substring(1, strdept.IndexOf("]") - 1);
            object objrel = server.GetCellValue("select attachment from bill_ys_xmfjbm where procode='" + strnd + "' and deptcode='" + strdept + "' and kmcode='" + stryskm + "'");
            string strattachment = objrel == null ? "" : objrel.ToString();
            if (!strattachment.Equals(""))
            {
                e.Row.Cells[9].Text = "<a href='../../Uploads/bmfjjeqr/" + System.IO.Path.GetFileName(strattachment) + @"' target='_blank'  >下载</a>";
                e.Row.Cells[10].Text = strattachment;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计：";
            e.Row.Cells[0].Style.Add("text-align", "right");
            e.Row.Cells[3].Style.Add("text-align", "right");
        }
    }



    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["yskm"] != null && Request.QueryString["nd"] != null)
        {
            Bind();
        }
        else
        {
            bindAll(Request.QueryString["nd"]);
        }
    }
    protected void ddlXm_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["yskm"] != null && Request.QueryString["nd"] != null)
        {
            Bind();
        }
        else
        {
            bindAll(Request.QueryString["nd"]);
        }
    }

}
