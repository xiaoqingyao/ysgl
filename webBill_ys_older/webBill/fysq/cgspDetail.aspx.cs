using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Ajax;
using System.Text;
using System.IO;
using Bll;
using System.Data.SqlClient;

public partial class webBill_fysq_cgspDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            ClientScript.RegisterArrayDeclaration("allGys", allGys());
            Ajax.Utility.RegisterTypeForAjax(typeof(webBill_fysq_cgspDetail));
            txtXjdw1.Focus();
            if (!IsPostBack)
            {
                DataSet temp = server.GetDataSet("select * from bill_dataDic where dicType='03' order by dicCode");
                //this.RadioButtonList1.DataTextField = "dicName";
                //this.RadioButtonList1.DataValueField = "dicCode";
                //this.RadioButtonList1.DataSource = temp;
                //this.RadioButtonList1.DataBind();
                this.ddl_cglb.DataTextField = "dicName";
                this.ddl_cglb.DataValueField = "dicCode";
                this.ddl_cglb.DataSource = temp;
                this.ddl_cglb.DataBind();

                this.bindData();
            }
        }
    }


    #region 页面数据绑定
    private void bindData()
    {
        string type = Page.Request.QueryString["type"].ToString().Trim();
        if (type == "add")
        {
            //this.txtCgrq.Attributes.Add("onfocus", "javascript:setday(this);");
            this.txtCgrq.Attributes.Add("onfocus", "edit()");
            this.ddl_cglb.SelectedIndex = 0;
            this.lblDept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            this.lblDeptShow.Text = (new billCoding()).getDeptLevel2Name(server.GetCellValue("select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'"));

            
            DateTime dt = System.DateTime.Now;
            this.txtCgrq.Text = System.DateTime.Now.ToString("yyyy-MM-dd");// dt.ToShortDateString();
            this.lblCbr.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'");

            this.lbl_BillCode.Text = (new GuidHelper()).getNewGuid();

            this.CreateCgspCode();

        }
        else
        {
            if (type == "edit")
            {
                //this.txtCgrq.Attributes.Add("onfocus", "javascript:setday(this);");
            }
            DataSet temp = server.GetDataSet("select cgbh,sj,cgdept,cglb,sm,(select '['+usercode+']'+username from bill_users where usercode=cbr) as cbr,spyj01,spyj02,spyj03,spyj04,spyj05,spyj06,spyj07,spyj08,gys,khh,zh from bill_cgsp where cgbh='" + Page.Request.QueryString["cgbh"].ToString().Trim() + "'");
            this.lbl_BillCode.Text = temp.Tables[0].Rows[0]["cgbh"].ToString().Trim();
            this.lblCgbh.Text = temp.Tables[0].Rows[0]["cgbh"].ToString().Trim();
            DateTime dt = DateTime.Parse(temp.Tables[0].Rows[0]["sj"].ToString().Trim());
            this.txtCgrq.Text = System.DateTime.Now.ToString("yyyy-MM-dd"); //dt.ToShortDateString();
            this.lblDept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + temp.Tables[0].Rows[0]["cgdept"].ToString().Trim() + "'");
            this.lblDeptShow.Text = (new billCoding()).getDeptLevel2Name(temp.Tables[0].Rows[0]["cgdept"].ToString().Trim());


            this.lblCbr.Text = temp.Tables[0].Rows[0]["cbr"].ToString().Trim();

            this.txtSm.Text = temp.Tables[0].Rows[0]["sm"].ToString().Trim();
            this.ddl_cglb.SelectedValue = temp.Tables[0].Rows[0]["cglb"].ToString().Trim();
            this.txtbmzgyj.Text = temp.Tables[0].Rows[0]["spyj01"].ToString().Trim();
            this.txtbmjlyj.Text = temp.Tables[0].Rows[0]["spyj02"].ToString().Trim();
            this.txtzhglbyj.Text = temp.Tables[0].Rows[0]["spyj03"].ToString().Trim();
            this.txtcwbmyj.Text = temp.Tables[0].Rows[0]["spyj04"].ToString().Trim();
            this.txtjsbmyj.Text = temp.Tables[0].Rows[0]["spyj05"].ToString().Trim();
            this.txtfgldsp.Text = temp.Tables[0].Rows[0]["spyj06"].ToString().Trim();
            this.txtzkjssp.Text = temp.Tables[0].Rows[0]["spyj07"].ToString().Trim();
            this.txtzjlsp.Text = temp.Tables[0].Rows[0]["spyj08"].ToString().Trim();
            this.txtGys.Value = temp.Tables[0].Rows[0]["gys"].ToString().Trim();
            this.txtKhh.Value = temp.Tables[0].Rows[0]["khh"].ToString().Trim();
            this.txtZh.Value = temp.Tables[0].Rows[0]["zh"].ToString().Trim();



            temp = server.GetDataSet("select * from bill_cgsp_xjb where cgbh='" + Page.Request.QueryString["cgbh"].ToString().Trim() + "'");
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                TextBox txtXjdw = (TextBox)Page.FindControl("txtXjdw" + (i + 1));
                txtXjdw.Text = temp.Tables[0].Rows[i]["xjdw"].ToString().Trim();

                TextBox txtXj = (TextBox)Page.FindControl("txtXj" + (i + 1));
                txtXj.Text = temp.Tables[0].Rows[i]["xjqk"].ToString().Trim();
            }
            //报告申请单
            this.bindLscgData();
            //询价附件
            this.setFjFileInfo();

            if (type == "look" || type == "audit")
            {
                Button5.Visible = false;
                btn_insert.Visible = false;
                this.btn_bc.Visible = false;
                this.Button2.Visible = false;
                this.Button3.Visible = false;
                this.btnRefresh.Visible = false;
                //this.myGrid.Columns[0].Visible = false;
                this.btnScdj.Visible = false;
                this.btnRefreshFj.Visible = false;
                this.upLoadFiles.Visible = false;
                this.txtCgrq.Enabled = false;
                
            }
        }
        //明细
        DataSet temp1 = server.GetDataSet("select * from bill_cgsp_mxb where cgbh='" + this.lbl_BillCode.Text.ToString().Trim() + "' order by cgIndex");
        this.DataGrid1.DataSource = temp1;
        this.DataGrid1.DataBind();

        if (type == "audit")
        {
            this.btn_ok.Visible = this.btn_cancel.Visible = true;
        }
        else
        {
            this.btn_ok.Visible = this.btn_cancel.Visible = false;
        }
    }
    #endregion

    protected void btn_bc_Click(object sender, EventArgs e)
    {
        double zje = 0;
        try
        {
            zje = double.Parse(server.GetCellValue("select isnull(sum(zj),0) from bill_cgsp_mxb where cgbhGuid='" + this.lbl_BillCode.Text.ToString().Trim() + "'"));
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('采购明细金额输入错误,请检查！');", true);
            return;
        }
        if (zje <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('采购明细总金额错误！');", true);
            return;
        }

        try {
            DateTime.Parse(this.txtCgrq.Text.ToString().Trim());
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('日期输入错误,请检查！');", true);
            return;
        }

        //mxl 2012.04.09 检测该申请类别是否需要附加申请单
        DataSet temp = server.GetDataSet("select isnull(cdj,'0') as cdj from bill_dataDic where dicType='03' and diccode = '" + this.ddl_cglb.SelectedValue+ "'");
        if (temp.Tables[0].Rows[0]["cdj"].ToString() == "1")
        {
            if (this.myGrid.Items.Count < 1) 
            {
                 ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该申请类别需要附加报告单,请添加！');", true);
                 return;
            }
        }
        //

        string str_stepid = "-1";
        string str_billuser = Session["userCode"].ToString().Trim();
        string str_billdate = this.txtCgrq.Text.ToString().Trim();
        string str_billdept = server.GetCellValue("select userdept from bill_users where usercode='" + str_billuser + "'");
        string bm = this.lblDept.Text.ToString().Trim();
        bm = bm.Substring(1, bm.IndexOf("]") - 1);

        string type = Page.Request.QueryString["type"].ToString().Trim();
        List<string> list = new List<string>();
        if (type == "add")
        {
            DataSet temp2 = server.GetDataSet("select cgbh from bill_cgsp where cgbh='" + this.lblCgbh.Text.ToString().Trim() + "'");
            if (temp2.Tables[0].Rows.Count != 0)
            {
                this.CreateCgspCode();
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的采购申请已存在,系统已重新生成,请保存！');", true);
                
                return;
            }

            list.Add("insert into bill_cgsp(cgbh,sj,cgdept,cglb,sm,cbr,cgze,spyj01,spyj02,spyj03,spyj04,spyj05,spyj06,spyj07,spyj08,gys,khh,zh) values('" + this.lblCgbh.Text.ToString().Trim() + "','" + str_billdate + "','" + bm + "','" + this.ddl_cglb.SelectedItem.Value + "','" + this.txtSm.Text.ToString().Trim() + "','" + Session["userCode"].ToString().Trim() + "'," + zje.ToString() + ",'" + this.txtbmzgyj.Text.ToString().Trim() + "','" + this.txtbmjlyj.Text.ToString().Trim() + "','" + this.txtzhglbyj.Text.ToString().Trim() + "','" + this.txtcwbmyj.Text.ToString().Trim() + "','" + this.txtjsbmyj.Text.ToString().Trim() + "','" + this.txtfgldsp.Text.ToString().Trim() + "','" + this.txtzkjssp.Text.ToString().Trim() + "','" + this.txtzjlsp.Text.ToString().Trim() + "','" + this.txtGys.Value.Trim() + "','" + this.txtKhh.Value.Trim() + "','" + this.txtZh.Value.Trim() + "') ");
            //申明主表添加
            list.Add("insert into bill_main(looptimes,billType,billcode,billname,flowid,stepid,billuser,billdate,billdept,billje) values(1,'1','" + this.lblCgbh.Text.ToString().Trim() + "','','cgsp','" + str_stepid + "','" + str_billuser + "','" + str_billdate + "','" + str_billdept + "','" + zje.ToString() + "')");
            //修改明细表的记录 cgbhGuid临时记录 变为 正式采购编号:弹窗中增加明细时，cgbh是空的，只使用cgbhGuid
            list.Add("update bill_cgsp_mxb set cgbh='"+this.lblCgbh.Text.ToString().Trim()+"',cgbhGuid='" + this.lblCgbh.Text.ToString().Trim() + "' where cgbhGuid='" + this.lbl_BillCode.Text.ToString().Trim() + "'");
        }
        else //编辑
        {
            //修改单据时
            list.Add("update bill_cgsp set sj='" + str_billdate + "',cglb='" + this.ddl_cglb.SelectedItem.Value + "',sm='" + this.txtSm.Text.ToString().Trim() + "',cgze=" + zje.ToString() + ",spyj01='" + this.txtbmzgyj.Text.ToString().Trim() + "',spyj02='" + this.txtbmjlyj.Text.ToString().Trim() + "',spyj03='" + this.txtzhglbyj.Text.ToString().Trim() + "',spyj04='" + this.txtcwbmyj.Text.ToString().Trim() + "',spyj05='" + this.txtjsbmyj.Text.ToString().Trim() + "',spyj06='" + this.txtfgldsp.Text.ToString().Trim() + "',spyj07='" + this.txtzkjssp.Text.ToString().Trim() + "',spyj08='" + this.txtzjlsp.Text.ToString().Trim() + "',gys='" + this.txtGys.Value.Trim() + "',khh='" + this.txtKhh.Value.Trim() + "',zh='"+this.txtZh.Value.Trim()+"' where cgbh='" + this.lblCgbh.Text.ToString().Trim() + "' ");
            list.Add("update bill_main set  billname='' , billuser='" + str_billuser + "',billdate='" + str_billdate + "',billdept='" + str_billdept + "',billje='" + zje.ToString() + "',stepid='" + str_stepid + "' where flowid='cgsp' and  billcode='" + Request.QueryString["cgbh"].ToString() + "'");
            //list.Add("delete from bill_cgsp_mxb where cgbh='" + Page.Request.QueryString["cgbh"].ToString().Trim() + "'");
            list.Add("delete from bill_cgsp_xjb where cgbh='" + Page.Request.QueryString["cgbh"].ToString().Trim() + "'");
            list.Add("update bill_cgsp_mxb set cgbh='" + this.lblCgbh.Text.ToString().Trim() + "' where cgbhGuid='" + this.lbl_BillCode.Text.ToString().Trim() + "'");
        }
        //附件
        list.Add("update bill_cgsp_fjb set djStatus='1' where djStatus='0' and billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "'");
        list.Add("delete from bill_cgsp_fjb where djStatus='2' and billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "'");
        list.Add("update bill_cgsp_fjb set billCode='" + this.lblCgbh.Text.ToString().Trim() + "' where billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "'");

        //询价表
        if (this.txtXjdw1.Text.ToString().Trim() != "")
        {
            list.Add("insert into bill_cgsp_xjb(cgbh,xjdw,xjqk) values('" + this.lblCgbh.Text.ToString().Trim() + "','" + this.txtXjdw1.Text.ToString().Trim() + "','" + this.txtXj1.Text.ToString().Trim() + "')");
        }
        if (this.txtXjdw2.Text.ToString().Trim() != "")
        {
            list.Add("insert into bill_cgsp_xjb(cgbh,xjdw,xjqk) values('" + this.lblCgbh.Text.ToString().Trim() + "','" + this.txtXjdw2.Text.ToString().Trim() + "','" + this.txtXj2.Text.ToString().Trim() + "')");
        }
        if (this.txtXjdw3.Text.ToString().Trim() != "")
        {
            list.Add("insert into bill_cgsp_xjb(cgbh,xjdw,xjqk) values('" + this.lblCgbh.Text.ToString().Trim() + "','" + this.txtXjdw3.Text.ToString().Trim() + "','" + this.txtXj3.Text.ToString().Trim() + "')");
        }

        list.Add("update bill_cgsp_lscg set status='1' where billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "' and status='0'");
        list.Add("delete from bill_cgsp_lscg where billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "' and status='2'");
        list.Add("update bill_cgsp_lscg set billCode='" + this.lblCgbh.Text.ToString().Trim() + "' where billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "'");

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
    }
    protected void btn_fh_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new List<string>();
        list.Add("delete from bill_cgsp_lscg where billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "' and status='0'");
        list.Add("update bill_cgsp_lscg set status='1' where billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "' and status='2'");
        list.Add("update bill_cgsp_fjb set djStatus='1' where djStatus='2' and billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "'");
        list.Add("delete from bill_cgsp_fjb where djStatus='0' and billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "'");

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('系统异常【单据编号：" + this.lbl_BillCode.Text.ToString().Trim() + "】！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"\";self.close();", true);
        }
    }

    public void CreateCgspCode()
    {
        string cgspCode = (new billCoding()).getCgspCode();
        if (cgspCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
            this.btn_bc.Visible = false;
        }
        else
        {
            this.lblCgbh.Text = cgspCode;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.CreateCgspCode();
    }

    [Ajax.AjaxMethod]
    public string CalZj(string v1, string v2)
    {
        try
        {
            double d = double.Parse(v1) * double.Parse(v2);

            return d.ToString("0.00");
        }
        catch
        {
            return "";
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string billCode = this.lbl_BillCode.Text.ToString().Trim();
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('selectLscg.aspx?billCode=" + billCode + "');", true);
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.bindLscgData();
    }

    public void bindLscgData()
    {
        string billCode = this.lbl_BillCode.Text.ToString().Trim();

        string sql = "select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,stepid as stepID_ID,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='lscg' and bill_workFlowStep.stepid=a.stepid ) end) as stepID,(select dicname from bill_dataDic where dictype='03' and diccode =b.cglb) as cglb from bill_main a,bill_lscg b where a.flowid='lscg' and a.billCode=b.cgbh ";
        sql += " and b.cgbh in (select lscgCode from bill_cgsp_lscg where billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "' and status in ('0','1'))";

        DataSet temp = server.GetDataSet(sql);
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();


        //明细
        temp = server.GetDataSet("select * from bill_cgsp_mxb where cgbhGuid='" + billCode + "' order by cgIndex");
        this.DataGrid1.DataSource = temp;
        this.DataGrid1.DataBind();

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new List<string>();

        string billCode = this.lbl_BillCode.Text.ToString().Trim();
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                string lscgCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                string status = server.GetCellValue("select status from bill_cgsp_lscg where billCode='" + billCode + "' and lscgCode='" + lscgCode + "'");
                
                    list.Add("delete from bill_cgsp_lscg where billCode='" + billCode + "' and lscgCode='" + lscgCode + "'");
                
                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待删除的报告申请单！');", true);
        }
        else
        {
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {
                this.bindLscgData();
            }
        }
    }
    protected void btn_Details_Click(object sender, EventArgs e) {
        string lscgCode = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++) {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                lscgCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                break;
            }
        }
        if (!lscgCode.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('lscgDetail.aspx?type=look&cgbh="+lscgCode+"');", true);
        }
        else {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择要查看的单据！');", true);
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        int count = 0;
        string index = "";
        for (int i = 0; i <= this.DataGrid1.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.DataGrid1.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                index += this.DataGrid1.Items[i].Cells[1].Text.ToString().Trim() + ",";
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待删除的项！');", true);
            return;
        }
        else
        {
            index = index.Substring(0, index.Length - 1);

            if (server.ExecuteNonQuery("delete from bill_cgsp_mxb where cgIndex in (" + index + ")") == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {
                this.bindLscgData();
            }
        }
    }
    protected void btnScdj_Click(object sender, EventArgs e)
    {
        if (this.upLoadFiles.PostedFile.ContentLength == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待上传文件！');", true);
            return;
        }

        string guid = (new GuidHelper()).getNewGuid();
        string fileName = this.upLoadFiles.PostedFile.FileName;
        string cFileName = fileName.Substring(fileName.LastIndexOf("\\") + 1, fileName.Length - fileName.LastIndexOf("\\") - 1);//0\\2
        string extName = fileName.Substring(fileName.LastIndexOf(".") + 1, fileName.Length - fileName.LastIndexOf(".") - 1);

        try
        {
            string newPath = Server.MapPath(".") + "/files/" + guid + "." + extName;

            FileInfo file2 = new FileInfo(newPath);

            if (file2.Exists)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('文件上传失败！');", true);
                return;
            }
            else
            {
                this.upLoadFiles.PostedFile.SaveAs(newPath);

                string sql = "insert into bill_cgsp_fjb(billCode,djUrl,djName,djGuid,djStatus) values('" + this.lbl_BillCode.Text.ToString().Trim() + "','" + guid + "." + extName + "','" + cFileName + "','" + guid + "','0')";

                if (server.ExecuteNonQuery(sql) == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('上传失败！');", true);
                    return;
                }
                else
                {
                    this.btnRefreshFj_Click(sender, e);
                }
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('数据库记录失败！');", true);
            return;
        }
    }
    protected void btnRefreshFj_Click(object sender, EventArgs e)
    {
        #region 询价单据
        this.setFjFileInfo();
        #endregion
    }
    public void setFjFileInfo()
    {
        string tempStr = "";
        DataSet temp = server.GetDataSet("select * from bill_cgsp_fjb where billCode='" + this.lbl_BillCode.Text.ToString().Trim() + "' and isnull(djStatus,'1')<>'2'");
        if (Page.Request.QueryString["type"].ToString().Trim() == "look")
        {
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                tempStr += "&nbsp;" + temp.Tables[0].Rows[i]["djName"].ToString().Trim() + "&nbsp;&nbsp;&nbsp;&nbsp;<a href=files/" + temp.Tables[0].Rows[i]["djUrl"].ToString().Trim() + " target=\"_blank\">查看</a><br/>";
            }
        }
        else
        {
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                tempStr += "&nbsp;" + temp.Tables[0].Rows[i]["djName"].ToString().Trim() + "&nbsp;&nbsp;&nbsp;&nbsp;<a href=# onclick=\"deleteBxdj('" + temp.Tables[0].Rows[i]["djGuid"].ToString().Trim() + "');\">删除</a>&nbsp;&nbsp;<a href=files/" + temp.Tables[0].Rows[i]["djUrl"].ToString().Trim() + " target=\"_blank\">查看</a><br/>";
            }
        }
        this.divBxdj.InnerHtml = tempStr;
        this.btnScdj.Text = "上传单据【已有单据:" + temp.Tables[0].Rows.Count + "】";
    }


    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        //if (e != null)
        //{
        //    if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        //    {
        //        e.Item.Cells[1].Text = "<a href=# style=\"color:Blue\" onclick=\"openDetail('lscgDetail.aspx?type=look&cgbh=" + e.Item.Cells[1].Text + "');\">" + e.Item.Cells[9].Text + "</a>";
        //    }
        //}
    }
    /// <summary>
    ///删除报销单据
    /// </summary>
    /// <param name="djGuid"></param>
    /// <returns></returns>
    [Ajax.AjaxMethod()]
    public bool DeleteBxdj(string djGuid)
    {
        if (server.ExecuteNonQuery("update bill_cgsp_fjb set djstatus='2' where djGuid='" + djGuid + "'") == -1) return false;
        else return true;
    }

     AddChangyong addcy = new AddChangyong();
    /// <summary>
    /// 添加报销人到常用  15
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddbxrTOChangyong_Click(object sender, EventArgs e)
    {
        string strgys = txtGys.Value.Trim();//摘要
        string strdept = this.lblDept.Text.ToString().Trim();//部门

        try
        {
            strdept = strdept.Substring(1, strdept.IndexOf(']') - 1);
        }
        catch (Exception)
        {
            strdept = "";
        }
       
       string temp=txtKhh.Value.Trim();
       if (!string.IsNullOrEmpty(temp))
       {
           strgys+="|"+temp;
       }
       else
       {
           strgys += "| ";
       }
       
       
       temp=txtZh.Value.Trim();
       if (!string.IsNullOrEmpty(temp))
       {
           strgys += "|" + temp;
       }
       else
       {
           strgys += "| ";
       }
       
        int iRel = addcy.intRowAdd("21", strdept, strgys);
        if (iRel <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败。');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加成功');", true);
        }
    }

    /// <summary>
    /// 获取供应商
    /// </summary>
    /// <returns></returns>
    private string allGys()
    {
        string strdept = this.lblDept.Text.ToString().Trim();//部门

        try
        {
            strdept = strdept.Substring(1, strdept.IndexOf(']') - 1);
        }
        catch (Exception)
        {
            strdept = "";
        }
        
        
      
        try
        {
            strdept = strdept.Substring(1, strdept.IndexOf(']') - 1);
        }
        catch (Exception)
        {
            strdept = "";
        }
        string strselectsql = "";
        SqlParameter[] arr;
        if (strdept.Equals(""))
        {
            strselectsql = "select  left(dicname, case charindex('|',dicname)-1 when -1 then LEN(dicname) else charindex('|',dicname)-1 end  )  as dicname from bill_datadic where dictype=@dictype  ";
            arr = new SqlParameter[] { new SqlParameter("@dictype", "21") };
        }
        else
        {
            strselectsql = "select left(dicname, case charindex('|',dicname)-1 when -1 then LEN(dicname) else charindex('|',dicname)-1 end  )  as dicname from bill_datadic where dictype='21' and diccode=@diccode ";
            arr = new SqlParameter[] { new SqlParameter("@diccode", strdept) };
        }
        DataSet ds = server.GetDataSet(strselectsql, arr);
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dicname"]));
            arry.Append("',");
        }
        string script = "";
        if (arry.Length > 0)
        {
            script = arry.ToString().Substring(0, arry.Length - 1);
        }
        return script;
    }


}