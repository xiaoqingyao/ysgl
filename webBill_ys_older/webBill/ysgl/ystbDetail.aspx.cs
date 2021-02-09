using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Ajax;

public partial class ysgl_ystbDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(ysgl_ystbDetail));
            if (!IsPostBack)
            {
                string gcbh = Page.Request.QueryString["gcbh"].ToString().Trim();
                DataSet temp = server.GetDataSet("select * from bill_ysgc where gcbh='" + gcbh + "'");
                this.Label1.Text = temp.Tables[0].Rows[0]["xmmc"].ToString().Trim();

                this.bindData();
            }
        }
    }

    void bindData()
    {
        string deptGuid = server.GetCellValue("select userdept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
        DataSet temp = server.GetDataSet("exec bill_pro_ysmxb_dept '" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','" + deptGuid + "'");

        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();

        temp = server.GetDataSet("select * from bill_main where billName='" + Page.Request.QueryString["gcbh"].ToString().Trim() + "' and billDept='" + deptGuid + "'");
        if (temp.Tables[0].Rows.Count == 0)
        {
            this.Button3.Visible = false;
            this.Button4.Visible = false;
            this.Button5.Visible = false;
        }
        else
        {
            this.lblBillCode.Text = temp.Tables[0].Rows[0]["billCode"].ToString().Trim();
            string stepID = temp.Tables[0].Rows[0]["stepID"].ToString().Trim();
            if (stepID == "-1")//未提交
            {
                this.Label2.Text = "预算信息未提交！";
                this.Button4.Visible = false;
                this.Button5.Visible = false;
                this.myGrid.Columns[3].Visible = true;
                this.myGrid.Columns[4].Visible = false;
            }
            else if (stepID == "0")
            {
                this.Label2.Text = "预算信息审核退回！";
                this.Button4.Visible = false;
                this.Button5.Visible = false;
                this.myGrid.Columns[3].Visible = true;
                this.myGrid.Columns[4].Visible = false;
            }
            else if (stepID == "begin")
            {
                this.Label2.Text = "预算信息已提交！";
                this.Button1.Visible = false;
                this.Button3.Visible = false;
                this.Button5.Visible = false;
                this.myGrid.Columns[3].Visible = false;
                this.myGrid.Columns[4].Visible = true;
                this.Button9.Visible = true;
            }
            else if (stepID == "end")
            {
                this.Label2.Text = "预算信息已审核通过！";
                this.Button1.Visible = false;
                this.Button3.Visible = false;
                this.Button5.Visible = false;
                this.myGrid.Columns[3].Visible = false;
                this.myGrid.Columns[4].Visible = true;
            }
            else
            {
                this.Label2.Text = "预算信息审核中！";
                this.Button1.Visible = false;
                this.Button3.Visible = false;
                this.Button5.Visible = false;
                this.myGrid.Columns[3].Visible = false;
                this.myGrid.Columns[4].Visible = true;
            }
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
        //Response.Redirect("ystbFrame.aspx");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string infoList = "";

        DataSet ysgc = server.GetDataSet("select * from bill_ysgc where gcbh='" + Page.Request.QueryString["gcbh"].ToString().Trim() + "'");
        string ysgc_nian = ysgc.Tables[0].Rows[0]["nian"].ToString().Trim();
        string ysgc_ysType = ysgc.Tables[0].Rows[0]["ysType"].ToString().Trim();
        string ysgc_yue = ysgc.Tables[0].Rows[0]["yue"].ToString().Trim();
        string sjgc = "";
        string ysmc = "";
        string yueYs_yues = "";
        if (ysgc_ysType == "0")//年度预算
        { }
        else if (ysgc_ysType == "1")//季度预算
        {
            ysmc = "年预算";
            sjgc = server.GetCellValue("select gcbh from bill_ysgc where nian='" + ysgc_nian + "' and ysType='0'");//获取年预算的编号
        }
        else if (ysgc_ysType == "2")//月预算
        {
            ysmc = "季度预算";
            string jd = "";
            if (int.Parse(ysgc_yue) >= 1 && int.Parse(ysgc_yue) <= 3)
            {
                jd = "一";
                yueYs_yues = "'1','2','3'";
            }
            else if (int.Parse(ysgc_yue) >= 4 && int.Parse(ysgc_yue) <= 6)
            {
                jd = "二";
                yueYs_yues = "'4','5','6'";
            }
            else if (int.Parse(ysgc_yue) >= 7 && int.Parse(ysgc_yue) <= 9)
            {
                jd = "三";
                yueYs_yues = "'7','8','9'";
            }
            else if (int.Parse(ysgc_yue) >= 10 && int.Parse(ysgc_yue) <= 12)
            {
                jd = "四";
                yueYs_yues = "'10','11','12'";
            }
            sjgc = server.GetCellValue("select gcbh from bill_ysgc where nian='" + ysgc_nian + "' and ysType='1' and yue='" + jd + "'");//获取年预算的编号
        }

        //获取当前人员的部门编号
        string deptGuid = server.GetCellValue("select userdept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
        //获取该部门此次预算的唯一标识
        string loopTimes = "";
        string ysGuid = "";
        DataSet temp = server.GetDataSet("select billCode,loopTimes from bill_main where billName='" + Page.Request.QueryString["gcbh"].ToString().Trim() + "' and billDept='" + deptGuid + "'");
        if (temp.Tables[0].Rows.Count == 0)//没有过预算
        {
            ysGuid = (new GuidHelper()).getNewGuid();//生成新的预算编号
            loopTimes = "1";
        }
        else
        {
            ysGuid = temp.Tables[0].Rows[0][0].ToString().Trim();
            loopTimes = Convert.ToString(int.Parse(temp.Tables[0].Rows[0][1].ToString().Trim()) + 1);
        }
        List<string> list = new List<string>();
        //删除原有数据
        list.Add("delete from bill_main where billCode='" + ysGuid + "'");
        list.Add("delete from bill_ysmxb where billCode='" + ysGuid + "'");

        double yszje = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            string yskm = this.myGrid.Items[i].Cells[0].Text.ToString().Trim();
            TextBox txt = (TextBox)this.myGrid.Items[i].FindControl("TextBox2");
            try
            {
                double ysje = double.Parse(txt.Text.ToString().Trim());
                if (ysje == 0)
                { }
                else
                {
                    if (yskm.Length == 2)
                    {
                        yszje += ysje;
                    }
                    //是否超过上级预算额度:首先 要获取该部门上级预算过程的预算金额
                    //季度预算
                    if (ysgc_ysType == "0")
                    { }
                    else if (ysgc_ysType == "1")//季度预算
                    {
                        DataSet temp1 = server.GetDataSet("select * from bill_ysmxb where gcbh='" + sjgc + "' and yskm='" + yskm + "' and ysdept='" + deptGuid + "'");
                        if (temp1.Tables[0].Rows.Count == 0)
                        {
                            infoList = "预算科目" + yskm + "在 【年度预算】 中不存在！";
                            break;
                        }
                        else
                        {
                            double je = double.Parse(temp1.Tables[0].Rows[0]["ysje"].ToString().Trim());
                            if (je < ysje)//超过上级预算额度
                            {
                                infoList = "预算科目" + yskm + "的预算金额 【" + ysje.ToString("0.00") + "】 大于对应 【年度预算】 的金额【" + je.ToString("0.00") + "】";
                                break;
                            }
                        }
                    }
                    else if (ysgc_ysType == "2")//月预算
                    {
                        //获取季度预算数据
                        DataSet temp1 = server.GetDataSet("select ysje from bill_ysmxb where gcbh='" + sjgc + "' and yskm='" + yskm + "' and ysdept='" + deptGuid + "'");
                        if (temp1.Tables[0].Rows.Count == 0)
                        {
                            infoList = "预算科目" + yskm + "在 【季度预算】 中不存在！";
                            break;
                        }
                        else
                        {
                            DataSet temp2 = server.GetDataSet("select isnull(sum(ysje),0) from bill_ysmxb where yskm='" + yskm + "' and ysdept='" + deptGuid + "' and gcbh in (select gcbh from bill_ysgc where nian='" + ysgc_nian + "' and yue in (" + yueYs_yues + ") and yue<>'" + ysgc_yue + "')");
                            double tempJe = double.Parse(temp2.Tables[0].Rows[0][0].ToString().Trim()) + ysje;
                            double jdysJe = double.Parse(temp1.Tables[0].Rows[0][0].ToString().Trim());
                            if (tempJe > jdysJe)
                            {
                                infoList = "预算科目" + yskm + "的月预算合计金额 【" + tempJe.ToString("0.00") + "】 大于对应 【季度预算】 的金额【" + jdysJe.ToString("0.00") + "】";
                                break;
                            }
                        }
                    }
                    //写入预算明细
                    list.Add("insert into bill_ysmxb(gcbh,billCode,yskm,ysje,ysDept,ysType) values('" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','" + ysGuid + "','" + yskm + "'," + ysje.ToString() + ",'" + deptGuid + "','1')");
                }
            }
            catch { }
        }
        //写入预算总表 记录
        list.Add("insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType) values('" + ysGuid + "','" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','ys','-1','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString() + "','" + deptGuid + "'," + yszje.ToString() + ",'" + loopTimes + "','1')");

        //是否有超过上级预算的科目 有则提示，无则保存
        if (infoList == "")
        {
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');self.close();", true);
            }
        }
        else
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + infoList + "');", true);
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        //获取当前人员的部门编号
        string deptGuid = server.GetCellValue("select userdept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
        if (server.ExecuteNonQuery("update bill_main set stepID='begin',loopTimes=loopTimes+1 where flowID='ys' and stepID='-1' and billDept='" + deptGuid + "' and billName='" + Page.Request.QueryString["gcbh"].ToString().Trim() + "'") == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('提交失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('提交成功！');self.close();", true);
        }
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../../workFlow/stepLook.aspx?billType=ys&billCode=" + this.lblBillCode.Text.ToString().Trim() + "');", true);
    }


    [Ajax.AjaxMethod(HttpSessionStateRequirement.Read)]
    public string[] getCalResult(string currentCode, int[] arrIndex, string[] arrCode, string[] arrVal)
    {
        string[] returnVal = new string[arrIndex.Length];
        int len = currentCode.Length;
        while (len >= 4)
        {
            double dValue = 0;
            for (int i = 0; i <= arrIndex.Length - 1; i++)
            {
                if (arrCode[i].Length == len && arrCode[i].Substring(0, len - 2) == currentCode.Substring(0, len - 2))//同级编号
                {
                    dValue += double.Parse(arrVal[i]);
                }
            }
            //找到上级并赋值
            for (int i = 0; i <= arrIndex.Length - 1; i++)
            {
                string cCode = currentCode.Substring(0, len - 2);
                if (arrCode[i] == cCode)//找到上级
                {
                    arrVal[i] = dValue.ToString();
                }
            }
            len = len - 2;
        }

        for (int i = 0; i <= arrIndex.Length - 1; i++)
        {
            returnVal[i] = arrIndex[i].ToString().Trim() + "," + double.Parse(arrVal[i].ToString().Trim()).ToString("0.00");
        }
        return returnVal;
    }
    protected void Button9_Click(object sender, EventArgs e)
    {
        //获取当前人员的部门编号
        string deptGuid = server.GetCellValue("select userdept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
        if (server.ExecuteNonQuery("update bill_main set stepID='-1',loopTimes=loopTimes+1 where flowID='ys' and stepID='begin' and billDept='" + deptGuid + "' and billName='" + Page.Request.QueryString["gcbh"].ToString().Trim() + "'") == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('撤销提交失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('撤销提交成功！');self.close();", true);
        }
    }
}