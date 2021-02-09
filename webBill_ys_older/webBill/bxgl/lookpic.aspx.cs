using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_bxgl_lookpic : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!string.IsNullOrEmpty(Request["billCode"]))
        {
            lbl_billname.Text = "报销单号:" + Request["billCode"].ToString();
            string strsql = @"select top 1  fujian,(select billname from bill_main where billcode=m.billcode)as billname,bxzy,* from bill_ybbxmxb  m 
                        where billCode=(select top 1 billcode from bill_main where billname='" + Request["billCode"] + "')";
            DataTable dt = new DataTable();
            dt = server.GetDataTable(strsql, null);
            if (dt != null)
            {
                string[] arrTemp = dt.Rows[0]["fujian"].ToString().Split('|');
                lbl_zy.Text = dt.Rows[0]["bxzy"].ToString();
                string[] arrname = arrTemp[0].Split(';');
                string[] arrfile = arrTemp[1].Split(';');

                for (int i = 0; i < arrname.Count() - 1; i++)
                {
                   
                    string strfile = arrfile[i];
                    strfile = strfile.Substring(1, strfile.Length - 1);
                    strfile = "../../" + strfile;

                    if (strfile.IndexOf(".jpg") > 1 || strfile.IndexOf(".png") > 1 || strfile.IndexOf(".gif") > 1)
                    {
                        Literal1.Text += " <div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>附件：</span><img src='" + strfile + "'  width='60%' title='" + arrname[i] + "'/></div>";
                    }
                }
            }

            // string fujian = server.GetCellValue("select top 1 fujian from bill_ybbxmxb where billCode =(select top 1 billcode from bill_main where billname='" + Request["billCode"] + "')");
            //if (!string.IsNullOrEmpty(fujian))
            //{
            //    string[] arrTemp = fujian.Split('|');

            //    string[] arrname = arrTemp[0].Split(';');
            //    string[] arrfile = arrTemp[1].Split(';');
            //    for (int i = 0; i < arrname.Count() - 1; i++)
            //    {

            //        string strfile = arrfile[i];
            //        strfile = strfile.Substring(1, strfile.Length - 1);
            //        strfile = "../../" + strfile;
            //        Literal1.Text += " <div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>附件" + (i + 1) + "：</span><img src='" + strfile + "'  title='" + strfile + "'/></div>";


            //        //                    Literal1.Text += @"<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;
            //        //                        <span style='font-weight:700'>附件" + (i + 1) + "：</span><a href='../../AFrame/download.aspx?filename=" + Server.UrlEncode(arrname[i]) + "&filepath=" + Server.UrlEncode(arrfile[i]) + "' target='_blank'>" + arrname[i] + "下载;</a></div>";

            //    }

            //}

         
        }
    }
}