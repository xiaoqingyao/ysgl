<%@ WebHandler Language="C#" Class="uploadFile" %>

using System;
using System.Web;

public class uploadFile : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        HttpFileCollection files = context.Request.Files;//这里只能用<input type="file" />才能有效果,因为服务器控件是HttpInputFile类型
        string msg = string.Empty;
        string error = string.Empty;
        string imgurl;
        if (files.Count > 0)
        {
            string filename = System.IO.Path.GetFileName(files[0].FileName);
            filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            string DynamicPath = System.DateTime.Now.ToString("yyyyMMdd") + "\\";
            string fileSn = DynamicPath + System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //服务器硬盘地址
            string serverpath = context.Server.MapPath(@"~\Uploads\ybbx\") + fileSn + "-" + filename;
            //转换成与相对地址,相对地址为将来访问图片提供
            string relativepath = @"~\Uploads\ybbx\" + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                                                                               ////绝对地址用来将上传文件夹保存到服务器的具体路下。
            if (!System.IO.Directory.Exists(context.Server.MapPath(@"~\Uploads\ybbx\" + DynamicPath)))
            {
                System.IO.Directory.CreateDirectory(context.Server.MapPath(@"~\Uploads\ybbx\" + DynamicPath));
            }
            files[0].SaveAs(serverpath);
            msg = " 成功! 文件大小为:" + files[0].ContentLength;
            imgurl = "/" + filename;
            string res = "{ error:'" + error + "', msg:'" + msg + "',imgurl:'" + context.Server.UrlEncode(context.Server.UrlPathEncode(imgurl)) + "',filename:'" + context.Server.UrlEncode(context.Server.UrlPathEncode(filename)) + "',fileurl:'" + context.Server.UrlEncode(context.Server.UrlPathEncode(relativepath)) + "'}";
            context.Response.Write(res);
            context.Response.End();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}