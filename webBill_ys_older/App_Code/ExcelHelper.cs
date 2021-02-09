using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;

public class ExcelHelper
{
    public void ExpExcel<T>(IList<T> list, string fileName, IDictionary<string, string> propertyName, string title)
    {
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
        HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8)));
        HttpContext.Current.Response.Clear();
        if (list != null && list.Count > 0)
        {
            HttpContext.Current.Response.BinaryWrite(ListToStream(list, propertyName, title).GetBuffer());
        }
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();

    }


    private MemoryStream ListToStream<T>(IList<T> list, IDictionary<string, string> propertyNameList, string title)
    {
        //创建内存流
        using (MemoryStream ms = new MemoryStream())
        {
            //将控制excel表头的参数写入到一个临时集合
            //List<IDictionary<string, string>> propertyNameList = new List<IDictionary<string, string>>();
            //if (propertyName != null)
            //{
            //    propertyNameList.AddRange(propertyName);
            //}

            //创建NOPI对象
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            //表头的行号  如果有标题就是1 没有标题就是0
            int headerRowNum = 0;
            if (!string.IsNullOrEmpty(title))
            {
                headerRowNum = 1;
                IRow titleRow = sheet.CreateRow(0);
                titleRow.Height = 999;

                ICell titleCell = titleRow.CreateCell(0);
                titleCell.SetCellValue(title);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                //设置字体
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 18;
                font.FontName = "微软雅黑";
                cellStyle.SetFont(font);
                //对齐方式
                cellStyle.Alignment = HorizontalAlignment.Center;
                titleCell.CellStyle = cellStyle;
            }
            IRow headerRow = sheet.CreateRow(headerRowNum);
            if (list.Count <= 0)
            {
                return null;
            }
            //通过反射得到对象的属性集合
            PropertyInfo[] propertys = list[0].GetType().GetProperties();

            //遍历属性集合生成excel的表头标题
            int cellIndex = 0;
            if (propertyNameList == null || propertyNameList.Count == 0)
            {
                //如果没有传入自定义的导出表头
                for (int i = 0; i < propertys.Count(); i++)
                {
                    headerRow.CreateCell(i).SetCellValue(propertys[i].Name);
                }
            }
            else
            {
                //用户自定义的
                foreach (KeyValuePair<string, string> item in propertyNameList)
                {
                    for (int i = 0; i < propertys.Count(); i++)
                    {
                        if (propertys[i].Name.Equals(item.Key))
                        {
                            headerRow.CreateCell(cellIndex).SetCellValue(item.Value);
                            cellIndex++;
                            break;
                        }
                    }
                }
            }
            #region 上面的方式可以根据传入的列的顺序导入
            //for (int i = 0; i < propertys.Count(); i++)
            //{
            //    //判断excel表头是否是用户定义
            //    if (propertyNameList.Count == 0)
            //    {
            //        headerRow.CreateCell(i).SetCellValue(propertys[i].Name);
            //    }
            //    else
            //    {
            //        foreach (KeyValuePair<string, string> item in propertyNameList)
            //        {
            //            if (item.Key.Equals(propertys[i].Name))
            //            {
            //                headerRow.CreateCell(cellIndex).SetCellValue(item.Value);
            //                cellIndex++;
            //                break;
            //            }
            //        }
            //    }
            //}
            #endregion
            //遍历生成excel的行集数据
            int rowIndex = headerRowNum + 1;
            if (propertyNameList == null || propertyNameList.Count == 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    for (int j = 0; j < propertys.Count(); j++)
                    {
                        object obj = propertys[j].GetValue(list[i], null);
                        dataRow.CreateCell(j).SetCellValue(obj == null ? "" : obj.ToString());
                    }
                    rowIndex++;
                }
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    int cellIndex2 = 0;
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (KeyValuePair<string, string> item in propertyNameList)
                    {

                        for (int j = 0; j < propertys.Count(); j++)
                        {
                            if (item.Key.Equals(propertys[j].Name))
                            {
                                object obj = propertys[j].GetValue(list[i], null);
                                dataRow.CreateCell(cellIndex2).SetCellValue(obj == null ? "" : obj.ToString());
                                cellIndex2++;
                                break;
                            }
                        }
                    }
                    rowIndex++;
                }
            }
            #region 上面的方式可以根据传入的列的顺序导入
            //int rowIndex = 1;
            //for (int i = 0; i < list.Count; i++)
            //{
            //    IRow dataRow = sheet.CreateRow(rowIndex);
            //    int cellIndex2 = 0;
            //    for (int j = 0; j < propertys.Count(); j++)
            //    {
            //        //指定了excel表头信息
            //        if (propertyNameList.Count == 0)
            //        {
            //            object obj = propertys[j].GetValue(list[i], null);
            //            dataRow.CreateCell(j).SetCellValue(obj == null ? "" : obj.ToString());
            //        }
            //        else
            //        {
            //            foreach (KeyValuePair<string, string> item in propertyNameList)
            //            {
            //                if (item.Key.Equals(propertys[j].Name))
            //                {
            //                    object obj = propertys[j].GetValue(list[i], null);
            //                    dataRow.CreateCell(cellIndex2).SetCellValue(obj == null ? "" : obj.ToString());
            //                    cellIndex2++;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //    rowIndex++;
            //}
            #endregion

            //求合并的列数
            int rangesize = headerRow.Cells.Count;
            //合并单元格
            NPOI.SS.Util.CellRangeAddress cellRangeAddress = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, rangesize - 1);
            sheet.AddMergedRegion(cellRangeAddress);
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;


        }
    }

    public void ExpExcel(DataTable dt, GridView gv)
    {
        ExpExcel(dt, gv, "Export");
    }
    /// <summary>
    /// 接收一个DataTable并导出excel
    /// </summary>
    /// <param name="dt"></param>
    public void ExpExcel(DataTable dt, GridView gv, string fileName)
    {
        //临时文件
        string strTempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        //使用OleDb链接
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strTempFile + ";Extended Properties=Excel 8.0");
        using (conn)
        {
            conn.Open();
            //创建Tablesheet
            System.Text.StringBuilder sbColumnsName = new System.Text.StringBuilder();
            int iColumns = gv.Columns.Count;
            //所有类型为BoundField的列
            var datafileds = gv.Columns.OfType<BoundField>();
            foreach (BoundField bf in datafileds)
            {
                string strColumnname = bf.HeaderText;
                if (string.IsNullOrEmpty(strColumnname))
                {
                    strColumnname = bf.DataField;
                }
                sbColumnsName.Append(string.Format(" [{0}] VarChar,", strColumnname));
            }
            string strcolumns = sbColumnsName.ToString().Substring(0, sbColumnsName.Length - 1);
            string strCreatSql = string.Format("CREATE TABLE SHEET1 ({0})", strcolumns);

            OleDbCommand cmdCreate = new OleDbCommand(strCreatSql, conn);
            cmdCreate.ExecuteNonQuery();

            //创建insert
            int iRowsCount = dt.Rows.Count;
            for (int j = 0; j < iRowsCount; j++)
            {
                System.Text.StringBuilder sbColumnsFiled = new System.Text.StringBuilder();
                //插入的sql
                string strInsertSql = "";
                OleDbCommand cmdInsert = new OleDbCommand();
                using (cmdInsert)
                {
                    DataRow dr = dt.Rows[j];
                    foreach (string item in datafileds.Select(f => f.DataField))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            string strF = string.Format("@{0},", item);
                            //处理sql
                            sbColumnsFiled.Append(strF);
                            //处理parameter
                            cmdInsert.Parameters.AddWithValue(strF, dr[item]);
                        }

                    }
                    //处理sql
                    string strFiles = sbColumnsFiled.ToString().Substring(0, sbColumnsFiled.Length - 1);
                    strInsertSql = string.Format("INSERT INTO [SHEET1$] VALUES({0})", strFiles);
                    cmdInsert.Connection = conn;
                    cmdInsert.CommandText = strInsertSql;
                    cmdInsert.ExecuteNonQuery();
                    cmdInsert.Dispose();
                }

            }
        }
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8)));
        HttpContext.Current.Response.BinaryWrite(System.IO.File.ReadAllBytes(strTempFile));
        HttpContext.Current.Response.End();
        System.IO.File.Delete(strTempFile);
    }
    /// <summary>
    /// 接收一个datatable导出Excel 第二个参数是可变参数  可以指定导出的列名
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="propertyName"></param>
    public void ExpExcel(DataTable dt, string fileName, IDictionary<string, string> propertyName)
    {
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
        HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8)));
        HttpContext.Current.Response.Clear();
        if (dt.Rows.Count <= 0)
        {
            return;
        }
        HttpContext.Current.Response.BinaryWrite(DataTableToStream(dt, propertyName).GetBuffer());
        HttpContext.Current.Response.End();
    }
    /// <summary>
    /// 将文件保存到服务器然后下载的方式 效率高
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="propertyName"></param>
    /// <param name="ExportFileName"></param>
    public void ExpExcelRedirect(DataTable dt, string ExportFileName, IDictionary<string, string> propertyName)
    {
        //临时文件
        string xuni = string.Format("~/Uploads/{0}.xls", Guid.NewGuid());
        string tempFile = HttpContext.Current.Server.MapPath(xuni);
        //string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        using (MemoryStream ms = DataTableToStream(dt, propertyName))
        {
            using (FileStream fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
        }
        HttpContext.Current.Response.Redirect(string.Format("~/AFrame/download.aspx?filename={0}&filepath={1}", ExportFileName, xuni));
        //HttpContext.Current.Response.Clear();
        //HttpContext.Current.Response.Charset = "UTF-8";
        //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
        //HttpContext.Current.Response.ContentType = "application/ms-excel;charset=UTF-8";
        //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpContext.Current.Server.UrlDecode(ExportFileName) + ".xls");
        //HttpContext.Current.Response.BinaryWrite(File.ReadAllBytes(tempFile));
        //HttpContext.Current.Response.End();
        //File.Delete(tempFile);
    }

    private MemoryStream DataTableToStream(DataTable dt, IDictionary<string, string> propertyNameList)
    {
        if (dt.Rows.Count <= 0)
        {
            return null;
        }
        //创建内存流
        using (MemoryStream ms = new MemoryStream())
        {
            ////将控制excel表头的参数写入到一个临时集合
            //List<string> propertyNameList = new List<string>();
            //if (propertyName != null)
            //{
            //    propertyNameList.AddRange(propertyName);
            //}
            //创建NOPI对象
            IWorkbook workbook = new HSSFWorkbook();
            int dataIndex = 0;//数据源行索引
            int rowIndex = 0;//在excel表格中的行索引
            int sheetIndex = 0;
            int linjie = 65535;
            ISheet sheet = workbook.CreateSheet(sheetIndex.ToString());
            IRow headerRow = sheet.CreateRow(0);

            //遍历属性集合生成excel的表头标题
            //通过反射得到对象的属性集合
            DataColumnCollection dcc = dt.Columns;
            int cellIndex = 0;

            //判断excel表头是否是用户定义
            if (propertyNameList == null || propertyNameList.Count == 0)
            {
                //表头部分
                for (int i = 0; i < dcc.Count; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(dcc[i].ColumnName);
                }
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    dataIndex = k;
                    sheetIndex = dataIndex / linjie + 1;
                    //新添加sheet 重新生成一次表头
                    if (k % linjie == 0 && k > 0)
                    {
                        rowIndex = 0;
                        sheet = workbook.CreateSheet(sheetIndex.ToString());
                        headerRow = sheet.CreateRow(0);
                        //重新生成一次表头
                        for (int i = 0; i < dcc.Count; i++)
                        {
                            headerRow.CreateCell(i).SetCellValue(dcc[i].ColumnName);
                        }
                    }
                    //遍历生成excel的行集数据
                    rowIndex++;
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    for (int j = 0; j < dcc.Count; j++)
                    {
                        string val = dt.Rows[k][dcc[j].ColumnName].ToString();
                        double dbVal = 0;
                        if (double.TryParse(val, out dbVal))
                        {
                            dataRow.CreateCell(j).SetCellValue(dbVal);
                        }
                        else
                        {
                            dataRow.CreateCell(j).SetCellValue(val);
                        }
                    }
                }
            }
            else
            {
                //表头部分
                foreach (KeyValuePair<string, string> item in propertyNameList)
                {
                    if (dcc.Contains(item.Key))
                    {
                        headerRow.CreateCell(cellIndex).SetCellValue(item.Value);
                        cellIndex++;
                    }
                }
                //表体部分
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    sheetIndex = dataIndex / linjie + 1;
                    //新添加sheet 重新生成一次表头
                    if (k % linjie == 0 && k > 0)
                    {
                        rowIndex = 0;
                        sheet = workbook.CreateSheet(sheetIndex.ToString());
                        headerRow = sheet.CreateRow(0);
                        cellIndex = 0;
                        foreach (KeyValuePair<string, string> item in propertyNameList)
                        {
                            if (dcc.Contains(item.Key))
                            {
                                headerRow.CreateCell(cellIndex).SetCellValue(item.Value);
                                cellIndex++;
                            }
                        }
                    }
                    //遍历生成excel的行集数据
                    int cellIndex2 = 0;
                    rowIndex++;
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (KeyValuePair<string, string> item2 in propertyNameList)
                    {
                        if (dcc.Contains(item2.Key))
                        {
                            string val = dt.Rows[k][item2.Key].ToString();
                            double dbVal = 0;
                            if (double.TryParse(val, out dbVal))
                            {
                                dataRow.CreateCell(cellIndex2).SetCellValue(dbVal);
                            }
                            else
                            {
                                dataRow.CreateCell(cellIndex2).SetCellValue(val);
                            }

                            cellIndex2++;
                        }
                    }
                }
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }
    }

    /// <summary>
    /// Excel文件导成Datatable
    /// </summary>
    /// <param name="strFilePath">Excel文件目录地址</param>
    /// <param name="strTableName">Datatable表名</param>
    /// <param name="iSheetIndex">Excel sheet index</param>
    /// <returns></returns>
    public static DataTable XlSToDataTable(string strFilePath, string strTableName, int iSheetIndex)
    {

        string strExtName = Path.GetExtension(strFilePath);

        DataTable dt = new DataTable();
        if (!string.IsNullOrEmpty(strTableName))
        {
            dt.TableName = strTableName;
        }

        if (strExtName.Equals(".xls") || strExtName.Equals(".xlsx"))
        {
            using (FileStream file = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(file);
                ISheet sheet = workbook.GetSheetAt(iSheetIndex);

                //列头
                foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
                {
                    dt.Columns.Add(item.ToString(), typeof(string));
                }

                //写入内容
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                while (rows.MoveNext())
                {
                    IRow row = (HSSFRow)rows.Current;
                    if (row.RowNum == sheet.FirstRowNum)
                    {
                        continue;
                    }

                    DataRow dr = dt.NewRow();
                    foreach (ICell item in row.Cells)
                    {
                        switch (item.CellType)
                        {
                            case CellType.Boolean:
                                dr[item.ColumnIndex] = item.BooleanCellValue;
                                break;
                            case CellType.Error:
                                dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                break;
                            case CellType.Formula:
                                switch (item.CachedFormulaResultType)
                                {
                                    case CellType.Boolean:
                                        dr[item.ColumnIndex] = item.BooleanCellValue;
                                        break;
                                    case CellType.Error:
                                        dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(item))
                                        {
                                            dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                        }
                                        else
                                        {
                                            dr[item.ColumnIndex] = item.NumericCellValue;
                                        }
                                        break;
                                    case CellType.String:
                                        string str = item.StringCellValue;
                                        if (!string.IsNullOrEmpty(str))
                                        {
                                            dr[item.ColumnIndex] = str.ToString();
                                        }
                                        else
                                        {
                                            dr[item.ColumnIndex] = null;
                                        }
                                        break;
                                    case CellType.Unknown:
                                    case CellType.Blank:
                                    default:
                                        dr[item.ColumnIndex] = string.Empty;
                                        break;
                                }
                                break;
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(item))
                                {
                                    dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                }
                                else
                                {
                                    dr[item.ColumnIndex] = item.NumericCellValue;
                                }
                                break;
                            case CellType.String:
                                string strValue = item.StringCellValue;
                                if (string.IsNullOrEmpty(strValue))
                                {
                                    dr[item.ColumnIndex] = strValue.ToString();
                                }
                                else
                                {
                                    dr[item.ColumnIndex] = null;
                                }
                                break;
                            case CellType.Unknown:
                            case CellType.Blank:
                            default:
                                dr[item.ColumnIndex] = string.Empty;
                                break;
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
        }

        return dt;
    }
}