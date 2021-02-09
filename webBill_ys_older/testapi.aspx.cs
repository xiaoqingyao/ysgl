using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class testapi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://api-data.xiaogj.com/dazhi/GetFeeList?sign=2584239397f66bbf5306a9b56fb4ce49&timestamp=1442560189&sdate=2015-09-18&edate=2015-09-22");
        //接收结果
        System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
        Stream stream = response.GetResponseStream();   //获取响应的字符串流
        StreamReader sr = new StreamReader(stream);
        string rel=sr.ReadToEnd();
        ShouRuRel modelshouru = new ShouRuRel();
        using (StringReader stringreader = new StringReader(rel))
        {
            JsonSerializer serializer = new JsonSerializer();
            modelshouru = (ShouRuRel)serializer.Deserialize(new JsonTextReader(stringreader), typeof(ShouRuRel));
        }
        this.GridView1.DataSource = modelshouru.Data;
        this.GridView1.DataBind();

        //System.IO.Stream responseStream = response.GetResponseStream();

        //StringBuilder sbend = new StringBuilder();
        //int readOnce = 2000;
        ////int offset = 0;
        //int readCont = 0;
        //byte[] result = new byte[readOnce];
        //string rel="";
        ////do
        ////{
        ////    readCont = responseStream.Read(result, 0, readOnce);
        ////    rel = System.Text.Encoding.UTF8.GetString(result);
        ////    //Response.Write(rel);
        ////    //offset += readCont;
        ////    if (readCont < 0) break;
        ////} while (true);



        //int lastindex = 0;
        //while (offset != lastindex)
        //{
        //    offset = lastindex;
        //    lastindex = responseStream.Read(result, offset, result.Length);
        //}



        ////返回结果转换字符串(需要考虑编码集问题)
        //String resultStr = System.Text.Encoding.UTF8.GetString(result);

        TextBox1.Text = rel.ToString();
        //requestStream.Close();
    }
}