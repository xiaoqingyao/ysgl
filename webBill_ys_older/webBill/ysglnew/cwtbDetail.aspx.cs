using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using Bll.UserProperty;
using System.Drawing;
using System.Data;
using System.Reflection;
using System.IO;
using Dal.Bills;

public partial class webBill_ysglnew_cwtbDetail : System.Web.UI.Page
{
    Bll.newysgl.YsglMainBll bll = new Bll.newysgl.YsglMainBll();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string nd = string.Empty;//填报年度
    string deptcode = string.Empty;//填报部门
    string yskmtype = string.Empty;//预算科目类型  如果传入了该参数  将对填报的科目类型进行限制   对应的传入url键为yskmtype  可以不传  不传默认该部门对应的所有科目
    string flowid = "ys";//跟yskmtype是一对的  通过预算科目类型获取对应的flowid
    //填报类型 对应的传入url键为yskmtype   bill_sysconfig有一个配置  是配置费用预算的填报方式的；但是由于其他类型的预算也使用该页面 
    //但是有的是汇总填报的 有的是分解的 所以直接根据客户的不同在url的参数上配置  两个值：zxer(自下而上汇总) zsex(自上而下分解)
    string tbtype = string.Empty;
    string IsLimitTotal = string.Empty;//如果是部门汇总填报的时候(zxes)  是否控制总金额 该配置项在URL中配置  如果控制总金额去部门预算分解表中读取
    decimal deDeptTotal = 0;//如果是部门汇总填报(zxes) 并且控制总金额时  获取分配给该部门的合计金额(bill_ys_xmfjbm)
    string jecheckflg = "";//传入参数jecheckflg 如果jecheckflg =0表示年度金额必须等于各月份的分解金额 如果jecheckflg=1表示年度金额必须大于等于各月份分解金额。
    public string HelpMsg = "";//提示信息

    string xmbh = "";//项目编号  用于处理大智的新开分校
    List<string> lstYskmsNoYs = new List<string>();//不占用预算的预算科目编号

    //跟预算表无关的参数
    public string deptcodes = string.Empty;//归口部门所在区域的所有部门
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null && hdUserCode.Value.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else if (Session["userCode"] != null)
        {
            this.hdUserCode.Value = Session["userCode"].ToString();
        }
        else if (!hdUserCode.Value.Equals(""))
        {
            Session["userCode"] = hdUserCode.Value;
        }
        Response.Cache.SetSlidingExpiration(true);
        Response.Cache.SetNoStore();

        #region 获取URL配置参数
        //年度
        object objnd = Request["nd"];
        if (objnd != null)
        {
            nd = objnd.ToString();
        }
        //部门编号
        object objdept = Request["deptCode"];
        if (objdept != null)
        {
            deptcode = objdept.ToString();
        }

        //判断是否是新开分校
        object objxmbh = Request["xmcode"];
        if (objxmbh != null && objxmbh.ToString() != "" && objxmbh != "?")
        {
            flowid = "xmys";
            xmbh = objxmbh.ToString();
            yskmtype = "02";
        }
        else
        {
            //预算科目类型
            object objYskmType = Request["yskmtype"];
            if (objYskmType != null && (!string.IsNullOrEmpty(Request["yskmtype"])))
            {
                yskmtype = objYskmType.ToString();
                flowid = new Dal.Bills.MainDal().getFlowId(yskmtype);
            }
        }

        //填报类型
        object objTbType = Request["tbtype"];
        if (objTbType != null && (!string.IsNullOrEmpty(Request["tbtype"])))
        {
            tbtype = objTbType.ToString();
        }
        else
        {
            getTbtype(nd);
        }

        //部门自下而上填报的时候  是否控制填报总金额
        object objIsLimitTotal = Request["limittotal"];
        if (objIsLimitTotal != null && (!string.IsNullOrEmpty(Request["limittotal"])))
        {
            IsLimitTotal = objIsLimitTotal.ToString();
        }

        //如果获取到billcode说明是通过billcode来查看  在这里通过billcode给年度部门等信息赋值
        object objbillcode = Request["billCode"];
        string billcode = string.Empty;
        if (objbillcode != null && (!string.IsNullOrEmpty(Request["billCode"])))
        {
            Bill_Main mainmodel = new MainDal().GetMainByCode(objbillcode.ToString());
            nd = mainmodel.BillName.Substring(0, 4);//年度
            deptcode = mainmodel.BillDept;//填报部门
            flowid = mainmodel.FlowId;//flowid
            getTbtype(nd);//填报方式 自上而下还是自下而上
            yskmtype = new MainDal().getYskmType(flowid);
        }
        #endregion
        getYskmsNoYs();//获取不控制预算的科目
        //如果部门汇总填报并且控制总金额
        if (IsLimitTotal.Equals("1"))
        {
            //获取为该部门分配的总额度
            deDeptTotal = getDeptNdAmount(nd, deptcode, yskmtype);
        }

        if (!IsPostBack)
        {

            #region 获取deptcodes 归口部门+分校  这个是为了查询区域预算明细服务的
            DataTable dtDeptcodes = server.GetDataTable("select deptcode from bill_departments where (deptcode like ''+substring('" + Request["deptCode"] + "',1,2)+'%' and (Isgk!='Y' or isgk is null) ) or deptcode = '" + Request["deptCode"] + "'", null);
            System.Text.StringBuilder sbdeptcodes = new System.Text.StringBuilder();
            for (int i = 0; i < dtDeptcodes.Rows.Count; i++)
            {
                sbdeptcodes.Append(dtDeptcodes.Rows[i]["deptcode"] + ",");
            }
            deptcodes = sbdeptcodes.ToString();
            #endregion

            //根据部门进行控制保存按钮的可编辑性
            //1.如果是归口部门并且各分校填报没有完成的时候不允许编辑 提示查看分校填报状态，只有填报完毕且都审批通过了的归口部门才能进行填报
            string strisgksql = @"select isgk from bill_departments where deptCode='" + Request["deptCode"] + "'";
            string strgk = server.GetCellValue(strisgksql);
            if (strgk != "N")
            {
                //如果是归口部门并且没有分校
                string strisfxsql = @"  select COUNT(*) from bill_departments   where deptcode like ''+substring('" + Request["deptCode"] + "',1,2)+'%'   and (Isgk!='Y' or isgk is null) ";
                int intfxrow = int.Parse(server.GetCellValue(strisfxsql));
                if (intfxrow == 0)
                {
                    btn_hz.Visible = false;//隐藏查看汇总的按钮
                    btn_mx.Visible = false;//隐藏查查看明细的按钮
                    btn_zt.Visible = false;//隐藏查看状态的按钮
                }
                else
                {

                    //如果是归口部门 并且底下有分校
                    string strdeptgksql = @" select COUNT(*) from bill_main main,bill_ysmxb mxb  where main.billCode=mxb.billCode
                      and flowID='ys' and LEFT(gcbh,4)='" + Request["nd"] + "' and ysDept in ( select deptCode from bill_departments where deptCode like SUBSTRING('" + Request["deptCode"] + "',1,2)+'%'  and (isgk!='Y' or isgk is null) ) and stepID='-1'";
                    int intztrow = Convert.ToInt32(server.GetCellValue(strdeptgksql));

                    //if (intztrow > 0)
                    //{
                    //    //归口部门下存在还没有审批通过的分校
                    //    btn_save.Enabled = false;//隐藏保存按钮
                    //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('区域内所有分校审核通过后归口部门才可以填报预算，可以点击【查看分校预算状态】查看各分校的填报及审批状态。'); ", true);
                    //    return;
                    //}
                }
            }
            else
            {
                //如果是分校 不是归口部门
                btn_hz.Visible = false;//隐藏查看汇总的按钮
                btn_mx.Visible = false;//隐藏查查看明细的按钮
                btn_zt.Visible = false;//隐藏查看状态的按钮
            }



            //绑定gridview
            Bind();
            ////如果是审核页面过来的，把返回列表页按钮隐藏掉
            //if (Request["checking"]!=null)
            //{
            //    fanhui.Visible = false;
            //}
            //如果是费用预算 且不是归口部门 不显示预算总额
            if (Request["jecheckflg"] != null && Request["jecheckflg"].ToString() != "")
            {
                jecheckflg = Request["jecheckflg"].ToString();
            }
            if (Request.QueryString["deptCode"] != null && Request.QueryString["nd"] != null)
            {
                string strcn = "";
                string deptcode = Request.QueryString["deptCode"].ToString();
                string strsql = @"select xmmc from bill_ysgc where nian='" + Request["nd"].ToString() + "' and yue=''";
                strcn = server.GetCellValue(strsql);

                ltdept.Text = strcn + ";    填报单位：" + server.GetCellValue("select deptname from bill_departments where deptcode='" + deptcode + "'");

                #region 处理附件
                DataTable dtfujian = server.GetDataTable("select fujian,filename from bill_ysfj where deptcode='" + deptcode + "' and nd='" + nd + "' and isnull(xmbh,'')='" + xmbh + "'", null);
                if (dtfujian != null && dtfujian.Rows.Count > 0)
                {
                    Lafilename.Text = dtfujian.Rows[0]["filename"].ToString();
                    this.hiddFileDz.Value = dtfujian.Rows[0]["fujian"].ToString();
                    upLoadFiles.Visible = false;
                    btn_sc.Text = "修改附件";
                    Literal1.Text += "<a href='../../AFrame/download.aspx?filename=" + dtfujian.Rows[0]["filename"].ToString() + "&filepath=" + dtfujian.Rows[0]["fujian"].ToString() + "' target='_blank'>" + dtfujian.Rows[0]["filename"].ToString() + "下载;</a>";
                }
                else
                {
                    //如果没有附件的话
                    btn_sc.Text = "上 传";
                    Lafilename.Text = "";
                    upLoadFiles.Visible = true;
                    hiddFileDz.Value = "";
                }
                #endregion
            }

            string strisgk = server.GetCellValue("select isgk from dbo.bill_departments where deptcode='" + Request.QueryString["deptCode"].ToString() + "' ");

            //判断是否是新开分校
            if (!string.IsNullOrEmpty(Request["xkfx"]) && Request["xkfx"].ToString() == "1")
            {
                lbtotalamout.Visible = false;
                lbmonemooney.Visible = false;

            }
            else
            {
                if (strisgk != "Y" && yskmtype == "02")
                {
                    lbtotalamout.Visible = false;
                    lbmonemooney.Visible = false;
                }
                else
                {
                    lbtotalamout.Visible = true;
                    lbmonemooney.Visible = true;
                }
            }


            //如果是查看就不显示保存按钮
            if (Request.QueryString["look"] != null)
            {
                if (Request.QueryString["look"].ToString() == "look")
                {
                    btn_save.Visible = false;
                }
            }
        }
        HelpMsg += "<li>各月份的预算额度总和不得超过年度预算。</li>";
        if (!string.IsNullOrEmpty(IsLimitTotal) && IsLimitTotal.Equals("1"))
        {
            HelpMsg += "<li>部门填报预算总和不得超过该部门预算总限额</li>";
        }
        else if (IsLimitTotal.Equals("0") && tbtype.Equals("zxes"))
        {
            HelpMsg += "<li>该类预算不控制预算总额。</li>";
        }
        if (!string.IsNullOrEmpty(jecheckflg) && jecheckflg.Equals("0"))
        {
            HelpMsg += "<li>年预算必须完全分解到十二个月份中，即：年度预算额度等于十二个月份预算额度总和。</li>";
        }
    }
    /// <summary>
    ///通过年度获取填报方式并未全局变量赋值
    /// </summary>
    /// <param name="nd"></param>
    private void getTbtype(string nd)
    {
        if (!string.IsNullOrEmpty(nd))
        {
            IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(nd);
            tbtype = sysConfig["ystbfs"].Equals("1") ? "zsex" : "zxes";//配置项中1代表自上而下分解
        }
    }
    private void Bind()
    {
        if (!string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(nd))// && Request.QueryString["type"] != null
        {
            //string tblx = Request.QueryString["type"].ToString() == "ystb" ? "01" : "02";  // 02是财务填报  "01"是部门填报

            string tblx = "01";
            if (deptcode != "")
            {
                try
                {
                    IList<YsgcTb> ysMainTable = new List<YsgcTb>();
                    if (nd != "")
                    {


                        ysMainTable = bll.GetMainTable(deptcode, nd, yskmtype, tblx, new string[] { "1", "5", "8" }, xmbh, "");

                        if (ysMainTable == null)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('找不到年份的预算过程！或者没有设置相对应的预算科目！');", true);
                        }
                        else
                        {
                            IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(nd);
                            if (sysConfig["MonthOrQuarter"] == "1")
                            {
                                GridView1.Visible = false;
                                GridView2.Visible = true;
                                GridView2.DataSource = ysMainTable;
                                GridView2.DataBind();
                            }
                            else
                            {
                                GridView2.Visible = false;
                                GridView1.Visible = true;
                                GridView1.DataSource = ysMainTable;
                                GridView1.DataBind();
                            }
                            RowsBound();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + ex + "');", true);
                }
            }
        }
    }
    /// <summary>
    /// 获取各分校汇总总年度预算金额
    /// </summary>
    /// <returns></returns>
    public string gethzje()
    {

        string strzhsql = @"  select sum(ysje) as ysje    from bill_main  m,Bill_Ysmxb b  
		       where m.billcode=b.billcode and flowid='ys' and stepid='end'   and billdept in (select deptcode from bill_departments  where (deptcode like ''+substring('" + deptcode + "',1,2)+'%'   and (Isgk!='Y' or isgk is null)) )  and b.gcbh in(select gcbh from bill_ysgc where nian='" + nd + "' and  ystype='2') and yskm in (select dept.yskmcode from bill_yskm_dept dept,bill_yskm yskm where dept.yskmcode=yskm.yskmcode and deptcode='" + deptcode + "' and isnull(yskm.iszyys,'1')='1')";

        return server.GetCellValue(strzhsql);
    }
    private void RowsBound()
    {
        string deptcode = Request.QueryString["deptCode"].ToString();
        SysManager sysmanager = new SysManager();
        string strxmcode = "";
        YsgcTb gcbh = bll.GetgcbhByNd(Request.QueryString["nd"].ToString()); // 获取预算过程编号
        IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(Request.QueryString["nd"].ToString());
        Dal.Bills.YsgcDal gc = new Dal.Bills.YsgcDal();

        //如果是自上而下分解，年预算不允许用户录入了 反之允许
        if (this.tbtype.Equals("zsex"))
        {
            #region 如果本次是分解填报  年度预算不允许修改
            HiddenisFjtb.Value = new Dal.newysgl.Bmfj().IsFjtb(Request.QueryString["nd"].ToString(), deptcode);
            if (GridView1.Visible == true)
            {
                // GridView1.Columns[2].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                GridView1.Columns[2].ItemStyle.CssClass = "unEdit";

            }
            if (GridView2.Visible == true)
            {
                //GridView2.Columns[2].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                GridView2.Columns[2].ItemStyle.CssClass = "unEdit";
            }
            #endregion
            LaTbfs.Text = "    填报方式：预算分解填报";
        }
        else
        {
            LaTbfs.Text = "    填报方式：预算汇总填报";
        }
        if (!string.IsNullOrEmpty(Request["xmcode"]))
        {
            strxmcode = Request["xmcode"].ToString();
        }
        if (!string.IsNullOrEmpty(strxmcode))
        {
            LaTbfs.Text += ";填报项目：" + server.GetCellValue("select '['+xmcode+']'+xmname from bill_xm where xmcode='" + strxmcode + "'");
        }


        #region 控制文本框的显示  以及科目名称前的空格

        float fTotalAmout = 0;
        float fTotalAmout2 = 0;
        float ftotalyue1 = 0;
        float ftotalyue2 = 0;
        float ftotalyue3 = 0;
        float ftotalyue4 = 0;
        float ftotalyue5 = 0;
        float ftotalyue6 = 0;
        float ftotalyue7 = 0;
        float ftotalyue8 = 0;
        float ftotalyue9 = 0;
        float ftotalyue10 = 0;
        float ftotalyue11 = 0;
        float ftotalyue12 = 0;
        float fyzmoney = 0;

        float ftotalspring = 0;
        float ftotalsummer = 0;
        float ftotalautumn = 0;
        float ftotalwinter = 0;
        float ftotaljijiemoney = 0;



        string strhzje = gethzje();//获取各分校汇总总年度金额
        decimal dechzje = 0;
        if (!string.IsNullOrEmpty(strhzje))
        {
            dechzje = decimal.Parse(strhzje);
        }
        decimal deckzje = deDeptTotal - dechzje;//显示总金额=预算比例得出的总金额-各分校汇总金额


        #region gridview1

        if (GridView1.Visible == true)
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.Rows[i].Cells[0].Text = (i + 1).ToString();                                          //行号
                string hiddkmbh = (GridView1.Rows[i].FindControl("HiddenKmbh") as HiddenField).Value;          //科目编号
                if (sysmanager.GetYskmIsmj(hiddkmbh) != "0")                                                   //将非末级的文本框隐藏
                {
                    TextBox txtJanuary = GridView1.Rows[i].FindControl("txtJanuary") as TextBox;
                    txtJanuary.CssClass = "basehidden";//.Style.Add("display", "none");
                    txtJanuary.Enabled = false;
                    TextBox txtFebruary = GridView1.Rows[i].FindControl("txtFebruary") as TextBox;
                    txtFebruary.Style.Add("display", "none");
                    txtFebruary.Enabled = false;

                    TextBox txtmarch = GridView1.Rows[i].FindControl("txtmarch") as TextBox;
                    txtmarch.Style.Add("display", "none");
                    txtmarch.Enabled = false;

                    TextBox txtApril = GridView1.Rows[i].FindControl("txtApril") as TextBox;
                    txtApril.Style.Add("display", "none");
                    txtApril.Enabled = false;

                    TextBox txtMay = GridView1.Rows[i].FindControl("txtMay") as TextBox;
                    txtMay.Style.Add("display", "none");
                    txtMay.Enabled = false;

                    TextBox txtJune = GridView1.Rows[i].FindControl("txtJune") as TextBox;
                    txtJune.Style.Add("display", "none");
                    txtJune.Enabled = false;

                    TextBox txtJuly = GridView1.Rows[i].FindControl("txtJuly") as TextBox;
                    txtJuly.Style.Add("display", "none");
                    txtJuly.Enabled = false;

                    TextBox txtAugust = GridView1.Rows[i].FindControl("txtAugust") as TextBox;
                    txtAugust.Style.Add("display", "none");
                    txtAugust.Enabled = false;

                    TextBox txtSeptember = GridView1.Rows[i].FindControl("txtSeptember") as TextBox;
                    txtSeptember.Style.Add("display", "none");
                    txtSeptember.Enabled = false;

                    TextBox txtOctober = GridView1.Rows[i].FindControl("txtOctober") as TextBox;
                    txtOctober.Style.Add("display", "none");
                    txtOctober.Enabled = false;

                    TextBox txtNovember = GridView1.Rows[i].FindControl("txtNovember") as TextBox;
                    txtNovember.Style.Add("display", "none");
                    txtNovember.Enabled = false;

                    TextBox txtDecember = GridView1.Rows[i].FindControl("txtDecember") as TextBox;

                    txtDecember.Style.Add("display", "none");
                    txtDecember.Enabled = false;


                    //if (this.tbtype.Equals("zsex"))
                    //{
                    //    //(GridView1.Rows[i].FindControl("") as TextBox).Style.Add("display", "none");
                    //    TextBox txtyear = GridView1.Rows[i].FindControl("txtyear") as TextBox;
                    //    txtyear.CssClass = "basehidden";
                    //    //txtyear.Style.Add("display", "none");
                    //    txtyear.Enabled = false;
                    //}
                    TextBox txtyear1 = GridView1.Rows[i].FindControl("txtyear") as TextBox;
                    txtyear1.CssClass = "basehidden";
                    //GridView1.Rows[i].BackColor = Color.FromName("#DEDEDE");
                    GridView1.Rows[i].CssClass = "unEdit";
                }

                #region 获取十二个月的金额

                if (!lstYskmsNoYs.Contains(hiddkmbh))//如果是占用预算的科目才进行计算
                {
                    float fJanuary = 0;

                    TextBox jeJanuary = GridView1.Rows[i].FindControl("txtJanuary") as TextBox;
                    if (jeJanuary.Text != "" && jeJanuary.Text != null)
                    {
                        float.TryParse(jeJanuary.Text, out fJanuary);
                        ftotalyue1 += fJanuary;
                    }

                    float fFebruary = 0;
                    TextBox jeFebruary = GridView1.Rows[i].FindControl("txtFebruary") as TextBox;
                    if (jeFebruary.Text != "" && jeFebruary.Text != null)
                    {
                        float.TryParse(jeFebruary.Text, out fFebruary);
                        ftotalyue2 += fFebruary;
                    }

                    float fmarch = 0;
                    TextBox jemarchy = GridView1.Rows[i].FindControl("txtmarch") as TextBox;
                    if (jemarchy.Text != "" && jemarchy.Text != null)
                    {
                        float.TryParse(jemarchy.Text, out fmarch);
                        ftotalyue3 += fmarch;

                    }

                    float fApril = 0;
                    TextBox jetxtApril = GridView1.Rows[i].FindControl("txtApril") as TextBox;
                    if (jetxtApril.Text != "" && jetxtApril.Text != null)
                    {
                        float.TryParse(jetxtApril.Text, out fApril);
                        ftotalyue4 += fApril;
                    }

                    float fMay = 0;
                    TextBox jetxtMay = GridView1.Rows[i].FindControl("txtMay") as TextBox;
                    if (jetxtMay.Text != "" && jetxtMay.Text != null)
                    {
                        float.TryParse(jetxtMay.Text, out fMay);
                        ftotalyue5 += fMay;

                    }


                    float fJune = 0;
                    TextBox jetxtJune = GridView1.Rows[i].FindControl("txtJune") as TextBox;
                    if (jetxtJune.Text != "" && jetxtJune.Text != null)
                    {
                        float.TryParse(jetxtJune.Text, out fJune);
                        ftotalyue6 += fJune;
                    }


                    float fJuly = 0;
                    TextBox jetxtJuly = GridView1.Rows[i].FindControl("txtJuly") as TextBox;
                    if (jetxtJuly.Text != "" && jetxtJuly.Text != null)
                    {
                        float.TryParse(jetxtJuly.Text, out fJuly);
                        ftotalyue7 += fJuly;

                    }

                    float fAugust = 0;
                    TextBox jetxtAugust = GridView1.Rows[i].FindControl("txtAugust") as TextBox;
                    if (jetxtAugust.Text != "" && jetxtAugust.Text != null)
                    {
                        float.TryParse(jetxtAugust.Text, out fAugust);
                        ftotalyue8 += fAugust;

                    }
                    float fSeptember = 0;
                    TextBox jetxtSeptember = GridView1.Rows[i].FindControl("txtSeptember") as TextBox;
                    if (jetxtSeptember.Text != "" && jetxtSeptember.Text != null)
                    {
                        float.TryParse(jetxtSeptember.Text, out fSeptember);
                        ftotalyue9 += fSeptember;
                    }

                    float fOctober = 0;
                    TextBox jetxtOctober = GridView1.Rows[i].FindControl("txtOctober") as TextBox;
                    if (jetxtOctober.Text != "" && jetxtOctober.Text != null)
                    {
                        float.TryParse(jetxtOctober.Text, out fOctober);
                        ftotalyue10 += fOctober;
                    }

                    float fNovember = 0;
                    TextBox jetxtNovember = GridView1.Rows[i].FindControl("txtNovember") as TextBox;
                    if (jetxtNovember.Text != "" && jetxtNovember.Text != null)
                    {
                        float.TryParse(jetxtNovember.Text, out fNovember);
                        ftotalyue11 += fNovember;
                    }

                    float fDecember = 0;
                    TextBox jetxtDecember = GridView1.Rows[i].FindControl("txtDecember") as TextBox;
                    if (jetxtDecember.Text != "" && jetxtDecember.Text != null)
                    {
                        float.TryParse(jetxtDecember.Text, out fDecember);
                        ftotalyue12 += fDecember;
                    }
                #endregion

                    float flAmount = 0;

                    TextBox je = GridView1.Rows[i].FindControl("txtyear") as TextBox;

                    if (je.Text != "" && je.Text != null)
                    {
                        float.TryParse(je.Text, out flAmount);
                        fTotalAmout += flAmount;
                    }
                }
                fyzmoney = ftotalyue1 + ftotalyue2 + ftotalyue3 + ftotalyue4 + ftotalyue5 + ftotalyue6 + ftotalyue7 + ftotalyue8 + ftotalyue9 + ftotalyue10 + ftotalyue11 + ftotalyue12;
            }

            float fsyje = 0;//未分配金额


            //部门汇总填报并控制总金额
            if (IsLimitTotal.Equals("1"))
            {
                lbtotalamout.Text = "年度预算额度：" + deckzje.ToString("N");//deDeptTotal.ToString("N");
                //预算比例得出的总金额-各分校汇总金额        -已填报了的占用预算科目的金额
                lbmonemooney.Text = "未分配额度：" + (double.Parse(deckzje.ToString()) - double.Parse(fyzmoney.ToString())).ToString("N");//(double.Parse(deDeptTotal.ToString()) - double.Parse(fyzmoney.ToString())).ToString("N");
            }
            else if (flowid != "chys" && flowid != "wlys" && flowid != "zcys")
            {

                fsyje = fTotalAmout - fyzmoney;//剩余金额
                lbtotalamout.Text = "年度预算额度：" + fTotalAmout.ToString("N");
                lbmonemooney.Text = "未分配额度：" + fsyje.ToString("N");
            }
        }
        #endregion

        #region gridview2

        if (GridView2.Visible == true)
        {
            for (int i = 0; i < GridView2.Rows.Count; i++)
            {
                string hiddkmbh = (GridView2.Rows[i].FindControl("HiddenKmbh") as HiddenField).Value;
                if (sysmanager.GetYskmIsmj(hiddkmbh) != "0")
                {
                    TextBox txtspring = GridView2.Rows[i].FindControl("txtspring") as TextBox;
                    txtspring.Style.Add("display", "none");
                    txtspring.Enabled = false;

                    TextBox txtsummer = GridView2.Rows[i].FindControl("txtsummer") as TextBox;
                    txtsummer.Style.Add("display", "none");
                    txtsummer.Enabled = false;
                    TextBox txtautumn = GridView2.Rows[i].FindControl("txtautumn") as TextBox;
                    txtautumn.Style.Add("display", "none");
                    txtautumn.Enabled = false;
                    TextBox txtwinter = GridView2.Rows[i].FindControl("txtwinter") as TextBox;
                    txtwinter.Style.Add("display", "none");
                    txtwinter.Enabled = false;
                    TextBox txtyear = GridView2.Rows[i].FindControl("txtyear") as TextBox;
                    txtyear.Style.Add("display", "none");
                    txtyear.Enabled = false;


                    //(GridView2.Rows[i].FindControl("txtspring") as TextBox).Style.Add("display", "none");
                    //(GridView2.Rows[i].FindControl("txtsummer") as TextBox).Style.Add("display", "none");
                    //(GridView2.Rows[i].FindControl("txtautumn") as TextBox).Style.Add("display", "none");
                    //(GridView2.Rows[i].FindControl("txtwinter") as TextBox).Style.Add("display", "none");
                    //(GridView2.Rows[i].FindControl("txtyear") as TextBox).Style.Add("display", "none");
                    //GridView2.Rows[i].BackColor = Color.FromName("#DEDEDE");
                    GridView2.Rows[i].CssClass = "unEdit";
                }
                GridView2.Rows[i].Cells[0].Text = (i + 1).ToString();
                //if (sysConfig["ystbfs"] != "1")
                //{
                //    int s = hiddkmbh.Length - 2;
                //    string rvstr = "";
                //    for (int k = 0; k <= s; k++)
                //    {
                //        rvstr += "&nbsp;&nbsp;";
                //    }
                //    GridView2.Rows[i].Cells[1].Text = rvstr + GridView2.Rows[i].Cells[1].Text;
                //}


                float flspring = 0;
                TextBox jetxtspring = GridView2.Rows[i].FindControl("txtspring") as TextBox;
                if (jetxtspring.Text != "" && jetxtspring.Text != null)
                {
                    float.TryParse(jetxtspring.Text, out flspring);
                    ftotalspring += flspring;

                }
                float flsummer = 0;
                TextBox jetxtsummer = GridView2.Rows[i].FindControl("txtsummer") as TextBox;
                if (jetxtsummer.Text != "" && jetxtsummer.Text != null)
                {
                    float.TryParse(jetxtsummer.Text, out flsummer);
                    ftotalsummer += flsummer;

                }
                float flautumn = 0;
                TextBox jetxtautumn = GridView2.Rows[i].FindControl("txtautumn") as TextBox;
                if (jetxtautumn.Text != "" && jetxtautumn.Text != null)
                {
                    float.TryParse(jetxtautumn.Text, out flautumn);
                    ftotalautumn += flautumn;

                }
                float fltxtwinter = 0;
                TextBox jetxtwinter = GridView2.Rows[i].FindControl("txtwinter") as TextBox;
                if (jetxtwinter.Text != "" && jetxtwinter.Text != null)
                {
                    float.TryParse(jetxtwinter.Text, out fltxtwinter);
                    ftotalwinter += fltxtwinter;

                }


                float flAmount2 = 0;
                TextBox je2 = GridView2.Rows[i].FindControl("txtyear") as TextBox;
                if (je2.Text != "" && je2.Text != null)
                {
                    float.TryParse(je2.Text, out flAmount2);
                    fTotalAmout2 += flAmount2;

                }

            }
            ftotaljijiemoney = ftotalwinter + ftotalsummer + ftotalspring + ftotalautumn;//已分解金额

            float fjijieamout = 0;//未分配金额
            //部门汇总填报并控制总金额
            if (IsLimitTotal.Equals("1"))
            {


                lbtotalamout.Text = "年度预算额度：" + deckzje.ToString("N");//deDeptTotal.ToString("N");
                lbmonemooney.Text = "未分配额度：" + (double.Parse(deckzje.ToString()) - double.Parse(fyzmoney.ToString())).ToString("N");//(double.Parse(deDeptTotal.ToString()) - double.Parse(fyzmoney.ToString())).ToString("N");

                fjijieamout = (float)(deckzje - decimal.Parse(ftotaljijiemoney.ToString()));// (float)(deDeptTotal - decimal.Parse(ftotaljijiemoney.ToString()));
                lbtotalamout.Text = "年度预算额度：" + fTotalAmout2.ToString("N");
                lbmonemooney.Text = "未分配额度：" + (double.Parse(deckzje.ToString()) - double.Parse(ftotaljijiemoney.ToString())).ToString("N");//(double.Parse(deDeptTotal.ToString()) - double.Parse(ftotaljijiemoney.ToString())).ToString("N");
            }
            else if (flowid != "chys" && flowid != "wlys" && flowid != "zcys")
            {
                fjijieamout = fTotalAmout2 - ftotaljijiemoney;
                lbtotalamout.Text = "年度预算额度：" + fTotalAmout2.ToString("N");
                lbmonemooney.Text = "未分配额度：" + fjijieamout.ToString("N");
            }
        }
        #endregion

        #endregion

        if (sysConfig["MonthOrQuarter"] == "1")//月度预算
        {
            #region 将季度的背景色不可以用的改变颜色
            if (!string.IsNullOrEmpty(yskmtype))
            {
                if (gc.IsState(gcbh.year, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView2.Columns[2].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView2.Columns[2].ItemStyle.CssClass = "unEdit";
                }

                if (gc.IsState(gcbh.spring, deptcode, flowid, yskmtype, xmbh))
                {
                    GridView2.Columns[3].ItemStyle.CssClass = "unEdit";
                    GridView2.Columns[3].HeaderStyle.CssClass = "unEdit";
                    //GridView2.Columns[3].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    //GridView2.Columns[3].HeaderStyle.BackColor = Color.FromName("#DEDEDE");
                }

                if (gc.IsState(gcbh.summer, deptcode, flowid, yskmtype, xmbh))
                {
                    GridView2.Columns[4].ItemStyle.CssClass = "unEdit";
                    GridView2.Columns[4].HeaderStyle.CssClass = "unEdit";

                    //GridView2.Columns[4].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    //GridView2.Columns[4].HeaderStyle.BackColor = Color.FromName("#DEDEDE");
                }

                if (gc.IsState(gcbh.autumn, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView2.Columns[5].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    //GridView2.Columns[5].HeaderStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView2.Columns[5].ItemStyle.CssClass = "unEdit";
                    GridView2.Columns[5].HeaderStyle.CssClass = "unEdit";
                }

                if (gc.IsState(gcbh.winter, deptcode, flowid, yskmtype, xmbh))
                {
                    GridView2.Columns[6].ItemStyle.CssClass = "unEdit";
                    GridView2.Columns[6].HeaderStyle.CssClass = "unEdit";

                    //GridView2.Columns[6].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    //GridView2.Columns[6].HeaderStyle.BackColor = Color.FromName("#DEDEDE");
                }

            }
            else
            {
                if (gc.IsState(gcbh.year, deptcode, flowid, xmbh))
                {
                    //GridView2.Columns[2].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView2.Columns[2].ItemStyle.CssClass = "unEdit";
                }

                if (gc.IsState(gcbh.spring, deptcode, flowid, xmbh))
                {
                    GridView2.Columns[3].ItemStyle.CssClass = "unEdit";
                    GridView2.Columns[3].HeaderStyle.CssClass = "unEdit";
                    //GridView2.Columns[3].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    //GridView2.Columns[3].HeaderStyle.BackColor = Color.FromName("#DEDEDE");
                }

                if (gc.IsState(gcbh.summer, deptcode, flowid, xmbh))
                {
                    GridView2.Columns[4].ItemStyle.CssClass = "unEdit";
                    GridView2.Columns[4].HeaderStyle.CssClass = "unEdit";

                    //GridView2.Columns[4].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    //GridView2.Columns[4].HeaderStyle.BackColor = Color.FromName("#DEDEDE");
                }

                if (gc.IsState(gcbh.autumn, deptcode, flowid, xmbh))
                {
                    //GridView2.Columns[5].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    //GridView2.Columns[5].HeaderStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView2.Columns[5].ItemStyle.CssClass = "unEdit";
                    GridView2.Columns[5].HeaderStyle.CssClass = "unEdit";
                }

                if (gc.IsState(gcbh.winter, deptcode, flowid, xmbh))
                {
                    GridView2.Columns[6].ItemStyle.CssClass = "unEdit";
                    GridView2.Columns[6].HeaderStyle.CssClass = "unEdit";

                    //GridView2.Columns[6].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    //GridView2.Columns[6].HeaderStyle.BackColor = Color.FromName("#DEDEDE");
                }

            }




            #endregion
        }
        if (sysConfig["MonthOrQuarter"] == "2")//季度预算
        {
            #region 将月度不可用的改变背景色

            if (!string.IsNullOrEmpty(yskmtype))
            {
                if (this.tbtype.Equals("zsex"))
                {

                    if (gc.IsState(gcbh.year, deptcode, flowid, yskmtype, xmbh))
                    {
                        //GridView1.Columns[2].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                        GridView1.Columns[2].ItemStyle.CssClass = "unEdit";
                    }
                }


                if (gc.IsState(gcbh.January, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView1.Columns[3].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[3].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.February, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView1.Columns[4].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[4].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.march, deptcode, flowid, yskmtype, xmbh))
                {
                    // GridView1.Columns[5].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[5].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.April, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView1.Columns[7].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[7].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.May, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView1.Columns[8].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[8].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.June, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView1.Columns[9].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[9].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.July, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView1.Columns[11].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[11].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.August, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView1.Columns[12].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[12].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.September, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView1.Columns[13].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[13].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.October, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView1.Columns[15].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[15].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.November, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView1.Columns[16].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[16].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.December, deptcode, flowid, yskmtype, xmbh))
                {
                    //GridView1.Columns[17].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[17].ItemStyle.CssClass = "unEdit";
                }
            }
            else
            {
                if (this.tbtype.Equals("zsex"))
                {

                    if (gc.IsState(gcbh.year, deptcode, flowid, xmbh))
                    {
                        //GridView1.Columns[2].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                        GridView1.Columns[2].ItemStyle.CssClass = "unEdit";
                    }
                }


                if (gc.IsState(gcbh.January, deptcode, flowid, xmbh))
                {
                    //GridView1.Columns[3].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[3].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.February, deptcode, flowid, xmbh))
                {
                    //GridView1.Columns[4].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[4].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.march, deptcode, flowid, xmbh))
                {
                    // GridView1.Columns[5].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[5].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.April, deptcode, flowid, xmbh))
                {
                    //GridView1.Columns[7].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[7].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.May, deptcode, flowid, xmbh))
                {
                    //GridView1.Columns[8].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[8].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.June, deptcode, flowid, xmbh))
                {
                    //GridView1.Columns[9].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[9].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.July, deptcode, flowid, xmbh))
                {
                    //GridView1.Columns[11].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[11].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.August, deptcode, flowid, xmbh))
                {
                    //GridView1.Columns[12].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[12].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.September, deptcode, flowid, xmbh))
                {
                    //GridView1.Columns[13].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[13].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.October, deptcode, flowid, xmbh))
                {
                    //GridView1.Columns[15].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[15].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.November, deptcode, flowid, xmbh))
                {
                    //GridView1.Columns[16].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[16].ItemStyle.CssClass = "unEdit";
                }
                if (gc.IsState(gcbh.December, deptcode, flowid, xmbh))
                {
                    //GridView1.Columns[17].ItemStyle.BackColor = Color.FromName("#DEDEDE");
                    GridView1.Columns[17].ItemStyle.CssClass = "unEdit";
                }
            }

            #endregion
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", "EnbleTxt();", true);          //将背景为#DEDEDE的TD内的textbox设置为不可用
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (Request["isdz"] == "1")
        {

            yanzhen();
        }

        if (Request["jecheckflg"] != null && Request["jecheckflg"].ToString() != "")
        {
            jecheckflg = Request["jecheckflg"].ToString();
        }
        if (Request.QueryString["deptCode"] != null && Request.QueryString["type"] != null)
        {
            string tblx = Request.QueryString["type"].ToString() == "ystb" ? "" : "02";
            string deptcode = Request.QueryString["deptCode"].ToString();
            string nd = Request.QueryString["nd"].ToString();
            IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(nd);
            IList<YsgcTb> TbMain = new List<YsgcTb>();


            string isfjtb = HiddenisFjtb.Value;
            if (isfjtb == "1")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门的年度预算金额没有确认'); ", true);
                RowsBound();
                return;
            }
            if (isfjtb == "2")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('找不到部门年度预算金额');", true);
                RowsBound();
                return;
            }

            #region 季度

            if (sysConfig["MonthOrQuarter"] == "1")
            {
                for (int k = 0; k < GridView2.Rows.Count; k++)
                {
                    string hiddkmbh = (GridView2.Rows[k].FindControl("HiddenKmbh") as HiddenField).Value;
                    string spring = (GridView2.Rows[k].FindControl("txtspring") as TextBox).Text.Trim();
                    string summer = (GridView2.Rows[k].FindControl("txtsummer") as TextBox).Text.Trim();
                    string autumn = (GridView2.Rows[k].FindControl("txtautumn") as TextBox).Text.Trim();
                    string winter = (GridView2.Rows[k].FindControl("txtwinter") as TextBox).Text.Trim();
                    string year = (GridView2.Rows[k].FindControl("txtyear") as TextBox).Text.Trim();
                    YsgcTb ys = new YsgcTb();
                    ys.kmbh = hiddkmbh;
                    ys.spring = spring;
                    ys.summer = summer;
                    ys.autumn = autumn;
                    ys.winter = winter;
                    ys.year = year;
                    decimal count = 0;
                    count += (spring == "" ? 0 : Convert.ToDecimal(spring));
                    count += (summer == "" ? 0 : Convert.ToDecimal(summer));
                    count += (autumn == "" ? 0 : Convert.ToDecimal(autumn));
                    count += (winter == "" ? 0 : Convert.ToDecimal(winter));
                    count = Math.Round(count, 2);
                    //如果是负数 各月份的和应该大于 年预算 2014-05-26 beg
                    if (Convert.ToDecimal(year) < 0 && count <= (year == "" ? 0 : Convert.ToDecimal(year)))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第" + (k + 1) + "行填写的月度预算合计大于年度预算！请仔细确认'); EnbleTxt();", true);
                        RowsBound();
                        return;
                    }
                    // 2014-05-26 end
                    if (Convert.ToDecimal(year) >= 0 && count > (year == "" ? 0 : Convert.ToDecimal(year)))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第" + (k + 1) + "行填写的月度预算合计大于年度预算！请仔细确认'); EnbleTxt();", true);
                        RowsBound();
                        return;
                    }
                    else
                    {
                        #region 把预算内追加的金额减掉
                        string springYsnzj = (GridView2.Rows[k].FindControl("hdspringYsnZj") as HiddenField).Value.Trim();
                        string summerYsnzj = (GridView2.Rows[k].FindControl("hdsummerYsnZj") as HiddenField).Value.Trim();
                        string autumnYsnzj = (GridView2.Rows[k].FindControl("hdautumnYsnZj") as HiddenField).Value.Trim();
                        string winterYsnzj = (GridView2.Rows[k].FindControl("hdwinterYsnZj") as HiddenField).Value.Trim();
                        string yearYsnzj = (GridView2.Rows[k].FindControl("hdyearYsnZj") as HiddenField).Value.Trim();
                        decimal deSpringYsnzj = 0;
                        decimal.TryParse(springYsnzj, out deSpringYsnzj);
                        decimal deSummerYsnzj = 0;
                        decimal.TryParse(summerYsnzj, out deSummerYsnzj);
                        decimal deAutumnYsnzj = 0;
                        decimal.TryParse(autumnYsnzj, out deAutumnYsnzj);
                        decimal deWinterYsnzj = 0;
                        decimal.TryParse(winterYsnzj, out deWinterYsnzj);
                        decimal deYearYsnzj = 0;
                        decimal.TryParse(yearYsnzj, out deYearYsnzj);
                        if (!ys.spring.Equals("")) { ys.spring = (decimal.Parse(ys.spring) - deSpringYsnzj).ToString(); }
                        if (!ys.summer.Equals("")) { ys.summer = (decimal.Parse(ys.summer) - deSummerYsnzj).ToString(); }
                        if (!ys.autumn.Equals("")) { ys.autumn = (decimal.Parse(ys.autumn) - deAutumnYsnzj).ToString(); }
                        if (!ys.winter.Equals("")) { ys.winter = (decimal.Parse(ys.winter) - deWinterYsnzj).ToString(); }
                        if (!ys.year.Equals("")) { ys.year = (decimal.Parse(ys.year) - deYearYsnzj).ToString(); }
                        #endregion
                        TbMain.Add(ys);
                    }
                }
            }
            #endregion

            #region 月度
            decimal dbTotalYear = 0;
            if (sysConfig["MonthOrQuarter"] == "2")
            {
                for (int s = 0; s < GridView1.Rows.Count; s++)
                {
                    string hiddkmbh = (GridView1.Rows[s].FindControl("HiddenKmbh") as HiddenField).Value; //科目编号
                    string hdIszyys = (GridView1.Rows[s].FindControl("hdIszyys") as HiddenField).Value; //获取科目是否占用预算限额的配置项
                    string January = (GridView1.Rows[s].FindControl("txtJanuary") as TextBox).Text.Trim();
                    string February = (GridView1.Rows[s].FindControl("txtFebruary") as TextBox).Text.Trim();
                    string march = (GridView1.Rows[s].FindControl("txtmarch") as TextBox).Text.Trim();
                    string April = (GridView1.Rows[s].FindControl("txtApril") as TextBox).Text.Trim();
                    string May = (GridView1.Rows[s].FindControl("txtMay") as TextBox).Text.Trim();
                    string June = (GridView1.Rows[s].FindControl("txtJune") as TextBox).Text.Trim();
                    string July = (GridView1.Rows[s].FindControl("txtJuly") as TextBox).Text.Trim();
                    string August = (GridView1.Rows[s].FindControl("txtAugust") as TextBox).Text.Trim();
                    string September = (GridView1.Rows[s].FindControl("txtSeptember") as TextBox).Text.Trim();
                    string October = (GridView1.Rows[s].FindControl("txtOctober") as TextBox).Text.Trim();
                    string November = (GridView1.Rows[s].FindControl("txtNovember") as TextBox).Text.Trim();
                    string December = (GridView1.Rows[s].FindControl("txtDecember") as TextBox).Text.Trim();
                    string year = (GridView1.Rows[s].FindControl("txtyear") as TextBox).Text.Trim();
                    YsgcTb ys = new YsgcTb();
                    ys.kmbh = hiddkmbh;
                    ys.January = January;
                    ys.February = February;
                    ys.march = march;
                    ys.April = April;
                    ys.May = May;
                    ys.June = June;
                    ys.July = July;
                    ys.August = August;
                    ys.September = September;
                    ys.October = October;
                    ys.November = November;
                    ys.December = December;
                    ys.year = year;
                    decimal count = 0;
                    count += (January == "" ? 0 : Convert.ToDecimal(January));
                    count += (February == "" ? 0 : Convert.ToDecimal(February));
                    count += (march == "" ? 0 : Convert.ToDecimal(march));
                    count += (April == "" ? 0 : Convert.ToDecimal(April));
                    count += (May == "" ? 0 : Convert.ToDecimal(May));
                    count += (June == "" ? 0 : Convert.ToDecimal(June));
                    count += (July == "" ? 0 : Convert.ToDecimal(July));
                    count += (August == "" ? 0 : Convert.ToDecimal(August));
                    count += (September == "" ? 0 : Convert.ToDecimal(September));
                    count += (October == "" ? 0 : Convert.ToDecimal(October));
                    count += (November == "" ? 0 : Convert.ToDecimal(November));
                    count += (December == "" ? 0 : Convert.ToDecimal(December));
                    //如果是负数 各月份的和应该大于 年预算 2014-05-26 beg
                    decimal y = 0;
                    if (!decimal.TryParse(year.Equals("") ? "0" : year, out y))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第" + (s + 1) + "行填写的年预算数非法！请仔细确认'); EnbleTxt();", true);
                    }
                    if (y < 0 && Math.Round(count, 2) < y)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第" + (s + 1) + "行填写的月度预算合计大于年度预算！请仔细确认'); EnbleTxt();", true);
                        RowsBound();
                        return;
                    }
                    // 2014-05-26 end
                    if (y >= 0 && Math.Round(count, 2) > y)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('第" + (s + 1) + "行填写的月度预算合计大于年度预算！系统无法保存！请仔细确认!'); EnbleTxt();", true);
                        RowsBound();
                        return;
                    }
                    else
                    {
                        #region 把预算内追加的金额减掉
                        string JanuaryYsnzj = (GridView1.Rows[s].FindControl("hdJanuaryYsnzj") as HiddenField).Value.Trim();
                        string FebruaryYsnzj = (GridView1.Rows[s].FindControl("hdFebruaryYsnZj") as HiddenField).Value.Trim();
                        string marchYsnzj = (GridView1.Rows[s].FindControl("hdmarchYsnZj") as HiddenField).Value.Trim();
                        string AprilYsnzj = (GridView1.Rows[s].FindControl("hdAprilYsnZj") as HiddenField).Value.Trim();
                        string MayYsnzj = (GridView1.Rows[s].FindControl("hdMayYsnZj") as HiddenField).Value.Trim();
                        string JuneYsnzj = (GridView1.Rows[s].FindControl("hdJuneYsnZj") as HiddenField).Value.Trim();
                        string JulyYsnzj = (GridView1.Rows[s].FindControl("hdJulyYsnZj") as HiddenField).Value.Trim();
                        string AugustYsnzj = (GridView1.Rows[s].FindControl("hdAugustYsnZj") as HiddenField).Value.Trim();
                        string SeptemberYsnzj = (GridView1.Rows[s].FindControl("hdSeptemberYsnZj") as HiddenField).Value.Trim();
                        string OctoberYsnzj = (GridView1.Rows[s].FindControl("hdOctoberYsnZj") as HiddenField).Value.Trim();
                        string NovemberYsnzj = (GridView1.Rows[s].FindControl("hdNovemberYsnZj") as HiddenField).Value.Trim();
                        string DecemberYsnzj = (GridView1.Rows[s].FindControl("hdDecemberYsnZj") as HiddenField).Value.Trim();
                        string yearYsnzj = (GridView1.Rows[s].FindControl("hdyearYsnZj") as HiddenField).Value.Trim();

                        decimal deJanuaryYsnzj = 0;
                        decimal.TryParse(JanuaryYsnzj, out deJanuaryYsnzj);
                        decimal deFebruaryYsnzj = 0;
                        decimal.TryParse(FebruaryYsnzj, out deFebruaryYsnzj);
                        decimal demarchYsnzj = 0;
                        decimal.TryParse(marchYsnzj, out demarchYsnzj);
                        decimal deAprilYsnzj = 0;
                        decimal.TryParse(AprilYsnzj, out deAprilYsnzj);
                        decimal deMayYsnzj = 0;
                        decimal.TryParse(MayYsnzj, out deMayYsnzj);
                        decimal deJuneYsnzj = 0;
                        decimal.TryParse(JuneYsnzj, out deJuneYsnzj);
                        decimal deJulyYsnzj = 0;
                        decimal.TryParse(JulyYsnzj, out deJulyYsnzj);
                        decimal deAugustYsnzj = 0;
                        decimal.TryParse(AugustYsnzj, out deAugustYsnzj);
                        decimal deSeptemberYsnzj = 0;
                        decimal.TryParse(SeptemberYsnzj, out deSeptemberYsnzj);
                        decimal deOctoberYsnzj = 0;
                        decimal.TryParse(OctoberYsnzj, out deOctoberYsnzj);
                        decimal deNovemberYsnzj = 0;
                        decimal.TryParse(NovemberYsnzj, out deNovemberYsnzj);
                        decimal deDecemberYsnzj = 0;
                        decimal.TryParse(DecemberYsnzj, out deDecemberYsnzj);
                        decimal deyearYsnzj = 0;
                        decimal.TryParse(yearYsnzj, out deyearYsnzj);

                        if (!ys.January.Equals(""))
                        {
                            ys.January = (decimal.Parse(ys.January) - deJanuaryYsnzj).ToString();
                        }
                        if (!ys.February.Equals(""))
                        {
                            ys.February = (decimal.Parse(ys.February) - deFebruaryYsnzj).ToString();
                        }
                        if (!ys.march.Equals(""))
                        {
                            ys.march = (decimal.Parse(ys.march) - demarchYsnzj).ToString();
                        }
                        if (!ys.April.Equals(""))
                        {
                            ys.April = (decimal.Parse(ys.April) - deAprilYsnzj).ToString();
                        }
                        if (!ys.May.Equals(""))
                        {
                            ys.May = (decimal.Parse(ys.May) - deMayYsnzj).ToString();
                        }
                        if (!ys.June.Equals(""))
                        {
                            ys.June = (decimal.Parse(ys.June) - deJuneYsnzj).ToString();
                        }
                        if (!ys.July.Equals(""))
                        {
                            ys.July = (decimal.Parse(ys.July) - deJulyYsnzj).ToString();
                        }
                        if (!ys.August.Equals(""))
                        {
                            ys.August = (decimal.Parse(ys.August) - deAugustYsnzj).ToString();
                        }
                        if (!ys.September.Equals(""))
                        {
                            ys.September = (decimal.Parse(ys.September) - deSeptemberYsnzj).ToString();
                        }
                        if (!ys.October.Equals(""))
                        {
                            ys.October = (decimal.Parse(ys.October) - deOctoberYsnzj).ToString();
                        }
                        if (!ys.November.Equals(""))
                        {
                            ys.November = (decimal.Parse(ys.November) - deNovemberYsnzj).ToString();
                        }
                        if (!ys.December.Equals(""))
                        {
                            ys.December = (decimal.Parse(ys.December) - deDecemberYsnzj).ToString();
                        }
                        if (!ys.year.Equals(""))
                        {
                            ys.year = (decimal.Parse(ys.year) - deyearYsnzj).ToString();
                        }
                        #endregion
                        TbMain.Add(ys);
                    }
                    //计算本表的年度预算额度合计
                    decimal dbeveYear = 0;
                    if (decimal.TryParse(year, out dbeveYear) && hdIszyys.Equals("1"))//占用预算的科目
                    {
                        dbTotalYear += dbeveYear;
                    }
                }
            }
            #endregion



            decimal fTotalAmout = 0;
            decimal ftotalyue1 = 0;
            decimal ftotalyue2 = 0;
            decimal ftotalyue3 = 0;
            decimal ftotalyue4 = 0;
            decimal ftotalyue5 = 0;
            decimal ftotalyue6 = 0;
            decimal ftotalyue7 = 0;
            decimal ftotalyue8 = 0;
            decimal ftotalyue9 = 0;
            decimal ftotalyue10 = 0;
            decimal ftotalyue11 = 0;
            decimal ftotalyue12 = 0;

            //传入参数jecheckflg 如果jecheckflg =0表示年度金额必须等于各月份的分解金额 如果jecheckflg=1表示年度金额必须大于等于各月份分解金额。
            if (!string.IsNullOrEmpty(jecheckflg) && jecheckflg == "0")
            {
                //1. 获取科目的年分解金额
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    #region 获取各月份填报数
                    string km = GridView1.Rows[i].Cells[1].Text;
                    decimal fJanuary = 0;

                    TextBox jeJanuary = GridView1.Rows[i].FindControl("txtJanuary") as TextBox;
                    if (jeJanuary.Text != "" && jeJanuary.Text != null)
                    {
                        decimal.TryParse(jeJanuary.Text, out fJanuary);
                        ftotalyue1 += fJanuary;
                    }

                    decimal fFebruary = 0;
                    TextBox jeFebruary = GridView1.Rows[i].FindControl("txtFebruary") as TextBox;
                    if (jeFebruary.Text != "" && jeFebruary.Text != null)
                    {
                        decimal.TryParse(jeFebruary.Text, out fFebruary);
                        ftotalyue2 += fFebruary;
                    }

                    decimal fmarch = 0;
                    TextBox jemarchy = GridView1.Rows[i].FindControl("txtmarch") as TextBox;
                    if (jemarchy.Text != "" && jemarchy.Text != null)
                    {
                        decimal.TryParse(jemarchy.Text, out fmarch);
                        ftotalyue3 += fmarch;

                    }

                    decimal fApril = 0;
                    TextBox jetxtApril = GridView1.Rows[i].FindControl("txtApril") as TextBox;
                    if (jetxtApril.Text != "" && jetxtApril.Text != null)
                    {
                        decimal.TryParse(jetxtApril.Text, out fApril);
                        ftotalyue4 += fApril;
                    }

                    decimal fMay = 0;
                    TextBox jetxtMay = GridView1.Rows[i].FindControl("txtMay") as TextBox;
                    if (jetxtMay.Text != "" && jetxtMay.Text != null)
                    {
                        decimal.TryParse(jetxtMay.Text, out fMay);
                        ftotalyue5 += fMay;

                    }


                    decimal fJune = 0;
                    TextBox jetxtJune = GridView1.Rows[i].FindControl("txtJune") as TextBox;
                    if (jetxtJune.Text != "" && jetxtJune.Text != null)
                    {
                        decimal.TryParse(jetxtJune.Text, out fJune);
                        ftotalyue6 += fJune;
                    }


                    decimal fJuly = 0;
                    TextBox jetxtJuly = GridView1.Rows[i].FindControl("txtJuly") as TextBox;
                    if (jetxtJuly.Text != "" && jetxtJuly.Text != null)
                    {
                        decimal.TryParse(jetxtJuly.Text, out fJuly);
                        ftotalyue7 += fJuly;

                    }

                    decimal fAugust = 0;
                    TextBox jetxtAugust = GridView1.Rows[i].FindControl("txtAugust") as TextBox;
                    if (jetxtAugust.Text != "" && jetxtAugust.Text != null)
                    {
                        decimal.TryParse(jetxtAugust.Text, out fAugust);
                        ftotalyue8 += fAugust;

                    }
                    decimal fSeptember = 0;
                    TextBox jetxtSeptember = GridView1.Rows[i].FindControl("txtSeptember") as TextBox;
                    if (jetxtSeptember.Text != "" && jetxtSeptember.Text != null)
                    {
                        decimal.TryParse(jetxtSeptember.Text, out fSeptember);
                        ftotalyue9 += fSeptember;
                    }

                    decimal fOctober = 0;
                    TextBox jetxtOctober = GridView1.Rows[i].FindControl("txtOctober") as TextBox;
                    if (jetxtOctober.Text != "" && jetxtOctober.Text != null)
                    {
                        decimal.TryParse(jetxtOctober.Text, out fOctober);
                        ftotalyue10 += fOctober;
                    }

                    decimal fNovember = 0;
                    TextBox jetxtNovember = GridView1.Rows[i].FindControl("txtNovember") as TextBox;
                    if (jetxtNovember.Text != "" && jetxtNovember.Text != null)
                    {
                        decimal.TryParse(jetxtNovember.Text, out fNovember);
                        ftotalyue11 += fNovember;
                    }

                    decimal fDecember = 0;
                    TextBox jetxtDecember = GridView1.Rows[i].FindControl("txtDecember") as TextBox;
                    if (jetxtDecember.Text != "" && jetxtDecember.Text != null)
                    {
                        decimal.TryParse(jetxtDecember.Text, out fDecember);
                        ftotalyue12 += fDecember;
                    }

                    decimal flAmount = 0;

                    TextBox je = GridView1.Rows[i].FindControl("txtyear") as TextBox;

                    if (je.Text != "" && je.Text != null)
                    {
                        decimal.TryParse(je.Text, out flAmount);
                        fTotalAmout += flAmount;
                    }
                    #endregion
                    decimal yfamount = fDecember + fNovember + fOctober + fSeptember + fAugust + fJuly + fJune + fMay + fApril + fmarch + fFebruary + fJanuary;
                    if (flAmount != yfamount)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + km + "科目 填报金额(" + yfamount.ToString("N") + ")不等于总分解额度(" + flAmount.ToString("N") + ")'); window.returnValue='1';", true);
                        RowsBound();
                        return;
                    }
                }
            }
            else
            {
                if (Request["xkfx"] != null && (!string.IsNullOrEmpty(Request["xkfx"].ToString())) && Request["xkfx"] == "1")//如果是新开分校不进行预算控制
                {

                }
                else
                {
                    //验证本表预算额度有没有超
                    if (IsLimitTotal.Equals("1"))
                    {
                        //string hzje = gethzje();//获取各分校总金额
                        //decimal dechzje = 0;
                        //if (!string.IsNullOrEmpty(hzje))
                        //{
                        //    dechzje = decimal.Parse(hzje);
                        //}
                        decimal deckzje = deDeptTotal;// - dechzje;//控制金额=预算总金额-各分校汇总金额  把这个控制放到了提交的时候了
                        //获取上一级分解的金额
                        if (dbTotalYear > deckzje)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('填报金额(" + dbTotalYear.ToString("N") + ")不得大于分配的总额度(" + deckzje.ToString("N") + ")，已超出：" + (dbTotalYear - deckzje) + "'); window.returnValue='1';", true);
                            RowsBound();
                            return;
                        }
                    }
                }

            }
            //02是财务填报  表   部门编号
            string usercode = hdUserCode.Value;//Session["userCode"].ToString().Trim();
            string xmcode = "";
            if (Request["xmcode"] != null && Request["xmcode"].ToString() != "")
            {
                xmcode = Request["xmcode"].ToString();
            }
            if (bll.Addtb(TbMain, deptcode, tblx, nd, usercode, "-1", flowid, xmcode))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功'); window.returnValue='1';fh();", true);
                RowsBound();
            }
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void btn_reload_Click(object sender, EventArgs e)
    {
        object objrel = Session["nowdt"];
        if (objrel == null)
        {
            return;
        }
        DataTable dt = (DataTable)objrel;
        Session.Remove("nowdt");
        //int irowcount = GridView1.Rows.Count;
        //for (int i = 0; i < irowcount; i++)
        //{
        //    TextBox txt1 = GridView1.Rows[i].Cells[2].FindControl("txtJanuary") as TextBox;
        //    if (txt1!=null)
        //    {
        //        txt1.Text=dt.Rows[i].
        //    }
        //}
        GridView1.DataSource = objrel;
        GridView1.DataBind();
    }
    /// <summary>
    /// 获取部门的年度预算额度 不同的预算获取年度预算的数据源不同 所以写了这个方法
    /// </summary>
    /// <param name="Year"></param>
    /// <param name="deptCode"></param>
    /// <param name="kmType">预算科目类型 01 收入 02 费用 03 固定资产 04 存货 05 往来</param>
    /// <returns></returns>
    private decimal getDeptNdAmount(string Year, string deptCode, string kmType)
    {
        decimal result = 0;
        if (kmType.Equals("02"))
        {
            string totalamount = server.GetCellValue("select isnull(sum(ddefine7),0) from bill_deptFyblDy where cdefine1='" + Year + "' and deptcode='" + deptCode + "'"); //server.GetCellValue("select isnull(sum(xjje),0) from bill_deptfj where nd='" + Year + "' and deptcode='" + deptCode + "'");
            decimal.TryParse(totalamount, out result);
            return result;
        }
        return 0;
    }

    decimal deyi = 0, deer = 0, desan = 0, desi = 0, dewu = 0, deliu = 0, deqi = 0, deba = 0, dejiu = 0, deshi = 0, deshiyi = 0, deshier = 0, denian = 0, dejd1 = 0, dejd2 = 0, dejd3 = 0, dejd4 = 0;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            #region 合计行
            decimal decnian = 0;
            TextBox txtnian = e.Row.Cells[2].FindControl("txtyear") as TextBox;

            if (txtnian != null)
            {
                string strnian = txtnian.Text;

                if (decimal.TryParse(strnian, out decnian))
                {
                    denian += decnian;
                }
            }
            decimal decyue1 = 0;
            TextBox txtyue1 = e.Row.Cells[3].FindControl("txtJanuary") as TextBox;
            if (txtyue1 != null)
            {
                string stryue1 = txtyue1.Text.Trim();
                if (decimal.TryParse(stryue1, out decyue1))
                {
                    deyi += decyue1;
                }
            }
            decimal decyue2 = 0;
            TextBox txtyue2 = e.Row.Cells[4].FindControl("txtFebruary") as TextBox;
            if (txtyue2 != null)
            {
                string stryue2 = txtyue2.Text.Trim();
                if (decimal.TryParse(stryue2, out decyue2))
                {
                    deer += decyue2;
                }
            }
            decimal decyue3 = 0;
            TextBox txtyue3 = e.Row.Cells[5].FindControl("txtmarch") as TextBox;
            if (txtyue3 != null)
            {
                string stryue3 = txtyue3.Text.Trim();
                if (decimal.TryParse(stryue3, out decyue3))
                {
                    desan += decyue3;
                }
            }

            //decimal dejdyi = 0;



            decimal decyue4 = 0;
            TextBox txtyue4 = e.Row.Cells[7].FindControl("txtApril") as TextBox;
            if (txtyue4 != null)
            {
                string stryue4 = txtyue4.Text.Trim();
                if (decimal.TryParse(stryue4, out decyue4))
                {
                    desi += decyue4;
                }
            }
            decimal decyue5 = 0;
            TextBox txtyue5 = e.Row.Cells[8].FindControl("txtMay") as TextBox;
            if (txtyue5 != null)
            {
                string stryue5 = txtyue5.Text.Trim();
                if (decimal.TryParse(stryue5, out decyue5))
                {
                    dewu += decyue5;
                }
            }
            decimal decyue6 = 0;
            TextBox txtyue6 = e.Row.Cells[9].FindControl("txtJune") as TextBox;
            if (txtyue6 != null)
            {
                string stryue6 = txtyue6.Text.Trim();
                if (decimal.TryParse(stryue6, out decyue6))
                {
                    deliu += decyue6;
                }
            }
            decimal decyue7 = 0;
            TextBox txtyue7 = e.Row.Cells[11].FindControl("txtJuly") as TextBox;
            if (txtyue7 != null)
            {
                string stryue7 = txtyue7.Text.Trim();
                if (decimal.TryParse(stryue7, out decyue7))
                {
                    deqi += decyue7;
                }
            }
            decimal decyue8 = 0;
            TextBox txtyue8 = e.Row.Cells[12].FindControl("txtAugust") as TextBox;
            if (txtyue8 != null)
            {
                string stryue8 = txtyue8.Text.Trim();
                if (decimal.TryParse(stryue8, out decyue8))
                {
                    deba += decyue8;
                }
            }
            decimal decyue9 = 0;
            TextBox txtyue9 = e.Row.Cells[13].FindControl("txtSeptember") as TextBox;
            if (txtyue9 != null)
            {
                string stryue9 = txtyue9.Text.Trim();
                if (decimal.TryParse(stryue9, out decyue9))
                {
                    dejiu += decyue9;
                }
            }
            decimal decyue10 = 0;
            TextBox txtyue10 = e.Row.Cells[15].FindControl("txtOctober") as TextBox;
            if (txtyue10 != null)
            {
                string stryue10 = txtyue10.Text.Trim();
                if (decimal.TryParse(stryue10, out decyue10))
                {
                    deshi += decyue10;
                }
            }
            decimal decyue11 = 0;
            TextBox txtyue11 = e.Row.Cells[16].FindControl("txtNovember") as TextBox;
            if (txtyue11 != null)
            {
                string stryue11 = txtyue11.Text.Trim();
                if (decimal.TryParse(stryue11, out decyue11))
                {
                    deshiyi += decyue11;
                }
            }
            decimal decyue12 = 0;
            TextBox txtyue12 = e.Row.Cells[17].FindControl("txtDecember") as TextBox;
            if (txtyue12 != null)
            {
                string stryue12 = txtyue12.Text.Trim();
                if (decimal.TryParse(stryue12, out decyue12))
                {
                    deshier += decyue12;
                }
            }
            #endregion
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "合计：";
            e.Row.Cells[2].Text = denian.ToString("0.00");

            e.Row.Cells[3].Text = deyi.ToString("0.00");
            e.Row.Cells[4].Text = deer.ToString("0.00");
            e.Row.Cells[5].Text = desan.ToString("0.00");

            e.Row.Cells[7].Text = desi.ToString("0.00");
            e.Row.Cells[8].Text = dewu.ToString("0.00");
            e.Row.Cells[9].Text = deliu.ToString("0.00");

            e.Row.Cells[11].Text = deqi.ToString("0.00");
            e.Row.Cells[12].Text = deba.ToString("0.00");
            e.Row.Cells[13].Text = dejiu.ToString("0.00");

            e.Row.Cells[15].Text = deshi.ToString("0.00");
            e.Row.Cells[16].Text = deshiyi.ToString("0.00");
            e.Row.Cells[17].Text = deshier.ToString("0.00");


        }
    }
    protected void btn_hzfx_Click(object sender, EventArgs e)
    {
        string strdept = Request["deptCode"];
        string strnd = Request["nd"];
        string strsql = " exec [pro_gkdeptys_hz] '" + strdept + "','" + strnd + "' ";

        DataTable dt = server.GetDataTable(strsql, null);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        RowsBound();
    }
    /// <summary>
    /// 上传附件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnScdj_Click(object sender, EventArgs e)
    {
        string filePath = "";
        string Name = "";
        string name = "";
        string exname = "";
        if (upLoadFiles.Visible == true)
        {
            string script;
            if (upLoadFiles.PostedFile.FileName == "")
            {
                laFilexx.Text = "请选择文件";
                return;
            }
            else
            {
                try
                {
                    filePath = upLoadFiles.PostedFile.FileName;
                    Name = this.upLoadFiles.PostedFile.FileName;
                    exname = System.IO.Path.GetExtension(Name);
                    if (isOK(exname))
                    {
                        string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                        string fileSn = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        ////转换成绝对地址,
                        string serverpath = Server.MapPath(@"~\Uploads\ystb\") + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////转换成与相对地址,相对地址为将来访问图片提供
                        string relativepath = @"~\Uploads\ystb\" + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////绝对地址用来将上传文件夹保存到服务器的具体路下。
                        if (!Directory.Exists(Server.MapPath(@"~\Uploads\ystb\")))
                        {
                            Directory.CreateDirectory(Server.MapPath(@"~\Uploads\ystb\"));
                        }
                        upLoadFiles.PostedFile.SaveAs(serverpath);
                        ////把相对路径的地址保存到页面hdImageUrl的value值上以供保存值时使用。
                        hiddFileDz.Value = relativepath;
                        Lafilename.Text = filename;
                        laFilexx.Text = "上传成功";
                        //保存附件
                        string fujian = this.hiddFileDz.Value;
                        string sql = "delete from bill_ysfj where deptcode='" + deptcode + "' and nd='" + nd + "';insert into bill_ysfj(deptcode,nd,fujian,filename,xmbh) values('" + deptcode + "','" + nd + "','" + fujian + "','" + Lafilename.Text.Trim() + "','" + xmbh + "')";
                        new sqlHelper.sqlHelper().ExecuteNonQuery(sql);
                        //  btn_sc.Text = "修改附件";
                        //upLoadFiles.Visible = false;
                    }
                    else
                    {
                        Response.Write("<script>alert('文件类型不合法');</script>");
                    }
                }
                catch (Exception ex)
                {
                    laFilexx.Text = ex.ToString();
                }
            }
        }
        else
        {
            btn_sc.Text = "上传";
            //laFilexx.Text = "";
            upLoadFiles.Visible = true;
            Lafilename.Text = "";
        }
    }
    bool isOK(string exname)
    {
        if (exname.ToLower() == ".doc" || exname.ToLower() == ".docx" || exname.ToLower() == ".jpg" || exname.ToLower() == ".png" || exname.ToLower() == ".gif" || exname.ToLower() == ".xls" || exname.ToLower() == ".xlsx" || exname.ToLower() == ".zip" || exname.ToLower() == ".txt" || exname.ToLower() == ".pdf" || exname.ToLower() == ".rar" || exname.ToLower() == ".ppt")
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    /// <summary>
    /// 合计年预算
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_hznian_Click(object sender, EventArgs e)
    {
        yanzhen();

    }

    private void yanzhen()
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "", "if (confirm(‘确定将年预算金额为十二个月填报总和吗？’)) { } else { return;}", true);

        //1. 获取科目的年分解金额
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            decimal dectotalyue1 = 0;
            decimal dectotalyue2 = 0;
            decimal dectotalyue3 = 0;
            decimal dectotalyue4 = 0;
            decimal dectotalyue5 = 0;
            decimal dectotalyue6 = 0;
            decimal dectotalyue7 = 0;
            decimal dectotalyue8 = 0;
            decimal dectotalyue9 = 0;
            decimal dectotalyue10 = 0;
            decimal dectotalyue11 = 0;
            decimal dectotalyue12 = 0;
            #region 获取各月份填报数
            string km = GridView1.Rows[i].Cells[1].Text;
            decimal fJanuary = 0;

            TextBox jeJanuary = GridView1.Rows[i].FindControl("txtJanuary") as TextBox;
            if (jeJanuary.Text != "" && jeJanuary.Text != null)
            {
                decimal.TryParse(jeJanuary.Text, out fJanuary);//1月份
                dectotalyue1 = fJanuary;
            }

            decimal fFebruary = 0;
            TextBox jeFebruary = GridView1.Rows[i].FindControl("txtFebruary") as TextBox;
            if (jeFebruary.Text != "" && jeFebruary.Text != null)
            {
                decimal.TryParse(jeFebruary.Text, out fFebruary);
                dectotalyue2 = fFebruary;
            }

            decimal fmarch = 0;
            TextBox jemarchy = GridView1.Rows[i].FindControl("txtmarch") as TextBox;
            if (jemarchy.Text != "" && jemarchy.Text != null)
            {
                decimal.TryParse(jemarchy.Text, out fmarch);
                dectotalyue3 = fmarch;

            }

            decimal fApril = 0;
            TextBox jetxtApril = GridView1.Rows[i].FindControl("txtApril") as TextBox;
            if (jetxtApril.Text != "" && jetxtApril.Text != null)
            {
                decimal.TryParse(jetxtApril.Text, out fApril);
                dectotalyue4 = fApril;
            }

            decimal fMay = 0;
            TextBox jetxtMay = GridView1.Rows[i].FindControl("txtMay") as TextBox;
            if (jetxtMay.Text != "" && jetxtMay.Text != null)
            {
                decimal.TryParse(jetxtMay.Text, out fMay);
                dectotalyue5 = fMay;

            }


            decimal fJune = 0;
            TextBox jetxtJune = GridView1.Rows[i].FindControl("txtJune") as TextBox;
            if (jetxtJune.Text != "" && jetxtJune.Text != null)
            {
                decimal.TryParse(jetxtJune.Text, out fJune);
                dectotalyue6 = fJune;
            }


            decimal fJuly = 0;
            TextBox jetxtJuly = GridView1.Rows[i].FindControl("txtJuly") as TextBox;
            if (jetxtJuly.Text != "" && jetxtJuly.Text != null)
            {
                decimal.TryParse(jetxtJuly.Text, out fJuly);
                dectotalyue7 = fJuly;

            }

            decimal fAugust = 0;
            TextBox jetxtAugust = GridView1.Rows[i].FindControl("txtAugust") as TextBox;
            if (jetxtAugust.Text != "" && jetxtAugust.Text != null)
            {
                decimal.TryParse(jetxtAugust.Text, out fAugust);
                dectotalyue8 = fAugust;

            }
            decimal fSeptember = 0;
            TextBox jetxtSeptember = GridView1.Rows[i].FindControl("txtSeptember") as TextBox;
            if (jetxtSeptember.Text != "" && jetxtSeptember.Text != null)
            {
                decimal.TryParse(jetxtSeptember.Text, out fSeptember);
                dectotalyue9 = fSeptember;
            }

            decimal fOctober = 0;
            TextBox jetxtOctober = GridView1.Rows[i].FindControl("txtOctober") as TextBox;
            if (jetxtOctober.Text != "" && jetxtOctober.Text != null)
            {
                decimal.TryParse(jetxtOctober.Text, out fOctober);
                dectotalyue10 = fOctober;
            }

            decimal fNovember = 0;
            TextBox jetxtNovember = GridView1.Rows[i].FindControl("txtNovember") as TextBox;
            if (jetxtNovember.Text != "" && jetxtNovember.Text != null)
            {
                decimal.TryParse(jetxtNovember.Text, out fNovember);
                dectotalyue11 = fNovember;
            }

            decimal fDecember = 0;
            TextBox jetxtDecember = GridView1.Rows[i].FindControl("txtDecember") as TextBox;
            if (jetxtDecember.Text != "" && jetxtDecember.Text != null)
            {
                decimal.TryParse(jetxtDecember.Text, out fDecember);
                dectotalyue12 = fDecember;
            }

            //decimal flAmount = 0;

            TextBox YearJe = GridView1.Rows[i].FindControl("txtyear") as TextBox;
            YearJe.Text = (dectotalyue12 + dectotalyue11 + dectotalyue10 + dectotalyue9 + dectotalyue8 + dectotalyue7 + dectotalyue6 + dectotalyue5 + dectotalyue4 + dectotalyue3 + dectotalyue2 + dectotalyue1).ToString();


            #endregion

        }


    }
    void getYskmsNoYs()
    {
        string sql = "select yskm.yskmcode from bill_yskm yskm,bill_yskm_dept dept where yskm.yskmcode=dept.yskmcode and dept.deptcode='" + deptcode + "' and  iszyys='0'";
        DataTable dt = server.GetDataTable(sql, null);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            lstYskmsNoYs.Add(dt.Rows[i][0].ToString());
        }
    }
}
