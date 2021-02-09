using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;

public partial class AFrame_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        string businesscode = this.TextBox1.Text.Trim();
        string date = this.TextBox2.Text.Trim(); ;
        DataSet ds = getData(businesscode, date);
        if (ds == null)
        {
            return;
        }
        //foreach (DataColumn item in ds.Tables[1].Columns)
        //{
        //    Response.Write(item.ColumnName + ",");
        //}
        this.GridView1.DataSource = ds.Tables[1];
        this.GridView1.DataBind();
        //tables0 包含errorcode和errormessage
        decimal deCharges = 0;
        //tables1包含主体内容 Rpdept,Ordept,Classname,Date,Charges,output_Id, 
        //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
        //{
        //    if (ds.Tables[1].Rows[i]["DeptCode"].ToString().Equals("40902"))
        //    {
        //        deCharges += decimal.Parse(ds.Tables[1].Rows[i]["Charges"].ToString());
        //    }
        //}
        //Response.Write(deCharges);
    }
    private DataSet getData(string businesscode, string date)
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
            ds = ConvertXMLToDataSet(strresult);
            return ds;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private DataSet ConvertXMLToDataSet(string xmlData)
    {
        StringReader stream = null;
        XmlTextReader reader = null;
        try
        {
            DataSet xmlDS = new DataSet();
            stream = new StringReader(xmlData);
            reader = new XmlTextReader(stream);
            xmlDS.ReadXml(reader);
            return xmlDS;
        }
        catch (Exception ex)
        {
            string strTest = ex.Message;
            return null;
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
    }

}
