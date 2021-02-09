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

public partial class AFrame_download : System.Web.UI.Page
{
   
         string fileName, filePath, originName;
        string fullName;
        string contentType;
        FileStream fs;
        long fileLen;

        protected void Page_Load(object sender, System.EventArgs e)
        {
           
            fileName = Request["filename"];
            
            filePath = Request["filepath"];
            fileName = Server.UrlDecode(fileName);
            filePath = Server.UrlDecode(filePath);
            originName = Request["originName"];

            if (fileName == null || fileName == "")
            {
                Response.Write("Exception: fileName lost");
                Response.End();
            }

            if (filePath == null || filePath == "")
            {
                Response.Write("Exception: filePath lost");
                Response.End();
            }
            //if (filePath.IndexOf("file") < 0)
            //{
            //    Response.Write("Sorry,Path not found!");
            //    Response.End();
            //}
            if (originName == null || originName.Length < 1)
                originName = fileName;

            string fileExtension;
            if (originName.IndexOf(".") > -1)
                fileExtension = originName.Substring(originName.LastIndexOf("."));
            else
                fileExtension = string.Empty;

            switch (fileExtension)
            {
                case ".txt":
                    contentType = "text/plain";
                    break;
                case ".asf":
                    contentType = "video/x-ms-asf";
                    break;
                case ".avi":
                    contentType = "video/avi";
                    break;
                case ".doc":
                    contentType = "application/msword";
                    break;
                case".docx":
                    contentType = "application/msword";
                    break;
                case ".zip":
                    contentType = "application/zip";
                    break;
                case ".xls":
                    contentType = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    contentType = "application/vnd.ms-excel";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".wav":
                    contentType = "audio/wav";
                    break;
                case ".mp3":
                    contentType = "audio/mpeg3";
                    break;
                case ".mpg":
                case ".mpeg":
                    contentType = "video/mpeg";
                    break;
                case ".rtf":
                    contentType = "application/rtf";
                    break;
                case ".htm":
                case ".html":
                    contentType = "text/html";
                    break;
                case ".aspx":
                case ".asp":
                case ".dll":
                case ".cs":
                case ".vb":
                    contentType = "text/plain";
                    break;

                default:
                    contentType = "application/octet-stream";
                    break;

            }

            fullName = Server.MapPath(filePath);
            //System.Text.UTF8Encoding  encoder  = new  System.Text.UTF8Encoding();

            fileName = HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(originName));
            if (!File.Exists(fullName))
            {
                Response.Write("File not Found!");
                Response.End();
            }

            fs = new FileStream(fullName, FileMode.Open, FileAccess.Read);

            fileLen = fs.Length;
            if (fileLen >= 5242880) //5M
            {
                fs.Close();
                Response.Write("Exception: the file is too large");
                Response.End();
            }

            Response.Buffer = true;
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.AddHeader("Content-Length", fileLen.ToString());
            Response.Charset = "UTF-8";
            Response.ContentType = contentType;

            Response.WriteFile(fullName);
            Response.Flush();
            Response.Clear();
            fs.Close();
            Response.End();

    }
}
