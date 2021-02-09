using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Reflection;
//using Excel = Microsoft.Office.Interop.Excel;

/// <summary>
///DataGridToExcel 的摘要说明
/// </summary>
public static class DataGridToExcel
{




    /// <summary>
    /// 把DataTable内容导出伟excel并返回客户端 
    ///  /// <param name="dgData">待导出的DataTable</param> 
    /// 创 建 人：zyl 
    /// 创建日期：2015年01月05日    
    /// </summary>
    /// <param name="dt">数据源</param>
    /// <param name="hiddenTexts">影藏列</param>
    /// <param name="fileName">列名,隐藏后的列名，可用于排序，如果该字段为空，则显示</param>
    /// <param name="headerTexts"></param>

    public static void DataTable2Excel(DataTable dt, string hiddens, string files, string headers)
    {
        string[] hiddenTexts, fileName, headerTexts;

        hiddenTexts = hiddens.Split(',');
        fileName = files.Split(',');
        headerTexts = headers.Split(',');
        //---------------------隐藏字段设置-------------
        //移除不需要显示的字段  
        foreach (string columnName in hiddenTexts)
        {
            dt.Columns.Remove(columnName);
        }


        for (int i = 0; i < fileName.Length; i++)
        {
            dt.Columns[fileName[i]].SetOrdinal(i);
        }

        //---------------------头部标题设置-------------
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            if (fileName.Length > i && headerTexts.Length > i)
            {
                dt.Columns[fileName[i]].ColumnName = headerTexts[i];
            }
        }


        System.Web.UI.WebControls.DataGrid dgExport = null;
        // 当前对话 
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        // IO用于导出并返回excel文件 
        System.IO.StringWriter strWriter = null;
        System.Web.UI.HtmlTextWriter htmlWriter = null;

        if (dt != null)
        {


            curContext.Response.Clear();//Clear方法删除所有缓存中的HTML输出 
            curContext.Response.Buffer = true;//服务器将不会向客户端发送任何信息 

            // 设置编码和附件格式 
            //  curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            curContext.Response.Charset = "utf-8";
            curContext.Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            curContext.Response.ContentType = "application/ms-excel";//文件类型  

            // 导出excel文件 
            strWriter = new System.IO.StringWriter();
            htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

            // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid 
            dgExport = new System.Web.UI.WebControls.DataGrid();
            dgExport.DataSource = dt.DefaultView;
            dgExport.AllowPaging = false;
            dgExport.DataBind();

            // 返回客户端 
            dgExport.RenderControl(htmlWriter);
            curContext.Response.Write(strWriter.ToString());
            curContext.Response.End();

        }
    }



    //dt当然是你一些的要导出的数据返回一个DataTable
    //fileName是自己定义的文件导出的名字
    public static void CreateExcel(DataTable dt, string hiddens, string files, string headers)
    {
        string[] hiddenTexts, fileName, headerTexts;

        hiddenTexts = hiddens.Split(',');
        fileName = files.Split(',');
        headerTexts = headers.Split(',');
        //---------------------隐藏字段设置-------------
        //移除不需要显示的字段  
        foreach (string columnName in hiddenTexts)
        {
            dt.Columns.Remove(columnName);
        }


        for (int i = 0; i < fileName.Length; i++)
        {
            dt.Columns[fileName[i]].SetOrdinal(i);
        }

        //---------------------头部标题设置-------------
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            if (fileName.Length > i && headerTexts.Length > i)
            {
                dt.Columns[fileName[i]].ColumnName = headerTexts[i];
            }
        }





        // 当前对话 
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        StringBuilder strb = new StringBuilder();
        strb.Append(" <table align=\"center\" border='1px' style='border-collapse:collapse;table-layout:fixed;font-size:12px'> <tr>");
        //写列标题    
        int columncount = dt.Columns.Count;
        for (int columi = 0; columi < columncount; columi++)
        {
            strb.Append(" <td> <b>" + dt.Columns[columi] + " </b> </td>");
        }
        strb.Append(" </tr>");
        //写数据    
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            strb.Append(" <tr>");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                strb.Append(" <td>" + dt.Rows[i][j].ToString() + " </td>");
            }
            strb.Append(" </tr>");
        }
        strb.Append(" </table>");

        curContext.Response.Clear();
        curContext.Response.Buffer = true;
        curContext.Response.Charset = "GB2312";
        curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName + ".xls");
        curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文   
        curContext.Response.ContentType = "application/vnd.xls";//设置输出文件类型为excel文件。     
        curContext.Response.Write(strb);
        curContext.Response.End();
    }




    /// <summary>
    /// dataTable存成excel表格 暂时不可用
    ///  备注：如果strFileName处已经存在文件,则报错
    /// </summary>
    /// <param name="dt">dataTable</param>
    /// <param name="strFileName">在服务端完整路径</param>
    //public static void DataTableToExcel(DataTable dt, string strFileName, string hiddens, string files, string headers)
    //{
    //    System.Web.HttpContext curContext = System.Web.HttpContext.Current;

    //    string[] hiddenTexts, fileName, headerTexts;

    //    hiddenTexts = hiddens.Split(',');
    //    fileName = files.Split(',');
    //    headerTexts = headers.Split(',');
    //    //---------------------隐藏字段设置-------------
    //    //移除不需要显示的字段  
    //    foreach (string columnName in hiddenTexts)
    //    {
    //        dt.Columns.Remove(columnName);
    //    }


    //    for (int i = 0; i < fileName.Length; i++)
    //    {
    //        dt.Columns[fileName[i]].SetOrdinal(i);
    //    }

    //    //---------------------头部标题设置-------------
    //    for (int i = 0; i < dt.Columns.Count; i++)
    //    {
    //        if (fileName.Length > i && headerTexts.Length > i)
    //        {
    //            dt.Columns[fileName[i]].ColumnName = headerTexts[i];
    //        }
    //    }


    //    int rowNum = dt.Rows.Count;
    //    int columnNum = dt.Columns.Count;
    //    int rowIndex = 1;
    //    int columnIndex = 0;

    //    if (dt == null || string.IsNullOrEmpty(strFileName))
    //    {
    //        return;
    //    }
    //    if (rowNum > 0)
    //    {
    //        Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
    //        xlApp.DefaultFilePath = curContext.Server.MapPath("a/");
    //        xlApp.DisplayAlerts = true;
    //        xlApp.SheetsInNewWorkbook = 1;
    //        Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
    //        //将DataTable的列名导入Excel表第一行
    //        foreach (DataColumn dc in dt.Columns)
    //        {
    //            columnIndex++;
    //            xlApp.Cells[rowIndex, columnIndex] = dc.ColumnName;
    //        }
    //        //将DataTable中的数据导入Excel中
    //        for (int i = 0; i < rowNum; i++)
    //        {
    //            rowIndex++;
    //            columnIndex = 0;
    //            for (int j = 0; j < columnNum; j++)
    //            {
    //                columnIndex++;
    //                xlApp.Cells[rowIndex, columnIndex] = dt.Rows[i][j].ToString();
    //            }
    //        }
    //        xlBook.SaveCopyAs(strFileName);
    //        xlApp = null;
    //        xlBook = null;
    //    }
    //}



}
