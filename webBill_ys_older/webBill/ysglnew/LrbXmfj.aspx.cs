using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using System.Data;
using Bll.UserProperty;
using System.IO;
using System.Data.OleDb;
using Dal;

public partial class webBill_ysglnew_LrbXmfj : System.Web.UI.Page
{
    Dal.newysgl.Xmlr dal = new Dal.newysgl.Xmlr();

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    SysManager smgr = new SysManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["kmCode"] != null && Request.QueryString["kmCode"].ToString() != "")
            {
                hf_km.Value = Request.QueryString["kmCode"];
            }

            Bind();


        }
    }

    private void Bind()
    {
        string selectndsql = @" select nian,xmmc from bill_ysgc where   yue='' and nian in (select distinct nd  from bill_SysConfig where configname = 'ystbfs' and configvalue='1' ) order by nian desc";
        DataTable selectdt = server.GetDataTable(selectndsql, null);
        drpNd.DataSource = selectdt;
        drpNd.DataTextField = "xmmc";
        drpNd.DataValueField = "nian";
        drpNd.DataBind();
        bindType();
        //List<string> ndlist = dal.GetNdByxmLrb("1");
        //if (ndlist.Count > 0)
        //{
        //    foreach (var i in ndlist)
        //    {
        //        drpNd.Items.Add(new ListItem(i, i));
        //    }
        //    //this.drpNd.SelectedValue = (DateTime.Now.Year + 1).ToString();
        //    //绑定项目类别
        //    bindType();
        //    //string strtxtlrxm = this.txtcx.Text.Trim();
        //    //DataTable dt = dal.GetxmbBynd(this.drpNd.SelectedValue, strtxtlrxm, hf_km.Value.Trim());
        //    //myGrid.DataSource = dt;
        //    //myGrid.DataBind();
        //    // RowsBound();
        //}
    }

    private void bindType()
    {
        ConfigDal condal = new ConfigDal();

        this.txtcx.Items.Clear();
        //单位

        if (!string.IsNullOrEmpty(this.drpNd.SelectedValue))
        {
            string strfjjesql = "  select sum(budgetmoney) as fjje from bill_ys_xmfjlrb where annual='" + drpNd.SelectedValue + "'";
            string strfjje = server.GetCellValue(strfjjesql);//已分解金额
            decimal decfjje = 0;
            txt_mas.Text = "";
            //判断是否是全面预算

            string Qmys = condal.GetValueByKey("Qmys");
            if (!string.IsNullOrEmpty(Qmys))
            {
                //1. 根据年度查询收入总目标
                string strsqlzed = "select zje from bill_Srmb where nd='" + drpNd.SelectedValue + "'";
                string strzje = server.GetCellValue(strsqlzed);//总金额
              
                if (!string.IsNullOrEmpty(strzje))
                {
                    decimal deczje = decimal.Parse(strzje);
                    
                    txt_mas.Text += drpNd.SelectedValue + "年总收入目标额度：" + deczje.ToString("N2");
                    if (!string.IsNullOrEmpty(strfjje))
                    {
                        decfjje = decimal.Parse(strfjje);

                    }
                    decimal decsyje = deczje - decfjje;
                    if (!string.IsNullOrEmpty(decsyje.ToString()))
                    {
                        txt_mas.Text += ";已分解金额：" + decfjje.ToString("N2") + " 剩余金额：" + decsyje.ToString("N2");
                    }
                }
            }
           

            //2. 


            IList<bill_ys_benefitpro> listDept = smgr.GetAllfy(this.drpNd.SelectedValue, "明细汇总");
            int iDeptCount = listDept.Count;
            if (iDeptCount > 0)
            {
                for (int i = 0; i < iDeptCount; i++)
                {
                    this.txtcx.Items.Add(new ListItem(listDept[i].proname, listDept[i].proname));
                }
            }
        }
        this.txtcx.Items.Insert(0, new ListItem("--全部--", ""));
        string strtxtlrxm = this.txtcx.Text.Trim();
        DataTable dt = dal.GetxmbBynd(drpNd.SelectedValue, strtxtlrxm, hf_km.Value.Trim());
        myGrid.DataSource = dt;
        myGrid.DataBind();
        RowsBound();
    }
    protected void drpNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindType();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        IList<bill_ys_xmfjlrb> yxxmfjb = new List<bill_ys_xmfjlrb>();
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            string istx = (myGrid.Items[i].FindControl("hiddistx") as HiddenField).Value;
            //if (istx == "1")
            //{
                string kmcode = (myGrid.Items[i].FindControl("hiddkmcode") as HiddenField).Value;
                string xmcode = (myGrid.Items[i].FindControl("hiddprocode") as HiddenField).Value;
                string je = (myGrid.Items[i].FindControl("txt_je") as TextBox).Text;
                bill_ys_xmfjlrb xmb = new bill_ys_xmfjlrb();
                xmb.annual = this.drpNd.SelectedValue;
                xmb.budgetmoney = Convert.ToDecimal(je == "" ? "0" : je);
                xmb.kmcode = kmcode;
                xmb.procode = xmcode;
                yxxmfjb.Add(xmb);
            //}
        }
        if (dal.InsertTb(yxxmfjb))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
            string strtxtlrxm = this.txtcx.Text.Trim();
            DataTable dt = dal.GetxmbBynd(this.drpNd.SelectedValue, strtxtlrxm, hf_km.Value.ToString().Trim());
            myGrid.DataSource = dt;
            myGrid.DataBind();
            RowsBound();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！请联系管理员！');", true);
        }
    }


    protected void btcx_Click(object sender, EventArgs e)
    {
        string strtxtlrxm = this.txtcx.Text.Trim();
        DataTable dt = dal.GetxmbBynd(this.drpNd.SelectedValue, strtxtlrxm, hf_km.Value.ToString().Trim());
        myGrid.DataSource = dt;
        myGrid.DataBind();
        RowsBound();

    }


    private void RowsBound()
    {
        IList<bill_ys_xmfjlrb> xmfj = dal.GetXmfj(drpNd.SelectedValue);
        int iItemCount = myGrid.Items.Count;
        double fTotalAmount = 0;
        for (int i = 0; i < iItemCount; i++)
        {
            string kmcode = (myGrid.Items[i].FindControl("hiddkmcode") as HiddenField).Value;
            string xmcode = (myGrid.Items[i].FindControl("hiddprocode") as HiddenField).Value;
            TextBox je = myGrid.Items[i].FindControl("txt_je") as TextBox;
            var temp = from p in xmfj
                       where p.kmcode == kmcode && p.procode == xmcode
                       select p;
            if (temp.Count() > 0)
            {
                je.Text = Convert.ToDecimal(temp.First().budgetmoney).ToString("N02");
            }
            #region 合计行
            double flAmount = 0;
            if (!string.IsNullOrEmpty(je.Text) && double.TryParse(je.Text, out flAmount))
            {
                fTotalAmount += flAmount;
            }
            #endregion
        }
        Table t = (Table)myGrid.Controls[0];
        DataGridItem item = (DataGridItem)t.Rows[t.Rows.Count - 1];
        Label control = item.FindControl("lbeTotalAmount") as Label;
        if (control != null)
        {
            fTotalAmount = Math.Round(fTotalAmount, 2);
            control.Text = fTotalAmount.ToString("N02");
        }
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        IList<bill_ys_xmfjlrb> yxxmfjb = new List<bill_ys_xmfjlrb>();
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox istx = (myGrid.Items[i].FindControl("CheckBox1") as CheckBox);
            if (istx.Checked == true)
            {
                string kmcode = (myGrid.Items[i].FindControl("hiddkmcode") as HiddenField).Value;
                string xmcode = (myGrid.Items[i].FindControl("hiddprocode") as HiddenField).Value;
                string je = (myGrid.Items[i].FindControl("txt_je") as TextBox).Text;
                bill_ys_xmfjlrb xmb = new bill_ys_xmfjlrb();
                xmb.annual = this.drpNd.SelectedValue;
                xmb.budgetmoney = Convert.ToDecimal(je);
                xmb.kmcode = kmcode;
                xmb.procode = xmcode;
                yxxmfjb.Add(xmb);
            }
        }
        if (dal.SubMitXmFlase(yxxmfjb))
        {
            string strtxtlrxm = this.txtcx.SelectedValue.ToString().Trim();
            DataTable dt = dal.GetxmbBynd(this.drpNd.SelectedValue, strtxtlrxm, hf_km.Value.ToString().Trim());
            myGrid.DataSource = dt;
            myGrid.DataBind();
            RowsBound();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('提交失败！');", true);
        }
    }

    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            e.Item.Cells[0].Text = (e.Item.ItemIndex + 1).ToString();
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            e.Item.Cells[0].Text = "合计：";
            e.Item.Cells[0].Style.Add("text-align", "right");
            e.Item.Cells[4].Style.Add("text-align", "right");
        }
    }

    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        //string sql = getSelectSql();
        //string strtxtlrxm = this.txtcx.Text.Trim();
        //DataTable dtExport = new DataTable();
        //dtExport = dal.GetxmbBynd(drpNd.SelectedValue, strtxtlrxm, hf_km.Value.Trim());
        //DataTableToExcel(dtExport, this.myGrid, null);
        //string deptcode = Request.QueryString["deptCode"].ToString();
        //string tblx = Request.QueryString["type"].ToString() == "ystb" ? "" : "02";  // 02是财务填报  ""是部门填报
        //string nd = Request.QueryString["nd"].ToString();
        //IList<YsgcTb> ysMainTable = bll.GetMainTable(deptcode, nd, "", tblx);

        //临时文件    
        string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        //使用OleDb连接  
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
        using (con)
        {
            con.Open();
            //创建Sheet   
            OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([序号] VarChar,[年度] VarChar,[利润项目] VarChar,[预算科目] VarChar,[预算控制金额] VarChar)", con);
            cmdCreate.ExecuteNonQuery();
            //插入数据     
            for (int i = 0; i < this.myGrid.Items.Count; i++)
            {
                using (OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@xuhao,@nian,@xm,@yskm,@je)", con))
                {
                    string strxuhao = myGrid.Items[i].Cells[0].Text.Trim();
                    string niandu = myGrid.Items[i].Cells[1].Text.Trim();
                    string lirunxiangmu = myGrid.Items[i].Cells[2].Text.Trim();
                    string yskm = myGrid.Items[i].Cells[3].Text.Trim();
                    string strkzje = "";
                    decimal dekzje = 0;
                    TextBox txtkzje = myGrid.Items[i].Cells[3].FindControl("txt_je") as TextBox;
                    if (txtkzje != null)
                    {
                        strkzje = txtkzje.Text.Trim();
                        decimal.TryParse(strkzje, out dekzje);
                    }
                    strkzje = dekzje.ToString();
                    cmd.Parameters.AddWithValue("@xuhao", strxuhao);
                    cmd.Parameters.AddWithValue("@nian", niandu);
                    cmd.Parameters.AddWithValue("@xm", lirunxiangmu);
                    cmd.Parameters.AddWithValue("@yskm", yskm);
                    cmd.Parameters.AddWithValue("@je", strkzje);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        Response.ContentType = "application/ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=lrbxmfj.xls");
        Response.BinaryWrite(File.ReadAllBytes(tempFile));
        Response.End();
        File.Delete(tempFile);
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