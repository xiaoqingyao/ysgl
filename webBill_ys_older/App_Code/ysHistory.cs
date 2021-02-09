using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// ysHistory 的摘要说明
/// </summary>
public class ysHistory
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public ysHistory()
    {
    }
    public DataSet getHistory(string deptCode, string gcbh)
    {
        DataSet gcInfo = server.GetDataSet("select * from bill_ysgc where gcbh='" + gcbh + "'");

        string qnGcbh = server.GetCellValue("select gcbh from bill_ysgc where nian='" + (int.Parse(gcInfo.Tables[0].Rows[0]["nian"].ToString()) - 1) + "' and yue='" + gcInfo.Tables[0].Rows[0]["yue"].ToString() + "' and ystype='" + gcInfo.Tables[0].Rows[0]["ystype"].ToString() + "'");

        DataSet temp = new DataSet();

        if (gcInfo.Tables[0].Rows[0]["ysType"].ToString().Trim() == "0")//年预算
        {
            temp = server.GetDataSet("select yskmCode,yskmBm,replicate('&nbsp;&nbsp;',len(yskmCode)-2)+yskmmc as yskmmc,tbsm,(case tblx when '01' then '单位填报' when '02' then '<font color=red>财务填报</font>' end) as tblx,ysje from bill_ysmxb,bill_yskm where bill_yskm.yskmCode=bill_ysmxb.yskm and ysType='1' and gcbh='" + qnGcbh + "'");

        }
        else if (gcInfo.Tables[0].Rows[0]["ysType"].ToString().Trim() == "1")//季度预算
        {
            temp = server.GetDataSet("exec bill_pro_ysb_history_jd '" + deptCode + "','" + gcbh + "','" + qnGcbh + "','" + server.GetCellValue("select gcbh from bill_ysgc where nian='" + gcInfo.Tables[0].Rows[0]["nian"].ToString()  + "' and ystype='0'") + "'");
        }
        else if (gcInfo.Tables[0].Rows[0]["ysType"].ToString().Trim() == "2")//月预算
        {
            string nian = gcInfo.Tables[0].Rows[0]["nian"].ToString().Trim();
            int yue = int.Parse(gcInfo.Tables[0].Rows[0]["yue"].ToString().Trim());
            string jidu = "";
            string months1 = "";
            string months2 = "";
            string months3 = "";
            if (yue >= 1 && yue <= 3)
            {
                jidu = "一";
                months1 = nian + "01";
                months2 = nian + "02";
                months3 = nian + "03";
            }
            else if (yue >= 4 && yue <= 6)
            {
                jidu = "二";
                months1 = nian + "04";
                months2 = nian + "05";
                months3 = nian + "06";
            }
            else if (yue >= 7 && yue <= 9)
            {
                jidu = "三";
                months1 = nian + "07";
                months2 = nian + "08";
                months3 = nian + "09";
            }
            else if (yue >= 10 && yue <= 12)
            {
                jidu = "四";
                months1 = nian + "10";
                months2 = nian + "11";
                months3 = nian + "12";
            }

            temp = server.GetDataSet("exec bill_pro_ysb_history_yue '" + deptCode + "','" + gcbh + "','" + qnGcbh + "','" + server.GetCellValue("select gcbh from bill_ysgc where nian='" + gcInfo.Tables[0].Rows[0]["nian"].ToString() + "' and ystype='0'") + "','" + server.GetCellValue("select gcbh from bill_ysgc where nian='" + gcInfo.Tables[0].Rows[0]["nian"].ToString() + "' and yue='" + jidu + "' and ystype='1'") + "'," + months1 + "," + months2 + "," + months3 + "");

        }
        return temp;
    }


    public void bindHistory(DataGrid myGrid,DataSet temp,string deptCode,string gcbh)
    {

        DataColumn dtCol1 = new DataColumn("qnysje");
        temp.Tables[0].Columns.Add(dtCol1);
        DataColumn dtCol2 = new DataColumn("nysje");
        temp.Tables[0].Columns.Add(dtCol2);
        DataColumn dtCol3 = new DataColumn("nbxe");
        temp.Tables[0].Columns.Add(dtCol3);
        DataColumn dtCol4 = new DataColumn("nysYe");
        temp.Tables[0].Columns.Add(dtCol4);
        DataColumn dtCol5 = new DataColumn("jdysJe");
        temp.Tables[0].Columns.Add(dtCol5);
        DataColumn dtCol6 = new DataColumn("jdbxe");
        temp.Tables[0].Columns.Add(dtCol6);
        DataColumn dtCol7 = new DataColumn("jdysYe");
        temp.Tables[0].Columns.Add(dtCol7);


        DataSet tempHistory = (new ysHistory()).getHistory(deptCode, gcbh);
        string ysType = server.GetCellValue("select ysType from bill_ysgc where gcbh='" + gcbh + "'");
        if (ysType == "0")//年度预算
        {
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                for (int j = 0; j <= tempHistory.Tables[0].Rows.Count - 1; j++)
                {
                    if (tempHistory.Tables[0].Rows[j]["yskmCode"].ToString().Trim() == temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim())
                    {
                        if (tempHistory.Tables[0].Rows[j]["ysje"] == DBNull.Value)
                        {
                            temp.Tables[0].Rows[i]["qnysje"] = 0;
                        }
                        else
                        {
                            temp.Tables[0].Rows[i]["qnysje"] = double.Parse(tempHistory.Tables[0].Rows[j]["ysje"].ToString().Trim());
                        }
                    }
                }
            }
        }
        else if (ysType == "1")//季度预算
        {
            myGrid.Columns[10].Visible = true;
            myGrid.Columns[11].Visible = true;
            myGrid.Columns[12].Visible = true;

            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                for (int j = 0; j <= tempHistory.Tables[0].Rows.Count - 1; j++)
                {
                    if (tempHistory.Tables[0].Rows[j]["yskmCode"].ToString().Trim() == temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim())
                    {
                        temp.Tables[0].Rows[i]["qnysje"] = double.Parse(tempHistory.Tables[0].Rows[j]["ysje"].ToString().Trim());
                        temp.Tables[0].Rows[i]["nysje"] = double.Parse(tempHistory.Tables[0].Rows[j]["nysje"].ToString().Trim());
                        temp.Tables[0].Rows[i]["nbxe"] = double.Parse(tempHistory.Tables[0].Rows[j]["bxe"].ToString().Trim());
                        temp.Tables[0].Rows[i]["nysYe"] = double.Parse(tempHistory.Tables[0].Rows[j]["nysye"].ToString().Trim());
                    }
                }
            }
        }
        else if (ysType == "2")//月预算
        {
            myGrid.Columns[10].Visible = true;
            myGrid.Columns[11].Visible = true;
            myGrid.Columns[12].Visible = true;
            myGrid.Columns[13].Visible = true;
            myGrid.Columns[14].Visible = true;
            myGrid.Columns[15].Visible = true;

            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                for (int j = 0; j <= tempHistory.Tables[0].Rows.Count - 1; j++)
                {
                    if (tempHistory.Tables[0].Rows[j]["yskmCode"].ToString().Trim() == temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim())
                    {
                        temp.Tables[0].Rows[i]["qnysje"] = double.Parse(tempHistory.Tables[0].Rows[j]["ysje"].ToString().Trim());
                        temp.Tables[0].Rows[i]["nysje"] = double.Parse(tempHistory.Tables[0].Rows[j]["nysJe"].ToString().Trim());
                        temp.Tables[0].Rows[i]["nbxe"] = double.Parse(tempHistory.Tables[0].Rows[j]["nbxe"].ToString().Trim());
                        temp.Tables[0].Rows[i]["nysYe"] = double.Parse(tempHistory.Tables[0].Rows[j]["nysYe"].ToString().Trim());
                        temp.Tables[0].Rows[i]["jdysJe"] = double.Parse(tempHistory.Tables[0].Rows[j]["jdysJe"].ToString().Trim());
                        temp.Tables[0].Rows[i]["jdbxe"] = double.Parse(tempHistory.Tables[0].Rows[j]["jdbxe"].ToString().Trim());
                        temp.Tables[0].Rows[i]["jdysYe"] = double.Parse(tempHistory.Tables[0].Rows[j]["jdysYe"].ToString().Trim());
                    }
                }
            }
        }
    }
}
