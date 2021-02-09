using Bll;
using Bll.UserProperty;
using Dal.Bills;
using Dal.FeiYong_DZ;
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

public partial class webBill_bxgl_zfzxsqd_dzDetails : System.Web.UI.Page
{
    string strctrl = "";//add 添加btnScdj_Click
    string billcode = "";
    string strflg = "k";//n区域内转校转费
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        object objctrl = Request["ctrl"];
        if (objctrl != null)
        {
            strctrl = objctrl.ToString();
        }
        object objbillcode = Request["billcode"];
        if (objbillcode != null)
        {
            billcode = objbillcode.ToString();
        }
        object objflg = Request["flg"];
        if (objflg != null)
        {
            strflg = objflg.ToString();
        }
        if (!string.IsNullOrEmpty(strflg)&&strflg=="n")
        {
            lbl_title.Text = "区域内转费转校申请单";
            lbl_zcfx.Text = "转出分校";
            lbl_zrfx.Text = "转入分校";
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
            if (strctrl.Equals("add"))
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
            else if (strctrl.Equals("edit") || strctrl.Equals("view") || strctrl.Equals("audit"))
            {
                Bill_Main main = new MainDal().GetMainByCode(billcode);
                this.txtdate.Text = ((DateTime)main.BillDate).ToString("yyyy-MM-dd");

                this.txtdept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + main.BillDept + "'");

                this.txtSm.Text = main.BillName2;

                this.txtsqr.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + main.BillUser + "'");
                bill_zfzxsqd_dz mxmodel = new bill_zfzxsqd_dz();
                bill_zfzxsqd_dzDal mxdal = new bill_zfzxsqd_dzDal();

                mxmodel = mxdal.GetmodelByCode(billcode);
                if (mxmodel != null)
                {
                    txt_zcqyfx.Text = mxmodel.zcfx;
                    txt_zrfx.Text = mxmodel.zrfx;
                    txt_xyxm.Text = mxmodel.xyxm;
                    txt_nianji.Text = mxmodel.nianji;
                    txt_yxyfdfy.Text = mxmodel.yxyfdfy.ToString();
                    txt_ybmkc.Text = mxmodel.ybmkc;
                    txt_ykcxsyh.Text = mxmodel.ykcxsyh;
                    txt_yxfks.Text = mxmodel.yxfks.ToString();
                    txt_dyksdj.Text = mxmodel.dyksdj.ToString();
                    txt_yxffy.Text = mxmodel.yxffy.ToString();
                    txt_ykqtfy.Text = mxmodel.ykqtfy.ToString();
                    txt_syje.Text = mxmodel.syjexx.ToString();
                    txt_syjedx.Text = mxmodel.syjedx;
                    txtSm.Text = mxmodel.zfyy;
                    txt_xbxs.Text = mxmodel.xbxs;
                    txt_xbjje.Text = mxmodel.xbjje.ToString();
                    txt_xbjjedx.Text = mxmodel.xbjjedx;


                }
                if (strctrl.Equals("view") || strctrl.Equals("audit"))
                {
                    btn_save.Visible = false;
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
    protected void btn_save_Click(object sender, EventArgs e)
    {

        string strflowid = "";
        if (strflg == "n")
        {
            strflowid = "nzfzxsqd";
        }
        else
        {
            strflowid = "zfzxsqd";
        }
        string strDate = this.txtdate.Text.Trim();

        if (string.IsNullOrEmpty(strDate))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请日期不能为空');", true);
            return;
        }

        string strsqr = this.txtsqr.Text.Trim();
        if (string.IsNullOrEmpty(strsqr))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请人不能为空');", true);
            return;
        }
        else if (strsqr.IndexOf("]") == -1 || strsqr.IndexOf("[") == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请人格式不正确，应该是[编号]人员，请根据系统的提示进行选择');", true);
            return;
        }
        string strdept = this.txtdept.Text.Trim();
        if (string.IsNullOrEmpty(strdept))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请部门不能为空');", true);
            return;
        }
        else if (strdept.IndexOf("]") == -1 || strdept.IndexOf("[") == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请部门格式不正确，应该是[编号]名称，请根据系统的提示进行选择');", true);
            return;
        }


        string dept = strdept.Substring(1, strdept.IndexOf("]") - 1);// CutVal(strdept);


        List<string> listsql = new List<string>();
        SysManager sysMgr = new SysManager();

        string billname = sysMgr.GetYbbxBillName(strflowid, DateTime.Now.ToString("yyyMMdd"), 1); ;
        if (!string.IsNullOrEmpty(billcode))
        {
            //List<string> listsqldel = new List<string>();
            Bill_Main main = new MainDal().GetMainByCode(billcode);
            if (main != null)
            {
                billname = main.BillName;
            }


            string sqltempdel = "delete bill_main where billcode='" + billcode + "'";
            listsql.Add(sqltempdel);
            string sqldelmx = " delete bill_zfzxsqd_dz where billcode='" + billcode + "'";
            listsql.Add(sqldelmx);

            //new sqlHelper.sqlHelper().ExecuteNonQuerys(listsqldel.ToArray());

        }
        else
        {
            billcode = new GuidHelper().getNewGuid();
        }

        //添加附件
        string fjname = hidfilnename.Value;
        string fjurl = hiddFileDz.Value;
        var fujian = "";
        if (!string.IsNullOrEmpty(fjname) && !string.IsNullOrEmpty(fjurl))
        {
            fujian = Server.UrlDecode(fjname + "|" + fjurl);
        }

       
        string sqltemp = "insert into bill_main (billcode,billname,flowid,stepid,billuser,billdate,billDept,billJe,loopTimes,billtype,billName2,note1,note2,note3) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')";

        string user = strsqr.Substring(1, strsqr.IndexOf("]") - 1);// CutVal(strsqr);

        sqltemp = string.Format(sqltemp, billcode, billname, strflowid, "-1", user, txtdate.Text.Trim(), dept, "0", "1", "1", this.txtSm.Text.Trim(), "", "", fujian);
        listsql.Add(sqltemp);
        string strmxsql = @"insert into bill_zfzxsqd_dz
                            (billCode,sqrq,zcfx,zrfx,xyxm,nianji,yxyfdfy
                            ,ybmkc,ykcxsyh,yxfks,dyksdj,yxffy,ykqtfy,syjexx,syjedx,zfyy,xbxs,xbjje,xbjjedx)
                            values('" + billcode + "','" + txtdate.Text + "','" + txt_zcqyfx.Text + "','" + txt_zrfx.Text + "','" + txt_xyxm.Text + "','" + txt_nianji.Text + "','" + txt_yxyfdfy.Text + "','" + txt_ybmkc.Text + "','" + txt_ykcxsyh.Text + "','" + txt_yxfks.Text + "','" + txt_dyksdj.Text + "','" + txt_yxffy.Text + "','" + txt_ykqtfy.Text + "','" + txt_syje.Text + "','" + txt_syjedx.Text + "','" + txtSm.Text + "','" + txt_xbxs.Text + "','" + txt_xbjje.Text + "','" + txt_xbjjedx.Text + "')";
        listsql.Add(strmxsql);
        try
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