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
using Bll.SaleBill;
using Bll.Sepecial;
using System.Collections.Generic;
using Models;
using System.Text;
using Bll;

public partial class SaleBill_Flsz_RebatesStandardlist : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    RebatesStandardBLL restandbll = new RebatesStandardBLL();
    T_SaleFeeAllocationNote spnotemodel = new T_SaleFeeAllocationNote();
    T_SaleFeeAllocationNoteBLL spnotebill = new T_SaleFeeAllocationNoteBLL();
    string strcartype = "";
    string strdeptcode = "";
    string strfeecode = "";
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
                DataDicList();
            }
        }
        this.txtbgtime.Attributes.Add("onfocus", "javascript:setday(this);");
        this.txtedtime.Attributes.Add("onfocus", "javascript:setday(this);");
        ClientScript.RegisterArrayDeclaration("availableTags", GetcarAll());
        ClientScript.RegisterArrayDeclaration("availableTagsdt", GetdetpAll());
        ClientScript.RegisterArrayDeclaration("availableTagsfy", GetdefyAll());

        ClientScript.RegisterArrayDeclaration("availablekz", getkz());

    }

    protected void DataDicList()
    {
        T_RebatesStandard redsmodel = new T_RebatesStandard();

        if (this.txtcartype.Text != null && this.txtcartype.Text != "")
        {
            string strc = this.txtcartype.Text.Trim();
            strc = strc.Substring(1, strc.IndexOf("]") - 1);
            redsmodel.TruckTypeCode = strc;
        }
        if (txtdept.Text != null && txtdept.Text != "")
        {
            string strd = txtdept.Text;
            strd = strd.Substring(1, strd.IndexOf("]") - 1);

            redsmodel.DeptCode = strd;
        }
        if (txtfeetype.Text != null && txtfeetype.Text != "")
        {
            string strf = txtfeetype.Text.ToString().Trim();
            strf = strf.Substring(1, strf.IndexOf("]") - 1);

            redsmodel.SaleFeeTypeCode = strf;
        }
        if (txtfeekz.Text != null && txtfeekz.Text != "")//费用控制
        {
           
            string strkz= txtfeekz.Text.ToString();
            strkz=strkz.Substring(1,strkz.IndexOf("]")-1);
            redsmodel.SaleProcessCode = strkz;
        }
        if (txttype.SelectedValue.ToString() != null && txttype.SelectedValue.ToString() != "")
        {
                redsmodel.Type = txttype.SelectedValue.ToString();
        }
        if (txtstatus.SelectedValue.ToString() != null && txtstatus.SelectedValue.ToString() != "")
        {
                redsmodel.Status =txtstatus.SelectedValue.ToString();

        }
        if (txtbgtime.Text != null && txtbgtime.Text != "")
        {
            redsmodel.EffectiveDateFrm = txtbgtime.Text.Trim();
        }
        if (txtedtime.Text != null && txtedtime.Text != "")
        {
            redsmodel.EffectiveDateTo = txtedtime.Text;
        }
        DataTable dt = restandbll.getalltable(redsmodel);

        #region 计算分页相关数据1
        this.lblPageSize.Text = this.myGrid.PageSize.ToString();
        this.lblItemCount.Text = dt.Rows.Count.ToString();
        double pageCountDouble = double.Parse(this.lblItemCount.Text) / double.Parse(this.lblPageSize.Text);
        int pageCount = Convert.ToInt32(Math.Ceiling(pageCountDouble));
        this.lblPageCount.Text = pageCount.ToString();
        this.drpPageIndex.Items.Clear();
        for (int i = 0; i <= pageCount - 1; i++)
        {
            int pIndex = i + 1;
            ListItem li = new ListItem(pIndex.ToString(), pIndex.ToString());
            if (pIndex == this.myGrid.CurrentPageIndex + 1)
            {
                li.Selected = true;
            }
            this.drpPageIndex.Items.Add(li);
        }
        this.showStatus();
        #endregion
        if (dt.Rows.Count == 0)
        {
            dt = null;
        }

        myGrid.DataSource = dt;
        myGrid.DataBind();
    }

    /// <summary>
    /// 车辆类型
    /// </summary>
    /// <returns></returns>
    private string GetcarAll()
    {
        string script = "";
        DataSet ds = server.GetDataSet("select '['+CAST(typeCode AS varchar(100)) +']'+typeName as kemu from  T_truckType");
        StringBuilder arry = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                arry.Append("'");
                arry.Append(Convert.ToString(dr["kemu"]));
                arry.Append("',");
            }

            script = arry.ToString().Substring(0, arry.Length - 1);

        }

        return script;

    }

    /// <summary>
    /// 费用控制
    /// </summary>
    /// <returns></returns>
    private string getkz()
    {
        string script = "";
        DataSet ds = server.GetDataSet(@"select '['+Code+']'+PName as EName from dbo.T_SaleProcess union
                select '['+Code+']'+CName as EName from dbo.T_ControlItem union
                select '[期初分配]期初分配'  as EName");
        StringBuilder arry = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                arry.Append("'");
                arry.Append(Convert.ToString(dr["EName"]));
                arry.Append("',");
            }

            script = arry.ToString().Substring(0, arry.Length - 1);

        }

        return script;

    }
    /// <summary>
    /// 部门
    /// </summary>
    /// <returns></returns>
    private string GetdetpAll()
    {
        DataSet ds = server.GetDataSet("select deptCode, '['+deptCode+']'+deptName as dtname from  bill_departments where IsSell='Y'");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dtname"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        return script;
    }

    /// <summary>
    /// 费用类别
    /// </summary>
    /// <returns></returns>
    private string GetdefyAll()
    {


        string strSql = "select '['+yskmCode+']'+yskmMC as kmMc from Bill_Yskm where yskmcode in(select yskmcode from bill_yskm_dept where 1=1)";

        if (!strdeptcode.Equals(""))
        {
            // strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);
            strSql += " and deptCode='" + strdeptcode + "'";
        }
        DataSet ds = server.GetDataSet(strSql);
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["kmMc"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }


    }


    #region showStatus 分页相关
    void showStatus()
    {
        if (this.drpPageIndex.Items.Count == 0)
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
            return;
        }
        if (int.Parse(this.lblPageCount.Text) == int.Parse(this.drpPageIndex.SelectedItem.Value))//最后一页
        {
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
        }
        else
        {
            this.lBtnNextPage.Enabled = true;
            this.lBtnLastPage.Enabled = true;
        }
        if (int.Parse(this.drpPageIndex.SelectedItem.Value) == 1)//第一页
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
        }
        else
        {
            this.lBtnFirstPage.Enabled = true;
            this.lBtnPrePage.Enabled = true;
        }
    }

    protected void lBtnFirstPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = 0;
        this.DataDicList();
    }
    protected void lBtnPrePage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex - 1;
        this.DataDicList();
    }
    protected void lBtnNextPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex + 1;
        this.DataDicList();
    }
    protected void lBtnLastPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.PageCount - 1;
        this.DataDicList();
    }
    protected void drpPageIndex_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = int.Parse(this.drpPageIndex.SelectedItem.Value) - 1;
        this.DataDicList();
    }
    #endregion

    #region 修改
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        string diccode = "";
        int count = 0;
        string stepID_ID = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            string strstatus = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
            if (chk.Checked == true)
            {
                if (strstatus == "已批复")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该记录已经确认完成不能修改！');", true);
                    return;
                }
                else
                {
                    diccode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                    count += 1;
                }

            }
        }

        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多条数据！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择未批复的记录！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetailedit('RebatesStandardedit.aspx?type=edit&diccode=" + diccode + "');", true);
        }
    }
    #endregion

    #region 删除
    protected void btn_del_Click(object sender, EventArgs e)
    {
        string diccode = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            string strstatus = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
            if (chk.Checked == true)
            {
                if (strstatus == "已批复")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择未批复的记录进行删除！');", true);
                    return;
                }
                else
                {
                    diccode += "'" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "',";
                    count += 1;
                }


            }
        }
        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择要删除的数据！');", true);
        }
        else
        {
            diccode = diccode.Substring(0, diccode.Length - 1);

            System.Collections.Generic.List<string> list = new List<string>();

            list.Add("delete from T_RebatesStandard where NID in (" + diccode + ")");
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {

                //同时删除note表里的记录

                int intad = 0;
                string strlb = "";
                spnotemodel = new T_SaleFeeAllocationNote();
                for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
                {
                    CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
                    if (chk.Checked == true)
                    {
                        intad += 1;
                        if (this.myGrid.Items[i].Cells[7].Text.ToString().Trim() == "期初分配")
                        {

                            spnotemodel.TruckTypeCode = this.myGrid.Items[i].Cells[13].Text.ToString().Trim();
                            spnotemodel.DeptCode = this.myGrid.Items[i].Cells[14].Text.ToString().Trim();
                            spnotemodel.SaleFeeTypeCode = this.myGrid.Items[i].Cells[15].Text.ToString().Trim();


                            int row = spnotebill.Del(spnotemodel);
                            if (row > -1)
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
                            }
                        }
                    }


                }

                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);



                this.DataDicList();
            }
        }

    }
    #endregion

    #region 查询
    protected void btn_sel_Click(object sender, EventArgs e)
    {
        DataDicList();
    }
    #endregion
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_add_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('NormalRebateStandard.aspx');", true);

    }
    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        T_RebatesStandard redsmodel = new T_RebatesStandard();

        if (this.txtcartype.Text != null && this.txtcartype.Text != "")
        {
            string strc = this.txtcartype.Text.Trim();
            strc = strc.Substring(1, strc.IndexOf("]") - 1);
            redsmodel.TruckTypeCode = strc;
        }
        if (txtdept.Text != null && txtdept.Text != "")
        {
            string strd = txtdept.Text;
            strd = strd.Substring(1, strd.IndexOf("]") - 1);

            redsmodel.DeptCode = strd;
        }
        if (txtfeetype.Text != null && txtfeetype.Text != "")
        {
            string strf = txtfeetype.Text.ToString().Trim();
            strf = strf.Substring(1, strf.IndexOf("]") - 1);

            redsmodel.SaleFeeTypeCode = strf;
        }
        if (txtfeekz.Text != null && txtfeekz.Text != "")//费用控制
        {

            string strkz = txtfeekz.Text.ToString();
            strkz = strkz.Substring(1, strkz.IndexOf("]") - 1);
            redsmodel.SaleProcessCode = strkz;
        }
        if (txttype.SelectedValue.ToString() != null && txttype.SelectedValue.ToString() != "")
        {
            redsmodel.Type = txttype.SelectedValue.ToString();
        }
        if (txtstatus.SelectedValue.ToString() != null && txtstatus.SelectedValue.ToString() != "")
        {
            redsmodel.Status = txtstatus.SelectedValue.ToString();

        }
        if (txtbgtime.Text != null && txtbgtime.Text != "")
        {
            redsmodel.EffectiveDateFrm = txtbgtime.Text.Trim();
        }
        if (txtedtime.Text != null && txtedtime.Text != "")
        {
            redsmodel.EffectiveDateTo = txtedtime.Text;
        }
        DataTable dt = restandbll.getalltable(redsmodel);
        DataTableToExcel(dt, this.myGrid, null);
    }
    /// <summary>
    /// 批复
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_pf_Click(object sender, EventArgs e)
    {
        string diccode = "";
        int count = 0;

        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            string strstatus = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
            if (chk.Checked == true)
            {
                if (strstatus == "已批复")
                {

                    continue;
                }
                else
                {
                    diccode += "'" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "',";
                    count += 1;
                }


            }


        }
        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择记录！');", true);
            return;
        }
        else
        {
            diccode = diccode.Substring(0, diccode.Length - 1);

            System.Collections.Generic.List<string> list = new List<string>();
            //批复
            list.Add("update T_RebatesStandard set Status='2', AuditUserCode='" + Session["userCode"].ToString().Trim() + "' where NID in (" + diccode + ")");

            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('批复失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('批复完成！');", true);
                //判断类别是期初分配的
                //向note表里面加字段

                spnotemodel.ActionDate = DateTime.Now.ToString("yyyy-MM-dd");
                spnotemodel.ActionTimes = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                spnotemodel.AuditUserCode = Session["userCode"].ToString().Trim();
                int intad = 0;
                string strlb = "";
                for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
                {
                    CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
                    if (chk.Checked == true)
                    {
                        intad += 1;
                        string strstatus = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
                      //  string strfee = this.myGrid.Items[i].Cells[6].Text.ToString().Trim();
                        if (this.myGrid.Items[i].Cells[9].Text.ToString().Trim() == "期初分配")
                        {
                            if (strstatus == "已批复")
                            {

                                continue;
                            }
                            else
                            {
                                spnotemodel.ActionNote = "期初分配";
                                spnotemodel.Status = "1";
                                spnotemodel.RebatesType = "0";
                                spnotemodel.TruckTypeCode = this.myGrid.Items[i].Cells[15].Text.ToString().Trim();
                                spnotemodel.DeptCode = this.myGrid.Items[i].Cells[16].Text.ToString().Trim();
                                spnotemodel.SaleFeeTypeCode = this.myGrid.Items[i].Cells[17].Text.ToString().Trim();

                                if (this.myGrid.Items[i].Cells[6].Text.ToString().Trim() != "" && this.myGrid.Items[i].Cells[6].Text.ToString().Trim() != null)
                                {
                                    spnotemodel.Fee = decimal.Parse(this.myGrid.Items[i].Cells[6].Text.ToString().Trim());
                                }
                                else
                                {
                                    continue;
                                }
                                int row = spnotebill.Add(spnotemodel);
                                if (row > 0)
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败！');", true);
                                }
                            }
                           
                        }
                    }


                }

                this.DataDicList();
            }
        }
    }
    protected void btn_sx_Click(object sender, EventArgs e)
    {
        DataDicList();
    }
    protected void bt_jy_Click(object sender, EventArgs e)
    {
        string diccode = "";
        int count = 0;
        string stepID_ID = "";
        System.Collections.Generic.List<string> list = new List<string>();
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            string strstatus = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
            if (chk.Checked == true)
            {
                if (strstatus != "已批复")
                {
                  //  ClientScript.RegisterStartupScript(this.GetType(), "", "alert('选择未批复的记录操作！');", true);
                    continue;
                }
                else
                {
                    diccode += "'"+this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "',";
                    count += 1;
                }

            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择已批复的记录！');", true);
        }
        else
        {
            diccode = diccode.Substring(0, diccode.Length - 1);
            list.Add("update  T_RebatesStandard set Status='0' where NID in (" + diccode + ")");
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('禁用失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('禁用成功！');", true);
                DataDicList();
            }
        }
    }
    protected void bt_qy_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new List<string>();
        string diccode = "";
        int count = 0;
        string stepID_ID = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            string strstatus = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
            if (chk.Checked == true)
            {
                if (strstatus != "禁用")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('选择禁用的记录操作！');", true);
                    continue;
                }
                else
                {
                    diccode += "'" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "',";
                    count += 1;
                }

            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择禁用的记录！');", true);
        }
        else
        {
            diccode = diccode.Substring(0, diccode.Length - 1);
            list.Add("update  T_RebatesStandard set Status='2' where NID in (" + diccode + ")");
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('启用失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('启用成功！');", true);
                DataDicList();
            }
        }
    }

    public delegate void MyDelegate(DataGrid gv);
    protected void DataTableToExcel(DataTable dtData, DataGrid stylegv, MyDelegate rowbound)
    {
        if (dtData != null)
        {
            // 设置编码和附件格式
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.Charset = "utf-8";

            // 导出excel文件
            // IO用于导出并返回excel文件
            System.IO.StringWriter strWriter = new System.IO.StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(strWriter);

            DataGrid gvExport = new DataGrid();


            gvExport.AutoGenerateColumns = false;
            BoundColumn bndColumn = new BoundColumn();
            for (int j = 0; j < stylegv.Columns.Count - 1; j++)
            {
                bndColumn = new BoundColumn();
                if (stylegv.Columns[j] is BoundColumn)
                {
                    bndColumn.DataField = ((BoundColumn)stylegv.Columns[j]).DataField.ToString();
                    bndColumn.HeaderText = ((BoundColumn)stylegv.Columns[j]).HeaderText.ToString();

                    //添加一列
                    gvExport.Columns.Add(bndColumn);
                }
            }
            gvExport.DataSource = dtData.DefaultView;
            gvExport.AllowPaging = false;
            gvExport.DataBind();
            if (rowbound != null)
            {
                rowbound(gvExport);
            }

            // 返回客户端
            gvExport.RenderControl(htmlWriter);
            Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\">");
            Response.Write(strWriter.ToString());
            Response.Write("</body></html>");
            Response.End();
        }
    }
}
