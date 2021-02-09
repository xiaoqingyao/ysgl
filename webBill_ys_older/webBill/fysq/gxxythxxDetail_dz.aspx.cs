using Bll;
using Bll.UserProperty;
using Dal.Bills;
using Dal.FeiYong_DZ;
using Dal.SysDictionary;
using Dal.UserProperty;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_fysq_gxxythxxDetail_dz : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    string strCtrl = "";
    string strbillcode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }

        if (Request["billcode"] != null && Request["billcode"].ToString() != "")
        {
            strbillcode = Request["billcode"].ToString();
        }
        if (Request["ctrl"] != null && Request["ctrl"].ToString() != "")
        {
            strCtrl = Request["ctrl"].ToString();
        }
        Response.Cache.SetSlidingExpiration(true);
        Response.Cache.SetNoStore();
        ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        if (!IsPostBack)
        {
            if (Page.Request.QueryString["ctrl"].ToString().Trim() == "audit")
            {
                tr_shyj.Visible = true;
                tr_shyj_history.Visible = true;
                //显示历史驳回意见
                DataTable dtHisMind = server.GetDataTable("select * from bill_ReturnHistory where billcode='" + Request.QueryString["billcode"] + "' order by dt desc", null);
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
                tr_shyj.Visible = false;
                tr_shyj.Visible = btn_ok.Visible = btn_cancel.Visible = false;
            }

            if (strCtrl.Equals("add"))
            {

                string usercode = Session["userCode"].ToString();
                UserMessage um = new UserMessage(usercode);
                txtsqr.Text = "[" + um.Users.UserCode + "]" + um.Users.UserName;
                string strdeptjc = new ConfigBLL().GetValueByKey("deptjc");//是否预算到末级

                if (!string.IsNullOrEmpty(strdeptjc) && strdeptjc == "Y")
                {
                    DepartmentDal depDal = new DepartmentDal();
                    string strdept = depDal.GetDeptByUser(usercode);
                    if (!string.IsNullOrEmpty(strdept))
                    {
                        txtdept.Text = strdept;
                    }
                }
                else
                {
                    Bill_Departments dept = um.GetRootDept();
                    txtdept.Text = "[" + dept.DeptCode + "]" + dept.DeptName;
                }

                this.txtdate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            }
            else
            {
                if (!strCtrl.Equals("edit"))
                {
                    btn_save.Visible = false;
                }
                Bill_Main main = new MainDal().GetMainByCode(strbillcode);
                this.txtdate.Text = ((DateTime)main.BillDate).ToString("yyyy-MM-dd");

                this.txtdept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + main.BillDept + "'");

                this.txtsqr.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + main.BillUser + "'");


                if (!string.IsNullOrEmpty(strbillcode))
                {

                    string strsql = @"select * from bill_gxxythxx_dz gx where billcode='" + strbillcode + "'";
                    DataTable dt = server.GetDataTable(strsql, null);

                    if (dt != null)
                    {
                        txt_fx.Text = dt.Rows[0]["fenxiao"].ToString();
                        txt_xyxm.Text = dt.Rows[0]["xyxm"].ToString();
                        txt_nj.Text = dt.Rows[0]["nianji"].ToString();
                        txt_bmkc.Text = dt.Rows[0]["bmkc"].ToString();
                        txt_ysf.Text = dt.Rows[0]["ysf"].ToString();
                        txt_xxyh.Text = dt.Rows[0]["xhyh"].ToString();
                        txt_youhui.Text = dt.Rows[0]["youhui"].ToString();
                        txt_zengsong1.Text = dt.Rows[0]["zengsong1"].ToString();
                        txt_zengsong2.Text = dt.Rows[0]["zengsong2"].ToString();
                        txt_beizhu.Text = dt.Rows[0]["beizhu"].ToString();
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败')", true);
                    }
                }
            }
            if (!string.IsNullOrEmpty(Request["billCode"]))
            {
                string fujian = server.GetCellValue("select top 1 note3 from bill_main where billcode='" + Request["billCode"] + "'");
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
                        Literal1.Text += "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>附件" + (i + 1) + "：</span><a href='../../AFrame/download.aspx?filename=" + Server.UrlEncode(arrname[i]) + "&filepath=" + Server.UrlEncode(arrfile[i]) + "' target='_blank'>" + arrname[i] + "下载;</a><a onclick='delfj(this);'>删除</a><span style='display:none'><input type='text' class='fujianurl' value='" + arrfile[i] + "'/><input type='text' class='fujianname' value='" + arrname[i] + "'/></span></div>";
                    }//
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
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_Click(object sender, EventArgs e)
    {
        List<string> listsql = new List<string>();
        SysManager sysMgr = new SysManager();
        string strdept = "";
        string strdate = "";
        string strflowid = "xyth";
        string usercode = "";
        string billname = "";
        string deptcode = "";
        if (!string.IsNullOrEmpty(strbillcode))
        {

            Bill_Main main = new MainDal().GetMainByCode(strbillcode);
            if (main != null)
            {
                billname = main.BillName;
                usercode = main.BillUser;
                deptcode = main.BillDept;
                strdate = main.BillDate.ToString();
                strdept = main.BillDept;
            }


            string sqltempdel = "delete bill_main where billcode='" + strbillcode + "'";
            listsql.Add(sqltempdel);
            string sqldelmx = " delete bill_gxxythxx_dz where billcode='" + strbillcode + "'";
            listsql.Add(sqldelmx);

            //new sqlHelper.sqlHelper().ExecuteNonQuerys(listsqldel.ToArray());

        }
        else
        {
            strbillcode = new GuidHelper().getNewGuid();
            billname = new DataDicDal().GetYbbxBillName(strflowid, DateTime.Now.ToString("yyyyMMdd"), 1);

            if (!string.IsNullOrEmpty(txtdept.Text))
            {
                strdept = txtdept.Text;
                deptcode = strdept.Substring(1, strdept.IndexOf("]") - 1);
            }
            if (!string.IsNullOrEmpty(txtsqr.Text))
            {
                usercode = txtsqr.Text;
                usercode = usercode.Substring(1, usercode.IndexOf("]") - 1);
            }


        }


        //添加附件
        string fjname = hidfilnename.Value;
        string fjurl = hiddFileDz.Value;
        var fujian = "";
        if (!string.IsNullOrEmpty(fjname) && !string.IsNullOrEmpty(fjurl))
        {
            fujian = Server.UrlDecode(fjname + "|" + fjurl);
        }
        string sqltemp = "insert into bill_main (billcode,billname,flowid,stepid,billuser,billdate,billDept,billJe,loopTimes,billtype,note3) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')";

        sqltemp = string.Format(sqltemp, strbillcode, billname, strflowid, "-1", usercode, strdate, deptcode, "0", "1", "1", fujian);
        listsql.Add(sqltemp);
        string strmxsql = @" insert into bill_gxxythxx_dz
                            (billCode,fenxiao,xyxm,nianji,bmkc,ysf,xhyh
                            ,youhui,zengsong1,zengsong2,beizhu)
                            values('" + strbillcode + "','" + txt_fx.Text + "','" + txt_xyxm.Text + "','" + txt_nj.Text + "','" + txt_bmkc.Text + "','" + txt_ysf.Text + "','" + txt_xxyh.Text + "','" + txt_youhui.Text + "','" + txt_zengsong1.Text + "','" + txt_zengsong2.Text + "','" + txt_beizhu.Text + "')";
        listsql.Add(strmxsql);
        try
        {
            if (listsql.Count > 0)
            {
                int introw = new sqlHelper.sqlHelper().ExecuteNonQuerysArray(listsql.ToList());
                if (introw > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');self.close();", true);

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);

                }
            }

        }
        catch (Exception)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
    }
    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users where userStatus='1' ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["username"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }

    protected void btnScdj_Click(object sender, EventArgs e)
    {
        //hidfilnename.Value = "";
        //hiddFileDz.Value = "";
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
                        string serverpath = Server.MapPath(@"~\Uploads\jfsq\") + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////转换成与相对地址,相对地址为将来访问图片提供
                        string relativepath = @"~\Uploads\jfsq\" + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////绝对地址用来将上传文件夹保存到服务器的具体路下。
                        if (!Directory.Exists(Server.MapPath(@"~\Uploads\jfsq\")))
                        {
                            Directory.CreateDirectory(Server.MapPath(@"~\Uploads\jfsq\"));
                        }
                        upLoadFiles.PostedFile.SaveAs(serverpath);
                        ////把相对路径的地址保存到页面hdImageUrl的value值上以供保存值时使用。
                        hiddFileDz.Value += relativepath + ";";
                        Lafilename.Text += "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>新附件" + filename + "：</span><a onclick='delfj(this);'>删除</a><span style='display:none'><input type='text' class='fujianurl' value='" + filename + "'/><input type='text' class='fujianname' value='" + relativepath + "'/></span></div>";//

                        hidfilnename.Value += filename + ";";
                        laFilexx.Text = "上传成功";
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
            upLoadFiles.Visible = true;

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