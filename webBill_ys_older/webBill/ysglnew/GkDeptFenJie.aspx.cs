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
using System.Data.SqlClient;
using System.Collections.Generic;
using Models;
using System.Data.OleDb;
using System.IO;
using Dal.UserProperty;

public partial class webBill_ysglnew_GkDeptFenJie : System.Web.UI.Page
{
    
    sqlHelper.sqlHelper sqlHelper = new sqlHelper.sqlHelper();
    string strUserCode="";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            strUserCode = Session["userCode"].ToString().Trim();
           
            if (!IsPostBack)
            {
                string strbinddeptsql = "select ('['+deptcode+']'+deptname)as showname,deptcode from bill_Departments where deptcode in (select distinct deptcode from bill_yskm_gkdept) ";
                if ( strUserCode!= "admin")
                {
                    strbinddeptsql += " and (deptCode in (select objectID from bill_userRight where rightType='2'and userCode='" + Session["userCode"].ToString().Trim() + "') or sjdeptcode in (select objectID from bill_userRight where rightType='2'and userCode='" + strUserCode + "'))";
                }
                DataTable dtDept = sqlHelper.GetDataTable(strbinddeptsql, null);
                LaDept.DataSource=dtDept;
                LaDept.DataTextField = "showname";
                LaDept.DataValueField = "deptcode";
                LaDept.DataBind();
                this.LaDept.Items.Insert(0, new ListItem("--全部--", ""));
                bindData();
            }
        }
    }

    private void bindData()
    {
        if (isTopDept("y", Session["userCode"].ToString().Trim()))
        {
            string dept = sqlHelper.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            LaDept.SelectedValue = dept;
        }
        else
        {
            //所在部门
            string Dept = sqlHelper.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            //上级部门
            string sjDept = sqlHelper.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
            LaDept.SelectedValue= Dept;
        }
        //绑定年份
        string strbindndsql = "select distinct  nian from  dbo.bill_ysgc order by nian desc";
        DataTable dtNd = sqlHelper.GetDataTable(strbindndsql, null);
        if (dtNd != null)
        {
            this.ddlNd.DataSource = dtNd;
            this.ddlNd.DataTextField = "nian";
            this.ddlNd.DataValueField = "nian";
            this.ddlNd.DataBind();
        }
       // bindkm();
        //showTotalJe();
        //绑定gridview
        bindGridview();
    }
    double dbTotalJe = 0;
    double dbTotalBili = 0;
    double db1 = 0;
    double db2 = 0;
    double db3 = 0;
    double db4 = 0;
    double db5 = 0;
    double db6 = 0;
    double db7 = 0;
    double db8 = 0;
    double db9 = 0;
    double db10 = 0;
    double db11 = 0;
    double db12 = 0;

    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.EmptyDataRow)
        {
            //总金额
            double dbeveje = 0;
            string streveje = e.Row.Cells[3].Text.Trim();
            if (double.TryParse(streveje, out dbeveje))
            {
                dbTotalJe += dbeveje;
            }
            //比例
            double dbevebili = 0;
            string strevebili = e.Row.Cells[2].Text.Trim();
            if (double.TryParse(strevebili, out dbevebili))
            {
                dbTotalBili += dbevebili;
            }

            double dbeve1 = 0;
            string streve1 = "";
            TextBox txt1;

            txt1 = e.Row.Cells[4].FindControl("one") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db1 += dbeve1;
                }
            }
            txt1 = e.Row.Cells[5].FindControl("two") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db2 += dbeve1;
                }
            }
            txt1 = e.Row.Cells[6].FindControl("three") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db3 += dbeve1;
                }
            }
            txt1 = e.Row.Cells[7].FindControl("four") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db4 += dbeve1;
                }
            }
            txt1 = e.Row.Cells[8].FindControl("five") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db5 += dbeve1;
                }
            }
            txt1 = e.Row.Cells[9].FindControl("six") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db6 += dbeve1;
                }
            }
            txt1 = e.Row.Cells[10].FindControl("seven") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db7 += dbeve1;
                }
            }
            txt1 = e.Row.Cells[11].FindControl("eight") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db8 += dbeve1;
                }
            }
            txt1 = e.Row.Cells[12].FindControl("nine") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db9 += dbeve1;
                }
            }
            txt1 = e.Row.Cells[13].FindControl("teen") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db10 += dbeve1;
                }
            }
            txt1 = e.Row.Cells[14].FindControl("eleven") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db11 += dbeve1;
                }
            }
            txt1 = e.Row.Cells[15].FindControl("twelve") as TextBox;
            if (txt1 != null)
            {
                streve1 = txt1.Text.Trim();
                if (double.TryParse(streve1, out dbeve1))
                {
                    db12 += dbeve1;
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计：";
            e.Row.Cells[2].Text = dbTotalBili.ToString();
            e.Row.Cells[3].Text = dbTotalJe.ToString();
        }
    }
    /// <summary>
    /// 切换科目
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlKm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
       // showTotalJe();
        bindGridview();
    }
    /// <summary>
    /// 切换年份
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void drpSelectNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindkm();
        //showTotalJe();
        bindGridview();
    }
    /// <summary>
    /// 绑定gridview
    /// </summary>
    private void bindGridview()
    {
        //年度
        string strnd = this.ddlNd.SelectedValue;
        if (string.IsNullOrEmpty(strnd))
        {
            return;
        }
        //预算科目编号
        //string stryskmcode = this.ddlKm.SelectedValue;
        //if (string.IsNullOrEmpty(stryskmcode))
        //{
        //    return;
        //}
        //归口部门
        string strgkdept =LaDept.SelectedValue;
        ////金额
        //string strtotalamount = this.lblgkdeptamount.Text.Trim();
        //if (string.IsNullOrEmpty(strtotalamount))
        //{
        //    return;
        //}
        //string strsql = "select *,(select deptname from bill_departments where deptcode=a.fjdeptcode) as deptname,round((@kmamount*fjbl),2) as nysje from bill_gkfjbili a where gkdeptcode=@gkdeptcode and nian=@nian and yskmcode=@yskmcode and fjbl!=0 order by fjdeptcode ";
        //SqlParameter[] sqlsp = new SqlParameter[] { new SqlParameter("@kmamount", strtotalamount), new SqlParameter("@gkdeptcode", strgkdept), new SqlParameter("@nian", strnd), new SqlParameter("@yskmcode", stryskmcode) };
        //DataTable dtRel = sqlHelper.GetDataTable(strsql, sqlsp);
        //GridView1.DataSource = dtRel;
        //GridView1.DataBind();
        string strsql = @"select *,(select deptname from bill_departments where deptcode=a.fjdeptcode) as deptname
                    ,(select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=a.yskmcode) as yskmmc
                ,round((xmfjbm.je*fjbl),2) as nysje from bill_gkfjbili a 
                left join 
                bill_ys_xmfjbm xmfjbm
                on a.gkdeptcode=xmfjbm.deptcode and a.nian=xmfjbm.procode and a.yskmcode=xmfjbm.kmcode
                where  nian=@procode and gkdeptcode=@deptcode   and fjbl!=0 order by fjdeptcode";
        SqlParameter[] arrsp = new SqlParameter[] { new SqlParameter("@procode", strnd), new SqlParameter("@deptcode", strgkdept)};
        DataTable dtRel = sqlHelper.GetDataTable(strsql, arrsp);
        GridView1.DataSource = dtRel;
        GridView1.DataBind();

    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_click(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        //年度
        string strnd = this.ddlNd.SelectedValue.Trim();
        if (strnd.Equals(""))
        {
            showMessage("请先选择年份。"); return;
        }
        //科目
        //string strkm = this.ddlKm.SelectedValue.Trim();
        //if (strkm.Equals(""))
        //{
        //    showMessage("请先选择科目。"); return;
        //}
        //部门
        string strgkdept =LaDept.SelectedValue;
       

        Bll.newysgl.YsglMainBll bll = new Bll.newysgl.YsglMainBll();
        IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(strnd);
        YsgcTb gcbh = new Bll.newysgl.YsglMainBll().GetgcbhByNd(strnd);
        bool isOk = true;
        if (sysConfig["MonthOrQuarter"].Equals("2"))
        {
            int iRowcount = this.GridView1.Rows.Count;
            for (int i = 0; i < iRowcount; i++)
            {
                string year = GridView1.Rows[i].Cells[3].Text.Trim();
                decimal deYear = 0;
                if (!decimal.TryParse(year,out deYear)||deYear==0)
                {
                    continue;
                }
                IList<YsgcTb> lstysgctb = new List<YsgcTb>();
                string January = (GridView1.Rows[i].FindControl("one") as TextBox).Text.Trim();
                string February = (GridView1.Rows[i].FindControl("two") as TextBox).Text.Trim();
                string march = (GridView1.Rows[i].FindControl("three") as TextBox).Text.Trim();
                string April = (GridView1.Rows[i].FindControl("four") as TextBox).Text.Trim();
                string May = (GridView1.Rows[i].FindControl("five") as TextBox).Text.Trim();
                string June = (GridView1.Rows[i].FindControl("six") as TextBox).Text.Trim();
                string July = (GridView1.Rows[i].FindControl("seven") as TextBox).Text.Trim();
                string August = (GridView1.Rows[i].FindControl("eight") as TextBox).Text.Trim();
                string September = (GridView1.Rows[i].FindControl("nine") as TextBox).Text.Trim();
                string October = (GridView1.Rows[i].FindControl("teen") as TextBox).Text.Trim();
                string November = (GridView1.Rows[i].FindControl("eleven") as TextBox).Text.Trim();
                string December = (GridView1.Rows[i].FindControl("twelve") as TextBox).Text.Trim();

                YsgcTb ys = new YsgcTb();
                string strkm = GridView1.Rows[i].Cells[1].Text.Trim();
                strkm = strkm.Replace("&nbsp;", "");
                strkm = strkm.Substring(1, strkm.IndexOf("]") - 1);
                if (strkm.Equals(""))
                {
                    isOk = false;
                    showMessage("对不起，第" + (i + 1) + "行科目信息有误，无法继续进行！"); break;
                }
                ys.kmbh = strkm;
                ys.January = January;
                ys.February = February;
                ys.march = march;
                ys.April = April;
                ys.May = May;
                ys.June = June;
                ys.July = July;
                ys.August = August;
                ys.September = September;
                ys.October = October;
                ys.November = November;
                ys.December = December;
                ys.year = year;
                decimal count = 0;
                count += (January == "" ? 0 : Convert.ToDecimal(January));
                count += (February == "" ? 0 : Convert.ToDecimal(February));
                count += (march == "" ? 0 : Convert.ToDecimal(march));
                count += (April == "" ? 0 : Convert.ToDecimal(April));
                count += (May == "" ? 0 : Convert.ToDecimal(May));
                count += (June == "" ? 0 : Convert.ToDecimal(June));
                count += (July == "" ? 0 : Convert.ToDecimal(July));
                count += (August == "" ? 0 : Convert.ToDecimal(August));
                count += (September == "" ? 0 : Convert.ToDecimal(September));
                count += (October == "" ? 0 : Convert.ToDecimal(October));
                count += (November == "" ? 0 : Convert.ToDecimal(November));
                count += (December == "" ? 0 : Convert.ToDecimal(December));
                if (count == 0)//表示一年内 一个都没有分配过
                {
                    continue;
                }
                if (Math.Round(count, 2) > (year == "" ? 0 : Convert.ToDecimal(year)))
                {
                    showMessage("第" + (i + 1) + "行填写的月度预算合计大于年度预算,系统无法保存,请重新确认。");
                    return;
                }
                else
                {
                    lstysgctb.Add(ys);
                }
                string strfjdept = this.LaDept.SelectedValue;
                string flowid = "ys";//因为只有市立医院自己用，所以暂时写死
                if (lstysgctb.Count > 0&&isOk)
                {
                    bll.Addtb(lstysgctb, strfjdept, "01", strnd, strUserCode, "end", flowid,"");
                }
                lstysgctb.Clear();
                //string strBillCode = System.Guid.NewGuid().ToString().ToUpper();
                ////bill_main主表
                //Bill_Main main = new Bill_Main();
                //main.BillCode = System.Guid.NewGuid().ToString().ToUpper();
                //main.BillName = strnd + "0001";
                //main.FlowId = "ys";
                //main.StepId = "-1";
                //main.BillUser = strUserCode;
                //main.BillDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                //main.BillDept = deptcode;
                //main.BillJe = 0;
                //main.LoopTimes = 1;
                //main.BillType = "1";
            }
            if (isOk)
            {
                showMessage("保存成功！");
            } 
           
        }

    }

    /// <summary>
    /// 导出excel模板
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_export_Click(object sender, EventArgs e)
    {
        //临时文件    
        string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        //使用OleDb连接  
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
        using (con)
        {
            con.Open();
            //创建Sheet   
            OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([年度] VarChar,[费用编号] VarChar,[费用名称] VarChar, [部门编号] VarChar,[部门名称] VarChar,[分配比例] VarChar,[年预算总额] VarChar,[1月份预算] VarChar,[2月份预算] VarChar,[3月份预算] VarChar,[4月份预算] VarChar,[5月份预算] VarChar,[6月份预算] VarChar,[7月份预算] VarChar,[8月份预算] VarChar,[9月份预算] VarChar,[10月份预算] VarChar,[11月份预算] VarChar,[12月份预算] VarChar)", con);
            cmdCreate.ExecuteNonQuery();
            //插入数据     
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                string strnd = this.ddlNd.SelectedValue;
                string strkmcode = this.GridView1.Rows[i].Cells[1].Text.Trim();//this.ddlKm.SelectedValue;
                strkmcode = strkmcode.Substring(1, strkmcode.Length - 1);
                string strkmmc = strkmcode;

                string strdeptcode = this.GridView1.Rows[i].Cells[0].Text.Trim();
                string strdeptname = this.GridView1.Rows[i].Cells[1].Text.Trim();
                string strbili = this.GridView1.Rows[i].Cells[2].Text.Trim();
                string strTotalAmount = this.GridView1.Rows[i].Cells[3].Text.Trim();

                using (OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@nd,@kmcode,@kmmc,@deptcode, @deptname,@fpbili,@nysje,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12)", con))
                {
                    cmd.Parameters.AddWithValue("@nd", strnd);
                    cmd.Parameters.AddWithValue("@kmcode", strkmcode);
                    cmd.Parameters.AddWithValue("@kmmc", strkmmc);
                    cmd.Parameters.AddWithValue("@deptcode", strdeptcode);
                    cmd.Parameters.AddWithValue("@deptname", strdeptname);
                    cmd.Parameters.AddWithValue("@fpbili", strbili);
                    cmd.Parameters.AddWithValue("@nysje", strTotalAmount);
                    cmd.Parameters.AddWithValue("@1", "");
                    cmd.Parameters.AddWithValue("@2", "");
                    cmd.Parameters.AddWithValue("@3", "");
                    cmd.Parameters.AddWithValue("@4", "");
                    cmd.Parameters.AddWithValue("@5", "");
                    cmd.Parameters.AddWithValue("@6", "");
                    cmd.Parameters.AddWithValue("@7", "");
                    cmd.Parameters.AddWithValue("@8", "");
                    cmd.Parameters.AddWithValue("@9", "");
                    cmd.Parameters.AddWithValue("@10", "");
                    cmd.Parameters.AddWithValue("@11", "");
                    cmd.Parameters.AddWithValue("@12", "");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        Response.ContentType = "application/ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=gkfj.xls");
        Response.BinaryWrite(File.ReadAllBytes(tempFile));
        Response.End();
        File.Delete(tempFile);
    }

    /// <summary>
    /// 导入excel模板
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_reload_Click(object sender, EventArgs e)
    {
        object objdt = Session["nowfjjedt"];
        if (objdt == null)
        {
            return;
        }
        string strnd = ddlNd.SelectedValue;
        string strkm = "";// this.ddlKm.SelectedValue;
        if (string.IsNullOrEmpty(strnd) || string.IsNullOrEmpty(strkm))
        {
            showMessage("请先选择部门或科目。"); return;
        }
        //strkm = strkm.Substring(1, strkm.IndexOf("]") - 1);
        DataTable dt = (DataTable)objdt;
        if (dt == null || dt.Rows.Count <= 0)
        {
            return;
        }
        else if (!dt.Rows[0]["nd"].Equals(strnd))
        {
            showMessage("请先选择与导入excel对应的报销年度。");
        }
        else if (!dt.Rows[0]["kmcode"].Equals(strkm))
        {
            showMessage("请先选择与导入excel对应的预算科目。");
        }
        else { }
        int irows = this.GridView1.Rows.Count;
        for (int i = 0; i < irows; i++)
        {
            string strdeptcode = this.GridView1.Rows[i].Cells[0].Text.Trim();
            int idtrows = dt.Rows.Count;
            TextBox txt1 = this.GridView1.Rows[i].Cells[4].FindControl("one") as TextBox;
            TextBox txt2 = this.GridView1.Rows[i].Cells[4].FindControl("two") as TextBox;
            TextBox txt3 = this.GridView1.Rows[i].Cells[4].FindControl("three") as TextBox;
            TextBox txt4 = this.GridView1.Rows[i].Cells[4].FindControl("four") as TextBox;
            TextBox txt5 = this.GridView1.Rows[i].Cells[4].FindControl("five") as TextBox;
            TextBox txt6 = this.GridView1.Rows[i].Cells[4].FindControl("six") as TextBox;
            TextBox txt7 = this.GridView1.Rows[i].Cells[4].FindControl("seven") as TextBox;
            TextBox txt8 = this.GridView1.Rows[i].Cells[4].FindControl("eight") as TextBox;
            TextBox txt9 = this.GridView1.Rows[i].Cells[4].FindControl("nine") as TextBox;
            TextBox txt10 = this.GridView1.Rows[i].Cells[4].FindControl("teen") as TextBox;
            TextBox txt11 = this.GridView1.Rows[i].Cells[4].FindControl("eleven") as TextBox;
            TextBox txt12 = this.GridView1.Rows[i].Cells[4].FindControl("twelve") as TextBox;

            DataView dv = dt.DefaultView;
            dv.RowFilter = "deptcode = '" + strdeptcode + "'";
            DataTable newdt = dv.ToTable();
            txt1.Text = newdt.Rows[0]["1"].ToString();
            txt2.Text = newdt.Rows[0]["2"].ToString();
            txt3.Text = newdt.Rows[0]["3"].ToString();
            txt4.Text = newdt.Rows[0]["4"].ToString();
            txt5.Text = newdt.Rows[0]["5"].ToString();
            txt6.Text = newdt.Rows[0]["6"].ToString();
            txt7.Text = newdt.Rows[0]["7"].ToString();
            txt8.Text = newdt.Rows[0]["8"].ToString();
            txt9.Text = newdt.Rows[0]["9"].ToString();
            txt10.Text = newdt.Rows[0]["10"].ToString();
            txt11.Text = newdt.Rows[0]["11"].ToString();
            txt12.Text = newdt.Rows[0]["12"].ToString();
        }
    }
    /// <summary>
    /// 绑定科目
    /// </summary>
    //private void bindkm()
    //{
    //    if (this.ddlNd.Items.Count > 0 && this.ddlNd.SelectedValue != null)
    //    {
    //        string strnd = this.ddlNd.SelectedValue.Trim();
    //        string strgkdept = LaDept.SelectedValue;
    //        string strbindkmsql = "select yskmcode,('['+yskmcode+']'+yskmmc) as yskmmc from bill_yskm where kmStatus='1' and yskmcode in (select distinct yskmcode from bill_yskm_gkdept gkdept inner join bill_ys_xmfjbm xmfjbm on gkdept.yskmcode=xmfjbm.kmcode where xmfjbm.procode='" + strnd + "' and xmfjbm.deptcode='" + strgkdept + "' and  gkdept.deptcode='" + strgkdept + "')";
    //        DataTable dtKm = sqlHelper.GetDataTable(strbindkmsql, null);
    //        if (dtKm != null)
    //        {
    //            this.ddlKm.DataSource = dtKm;
    //            this.ddlKm.DataTextField = "yskmmc";
    //            this.ddlKm.DataValueField = "yskmcode";
    //            this.ddlKm.DataBind();
    //        }
    //    }
    //}

    ///// <summary>
    ///// 显示科目预算总额
    ///// </summary>
    //private void showTotalJe()
    //{
    //    string strnd = this.ddlNd.SelectedValue;
    //    if (string.IsNullOrEmpty(strnd))
    //    {
    //        return;
    //    }
    //    string stryskmcode = this.ddlKm.SelectedValue;
    //    if (string.IsNullOrEmpty(stryskmcode))
    //    {
    //        return;
    //    }
    //    string strgkdept =LaDept.SelectedValue;
      
    //    string strsql = "select je from bill_ys_xmfjbm where procode=@procode and deptcode=@deptcode and kmcode=@kmcode and by3='2'";
    //    SqlParameter[] arrsp = new SqlParameter[] { new SqlParameter("@procode", strnd), new SqlParameter("@deptcode ", strgkdept), new SqlParameter("@kmcode", stryskmcode) };
    //    this.lblgkdeptamount.Text = sqlHelper.GetCellValue(strsql, arrsp);
    //}
    
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
        if (sqlHelper.GetCellValue(sql) == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    private void showMessage(string strMsg)
    {
        string strScript = "alert('" + strMsg + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
    }

    protected void LaDept_SelectedIndexChanged(object sender, EventArgs e)
    {
       // bindkm();
        bindGridview();
    }
}
