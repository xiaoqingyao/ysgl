using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Ajax;
using System.Collections.Generic;
using Dal.UserProperty;
using Bll.newysgl;

public partial class webBill_ysgl_ysnzjAdd : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    DepartmentDal deptDal = new DepartmentDal();
    bill_ysmxbBll ysmxbill = new bill_ysmxbBll();
    string strNowDeptCode = "";
    string strNowDeptName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["gcbh"])))
            {
                Label2.Text = "当前预算过程为：" + server.GetCellValue("select xmmc  from biLL_ysgc where gcbh='" + Convert.ToString(Request.QueryString["gcbh"]) + "'");
            }

            Ajax.Utility.RegisterTypeForAjax(typeof(webBill_ysgl_ysnzjAdd));

            string usercode = Session["userCode"].ToString().Trim();
            DataTable dtdept = deptDal.getUsercodeName(Session["userCode"].ToString().Trim());


            //获取当前用户所在的部门编号及其部门名称
            strNowDeptCode = dtdept.Rows[0]["deptcode"].ToString();
            strNowDeptName = dtdept.Rows[0]["deptName"].ToString();



            string strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");
            dtuserRightDept = deptDal.getRigtusers(strDeptCodes, strNowDeptCode);
            if (!IsPostBack)
            {
                #region 绑定人员管理下的部门
                if (!strNowDeptCode.Equals(""))
                {
                    //获取人员管理下的部门
                    if (strDeptCodes != "")
                    {
                        if (dtuserRightDept.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtuserRightDept.Rows.Count; i++)
                            {
                                ListItem li = new ListItem();
                                li.Text = "[" + dtuserRightDept.Rows[i]["deptCode"].ToString().Trim() + "]" + dtuserRightDept.Rows[i]["deptName"].ToString().Trim();
                                li.Value = dtuserRightDept.Rows[i]["deptCode"].ToString().Trim();
                                this.LaDept.Items.Add(li);
                            }
                        }
                        this.LaDept.Items.Insert(0, new ListItem("[" + strNowDeptCode + "]" + strNowDeptName, strNowDeptCode));
                        this.LaDept.SelectedIndex = 0;
                    }
                }
                #endregion

                this.bindData();


            }
        }
    }

    void bindData()
    {
        if (LaDept.SelectedValue != null)
        {
            string deptGuid = LaDept.SelectedValue.Trim();
            DataSet temp = server.GetDataSet("select yskmCode,yskmBm,replicate('　　',len(yskmCode)-2)+yskmmc as yskmMc,(case tblx when '01' then '单位填报' when '02' then '<font color=red>财务填报</font>' end) as tblx from bill_yskm where yskmcode in (select yskmcode from bill_yskm_dept where deptCode='" + deptGuid + "') or tblx='02' order by yskmCode");
            this.myGrid.DataSource = temp;
            this.myGrid.DataBind();

            string strmoney = "";

            string strkmcode = "";
            string strdeptcode = this.LaDept.SelectedValue.Trim(); //部门编号
            string strnd = Request["gcbh"].ToString();//预算过程编号
            strnd = strnd.Substring(0, 4);//获取年度

            for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
            {
                strkmcode = this.myGrid.Items[i].Cells[0].Text.ToString().Trim();//科目编号
                TextBox txt = (TextBox)this.myGrid.Items[i].FindControl("TextBox2");
                TextBox txtmoney = (TextBox)this.myGrid.Items[i].FindControl("txtkzjmoney");

                try
                {
                    strmoney = server.GetCellValue("exec GetCouldAddAmount '" + strdeptcode + "','" + strkmcode + "','" + strnd + "'");

                    double doumoney = 0;


                    if (strmoney != "" && strmoney != null)
                    {
                        double.TryParse(strmoney, out doumoney);
                        if (doumoney >= 0)
                        {
                            //txt.Text = strmoney.Trim();
                            txtmoney.Text = strmoney.Trim();

                        }
                    }
                    else
                    {
                        txt.Text = "0.00";
                        txtmoney.Text = "0.00";
                    }

                }
                catch { }
            }
        }


    }
    /// <summary>
    /// 取消返回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("yszjFrame.aspx?Ctrl=ysnzj" + "&page=ysnzj");

    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        //获取当前人员的部门编号
        string deptGuid = this.LaDept.SelectedValue.Trim();

        List<string> list = new List<string>();
        double yszje = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            string yskm = this.myGrid.Items[i].Cells[0].Text.ToString().Trim();
            TextBox txt = (TextBox)this.myGrid.Items[i].FindControl("TextBox2");
            TextBox txtmoney = (TextBox)this.myGrid.Items[i].FindControl("txtkzjmoney");
            try
            {

                double ysje = double.Parse(txt.Text.ToString().Trim());
                double kzjmoney = double.Parse(txtmoney.Text.ToString().Trim());
                //判断如果填写的追加金额大于默认值则提示“追加金额超限”
                if (ysje == 0)
                {
                    continue;
                }
                if (ysje > kzjmoney)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('追加金额超限！');", true);
                    return;
                }
                yszje += ysje;
                string ysGuid = (new GuidHelper()).getNewGuid();//mian表中的code
                string stryscg = Request["gcbh"].ToString().Trim();
                list.Add("insert into bill_main (billCode,billName,flowid,stepid,billDept,billUser,billDate,billJe,LoopTimes,billType) values('" + ysGuid + "','" + stryscg + "','ysnzj','-1','" + deptGuid + "','" + Session["userCode"].ToString().Trim() + "','" + Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) + "','" + ysje.ToString() + "','1','5')");
                list.Add("insert into bill_ysmxb (gcbh,billcode,yskm,ysje,ysdept,ystype) values('" + stryscg + "','" + ysGuid + "','" + yskm + "','" + ysje.ToString() + "','" + deptGuid + "','5')");

                #region 屏蔽直接修改预算的代码
                //string strsqlmoney = ysmxbill.getmoney(deptGuid, yskm, Request["gcbh"].ToString().Trim());

                //if (strsqlmoney != null && strsqlmoney != "")//先判断 如果有更新的目标的记录直接更新
                //{
                //    yszje = double.Parse(strsqlmoney.Trim());
                //    yszje += ysje;
                //    //写入预算明细
                //    list.Add("Update bill_ysmxb set ysje='" + yszje + "' where ysdept='" + deptGuid + "'  and yskm='" + yskm + "' and gcbh='" + Request["gcbh"].ToString().Trim() + "' and ysType='1'");
                //}
                //else
                //{

                //    //如果没有 让他去找有相同预算过程编号的billcode 如果有这个编号（也就是生成了填报了预算） 然后往bill_ysmxb中添加一条记录 gcbh 找到的billCode,yskm,zjje,ysDept,ystype

                //    string strbillcode = server.GetCellValue("select top 1 bill_main.billCode from bill_main,bill_ysmxb where bill_main.billcode=bill_ysmxb.billcode and gcbh='" + Request["gcbh"].ToString().Trim() + "' and ysDept='" + deptGuid + "' and ysType='1'");
                //    if (strbillcode != "" && strbillcode != null)
                //    {
                //        list.Add("insert into bill_ysmxb(gcbh,billCode,yskm,ysje,ysDept,ysType) values('" + Request["gcbh"].ToString().Trim() + "','" + strbillcode + "','" + yskm + "','" + ysje.ToString() + "','" + deptGuid + "','1')");

                //    }
                //    else
                //    {
                //        list.Add("insert into bill_ysmxb(gcbh,billCode,yskm,ysje,ysDept,ysType) values('" + Request["gcbh"].ToString().Trim() + "','" + ysGuid + "','" + yskm + "','" + ysje.ToString() + "','" + deptGuid + "','1')");
                //        list.Add("insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType) values('" + ysGuid + "','" + Request["gcbh"].ToString().Trim() + "','ys','end','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString() + "','" + deptGuid + "','" + ysje.ToString() + "','1','1')");
                //    }
                //}
                #endregion


            }
            catch (Exception ex) { throw; }

        }
        if (yszje == 0)
        {
            Response.Write("<script>alert('总金额为0，保存失败！')</script>");
            return;
        }
        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.open('yszjFrame.aspx?Ctrl=ysnzj','_self');", true);
        }


    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentCode"></param>
    /// <param name="arrIndex"></param>
    /// <param name="arrCode"></param>
    /// <param name="arrVal"></param>
    /// <returns></returns>

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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LaDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            TextBox zjtext = e.Item.FindControl("TextBox2") as TextBox;
            string kmcode = Convert.ToString(e.Item.Cells[0].Text.Trim());
            if (LaDept.SelectedValue != null)
            {
                string dept = LaDept.SelectedValue.Trim();
                int count = Convert.ToInt32(server.GetCellValue("select count(*)  from bill_yskm_dept where deptCode='" + dept + "' AND  yskmCode LIKE '" + kmcode + "%'"));
                if (count > 1)
                {
                    zjtext.Enabled = false;
                    zjtext.BackColor=System.Drawing.Color.LightGray;
                }
            }
        }
    }
}
