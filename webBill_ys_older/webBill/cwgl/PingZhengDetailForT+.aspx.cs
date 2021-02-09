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
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dal;

public partial class webBill_cwgl_PingZhengDetailForT_ : System.Web.UI.Page
{
    string strCode = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    List<string> lstOsDept = new List<string>();
    string strInBillCode = "";//可用于sql查询的编号字符串
    string djlx = string.Empty;//单据类型（bill_yskm_dept）
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        //}
        //else
        //{
        object objCode = Request["Code"];
        if (objCode != null)
        {
            strCode = objCode.ToString();
        }
        object objdjlx = Request["djlx"];
        if (objdjlx != null)
        {
            djlx = objdjlx.ToString();
        }
        //组合可用于sql查询的编号字符串
        string[] arrCode = strCode.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < arrCode.Length; i++)
        {
            strInBillCode += string.Format("'{0}',", arrCode[i]);
        }
        strInBillCode = strInBillCode.Substring(0, strInBillCode.Length - 1);

        if (!IsPostBack)
        {
            this.bindZhangTao();
            bindZhaiYao();
            this.bindNd();
        }
        if (!IsPostBack)
        {
            this.txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.BindDataGrid();
            bindPingZhengType();
        }
        //}
    }



    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        string strBillCount = this.txtBillCount.Text.Trim();
        int iBillCount = -1;
        if (!int.TryParse(strBillCount, out iBillCount))
        {
            showMessage("附加单据数必须为阿拉伯数字！", false, "");
            return;
        }
        string strZhangTaodb = this.ddlZhangTao.SelectedValue;
        strZhangTaodb = strZhangTaodb.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries)[1];
        if (string.IsNullOrEmpty(strZhangTaodb))
        {
            showMessage("必须选择帐套！", false, "");
            return;
        }
        //凭证类型
        string csign = "记账凭证";//凭证类型
        string iSignSeq = "";//凭证类别排序号 
        string iddoctype = "";//凭证类型ID
        if (this.ddlPingZhengType.Items.Count < 1 || this.ddlPingZhengType.SelectedValue == null)
        {
            showMessage("请先选择凭证类型", false, ""); return;
        }
        else
        {
            string strval = this.ddlPingZhengType.SelectedValue;
            string[] arrval = strval.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            csign = arrval[0];
            iSignSeq = arrval[1];
            iddoctype = arrval[2];
        }
        //时间
        string strBillDate = this.txtDate.Text.Trim();
        DateTime dt;
        if (!DateTime.TryParse(strBillDate, out dt))
        {
            showMessage("单据时间不合法！", false, "");
            return;
        }
        //验证制单日期对应的帐套是否正确
        //string strzhangtaoname = this.ddlZhangTao.SelectedItem.Text;
        //if (!dt.Year.ToString().Equals(strzhangtaoname.Substring(strzhangtaoname.Length - 4, 4)))
        //{
        //    showMessage("制单日期与选择的帐套不对应，请重新选择", false, ""); return;
        //}
        //连接服务器地址
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
        if (strlinkdbname.Equals(""))
        {
            showMessage("没有找到用友系统数据库链接", false, "");
            return;
        }
        string docbusinesstype = "75D00435-6A3E-4804-A701-32EDBFE1B588";//凭证业务类型
        //获取凭证号
        string strpingzhenghao = server.GetCellValue("select right('000000'+cast(isnull(max(code),'0')+1 as varchar(10)),5) from [" + strlinkdbname + "]." + strZhangTaodb + ".dbo.GL_Doc where AccountingPeriod='" + dt.Month.ToString() + "' and DocBusinessType='" + docbusinesstype + "' and AccountingYear='" + this.ddlNd.SelectedValue + "'");
        //获取外部接口号
        string strcoutno_id = "";

        using (SqlConnection conn = new SqlConnection(DataHelper.constr))
        {
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                List<pingzheng> lstpingzheng = new List<pingzheng>();
                int irows = this.GridView.Rows.Count;
                string ccode_equaljie = "";//借方的对方科目编码 
                string ccode_equaldai = "";//贷方的对方科目编码
                Guid strNewid = Guid.NewGuid();//凭证ID
                string Maker = "孙桂平";//制单人名称
                string MakerId = "115A8208-E74C-4310-B8DA-9AD6CBFAB6D5";//制单人id  现场配置
                string dbill_date = strBillDate;//制单日期
                string iyear = dt.Year.ToString();//凭证的会计年度
                string iperiod = dt.Month.ToString();//会计期间
                string sqlGetPeriod = string.Format("select id from [{0}].{1}.dbo.sm_period where currentyear='{2}' and currentperiod='{3}'", strlinkdbname, strZhangTaodb, dt.Year.ToString(), dt.Month.ToString());
                string idperiod = server.GetCellValue(sqlGetPeriod, null);// idperiod 

                for (int i = 0; i < irows; i++)
                {
                    string ccode = "";//科目编码
                    TextBox txtmxkm = this.GridView.Rows[i].Cells[3].FindControl("txtMingXiKemu") as TextBox;
                    if (txtmxkm != null)
                    {
                        ccode = txtmxkm.Text.Trim();
                        ccode = ccode.Equals("&nbsp;") ? "" : ccode;
                    }
                    if (ccode.Equals(""))
                    {
                        showMessage("第" + (i + 1) + "行明细科目不能为空，请先到部门预算科目处添加对应或检查对应的财务科目是否为末级科目。", false, "");
                        return;
                    }
                    if (ccode.IndexOf("]") > -1)
                    {
                        ccode = ccode.Substring(1, ccode.IndexOf(']') - 1);
                    }
                    //拼字符串  对方科目编码
                    string strpingzhengtype = this.GridView.Rows[i].Cells[0].Text.Trim().Replace("&nbsp;", "");
                    if (strpingzhengtype.Equals("jf") && ccode_equaldai.IndexOf(ccode) < 0 && ccode_equaljie.Length < 40)
                    {
                        ccode_equaldai = ccode_equaldai + ccode + ",";
                    }
                    else if (strpingzhengtype.Equals("df") && ccode_equaljie.IndexOf(ccode) < 0 && ccode_equaljie.Length < 40)
                    {
                        ccode_equaljie = ccode_equaljie + ccode + ",";
                    }
                    else { }

                    bool bohasxianjinliuhesuan = this.GridView.Rows[i].Cells[13].Text.Trim().Replace("&nbsp;", "").Equals("True");//是否包含现金流量核算
                    #region 现金流量项目验证
                    //string strxianjinliuliangcode = "";

                    //DropDownList ddlxianjinliuliang = this.GridView.Rows[i].Cells[4].FindControl("ddlxianjinliu") as DropDownList;
                    //if (ddlxianjinliuliang != null)
                    //{
                    //    strxianjinliuliangcode = ddlxianjinliuliang.SelectedValue;
                    //    if (bohasxianjinliuhesuan && strxianjinliuliangcode.Equals(""))
                    //    {
                    //        showMessage("第" + (i + 1).ToString() + "行有现金流量项目核算，请选择明细", false, ""); return;
                    //    }
                    //}
                    //else
                    //{
                    //    showMessage("第" + (i + 1).ToString() + "行有现金流量项目核算，请选择明细", false, ""); return;
                    //}
                    #endregion
                }
                // 对方科目编码  去掉最后的字符
                if (ccode_equaldai.Length > 1)
                {
                    ccode_equaldai = ccode_equaldai.Substring(0, ccode_equaldai.Length - 1);
                }
                if (ccode_equaljie.Length > 1)
                {
                    ccode_equaljie = ccode_equaljie.Substring(0, ccode_equaljie.Length - 1);
                }
                double dbsummd = 0;//借方金额总数  （用于检测借贷平衡）
                double dbsummc = 0;//贷方金额总数（用于检测借贷平衡）
                //获取货币id
                string idcurrencysql = string.Format("select id from [{0}].{1}.dbo.AA_Currency where name='人民币'", strlinkdbname, strZhangTaodb);
                string strcurrency = server.GetCellValue(idcurrencysql, null);
                Guid idcurrency = new Guid(strcurrency);
                for (int i = 0; i < irows; i++)
                {
                    double md = 0;//借方金额
                    #region 借方金额
                    TextBox jfje = this.GridView.Rows[i].Cells[7].FindControl("jfje") as TextBox;
                    if (jfje == null)
                    {
                        showMessage("行" + (i + 1) + "借方金额没有被检测到，请联系开发商解决！", false, "");
                        return;
                    }
                    string strJie = jfje.Text.Trim();
                    if (!double.TryParse(strJie, out md))
                    {
                        showMessage("行" + (i + 1) + "借方金额不合法！", false, "");
                        return;
                    }
                    #endregion
                    double mc = 0;//贷方金额
                    #region 贷方金额
                    TextBox dfje = this.GridView.Rows[i].Cells[8].FindControl("dfje") as TextBox;
                    if (dfje == null)
                    {
                        showMessage("行" + (i + 1) + "贷方金额没有被检测到，请联系开发商解决！", false, "");
                        return;
                    }
                    string strDai = dfje.Text.Trim();
                    if (!double.TryParse(strDai, out mc))
                    {
                        showMessage("行" + i + "贷方金额不合法！", false, "");
                        return;
                    }
                    #endregion
                    string cdept_id = "";//核算部门id
                    #region 核算部门
                    TextBox txtfordept = this.GridView.Rows[i].Cells[4].FindControl("txtForDept") as TextBox;
                    cdept_id = txtfordept.Text.Trim().Replace("&nbsp;", "");
                    bool bdept = this.GridView.Rows[i].Cells[14].Text.Trim().Replace("&nbsp;", "").Equals("True");//是否有部门核算
                    if (bdept && cdept_id.Equals(""))
                    {
                        showMessage("第" + (i + 1).ToString() + "行有部门核算，请到‘部门档案’下维护对应T+系统id或直接填写", false, ""); return;
                    }
                    else if (bdept && !cdept_id.Equals(""))
                    {
                        //判断核算部门是否是末级部门
                        bool boislast = this.islastdept(cdept_id);
                        if (!boislast)
                        {
                            showMessage("第" + (i + 1).ToString() + "行的" + cdept_id + "部门对应U8id不是末级部门，请修改。", false, ""); return;
                        }
                    }
                    #endregion
                    string cdigest = "";//摘要
                    #region 获取行摘要
                    TextBox txtZhaiYao = this.GridView.Rows[i].Cells[2].FindControl("txtZhaiYao") as TextBox;
                    if (txtZhaiYao != null)
                    {
                        cdigest = txtZhaiYao.Text.Trim();
                    }
                    #endregion

                    //插入分录表
                    string id = "";//唯一主键
                    string icode = "00000" + i.ToString();//排序号  i
                    icode = icode.Substring(icode.Length - 4, 4);
                    int exchangerate = 1;//汇率

                    //获取财务科目主键
                    Guid idAccount;
                    string strkm = this.GridView.Rows[i].Cells[23].Text.Trim();
                    if (string.IsNullOrEmpty(strkm))
                    {
                        showMessage("会计科目不能为空，请联系管理员解决", false, ""); return;
                    }
                    else
                    {
                        idAccount = new Guid(strkm);
                    }
                    #region 插入分录表
                    Guid fenluGuid = Guid.NewGuid();
                    //唯一主键   排序号  摘要  汇率   原币借方  原币贷方
                    //本币借方  本币贷方  序号（未知）  时间戳  货币id  会计科目id  iddocdto 
                    string fenluSql = @"
                                           insert into [{0}].{1}.dbo.GL_Entry(
                                                     id,code,summary,exchangerate,origamountdr,origamountcr
                                                     ,amountdr,amountcr,sequencenumber,idcurrency,idaccount,iddocdto)
                                           values(
                                                     @id,@code,@summary,@exchangerate,@origamountdr,@origamountcr
                                                     ,@amountdr,@amountcr,@sequencenumber,@idcurrency,@idaccount,@iddocdto
                                                 )
                                       ";
                    SqlParameter[] fenluSp = { new SqlParameter("@id",fenluGuid),new SqlParameter("@code",icode),new SqlParameter("@summary",cdigest),new SqlParameter("@exchangerate",exchangerate),new SqlParameter("@origamountdr",md),new SqlParameter("@origamountcr",mc),
                                             new SqlParameter("@amountdr",md),new SqlParameter("@amountcr",mc),new SqlParameter("@sequencenumber","0"),new SqlParameter("@idcurrency",idcurrency),new SqlParameter("@idaccount",idAccount),new SqlParameter("@iddocdto",strNewid)
                                             };
                    fenluSql = string.Format(fenluSql, strlinkdbname, strZhangTaodb);
                    lstpingzheng.Add(new pingzheng() { sql = fenluSql, arrsp = fenluSp });
                    #endregion

                    #region 插入明细表
                    //行主键  凭证id  分录id  凭证号  行号 摘要 制单人姓名 制单日期 是否计税 借贷方向 转换率 是否转出  会计年度 会计月度 币种 会计科目id 会计期间id md mc md 凭证类型 mc 是否期初 制单人id 
                    string mxSql = @"  insert into [{0}].{1}.dbo.GL_Journal(id,docid,entryid,docno,rowno,summary,maker,madedate,istaxed,direction,exchangerate,iscarriedforwardout,iscarriedforwardin,ispost,year,currentperiod,idcurrency,idaccount,idaccountingperiod,amountDr,amountCr,origAmountDr,idDocType,origamountcr,isPeriodBegin,makerid,DocDrCrType)
                        values(@id,@docid,@entryid,@docno,@rowno,@summary,@maker,@madedate,@istaxed,@direction,@exchangerate,@iscarriedforwardout,@iscarriedforwardin,@ispost,@year,@currentperiod,@idcurrency,@idaccount,@idaccountingperiod,@amountDr,@amountCr,@origAmountDr,@idDocType,@origamountcr,@isPeriodBegin,@makerid,@DocDrCrType
                        )
                    ";
                    string direction = "";//借贷方向BCB904BB-5154-4B1F-8A46-6ECAF2BE4E12，36489A73-7C4F-4C90-A349-ED76ABE47990
                    SqlParameter[] mxSp = { new SqlParameter("@id",Guid.NewGuid()), new SqlParameter("@docid",strNewid), new SqlParameter("@entryid",fenluGuid), new SqlParameter("@docno",strpingzhenghao), new SqlParameter("@rowno",icode), new SqlParameter("@summary",cdigest), new SqlParameter("@maker",Maker), new SqlParameter("@madedate",dbill_date+" 00:00:00"), new SqlParameter("@IsTaxed","0"), new SqlParameter("@direction",direction), new SqlParameter("@exchangerate",1), new SqlParameter("@iscarriedforwardout",0), new SqlParameter("@iscarriedforwardin",0), new SqlParameter("@ispost",0), new SqlParameter("@year",iyear), new SqlParameter("@currentperiod",iperiod), new SqlParameter("@idcurrency",idcurrency), new SqlParameter("@idaccount",idAccount), new SqlParameter("@idaccountingPeriod",idperiod), new SqlParameter("@amountDr",md), new SqlParameter("@amountCr",mc), new SqlParameter("@origAmountDr",md), new SqlParameter("@idDocType",iddoctype), new SqlParameter("@origamountcr",mc), new SqlParameter("@isPeriodBegin",0), new SqlParameter("@makerid",MakerId), new SqlParameter("@DocDrCrType","11")
                                             };
                    mxSql = string.Format(mxSql, strlinkdbname, strZhangTaodb);
                    lstpingzheng.Add(new pingzheng() { sql = mxSql, arrsp = mxSp });
                    #endregion

                    #region 辅助核算表

                    #endregion

                    dbsummd += md;
                    dbsummc += mc;
                }
                #region 插入凭证表


                string iperiod2 = dt.Month.ToString();//会计期间2  如果是1月份 表示为01

                string iYPeriod = iyear + iperiod;//包括年度的会计期间
                string ino_id = strpingzhenghao;// 凭证号 11.5 对应DocSystemNo 大智是11.6 不需要该字段

                string idoc = int.Parse(strBillCount) >= 1 ? strBillCount : "-1";
                string cbill = new Bll.UserProperty.UserMessage(Session["userCode"].ToString()).Users.UserName;//制单人
                //主键  凭证号 凭证系统编号   借方原币合计   贷方原币合计     借方本币合计 贷方本币合计 是否主管签字     
                //是否出纳签字    出纳名称  是否记账  记账人名称   记账人ID    记账日期  是否错误  是否作废  是否计税 是否现金流量分配   是否数量凭证  是否外币凭证  凭证业务类型 会计期间
                //会计年度   单据状态   制单日期   制单人   审核日期   制单人ID   是否已经期间结转  是否结转来的单据  是否手工修改单据编码  CreatedTime  顺序号  现金流量分配状态
                //分布时间戳  更新时间 iddocType  idperiod  作废状态  标错状态  是否出纳凭证   凭证排序号  打印次数

                string strsql = @"insert into [{0}].{1}.dbo.GL_Doc(id,code,name,accuorigamountdr,accuorigamountcr,accuamountdr,accuamountcr,issupervisoraudit
                    ,iscashieraudit,cashiername,ispost,bookkeepername,postdate,iserror,isinvalidate,istaxed,iscashflowed,isquantitydoc,isforeigncurrencydoc,docbusinesstype,accountingperiod
                    ,accountingyear,voucherstate,madedate,maker,auditeddate,makerid,iscarriedforwardout,iscarriedforwardin,ismodifiedcode,createdtime,sequencenumber,cashflowedstate
                    ,updated,updatedBy,iddoctype,idperiod,invalidateState,makeErrorState,isCashierDoc,DocOrderNum,SourceContent,AttachedVoucherNum,VoucherDate,IsCashFlowed) 
                        values(
                            @id,@code,@name,@accuorigamountdr,@accuorigamountcr,@accuamountdr,@accuamountcr,@issupervisoraudit
                    ,@iscashieraudit,@cashiername,@ispost,@bookkeepername,@postdate,@iserror,@isinvalidate,@istaxed,@iscashflowed,@isquantitydoc,@isforeigncurrencydoc,@docbusinesstype,@accountingperiod
                    ,@accountingyear,@voucherstate,@madedate,@maker,@auditeddate,@makerid,@iscarriedforwardout,@iscarriedforwardin,@ismodifiedcode,@createdtime,@sequencenumber,@cashflowedstate
                    ,@updated,@updatedBy,@iddoctype,@idperiod,@invalidateState,@makeErrorState,@isCashierDoc,@DocOrderNum,@SourceContent,@AttachedVoucherNum,@VoucherDate,@IsCashFlowed
                        )
                    ";
                strsql = string.Format(strsql, strlinkdbname, strZhangTaodb);
                string code = ino_id;

                string voucherstate = "D6C5E975-900D-40D3-AEF0-5D189D230FB1";//单据状态

                string auditeddate = "";//审核日期
                string iscarriedforwardout = "0";//是否已经期间结转
                string iscarriedforwardin = "0";//是否结转来的单据
                string ismodifiedcode = "0";//是否手工修改单据编码
                string createdtime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");//CreatedTime
                string cashflowedstate = "F2E9D929-8682-4475-86DA-D9A329940612";//现金流量分配状态
                string updated = "";//更新时间
                string updatedBy = "";//更新者

                if (string.IsNullOrEmpty(idperiod))
                {
                    showMessage("对不起，未检测到对应的会计月度", false, ""); return;
                }
                string invalidateState = "056D8D81-2D49-4AD9-B02F-EECC700F806D";//作废状态
                string makeErrorState = "085B0ED1-8109-4C6E-8A9E-912C866B3151";//标错状态
                string isCashierDoc = "1";//是否出纳凭证
                string DocOrderNum = "00000000000000000000000000000" + ino_id;//凭证排序号
                DocOrderNum = iyear + iperiod2 + DocOrderNum.Substring(DocOrderNum.Length - 23, 23);

                string PrintCount = "0";//打印次数

                SqlParameter[] arrsp1 = { 
                                            
                                              new SqlParameter("@id",strNewid),new SqlParameter("@code",SqlNull(code)),new SqlParameter("@name",""),new SqlParameter("@accuorigamountdr",SqlNull(dbsummd)),new SqlParameter("@accuorigamountcr",SqlNull(dbsummc)),
                                              new SqlParameter("@accuamountdr",SqlNull(dbsummd)),new SqlParameter("@accuamountcr",SqlNull(dbsummc)),new SqlParameter("@issupervisoraudit","0"),


                                              new SqlParameter("@iscashieraudit","0"),new SqlParameter("@cashiername",""),new SqlParameter("@ispost","0"),
                                              new SqlParameter("@bookkeepername",""),new SqlParameter("@postdate",SqlNull("")),new SqlParameter("@iserror","0"),
                                              new SqlParameter("@isinvalidate","0"),new SqlParameter("@istaxed","0"),new SqlParameter("@iscashflowed",SqlNull("0")),
                                              new SqlParameter("@isquantitydoc","0"),new SqlParameter("@isforeigncurrencydoc","0"),new SqlParameter("@docbusinesstype",new Guid(docbusinesstype)),
                                              new SqlParameter("@accountingperiod",SqlNull(iperiod))
                                              
                                              ,new SqlParameter("@accountingyear",iyear),new SqlParameter("@voucherstate",new Guid(voucherstate)),new SqlParameter("@madedate",dbill_date+" 00:00:00"),new SqlParameter("@maker",SqlNull(Maker))
                                              ,new SqlParameter("@auditeddate",SqlNull(auditeddate)),new SqlParameter("@makerid",SqlNull(new Guid(MakerId))),new SqlParameter("@iscarriedforwardout",SqlNull(iscarriedforwardout)),new SqlParameter("@iscarriedforwardin",SqlNull(iscarriedforwardin))
                                              ,new SqlParameter("@ismodifiedcode",SqlNull(ismodifiedcode)),new SqlParameter("@createdtime",SqlNull(createdtime)),new SqlParameter("@sequencenumber","0"),new SqlParameter("@cashflowedstate",SqlNull(new Guid(cashflowedstate)))

                                              ,new SqlParameter("@updated",SqlNull(updated)),new SqlParameter("@updatedBy",SqlNull(updatedBy))
                                              ,new SqlParameter("@iddoctype",SqlNull(new Guid(iddoctype))),new SqlParameter("@idperiod",SqlNull(new Guid(idperiod))),new SqlParameter("@invalidateState",SqlNull(invalidateState))  
                                              ,new SqlParameter("@makeErrorState",SqlNull(makeErrorState)),new SqlParameter("@isCashierDoc",SqlNull(isCashierDoc)),new SqlParameter("@DocOrderNum",SqlNull(DocOrderNum)),new SqlParameter("@SourceContent","ysgl"),new SqlParameter("@AttachedVoucherNum",idoc)
                                         ,new SqlParameter("@VoucherDate",dbill_date+" 00:00:00"),new SqlParameter("@IsCashFlowed","0")
                                           };
                lstpingzheng.Add(new pingzheng() { sql = strsql, arrsp = arrsp1 });
                #endregion
                //验证借贷平衡 AttachedVoucherNum
                if (Math.Round(dbsummc, 2) != Math.Round(dbsummd, 2))
                {
                    showMessage("保存失败，原因：借贷不平衡，请检查是否有核算科室没有设置对应关系。", false, ""); return;
                }
                bool executeflg = false;
                if (lstpingzheng.Count > 0)
                {
                    foreach (pingzheng pingzhengeve in lstpingzheng)
                    {

                        if (server.ExecuteNonQuery(pingzhengeve.sql, pingzhengeve.arrsp) <= 0)
                        {
                            executeflg = false;
                            break;
                        }
                        else
                        {
                            executeflg = true;
                        }
                    }
                }

                lbeMsg.Text = "操作结果：处理成功，凭证号为：" + strpingzhenghao;
                return;
                //如果凭证表插入成功，修改预算系统表
                if (executeflg)
                {
                    //修改bill_ybbxmxb
                    string[] arrCode = strCode.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries);
                    string strInBillCode = "";
                    for (int i = 0; i < arrCode.Length; i++)
                    {
                        strInBillCode += string.Format("'{0}',", arrCode[i]);
                    }
                    strInBillCode = strInBillCode.Substring(0, strInBillCode.Length - 1);
                    if (strpingzhenghao.Equals(""))
                    {
                        showMessage("凭证号获取失败！", false, ""); return;
                    }
                    try
                    {
                        string zhangtao = this.ddlZhangTao.SelectedValue;
                        zhangtao = zhangtao.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        if (new Bll.newysgl.bill_ysmxbBll().SetPingZhengByBillName(strInBillCode, strpingzhenghao, strZhangTaodb, strBillDate) > 0)
                        {
                            lbeMsg.Text = "操作结果：处理成功，凭证号为：" + strpingzhenghao;
                        }
                        else
                        {
                            lbeMsg.Text = "操作结果：凭证保存成功（凭证号：" + strpingzhenghao + "），但报销单处理失败，请联系管理员解决！";
                        }
                    }
                    catch (Exception ex)
                    {
                        lbeMsg.Text = "保存失败，原因：" + ex.Message;
                    }
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                this.lbeMsg.Text = "错误信息：" + ex.Message;
                this.btnSave.Enabled = false;
                return;
            }
        }
    }
    class pingzheng
    {
        public string sql { get; set; }
        public SqlParameter[] arrsp { get; set; }
    }
    private object SqlNull(object obj)
    {
        if (obj == null || obj.ToString().Equals(""))
            return DBNull.Value;
        return obj;
    }


    /// <summary>
    /// 重新加载gridview 一般在处理重新选择了财务科目之后
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void reLoad_OnClick(object sender, EventArgs e)
    {
        reload();
    }

    private void reload()
    {
        System.Collections.Generic.List<itemModel> lstItemModel = new System.Collections.Generic.List<itemModel>();
        int iCount = this.GridView.Rows.Count;
        for (int i = 0; i < iCount; i++)
        {
            itemModel itemmodel = new itemModel();
            itemmodel.billDept = this.GridView.Rows[i].Cells[5].Text.Trim();
            itemmodel.dfje = ((TextBox)this.GridView.Rows[i].Cells[8].FindControl("dfje")).Text.Trim();
            itemmodel.jfje = ((TextBox)this.GridView.Rows[i].Cells[7].FindControl("jfje")).Text.Trim();
            TextBox txtMxkm = this.GridView.Rows[i].Cells[3].FindControl("txtMingXiKemu") as TextBox;
            if (txtMxkm != null)
            {
                string strmxkm = txtMxkm.Text.Trim();
                strmxkm = strmxkm.Equals("&nbsp;") ? "" : strmxkm;
                itemmodel.fykmName = strmxkm;
            }
            else
            {
                itemmodel.fykmName = "";
            }
            string strcwkm = itemmodel.fykmName;
            if (!string.IsNullOrEmpty(strcwkm))
            {
                string strsql = "select FuZhuHeSuan from bill_cwkm where cwkmCode='" + strcwkm.Substring(1, strcwkm.IndexOf("]") - 1) + "'";
                object objRel = server.ExecuteScalar(strsql);
                itemmodel.fuzhuhesuan = objRel == null ? "" : objRel.ToString();
            }
            else
            {
                itemmodel.fuzhuhesuan = "";
            }
            itemmodel.PingZhengType = this.GridView.Rows[i].Cells[0].Text.Trim();
            itemmodel.bxzy = this.GridView.Rows[i].Cells[1].Text.Trim().Equals("&nbsp;") ? "" : this.GridView.Rows[i].Cells[1].Text.Trim();
            itemmodel.billUser = this.GridView.Rows[i].Cells[10].Text.Trim();
            TextBox txtOsDept = this.GridView.Rows[i].Cells[6].FindControl("txtForDept") as TextBox;
            if (txtOsDept != null)
            {
                itemmodel.OsDept = txtOsDept.Text.Trim();
            }
            ////现金流量项目
            //DropDownList ddlxianjinliuliang = this.GridView.Rows[i].Cells[4].FindControl("ddlxianjinliu") as DropDownList;
            //if (ddlxianjinliuliang != null)
            //{
            //    itemmodel.xianjinliuxiangmucode = ddlxianjinliuliang.SelectedValue;
            //}
            ////大类
            //DropDownList ddlbigclass = this.GridView.Rows[i].Cells[4].FindControl("ddlbigclass") as DropDownList;
            //if (ddlbigclass != null)
            //{
            //    itemmodel.bigclasscode = ddlbigclass.SelectedValue;
            //}
            ////小类
            //DropDownList ddlsmallclass = this.GridView.Rows[i].Cells[4].FindControl("ddlsmallclass") as DropDownList;
            //if (ddlsmallclass != null)
            //{
            //    itemmodel.smallclasscode = ddlsmallclass.SelectedValue;
            //}
            ////现金流项目
            //string bcash = this.GridView.Rows[i].Cells[13].Text.Trim();
            //itemmodel.bcash = bcash.Replace("&nbsp;", "");


            string bdept = this.GridView.Rows[i].Cells[14].Text.Trim();
            itemmodel.bdept = bdept.Replace("&nbsp;", "");

            string bperson = this.GridView.Rows[i].Cells[15].Text.Trim();
            itemmodel.bperson = bperson.Replace("&nbsp;", "");

            string bcus = this.GridView.Rows[i].Cells[16].Text.Trim();
            itemmodel.bcus = bcus.Replace("&nbsp;", "");

            string bsup = this.GridView.Rows[i].Cells[17].Text.Trim();
            itemmodel.bsup = bsup.Replace("&nbsp;", "");

            string bitem = this.GridView.Rows[i].Cells[18].Text.Trim();
            itemmodel.bitem = bitem.Replace("&nbsp;", "");

            string cass_item = this.GridView.Rows[i].Cells[19].Text.Trim();
            itemmodel.cass_item = cass_item.Replace("&nbsp;", "");
            lstItemModel.Add(itemmodel);
        }
        this.GridView.DataSource = lstItemModel;
        this.GridView.DataBind();
    }

    double dbjfjeZong = 0;
    double dbdfjeZong = 0;
    bool isHasFuZhuHasNoCount = false;
    protected void GridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e == null)
        {
            return;
        }
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {
            //操作借款贷款
            string strPingZhengType = e.Row.Cells[0].Text.Trim();
            if (strPingZhengType.Equals("df"))
            {
                //e.Row.Cells[7].Text = "0";
                TextBox jfje = e.Row.Cells[7].FindControl("jfje") as TextBox;
                if (jfje != null)
                {
                    jfje.Text = "0";
                }
            }
            else
            {
                TextBox dfje = e.Row.Cells[8].FindControl("dfje") as TextBox;
                if (dfje != null)
                {
                    dfje.Text = "0";
                }
                //e.Row.Cells[8].Text = "0";
            }

            //显示摘要到textbox
            string strZY = e.Row.Cells[1].Text.Trim();
            System.Web.UI.WebControls.TextBox textbox = e.Row.Cells[2].FindControl("txtZhaiYao") as System.Web.UI.WebControls.TextBox;
            if (textbox != null)
            {
                textbox.Text = strZY;
            }
            //合计行
            TextBox txtjfje = e.Row.Cells[7].FindControl("jfje") as TextBox;
            string strJie = txtjfje == null ? "" : txtjfje.Text.ToString();
            double dbJie = 0;
            if (double.TryParse(strJie, out dbJie))
            {
                dbjfjeZong += dbJie;
            }

            TextBox txtdfje = e.Row.Cells[8].FindControl("dfje") as TextBox;
            string strDai = txtdfje == null ? "" : txtdfje.Text.ToString();
            double dbDai = 0;
            if (double.TryParse(strDai, out dbDai))
            {
                dbdfjeZong += dbDai;
            }

            string strisbodept = e.Row.Cells[14].Text.Trim();
            strisbodept = strisbodept.Replace("&nbsp;", "");
            bool hasdeptfuzhu = strisbodept.Equals("True") ? true : false;
            //默认核算部门
            #region 核算部门
            string strosdept = e.Row.Cells[11].Text.Trim();
            TextBox txtDept2 = e.Row.Cells[6].FindControl("txtForDept") as TextBox;
            string strNowDeptName = e.Row.Cells[5].Text.Trim();
            strNowDeptName = strNowDeptName.Equals("&nbsp;") ? "" : strNowDeptName;
            e.Row.Cells[5].Text = strNowDeptName;
            string stru8id = "";
            if (!strosdept.Equals("") && !strosdept.Equals("&nbsp;") && hasdeptfuzhu)
            {
                txtDept2.Text = strosdept;
            }
            else
            { //如果没有给部门设置foru8id那默认用预算系统中的编号
                if (!strNowDeptName.Equals(""))
                {
                    strNowDeptName = strNowDeptName.Substring(1, strNowDeptName.IndexOf(']') - 1);
                    if (txtDept2 != null && hasdeptfuzhu)
                    {
                        txtDept2.Text = stru8id;
                        if (stru8id.Equals(""))
                        {
                            txtDept2.Text = strNowDeptName;
                        }
                    }
                }
            }

            if (IsPostBack)
            {
                string strdeptname = e.Row.Cells[11].Text.Trim();
                strdeptname = strdeptname.Equals("&nbsp;") ? "" : strdeptname;
                if (txtDept2 != null)
                {
                    txtDept2.Text = strdeptname;
                }
            }
            #endregion

            #region 显示明细科目(费用科目对应贷方财务科目)
            string strMxKm = e.Row.Cells[12].Text.Trim();
            if (strMxKm.Replace("&nbsp;", "").Equals(""))
            {
                showMessage("第" + (e.Row.RowIndex) + "行的部门" + strNowDeptName + "未设置该预算科目的对应会计科目，请先到‘部门预算科目’菜单下设置。", false, "");
                btnSave.Enabled = false;
                return;
            }
            strMxKm = strMxKm.Equals("&nbsp;") ? "" : strMxKm;
            TextBox txtMxkm = e.Row.Cells[3].FindControl("txtMingXiKemu") as TextBox;
            if (txtMxkm != null)
            {
                txtMxkm.Text = strMxKm;
                //如果是税额 默认明细科目
                if (strPingZhengType.Equals("se"))
                {
                    txtMxkm.Text = server.GetCellValue("select '['+cwkmcode+']'+cwkmmc from bill_cwkm where cwkmcode='22210101'");
                }
            }
            #endregion

            #region 绑定现金流量项目  大类 小类
            ////绑定现金流量项目  大类 小类
            //DropDownList ddlxianjinliuliang = e.Row.Cells[4].FindControl("ddlxianjinliu") as DropDownList;
            //if (ddlxianjinliuliang != null)
            //{
            //    ddlxianjinliuliang.DataSource = this.getAllXianJinLiuXiang();
            //    ddlxianjinliuliang.DataTextField = "citemname";
            //    ddlxianjinliuliang.DataValueField = "citemcode";
            //    ddlxianjinliuliang.DataBind();
            //    ddlxianjinliuliang.Items.Insert(0, new ListItem("-现金流量项目-", ""));
            //    //为现金流量项目设置默认值
            //    string strxianjinliuliang = e.Row.Cells[22].Text.Trim();
            //    strxianjinliuliang = strxianjinliuliang.Replace("&nbsp;", "");
            //    if (!strxianjinliuliang.Equals(""))
            //    {
            //        ddlxianjinliuliang.SelectedValue = strxianjinliuliang;
            //    }
            //}

            //获取大类
            //DropDownList ddlbigclass = e.Row.Cells[4].FindControl("ddlbigclass") as DropDownList;
            //if (ddlbigclass != null)
            //{
            //    ddlbigclass.DataSource = this.getBigClass();
            //    ddlbigclass.DataTextField = "citem_name";
            //    ddlbigclass.DataValueField = "citem_class";
            //    ddlbigclass.DataBind();
            //    ddlbigclass.Items.Insert(0, new ListItem("-大类-", ""));
            //    //为大类设置默认值
            //    string strcass_item = e.Row.Cells[19].Text.Trim(); //原来财务科目对应的大类
            //    strcass_item = strcass_item.Replace("&nbsp;", "");
            //    if (!strcass_item.Equals(""))//
            //    {
            //        ddlbigclass.SelectedValue = strcass_item;
            //    }
            //    string strbigclass = e.Row.Cells[20].Text.Trim();//可能后来改的
            //    strbigclass = strbigclass.Replace("&nbsp;", "");
            //    if (!strbigclass.Equals(""))
            //    {
            //        ddlbigclass.SelectedValue = strbigclass;
            //    }
            //    //根据大类绑定小类
            //    DropDownList ddlsmallclass = e.Row.Cells[4].FindControl("ddlsmallclass") as DropDownList;
            //    if (ddlsmallclass != null)
            //    {
            //        ddlsmallclass.Items.Clear();
            //        string strbigclasscode = ddlbigclass.SelectedValue;
            //        if (!string.IsNullOrEmpty(strbigclasscode))
            //        {
            //            ddlsmallclass.DataSource = this.getSmallClass(strbigclasscode);
            //            ddlsmallclass.DataTextField = "citemname";
            //            ddlsmallclass.DataValueField = "citemcode";
            //            ddlsmallclass.DataBind();
            //            ddlsmallclass.Items.Insert(0, new ListItem("-小类-", ""));
            //            //为小类设置默认值
            //            string strsmallclass = e.Row.Cells[21].Text.Trim();
            //            strsmallclass = strsmallclass.Replace("&nbsp;", "");
            //            if (!strsmallclass.Equals(""))
            //            {
            //                ddlsmallclass.SelectedValue = strsmallclass;
            //            }
            //        }
            //    }
            //}
            #endregion

            //根据会计科目  设置隐藏域是否有现金流项目的控制
            //TextBox txtcwkm = e.Row.Cells[3].FindControl("txtMingXiKemu") as TextBox;
            //if (txtcwkm != null)
            //{
            //    string strcwkm = txtcwkm.Text.Trim().Replace("&nbsp;", "");
            //    try
            //    {
            //        strcwkm = strcwkm.Substring(1, strcwkm.IndexOf("]") - 1);
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    if (!strcwkm.Equals(""))
            //    {
            //        //根据财务科目读取是否有现金流项目的控制
            //        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
            //        if (strlinkdbname.Equals(""))
            //        {
            //            return;
            //        }
            //        string strZhangTaodb = this.ddlZhangTao.SelectedValue;
            //        if (string.IsNullOrEmpty(strZhangTaodb))
            //        {
            //            return;
            //        }
            //        strZhangTaodb = strZhangTaodb.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries)[1];
            //        string sql = "select bcash from [{0}].{1}.dbo.code where ccode='{2}'";
            //        sql = string.Format(sql, strlinkdbname, strZhangTaodb, strcwkm);
            //        string bcash = server.GetCellValue(sql);
            //        if (!string.IsNullOrEmpty(bcash))
            //        {
            //            e.Row.Cells[13].Text = bcash;
            //        }
            //    }
            //}
        }
        else if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "合计：";
            e.Row.Cells[2].Style.Add("text-align", "right");
            e.Row.Cells[7].Text = dbjfjeZong.ToString("N");
            e.Row.Cells[7].Style.Add("text-align", "right");
            e.Row.Cells[8].Text = dbdfjeZong.ToString("N");
            e.Row.Cells[8].Style.Add("text-align", "right");
            if (isHasFuZhuHasNoCount)
            {
                showMessage("系统已自动添加记录中未维护的辅助项目到辅助项目，请先为其添加下级项目作为项目说明，否则单据将保存失败！", true, "");
            }
        }
    }

    protected void OnddlZhangTao_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindPingZhengType();
        bindNd();
    }

    #region 私有方法
    private void bindNd()
    {
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
        if (strlinkdbname.Equals(""))
        {
            showMessage("没有找到用友系统数据库链接，无法绑定凭证类型", false, "");
            return;
        }

        string strZhangTaodb = this.ddlZhangTao.SelectedValue;
        if (string.IsNullOrEmpty(strZhangTaodb))
        {
            return;
        }
        strZhangTaodb = strZhangTaodb.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries)[1];
        string sql = "select distinct AccountingYear from [{0}].{1}.dbo.AA_Account order by accountingyear desc";
        sql = string.Format(sql, strlinkdbname, strZhangTaodb);
        this.ddlNd.DataSource = server.GetDataTable(sql, null);
        this.ddlNd.DataTextField = "AccountingYear";
        this.ddlNd.DataValueField = "AccountingYear";
        this.ddlNd.DataBind();
        BindDataGrid();
    }
    /// <summary>
    /// 页面初始化
    /// </summary>
    private void BindDataGrid()
    {
        if (strCode.Equals(""))
        {
            showMessage("系统参数丢失，请回到列表页刷新后重试！", true, "");
            return;
        }
        //附加单据数
        this.txtBillCount.Text = "0";

        string strSql = "";
        string strflg = new Bll.ConfigBLL().GetValueByKey("pingzhengbygkorsy");
        bool boguikoufenjie = new Bll.ConfigBLL().GetValueByKey("UseGKFJ").Equals("1");
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
        if (strlinkdbname.Equals(""))
        {
            showMessage("没有找到用友系统数据库链接，无法绑定凭证类型", false, "");
            return;
        }

        string strZhangTaodb = this.ddlZhangTao.SelectedValue;
        if (string.IsNullOrEmpty(strZhangTaodb))
        {
            return;
        }
        strZhangTaodb = strZhangTaodb.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries)[1];


        //注：上半个是isnull(IsAuxAccDepartment,'1') 下半个isnull(IsAuxAccDepartment,'0')是为了没有设置借贷方的不会重复显示
        strSql = @"select * from (
                    select '' as bigclasscode,'' as smallclasscode,'' as xianjinliuxiangmucode, bxzy,sum(jfje) as jfje,sum(dfje) as dfje,billdept,billuser,fykmname,fykmcode,fuzhuhesuan,pingzhengtype,osdept,IsCash,IsAuxAccPerson,IsAuxAccCustomer,IsAuxAccSupplier,IsAuxAccDepartment,IsAuxAccProject,idcurrency from (
	                select fymx.*,IsCash,IsAuxAccPerson,IsAuxAccCustomer,IsAuxAccSupplier,IsAuxAccDepartment,IsAuxAccProject,id as idcurrency
		            from (
					select
					(select '['+deptCode+']'+deptName from bill_departments where deptCode=bill_ybbxmxb_fykm_dept.deptcode) as billDept,
                    (select billUser from bill_main where billCode=bill_ybbxmxb_fykm.billCode) as billUser,
                    fykm,bill_ybbxmxb_fykm_dept.je as jfje,bill_ybbxmxb_fykm_dept.je as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm  and  deptCode =bill_ybbxmxb_fykm_dept.deptcode and djlx='{4}')) as fykmName,
					(select cwkmCode from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm  and deptCode =bill_ybbxmxb_fykm_dept.deptcode and djlx='{4}')) as fykmCode,
                    (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =bill_ybbxmxb_fykm_dept.deptcode)) as fuzhuhesuan,
                    (select top 1 bxzy from bill_ybbxmxb where billCode=bill_ybbxmxb_fykm.billCode) as bxzy,'jf' as PingZhengType,(select isnull(forU8id,'') from bill_departments where deptCode=bill_ybbxmxb_fykm_dept.deptcode) as OsDept  from bill_ybbxmxb_fykm,bill_ybbxmxb_fykm_dept where bill_ybbxmxb_fykm.mxGuid=bill_ybbxmxb_fykm_dept.kmmxGuid and billCode in (select billcode from bill_main where billname in (select billname from bill_main where billcode in ({0})))
                    union all
                    select billdept,billuser,fykm,sum(jfje) as jfje,sum(dfje) as dfje,fykmname,fykmcode,fuzhuhesuan,bxzy,pingzhengtype,osdept from (
                        select (select '['+deptCode+']'+deptName from bill_departments where deptCode=(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)) as billDept,
                        (select billUser from bill_main where billCode=bill_ybbxmxb_fykm.billCode) as billUser,
                        '' as fykm,je as jfje,je as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode) and djlx='{4}')) as fykmName,
					    (select cwkmCode from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)  and djlx='{4}')) as fykmCode,
                        (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fuzhuhesuan,
                        (select top 1 bxzy from bill_ybbxmxb where billCode=bill_ybbxmxb_fykm.billCode) as bxzy,'df' as PingZhengType,(select isnull(forU8id,'') from bill_departments where deptCode=(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)) as OsDept  
                        from bill_ybbxmxb_fykm where billCode in (select billcode from bill_main where billname in (select billname from bill_main where billcode in ({0})))
                    )lsb group by billdept,billuser,fykm,fykmname,fykmcode,fuzhuhesuan,bxzy,pingzhengtype,osdept
		   ) fymx left join 
			(select * from [{1}].{2}.dbo.AA_Account where IsEndNode='1' and accountingyear='{3}') code 
			on fymx.fykmcode=code.code where isnull(IsAuxAccDepartment,'1')='1'
		) tend 
		group by billdept,billuser,fykmname,fykmcode,fuzhuhesuan,pingzhengtype,osdept,IsCash,IsAuxAccPerson,IsAuxAccCustomer,IsAuxAccSupplier,IsAuxAccDepartment,IsAuxAccProject,bxzy,idcurrency
        union all
        select  '' as bigclasscode,'' as smallclasscode,'' as xianjinliuxiangmucode,bxzy,sum(jfje) as jfje,sum(dfje) as dfje,'',billuser,fykmname,fykmcode,fuzhuhesuan,pingzhengtype,'',IsCash,IsAuxAccPerson,IsAuxAccCustomer,IsAuxAccSupplier,IsAuxAccDepartment,IsAuxAccProject,idcurrency from (
	        select fymx.*,IsCash,IsAuxAccPerson,IsAuxAccCustomer,IsAuxAccSupplier,IsAuxAccDepartment,IsAuxAccProject,id as idcurrency
		        from (
					select
					(select '['+deptCode+']'+deptName from bill_departments where deptCode=bill_ybbxmxb_fykm_dept.deptcode) as billDept,
                    (select billUser from bill_main where billCode=bill_ybbxmxb_fykm.billCode) as billUser,
                    fykm,bill_ybbxmxb_fykm_dept.je as jfje,bill_ybbxmxb_fykm_dept.je as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =bill_ybbxmxb_fykm_dept.deptcode and djlx='{4}')) as fykmName,
					(select cwkmCode from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =bill_ybbxmxb_fykm_dept.deptcode  and djlx='{4}')) as fykmCode,
                    (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm  and deptCode =bill_ybbxmxb_fykm_dept.deptcode)) as fuzhuhesuan,
                    (select top 1 bxzy from bill_ybbxmxb where billCode=bill_ybbxmxb_fykm.billCode) as bxzy,'jf' as PingZhengType,(select isnull(forU8id,'') from bill_departments where deptCode=bill_ybbxmxb_fykm_dept.deptcode) as OsDept  from bill_ybbxmxb_fykm,bill_ybbxmxb_fykm_dept where bill_ybbxmxb_fykm.mxGuid=bill_ybbxmxb_fykm_dept.kmmxGuid and billCode in (select billcode from bill_main where billname in (select billname from bill_main where billcode in ({0})))
                    union all
                    select billdept,billuser,fykm,sum(jfje) as jfje,sum(dfje) as dfje,fykmname,fykmcode,fuzhuhesuan,bxzy,pingzhengtype,osdept from (
                        select (select '['+deptCode+']'+deptName from bill_departments where deptCode=(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)) as billDept,
                        (select billUser from bill_main where billCode=bill_ybbxmxb_fykm.billCode) as billUser,
                       '' as fykm,je as jfje,je as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode) and djlx='{4}')) as fykmName,
					    (select cwkmCode from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)  and djlx='{4}' )) as fykmCode,
                        (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm  and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fuzhuhesuan,
                        (select top 1 bxzy from bill_ybbxmxb where billCode=bill_ybbxmxb_fykm.billCode) as bxzy,'df' as PingZhengType,(select isnull(forU8id,'') from bill_departments where deptCode=(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)) as OsDept  
                        from bill_ybbxmxb_fykm where billCode in (select billcode from bill_main where billname in (select billname from bill_main where billcode in ({0})))
                    )lsb group by billdept,billuser,fykm,fykmname,fykmcode,fuzhuhesuan,bxzy,pingzhengtype,osdept
		   ) fymx left join 
			(select * from [{1}].{2}.dbo.AA_Account where IsEndNode='1' and AccountingYear='{3}') code 
			on fymx.fykmcode=code.code where isnull(IsAuxAccDepartment,'1')='0' 
		) tend
		group by billuser,fykmname,fykmcode,fuzhuhesuan,pingzhengtype,IsCash,IsAuxAccPerson,IsAuxAccCustomer,IsAuxAccSupplier,IsAuxAccDepartment,IsAuxAccProject,bxzy,idcurrency
)a where jfje!=0 and dfje!=0  order by pingzhengtype desc
";//最外面一层group by 把fykm去掉了 这个字段应该没用 
        strSql = string.Format(strSql, strInBillCode, strlinkdbname, strZhangTaodb, this.ddlNd.SelectedValue, djlx);
        //}
        DataTable dtRel = server.GetDataTable(strSql, null);
        this.GridView.DataSource = dtRel;
        this.GridView.DataBind();
        this.txtBillCount.Text = strSql;

    }
    /// <summary>
    /// 绑定常用摘要
    /// </summary>
    private void bindZhaiYao()
    {

        this.ddlZhaiYao.DataSource = server.GetDataTable("select dicName from bill_dataDic where dicType='07'", null);
        this.ddlZhaiYao.DataTextField = "dicName";
        this.ddlZhaiYao.DataValueField = "dicName";
        this.ddlZhaiYao.DataBind();
        this.ddlZhaiYao.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-常用摘要-", ""));
    }
    /// <summary>
    /// 绑定帐套
    /// </summary>
    private void bindZhangTao()
    {
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
        string strselectsql = @"select dsname as db_data,cacc_mastername+cacc_name+cast(iyear as varchar(4)) as cAcc_Name,iyear,
                        cast(cAcc_Num as varchar(50))+'|*|'+dsname as tval,
                        * from [{0}].UFTSystem.dbo.EAP_Account";
        strselectsql = string.Format(strselectsql, strlinkdbname);
        this.ddlZhangTao.DataSource = server.GetDataTable(strselectsql, null);
        this.ddlZhangTao.DataTextField = "companyname";
        this.ddlZhangTao.DataValueField = "tval";
        this.ddlZhangTao.DataBind();
    }

    /// <summary>
    /// 绑定凭证类型
    /// </summary>
    private void bindPingZhengType()
    {
        this.ddlPingZhengType.Items.Clear();
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
        if (strlinkdbname.Equals(""))
        {
            showMessage("没有找到用友系统数据库链接，无法绑定凭证类型", false, "");
            return;
        }

        string strZhangTaodb = this.ddlZhangTao.SelectedValue;
        if (string.IsNullOrEmpty(strZhangTaodb))
        {
            return;
        }
        strZhangTaodb = strZhangTaodb.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries)[1];
        if (!string.IsNullOrEmpty(strZhangTaodb))
        {
            string strsql = @"select Name,Code+'|'+cast(SequenceNumber as varchar(10))+'|'+cast(id as varchar(50)) as cval from [{0}].{1}.dbo.AA_DocType";
            strsql = string.Format(strsql, strlinkdbname, strZhangTaodb);
            DataTable dtrel = server.GetDataTable(strsql, null);
            ddlPingZhengType.DataSource = dtrel;
            this.ddlPingZhengType.DataTextField = "Name";
            this.ddlPingZhengType.DataValueField = "cval";
            this.ddlPingZhengType.DataBind();
        }
    }
    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    /// <param name="isExit">提示后是否退出</param>
    /// <param name="strReturnVal">返回值</param>
    private void showMessage(string strMsg, bool isExit, string strReturnVal)
    {
        string strScript = "alert('" + strMsg + "');";
        if (!strReturnVal.Equals(""))
        {
            strScript += "window.returnValue=\"" + strReturnVal + "\";";
        }
        if (isExit)
        {
            strScript += "self.close();";
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
    }

    #endregion

    /// <summary>
    /// 常用摘要切换
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlZhaiYao_SelectIndexChanged(object sender, EventArgs e)
    {
        this.txtZhaiYao.Text = this.ddlZhaiYao.SelectedValue;
    }

    /// <summary>
    /// 统一到摘要
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnZhaiYaoToItem_Click(object sender, EventArgs e)
    {
        string strZhaiYao = this.txtZhaiYao.Text.Trim();
        if (strZhaiYao.Equals(""))
        {
            showMessage("摘要不能为空！", false, "");
            return;
        }
        //是否已经存在
        string strSql = "select count(*) from bill_dataDic where dicName=@ZhaiYao";
        int iRel = server.ExecuteNonQuery(strSql, new System.Data.SqlClient.SqlParameter[] { new SqlParameter("ZhaiYao", strZhaiYao) });
        if (iRel <= 0 && this.cbAddToUsually.Checked)
        {
            string strAddSql = "insert into bill_dataDic(dicType,dicCode,dicName) values('07','',@val)";
            server.ExecuteNonQuery(strAddSql, new System.Data.SqlClient.SqlParameter[] { new SqlParameter("val", strZhaiYao) });
        }
        System.Collections.Generic.List<itemModel> lstItemModel = new System.Collections.Generic.List<itemModel>();
        int iCount = this.GridView.Rows.Count;
        for (int i = 0; i < iCount; i++)
        {
            itemModel itemmodel = new itemModel();
            itemmodel.billDept = this.GridView.Rows[i].Cells[5].Text.Trim();
            itemmodel.dfje = (this.GridView.Rows[i].Cells[8].FindControl("jfje") as TextBox).Text.Trim();
            itemmodel.jfje = (this.GridView.Rows[i].Cells[7].FindControl("dfje") as TextBox).Text.Trim();
            TextBox txtMxkm = this.GridView.Rows[i].Cells[3].FindControl("txtMingXiKemu") as TextBox;
            if (txtMxkm != null)
            {
                string strmxkm = txtMxkm.Text.Trim();
                strmxkm = strmxkm.Equals("&nbsp;") ? "" : strmxkm;
                itemmodel.fykmName = strmxkm;
            }
            else
            {
                itemmodel.fykmName = "";
            }
            itemmodel.fuzhuhesuan = this.GridView.Rows[i].Cells[9].Text.Trim().Equals("&nbsp;") ? "" : this.GridView.Rows[i].Cells[9].Text.Trim();
            itemmodel.PingZhengType = this.GridView.Rows[i].Cells[0].Text.Trim();
            itemmodel.bxzy = strZhaiYao;
            itemmodel.billUser = this.GridView.Rows[i].Cells[10].Text.Trim();
            TextBox txtOsDept = this.GridView.Rows[i].Cells[6].FindControl("txtForDept") as TextBox;
            if (txtOsDept != null)
            {
                itemmodel.OsDept = txtOsDept.Text.Trim();
            }
            itemmodel.idcurrency = this.GridView.Rows[i].Cells[23].Text.Trim();//会计科目主键
            lstItemModel.Add(itemmodel);
        }
        this.GridView.DataSource = lstItemModel;
        this.GridView.DataBind();
        bindZhaiYao();
    }

    private class itemModel
    {
        private string _pingZhengType = "";

        public string PingZhengType
        {
            get { return _pingZhengType; }
            set { _pingZhengType = value; }
        }
        private string _zhaiYao = "";

        public string bxzy
        {
            get { return _zhaiYao; }
            set { _zhaiYao = value; }
        }
        private string _feiYongKeMu = "";

        public string fykmName
        {
            get { return _feiYongKeMu; }
            set { _feiYongKeMu = value; }
        }
        private string _fuZhuHeSuan = "";

        public string fuzhuhesuan
        {
            get { return _fuZhuHeSuan; }
            set { _fuZhuHeSuan = value; }
        }
        private string _buMenHeSuan = "";

        public string billDept
        {
            get { return _buMenHeSuan; }
            set { _buMenHeSuan = value; }
        }

        private string _osDept = "";
        /// <summary>
        /// 对应系统的dept名称
        /// </summary>
        public string OsDept
        {
            get { return _osDept; }
            set { _osDept = value; }
        }

        private string _jfje = "";

        public string jfje
        {
            get { return _jfje; }
            set { _jfje = value; }
        }
        private string _dfje = "";

        public string dfje
        {
            get { return _dfje; }
            set { _dfje = value; }
        }

        private string _billUser = "";
        public string billUser
        {
            get { return _billUser; }
            set { _billUser = value; }
        }
        /// <summary>
        /// 现金流项目
        /// </summary>
        public string xianjinliuxiangmucode
        {
            get;
            set;
        }
        /// <summary>
        /// 大类code
        /// </summary>
        public string bigclasscode { get; set; }

        /// <summary>
        /// 小类code
        /// </summary>
        public string smallclasscode { get; set; }

        public string bcash { get; set; }
        public string bdept { get; set; }
        public string bperson { get; set; }
        public string bcus { get; set; }
        public string bsup { get; set; }
        public string bitem { get; set; }
        public string cass_item { get; set; }
        /// <summary>
        /// 会计科目主键
        /// </summary>
        public string idcurrency { get; set; }


    }

    /// <summary>
    /// 判断部门id是否是T+系统内的末级部门
    /// </summary>
    /// <param name="strid"></param>
    /// <returns></returns>
    private bool islastdept(string strid)
    {
        if (string.IsNullOrEmpty(strid))
        {
            return false;
        }
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
        if (strlinkdbname.Equals(""))
        {
            showMessage("没有找到用友系统数据库链接，无法绑定凭证类型", false, "");
            return false;
        }
        string strZhangTaodb = this.ddlZhangTao.SelectedValue.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries)[1];
        if (string.IsNullOrEmpty(strZhangTaodb))
        {
            return false;
        }
        string strsql = "select IsEndNode from [{0}].{1}.dbo.AA_Department where Code ='{2}'";
        strsql = string.Format(strsql, strlinkdbname, strZhangTaodb, strid);
        object obj = server.ExecuteScalar(strsql);
        if (obj == null)
        {
            return false;
        }
        return (bool)obj;
    }
    protected void OnNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
}