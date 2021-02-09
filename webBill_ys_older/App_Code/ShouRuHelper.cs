using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Bll;

/// <summary>
///ShouRuHelper 的摘要说明
/// </summary>
public class ShouRuHelper
{
    public ShouRuHelper()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    public DataSet getData(string businesscode, string date)
    {
        date = string.Format("<Request><Date>{0}</Date></Request>", date);
        ShouRu.n_bank_webservice service = new ShouRu.n_bank_webservice();
        DataSet ds = new DataSet();
        try
        {
            string strresult = service.f_yonyou_inter(businesscode, date);
            //byte[] buffer = Encoding.UTF8.GetBytes(strresult);
            //MemoryStream stream = new MemoryStream(buffer);
            //ds.ReadXml(stream);
            ds = new PublicServiceBLL().ConvertXMLToDataSet(strresult);
            return ds;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
