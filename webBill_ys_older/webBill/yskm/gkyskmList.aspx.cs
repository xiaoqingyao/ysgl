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
using Bll.UserProperty;
using System.Collections.Generic;
using Models;
using System.Drawing;

public partial class webBill_yskm_gkyskmList : System.Web.UI.Page
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
                this.BindDataGrid();
            }
        }
    }

    public void BindDataGrid()
    {
        SysManager sysMgr = new SysManager();

        string deptCode = Request.Params["deptCode"];
        IList<Bill_Yskm> list;
        list = sysMgr.GetGkYskmAll();
        sysMgr.SetEndYsbm(list);
        this.myGrid.DataSource = list;
        myGrid.DataBind();

        string[] deptYskm = sysMgr.GetGkYskmCodeByDept(deptCode);//将预算科目code保存到数组
        for (int i = 0; i < myGrid.Items.Count; i++)//循环myGrid
        {
            var count = (from temp in deptYskm
                         where temp == myGrid.Items[i].Cells[1].Text
                         select temp).Count();//查询该部门是否已经对照了该科目


            if (count > 0 && myGrid.Items[i].Cells[4].Text == "1")//如果是并且为末级复选框勾选
            {
                ((CheckBox)myGrid.Items[i].FindControl("CheckBox1")).Checked = true;
            }
        }
    }


    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        List<bill_yskm_dept> listmode = new List<bill_yskm_dept>();
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            bill_yskm_dept mode = new bill_yskm_dept();
            if (chk.Checked)
            {
                mode.deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
                mode.yskmCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                listmode.Add(mode);
            }
        }
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
        list.Add("delete from bill_yskm_gkdept where deptCode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");
        for (int i = 0; i < listmodeend.Count; i++)
        {
            list.Add("insert into bill_yskm_gkdept (deptCode,yskmCode) values ('" + listmodeend[i].deptCode + "','" + listmodeend[i].yskmCode + "')");
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
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            if (e.Item.Cells[4].Text == "0")
            {
                e.Item.BackColor = Color.Silver;
                ((CheckBox)e.Item.FindControl("CheckBox1")).Enabled = false;
            }
        }
    }
}
