using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using WorkFlowLibrary.WorkFlowBll;
using Bll.FeeApplication;
using Bll.Sepecial;
using Bll;
using Dal;

public partial class webBill_MyWorkFlow_BillMainToApprove : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strUserCode = "";
    //string xmzj = "";
    string isDz = "0";//是否是大智
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            strUserCode = Session["userCode"].ToString().Trim();
        }
        ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
        ClientScript.RegisterArrayDeclaration("avaiusertb", GetUsersAll());
        object objisdz = Request["isdz"];
        if (objisdz != null)
        {
            isDz = objisdz.ToString();
        }
        //if (!string.IsNullOrEmpty(Request["xmzj"]))
        //{
        //    xmzj = Request["xmzj"].ToString();
        //}

        if (!IsPostBack)
        {

            bindData();
        }
    }

    protected void bindData()
    {
        if (Request["flowid"] == "jksq")
        {
            GridView1.Columns[8].HeaderText = "借款事由";
            GridView1.Columns[16].Visible = false;
        }
        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 110);
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
        this.GridView1.DataSource = dtrel;
        this.GridView1.DataBind();

    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        bindData();
    }
    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string date = (Convert.ToString(DateTime.Now)).Substring(0, 4) + "0001";
        string request = Request.QueryString["flowid"];
        if (Request["yskmtype"] != null && Request["yskmtype"].ToString() != "")
        {
            string yskmtype = Request["yskmtype"].ToString();
        }

        string flowid = request;
        if (flowid == "xmzf")
        {
            request = "ybbx";
        }
        string usercode = strUserCode;
        //IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, request);

        string strStatus = this.rdoStatusNow.Checked ? "1" : "2";
        IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, request, strStatus);
        StringBuilder sbCodes = new StringBuilder();



        StringBuilder sb = new StringBuilder();

        if (list.Count < 1)
        {
            if (flowid == "gkbx" || flowid == "tfsq" || (isDz.Equals("1") && (flowid.Equals("ybbx") || flowid.Equals("yksq_dz"))))
            {
                sb.Append(@"select (select top 1 billcode from bill_main where billname=main.billName) as billcode, sum(billJe) as billJe,convert(varchar(10),billdate,121) as tbilldate
                ,billname as tbillName,flowid,stepid,billuser,billdate,isgk, billdept as gkdept,(select bxzy from bill_ybbxmxb 
                where bill_ybbxmxb.billCode=(select top 1 billcode from bill_main where billname=main.billName)) as bxzy,(select xmmc from bill_ysgc where gcbh=billName) as billName
                 ,(select top 1 billdept from bill_main where billname=main.billname) as billdept
                 , (select xmname from bill_xm where xmCode=main.note3) as xm");
                if (rdoStatusNow.Checked)//正在进行
                {
                    sb.Append(" ,Row_number()over(order by billName ,billdept  ) as crow ");
                }
                else
                {
                    sb.Append("  ,Row_number()over(order by billName desc,billdept desc) as crow ");
                }

                sb.Append("from bill_main main where flowid='gkbx' and 1=2");
            }
            else
            {
                sb.Append(@" select convert(varchar(10),billdate,121) as tbilldate,*,(case flowID when 'ys' then (select xmmc from bill_ysgc where gcbh=bill_main.billName) else billName end) as tbillName,
                               (select xmname from bill_xm where xmCode=bill_main.note3) as xm,Row_number()over(order by billdept,billName asc) as crow from bill_main where flowid='" + flowid + "' and 1=2 ");

                //if (xmzj == "1")
                //{
                //    sb.Append("  and isnull(note3,'')!='' ");
                //}
                //else
                //{
                //    sb.Append("  and isnull(note3,'')='' ");
                //}
            }
        }
        else
        {
            foreach (string billcode in list)
            {
                sbCodes.Append("'");
                sbCodes.Append(billcode);
                sbCodes.Append("',");
            }
            sbCodes.Remove(sbCodes.Length - 1, 1);
            string strpx = "";
            if (rdoStatusNow.Checked)//正在进行
            {
                strpx = " ,Row_number()over(order by  billName,billdept) as crow ";
            }
            else
            {
                strpx = "  ,Row_number()over(order by billName desc,billdept desc) as crow ";
            }

            if (flowid == "gkbx" || flowid == "tfsq" || (isDz.Equals("1") && (flowid.Equals("ybbx") || flowid.Equals("yksq_dz"))))//如果是归口报销（市立医院）或isdz=1大智学校
            {
                sb.Append(string.Format(@"select main.note3,(select top 1 billcode from bill_main where billname=main.billName) as billcode, sum(billJe) as billJe
                ,convert(varchar(10),billdate,121) as tbilldate,billname as tbillName,flowid,stepid,billuser,billdate,isgk,(billdept) as gkdept,(select bxzy from bill_ybbxmxb
                where bill_ybbxmxb.billCode=(select top 1 billcode from bill_main where billname=main.billName)) as bxzy,(select xmmc from bill_ysgc where gcbh=billName) as billName
                ,(select top 1 billdept from bill_main where billname=main.billname) as billdept, (select xmname from bill_xm where xmCode=main.note3) as xm
                {0} from bill_main main where flowid='" + flowid + "' and (billname in({1}) or billcode in ({1}))", strpx, sbCodes.ToString()));
            }
            else
            {
                sb.Append(string.Format(@"select convert(varchar(10),billdate,121) as tbilldate,*,
                                (case flowID when 'ys' then (select xmmc from bill_ysgc where gcbh=bill_main.billName) else billName end) as tbillName, 
                            (select xmname from bill_xm where xmCode=bill_main.note3) as xm,Row_number()over(order by billdept,billName asc) as crow 
                                from bill_main where flowid='" + flowid + "' and billcode in({0})", sbCodes.ToString()));
                //if (xmzj == "1")
                //{
                //    sb.Append("  and isnull(note3,'')!='' ");
                //}
                //else
                //{
                //    sb.Append("  and isnull(note3,'')='' ");
                //}
            }
        }

        string strdeptcode = (new PublicServiceBLL()).SubSting(TextBox2.Text.Trim());
        if (!string.IsNullOrEmpty(strdeptcode))
        {
            sb.Append(" and (billdept in(select deptcode from bill_departments where deptcode like '" + strdeptcode + "%'))");
        }

        ////if (!string.IsNullOrEmpty(yskmtype))
        ////{ flowid=yszj&isdz=1
        ////    sb.Append(" and ");
        ////}

        string strusercode = (new PublicServiceBLL()).SubSting(TextBox3.Text.Trim());
        if (!string.IsNullOrEmpty(strusercode))
        {
            sb.Append(" and billuser = '" + strusercode + "'");
        }

        if (flowid != "gkbx")
        {
            if (!string.IsNullOrEmpty(TextBox4.Text))
            {
                sb.Append(" and (billje =" + TextBox4.Text + ")");
            }
        }

        if (!string.IsNullOrEmpty(TextBox5.Text))
        {
            sb.Append(" and (convert(varchar(10),billdate,121)>='" + TextBox5.Text + "')");
        }
        string strDateTo = this.txtDateTo.Text.Trim();
        if (!string.IsNullOrEmpty(strDateTo))
        {
            sb.Append(" and (convert(varchar(10),billdate,121)<='" + strDateTo + "')");
        }
        string strBillCode = this.txtBillCode.Text.Trim();
        if (!string.IsNullOrEmpty(strBillCode))
        {
            sb.Append(" and ( billName like '%" + strBillCode + "%' or billCode like '%" + strBillCode + "%') ");
        }

        string strendsql = "";
        if (flowid == "gkbx" || flowid == "tfsq" || (isDz.Equals("1") && (flowid == "ybbx" || flowid == "yksq_dz")))//如果是归口报销（市立医院）或isdz=1大智学校
        {
            sb.Append(" group by billname,flowid,stepid,billuser,billdate,isgk,billdept,note3 ");
            if (!string.IsNullOrEmpty(TextBox4.Text))
            {

                strendsql = "select * from (" + sb.ToString() + ") a where billje=" + TextBox4.Text;

            }
            else
            {
                //sb.Append("  order by billName");
            }

        }
        else
        {
            // sb.Append("  order by billName desc");

        }


        string strsqlcount = "select count(*) from ( {0} ) t ";
        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2} ";

        if (!string.IsNullOrEmpty(strendsql))
        {
            strsqlcount = string.Format(strsqlcount, strendsql);
            strsqlframe = string.Format(strsqlframe, strendsql, pagefrm, pageto);
        }

        else
        {
            strsqlcount = string.Format(strsqlcount, sb.ToString());
            strsqlframe = string.Format(strsqlframe, sb.ToString(), pagefrm, pageto);

        }
        //Response.Write(strsqlframe);
        //count = 0;
        //return new DataTable();
        count = int.Parse(server.GetCellValue(strsqlcount));
        return server.GetDataTable(strsqlframe, null);

    }

    //protected void PaginationToGV1_GvBind(object sender, EventArgs e)
    //{
    //    bindData();
    //}
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        bindData();
    }
    private string GetDeoptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    //private void PreparePagination()
    //{
    //    PaginationToGV1.PageIndex = "1";
    //    string rowsql = @"select count(*) from (" + SqlMaker() + ")t";
    //    PaginationToGV1.RowsCount = Convert.ToString(server.ExecuteScalar(rowsql));
    //}
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e == null)
        {
            return;
        }

        string flowid = Request.QueryString["flowid"];


        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (flowid != "xmys" && flowid != "xmyshz" && flowid != "yszjhz" && flowid != "yszj")// && xmzj != "1"
            {
                e.Row.Cells[17].CssClass = "hiddenbill";
            }
            if (flowid == "cksj")
            {
                e.Row.Cells[9].Text = "回款金额";
                e.Row.Cells[2].Text = "订单号";
            }
            else if (flowid == "xmzf")
            {
                e.Row.Cells[7].Text = "项目";
            }
            else if (flowid == "jksq")
            {
                e.Row.Cells[4].Text = "借款人";
                e.Row.Cells[7].CssClass = "hiddenbill";
                e.Row.Cells[10].Text = "借款时间";
            }
            if (flowid == "xmys" || flowid == "xmyshz" || flowid == "yszjhz" || flowid != "yszj") //xmzj == "1"
            {
                e.Row.Cells[17].Text = "项目";
            }
            if (flowid != "xmys" && flowid != "xmyshz" && flowid != "yszj" && flowid != "yszjhz")// xmzj != "1"
            {
                e.Row.Cells[17].CssClass = "hiddenbill";

            }
            if (flowid == "ybbx" || flowid == "yksq_dz" || flowid == "tfsq")
            {
                e.Row.Cells[7].CssClass = "hiddenbill";
            }
        }

        else if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Pager)
        {
            if (flowid != "xmys" && flowid != "xmyshz" && flowid != "yszj" && flowid != "yszjhz")//xmzj != "1"
            {
                e.Row.Cells[17].CssClass = "hiddenbill";
            }
            string billCode = e.Row.Cells[1].Text;


            string billname = e.Row.Cells[2].Text;
            string usercode = e.Row.Cells[4].Text;
            string billtype = e.Row.Cells[3].Text;
            string deptcode = e.Row.Cells[5].Text;
            string gkdept = e.Row.Cells[7].Text;

            e.Row.Cells[3].Text = server.GetCellValue("select billname from dbo.billtoworkflow where flowid='" + billtype + "'");
            e.Row.Cells[4].Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + usercode + "'");
            e.Row.Cells[5].Text = server.GetCellValue("select '['+deptcode+']'+deptname from bill_departments where deptcode='" + deptcode + "'");

            if (flowid == "jksq")
            {
                e.Row.Cells[8].Text = server.GetCellValue("select LoanExplain  from	T_LoanList	where 	 ListId ='" + billCode + "'");
            }

            if (billtype == "ybbx" || billtype == "gkbx" || billtype == "tfsq" || billtype == "yksq_dz")
            {
                e.Row.Cells[8].Text = server.GetCellValue("select bxzy from dbo.bill_ybbxmxb where billcode='" + billCode + "'");
                string strFuJiaDanJu = this.getFuJiaDanJu(billCode);
                if (!strFuJiaDanJu.Equals(""))
                {
                    string[] arrDanJuCode = strFuJiaDanJu.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string strEndText = "";
                    for (int i = 0; i < arrDanJuCode.Length; i++)
                    {
                        string strDanJuType = arrDanJuCode[i].Substring(0, 4);
                        switch (strDanJuType)
                        {
                            case "ccsq": strEndText += string.Format("<a href='#' style=\"color:Blue\" onclick=\"window.showModalDialog('../fysq/travelReportDetail.aspx?Ctrl=View&AppCode={0}', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:940px;status:no;scroll:yes');\">{0}</a>", arrDanJuCode[i]); break;
                            case "lscg": strEndText += string.Format("<a href='#' style=\"color:Blue\" onclick=\"window.showModalDialog('../fysq/lscgDetail.aspx?type=look&cgbh={0}','newwindow','center:yes;dialogHeight:560px;dialogWidth:940px;status:no;scroll:yes');\">{0}</a>", arrDanJuCode[i]); break;
                            case "cgsp": strEndText += string.Format("<a href='#' style=\"color:Blue\" onclick=\"window.showModalDialog('../fysq/cgspDetail.aspx?type=look&cgbh={0}', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:940px;status:no;scroll:yes');\">{0}</a>", arrDanJuCode[i]); break;
                            default: strEndText += strFuJiaDanJu;
                                break;
                        }
                        strEndText += ",";
                    }
                    strEndText = strEndText.Substring(0, strEndText.Length - 1);
                    e.Row.Cells[15].Text = strEndText;
                }
                if (billtype == "gkbx")
                {
                    e.Row.Cells[16].Text = this.getaudituser(billname);
                }
            }
            else if (billtype == "cgsp")
            {
                e.Row.Cells[8].Text = server.GetCellValue("select sm from dbo.bill_cgsp where cgbh='" + billCode + "'");
            }
            else if (billtype == "lscg")
            {
                e.Row.Cells[8].Text = server.GetCellValue("select zynr from dbo.bill_lscg where cgbh='" + billCode + "'");
            }
            else if (billtype == "xmzf")
            {
                e.Row.Cells[8].Text = server.GetCellValue("select zynr from dbo.bill_xmzfd where billcode='" + billCode + "'");
                e.Row.Cells[5].Text = server.GetCellValue("select '['+xmcode+']'+xmname from bill_xm a,bill_xmzfd b where a.xmcode=b.zfxm and b.billcode='" + billCode + "'");
                e.Row.Cells[3].Text = "项目支付申请单";
            }
            else if (billtype == "ccsq")
            {
                //摘要说明
                e.Row.Cells[8].Text = server.GetCellValue("select top 1 reasion from dbo.bill_travelApplication where maincode='" + billCode + "'");
                e.Row.Cells[3].Text = "出差管理单";
                string strbillCode = e.Row.Cells[1].Text.Trim();
                e.Row.Cells[14].Text = new bill_travelApplicationBLL().GetPersionStrByTravelAppCode(strbillCode);
            }
            if (flowid == "cksj")
            {
                if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
                {
                    e.Row.Cells[6].Text = new RemittanceBll().getBillTruckCode(billCode);
                }
            }
            else if (flowid.Equals("jksq"))
            {
                e.Row.Cells[4].Text = getJkr(billCode);
                e.Row.Cells[7].CssClass = "hiddenbill";
            }
            else
            {
                if (!string.IsNullOrEmpty(gkdept) && gkdept != "&nbsp;")
                {
                    e.Row.Cells[7].Text = server.GetCellValue("select '['+deptcode+']'+deptname from bill_departments where deptcode='" + gkdept + "'");
                }
            }

            if (flowid == "xmys" || flowid == "xmyshz" || flowid == "yszjhz" || flowid == "yszj")//xmzj != "1"
            {
                string xmbh = e.Row.Cells[18].Text;
                List<string> lisql = new List<string>();
                string xmname = "";
                string[] sArray = xmbh.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (sArray.Length > 0)
                {
                    for (int i = 0; i < sArray.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(sArray[i]) && sArray[i] != "&nbsp;")
                        {
                            xmname += server.GetCellValue("select xmName from bill_xm where xmCode='" + sArray[i] + "'") + ";";

                        }
                    }
                }
                if (!string.IsNullOrEmpty(xmname))
                {
                    e.Row.Cells[17].Text = xmname;
                }
            }
            if (flowid == "ybbx" || flowid == "yksq_dz" || billtype == "tfsq")
            {
                e.Row.Cells[7].CssClass = "hiddenbill";
            }
        }
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            
            if (flowid == "xmzf" && flowid == "cksj")
            {
                e.Row.Cells[2].CssClass = "hiddenbill";
            }
            if (flowid == "ybbx" || flowid == "jfsq" || flowid == "srd" || flowid == "zfzxsqd" || flowid == "nzfzxsqd" || flowid == "xyth" || flowid == "yszjhz" || flowid == "zcgzbx" || flowid == "chly" || flowid == "wlfk" || flowid == "qtbx" || flowid == "gkbx" || flowid == "ys" || flowid == "yshz" || flowid == "xmys" || flowid == "xmyshz" || flowid == "srys" || flowid == "zcys" || flowid == "chys" || flowid == "wlys" || flowid == "zjys" || flowid == "yszj" || flowid == "ystz" || flowid == "kmystz" || flowid == "cgzf" || flowid == "yksq_dz" || flowid == "tfsq" || flowid == "yshz")
            {
                if (flowid == "yszj" && isDz == "1")
                {
                    e.Row.Cells[7].CssClass = "hiddenbill";//归口部门
                    e.Row.Cells[8].CssClass = "hiddenbill";
                    e.Row.Cells[12].CssClass = "hiddenbill";
                    e.Row.Cells[15].CssClass = "hiddenbill";
                }
                //隐藏主键
                e.Row.Cells[1].CssClass = "hiddenbill";
                if (flowid == "ystz")
                {
                    e.Row.Cells[2].CssClass = "hiddenbill";
                }
                if (flowid == "ybbx" || flowid == "gkbx")
                {
                    e.Row.Cells[12].CssClass = "hiddenbill";
                }//xmzj != "1" 
                if (flowid == "ys" || flowid == "jfsq" || flowid == "yshz" || flowid == "zfzxsqd" || flowid == "nzfzxsqd" || flowid == "xyth" || flowid == "yszjhz" || flowid == "srys" || flowid == "xmys" || flowid == "xmyshz" || flowid == "zcys" || flowid == "chys" || flowid == "wlys" || flowid == "zjys")
                {



                    e.Row.Cells[8].CssClass = "hiddenbill";
                    e.Row.Cells[7].CssClass = "hiddenbill";//归口部门
                    e.Row.Cells[12].CssClass = "hiddenbill";
                    e.Row.Cells[15].CssClass = "hiddenbill";


                }
                //根据单据编号（预算过程编号）获取预算过程名称
                string billname = e.Row.Cells[2].Text.Trim();

                string gcmc = server.GetCellValue("select xmmc from bill_ysgc where gcbh='" + billname + "'");
                if (!string.IsNullOrEmpty(gcmc))
                {
                    e.Row.Cells[2].Text = gcmc;
                }

            }


            else
            {
                e.Row.Cells[2].CssClass = "hiddenbill";
            }
            if (flowid != "ccsq")
            {
                e.Row.Cells[14].CssClass = "hiddenbill";
            }

            if (flowid == "kpsq")
            {
                e.Row.Cells[9].CssClass = "hiddenbill";
            }
            //如果是开票申请或者是特殊返利申请单或者是车款上缴明细单则隐藏归口列和单据金额
            if (flowid == "tsfl" || flowid == "kpsq" || flowid == "ccsq" || flowid == "yzsq")
            {
                if (flowid == "tsfl")
                {
                    e.Row.Cells[2].CssClass = "hiddenbill";
                    e.Row.Cells[9].CssClass = "hiddenbill";
                }
                e.Row.Cells[8].CssClass = "hiddenbill";
                e.Row.Cells[6].CssClass = "hiddenbill";
                e.Row.Cells[7].CssClass = "hiddenbill";

            }
            if (flowid == "cksj")
            {
                e.Row.Cells[7].CssClass = "hiddenbill";
            }
            else
            {
                e.Row.Cells[6].CssClass = "hiddenbill";
            }
            if (flowid != "gkbx")
            {
                e.Row.Cells[14].CssClass = "hiddenbill";
            }
            if (flowid == "yshz")
            {
                e.Row.Cells[7].CssClass = "hiddenbill";
                e.Row.Cells[8].CssClass = "hiddenbill";
                e.Row.Cells[9].CssClass = "hiddenbill";
                e.Row.Cells[11].CssClass = "hiddenbill";
                e.Row.Cells[14].CssClass = "hiddenbill";
            }
        }
    }

    private string getJkr(string billCode)
    {
        return server.GetCellValue("select '['+usercode+']'+userName from bill_users where usercode =(select ResponsibleCode from T_LoanList where Listid='" + billCode + "')");
    }
    private string getFuJiaDanJu(string strBillCode)
    {
        string strSql = "select sqCode from bill_ybbx_fysq where billCode='" + strBillCode + "'";
        DataTable dtRel = server.GetDataTable(strSql, null);
        string strReturn = "";
        if (dtRel != null && dtRel.Rows.Count > 0)
        {
            for (int i = 0; i < dtRel.Rows.Count; i++)
            {
                strReturn += dtRel.Rows[i][0].ToString() + ",";
            }
            strReturn = strReturn.Substring(0, strReturn.Length - 1);
        }
        return strReturn;
    }

    protected void btn_summit_Click(object sender, EventArgs e)
    {
        bindData();
    }


    private string GetUsersAll()
    {
        DataSet ds = server.GetDataSet("select '['+userCode+']'+userName as usercodename from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["usercodename"]));
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
    private string getaudituser(string strbillcode)
    {
        DataTable dt = server.GetDataTable("select '['+usercode+']'+username as username from bill_users where usercode in(select checkuser from workflowrecord a,workflowrecords  b where a.recordid=b.recordid and billcode=@billcode)", new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@billcode", strbillcode) });
        string endstr = "";
        StringBuilder strendstr = new StringBuilder();
        if (dt != null)
        {
            foreach (DataRow dr in dt.Rows)
            {
                strendstr.Append(dr["username"]);
                strendstr.Append(",");
            }
        }
        if (strendstr.Length > 1)
        {
            endstr = strendstr.ToString().Substring(0, strendstr.Length - 1).ToString();
        }
        return endstr;

    }


    protected void btn_bh_Click(object sender, EventArgs e)
    {

        List<string> list = new List<string>();

        string usercode = Convert.ToString(Session["userCode"]);
        int count = this.GridView1.Rows.Count;

        for (int i = 0; i < count; i++)
        {
            CheckBox cbox = (CheckBox)GridView1.Rows[i].FindControl("CheckBox1");


            if (cbox.Checked == true)
            {

                TextBox txt = (TextBox)this.GridView1.Rows[i].Cells[13].FindControl("TextBox1");
                string billcode = this.GridView1.Rows[i].Cells[2].Text;
                string strsql = @"select stepid from workflowrecord a,workflowrecords b
                                    where a.recordid=b.recordid and billCode ='" + billcode + "' and checkuser='" + usercode + "'";


               // DataTable dt = DataHelper.GetDataTable(strsql, null, false);
                string code = server.GetCellValue(strsql);// dt.Rows[0][0].ToString();
                if (code == "1")
                {
                    string strsql1 = @"update b set b.mind='驳回上一步，原因：" + txt.Text + @"', b.rdState='3' 
                                        from workflowrecord a,workflowrecords b
                                        where a.recordid=b.recordid
                                        and a.billCode ='" + billcode + @"' and checkuser='" + usercode + "'";
                    string strsql2 = @"update a set a.rdState='3' 
                                        from workflowrecord a,workflowrecords b
                                        where a.recordid=b.recordid and billCode ='" + billcode + "' and checkuser='" + usercode + "'";
                    list.Add(strsql1);
                    list.Add(strsql2);
                }
                else
                {
                    string sql1 = @"update b set b.mind='驳回上一步，原因：" + txt.Text + @"'
                                        from workflowrecord a,workflowrecords b
                                        where a.recordid=b.recordid
                                        and a.billCode ='" + billcode + "' and checkuser='" + usercode + "'";

                    string sql2 = @"update b set b.rdstate='1'
                                        from workflowrecord a,workflowrecords b
                                        where a.recordid=b.recordid and a.billCode='"+billcode+@"' and 
                                        b.stepid=(select stepid-1 from workflowrecord a,workflowrecords b
                                        where a.recordid=b.recordid and a.billCode='"+billcode+"' and checkuser='" + usercode + "')";
                    string sql3 = @"update b set b.rdstate='0'
                                        from workflowrecord a,workflowrecords b
                                        where a.recordid=b.recordid and a.billCode='" + billcode + "' and checkuser='" + usercode + "'";
                    list.Add(sql1);
                    list.Add(sql2);
                    list.Add(sql3);
                }
            }
        }
        if (list.Count > 0)
        {
            int cont = server.ExecuteNonQuerys(list.ToArray());
            if (cont > 0)
            {
                Response.Write("<script>alert('驳回上一步成功!')</script>");
                bindData();
            }
            else
            {
                Response.Write("<script>alert('驳回上一步失败!')</script>");
                bindData();
            }
        }
        else
        {
            Response.Write("<script>alert('请先选择单据!')</script>");
        }
    }
    
}

