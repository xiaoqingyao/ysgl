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

public partial class webBill_yskm_deptyskmcwList : System.Web.UI.Page
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
                this.BindDataGrid();
            }
        }
    }

    public void BindDataGrid()
    {
        SysManager sysMgr = new SysManager();

        string deptCode = Request.Params["deptCode"];
        IList<Bill_Yskm> list;
        list = sysMgr.GetYskmAll();
        sysMgr.SetEndYsbm(list);
        this.myGrid.DataSource = list;
        myGrid.DataBind();

        string[] deptYskm = sysMgr.GetYskmCodeByDept(deptCode);//将预算科目code保存到数组
        for (int i = 0; i < myGrid.Items.Count; i++)//循环myGrid
        {
            var count = (from temp in deptYskm
                         where temp == myGrid.Items[i].Cells[1].Text
                         select temp).Count();//查询该部门是否已经对照了该科目
            if (myGrid.Items[i].Cells[4].Text != "")
            {
                string strjfcoed = server.GetCellValue("select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode='" + myGrid.Items[i].Cells[4].Text + "'");
                (myGrid.Items[i].FindControl("txt_jfkmcode1") as TextBox).Text = strjfcoed;
            }
            if (myGrid.Items[i].Cells[5].Text != "")
            {
                string strdfcoed = server.GetCellValue("select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode='" + myGrid.Items[i].Cells[5].Text + "'");
                (myGrid.Items[i].FindControl("txt_dfkmcode1") as TextBox).Text = strdfcoed;
            }


            if (count > 0 && myGrid.Items[i].Cells[10].Text == "1")//如果是并且为末级复选框勾选
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
            
            bill_yskm_dept mode = new bill_yskm_dept();
        
                string strjfcode1 = "";
                string strdfcode1 = "";
                string strjfcode2 = "";
                string strdfcode2 = "";

                mode.deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
                mode.yskmCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                if ((myGrid.Items[i].FindControl("txt_jfkmcode1") as TextBox).Text.Trim() != "")
                {
                    strjfcode1 = (myGrid.Items[i].FindControl("txt_jfkmcode1") as TextBox).Text.Trim();
                    strjfcode1 = strjfcode1.Substring(1, strjfcode1.IndexOf("]") - 1);
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
                if (mode.yskmCode!=""&&(mode.jfkmcode1!=""||mode.dfkmcode1!=""))
                {
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
        list.Add("delete from bill_yskm_dept where deptCode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");
        for (int i = 0; i < listmodeend.Count; i++)
        {
            list.Add("insert into bill_yskm_dept (deptCode,yskmCode,cwkmCode,jfkmcode1,dfkmcode1,jfkmcode2,dfkmcode2,kmdytype) values ('" + listmodeend[i].deptCode + "','" + listmodeend[i].yskmCode + "','','" + listmodeend[i].jfkmcode1 + "','" + listmodeend[i].dfkmcode1 + "','" + listmodeend[i].jfkmcode2 + "','" + listmodeend[i].dfkmcode2 + "','1')");
        }
        if (server.ExecuteNonQuerysArray(list) == -1)//执行list中的sql
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
        }
        //  System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        //  string tempStr = "";
        //  string tempFstr = "";
        //  string cstrtemp = "";
        //  string strjfcwcode1 = "";
        //  string strdfcwcode1 = "";
        ////  list.Add("delete from bill_yskm_dept where deptCode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");
        //  for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        //  {
        //      CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");

        //      if (chk.Checked)
        //      {
        //          strjfcwcode1 = (myGrid.Items[i].FindControl("txt_jfkmcode1") as TextBox).Text.Trim();
        //          strdfcwcode1 = (myGrid.Items[i].FindControl("txt_dfkmcode1") as TextBox).Text.Trim();

        //           tempFstr += this.getFather(this.myGrid.Items[i].Cells[1].Text.ToString().Trim());//获取父级预算科目code


        //          tempStr = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
        //          cstrtemp += "'" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim()+"','";

        //          if (strjfcwcode1 != "")
        //          {
        //              strjfcwcode1 = strjfcwcode1.Substring(1, strjfcwcode1.IndexOf("]") - 1);
        //          }
        //          if (strdfcwcode1 != "")
        //          {
        //              strdfcwcode1 = strdfcwcode1.Substring(1, strdfcwcode1.IndexOf("]") - 1);
        //          }

        //          if (tempFstr != "")
        //          {
        //              tempFstr = tempFstr.Substring(0, tempFstr.Length - 1);//去掉最后一个","
        //          }
        //          else
        //          {
        //              tempFstr = "''";//如果返回值为空 则用''
        //          }
        //          if (cstrtemp != "")
        //          {
        //              cstrtemp = cstrtemp.Substring(0, cstrtemp.Length - 1);//去掉最后一个","
        //          }
        //          else
        //          {
        //              cstrtemp = "''";//如果返回值为空 则用''
        //          }

        //            list.Add("delete from bill_yskm_dept where deptCode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");

        //            list.Add("insert into bill_yskm_dept select '" + Page.Request.QueryString["deptCode"].ToString().Trim() + "',yskmCode,'','" + strjfcwcode1 + "','" + strdfcwcode1 + "','','' from bill_yskm where yskmCode in (" + tempStr + ")");

        //      }

        //  }
        //  //if (tempStr != "")
        //  //{
        //  //    tempStr = tempStr.Substring(0, tempStr.Length - 1);//去掉最后一个","
        //  //}
        //  //else {
        //  //    tempStr = "''";//如果返回值为空 则用''
        //  //}
        // // list.Add("delete from bill_yskm_dept where deptCode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "' and yskmCode not in (" + tempStr + ")");//删除掉本部门下没有选择的科目
        //  list.Add("insert into bill_yskm_dept select '" + Page.Request.QueryString["deptCode"].ToString().Trim() + "',yskmCode,'','','','','' from bill_yskm where yskmCode in (" + tempFstr + ") and yskmCode not in(" + cstrtemp + ") and yskmCode not in (select yskmCode from bill_yskm_dept where deptCode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "')");

        //添加勾选复选框的科目
        //if (server.ExecuteNonQuerysArray(list) == -1)//执行list中的sql
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        //}
        //else
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
        //}
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
            if (e.Item.Cells[9].Text == "01")
            {
                e.Item.Cells[9].Text = "单位填报";
            }
            else
            {
                e.Item.Cells[9].Text = "财务填报";
                e.Item.Cells[9].CssClass = "cwtb";
            }

            if (e.Item.Cells[10].Text == "0")
            {
                e.Item.BackColor = Color.Silver;
                ((CheckBox)e.Item.FindControl("CheckBox1")).Enabled = false;
            }
            string yskmCode = e.Item.Cells[1].Text.Trim();
            TextBox txtjfkmcode1 = e.Item.Cells[4].FindControl("txt_jfkmcode1") as TextBox;
            if (txtjfkmcode1 != null)
            {
                txtjfkmcode1.Text = GetCwkmString(yskmCode, "jfkmcode1");
            }
            TextBox txtdfkmcode1 = e.Item.Cells[5].FindControl("txt_dfkmcode1") as TextBox;
            if (txtdfkmcode1 != null)
            {
                txtdfkmcode1.Text = GetCwkmString(yskmCode, "dfkmcode1");
            }
            TextBox txtjfkmcode2 = e.Item.Cells[6].FindControl("txt_jfkmcode2") as TextBox;
            if (txtjfkmcode2 != null)
            {
                txtjfkmcode2.Text = GetCwkmString(yskmCode, "jfkmcode2");
            }
            TextBox txtdfkmcode2 = e.Item.Cells[7].FindControl("txt_dfkmcode2") as TextBox;
            if (txtdfkmcode2 != null)
            {
                txtdfkmcode2.Text = GetCwkmString(yskmCode, "dfkmcode2");
            }
        }
    }

    private string GetCwkmString(string stryskmCode, string strFile)
    {
        string deptCode = Request.Params["deptCode"];
        string strSql = " select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select " + strFile + " from bill_yskm_dept where deptCode='" + deptCode + "' and yskmCode='" + stryskmCode + "')";
        object obj = server.ExecuteScalar(strSql);
        return obj == null ? "" : obj.ToString();
    }
}
