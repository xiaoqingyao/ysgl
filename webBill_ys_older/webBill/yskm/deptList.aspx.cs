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
using System.Data.SqlClient;
using Models;
using Bll.UserProperty;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Dal;

public partial class webBill_yskm_deptList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ConfigDal configdal = new ConfigDal();

    string strqmys = "";//是否是全面预算

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {

            if (!IsPostBack)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["deptCode"]).Trim()))
                {
                    this.Label1.Text = "当前部门：[" + Convert.ToString(Request.QueryString["deptCode"]).Trim() + "]" + server.GetCellValue(" select deptName  from  bill_departments where deptCode='" + Convert.ToString(Request.QueryString["deptCode"]).Trim() + "'");
                }
                else
                {
                    Button1.Enabled = false;
                }
                //绑定单据类型
                binddjlx();
                //绑定数据表格
                this.BindDataGrid();
            }
        }
    }


    public void BindDataGrid()
    {
        string deptCode = Request.Params["deptCode"];
        if (string.IsNullOrEmpty(deptCode))
        {
            return;
        }
        //string strdjlx = ddlBill.SelectedValue.Split(new string[] { "|" }, StringSplitOptions.None)[0];//单据类型
        string strdjlx = "02";
        if (!string.IsNullOrEmpty(ddlBill.SelectedValue))// && ddlBill.SelectedValue != "00|"
        {
            strdjlx = ddlBill.SelectedValue.Split(new string[] { "|" }, StringSplitOptions.None)[1];
        }
        DataTable dt = server.GetDataTable("exec getYskm_Dept '" + deptCode + "','" + strdjlx + "'", null);
        this.myGrid.DataSource = dt;
        this.myGrid.DataBind();
        #region 之前的程序实现方式  已注释
        //SysManager sysMgr = new SysManager();
        //string deptCode = Request.Params["deptCode"];
        //string strdjlx = ddlBill.SelectedValue.Split(new string[] { "|" }, StringSplitOptions.None)[0];//决算单类型
        //string strdydj = ddlBill.SelectedValue.Split(new string[] { "|" }, StringSplitOptions.None)[1];//该类型对应的预算类型 01收入 02费用……
        //IList<Bill_Yskm> list;
        //list = new Dal.SysDictionary.YskmDal().GetYskm("", strdydj, "");
        //sysMgr.SetEndYsbm(list);
        //this.myGrid.DataSource = list;
        //myGrid.DataBind();

        //string[] deptYskm = sysMgr.GetYskmCodeByDept(deptCode, strdjlx);//将预算科目code保存到数组
        //for (int i = 0; i < myGrid.Items.Count; i++)//循环myGrid
        //{
        //    var count = (from temp in deptYskm
        //                 where temp == myGrid.Items[i].Cells[1].Text
        //                 select temp).Count();//查询该部门是否已经对照了该科目
        //    if (count > 0 && myGrid.Items[i].Cells[10].Text == "1")//如果是并且为末级复选框勾选
        //    {
        //        ((CheckBox)myGrid.Items[i].FindControl("CheckBox1")).Checked = true;
        //    }
        //}
        #endregion
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {

        if (ddlBill.Visible==true)
        {
            if (string.IsNullOrEmpty(ddlBill.SelectedValue))// || ddlBill.SelectedValue == "00|"
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择单据类型。');", true);
                return;
            }
        }
        
        string strdjlb = "02";
        if (!string.IsNullOrEmpty(ddlBill.SelectedValue))// && ddlBill.SelectedValue != "00|"
        {
            strdjlb = ddlBill.SelectedValue.Split(new string[] { "|" }, StringSplitOptions.None)[1];

        }
        List<bill_yskm_dept> listmode = new List<bill_yskm_dept>();
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            bill_yskm_dept mode = new bill_yskm_dept();
            if (chk.Checked)
            {
                string strjfcode1 = "";
                string strdfcode1 = "";
                string strjfcode2 = "";
                string strdfcode2 = "";
                string isend = myGrid.Items[i].Cells[10].Text.Trim();
                if (isend=="0")//如果不是末级部门
                {
                    continue;
                }
                mode.deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
                mode.yskmCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                if ((myGrid.Items[i].FindControl("txt_jfkmcode1") as TextBox).Text.Trim() != "")
                {
                    strjfcode1 = (myGrid.Items[i].FindControl("txt_jfkmcode1") as TextBox).Text.Trim();
                    if (strjfcode1.IndexOf("]")>0)
                    {
                        strjfcode1 = strjfcode1.Substring(1, strjfcode1.IndexOf("]") - 1);
                    }
                  
                    mode.jfkmcode1 = strjfcode1;
                }
                if ((myGrid.Items[i].FindControl("txt_dfkmcode1") as TextBox).Text.Trim() != "")
                {
                    strdfcode1 = (myGrid.Items[i].FindControl("txt_dfkmcode1") as TextBox).Text.Trim();

                    strdfcode1 = strdfcode1.Substring(1, strdfcode1.IndexOf("]") - 1);
                    mode.dfkmcode1 = strdfcode1;
                }

                if ((myGrid.Items[i].FindControl("txt_jfkmcode2") as TextBox).Text.Trim() != "")
                {
                    strjfcode2 = (myGrid.Items[i].FindControl("txt_jfkmcode2") as TextBox).Text.Trim();
                    strjfcode2 = strjfcode1.Substring(1, strjfcode2.IndexOf("]") - 1);
                    mode.jfkmcode2 = strjfcode2;
                }
                if ((myGrid.Items[i].FindControl("txt_dfkmcode2") as TextBox).Text.Trim() != "")
                {
                    strdfcode2 = (myGrid.Items[i].FindControl("txt_dfkmcode2") as TextBox).Text.Trim();
                    strdfcode2 = strdfcode1.Substring(1, strdfcode2.IndexOf("]") - 1);
                    mode.dfkmcode2 = strdfcode2;
                }
                listmode.Add(mode);

            }
        }

        //if (listmode.Count == 0)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择对应科目。');", true);
        //    return;
        //}
        List<bill_yskm_dept> listmodeend = new List<bill_yskm_dept>();
        for (int i = 0; i < listmode.Count; i++)
        {
            string strkmcode = getFather(listmode[i].yskmCode);
            if (strkmcode != "")
            {
                strkmcode = strkmcode.Substring(0, strkmcode.Length - 2);//去掉最后一个","
            }
            else
            {
                strkmcode = "''";//如果返回值为空 则用''
            }
            string[] arrKm = strkmcode.Split(new string[] { "'," }, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < arrKm.Length; j++)
            {
                bill_yskm_dept modeend = new bill_yskm_dept();
                string strkm = arrKm[j].Substring(1, arrKm[j].Length - 1);
                if (strkm != listmode[i].yskmCode)
                {
                    bool boNoExit = true;
                    for (int k = 0; k < listmodeend.Count; k++)
                    {
                        if (listmodeend[k].yskmCode.Equals(strkm))
                        {
                            boNoExit = false;
                            break;
                        }
                    }
                    if (boNoExit)
                    {
                        modeend.yskmCode = strkm;
                        modeend.deptCode = listmode[0].deptCode;
                    }
                    else { continue; }
                }
                else
                {
                    modeend = listmode[i];
                }
                listmodeend.Add(modeend);
            }
        }

        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        //1.

        list.Add("delete from bill_yskm_dept where deptCode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "' and djlx='" + strdjlb + "'");
        //找出对应父级科目 如果没有子科目 把父级科目的对应也删除掉


        for (int i = 0; i < listmodeend.Count; i++)
        {
            list.Add("insert into bill_yskm_dept (deptCode,yskmCode,cwkmCode,jfkmcode1,dfkmcode1,jfkmcode2,dfkmcode2,djlx) values ('" + listmodeend[i].deptCode + "','" + listmodeend[i].yskmCode + "','','" + listmodeend[i].jfkmcode1 + "','" + listmodeend[i].dfkmcode1 + "','" + listmodeend[i].jfkmcode2 + "','" + listmodeend[i].dfkmcode2 + "','" + strdjlb + "')");
        }
        if (server.ExecuteNonQuerysArray(list) == -1)//执行list中的sql
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
        }
    }


    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            if (e.Item.Cells[9].Text == "01")
            {
                e.Item.Cells[9].Text = "单位填报";
            }
            else
            {
                e.Item.Cells[9].Text = "财务填报";
                e.Item.Cells[9].CssClass = "cwtb";
            }
            //如果科目不是末级科目
            if (e.Item.Cells[10].Text == "0")
            {
                e.Item.BackColor = Color.Silver;
                ((CheckBox)e.Item.FindControl("CheckBox1")).Enabled = false;
            }
            if (e.Item.Cells[11].Text == "1")
            {
                ((CheckBox)e.Item.FindControl("CheckBox1")).Checked = true;
            }
            #region 之前的程序实现方式
            //string yskmCode = e.Item.Cells[1].Text.Trim();
            //TextBox txtjfkmcode1 = e.Item.Cells[4].FindControl("txt_jfkmcode1") as TextBox;
            //if (txtjfkmcode1 != null)
            //{
            //    txtjfkmcode1.Text = GetCwkmString(yskmCode, "jfkmcode1");
            //}
            //TextBox txtdfkmcode1 = e.Item.Cells[5].FindControl("txt_dfkmcode1") as TextBox;
            //if (txtdfkmcode1 != null)
            //{
            //    txtdfkmcode1.Text = GetCwkmString(yskmCode, "dfkmcode1");
            //}
            //TextBox txtjfkmcode2 = e.Item.Cells[6].FindControl("txt_jfkmcode2") as TextBox;
            //if (txtjfkmcode2 != null)
            //{
            //    txtjfkmcode2.Text = GetCwkmString(yskmCode, "jfkmcode2");
            //}
            //TextBox txtdfkmcode2 = e.Item.Cells[7].FindControl("txt_dfkmcode2") as TextBox;
            //if (txtdfkmcode2 != null)
            //{
            //    txtdfkmcode2.Text = GetCwkmString(yskmCode, "dfkmcode2");
            //}
            #endregion
        }
    }


    /// <summary>
    /// 绑定决算单据类型
    /// </summary>
    public void binddjlx()
    {
        DataTable dtbill = server.GetDataTable("select diccode+'|'+isnull(note1,'') as val,dicname from bill_datadic where dictype='07' order by diccode", null);
        if (dtbill != null && dtbill.Rows.Count > 0)
        {
            this.ddlBill.DataSource = dtbill;
            this.ddlBill.DataTextField = "dicname";
            this.ddlBill.DataValueField = "val";
            this.ddlBill.DataBind();
            divdjlx.Visible = true;
        }
        else
        {
            divdjlx.Visible = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBill_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    public string getFather(string pCode)
    {
        string code = pCode;//勾选的科目code
        string tempStr = "";
        if (code.Length == 2)//最上级为两位
        {
            return "'" + code + "',";
        }
        else
        {
            int len = code.Length;
            while (len >= 4)//如果长度大于2则表示为子级
            {

                tempStr += "'" + code.Substring(0, len - 2) + "',";//为tempStr依次赋值
                code = code.Substring(0, code.Length - 2);//重新为code赋值
                len = code.Length;
            }
        }
        return tempStr + "'" + pCode + "',";//返回子级和父级
    }

    public void btn_Export_Click(object sender, EventArgs e)
    {

        string deptCode = Request.Params["deptCode"];
        string strsql = @"select a.deptCode,deptName,a.yskmCode,yskmMc
                            from bill_yskm_dept a
                            left join  bill_departments  b on a.deptCode =b.deptCode
                            left join bill_yskm c on a.yskmCode=c.yskmCode
                            where a.deptCode='"+deptCode+"'";
        DataTable dt = server.GetDataTable(strsql, null);

        Dictionary<string,string> dic = new Dictionary<string,string>();
        dic.Add("deptCode", "部门编号");
        dic.Add("deptName", "部门名称");
        dic.Add("yskmCode", "预算科目编号");
        dic.Add("yskmMc", "预算科目名称");
        new ExcelHelper().ExpExcel(dt, "ExportFile", dic);
       
    }

    #region 之前的实现方式  已注释
    //private string GetCwkmString(string stryskmCode, string strFile)
    //{
    //    string strdjlx ="02"; 
    //    if (!string.IsNullOrEmpty(ddlBill.SelectedValue.Split(new string[] { "|" }, StringSplitOptions.None)[0]))
    //    {
    //         strdjlx =ddlBill.SelectedValue.Split(new string[] { "|" }, StringSplitOptions.None)[0];
    //    }
    //    string deptCode = Request.Params["deptCode"];
    //    string strSql = " select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select " + strFile + " from bill_yskm_dept where deptCode='" + deptCode + "' and yskmCode='" + stryskmCode + "' and djlx='" + strdjlx + "')";
    //    object obj = server.ExecuteScalar(strSql);
    //    return obj == null ? "" : obj.ToString();
    //}
    #endregion
}