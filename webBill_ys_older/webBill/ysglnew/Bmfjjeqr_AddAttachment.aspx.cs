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
using Models;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

public partial class webBill_ysglnew_Bmfjjeqr_AddAttachment : System.Web.UI.Page
{
    string strnd = "";//年度
    string strdeptcode = "";//部门
    string strkmcode = "";//科目编号
    protected void Page_Load(object sender, EventArgs e)
    {
        object objnd = Request["nd"];
        if (objnd != null)
        {
            strnd = objnd.ToString();
        }
        object objdept = Request["deptcode"];
        if (objdept != null)
        {
            strdeptcode = objdept.ToString();
        }
        object objkmcode = Request["kmcode"];
        if (objkmcode != null)
        {
            strkmcode = objkmcode.ToString();
        }
        if (!IsPostBack)
        {
            binddata();
        }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void binddata()
    {
        if (strnd.Equals("") || strdeptcode.Equals("") || strkmcode.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('参数不完整。');", true);
            return;
        }
        IList<bill_ys_xmfjbm> lst = new Dal.newysgl.Bmfj().GetData(strnd, strkmcode, strdeptcode);
        if (lst == null || lst.Count <= 0)
        {
            return;
        }
        if (lst[0].Attachment != "")
        {
            Lafilename.Text = "我的附件";
            FileUpload1.Visible = false;
            btn_sc.Text = "修改附件";
            hiddFileDz.Value = lst[0].Attachment;
            Literal1.Text = "<a href='../Uploads/bmfjjeqr/" + System.IO.Path.GetFileName(hiddFileDz.Value) + @"' target='_blank'  >下载</a> ";
        }
        else
        {
            //如果没有附件的话
            btn_sc.Text = "上 传";
            Lafilename.Text = "";
            FileUpload1.Visible = true;
            hiddFileDz.Value = "";
        }
    }
    //上传按钮
    protected void btn_sc_Click(object sender, EventArgs e)
    {
        if (FileUpload1.Visible == true)
        {
            string script;
            if (FileUpload1.PostedFile.FileName == "")
            {
                laFilexx.Text = "请选择文件";
                return;
            }
            else
            {
                try
                {
                    string filePath = FileUpload1.PostedFile.FileName;
                    string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                    string fileSn = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    ////转换成绝对地址,
                    string serverpath = Server.MapPath(@"~\Uploads\bmfjjeqr\") + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                    ////转换成与相对地址,相对地址为将来访问图片提供
                    string relativepath = @"~\Uploads\bmfjjeqr\" + fileSn +"-"+ filename;//.Substring(filename.LastIndexOf("."));
                    ////绝对地址用来将上传文件夹保存到服务器的具体路下。
                    if (!Directory.Exists(Server.MapPath(@"~\Uploads\bmfjjeqr\")))
                    {
                        Directory.CreateDirectory(Server.MapPath(@"~\Uploads\bmfjjeqr\"));
                    }
                    FileUpload1.PostedFile.SaveAs(serverpath);
                    ////把相对路径的地址保存到页面hdImageUrl的value值上以供保存值时使用。
                    hiddFileDz.Value = relativepath;
                    Lafilename.Text = filename;
                    laFilexx.Text = "上传成功";
                    btn_sc.Text = "修改附件";
                    FileUpload1.Visible = false;
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
            FileUpload1.Visible = true;
            Lafilename.Text = "";
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string strdz = hiddFileDz.Value;
        string strsql = "update bill_ys_xmfjbm set attachment=@attachment where procode=@procode and deptcode=@deptcode and kmcode=@kmcode";
        SqlParameter[] paramter = { 
                                      new SqlParameter("@attachment",strdz),
                                      new SqlParameter("@procode",strnd),
                                       new SqlParameter("@kmcode",strkmcode),
                                      new SqlParameter("@deptcode",strdeptcode)
                                      };
        int irel = new sqlHelper.sqlHelper().ExecuteNonQuery(strsql, paramter);
        if (irel > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功。');self.close();", true);
        }
    }
}
