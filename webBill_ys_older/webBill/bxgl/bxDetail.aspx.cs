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
using Ajax;
using System.IO;

public partial class webBill_bxgl_bxDetail : System.Web.UI.Page
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
            //Session["userCode"] = "000001";
            //Session["userName"] = "admin";
            Ajax.Utility.RegisterTypeForAjax(typeof(webBill_bxgl_bxDetail));
            this.btnSelectBxr.Attributes.Add("onclick", "javascript:selectry('../select/userFrame.aspx');return false;");

            

            if (Page.Request.QueryString["type"] == null || Page.Request.QueryString["type"].ToString().Trim() == "add")
            {
                this.txtSqrq.Attributes.Add("onfocus", "javascript:setday(this);");
            }
            else
            {
                this.txtSqrq.ReadOnly = true;
            }
            this.rdoJkdk1.Attributes.Add("onclick", "javascript:changeJkStatus();");
            this.rdoJkdk0.Attributes.Add("onclick", "javascript:changeJkStatus();");

            if (!IsPostBack)
            {
                (new webBillLibrary.bxgl()).bindBxmxlx(this.drpBxmxlx);

                this.bindShow();
            }
        }
    }

    void bindShow()
    {
        string type = Page.Request.QueryString["type"].ToString().Trim();
        this.lblType.Text = type;
        if (type == "add")
        {
            this.lblBillCode.Text = (new GuidHelper()).getNewGuid();
            //this.lblBillCode.Text = "A3546967-C01C-4670-97D5-8ABF27997F1E";
            this.txtJbr.Text = "[" + Session["userCode"].ToString().Trim() + "]" + Session["userName"].ToString().Trim();
            this.txtSqrq.Text = System.DateTime.Now.ToShortDateString();


            (new webBillLibrary.bxgl()).bindFysq(this.lblBillCode.Text.ToString().Trim(), this.myGrid);
            this.btn_Print.Visible = false;

        }
        else
        {
            this.lblBillCode.Text = Page.Request.QueryString["billCode"].ToString().Trim();

            //脚本数据
            this.bindJsInfo();

            //主单数据
            DataSet tempMain = server.GetDataSet("select billCode,(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUser, billDept,billDate,billJe from bill_main where flowid='ybbx' and billCode='" + this.lblBillCode.Text.ToString().Trim() + "'");
            this.txtBxr.Value = tempMain.Tables[0].Rows[0]["billUser"].ToString().Trim();
            this.btnSelectBxr.Visible = false;
            //this.txtDept.Value = tempMain.Tables[0].Rows[0]["billdept"].ToString().Trim();
            string strdsDept = tempMain.Tables[0].Rows[0]["billdept"].ToString().Trim();
            //string strDept = "select  isnull(sjdeptcode,'') from bill_departments where deptcode='" + strdsDept + "' ";
            //是
            if (isTopDept("n", strdsDept))
            {
                string dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + strdsDept + "'");
                this.txtDept.Value = dept;
                this.txtbxdept.Value = dept;
            }
            else
            {
                //所在部门
                string Dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + strdsDept + "'");
                //上级部门
                string sjDept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode='" + strdsDept + "')");
                this.txtDept.Value = sjDept;
                this.txtbxdept.Value = Dept;
            }

            this.txtSqrq.Text = DateTime.Parse(tempMain.Tables[0].Rows[0]["billdate"].ToString().Trim()).ToShortDateString();
            this.txtHjjeXx.Value = double.Parse(tempMain.Tables[0].Rows[0]["billje"].ToString().Trim()).ToString("0.00");
            //详细信息
            DataSet tempYbbx = server.GetDataSet("select bxmxlx,(select '['+usercode+']'+username from bill_users where usercode=bxr) as jbr,bxzy,bxsm,sfdk,ytje,ybje from bill_ybbxmxb where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'");
            this.txtBxsm.Text = tempYbbx.Tables[0].Rows[0]["bxsm"].ToString().Trim();
            this.txtBxzy.Text = tempYbbx.Tables[0].Rows[0]["bxzy"].ToString().Trim();
            this.txtJbr.Text = tempYbbx.Tables[0].Rows[0]["jbr"].ToString().Trim();
            this.drpBxmxlx.SelectedValue = tempYbbx.Tables[0].Rows[0]["bxmxlx"].ToString().Trim();
            //this.lblYtje.Text = double.Parse(tempYbbx.Tables[0].Rows[0]["ytje"].ToString().Trim()).ToString("0.00");
            this.txtytje.Value = double.Parse(tempYbbx.Tables[0].Rows[0]["ytje"].ToString().Trim()).ToString("0.00");
            //this.lblYbje.Text = double.Parse(tempYbbx.Tables[0].Rows[0]["ybje"].ToString().Trim()).ToString("0.00");
            this.txtYbje.Value = double.Parse(tempYbbx.Tables[0].Rows[0]["ybje"].ToString().Trim()).ToString("0.00");
            if (tempYbbx.Tables[0].Rows[0]["sfdk"].ToString().Trim() == "1")
            {
                this.rdoJkdk1.Checked = true;
            }
            if (Page.Request.QueryString["type"].ToString().Trim() == "look")
            {
                this.btn_Save.Visible = false;
                this.drpBxmxlx.Enabled = false;
                rdoJkdk0.Enabled = false;
                txtBxsm.ReadOnly = true;
                rdoJkdk1.Enabled = false;
                this.txtBxzy.ReadOnly = true;
                this.btnAddFysq.Style["display"] = "none";
                this.btnDelteFysq.Visible = false;
                try
                {
                    this.myGrid.Items[0].Visible = false;
                }
                catch
                { }
                btnAddFykm.Visible = false;
                btnDelFykm.Visible = false;
            }
        }
        
    }


    protected void btn_Save_Click(object sender, EventArgs e)
    {
        //string zje1 = server.GetCellValue("select sum(je) from bill_ybbxmxb_fykm where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'");
        //string zje2 = server.GetCellValue("select billje from bill_main where billcode='" + this.lblBillCode.Text.ToString().Trim() + "'");
        //if (zje1.ToString().Trim() == "" || zje2.ToString().Trim() == "" || double.Parse(zje1) != double.Parse(zje2))
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('单据总额与科目合计不符！');", true);
        //    this.bindJsInfo();
        //    return;
        //}
        //if (server.GetDataSet("select * from bill_ybbxmxb_fykm where je<>(select sum(je) from bill_ybbxmxb_fykm_dept where bill_ybbxmxb_fykm_dept.kmmxguid=bill_ybbxmxb_fykm.mxGuid) and billcode='" + this.lblBillCode.Text.ToString().Trim() + "'").Tables[0].Rows.Count != 0)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('报销科目总额与单位分摊合计不符！');", true);
        //    this.bindJsInfo();
        //    return;
        //}


        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();


        string user = this.txtBxr.Value.ToString().Trim();
        if (user == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择报销人！');", true);
            return;
        }
        else
        {
            user = user.Substring(1, user.IndexOf("]") - 1);
        }
        string dept = this.txtbxdept.Value.ToString().Trim();
        if (dept == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择报销人！');", true);
            return;
        }
        else
        {
            dept = dept.Substring(1, dept.IndexOf("]") - 1);
        }
        string sfdk = "0";
        if (this.rdoJkdk1.Checked == true)
        {
            sfdk = "1";
        }
        if (Page.Request.QueryString["type"].ToString().Trim() == "add")
        {
            string sqlMain = "insert into bill_main(billCode,flowid,stepid,billuser,billdate,billdept,billje,looptimes) values('" + this.lblBillCode.Text.ToString().Trim() + "','ybbx','-1','" + user + "','" + this.txtSqrq.Text.ToString().Trim() + "','" + dept + "','" + this.txtHjjeXx.Value.ToString().Trim() + "','0')";
            list.Add(sqlMain);

            //string sqlYbbxmxb = "insert into bill_ybbxmxb(billCode,bxr,bxzy,bxsm,sfdk,ytje,ybje,sfgf,bxmxlx) values('" + this.lblBillCode.Text.ToString().Trim() + "','" + Session["userCode"].ToString().Trim() + "','" + this.txtBxzy.Text.ToString().Trim() + "','" + this.txtBxsm.Text.ToString().Trim() + "','" + sfdk + "','" + this.lblYtje.Text.ToString().Trim() + "','" + this.lblYbje.Text.ToString().Trim() + "','0','" + this.drpBxmxlx.SelectedItem.Value + "')";
            string sqlYbbxmxb = "insert into bill_ybbxmxb(billCode,bxr,bxzy,bxsm,sfdk,ytje,ybje,sfgf,bxmxlx) values('" + this.lblBillCode.Text.ToString().Trim() + "','" + Session["userCode"].ToString().Trim() + "','" + this.txtBxzy.Text.ToString().Trim() + "','" + this.txtBxsm.Text.ToString().Trim() + "','" + sfdk + "','" + this.txtytje.Value.ToString().Trim() + "','" + this.txtYbje.Value.ToString().Trim() + "','0','" + this.drpBxmxlx.SelectedItem.Value + "')";
            list.Add(sqlYbbxmxb);
        }
        else
        {
            string sqlMain = "update bill_main set billje='" + this.txtHjjeXx.Value.ToString().Trim() + "' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'";
            list.Add(sqlMain);

            //string sqlYbbxmxb = "update bill_ybbxmxb set bxzy='" + this.txtBxzy.Text.ToString().Trim() + "',bxsm='" + this.txtBxsm.Text.ToString().Trim() + "',sfdk='" + sfdk + "',ytje='" + this.lblYtje.Text.ToString().Trim() + "',ybje='" + this.lblYbje.Text.ToString().Trim() + "',bxmxlx='" + this.drpBxmxlx.SelectedItem.Value + "' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'";
            string sqlYbbxmxb = "update bill_ybbxmxb set bxzy='" + this.txtBxzy.Text.ToString().Trim() + "',bxsm='" + this.txtBxsm.Text.ToString().Trim() + "',sfdk='" + sfdk + "',ytje='" + this.txtytje.Value.ToString().Trim() + "',ybje='" + this.txtYbje.Value.ToString().Trim() + "',bxmxlx='" + this.drpBxmxlx.SelectedItem.Value + "' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'";
            list.Add(sqlYbbxmxb);
        }

        if (sfdk == "0")
        {
            list.Add("delete from bill_ybbxmxb_fydk where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'");
        }
        else
        {
            list.Add("update bill_ybbxmxb_fydk set status='1' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='0'");
            list.Add("update bill_ybbxmxb_fydk set status='1' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='2'");
        }
        //list.Add("update  bill_ybbxmxb_bxdj set djStatus='1' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and djStatus='0'");//删除临时的数据
        //list.Add("delete from bill_ybbxmxb_bxdj where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and djStatus='2'");//恢复删除的数据
        //list.Add("update bill_ybbxmxb_fykm set status='1' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='0'");
        //list.Add("delete from bill_ybbxmxb_fykm where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='2'");
        //list.Add("update bill_ybbxmxb_fykm_ft set status='1' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='0'");
        //list.Add("delete from bill_ybbxmxb_fykm_ft where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='2'");

        //list.Add("update bill_ybbx_fysq set status='1' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='0'");
        //list.Add("delete from bill_ybbx_fysq where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='2'");

        //list.Add("delete from bill_ybbxmxb_hsxm where djGuid='" + this.lblBillCode.Text.ToString().Trim() + "'");

        //int newCount = 0;
        //for (int i = 0; i <= this.DataGrid1.Items.Count - 1; i++)
        //{
        //    CheckBox chk = (CheckBox)this.DataGrid1.Items[i].FindControl("CheckBox1");
        //    if (chk.Checked)
        //    {
        //        newCount += 1;
        //        list.Add("insert into bill_ybbxmxb_hsxm values('" + this.lblBillCode.Text.ToString().Trim() + "','" + (new GuidHelper()).getNewGuid() + "','" + this.DataGrid1.Items[i].Cells[1].Text.ToString().Trim() + "')");
        //    }
        //}
        //if (newCount > 1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('仅能选择一个报销项目！');", true);
        //    return;
        //}
        //是否控制超预算
        bool flagCys = false;
        string yslx = this.drpBxmxlx.SelectedItem.Value;
        string sfkz = server.GetCellValue("select isnull(cys,'1') from bill_dataDic where dictype='02' and diccode='" + yslx + "'");
        if (sfkz == "1")
        {
            System.Data.DataSet dtMx = server.GetDataSet("select * from bill_ybbxmxb_fykm where billcode='" + this.lblBillCode.Text.ToString().Trim() + "'");
            for (int i = 0; i <= dtMx.Tables[0].Rows.Count - 1; i++)
            {
                string yskm = dtMx.Tables[0].Rows[i]["fykm"].ToString().Trim();
                string time = this.txtSqrq.Text.ToString().Trim();
                double je = double.Parse(dtMx.Tables[0].Rows[i]["je"].ToString().Trim());

                double[] nianYs = (new yskm()).getNianYs(dept, time, yskm);
                if (je > nianYs[1])
                {
                    flagCys = true;
                }
                double[] jdYs = (new yskm()).getJdYs(dept, time, yskm);
                if (je > jdYs[1])
                {
                    flagCys = true;
                }
                double[] yueYs = (new yskm()).getYueYs(dept, time, yskm);
                if (je > yueYs[1])
                {
                    flagCys = true;
                }
            }
        }
        if (flagCys == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('报销金额超预算！');", true);
            return;
        }
        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            this.btn_Save.Visible = false;
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"sucess\";self.close();", true);
        }
    }
    protected void btn_Cancle_Click(object sender, EventArgs e)
    {
        if (Page.Request.QueryString["type"].ToString().Trim() == "add")
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            //list.Add("delete from bill_ybbxmxb_fydk where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='0'");
            //list.Add("update bill_ybbxmxb_fydk set status='1' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='2'");

            //list.Add("delete from bill_ybbxmxb_bxdj where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and djStatus='0'");//删除临时的数据
            //list.Add("update bill_ybbxmxb_bxdj set djStatus='1' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and djStatus='2'");//恢复删除的数据

            //list.Add("delete from bill_ybbxmxb_fykm where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='0'");
            //list.Add("update bill_ybbxmxb_fykm set status='1' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='2'");

            //list.Add("delete from bill_ybbxmxb_fykm_ft where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='0'");
            //list.Add("update bill_ybbxmxb_fykm_ft set status='1' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='2'");
            //list.Add("delete from bill_ybbx_fysq where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='0'");

            //list.Add("delete from bill_ybbxmxb_fydk where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='0'");
            //list.Add("update bill_ybbxmxb_fydk set status='1' where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and status='2'");

            list.Add("delete from bill_ybbxmxb_hsxm where kmmxGuid in (select mxGuid from bill_ybbxmxb_fykm where billCode='" + this.lblBillCode.Text.ToString().Trim() + "')");
            list.Add("delete from bill_ybbxmxb_fykm_dept where kmmxGuid in (select mxGuid from bill_ybbxmxb_fykm where billCode='" + this.lblBillCode.Text.ToString().Trim() + "')");
            list.Add("delete from bill_ybbxmxb_bxdj where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'");
            list.Add("delete from bill_ybbxmxb_fykm where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'");
            list.Add("delete from bill_ybbx_fysq where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'");

            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"sucess\";self.close();", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"sucess\";self.close();", true);
        }
    }

    /// <summary>
    /// 获取单位信息
    /// </summary>
    /// <param name="userInfo"></param>
    /// <returns>0二级部门；1所在部门
    /// </returns>
    [Ajax.AjaxMethod()]
    public string[] getUserInfo(string userInfo)
    {
        string[] arr = new string[2] { "",""}; 
        if (userInfo == "")
        {
            return arr;
        }
        userInfo = userInfo.Substring(1, userInfo.IndexOf("]") - 1);
        if (isTopDept("y",userInfo))
        {
            arr[0] = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode =(select userdept from bill_users where usercode='" + userInfo + "')");
        }
        else
        {
            arr[0] = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + userInfo + "'))");
        }
        arr[1] = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode =(select userdept from bill_users where usercode='" + userInfo + "')");
        return arr;
    }

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
        return true;
    }

    /// <summary>
    ///删除报销单据
    /// </summary>
    /// <param name="djGuid"></param>
    /// <returns></returns>
    [Ajax.AjaxMethod()]
    public bool DeleteBxdj(string djGuid)
    {
        if (server.ExecuteNonQuery("update bill_ybbxmxb_bxdj set djstatus='2' where djGuid='" + djGuid + "'") == -1) return false;
        else return true;
    }

    /// <summary>
    /// 绑定脚本生成的数据信息
    /// </summary>
    public void bindJsInfo()
    {
        
        this.btnSelectBxr.Visible = false;
        string type = Page.Request.QueryString["type"].ToString().Trim();

        #region 报销单据
        string tempStr = "";
        DataSet temp = server.GetDataSet("select * from bill_ybbxmxb_bxdj where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'");
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
        #endregion

        #region 费用科目
        double fyzjeHj = 0;
        tempStr = "";
        DataSet tempFykm = server.GetDataSet("select ms,mxGuid,je,se,(select yskmCode from bill_yskm where yskmCode=fykm) as kmCode,(select yskmMc from bill_yskm where yskmCode=fykm) as kmName from bill_ybbxmxb_fykm where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'");
        DataSet tempCbzx = server.GetDataSet("select * from bill_cbzx");

        for (int i = 0; i <= tempFykm.Tables[0].Rows.Count - 1; i++)
        {
            //分摊信息
            //DataSet tempFykmFt = server.GetDataSet("select * from bill_ybbxmxb_fykm_ft where kmmxGuid='" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "' and isnull(status,'1')<>'2'");
            //int tempRowCount = 1;
            //int tempRowCount2 = 1;
            //if (tempFykmFt.Tables[0].Rows.Count != 0)
            //{
            //    tempRowCount = tempFykmFt.Tables[0].Rows.Count + 3;
            //    tempRowCount2 = tempFykmFt.Tables[0].Rows.Count + 2;
            //}

            tempStr += "<tr>";
            //tempStr += "<td rowspan=\"" + tempRowCount.ToString() + "\" style=\"text-align: center\" class=\"tableBg2\">";
            tempStr += "<td colspan=2 style=\"text-align: center\" class=\"tableBg2\">";
            tempStr += "<input type=checkbox class=fykmMx value=\"" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "\" /></td>";
            tempStr += "<td colspan=\"9\" class=\"tableBg2\" style=\"text-align: left\">";
            string tempFykmString = "【" + tempFykm.Tables[0].Rows[i]["kmCode"].ToString().Trim() + "】" + tempFykm.Tables[0].Rows[i]["kmName"].ToString().Trim() + "";

            tempStr += "<input class=\"noneBorder\" type=textbox value=\"【" + tempFykm.Tables[0].Rows[i]["kmCode"].ToString().Trim() + "】" + tempFykm.Tables[0].Rows[i]["kmName"].ToString().Trim() + "\" readonly=\"readonly\" />";

            if (Page.Request.QueryString["type"].ToString().Trim() == "look")
            {
                tempStr += "<span>金额:</span>";
                tempStr += "<input style=\"text-align:right;\" type=textbox class=\"fykm\" readonly=\"readonly\" id=\"txtFykm" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "\" value=\"" + double.Parse(tempFykm.Tables[0].Rows[i]["je"].ToString().Trim()).ToString("0.00") + "\" />";
                tempStr += "<span>税额:</span>";

                tempStr += "<input style=\"text-align:right;\" type=textbox class=\"fyse\" readonly=\"readonly\" id=\"txtHjse" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "\" value=\"" + tempFykm.Tables[0].Rows[i]["se"].ToString().Trim() + "\" />";
            }
            else
            {
                tempStr += "<span>金额:</span>";
                tempStr += "<input style=\"text-align:right;\" type=textbox class=\"fykm\" onblur=\"onInputChange();\" id=\"txtFykm" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "\" value=\"" + double.Parse(tempFykm.Tables[0].Rows[i]["je"].ToString().Trim()).ToString("0.00") + "\" />";
                tempStr += "<span>税额:</span>";
                tempStr += "<input style=\"text-align:right;\" type=textbox class=\"fyse\" onblur=\"onSeChange();\" id=\"txtHjse" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "\" value=\"" + tempFykm.Tables[0].Rows[i]["se"].ToString().Trim() + "\" />";
            }
            fyzjeHj += double.Parse(tempFykm.Tables[0].Rows[i]["je"].ToString().Trim());
            //费用预算信息
            tempStr += tempFykm.Tables[0].Rows[i]["ms"].ToString().Trim();

            //部门金额
            string tempSSSS = "</br></br><a href=# onclick=\"SetDept('" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "');\">【使用单位设置】</a>";
            DataSet dtt = server.GetDataSet("select mxGuid,(select '['+deptcode+']'+deptname from bill_departments where bill_departments.deptcode=bill_ybbxmxb_fykm_dept.deptCode) as dept,je from bill_ybbxmxb_fykm_dept where kmmxGuid='" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "'");
            for (int hh = 0; hh <= dtt.Tables[0].Rows.Count - 1; hh++)
            {
                //tempSSSS += dtt.Tables[0].Rows[hh]["dept"].ToString().Trim() + "：" + double.Parse(dtt.Tables[0].Rows[hh]["je"].ToString().Trim()).ToString("0.00") + "&nbsp;&nbsp;";
                tempSSSS += dtt.Tables[0].Rows[hh]["dept"].ToString().Trim() + "：<input type=text style=\"width:100px;text-align:right;\" onblur=\"onInputChangeMoneyDept('" + dtt.Tables[0].Rows[hh]["mxGuid"].ToString().Trim() + "');\" id=\"txt" + dtt.Tables[0].Rows[hh]["mxGuid"].ToString().Trim() + "\" value=\"" + double.Parse(dtt.Tables[0].Rows[hh]["je"].ToString().Trim()).ToString("0.00") + "\" />";
            }
            tempStr += tempSSSS;

            //项目金额
            tempSSSS = "</br></br><a href=# onclick=\"SetYskm('" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "');\">【科目项目设置】</a>";
            dtt = server.GetDataSet("select mxGuid,(select '['+xmcode+']'+xmname from bill_xm where bill_xm.xmcode=bill_ybbxmxb_hsxm.xmcode) as xm,je from bill_ybbxmxb_hsxm where kmmxGuid='" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "'");
            for (int hh = 0; hh <= dtt.Tables[0].Rows.Count - 1; hh++)
            {
                //tempSSSS += dtt.Tables[0].Rows[hh]["xm"].ToString().Trim() + "：" + double.Parse(dtt.Tables[0].Rows[hh]["je"].ToString().Trim()).ToString("0.00") + "&nbsp;&nbsp;";
                tempSSSS += dtt.Tables[0].Rows[hh]["xm"].ToString().Trim() + "：<input type=text style=\"width:100px;text-align:right;\" onblur=\"onInputChangeMoneyXm('" + dtt.Tables[0].Rows[hh]["mxGuid"].ToString().Trim() + "');\" id=\"txt" + dtt.Tables[0].Rows[hh]["mxGuid"].ToString().Trim() + "\" value=\"" + double.Parse(dtt.Tables[0].Rows[hh]["je"].ToString().Trim()).ToString("0.00") + "\" />";
            }
            tempStr += tempSSSS;

            tempStr += "</td>";



            tempStr += "</tr>";
            //tempStr += "<tr>";
            //tempStr += "<td colspan=\"2\" rowspan=\"" + tempRowCount2.ToString() + "\" class=\"tableBg2\" style=\"width: 200px\">";
            //tempStr += "经费摊销</td>";
            //tempStr += "<td style=\"text-align: right;\" colspan=\"6\" class=\"tableBg2\">";
            //tempStr += "<input type=button class=\"baseButton\" value=\"增加分摊\" onclick=\"AddFT('" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "');\" />&nbsp;";
            //tempStr += "<input type=button class=\"baseButton\" value=\"删除分摊\" onclick=\"DeleteFT('" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "');\" />&nbsp;</td>";
            //tempStr += "</tr><tr>";
            //tempStr += "<td class=\"tableBg2\" style=\"text-align: center;\">选择</td>";
            //tempStr += "<td colspan=\"2\" class=\"tableBg2\" style=\"width: 200px\">成本中心</td>";
            //tempStr += "<td class=\"tableBg2\" colspan=\"3\">摊销金额</td>";
            //tempStr += "</tr>";
            //tempStr += "<tr>";

            //for (int j = 0; j <= tempFykmFt.Tables[0].Rows.Count - 1; j++)
            //{
            //    tempStr += "<td style=\"text-align: center; height: 25px;\">";
            //    tempStr += "<input type=\"checkbox\" id=\"Checkbox2\" class=\"" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "\" value=\"" + tempFykmFt.Tables[0].Rows[j]["ftmxGuid"].ToString().Trim() + "\" /></td>";
            //    tempStr += "<td colspan=\"2\" style=\"width: 200px; height: 25px;text-align:center;\">";

            //    if (type == "look")
            //    {
            //        tempStr += server.GetCellValue("select zxName from bill_cbzx where zxCode='" + tempFykmFt.Tables[0].Rows[j]["cbzx"].ToString().Trim() + "'");
            //    }
            //    else
            //    {
            //        tempStr += "<select id=\"drp" + tempFykmFt.Tables[0].Rows[j]["ftmxGuid"].ToString().Trim() + "\" class=\"select" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "\" onchange=\"changeSelect(this,'" + tempFykmFt.Tables[0].Rows[j]["ftmxGuid"].ToString().Trim() + "');\">";

            //        for (int k = 0; k <= tempCbzx.Tables[0].Rows.Count - 1; k++)
            //        {
            //            if (tempFykmFt.Tables[0].Rows[j]["cbzx"].ToString().Trim() == tempCbzx.Tables[0].Rows[k]["zxCode"].ToString().Trim())
            //            {
            //                tempStr += "<option selected=\"selected\" value=\"" + tempCbzx.Tables[0].Rows[k]["zxCode"].ToString().Trim() + "\">" + tempCbzx.Tables[0].Rows[k]["zxName"].ToString().Trim() + "</option>";
            //            }
            //            else
            //            {
            //                tempStr += "<option value=\"" + tempCbzx.Tables[0].Rows[k]["zxCode"].ToString().Trim() + "\">" + tempCbzx.Tables[0].Rows[k]["zxName"].ToString().Trim() + "</option>";
            //            }
            //        }

            //        tempStr += "</select>";
            //    }
            //    tempStr += "</td>";
            //    tempStr += "<td style=\"height: 25px;text-align:center;\" colspan=\"3\">";
            //    tempStr += "<input type=text id=\"txt" + tempFykmFt.Tables[0].Rows[j]["ftmxGuid"].ToString().Trim() + "\" class=\"txt" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "\" value=\"" + double.Parse(tempFykmFt.Tables[0].Rows[j]["je"].ToString()).ToString("0.00") + "\" onblur=\"onInputChange(this,'" + tempFykm.Tables[0].Rows[i]["mxGuid"].ToString().Trim() + "');\" /></td>";
            //    fyzjeHj += double.Parse(tempFykmFt.Tables[0].Rows[j]["je"].ToString());
            //    tempStr += "</tr>";
            //}
        }
        this.divFykm.InnerHtml = tempStr;
        this.txtHjjeXx.Value = fyzjeHj.ToString("0.00");
        #endregion

        #region 费用抵扣：借款单已给付 未退还 且明细未抵扣的，且借款人是当前保险人的：故此，修改时 不能修改报销人
        tempStr = "";
        tempStr=(new webBillLibrary.bxgl()).getFysk_string(this.lblBillCode.Text.ToString().Trim());
        this.divJkdk.InnerHtml = tempStr;
        #endregion



        DataSet temp1 = server.GetDataSet("select isnull(sum(je),0) from bill_ybbxmxb_fykm where billCode='" + this.lblBillCode.Text.ToString().Trim() + "'");//总金额
        DataSet temp2 = server.GetDataSet("select isnull(sum(je),0) from bill_fysq_mxb where mxGuid in (select jkmxCode from bill_ybbxmxb_fydk where billCode='" + this.lblBillCode.Text.ToString().Trim() + "')");

        double je = double.Parse(temp1.Tables[0].Rows[0][0].ToString().Trim());
        double dkje = double.Parse(temp2.Tables[0].Rows[0][0].ToString().Trim());
        double cha = je - dkje;
        if (cha > 0)
        {
            //this.lblYtje.Text = Math.Abs(cha).ToString("0.00");
            //this.lblYbje.Text = "0.00";
            this.txtytje.Value = Math.Abs(cha).ToString("0.00");
            this.txtYbje.Value = "0.00";
        }
        else
        {
            //this.lblYbje.Text = Math.Abs(cha).ToString("0.00");
            //this.lblYtje.Text = "0.00";
            this.txtytje.Value = "0.00";
            this.txtYbje.Value = Math.Abs(cha).ToString("0.00");
        }


        #region 费用申请单
        (new webBillLibrary.bxgl()).getFysq_Ybbx(lblBillCode.Text.ToString().Trim(),this.myGrid);
        #endregion
    }

    /// <summary>
    /// 刷新脚本数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefresh_Click1(object sender, EventArgs e)
    {
        this.bindJsInfo();
        //刷新页面数据
        //bindShow();
    }
    [Ajax.AjaxMethod(HttpSessionStateRequirement.ReadWrite)]
    public bool ChangeMoneyDept(string mxGuid, string money)
    {
        if (server.ExecuteNonQuery("update bill_ybbxmxb_fykm_dept set je=" + double.Parse(money).ToString().Trim() + " where mxGuid='" + mxGuid + "'") == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    [Ajax.AjaxMethod(HttpSessionStateRequirement.ReadWrite)]
    public bool ChangeMoneyXm(string mxGuid, string money)
    {
        if (server.ExecuteNonQuery("update bill_ybbxmxb_hsxm set je=" + double.Parse(money).ToString().Trim() + " where mxGuid='" + mxGuid + "'") == -1)
        {
            return false;
        }
        else
        {
            return true;
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

                string sql = "insert into bill_ybbxmxb_bxdj(billCode,djUrl,djName,djGuid,djStatus) values('" + this.lblBillCode.Text.ToString().Trim() + "','" + guid + "." + extName + "','" + cFileName + "','" + guid + "','0')";

                if (server.ExecuteNonQuery(sql) == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('上传失败！');", true);
                    return;
                }
                else
                {
                    this.btnRefresh_Click1(sender, e);
                }
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('数据库记录失败！');", true);
            return;
        }
    }

    /// <summary>
    /// 删除费用科目
    /// </summary>
    /// <param name="fykm"></param>
    /// <returns></returns>
    [Ajax.AjaxMethod()]
    public bool DeleteFykmMx(string fykm, string billcode)
    {
        return (new webBillLibrary.bxgl()).DeleteFykmMx(fykm, billcode);
    }

    /// <summary>
    /// 删除分摊明细
    /// </summary>
    /// <param name="ftmxGuids"></param>
    /// <returns></returns>
    [Ajax.AjaxMethod()]
    public bool DeleteFyFTMx(string ftmxGuids, string billcode)
    {
        return (new webBillLibrary.bxgl()).DeleteFyFTMx(ftmxGuids, billcode);
    }

    /// <summary>
    /// 增加分摊
    /// </summary>
    /// <param name="billCode"></param>
    /// <param name="kmmxGuid"></param>
    /// <returns></returns>
    [Ajax.AjaxMethod()]
    public string AddFtMxb(string billCode, string kmmxGuid)
    {
        return (new webBillLibrary.bxgl()).AddFtMxb(billCode, kmmxGuid);
    }

    /// <summary>
    /// 更新分摊金额
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    [Ajax.AjaxMethod()]
    public bool UpdateFyftMxb(string[] list, string billcode)
    {
        return (new webBillLibrary.bxgl()).UpdateFyftMxb(list, billcode);
    }

    /// <summary>
    /// 更新税额
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    [Ajax.AjaxMethod()]
    public bool UpdateFySe(string[] list, string billcode)
    {
        return (new webBillLibrary.bxgl()).UpdateFySe(list, billcode);
    }

    [Ajax.AjaxMethod()]
    public string UpdateFtmxCbzx(string ftmxGuid, string val)
    {
        return (new webBillLibrary.bxgl()).UpdateFtmxCbzx(ftmxGuid, val);
    }

    /// <summary>
    /// 删除修改为2状态
    /// </summary>
    /// <param name="jkdkGuid"></param>
    /// <returns></returns>
    [Ajax.AjaxMethod()]
    public bool DeleteJkmxInfo(string jkdkGuid)
    {
        return (new webBillLibrary.bxgl()).DeleteJkmxInfo(jkdkGuid);
    }

    [Ajax.AjaxMethod()]
    public string CaluateYtYb(string billCode)
    {
        return (new webBillLibrary.bxgl()).CaluateYtYb(billCode);
    }
    [Ajax.AjaxMethod()]
    public string ToDoubleFormate(double d)
    {
        return d.ToString("0.00");
    }
    protected void btnDelteFysq_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        string billCode = this.lblBillCode.Text.ToString().Trim();
        int count = 0;
        bool isBegin = false;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                string sqlCode =  this.myGrid.Items[i].Cells[1].Text.ToString().Trim() ;

                list.Add((new webBillLibrary.bxgl()).DeleteFysq(billCode, sqlCode));

                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择要删除的单据！');", true);
        }
        else
        {
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {
                this.bindJsInfo();
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
            }
        }
        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个申请单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择查看的申请单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openLookShgc('../fysq/cgspDetail.aspx?type=look&cgbh=" + billGuid + "');", true);
            //Response.Redirect("ystbDetail.aspx?gcbh=" + billGuid );
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        
    }
}