using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using Bll.UserProperty;
using Bll;

public partial class ysgl_ysgcList : BasePage
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
            this.Button1.Attributes.Add("onclick", "javascript:openDetail('ysgcDetail.aspx?type=add');");
            if (!IsPostBack)
            {
                this.BindDataGrid();
            }
        }
    }

    public void BindDataGrid()
    {

        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 80);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>
        DataTable dtrel = GetData(arrpage[0], arrpage[1], out icount);
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount;
        //----------给gridview赋值
        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();
    }


    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string sql = "select (case ysType when '0' then '年预算' when '1' then '季度预算' when '2' then '月预算' end) as ysType,nian,(case ysType when '0' then '' when '1' then '第'+yue+'季度' when '2' then yue+'月' end) as yue,gcbh,xmmc,kssj,jzsj,(select username from bill_users where usercode=fqr) as fqr,fqsj,(case status when '0' then '未开始' when '1' then '进行中' when '2' then '已结束' end) as statusName,status,Row_Number()over(order by nian desc) as crow  from bill_ysgc where 1=1";
        string strsqlwhere = "";
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            strsqlwhere += " and gcbh like '%" + this.TextBox1.Text.ToString().Trim() + "%' or  xmmc like '%" + this.TextBox1.Text.ToString().Trim() + "%'";
        }
        sql += strsqlwhere;
        string strsqlcount = "select count(*) from bill_ysgc where 1=1";
        strsqlcount += strsqlwhere;
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<{2} ORDER BY LEFT(gcbh, 4) DESC, RIGHT(gcbh, 4)";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("ysgcDetail.aspx?type=add");
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        string status = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                status = this.myGrid.Items[i].Cells[7].Text.ToString().Trim();
            }
        }
        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个过程！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待修改过程！');", true);
        }
        else
        {
            if (status == "2")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择的过程已结束,不能修改！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('ysgcDetail.aspx?gcbh=" + billGuid + "&type=edit');", true);
                //Response.Redirect("ysgcDetail.aspx?guid=" + billGuid + "&type=edit");
            }
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        string status = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {

            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid += this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                status = this.myGrid.Items[i].Cells[7].Text.ToString().Trim();
            }
        }
        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个过程！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待删除过程！');", true);
        }
        else
        {
            if (status == "1" || status == "2")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择的过程已在预算中,不能修改！');", true);
            }
            else
            {
                List<string> list = new List<string>();
                list.Add("delete from bill_ysgc where gcbh in (" + billGuid + ")");
                list.Add("delete from bill_main where billCode in (" + billGuid + ") and flowID='ys'");
                list.Add("delete from bill_ysmxb where gcbh in (" + billGuid + ")");
                if (server.ExecuteNonQuerysArray(list) == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
                    this.BindDataGrid();
                }
            }
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid += "'" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "',";
            }
        }
        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待操作的过程！');", true);
        }
        else
        {
            billGuid = billGuid.Substring(0, billGuid.Length - 1);
            //DataSet temp = server.GetDataSet("select * from bill_ysgc where gcbh in (" + billGuid + ")");
            //if (DateTime.Parse(temp.Tables[0].Rows[0]["jzsj"].ToString().Trim()) < DateTime.Parse(System.DateTime.Now.ToShortDateString()))
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('选择的预算过程已超出预算时间,请修改后再启动预算过程！');", true);
            //    return;
            //}

            List<string> list = new List<string>();
            list.Add("update bill_ysgc set status='1' where gcbh in (" + billGuid + ")");
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作成功！');", true);
                this.BindDataGrid();
            }
        }
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid += "'" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "',";
            }
        }
        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待操作的过程！');", true);
        }
        else
        {
            billGuid = billGuid.Substring(0, billGuid.Length - 1);
            List<string> list = new List<string>();
            list.Add("update bill_ysgc set status='2' where gcbh in (" + billGuid + ")");
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作成功！');", true);
                this.BindDataGrid();
            }
        }
    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        ConfigBLL configbll = new ConfigBLL();
        string strCYLX = configbll.GetValueByKey("CYLX");//从配置项中读取是否是财年
        if (server.GetCellValue("select count(1) from bill_ysgc where nian='" + this.TextBox2.Text.ToString().Trim() + "'") != "0")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('输入年度的预算过程已存在！');", true);
            return;
        }
        int nd = 0;
        try
        {
            nd = int.Parse(this.TextBox2.Text.ToString().Trim());
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算年度输入错误！');", true);
            return;
        }
        System.Collections.Generic.List<string> list = new List<string>();

        string ndStr = this.TextBox2.Text.ToString().Trim();

        //2012.4.6 mxl 检测是否启用年度，月度，季度预算

        // IDictionary<string, string> sysConfig = new SysManager().GetsysConfig();
        IDictionary<string, string> sysConfig = new SysManager().GetsysConfigBynd(TextBox2.Text.Trim());

        if (sysConfig.Count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成预算过程失败！没有找到当前年份的参数设置');", true);
        }
        else
        {

            string vBegin, vEnd;
            if (sysConfig["YearBudget"] == "1")
            {
                #region 年度预算过程
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='年度预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('年度预算开始时间参数未设置！');", true);
                    return;
                }
                if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")
                {
                    if (vBegin.Substring(0, 2) == "01")
                    {
                        vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                    }
                    else
                    {
                        vBegin = (int.Parse(this.TextBox2.Text.ToString().Trim()) - 1) + "-" + vBegin;
                    }
                }

                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='年度预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('年度预算截止时间参数未设置！');", true);
                    return;
                }
                if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    if (vEnd.Substring(0, 2) == "01")
                    {
                        vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                    }
                    else
                    {
                        vEnd = (int.Parse(this.TextBox2.Text.ToString().Trim()) - 1) + "-" + vEnd;
                    }
                }


                list.Add("insert into bill_ysgc values('" + ndStr + "0001','" + ndStr + "预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','','0')");
                #endregion
            }
            if (sysConfig["MonthOrQuarter"] == "1")
            {
                #region 季度预算过程
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='第1季度预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第1季度预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    if (vBegin.Substring(0, 2) == "01")
                    {
                        vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                    }
                    else
                    {
                        vBegin = (int.Parse(this.TextBox2.Text.ToString().Trim()) - 1) + "-" + vBegin;
                    }
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='第1季度预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第1季度预算截止时间参数未设置！');", true);
                    return;
                }
                if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    if (vEnd.Substring(0, 2) == "01")
                    {
                        vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                    }
                    else
                    {
                        vEnd = (int.Parse(this.TextBox2.Text.ToString().Trim()) - 1) + "-" + vEnd;
                    }
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0002','" + ndStr + "年第一季度预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','一','1')");

                //2
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='第2季度预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第2季度预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='第2季度预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第2季度预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0003','" + ndStr + "年第二季度预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','二','1')");

                //3
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='第3季度预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第3季度预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='第3季度预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第3季度预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0004','" + ndStr + "年第三季度预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','三','1')");


                //4
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='第4季度预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第4季度预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='第4季度预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第4季度预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0005','" + ndStr + "年第四季度预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','四','1')");

                #endregion
            }
            if (sysConfig["MonthOrQuarter"] == "2")
            {
                #region 月预算过程
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='1月份预算开始时间'");
                string strzrn = "";
                if (vBegin == "")
                {
                   
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('1月份预算开始时间参数未设置！');", true);
                    return;
                }
                if (!string.IsNullOrEmpty(vBegin))
                {
                    strzrn = vBegin.Substring(0, 5);
                }
                if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    if (vBegin.Substring(0, 2) == "01")
                    {
                        vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                    }
                    else
                    {
                        vBegin = (int.Parse(this.TextBox2.Text.ToString().Trim()) - 1) + "-" + vBegin;
                    }
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='1月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('1月份预算截止时间参数未设置！');", true);
                    return;
                }
                if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    if (vEnd.Substring(0, 2) == "01")
                    {
                        vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                    }
                    else
                    {
                        vEnd = (int.Parse(this.TextBox2.Text.ToString().Trim()) - 1) + "-" + vEnd;
                    }
                }
                
                list.Add("insert into bill_ysgc values('" + ndStr + "0006','" +strzrn+ ndStr + "年01月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','1','2')");


                //2
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='2月份预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('2月份预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='2月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('2月份预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0007','" + strzrn + ndStr + "年02月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','2','2')");

                //3
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='3月份预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('3月份预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='3月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('3月份预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0008','" + strzrn + ndStr + "年03月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','3','2')");

                //4
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='4月份预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('4月份预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='4月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('4月份预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0009','" + strzrn + ndStr + "年04月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','4','2')");

                //5
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='5月份预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('5月份预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='5月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('5月份预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0010','" + strzrn + ndStr + "年05月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','5','2')");

                //6
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='6月份预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('6月份预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='6月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('6月份预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0011','" + strzrn + ndStr + "年06月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','6','2')");

                //7
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='7月份预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('7月份预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='7月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('7月份预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0012','" + strzrn + ndStr + "年07月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','7','2')");

                //8
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='8月份预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('8月份预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='8月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('8月份预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0013','" + strzrn + ndStr + "年08月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','8','2')");

                //9
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='9月份预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('9月份预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='9月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('9月份预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0014','" + strzrn + ndStr + "年09月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','9','2')");

                //10
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='10月份预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('10月份预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='10月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('10月份预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0015','" + strzrn + ndStr + "年10月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','10','2')");

                //11
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='11月份预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('11月份预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='11月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('11月份预算截止时间参数未设置！');", true);
                    return;
                } 
                if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0016','" + strzrn + ndStr + "年11月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','11','2')");

                //12
                vBegin = server.GetCellValue("select parVal from bill_syspar where parName='12月份预算开始时间'");
                if (vBegin == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('12月份预算开始时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vBegin = this.TextBox2.Text.ToString().Trim() + "-" + vBegin;
                }
                vEnd = server.GetCellValue("select parVal from bill_syspar where parName='12月份预算截止时间'");
                if (vEnd == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('12月份预算截止时间参数未设置！');", true);
                    return;
                } if (string.IsNullOrEmpty(strCYLX) || strCYLX == "N")//如果是财年
                {
                    vEnd = this.TextBox2.Text.ToString().Trim() + "-" + vEnd;
                }
                list.Add("insert into bill_ysgc values('" + ndStr + "0017','" + strzrn + ndStr + "年12月预算过程','" + vBegin + "','" + vEnd + "','0','" + Session["userCode"].ToString().Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd") + "','" + ndStr + "','12','2')");
                #endregion
            }
         
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成预算过程失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成预算过程完成！');", true);
                this.BindDataGrid();
            }
        }
    }
    protected void btn_Delysgc_Click(object sender, EventArgs e)
    {
        string ysgcbh = TextBox2.Text.Trim();
        if (ysgcbh != "")
        {
            if (server.GetCellValue("select count(1) from bill_ysgc where nian='" + this.TextBox2.Text.ToString().Trim() + "'") != "0")
            {
                if (server.GetCellValue("select count(*) from bill_ysmxb where left(gcbh,4) = '" + ysgcbh + "'") == "0")
                {
                    if (server.ExecuteNonQuery("delete bill_ysgc where nian = '" + ysgcbh + "'", null) == -1)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除预算过程失败！');", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除预算过程成功！');", true);
                        this.BindDataGrid();
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算年度内的预算过程已经填报！');", true);
                    return;
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('未找到输入年份的预算过程！');", true);
                return;
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请在预算过程文本框中输入年份！');", true);
            return;
        }
    }

    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
}
