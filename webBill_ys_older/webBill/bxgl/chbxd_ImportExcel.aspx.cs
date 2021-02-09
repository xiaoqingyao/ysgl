using Bll;
using Bll.UserProperty;
using Dal.Bills;
using Dal.UserProperty;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkFlowLibrary.WorkFlowBll;

public partial class webBill_bxgl_chbxd_ImportExcel : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    DepartmentDal deptDal = new DepartmentDal();
    string strNowDeptCode = "";
    string strNowDeptName = "";
    //一般报销单是否需要审核 默认是1 需要 edit by Lvcc
    bool boYbbxNeedAudit = new Bll.ConfigBLL().GetValueByKey("YbbxNeedAudit").Equals("0") ? false : true;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            string usercode = Convert.ToString(Session["userCode"]);
            if (!IsPostBack)
            {
                txt_billDate.Text = GetDateTimeMonthFirstDay(DateTime.Now).ToString("yyyy-MM-dd");//DateTime.Now.ToString("yyyy-MM-01");
                txtLoanDateTo.Text = GetDateTimeMonthLastDay(DateTime.Now).ToString("yyyy-MM-dd");
                txt_zdrq.Text = GetDateTimeMonthLastDay(DateTime.Now).ToString("yyyy-MM-dd");
                DepartmentDal depDal = new DepartmentDal();
                string strdept = depDal.GetDeptByUser(usercode);
                UserMessage um = new UserMessage(usercode);
                txt_user.Text = "[" + um.Users.UserCode + "]" + um.Users.UserName;
                if (!string.IsNullOrEmpty(strdept))
                {
                    txt_dept.Text = strdept;
                }
                this.bindZhangTao();
                this.bindData();
            }

            ClientScript.RegisterArrayDeclaration("availabledept", getdept());
            ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());

        }
    }

    /// <summary>  
    /// 获取指定日期的月份第一天  
    /// </summary>  
    /// <param name="dateTime"></param>  
    /// <returns></returns>  
    public static DateTime GetDateTimeMonthFirstDay(DateTime dateTime)
    {
        if (dateTime == null)
        {
            dateTime = DateTime.Now;
        }

        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }

    /// <summary>  
    /// 获取指定月份最后一天  
    /// </summary>  
    /// <param name="dateTime"></param>  
    /// <returns></returns>  
    public static DateTime GetDateTimeMonthLastDay(DateTime dateTime)
    {
        int day = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

        return new DateTime(dateTime.Year, dateTime.Month, day);
    }

    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users where userStatus='1' ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["username"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }
    private string getdept()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as showname from dbo.bill_departments where sjdeptcode!='000001' and sjdeptcode!='' ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow item in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(item["showname"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    void bindData()
    {

        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        //int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        ////ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        //int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 90);
        ////获取pagesize 每页的高度
        //int ipagesize = arrpage[2];
        ////总的符合条件的记录数
        //int icount = 0;
        ////----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>
        DataTable dtrel = GetData();
        ////给分页控件赋值 告诉分页控件 当前页显示的行数
        //this.ucPager.PageSize = ipagesize;
        ////告诉分页控件 所有的记录数
        //this.ucPager.RecordCount = icount == 0 ? 1 : icount;
        //----------给gridview赋值
        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();
        ////注册
        //object objregistermark_date = System.Configuration.ConfigurationManager.AppSettings["RegistDate"];
        //DateTime dtReg;
        //if (objregistermark_date != null)
        //{
        //    dtReg = DateTime.Parse(objregistermark_date.ToString());
        //    DateTime strnowdate = DateTime.Now;
        //    if (strnowdate > dtReg)
        //    {
        //        Random dom = new Random();
        //        int idom = dom.Next(0, 10);
        //        if (idom % 3 == 0)
        //        {
        //            TimeSpan aa = DateTime.Parse("2015-10-17") - DateTime.Now;
        //            int iDays = aa.Days + 1;
        //            if (iDays > 0)
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('您好，试用版本已经到期，还有" + iDays + "天系统将锁定，请联系软件开发商！');", true);
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('您好，试用版本已经到期,，请联系软件开发商！');", true);

        //            }
        //        }
        //    }
        //}
    }
    /// <summary>
    /// 绑定帐套
    /// </summary>
    private void bindZhangTao()
    {
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");

        string strselectsql = @"select dsname as db_data, cAcc_Name,iYear,
                        cast(cAcc_Num as varchar(50))+'|*|'+dsname as tval,
                        * from [{0}].UFTSystem.dbo.EAP_Account  as m    where iYear>='2014' order by m.iYear desc";
        strselectsql = string.Format(strselectsql, strlinkdbname);


        this.ddlZhangTao.DataSource = server.GetDataTable(strselectsql, null);
        this.ddlZhangTao.DataTextField = "companyname";
        this.ddlZhangTao.DataValueField = "db_data";
        this.ddlZhangTao.DataBind();
    }
    protected void OnddlZhangTao_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.bindData();
    }

    private DataTable GetData()
    {

        //BB007F33-C0F0-4A19-8588-1E0E314D1F56    销售出库单
        //21EA9921-40C1-46EB-BF55-2DF64C7CDDFB  其他出库单
        //大智孙桂平要求 由其他出库单改为销售出库单 2016-07-04
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
        string strztname = ddlZhangTao.SelectedValue;
        string sql = @"select  Row_Number()over(order by  main.voucherdate desc) as crow ,main.code,main.voucherState,main.amount 
                  as amount_total,main.rdDirectionFlag as fangxiang,convert(char(10), main.voucherdate,23) as zdDate,main.maker
                    ,main.VoucherPeriod,main.VoucherYear,main.createdtime
                     ,(case (select top 1 mark from bill_chly_mark where
                    chcode=(select code from [" + strlinkdbname + "]." + strztname + ".dbo.AA_Inventory where id=item.idinventory) and pocode=main.code )when '1' then '已导入' else '未导入' end) as showstatus,item.quantity,item.price,item.amount as amount_row,item.priuserdefnvc1 as lingyongType,(select '['+code+']'+name from [" + strlinkdbname + "]." + strztname + ".dbo.AA_Inventory where id=item.idinventory) as chname,(select code from [" + strlinkdbname + "]." + strztname + ".dbo.AA_Inventory where id=item.idinventory) as chcode, ( select '['+code+']'+name from  [" + strlinkdbname + "]." + strztname + ".dbo.AA_Department where id=main.iddepartment) as dept from [" + strlinkdbname + "]." + strztname + ".dbo.ST_RDRecord main ,[" + strlinkdbname + "]." + strztname + ".dbo.ST_RDRecord_b item where main.id=item.idrDRecordDTO and main.idvoucherType=30 and main.rdDirectionFlag='0'  and item.priuserdefnvc2='是' ";// and stepid !='end'   '21EA9921-40C1-46EB-BF55-2DF64C7CDDFB'

        if (this.txt_billDate.Text.ToString().Trim() != "")
        {
            sql += " and (main.voucherdate >='" + this.txt_billDate.Text.ToString().Trim() + "') ";
        }
        if (this.txtLoanDateTo.Text.ToString().Trim() != "")
        {
            sql += " and (main.voucherdate <='" + this.txtLoanDateTo.Text.ToString().Trim() + "') ";
        }

    


        if (!string.IsNullOrEmpty(this.txt_pocode.Text))
        {
            sql += " and (main.code like'%" + this.txt_pocode.Text.ToString().Trim() + "%') ";

        }
        if (!string.IsNullOrEmpty(txt_ch.Text))
        {
            sql += @" and ((select code from [" + strlinkdbname + "]." + strztname + ".dbo.AA_Inventory where id=item.idinventory) like'%" + this.txt_ch.Text.ToString().Trim() + "%' or (select name from [" + strlinkdbname + "]." + strztname + ".dbo.AA_Inventory where id=item.idinventory)  like'%" + this.txt_ch.Text.ToString().Trim() + "%')";

        }
        if (!string.IsNullOrEmpty(ddl_status.SelectedValue))
        {
            if (ddl_status.SelectedValue == "1")
            {
                sql += @" and isnull((select top 1 mark from bill_chly_mark where chcode=(select code from  [" + strlinkdbname + "]." + strztname + ".dbo.AA_Inventory where id=item.idinventory) and pocode=main.code),0) ='1'";

                // sql += @" and (select mark from bill_chly_mark where chcode=(select code from  [" + strlinkdbname + "]." + strztname + ".dbo.AA_Inventory where id=item.idinventory) and pocode=main.code)='1'";

            }
            else if (ddl_status.SelectedValue == "0")
            {
                sql += @" and isnull((select top 1 mark from bill_chly_mark where chcode=(select code from  [" + strlinkdbname + "]." + strztname + ".dbo.AA_Inventory where id=item.idinventory) and pocode=main.code),0) ='0'";

            }


        }

        //string strsqlcount = "select count(*) from ( {0} ) t";
        //strsqlcount = string.Format(strsqlcount, sql);
        //count = int.Parse(server.GetCellValue(strsqlcount));

        //string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}  order by showstatus";
        //strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        // Response.Write(sql);
        return server.GetDataTable(sql, null);

    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        bindData();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.bindData();
    }
    /// <summary>
    /// 导入制单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_dr_Click(object sender, EventArgs e)
    {
        string strdept = "";
        string struser = "";
        //单据唯一键
        if (!string.IsNullOrEmpty(txt_dept.Text))
        {
            strdept = txt_dept.Text.Substring(1, txt_dept.Text.IndexOf("]") - 1);
        }
        if (!string.IsNullOrEmpty(txt_user.Text))
        {
            struser = txt_user.Text.Substring(1, txt_user.Text.IndexOf("]") - 1);
        }
        else
        {
            showMessage("请选择部门。", false, "");
            return;
        }

        string strdate = "";

        //if ((DateTime.Parse(txt_billDate.Text).Year + DateTime.Parse(txt_billDate.Text).Month) != (DateTime.Parse(txtLoanDateTo.Text).Year + DateTime.Parse(txtLoanDateTo.Text).Month))
        //{
        //    showMessage("请选择相同的月份", false, "");
        //    return;
        //}

        //string strzy = txt_zy.Text;
        //if (string.IsNullOrEmpty(txt_zy.Text))
        //{
        //    showMessage("请填写摘要。", false, "");
        //    return;

        //}
        //else
        //{
        //    strzy = txt_zy.Text;
        //}
        string strsm = txt_sm.Text;

        string strufdata = ddlZhangTao.SelectedValue;//帐套
        string strbillcode = new GuidHelper().getNewGuid();



        string strysgc = "";
        YsManager ysmgr = new YsManager();
        if (!string.IsNullOrEmpty(txt_zdrq.Text))
        {
            strdate = txt_zdrq.Text;//txt_billDate.Text;

            strysgc = ysmgr.GetYsgcCode(DateTime.Parse(txt_zdrq.Text));
        }
        else
        {
            showMessage("请选择制单日期", false, "");
            return;
        }
        string strmouth = server.GetCellValue("select yue from bill_ysgc where gcbh='" + strysgc + "'");
        if (string.IsNullOrEmpty(strysgc) || string.IsNullOrEmpty(strmouth))
        {
            showMessage("没有对应的预算过程，请开启预算过程。", false, "");
            return;
        }
        List<string> lstsql = new List<string>();

        // List<string> lstchsql = new List<string>();

        List<string> lstdrbjsql = new List<string>();

        string strchname = "";
        string strbxzy = "";// txt_zy.Text.Trim();
        dr_dyb();//将没有对应的的存货加到对应表
        string strErrorMsg = "导入失败，原因：";
        //DataTable dt = new DataTable();
        //dt = GetData();
        for (int i = 0; i < myGrid.Items.Count; i++)
        {

            CheckBox ck = myGrid.Items[i].Cells[0].FindControl("CheckBox1") as CheckBox;

            if (ck.Checked)
            {
                //判断该存货是否做过科目对应
                string pocode = myGrid.Items[i].Cells[1].Text;
                string strgkdept = myGrid.Items[i].Cells[5].Text;//dt.Rows[i]["dept"].ToString();
                if (!string.IsNullOrEmpty(strgkdept))
                {
                    strgkdept = strgkdept.Substring(1, strgkdept.IndexOf("]") - 1);
                }
                string chcode = myGrid.Items[i].Cells[6].Text;//存货编号
                string chname = myGrid.Items[i].Cells[7].Text;//存货名称
                string stryzsql = @"select count(*) from bill_yskmchdy where chcode='" + chcode + "' and yslx='04' and ufdata='" + strufdata + "'and note1='" + myGrid.Items[i].Cells[5].Text + "' and kmcode!=''";
                string StrRow = server.GetCellValue(stryzsql);//
                if (Convert.ToInt32(StrRow) == 0)
                {
                    strchname += chname + ";";
                    //lstchsql.Add("delete from  bill_yskmchdy  where chcode='" + chcode + "' and yslx='04' and ufdata='" + strufdata + "' and kmcode is null");
                    //string strinsert = "insert into bill_yskmchdy(chcode,chname,yslx,ufdata)values('" + chcode + "','" + chname + "','04','" + strufdata + "')";
                    //lstchsql.Add(strinsert);
                    //if (lstchsql.Count > 0)
                    //{
                    //   server.ExecuteNonQuerysArray(lstchsql);
                    //}

                }
                string strsql = "";
                if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
                    return;
                }
                string strzdrcode = Session["userCode"].ToString().Trim();
                string strzddept = strdept;
                string strhsje = myGrid.Items[i].Cells[10].Text;


                string stryskmcode = "";
                string strhsdeptcode = strdept;
                string strgetkmsql = @" select kmcode from bill_yskmchdy where chcode='" + chcode + "' and yslx='04' and ufdata='" + strufdata + "' and note1='" + myGrid.Items[i].Cells[5].Text + "' ";
                stryskmcode = server.GetCellValue(strgetkmsql);
                if (string.IsNullOrEmpty(stryskmcode))
                {
                    strErrorMsg += " 没有设置存货科目对应关系，请做好对应再导入。 <a href='chkm_dy.aspx' style='color:blue'>点我去设置存货科目对照关系</a>";

                    Response.Write(strErrorMsg);
                    this.bindData();
                    return;
                    //showMessage(chname + "存货没有对应的科目，请设置好对应再导入。", false, "");
                    //return;
                }

                //判断部门是否做过对应
                string strdydeptsql = "  select ysdeptcode from bill_ys_uf_dept where ufdeptcode='" + strgkdept + "' and ufdata='" + strufdata + "'";
                string strysgkdept = server.GetCellValue(strdydeptsql);
                if (string.IsNullOrEmpty(strysgkdept))
                {
                    strErrorMsg += "没有设置部门对照关系，请做好对应再导入。<a href='Dept_uf_dy.aspx' style='color:blue'>点我去设置部门对照关系</a>";

                    Response.Write(strErrorMsg);
                    this.bindData();
                    return;
                }

                //添加导入标记表

                lstdrbjsql.Add("insert into bill_chly_mark(billCode,billuser,billdept,billdate,pocode,mark,chcode,ufdata) values ('" + strbillcode + "','" + struser + "','" + strdept + "','" + strdate + "','" + pocode + "','1','" + chcode + "','" + strufdata + "')");

                strbxzy = server.GetCellValue("select xmmc from bill_ysgc where gcbh='" + strysgc + "'") + "领用单导入";

                strsql = @"insert into lsbxd_main(billcode,flowid,billUser,billDate,billDept,je,se,isgk,gkdept,bxzy,bxsm,fykmcode,sydept,bxlx,note1)
                                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')";
                strsql = string.Format(strsql, strbillcode, "yksq_dz", strzdrcode, strdate, strzddept, strhsje, "0", "1", strysgkdept, strbxzy, strsm, stryskmcode, strysgkdept, "01", strufdata);
                lstsql.Add(strsql);
            }


        }
        if (!string.IsNullOrEmpty(strchname))
        {
            //int introw = server.ExecuteNonQuerysArray(lstchsql);
            //if (introw > 0)
            //{
            // showMessage("友情提示:以下存货没有做科目对应" + strchname + ",请到基础数据模块下的存货科目对应菜单下进行对应设置，然后再重新导入。", false, "");

            strErrorMsg += "<a href='chkm_dy.aspx' style='color:blue'>点我去设置存货科目对照关系</a>";

            Response.Write(strErrorMsg);
            this.bindData();
            return;
            //}
        }
        if (lstsql.Count > 0)
        {
            int irels = server.ExecuteNonQuerysArray(lstsql);
            if (irels >= 1)
            {
                string strbillname = server.GetCellValue(" exec [pro_makebxd] '" + strbillcode + "','chly'");
                if (!string.IsNullOrEmpty(strbillname))
                {
                    //标记已经导入的单子
                    if (lstdrbjsql.Count > 0)
                    {
                        int introw = server.ExecuteNonQuerysArray(lstdrbjsql);
                    }
                    //
                }
                server.ExecuteNonQuery("delete lsbxd_main  where billcode='" + strbillcode + "'", null);
                showMessage("保存成功", true, "1");

            }
        }
    }
    /// <summary>
    /// 导入对应表
    /// </summary>
    public void dr_dyb()
    {

        string strufdata = ddlZhangTao.SelectedValue;//帐套


        List<string> lstchsql = new List<string>();
        //DataTable dt = new DataTable();
        //dt = GetData();
        for (int i = 0; i < myGrid.Items.Count; i++)
        {

            CheckBox ck = myGrid.Items[i].Cells[0].FindControl("CheckBox1") as CheckBox;

            if (ck.Checked)
            {
                //判断该存货是否做过科目对应
                //string pocode = myGrid.Items[i].Cells[1].Text;//dt.Rows[i]["code"].ToString();
                //string chcode = myGrid.Items[i].Cells[6].Text;//存货编号 dt.Rows[i]["chcode"].ToString();//
                //string chname = dt.Rows[i]["chname"].ToString();
                string pocode = myGrid.Items[i].Cells[1].Text;
                string strdept = myGrid.Items[i].Cells[5].Text;//部门
                if (!string.IsNullOrEmpty(strdept))
                {
                    strdept = strdept.Substring(1, strdept.IndexOf("]") - 1);
                }
                string chcode = myGrid.Items[i].Cells[6].Text;//存货编号
                string chname = myGrid.Items[i].Cells[7].Text;//存货名称
                string stryzsql = @"select count(*) from bill_yskmchdy where chcode='" + chcode + "' and yslx='04' and note1='" + myGrid.Items[i].Cells[5].Text + "' and ufdata='" + strufdata + "' and kmcode!=''";
                string StrRow = server.GetCellValue(stryzsql);//
                if (Convert.ToInt32(StrRow) == 0)
                {
                    lstchsql.Add("delete from  bill_yskmchdy  where chcode='" + chcode + "' and yslx='04' and ufdata='" + strufdata + "' and note1='" + myGrid.Items[i].Cells[5].Text + "'  and kmcode is null");
                    string strinsert = "insert into bill_yskmchdy(chcode,chname,yslx,ufdata,note1)values('" + chcode + "','" + chname + "','04','" + strufdata + "','" + myGrid.Items[i].Cells[5].Text + "')";
                    lstchsql.Add(strinsert);

                }
            }


        }
        if (lstchsql.Count > 0)
        {
            server.ExecuteNonQuerysArray(lstchsql);
        }
    }
}