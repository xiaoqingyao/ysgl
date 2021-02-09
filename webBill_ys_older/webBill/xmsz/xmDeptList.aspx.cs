using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using System.Data;
using Bll.UserProperty;
using System.IO;
using System.Data.OleDb;


public partial class webBill_xmsz_xmDeptList :BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    Dal.Bills.XmDeptNdDal dal = new Dal.Bills.XmDeptNdDal();
    Dal.newysgl.Xmlr Xmlrdal = new Dal.newysgl.Xmlr();
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

                if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
                {

                    btn_Copy.Disabled = true;
                }
                List<string> ndlist = Xmlrdal.GetNdByxmLrb("1");
                if (ndlist.Count > 0)
                {
                    drpNd.DataSource = ndlist;
                    drpNd.DataBind();
                }
                this.BindDataGrid();
            }
        }
    }

 
    public void BindDataGrid()
    {


        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 140);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>
        DataTable dtrel = GetData(arrpage[0], arrpage[1], out icount);
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount == 0 ? 1 : icount;
        //----------给gridview赋值
        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();
        RowsBound();
    }

    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string nd = "";
        if (!string.IsNullOrEmpty(drpNd.SelectedValue))
        {
            nd = Convert.ToString(drpNd.SelectedValue);
        }
        string sql = "select Row_Number()over(order by xmCode) as crow,(case isnull(status,'0') when '1' then '正常' when '0' then '停用' end) as status,nd,(select top 1 '['+xmCode+']'+xmName from bill_xm where xmCode = a.xmCode) as xmCode,(select  top 1 '['+deptcode+']'+deptname from bill_departments where deptcode=a.xmdept) as xmDept ,je,(case isnull(isCtrl,'0') when '1' then '是' when '0' then '否' end) as isCtrl from bill_xm_dept_nd  as a  where 1=1  and  a.nd='" + nd + "'  ";
        string deptCode = "";
        string deptCodes = "";
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {

            deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and xmdept in (" + deptCodes + ")";
            deptCode = "000001";
        }
        else
        {
            deptCodes = Page.Request.QueryString["deptCode"].ToString().Trim();
            sql += " and xmdept in ('" + deptCodes + "')";
            deptCode = deptCodes;
        }


        sql += " and xmCode not in (select xmCode from bill_xm where xmDept='" + deptCode + "' and xmStatus='0')";
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));
        if (count <= 0)
        {
            dal.NullListMaker(this.drpNd.SelectedValue, deptCode);
        }
        
        
        if (txb_where.Text != "")
        {
            sql += " and (xmname like '%" + txb_where.Text + "%' or xmname like '%" + txb_where.Text + "%')";
        }
       
        //同步数据     
        if (dal.IsNewXm(this.drpNd.SelectedValue, deptCode))
        {
            dal.NullListMakerNew(this.drpNd.SelectedValue, deptCode);
        }


        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
        
       // return dal.GetDataAll(this.drpNd.SelectedValue, deptCode, pagefrm, pageto, out  count);
    }



    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }


    protected void drpNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    



    private void RowsBound()
    {
        IList<bill_xm_dept_nd> xmfj = dal.GetXmfj(drpNd.SelectedValue);
        int iItemCount = myGrid.Items.Count;
        double fTotalAmount = 0;
        for (int i = 0; i < iItemCount; i++)
        {
            string xmcode = SubString((myGrid.Items[i].FindControl("hfxmCode") as HiddenField).Value);
            string xmdept = SubString((myGrid.Items[i].FindControl("hfxmDept") as HiddenField).Value);
            //isCtrl 
            string istx = ((myGrid.Items[i].FindControl("hiddistx") as HiddenField).Value)=="是"?"1":"0";

            string xmStatus = ((myGrid.Items[i].FindControl("hfStatus") as HiddenField).Value) == "正常" ? "1" : "0";

            CheckBox cb = myGrid.Items[i].FindControl("CheckBox1") as CheckBox;
          


            DropDownList ddl = myGrid.Items[i].FindControl("ddlIsCtrl") as DropDownList;

            TextBox je = myGrid.Items[i].FindControl("txt_je") as TextBox;
            var temp = from p in xmfj
                       where p.xmCode == xmcode && p.xmDept == xmdept && p.nd == drpNd.SelectedValue
                       select p;


            if (xmStatus=="1")
            {
                cb.Checked = true;
            }
            ddl.SelectedValue = istx;

            if (temp.Count() > 0)
            {
                je.Text = Convert.ToDecimal(temp.First().je).ToString("N02");
            }
            //#region 合计行
            //double flAmount = 0;
            //if (!string.IsNullOrEmpty(je.Text) && double.TryParse(je.Text, out flAmount))
            //{
            //    fTotalAmount += flAmount;
            //}
            //#endregion
        }
        //Table t = (Table)myGrid.Controls[0];
        //DataGridItem item = (DataGridItem)t.Rows[t.Rows.Count - 1];
        //Label control = item.FindControl("lbeTotalAmount") as Label;
        //if (control != null)
        //{
        //    fTotalAmount = Math.Round(fTotalAmount, 2);
        //    control.Text = fTotalAmount.ToString("N02");
        //}
    }

    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        IList<bill_xm_dept_nd> xmdeptList = new List<bill_xm_dept_nd>();
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            string istx = (myGrid.Items[i].FindControl("hiddistx") as HiddenField).Value;
            string xmcode = (myGrid.Items[i].FindControl("hfxmCode") as HiddenField).Value;
            string xmdept = Page.Request.QueryString["deptCode"].ToString().Trim();
            // ClientScript.RegisterStartupScript(this.GetType(), "", "alert('"+xmdept+"');", true);
            string je = (myGrid.Items[i].FindControl("txt_je") as TextBox).Text;
            //是否选择
            CheckBox cb = myGrid.Items[i].FindControl("CheckBox1") as CheckBox;
            //是否控制
            DropDownList ddl = myGrid.Items[i].FindControl("ddlIsCtrl") as DropDownList;


            bill_xm_dept_nd temp = new bill_xm_dept_nd();
            temp.xmCode = SubString(xmcode);
            //temp.xmDept = SubString(xmdept);

            temp.xmDept = xmdept;
            temp.nd = this.drpNd.SelectedValue;
            temp.je = Convert.ToDecimal(je);
           

            if (cb.Checked == true)
            {
                temp.status = "1";
            }
            else
            {
                temp.status = "0";
            }
            temp.isCtrl = ddl.SelectedValue;
            xmdeptList.Add(temp);
        }

        //for (int i = 0; i < xmdeptList.Count; i++)
        //{
        //    Response.Write(xmdeptList[i].je+"|");
        //}
        if (dal.Update(xmdeptList))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
            BindDataGrid();
            //string strtxtlrxm = this.txtcx.Text.Trim();
            //DataTable dt = dal.GetxmbBynd(this.drpNd.SelectedValue, strtxtlrxm, hf_km.Value.ToString().Trim());
            //myGrid.DataSource = dt;
            //myGrid.DataBind();
            //RowsBound();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！请联系管理员！');", true);
        }
    }


    #region 查询
    protected void btn_sele_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    #endregion


    public string SubString(string longStr)
    {

        try
        {
            string result = "";
            if (!string.IsNullOrEmpty(longStr) && longStr.Length > 1 && longStr.IndexOf("[") != -1 && longStr.IndexOf("]") != -1)
            {
                int i = longStr.LastIndexOf("]");
                result = longStr.Substring(1, i - 1);
            }
            else
            {
                result = longStr;
            }
            return result;
        }
        catch (Exception e)
        {

            throw e;
        }
    }

}

