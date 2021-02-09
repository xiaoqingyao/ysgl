using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkFlowLibrary.WorkFlowBll;
using System.Data;

public partial class webBill_ysglnew_ystbFrame : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    string strNowDeptCode = "";
    string strNowDeptName = "";
    string stryskmtype = "";//预算类型  01收入 02费用 ……
    public string flowid = "ys";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!string.IsNullOrEmpty(Request["yskmtype"]))
            {
                stryskmtype = Request["yskmtype"].ToString();
                flowid = new Dal.Bills.MainDal().getFlowId(stryskmtype);
            }

            string usercode = Session["userCode"].ToString().Trim();
            if (isTopDept("y", usercode))
            {
                //获取当前用户所在的部门编号及其部门名称 
                strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            }
            else
            {
                //上级部门
                strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
                strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
            }
            string strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");




            //是否预算到末级
            string strsfmj = new Bll.ConfigBLL().GetValueByKey("deptjc");// server.GetCellValue("select avalue from dbo.t_Config where akey='deptjc'");

            strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");


            if (!IsPostBack)
            {

                string selectndsql = "select nian,xmmc from bill_ysgc where   yue='' order by nian desc";
                DataTable selectdt = server.GetDataTable(selectndsql, null);
                drpSelectNd.DataSource = selectdt;
                drpSelectNd.DataTextField = "xmmc";
                drpSelectNd.DataValueField = "nian";
                drpSelectNd.DataBind();
                if (!string.IsNullOrEmpty(strsfmj) && strsfmj == "Y")//如果是预算到末级
                {
                    string strnd = drpSelectNd.SelectedValue;
                    if (!string.IsNullOrEmpty(strnd))
                    {

                        dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where   deptCode in (" + strDeptCodes + ") and deptCode not in (" + strNowDeptCode + ") order by deptcode", null);
                        //dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  deptCode in (" + strDeptCodes + ") and deptCode not in (" + strNowDeptCode + ") and  deptcode in( select distinct deptcode from bill_ys_xmfjbm where procode='" + strnd + "')", null);

                    }

                }
                else
                {

                    dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strDeptCodes + ") and deptCode not in ('" + strNowDeptCode + "') order by deptcode", null);
                }

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

                        //this.LaDept.Items.Insert(0, new ListItem("[" + strNowDeptCode + "]" + strNowDeptName, strNowDeptCode));
                        //this.LaDept.Items.Insert(0, new ListItem("[0104]历下培训学校", "0104"));
                        //this.LaDept.Items.Insert(0, new ListItem("[0105]天桥培训学校", "0105"));
                        //this.LaDept.SelectedIndex = 0;
                    }

                }

                #endregion

                if (!string.IsNullOrEmpty(Request["deptCode"]))
                {
                    LaDept.SelectedValue = Request["deptCode"].ToString();
                    // deptCodes = Request["deptCode"].ToString();
                }
                this.BindDataGrid();

            }
            //验证是否是大智学校
            object objIsdz = Request["isdz"];
            if (objIsdz != null && objIsdz.ToString() != "")
            {
                yztj();
            }
        }
    }
    protected void LaDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    void BindDataGrid()
    {
        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 90);
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
    }

    private DataTable GetData(int pagefrm, int pageto, out int count)
    {

        if (!string.IsNullOrEmpty(drpSelectNd.SelectedValue) && drpSelectNd.SelectedValue != "0")
        {
           
           

            string deptCodes = LaDept.SelectedValue.Trim();

            //string deptCodes = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());

            string IsfjhzSql = "";
            if (new Bll.UserProperty.SysManager().GetsysConfigBynd(drpSelectNd.SelectedValue)["ystbfs"] == "0")//如果是单位填报 
            {
                IsfjhzSql = " 	and yskm in (select yskmcode from bill_yskm where tblx='01')  ";
            }
            //是否新开分校
            string strxkfx = "";
            if (Request["xkfx"] != null)
            {
                strxkfx = Request["xkfx"].ToString();
            }
            if (!string.IsNullOrEmpty(strxkfx) && strxkfx == "1")
            {
                flowid = "xmys";
            }

            string sql = @"select billCode, (select xmmc from bill_ysgc where gcbh=billName) as billName,billdept as showbilldept,note3,(select xmName from bill_xm where xmCode=note3 )as xmname,
        (select deptname from bill_departments where deptcode=billdept) as billDept,(select username from bill_users where usercode=billuser) as billUser
            ,billDate,stepid,billje,(select top 1 mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord where billCode=bill_main.billCode) 
and rdstate='3') as mind ,Row_Number()over(order by billdate desc,note3) as crow  from bill_main where flowid='" + flowid + "' and billCode in (select billCode from bill_ysmxb where ysdept='" + deptCodes + "' " + IsfjhzSql + " ";

            if (!string.IsNullOrEmpty(Request["yskmtype"]))
            {
                sql += " and yskm in (select yskmcode from bill_yskm where dydj='" + Request["yskmtype"] + "') )";
            }
            else
            {
                sql += ")";
            }
            if (drpSelectNd.SelectedValue != "0")
            {
                sql += " and  left(billname,4) = '" + drpSelectNd.SelectedValue + "'";
            }

            if (!string.IsNullOrEmpty(strxkfx) && strxkfx == "1")
            {
                sql += " and note3 is not null and note3!=''";
            }
            sql += " and billDept='" + deptCodes + "' and billType='1' ";

            DataSet temp = server.GetDataSet(sql);

            string strsqlcount = "select count(*) from ( {0} ) t";
            strsqlcount = string.Format(strsqlcount, sql);
            count = int.Parse(server.GetCellValue(strsqlcount));

            string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2} ";
            strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
            return server.GetDataTable(strsqlframe, null);
        }
        else
        {
            count = 0;
            return null;
        }
    }
    /// <summary>
    /// 查询是不是二级单位
    /// </summary>
    /// <param name="strus">是人员CODE？y:n</param>
    /// <param name="usercode">人员CODE</param>
    /// <returns></returns>
    public bool isTopDept(string strus, string usercode)
    {
        string sql = "";
        if (strus == "y")
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "')";
        }
        else
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
        }
        if (server.GetCellValue(sql) == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("ystbDetail.aspx");
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void btn_look_Click(object sender, EventArgs e)
    {
        string deptCode = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());
        string strxmcode = hidxmcode.Value;


        if (flowid == "xmys")
        {
            Response.Redirect("cwtbDetail.aspx?from=ystbFrame&billCode=" + choosebill.Value + "&deptCode=" + deptCode + "&xmcode=" + strxmcode + "&xkfx=1");

        }
        else
        {
            Response.Redirect("cwtbDetail.aspx?from=ystbFrame&billCode=" + choosebill.Value + "&deptCode=" + deptCode);

        }
    }
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ystbEdit.aspx?billCode=" + choosebill.Value);
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header)
        {
            string billcode = e.Item.Cells[1].Text;
            WorkFlowRecordManager bll = new WorkFlowRecordManager();

            if (e.Item.Cells[7].Text == "end")
            {
                e.Item.Cells[7].Text = "审批通过";
            }
            else
            {
                string state = bll.WFState(billcode);
                e.Item.Cells[7].Text = state;
            }

        }
        if (flowid == "ys")
        {
            e.Item.Cells[11].CssClass = "hiddenbill";
        }
    }
    protected void drpSelectNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    /// <summary>
    /// 验证金额是不是符合提交条件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void yztj()
    {
        this.Hidtjzt.Value = "";
        //1.必须是归口部门 并且有分校的进行控制

        string strdept = LaDept.SelectedValue;
        string strnd = drpSelectNd.SelectedValue;

        string xkfx = "";
        if (!string.IsNullOrEmpty(Request["xkfx"]))
        {
            xkfx = Request["xkfx"].ToString();
        }
        if (!string.IsNullOrEmpty(strdept))
        {
            string strisgksql = @"select isgk from bill_departments where deptCode='" + strdept + "'";
            string isgk = server.GetCellValue(strisgksql);
            if (isgk != "N")//如果是归口部门
            {
                //如果是归口部门并且没有分校
                string strisfxsql = @"  select COUNT(*) from bill_departments   where deptcode like ''+substring('" + strdept + "',1,2)+'%'   and (Isgk!='Y' or isgk is null) ";
                int intfxrow = int.Parse(server.GetCellValue(strisfxsql));
                if (intfxrow > 0)//有分校
                {
                    string strdeptgksql = "";
                    int intztrow = 0;
                    //查询归口部门下分校审核情况
                    if (!string.IsNullOrEmpty(xkfx) && xkfx == "1") //如果是新开分校
                    {
                        strdeptgksql = @" select COUNT(*) from bill_main main,bill_ysmxb mxb  where main.billCode=mxb.billCode
                               and flowID='xmys' and LEFT(gcbh,4)='" + strnd + "' and ysDept in ( select deptCode from bill_departments where deptCode like SUBSTRING('" + strdept + "',1,2)+'%'  and  (isgk!='Y' or isgk is null)  and isnull(iskzys,'Y')='N' ) and stepID='-1'";
                        intztrow = Convert.ToInt32(server.GetCellValue(strdeptgksql));

                    }
                    else
                    {
                        strdeptgksql = @" select COUNT(*) from bill_main main,bill_ysmxb mxb  where main.billCode=mxb.billCode
                      and flowID='ys' and LEFT(gcbh,4)='" + strnd + "' and ysDept in ( select deptCode from bill_departments where deptCode like SUBSTRING('" + strdept + "',1,2)+'%'  and (isgk!='Y' or isgk is null)  and isnull(iskzys,'Y')='Y') and stepID='-1'";
                        intztrow = Convert.ToInt32(server.GetCellValue(strdeptgksql));

                    }

                    if (intztrow > 0)//存在没有审批通过的分校
                    {
                        Hidtjzt.Value = "区域内所有分校审核通过后归口部门才可以填报预算，可以进入详细页面点击【查看分校预算状态】查看各分校的填报及审批状态。";
                        //  ClientScript.RegisterStartupScript(this.GetType(), "", "alert('区域内所有分校审核通过后归口部门才可以填报预算，可以进入详细页面点击【查看分校预算状态】查看各分校的填报及审批状态。');", true);
                        return;
                    }
                    else// 如果分校审批通过 验证金额是否允许提交（限额-（年预算+分校汇总金额）大于0可以提交，否则不可以）
                    {
                        if (xkfx != "1")//如果不是新开分校
                        {
                            //1.根据部门获取限额
                            decimal decxe = 0;
                            string strxesql = "select ddefine7 from bill_deptFyblDy where deptCode='" + strdept + "' and cdefine1='" + strnd + "'";
                            string strxe = server.GetCellValue(strxesql);
                            if (!string.IsNullOrEmpty(strxe))
                            {
                                decxe = decimal.Parse(strxe);
                            }
                            //Response.Write("限额"+decxe);
                            //2.获取本部门年预算
                            decimal decnys = 0;
                            string strnyssql = @" select sum(mxb.ysje) from bill_main m,bill_ysmxb mxb
                                                    where flowID='ys'  and mxb.gcbh in (select gcbh from bill_ysgc where nian='" + strnd + "' and ysType='2')    and m.billCode=mxb.billCode and ysDept='" + strdept + "' and mxb.yskm in (select distinct dept.yskmcode from bill_yskm_dept dept,bill_yskm yskm where dept.yskmcode=yskm.yskmcode and deptCode='" + strdept + "'  and isnull(iszyys,'1')!=0)";


                            string strnys = server.GetCellValue(strnyssql);
                            if (!string.IsNullOrEmpty(strnys))
                            {
                                decnys = decimal.Parse(strnys);
                            }
                            //Response.Write("年预算" + decnys);
                            //3.获取汇总金额
                            decimal dechzje = 0;


                            string strhzjesql = @"select SUM(mxb.ysje) from bill_main main,bill_ysmxb mxb
                                                        where main.billCode=mxb.billCode
                                                        and main.stepID='end' and main.flowID='ys' and  mxb.gcbh in (select gcbh from bill_ysgc where nian='{0}' and ysType='2')
                                                        and mxb.yskm in (select distinct dept.yskmcode from bill_yskm_dept dept,bill_yskm yskm where dept.yskmcode=yskm.yskmcode and deptCode='{1}' and isnull(iszyys,'1')!=0)
                                                        and mxb.ysDept in ( select deptCode from bill_departments where deptCode like SUBSTRING('{1}',1,2)+'%'  and (isgk!='Y' or isgk is null)  and isnull(iskzys,'Y')='Y' and  isnull(iskzys,'Y')='Y' )";
                            strhzjesql = string.Format(strhzjesql, strnd, strdept);


                            string strhzje = server.GetCellValue(strhzjesql);
                            if (!string.IsNullOrEmpty(strhzje))
                            {
                                dechzje = decimal.Parse(strhzje);
                            }
                            //Response.Write("汇总金额" + dechzje+"<br>");
                            //4.验证差值是否大于0 如果大于0可以提交否则不可以

                            decimal deccz = 0;
                            deccz = decxe - decnys - dechzje;
                            //Response.Write(deccz.ToString());
                            if (deccz < 0)
                            {
                                Hidtjzt.Value = "提交失败，原因：归口部门预算填报金额+区域分校归口费用汇总金额超出限额：" + (-deccz);
                                // ClientScript.RegisterStartupScript(this.GetType(), "", "alert('填报金额超出限额不允许提交。');", true);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
