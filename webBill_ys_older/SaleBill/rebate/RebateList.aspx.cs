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
using System.Collections.Generic;
using System.Text;
using System.IO;
public partial class SaleBill_rebate_RebateList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    private string strFlg="";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            this.txb_sqrqbegin.Attributes.Add("onfocus", "javascript:setday(this);");
            this.txb_sqrqend.Attributes.Add("onfocus", "javascript:setday(this);");
            ClientScript.RegisterArrayDeclaration("availableTags", GetcarAll());
            ClientScript.RegisterArrayDeclaration("availableTagsdt", GetdetpAll());
            ClientScript.RegisterArrayDeclaration("availableTagsfy", GetdefyAll());
            object objCtrl=Request["Ctrl"];
            if (objCtrl!=null)
	        {
        		 strFlg=objCtrl.ToString();
	        }
            object objDateFrm=Request["DateFrm"];
            if (objDateFrm!=null)
	        {
        		 this.txb_sqrqbegin.Text=objDateFrm.ToString();
	        }
            object objDateTo=Request["DateTo"];
            if (objDateTo!=null)
	        {
                txb_sqrqend.Text=objDateTo.ToString();    		 
	        }
            object objDeptCode=Request["DeptCode"];
            if (objDeptCode!=null)
	        {
                txtdept.Text=objDeptCode.ToString();    		 
	        }
            object objkmCode=Request["kmCode"];
            if (objkmCode!=null)
	        {
                txtfeename.Text=objkmCode.ToString();    		 
	        }
            if (!IsPostBack)
            {
                BindDataGrid();
            }
        }
    }
    /// <summary>
    /// 绑定GridView
    /// </summary>
    protected void BindDataGrid()
    {
        //控制按钮
        if (strFlg.Equals("Select"))
        {
            btn_edit.Visible = btn_delete.Visible = false;
            this.txtdept.Enabled = false;
        }
        string sql = getSelectSql();
        DataSet temp = server.GetDataSet(sql);
        #region 计算分页相关数据1
        this.lblPageSize.Text = this.myGrid.PageSize.ToString();
        this.lblItemCount.Text = temp.Tables[0].Rows.Count.ToString();
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
        if (temp.Tables[0].Rows.Count== 0)
        {
            temp = null;
        }

        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
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
    /// 部门
    /// </summary>
    /// <returns></returns>
    private string GetdetpAll()
    {
        DataSet ds = server.GetDataSet("select deptCode, '['+deptCode+']'+deptName as dtname from  bill_departments  where IsSell='Y'");
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
   
    protected void btn_sel_Click(object sender, EventArgs e)
    {
        BindDataGrid();
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
        this.BindDataGrid();
    }
    protected void lBtnPrePage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex - 1;
        this.BindDataGrid();
    }
    protected void lBtnNextPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex + 1;
        this.BindDataGrid();
    }
    protected void lBtnLastPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.PageCount - 1;
        this.BindDataGrid();
    }
    protected void drpPageIndex_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = int.Parse(this.drpPageIndex.SelectedItem.Value) - 1;
        this.BindDataGrid();
    }
    #endregion
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        string diccode = "";
        int count = 0;

        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            string strstatus = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
            if (chk.Checked == true)
            {
                //if (strstatus == "已批复")
                //{

                //    continue;
                //}
                //else
                //{
                    diccode += "'" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "',";
                    count += 1;
                //}


            }
        }
        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择未确认的数据删除！');", true);
        }

        else
        {
            diccode = diccode.Substring(0, diccode.Length - 1);

            System.Collections.Generic.List<string> list = new List<string>();
            //删除
            string strdelesmg = "["+Session["userCode"].ToString()+"]" + Session["userName"].ToString() + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"删除该记录";
            list.Add("update T_SaleFeeAllocationNote set Status='D',ActionNote='" + strdelesmg + "' where Nid in (" + diccode + ")");

            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除完成！');", true);
                this.BindDataGrid();
            }
        }
    }
    /// <summary>
    /// 批复
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btn_pf_Click(object sender, EventArgs e)
    //{
    //    string diccode = "";
    //    int count = 0;

    //    for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
    //    {
    //        CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
    //        string strstatus = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
    //        if (chk.Checked == true)
    //        {
    //            if (strstatus == "已批复")
    //            {

    //                continue;
    //            }
    //            else
    //            {
    //                diccode += "'" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "',";
    //                count += 1;
    //            }


    //        }
    //    }
    //    if (count == 0)
    //    {
    //        return;
    //    }

    //    else
    //    {
    //        diccode = diccode.Substring(0, diccode.Length - 1);

    //        System.Collections.Generic.List<string> list = new List<string>();
    //        //批复
    //        string strpfsmg = "[" + Session["userCode"].ToString() + "]" + Session["userName"].ToString() + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "批复该记录";

    //        list.Add("update T_SaleFeeAllocationNote set Status='1',RebatesType='0',ActionNote='" + strpfsmg + "', AuditUserCode='" + Session["userCode"].ToString().Trim() + "' where Nid in (" + diccode + ")");

    //        if (server.ExecuteNonQuerysArray(list) == -1)
    //        {
    //            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('批复失败！');", true);
    //        }
    //        else
    //        {
    //            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('批复完成！');", true);
    //            this.BindDataGrid();
    //        }
    //    }
    //}
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        string diccode = "";
        int count = 0;
        string stepID_ID = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            //string strstatus = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
            if (chk.Checked == true)
            {
                //if (strstatus == "已批复")
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该记录已经确认完成不能修改！');", true);
                //    return;
                //}
                //else
                //{
                    diccode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                    count += 1;
                //}

            }
        }

        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择一条数据进行修改！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择修改记录！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('RebateDetail.aspx?type=edit&diccode=" + diccode + "');", true);
        }
    }
    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        string sql = getSelectSql();
        DataTable dtExport = new DataTable();
        dtExport = server.GetDataSet(sql).Tables[0];
        DataTableToExcel(dtExport, this.myGrid, null);
    }

    private string getSelectSql()
    {
        string sql = @"select a.*,(select '['+deptCode+']'+ deptName from bill_departments where deptCode=a.DeptCode)as deptName,
                (select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode= a.SaleFeeTypeCode )as feename,
                (case Status when '0' then '未生效' when '1' then '正常' else '删除' end)as astatus,
                (case RebatesType when '0' then '期初分配' when '1' then '销售提成' when '2' then '配置项' end) as rtype,
                (case ControlItemCode  when (select Code from T_SaleProcess where Code=a.ControlItemCode) 
                 then (select  '['+Code+']'+PName from T_SaleProcess where Code=a.ControlItemCode) 
                when (select Code from  T_ControlItem where Code=a.ControlItemCode)
                 then (select'['+Code+']'+ CName from  T_ControlItem where Code=a.ControlItemCode)
                end)as feekz,
				(select '['+userCode+']'+userName from bill_users where userCode=a.AuditUserCode)as username,
				(select '['+typeCode+']'+typeName  from T_truckType where typeCode= a.TruckTypeCode)as trucktypename
                from T_SaleFeeAllocationNote a where 1=1 ";
        #region 查询条件
        //开始日期

        if (!this.txb_sqrqbegin.Text.Trim().Equals(""))
        {
            sql += " and ActionDate>='" + this.txb_sqrqbegin.Text.Trim() + "'";
        } if (this.txb_sqrqbegin.Text.Trim().Equals(""))
        {
            sql += " and ActionDate>='" + this.txb_sqrqbegin.Text.Trim() + "'";
        }
        if (this.txtdept.Text.Trim() != null && this.txtdept.Text.Trim() != "")
        {
            string strdept = this.txtdept.Text.Trim();
            strdept = strdept.Substring(1, strdept.IndexOf("]") - 1).Trim();
            sql += " and DeptCode='" + strdept + "'";
        }
        if (this.txtTruckCode.Text.Trim() != null && this.txtTruckCode.Text.Trim() != "")
        {
            sql += " and TruckCode like'%" + this.txtTruckCode.Text.Trim() + "%'";
        }
        if (this.txtcartype.Text.Trim() != null && this.txtcartype.Text.Trim() != "")
        {
            string strcartype = this.txtcartype.Text.Trim();
            strcartype = strcartype.Substring(1, strcartype.IndexOf("]") - 1).Trim();
            sql += " and TruckTypeCode='" + strcartype + "'";
        }
        if (this.txtfeename.Text.Trim() != null && this.txtfeename.Text.Trim() != "")
        {
            string strfeename = this.txtfeename.Text.Trim();
            strfeename = strfeename.Substring(1, strfeename.IndexOf("]") - 1).Trim();
            sql += " and SaleFeeTypeCode='" + strfeename + "'";
        }
        if (txttype.SelectedValue.ToString() != null && txttype.SelectedValue.ToString() != "")
        {
            sql += " and RebatesType='" + txttype.SelectedValue.ToString() + "'";
        }
        if (txtstatus.SelectedValue.ToString() != null && txtstatus.SelectedValue.ToString() != "")
        {
            sql += " and Status='" + txtstatus.SelectedValue.ToString() + "'";

        }
        #endregion
        sql += " order by ActionTimes desc";
        return sql;
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
            StringWriter strWriter = new StringWriter();
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
    float fSumAmount = 0;
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        //显示合计行
        if (e.Item.ItemType != ListItemType.Footer)
        {
            float feveAmount = 0;
            bool bofeveAmount = float.TryParse(e.Item.Cells[8].Text.Trim(), out feveAmount);
            fSumAmount += feveAmount;
        }
        else
        {
            e.Item.Cells[0].Text = "合计：";
            e.Item.Cells[8].Text = fSumAmount.ToString();
        }
    }
}
