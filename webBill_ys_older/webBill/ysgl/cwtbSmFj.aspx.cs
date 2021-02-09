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
using System.IO;

public partial class webBill_ysgl_cwtbSmFj : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
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

                string sql = "";
                if (Page.Request.QueryString["type"].ToString().Trim() == "add")
                {
                    sql = "insert into bill_ysmxb_smfj(billCode,yskm,fj) values('" + Page.Request.QueryString["billCode"].ToString().Trim() + "','" + Page.Request.QueryString["yskm"].ToString().Trim() + "','" + guid + "." + extName + "')";
                }
                else
                {
                    if (server.GetCellValue("select count(1) from bill_ysmxb_smfj where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "' and yskm='" + Page.Request.QueryString["yskm"].ToString().Trim() + "'") == "0")
                    {
                        sql = "insert into bill_ysmxb_smfj(billCode,yskm,fj) values('" + Page.Request.QueryString["billCode"].ToString().Trim() + "','" + Page.Request.QueryString["yskm"].ToString().Trim() + "','" + guid + "." + extName + "')";
                    }
                    else
                    {
                        sql = "update bill_ysmxb_smfj set fj='" + guid + "." + extName + "' where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "' and yskm='" + Page.Request.QueryString["yskm"].ToString().Trim() + "'";
                    }
                }
                if (server.ExecuteNonQuery(sql) == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('上传失败！');", true);
                    return;
                }
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('数据库记录失败！');", true);
            return;
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('上传完成！');window.returnValue=\"sucess\";self.close();", true);
    }
}
