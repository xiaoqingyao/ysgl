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
using WorkFlowLibrary.WorkFlowBll;
using Dal.newysgl;
using Dal.UserProperty;

public partial class webBill_ysgl_kmystzlist : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    bill_ysmxbDal ysmxDal = new bill_ysmxbDal();
    DepartmentDal deptDal = new DepartmentDal();
    string strNowDeptCode = "";
    string strNowDeptName = "";
    bool boKmystzNeedAudit = new Bll.ConfigBLL().GetValueByKey("KmystzNeedAudit").Equals("0") ? false : true;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            string usercode = Session["userCode"].ToString().Trim();
           
           
                DataTable dtdept = deptDal.getUsercodeName(Session["userCode"].ToString().Trim());
                strNowDeptCode = dtdept.Rows[0]["deptcode"].ToString();
                strNowDeptName = dtdept.Rows[0]["deptName"].ToString();
                string strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");
                dtuserRightDept = deptDal.getRigtusers(strDeptCodes, strNowDeptCode);
                    //server.GetDataTable("select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strDeptCodes + ") and deptCode not in (" + strNowDeptCode + ")", null);

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
                this.BindDataGrid();
            }
        }
        this.hd_KmystzNeedAudit.Value = boKmystzNeedAudit ? "1" : "0";
    }
    void BindDataGrid()
    {
        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex,  ipagesheight,90);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>
        string deptGuid = this.LaDept.Text;
        DataTable temp = ysmxDal.getAlltzlist(deptGuid, true, arrpage[0], arrpage[1], out icount);
        
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount==0?1:icount;
        //----------给gridview赋值
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }


    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button6_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LaDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string zt = e.Item.Cells[6].Text;
            if (zt == "end")
            {
                e.Item.Cells[6].Text = "审批通过";
            }
            else
            {
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[6].Text = state;
            }
        }
    }
}
