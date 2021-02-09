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
using System.IO;

public partial class webBill_fysq_lscgDetail : System.Web.UI.Page
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
            txtZynr.Focus();
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
            //this.txtCgrq.Attributes.Add("onfocus","javascript:setday(this);");
            this.txtCgrq.Attributes.Add("onfocus", "edit()");
            this.ddl_cglb.SelectedIndex = 0;
            //查询所在部门，是二级部门则显示，不是则另显示
            //string strDept = server.GetCellValue("select  isnull(sjdeptcode,'') from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "') ") ;
            //是
            if (isTopDept("y", Session["userCode"].ToString().Trim()))
            {
                string dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                this.lblDept.Text = dept;
                lblxsDept.Text = dept;
            }
            else
            {
                //所在部门
                string Dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                //上级部门
                string sjDept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
                this.lblDept.Text = Dept;
                lblxsDept.Text = sjDept + "-" + Dept;
            }
            //this.lblDept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            DateTime dt = System.DateTime.Now;
            this.txtCgrq.Text = System.DateTime.Now.ToString("yyyy-MM-dd"); //dt.ToShortDateString();

            this.lblCbr.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'");

            this.CreateLscgCode();
            this.btn_print.Visible = false;
        }
        else
        {
            if (type == "edit")
            {
                this.txtCgrq.Attributes.Add("onfocus", "edit()");
            }
            DataSet temp = server.GetDataSet("select cgbh,sj, cgdept,cglb,zynr,sm,(select '['+usercode+']'+username from bill_users where usercode=cbr) as cbr,spyj01,spyj02,spyj03,spyj04,spyj05,spyj06,spyj07,spyj08,yjfy,fjName,fjUrl from bill_lscg where cgbh='" + Page.Request.QueryString["cgbh"].ToString().Trim() + "'");
            this.lblCgbh.Text = temp.Tables[0].Rows[0]["cgbh"].ToString().Trim();
            DateTime dt = DateTime.Parse(temp.Tables[0].Rows[0]["sj"].ToString().Trim());
            this.txtCgrq.Text = System.DateTime.Now.ToString("yyyy-MM-dd"); //dt.ToShortDateString();
            //this.lblDept.Text = temp.Tables[0].Rows[0]["cgdept"].ToString().Trim();
            string strdsDept = temp.Tables[0].Rows[0]["cgdept"].ToString().Trim();
            //string strDept = server.GetCellValue("select  isnull(sjdeptcode,'') from bill_departments where deptcode='" + strdsDept + "' ");
            //是
            if (isTopDept("n", strdsDept))
            {
                string dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + strdsDept + "'");
                this.lblDept.Text = dept;
                lblxsDept.Text = dept;
            }
            else
            {
                //所在部门
                string Dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + strdsDept + "'");
                //上级部门
                string sjDept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode='" + strdsDept + "')");
                this.lblDept.Text = Dept;
                lblxsDept.Text = sjDept + "-" + Dept;
            }
            this.lblCbr.Text = temp.Tables[0].Rows[0]["cbr"].ToString().Trim();
            this.txtZynr.Text = temp.Tables[0].Rows[0]["zynr"].ToString().Trim();
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
            this.txtYjfy.Text = temp.Tables[0].Rows[0]["yjfy"].ToString().Trim();

            //2015-04-07 附件处理 edit zyl 
            string fjName = Convert.ToString(temp.Tables[0].Rows[0]["fjName"]).Trim();
            string fjUrl = Convert.ToString(temp.Tables[0].Rows[0]["fjUrl"]).Trim();
            if (!string.IsNullOrEmpty(fjName) && !string.IsNullOrEmpty(fjUrl))
            {

                Lafilename.Text = "我的附件";
                upLoadFiles.Visible = false;
                btn_sc.Text = "修改附件";
                // Literal1.Text = "<a href='"+arrTemp[1]+"' target='_blank' >下载</a> ";
                //Literal1.Text = "<a href='../../Uploads/ybbx/" + System.IO.Path.GetFileName(arrTemp[1]) + @"' target='_blank'  >下载</a> ";
                Literal1.Text = "<a href='../../AFrame/download.aspx?filename=" + fjName + "&filepath=" + fjUrl + "' target='_blank'>下载</a>";
            }
            else
            {
                //如果没有附件的话
                btn_sc.Text = "上 传";
                Lafilename.Text = "";
                upLoadFiles.Visible = true;
                hiddFileDz.Value = "";
            }
            
            
            if (type == "look" || type == "audit")
            {
                this.btn_bc.Visible = false;
                this.txtCgrq.Enabled = false;
                this.ddl_cglb.Enabled = false;
                this.txtZynr.Enabled = false;
                this.txtSm.Enabled = false;
                this.txtYjfy.Enabled = false;
            }

        }
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
        return true;
    }

    protected void btn_bc_Click(object sender, EventArgs e)
    {
        string str_stepid = "-1";
        string str_billuser = Session["userCode"].ToString().Trim();
        string str_billdate = this.txtCgrq.Text.ToString().Trim();
        string str_billdept = server.GetCellValue("select userdept from bill_users where usercode='" + str_billuser + "'");
        string bm = this.lblDept.Text.ToString().Trim();
        bm = bm.Substring(1, bm.IndexOf("]") - 1);

        string fjName = Lafilename.Text.Trim();
        string fjUrl = hiddFileDz.Value.Trim();




        string type = Page.Request.QueryString["type"].ToString().Trim();
        List<string> list = new List<string>();
        if (type == "add")
        {
            DataSet temp = server.GetDataSet("select cgbh from bill_lscg where cgbh='" + this.lblCgbh.Text.ToString().Trim() + "'");
            if (temp.Tables[0].Rows.Count != 0)
            {
                this.CreateLscgCode();
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的报告申请申请已存在,系统已重新生成,请保存！');", true);
                this.Button1.Visible = true;
                return;
            }

            list.Add("insert into bill_lscg(cgbh,sj,cgdept,cglb,zynr,sm,cbr,spyj01,spyj02,spyj03,spyj04,spyj05,spyj06,spyj07,spyj08,yjfy,fjName,fjUrl) values('" + this.lblCgbh.Text.ToString().Trim() + "','" + str_billdate + "','" + bm + "','" + this.ddl_cglb.SelectedItem.Value + "','" + this.txtZynr.Text.ToString().Trim() + "','" + this.txtSm.Text.ToString().Trim() + "','" + Session["userCode"].ToString().Trim() + "','" + this.txtbmzgyj.Text.ToString().Trim() + "','" + this.txtbmjlyj.Text.ToString().Trim() + "','" + this.txtzhglbyj.Text.ToString().Trim() + "','" + this.txtcwbmyj.Text.ToString().Trim() + "','" + this.txtjsbmyj.Text.ToString().Trim() + "','" + this.txtfgldsp.Text.ToString().Trim() + "','" + this.txtzkjssp.Text.ToString().Trim() + "','" + this.txtzjlsp.Text.ToString().Trim() + "','" + this.txtYjfy.Text.ToString().Trim() + "','" + fjName + "','" + fjUrl + "') ");
            //申明主表添加
            list.Add("insert into bill_main(looptimes,billType,billcode,billname,flowid,stepid,billuser,billdate,billdept,billje) values(1,'1','" + this.lblCgbh.Text.ToString().Trim() + "','','lscg','" + str_stepid + "','" + str_billuser + "','" + str_billdate + "','" + str_billdept + "','" + this.txtYjfy.Text.ToString().Trim() + "')");

        }
        else //编辑
        {
       
            //修改单据时
            list.Add("update bill_lscg set  sj='" + str_billdate + "',cgdept='" + bm + "',cglb='" + this.ddl_cglb.SelectedItem.Value + "',zynr='" + txtZynr.Text.ToString().Trim() + "',sm='" + this.txtSm.Text.ToString().Trim() + "',cbr='" + Session["userCode"].ToString().Trim() + "',spyj01='" + this.txtbmzgyj.Text.ToString().Trim() + "',spyj02='" + this.txtbmjlyj.Text.ToString().Trim() + "',spyj03='" + this.txtzhglbyj.Text.ToString().Trim() + "',spyj04='" + this.txtcwbmyj.Text.ToString().Trim() + "',spyj05='" + this.txtjsbmyj.Text.ToString().Trim() + "',spyj06='" + this.txtfgldsp.Text.ToString().Trim() + "',spyj07='" + this.txtzkjssp.Text.ToString().Trim() + "',spyj08='" + this.txtzjlsp.Text.ToString().Trim() + "',yjfy='" + this.txtYjfy.Text.ToString().Trim() + "' , fjName='" + fjName + "' , fjUrl='" + fjUrl + "' WHERE cgbh='" + Request.QueryString["cgbh"].ToString() + "'");
            list.Add("update bill_main set  billname='' , billuser='" + str_billuser + "',billdate='" + str_billdate + "',billdept='" + str_billdept + "',billje='" + this.txtYjfy.Text.ToString().Trim() + "',stepid='" + str_stepid + "' where flowid='lscg' and  billcode='" + Request.QueryString["cgbh"].ToString() + "'");

        }

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
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"\";self.close();", true);
    }

    public void CreateLscgCode()
    {
        string lscgCode = (new billCoding()).getLscgCode();
        if (lscgCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
            this.btn_bc.Visible = false;
        }
        else
        {
            this.lblCgbh.Text = lscgCode;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.CreateLscgCode();
    }

    #region 打印
    protected void btn_print_Click(object sender, EventArgs e)
    {
        try
        {
            string stgrcgbh = Page.Request.QueryString["cgbh"].ToString().Trim();
            Session["printsql"] = "select cgbh,Convert(Varchar(12),sj,23) as sj,(select '['+deptcode+']'+deptname from bill_departments where deptcode=cgdept) as cgdept,cglb,zynr,sm,(select '['+usercode+']'+username from bill_users where usercode=cbr) as cbr,yjfy from bill_lscg where cgbh='" + Page.Request.QueryString["cgbh"].ToString().Trim() + "'";

            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('cgPrint.aspx?');", true);

        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请确定单据信息！');", true);
        }
    }
    #endregion

    protected void btnScdj_Click(object sender, EventArgs e)
    {
        if (upLoadFiles.Visible == true)
        {
            string script;
            if (upLoadFiles.PostedFile.FileName == "")
            {
                laFilexx.Text = "请选择文件";
                return;
            }
            else
            {
                try
                {
                    string filePath = upLoadFiles.PostedFile.FileName;

                    string Name = this.upLoadFiles.PostedFile.FileName;
                    string name = System.IO.Path.GetFileName(Name).Split('.')[0];
                    string exname = System.IO.Path.GetExtension(Name);
                    if (isOK(exname))
                    {
                        string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                        string fileSn = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        ////转换成绝对地址,
                        string serverpath = Server.MapPath(@"~\Uploads\lscg\") + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////转换成与相对地址,相对地址为将来访问图片提供
                        string relativepath = @"~\Uploads\lscg\" + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////绝对地址用来将上传文件夹保存到服务器的具体路下。
                        if (!Directory.Exists(Server.MapPath(@"~\Uploads\lscg\")))
                        {
                            Directory.CreateDirectory(Server.MapPath(@"~\Uploads\lscg\"));
                        }
                        upLoadFiles.PostedFile.SaveAs(serverpath);
                        ////把相对路径的地址保存到页面hdImageUrl的value值上以供保存值时使用。
                        hiddFileDz.Value = relativepath;
                        Lafilename.Text = filename;
                        laFilexx.Text = "上传成功";
                        btn_sc.Text = "修改附件";
                        upLoadFiles.Visible = false;
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
            laFilexx.Text = "";
            upLoadFiles.Visible = true;
            Lafilename.Text = "";
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
}
