using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

/// <summary>
///ErrorModule 的摘要说明
/// </summary>
public class ErrorModule : IHttpModule
{
    public void Dispose()
    {

    }

    public void Init(HttpApplication context)
    {
        context.Error += new EventHandler(customcontext_Error);
    }

    private void customcontext_Error(object sender, EventArgs e)
    {
        HttpContext context = HttpContext.Current;
        Exception exp = context.Server.GetLastError();

        if (exp.GetType().Name != "HttpException")
        {

            string url = context.Request.Url.ToString();
            StringBuilder sb = new StringBuilder();

            sb.Append("Date:").Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")).Append("\n");
            sb.Append("Url:").Append(url).Append("\n");
            int count = context.Request.Form.Count;
            sb.Append("Form");
            for (int i = 0; i < count; i++)
            {
                sb.Append(context.Request.Form.Keys[i]).Append(":").Append(context.Request.Form[i]);
                sb.Append(" ");
            }
            sb.Append("\n");

            sb.Append("Exp:").Append(exp.Message).Append("\n");

            if (exp.InnerException != null)
            {
                sb.Append("innerExp:").Append(exp.InnerException.Message).Append("\n");
            }
            if (exp.StackTrace != null)
            {
                sb.Append("StackTrace:").Append(exp.StackTrace).Append("\n\n\n");
            }
            //context.Response.Write(exp.Message+exp.InnerException.Message + url);
            //context.Server.ClearError();

            string path = context.Server.MapPath("../../Log") + "/ErrorLog.txt";
            WriteFile(sb.ToString(), path);
        }
    }

    private void WriteFile(string input, string fname)
    {
        FileInfo finfo = new FileInfo(fname);
        ///创建只写文件流
        using (FileStream fs = finfo.OpenWrite())
        {
            ///根据上面创建的文件流创建写数据流
            StreamWriter w = new StreamWriter(fs);
            //设置写数据流的起始位置为文件流的末尾
            w.BaseStream.Seek(0, SeekOrigin.End);
            w.Write(input + "\n");
            w.Write("----------------------------------------------------------------------\n");
            w.Flush();
            w.Close();
        }
    }
}
