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
using System.IO;

public partial class SaleBill_Upload_Upload : System.Web.UI.Page
{
    protected string _strPath;
    static string strfileName;
    static string strFile;
    protected string _strType = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        this._strPath = cFilesPath.Value;
        if (Request["Type"] != null)
        {
            _strType = Request["Type"].ToString();
        }
        if (Request["UseName"] != null && Request["UseName"].Trim().Equals("false"))
        {
            this.txtFileNames.Style.Add("display", "none");
        }
        if (_strType == "ProductImage")
        {
            _strPath = cProductPath.Value;
            this.trComment.Style.Add("display", "none");
        }
        if (_strType == "Picture")
        {
            _strPath = cImagePath.Value;
            this.trComment.Style.Add("display", "none");
        }
        if (_strType == "Media")
        {
            _strPath = cMediaPath.Value;
            this.trComment.Style.Add("display", "none");
        }
        if (_strType == "Flash")
        {
            _strPath = cFlashPath.Value;
            this.trComment.Style.Add("display", "none");
        }
        if (_strType == "Person")
        {
            _strPath = cPhotoPath.Value;
            this.trComment.Style.Add("display", "none");
        }
        if (_strType == "File")
        {
            this.trComment.Style.Add("display", "none");
        }
    }

    protected void btnAcc_Click(object sender, EventArgs e)
    {
        string _Name = savefile();
        if (_Name != "")
        {
            if (this.txtFileNames.Text == "")
                this.txtFileNames.Text = _Name;
            this.txtFile.Text = strFile;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "selectFile", "selectFile()", true);
        }
    }
    string savefile()
    {
        string _strName = string.Empty;
        string UrlName = string.Empty;
        string name = string.Empty;
        string exname = string.Empty;
        int intFlag = 0;
        if (this.ctrlPicture.PostedFile.ContentLength != 0)
        {
            if (this.ctrlPicture.HasFile)
            {
                DirectoryInfo di = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(_strPath));
                if (!di.Exists)
                {
                    di.Create();
                }
                DirectoryInfo[] arrdi = di.GetDirectories();
                for (int i = 0; i < arrdi.Length; i++)
                {
                    if (arrdi[i].Name == (DateTime.Now.ToString("yyyy-MM")))
                    {
                        intFlag = 1;
                        di = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(_strPath + DateTime.Now.ToString("yyyy-MM") + "\\"));
                        break;
                    }
                }
                if (intFlag == 0)
                {
                    di = Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(_strPath) + DateTime.Now.ToString("yyyy-MM") + "\\");
                }
                string fileName = this.ctrlPicture.PostedFile.FileName;
                name = System.IO.Path.GetFileName(fileName).Split('.')[0];
                exname = System.IO.Path.GetExtension(fileName);

                if (isOK(exname))
                {
                    _strName = DateTime.Now.ToString("yyyyMMddhhmmss") + exname;
                    if (_strName != "")
                    {
                        UrlName = di.FullName + _strName;
                        this.ctrlPicture.SaveAs(UrlName);
                        strFile = getPath(UrlName);
                    }
                }
                else
                {
                    return "";
                }

            }
        }
        return name + exname;

    }
    public string getPath(string AbuPath)
    {
        string strPath = string.Empty;
        string finalpath = string.Empty;
        if (AbuPath != "")
        {
            strPath = Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString() + "\\");
            finalpath = AbuPath.Replace(strPath, "").Replace("\\", "/");
            return finalpath;
        }
        return "";
    }
    bool isOK(string exname)
    {
        if (_strType == "Picture" || _strType == "ProductImage")
        {
            if (exname.ToLower() != ".jpg" && exname.ToLower() != ".png" && exname.ToLower() != ".gif")
            {
                Response.Write("<script>alert('只能上传*.jpg *.png,*.gif类型的文件');</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hiddenInfoLayer", "try{hiddenInfoLayer_Control();}catch(e){};", true);
                return false;
            }
        }
        if (_strType == "Media")
        {
            if (exname.ToLower() != ".wmv" && exname.ToLower() != ".mp3" && exname.ToLower() != ".mpg")
            {
                Response.Write("<script>alert('只能上传*.wmv *.mp3,*.mpg类型的文件');</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hiddenInfoLayer", "try{hiddenInfoLayer_Control();}catch(e){};", true);
                return false;
            }
        }
        if (_strType == "Flash")
        {
            if (exname.ToLower() != ".swf" && exname.ToLower() != ".flv")
            {
                Response.Write("<script>alert('只能上传*.swf *.flv类型的文件');</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hiddenInfoLayer", "try{hiddenInfoLayer_Control();}catch(e){};", true);
                return false;
            }
        }
        if (_strType == "File")
        {
            if (exname.ToLower() == ".exe" || exname.ToLower() == ".ini" || exname.ToLower() == ".htm" || exname.ToLower() == ".asp" || exname.ToLower() == ".aspx" || exname.ToLower() == ".ascx" || exname.ToLower() == ".config" || exname.ToLower() == ".asmx" || exname.ToLower() == ".master")
            {
                Response.Write("<script>alert('文件格式不正确，不能上传.exe,.ini文件！');</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hiddenInfoLayer", "try{hiddenInfoLayer_Control();}catch(e){};", true);
                return false;
            }
        }
        return true;
    }
}
