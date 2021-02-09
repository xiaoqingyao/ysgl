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

public partial class webBill_cwgl_PingZhengDetailForU8 : System.Web.UI.Page
{
    string strCode = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    List<string> lstOsDept = new List<string>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            object objCode = Request["Code"];
            if (objCode != null)
            {
                strCode = objCode.ToString();
            }
            if (!IsPostBack)
            {
                this.bindZhangTao();
                this.BindDataGrid();
                bindPingZhengType();
            }
        }
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
        string strzhangtaoname = this.ddlZhangTao.SelectedItem.Text;
        if (!dt.Year.ToString().Equals(strzhangtaoname.Substring(strzhangtaoname.Length - 4, 4)))
        {
            showMessage("制单日期与选择的帐套不对应，请重新选择", false, ""); return;
        }
        //连接服务器地址
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
        if (strlinkdbname.Equals(""))
        {
            showMessage("没有找到用友系统数据库链接", false, "");
            return;
        }
        //获取凭证号
        string strpingzhenghao = server.GetCellValue("select isnull(max(ino_id),'0')+1 from [" + strlinkdbname + "]." + strZhangTaodb + ".dbo.GL_accvouch where iperiod='" + dt.Month.ToString("00") + "' and csign='" + csign + "'");
        //获取外部接口号
        string strcoutno_id = "";
        //string strcoutno_id = server.GetCellValue("select isnull(coutno_id,'0') from [" + strlinkdbname + "]." + strZhangTaodb + ".dbo.GL_accvouch where left(coutno_id,2)='GL'  order by dbill_date desc ");
        //if (strcoutno_id != "" && strcoutno_id != "0")
        //{
        //    string strnum = strcoutno_id.Substring(2, strcoutno_id.Length - 2);
        //    int num = 0;
        //    if (!int.TryParse(strnum, out num))
        //    {
        //        showMessage("获取外部编号失败", false, ""); return;
        //    }
        //    num += 1;
        //    strcoutno_id = "00000000000000000" + num.ToString();
        //    strcoutno_id = strcoutno_id.Substring(strcoutno_id.Length - 13, 13);
        //    strcoutno_id = "GL" + strcoutno_id;
        //}
        //else
        //{
        //    // showMessage("获取外部编号失败", false, ""); return;
        //    strcoutno_id = "";
        //}
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
                    if (strpingzhengtype.Equals("jf") && ccode_equaldai.IndexOf(ccode) < 0)
                    {
                        ccode_equaldai = ccode_equaldai + ccode + ",";
                    }
                    else if (strpingzhengtype.Equals("df") && ccode_equaljie.IndexOf(ccode) < 0)
                    {
                        ccode_equaljie = ccode_equaljie + ccode + ",";
                    }
                    else { }

                    bool bohasxianjinliuhesuan = this.GridView.Rows[i].Cells[13].Text.Trim().Replace("&nbsp;", "").Equals("True");//是否包含现金流量核算
                    #region 现金流量项目验证
                    string strxianjinliuliangcode = "";

                    DropDownList ddlxianjinliuliang = this.GridView.Rows[i].Cells[4].FindControl("ddlxianjinliu") as DropDownList;
                    if (ddlxianjinliuliang != null)
                    {
                        strxianjinliuliangcode = ddlxianjinliuliang.SelectedValue;
                        if (bohasxianjinliuhesuan && strxianjinliuliangcode.Equals(""))
                        {
                            showMessage("第" + (i + 1).ToString() + "行有现金流量项目核算，请选择明细", false, ""); return;
                        }
                    }
                    else
                    {
                        showMessage("第" + (i + 1).ToString() + "行有现金流量项目核算，请选择明细", false, ""); return;
                    }
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
                for (int i = 0; i < irows; i++)
                {
                    //插入凭证表
                    string strNewid = new GuidHelper().getNewGuid();//行表示
                    string iperiod = dt.Month.ToString("00");//会计期间
                    string ino_id = strpingzhenghao;//凭证号
                    string inid = (i + 1).ToString();//行号
                    string dbill_date = strBillDate;//制单日期
                    string idoc = int.Parse(strBillCount) >= 1 ? strBillCount : "-1";
                    string cbill = new Bll.UserProperty.UserMessage(Session["userCode"].ToString()).Users.UserName;//制单人
                    string ibook = "0";//记账标记
                    string cdigest = "";//摘要
                    #region 获取行摘要
                    TextBox txtZhaiYao = this.GridView.Rows[i].Cells[2].FindControl("txtZhaiYao") as TextBox;
                    if (txtZhaiYao != null)
                    {
                        cdigest = txtZhaiYao.Text.Trim();
                    }
                    #endregion
                    string ccode = "";//科目编码
                    #region 科目编码
                    TextBox txtmxkm = this.GridView.Rows[i].Cells[3].FindControl("txtMingXiKemu") as TextBox;
                    if (txtmxkm != null)
                    {
                        ccode = txtmxkm.Text.Trim();
                        ccode = ccode.Equals("&nbsp;") ? "" : ccode;
                    }
                    if (ccode.Equals(""))
                    {
                        showMessage("第" + (i + 1) + "行明细科目不能为空，请先到部门预算科目处添加对应！", false, "");
                        return;
                    }
                    if (ccode.IndexOf("]") > -1)
                    {
                        ccode = ccode.Substring(1, ccode.IndexOf(']') - 1);
                    }
                    #endregion
                    double md = 0;//借方金额
                    #region 借方金额
                    string strJie = this.GridView.Rows[i].Cells[7].Text;
                    if (!double.TryParse(strJie, out md))
                    {
                        showMessage("行" + i + "借方金额不合法！", false, "");
                        return;
                    }
                    #endregion
                    double mc = 0;//贷方金额
                    #region 贷方金额
                    string strDai = this.GridView.Rows[i].Cells[8].Text;
                    if (!double.TryParse(strDai, out mc))
                    {
                        showMessage("行" + i + "贷方金额不合法！", false, "");
                        return;
                    }
                    #endregion
                    int md_f = 0;//外币借方金额 
                    int mc_f = 0;//外币贷方金额 
                    int nfrat = 0;//汇率
                    int nd_s = 0;//数量借方
                    int nc_s = 0;//数量贷方
                    string cdept_id = "";//核算部门id
                    #region 核算部门6
                    TextBox txtfordept = this.GridView.Rows[i].Cells[4].FindControl("txtForDept") as TextBox;
                    cdept_id = txtfordept.Text.Trim().Replace("&nbsp;", "");
                    bool bdept = this.GridView.Rows[i].Cells[14].Text.Trim().Replace("&nbsp;", "").Equals("True");//是否有部门核算
                    if (bdept && cdept_id.Equals(""))
                    {
                        showMessage("第" + (i + 1).ToString() + "行有部门核算，请到‘部门档案’下维护对应U8系统id或直接填写", false, ""); return;
                    }
                    else if (bdept && !cdept_id.Equals(""))
                    {
                        //判断核算部门是否是末级部门
                        bool boislast = this.islastdept(cdept_id);
                        if (!boislast)
                        {
                            showMessage("第" + (i + 1).ToString() + "行的部门对应U8id不是末级部门，请修改。", false, ""); return;
                        }
                    }
                    #endregion

                    string strxianjinliuxiangname = "";//现金流项目
                    string strxianjinliuliangcode = "";//现金流项目编号
                    bool bohasxianjinliuhesuan = this.GridView.Rows[i].Cells[13].Text.Trim().Replace("&nbsp;", "").Equals("True");//是否包含现金流量核算
                    #region 现金流量项目
                    DropDownList ddlxianjinliuliang = this.GridView.Rows[i].Cells[4].FindControl("ddlxianjinliu") as DropDownList;
                    if (ddlxianjinliuliang != null)
                    {
                        strxianjinliuxiangname = ddlxianjinliuliang.SelectedItem.Text;
                        strxianjinliuliangcode = ddlxianjinliuliang.SelectedValue;
                    }
                    if (bohasxianjinliuhesuan && strxianjinliuliangcode.Equals(""))
                    {
                        showMessage("第" + (i + 1).ToString() + "行有现金流量项目核算，请选择明细", false, ""); return;
                    }
                    #endregion
                    string strbigclasscode = "";//项目大类编号
                    string strsmallclasscode = "";//项目编号
                    #region 大类 小类
                    DropDownList ddlbigclass = this.GridView.Rows[i].Cells[4].FindControl("ddlbigclass") as DropDownList;//大类
                    if (ddlbigclass != null)
                    {
                        strbigclasscode = ddlbigclass.SelectedValue;
                    }
                    DropDownList ddlsmallclass = this.GridView.Rows[i].Cells[4].FindControl("ddlsmallclass") as DropDownList;//小类
                    if (ddlsmallclass != null)
                    {
                        strsmallclasscode = ddlsmallclass.SelectedValue;
                    }
                    bool bitem = this.GridView.Rows[i].Cells[18].Text.Trim().Replace("&nbsp;", "").Equals("True");
                    if (bitem && (strbigclasscode.Equals("") || strsmallclasscode.Equals("")))
                    {
                        showMessage("第" + (i + 1).ToString() + "行有项目核算，请选择大类小类", false, ""); return;
                    }
                    #endregion

                    string duifangkemubianma = "";//对方科目编码
                    #region 对方科目编码
                    string strpingzhengtype = this.GridView.Rows[i].Cells[0].Text.Trim().Replace("&nbsp;", "");
                    if (strpingzhengtype.Equals("jf"))
                    {
                        duifangkemubianma = ccode_equaljie;
                    }
                    else
                    {
                        duifangkemubianma = ccode_equaldai;
                    }
                    #endregion

                    string bFlagOut = "0";//公司对帐是否导出过对帐单 
                    string strsql = "";
                    //行标示   会计期间（月） 凭证类别字 凭证编号   行号  制单日期   附加单据数 制单人 记账标志
                    //摘要  科目编码 币种名称 借方金额  贷方金额  外币借方金额 外币贷方金额 汇率 数量借方 数量贷方  公司对帐是否导出过对帐单 凭证类别排序号 部门 项目大类 项目编号
                    strsql = @"insert into [{0}].{1}.dbo.GL_accvouch (RowGuid,iperiod,csign,ino_id,inid,dbill_date,idoc,cbill,ibook,
                                    cdigest,ccode,md,mc,md_f,mc_f,nfrat,nd_s,nc_s,bFlagOut,isignseq,cdept_id,citem_id,citem_class,bvalueedit,bcodeedit,bPcsedit,bdeptedit,bitemedit,coutno_id,ccode_equal,ioutperiod)
                                    values(
                                      @RowGuid,@iperiod,@csign,@ino_id,@inid,@dbill_date,@idoc,@cbill,@ibook,
                                    @cdigest,@ccode,@md,@mc,@md_f,@mc_f,@nfrat,@nd_s,@nc_s,@bFlagOut,@isignseq,@cdept_id,@citem_id,@citem_class,@bvalueedit,@bcodeedit,@bPcsedit,@bdeptedit,@bitemedit,@coutno_id,@ccode_equal,@ioutperiod
                                    )";
                    strsql = string.Format(strsql, strlinkdbname, strZhangTaodb);
                    SqlParameter[] arrsp1 = { 
                                              new SqlParameter("@RowGuid",SqlNull(strNewid)),new SqlParameter("@iperiod",SqlNull(iperiod)),new SqlParameter("@csign",SqlNull(csign)),
                                              new SqlParameter("@ino_id",SqlNull(ino_id)),new SqlParameter("@inid",SqlNull(inid)),new SqlParameter("@dbill_date",SqlNull(dbill_date)),
                                              new SqlParameter("@idoc",SqlNull(idoc)),new SqlParameter("@cbill",SqlNull(cbill)),new SqlParameter("@ibook",SqlNull(ibook)),
                                              new SqlParameter("@cdigest",SqlNull(cdigest)),new SqlParameter("@ccode",SqlNull(ccode)),new SqlParameter("@md",SqlNull(Math.Round(md,2))),
                                              new SqlParameter("@mc",SqlNull(Math.Round(mc,2))),new SqlParameter("@md_f",SqlNull(md_f)),new SqlParameter("@mc_f",SqlNull(mc_f)),
                                              new SqlParameter("@nfrat",SqlNull(nfrat)),new SqlParameter("@nd_s",SqlNull(nd_s)),new SqlParameter("@nc_s",SqlNull(nc_s)),
                                              new SqlParameter("@bFlagOut",SqlNull(bFlagOut)),new SqlParameter("@isignseq",SqlNull(iSignSeq)),new SqlParameter("@cdept_id",SqlNull(cdept_id)),
                                              new SqlParameter("@citem_id",SqlNull(strbigclasscode)),new SqlParameter("@citem_class",SqlNull(strsmallclasscode)),new SqlParameter("@bvalueedit",'1'),
                                              new SqlParameter("@bcodeedit",'1'),new SqlParameter("@bPcsedit",'1'),new SqlParameter("@bdeptedit",'1'),
                                              new SqlParameter("@bitemedit",'1'),new SqlParameter("@coutno_id",SqlNull(strcoutno_id)),new SqlParameter("@ccode_equal",SqlNull(duifangkemubianma)),new SqlParameter("@ioutperiod", DBNull.Value)
                                           };
                    lstpingzheng.Add(new pingzheng() { sql = strsql, arrsp = arrsp1 });
                    //int irel = DataHelper.ExcuteNonQuery(strsql, null, false);//之前是先插入凭证表 然后再插入流量表  后来有了问题是  前面插入一遍之后 后边出错就没法往后退  又不能使用事务
                    //插入财务凭证表
                    //如果有现金流量核算 需要在现金流量表插入记录
                    if (bohasxianjinliuhesuan)
                    {
                        string strinsertsql = "";
                        strinsertsql = @"insert into [{0}].{1}.dbo.GL_cashtable(iPeriod,iSignSeq,iNo_id,inid,cCashItem,md,mc,ccode,cdept_id,dbill_date,csign,md_f,mc_f,nd_s,nc_s)
                                             values(@iPeriod,@iSignSeq,@iNo_id,@inid,@cCashItem,@md,@mc,@ccode,@cdept_id,@dbill_date,@csign,'0','0','0','0')
                                            ";
                        strinsertsql = string.Format(strinsertsql, strlinkdbname, strZhangTaodb);
                        SqlParameter[] arrsp2 = { 
                                                new SqlParameter("@iPeriod",SqlNull(iperiod)),new SqlParameter("@iSignSeq",SqlNull(iSignSeq)),new SqlParameter("@iNo_id",SqlNull(ino_id)),
                                                new SqlParameter("@inid",SqlNull(inid)),new SqlParameter("@cCashItem",SqlNull(strxianjinliuliangcode)),new SqlParameter("@md",SqlNull(Math.Round(md,2))),
                                                new SqlParameter("@mc",SqlNull(Math.Round(mc,2))),new SqlParameter("@ccode",SqlNull(ccode)),new SqlParameter("@cdept_id",SqlNull(cdept_id)),
                                                new SqlParameter("@dbill_date",SqlNull(dbill_date)),new SqlParameter("@csign",SqlNull(csign))
                                                };
                        lstpingzheng.Add(new pingzheng() { sql = strinsertsql, arrsp = arrsp2 });
                    }
                    dbsummd += md;
                    dbsummc += mc;
                }
                //验证借贷平衡
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
                            lbeMsg.Text = "操作结果：凭证保存成功，但报销单处理失败，可以自动将凭证号保存或联系管理员解决！";
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
            itemmodel.dfje = this.GridView.Rows[i].Cells[8].Text.Trim();
            itemmodel.jfje = this.GridView.Rows[i].Cells[7].Text.Trim();
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
            //现金流量项目
            DropDownList ddlxianjinliuliang = this.GridView.Rows[i].Cells[4].FindControl("ddlxianjinliu") as DropDownList;
            if (ddlxianjinliuliang != null)
            {
                itemmodel.xianjinliuxiangmucode = ddlxianjinliuliang.SelectedValue;
            }
            //大类
            DropDownList ddlbigclass = this.GridView.Rows[i].Cells[4].FindControl("ddlbigclass") as DropDownList;
            if (ddlbigclass != null)
            {
                itemmodel.bigclasscode = ddlbigclass.SelectedValue;
            }
            //小类
            DropDownList ddlsmallclass = this.GridView.Rows[i].Cells[4].FindControl("ddlsmallclass") as DropDownList;
            if (ddlsmallclass != null)
            {
                itemmodel.smallclasscode = ddlsmallclass.SelectedValue;
            }
            //现金流项目
            string bcash = this.GridView.Rows[i].Cells[13].Text.Trim();
            itemmodel.bcash = bcash.Replace("&nbsp;", "");


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
                e.Row.Cells[7].Text = "0";
            }
            else
            {
                e.Row.Cells[8].Text = "0";
            }

            //显示摘要到textbox
            string strZY = e.Row.Cells[1].Text.Trim();
            System.Web.UI.WebControls.TextBox textbox = e.Row.Cells[2].FindControl("txtZhaiYao") as System.Web.UI.WebControls.TextBox;
            if (textbox != null)
            {
                textbox.Text = strZY;
            }
            //合计行
            string strJie = e.Row.Cells[7].Text.Trim();
            double dbJie = 0;
            if (double.TryParse(strJie, out dbJie))
            {
                dbjfjeZong += dbJie;
            }
            string strDai = e.Row.Cells[8].Text.Trim();
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

            //绑定现金流量项目
            DropDownList ddlxianjinliuliang = e.Row.Cells[4].FindControl("ddlxianjinliu") as DropDownList;
            if (ddlxianjinliuliang != null)
            {
                ddlxianjinliuliang.DataSource = this.getAllXianJinLiuXiang();
                ddlxianjinliuliang.DataTextField = "citemname";
                ddlxianjinliuliang.DataValueField = "citemcode";
                ddlxianjinliuliang.DataBind();
                ddlxianjinliuliang.Items.Insert(0, new ListItem("-现金流量项目-", ""));
                //为现金流量项目设置默认值
                string strxianjinliuliang = e.Row.Cells[22].Text.Trim();
                strxianjinliuliang = strxianjinliuliang.Replace("&nbsp;", "");
                if (!strxianjinliuliang.Equals(""))
                {
                    ddlxianjinliuliang.SelectedValue = strxianjinliuliang;
                }
            }

            //获取大类
            DropDownList ddlbigclass = e.Row.Cells[4].FindControl("ddlbigclass") as DropDownList;
            if (ddlbigclass != null)
            {
                ddlbigclass.DataSource = this.getBigClass();
                ddlbigclass.DataTextField = "citem_name";
                ddlbigclass.DataValueField = "citem_class";
                ddlbigclass.DataBind();
                ddlbigclass.Items.Insert(0, new ListItem("-大类-", ""));
                //为大类设置默认值
                string strcass_item = e.Row.Cells[19].Text.Trim(); //原来财务科目对应的大类
                strcass_item = strcass_item.Replace("&nbsp;", "");
                if (!strcass_item.Equals(""))//
                {
                    ddlbigclass.SelectedValue = strcass_item;
                }
                string strbigclass = e.Row.Cells[20].Text.Trim();//可能后来改的
                strbigclass = strbigclass.Replace("&nbsp;", "");
                if (!strbigclass.Equals(""))
                {
                    ddlbigclass.SelectedValue = strbigclass;
                }
                //根据大类绑定小类
                DropDownList ddlsmallclass = e.Row.Cells[4].FindControl("ddlsmallclass") as DropDownList;
                if (ddlsmallclass != null)
                {
                    ddlsmallclass.Items.Clear();
                    string strbigclasscode = ddlbigclass.SelectedValue;
                    if (!string.IsNullOrEmpty(strbigclasscode))
                    {
                        ddlsmallclass.DataSource = this.getSmallClass(strbigclasscode);
                        ddlsmallclass.DataTextField = "citemname";
                        ddlsmallclass.DataValueField = "citemcode";
                        ddlsmallclass.DataBind();
                        ddlsmallclass.Items.Insert(0, new ListItem("-小类-", ""));
                        //为小类设置默认值
                        string strsmallclass = e.Row.Cells[21].Text.Trim();
                        strsmallclass = strsmallclass.Replace("&nbsp;", "");
                        if (!strsmallclass.Equals(""))
                        {
                            ddlsmallclass.SelectedValue = strsmallclass;
                        }
                    }
                }
            }
            //根据会计科目  设置隐藏域是否有现金流项目的控制
            TextBox txtcwkm = e.Row.Cells[3].FindControl("txtMingXiKemu") as TextBox;
            if (txtcwkm != null)
            {
                string strcwkm = txtcwkm.Text.Trim().Replace("&nbsp;", "");
                try
                {
                    strcwkm = strcwkm.Substring(1, strcwkm.IndexOf("]") - 1);
                }
                catch (Exception)
                {
                }
                if (!strcwkm.Equals(""))
                {
                    //根据财务科目读取是否有现金流项目的控制
                    string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
                    if (strlinkdbname.Equals(""))
                    {
                        return;
                    }
                    string strZhangTaodb = this.ddlZhangTao.SelectedValue;
                    if (string.IsNullOrEmpty(strZhangTaodb))
                    {
                        return;
                    }
                    strZhangTaodb = strZhangTaodb.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    string sql = "select bcash from [{0}].{1}.dbo.code where ccode='{2}'";
                    sql = string.Format(sql, strlinkdbname, strZhangTaodb, strcwkm);
                    string bcash = server.GetCellValue(sql);
                    if (!string.IsNullOrEmpty(bcash))
                    {
                        e.Row.Cells[13].Text = bcash;
                    }
                }
            }
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
        BindDataGrid();
    }

    #region 私有方法
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
        this.txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        bindZhaiYao();


        //绑定明细
        string[] arrCode = strCode.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries);
        string strInBillCode = "";
        for (int i = 0; i < arrCode.Length; i++)
        {
            strInBillCode += string.Format("'{0}',", arrCode[i]);
        }
        strInBillCode = strInBillCode.Substring(0, strInBillCode.Length - 1);
        #region 检查所选单据是否有已经生成凭证了的了
        //string strexitsql = "select count(*) from bill_ybbxmxb where billcode in (" + strInBillCode + ") and isnull(pzcode,'')!=''";
        //if (int.Parse(server.GetCellValue(strexitsql)) > 0)
        //{
        //    showMessage("所选单据包含已经生成了凭证的纪录。", true, "");
        //    return;
        //}
        #endregion

        string strSql = "";
        string strflg = new Bll.ConfigBLL().GetValueByKey("pingzhengbygkorsy");
        bool boguikoufenjie = new Bll.ConfigBLL().GetValueByKey("UseGKFJ").Equals("1");
        //if (strflg.Equals("1") && !boguikoufenjie)
        //{
        //            strSql = @"					select 
        //	                    (select '['+deptCode+']'+deptName from bill_departments where deptCode=dept.deptcode) as billDept,
        //	                    (select billUser from bill_main where billCode=fykm.billCode) as billUser,
        //	                    fykm,dept.je as jfje,dept.je as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=fykm.    fykm and deptCode =(select gkdept from bill_main where billCode=fykm.billCode))) as fykmName,
        //	                    (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=fykm.fykm and deptCode =(select gkdept from bill_main     where billCode=fykm.billCode))) as fuzhuhesuan,
        //	                    (select top 1 bxzy from bill_ybbxmxb where billcode=fykm.billcode) as bxzy,'jf' as PingZhengType,(select isnull(forU8id,'') from bill_departments where deptCode=dept.deptcode) as OsDept 
        //	                    from bill_ybbxmxb_fykm fykm,bill_ybbxmxb_fykm_dept dept where fykm.mxGuid=dept.kmmxGuid
        //		                    and fykm.billCode in ({0})
        //	                    union all
        //                        select	(select '['+deptCode+']'+deptName from bill_departments where deptCode=(select top 1 dept.deptcode from bill_ybbxmxb_fykm_dept dept where  fykm.mxGuid=dept.kmmxGuid)) as billDept,
        //	                    '' as billUser,
        //	                    fykm,fykm.se as jfje,fykm.se as dfje,'' as fykmName,
        //	                   (select FuZhuHeSuan from bill_cwkm where cwkmCode='22210101') as fuzhuhesuan,
        //	                    '税额' as bxzy,'se' as PingZhengType,'' as OsDept
        //	                    from bill_ybbxmxb_fykm fykm
        //	                    where fykm.billCode in ({0}) and fykm.se>0
        //	                    union all
        //                         select 
        //	                    '' as billDept,
        //	                    '' as billUser,
        //	                    fykm,fykm.je as jfje,(fykm.je+fykm.se) as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=fykm.    fykm and deptCode =(select gkdept from bill_main where billCode=fykm.billCode))) as fykmName,
        //	                    (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=fykm.fykm and deptCode =(select gkdept from bill_main     where billCode=fykm.billCode))) as fuzhuhesuan,
        //	                    (select top 1 bxzy from bill_ybbxmxb where billcode=fykm.billcode) as bxzy,'df' as PingZhengType,(select isnull(forU8id,'') from bill_departments where deptCode=dept.deptcode) as OsDept 
        //	                    from bill_ybbxmxb_fykm fykm,bill_ybbxmxb_fykm_dept dept where fykm.mxGuid=dept.kmmxGuid
        //		                    and fykm.billCode in ({0}) 
        //                    ";
        //            strSql = string.Format(strSql, strInBillCode);
        //}
        //else
        //{
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

        strSql = @"
                    select '' as bigclasscode,'' as smallclasscode,'' as xianjinliuxiangmucode, bxzy,sum(jfje) as jfje,sum(dfje) as dfje,billdept,billuser,fykm,fykmname,fykmcode,fuzhuhesuan,pingzhengtype,osdept,bcash,bperson,bcus,bsup,bdept,bitem,cass_item from (
	                select fymx.*,bcash,bperson,bcus,bsup,bdept,bitem,cass_item
		            from (
					select
					(select '['+deptCode+']'+deptName from bill_departments where deptCode=bill_ybbxmxb_fykm_dept.deptcode) as billDept,
                    (select billUser from bill_main where billCode=bill_ybbxmxb_fykm.billCode) as billUser,
                    fykm,bill_ybbxmxb_fykm_dept.je as jfje,bill_ybbxmxb_fykm_dept.je as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =bill_ybbxmxb_fykm_dept.deptcode)) as fykmName,
					(select cwkmCode from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =bill_ybbxmxb_fykm_dept.deptcode)) as fykmCode,
                    (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =bill_ybbxmxb_fykm_dept.deptcode)) as fuzhuhesuan,
                    (select top 1 bxzy from bill_ybbxmxb where billCode=bill_ybbxmxb_fykm.billCode) as bxzy,'jf' as PingZhengType,(select isnull(forU8id,'') from bill_departments where deptCode=bill_ybbxmxb_fykm_dept.deptcode) as OsDept  from bill_ybbxmxb_fykm,bill_ybbxmxb_fykm_dept where bill_ybbxmxb_fykm.mxGuid=bill_ybbxmxb_fykm_dept.kmmxGuid and bill_ybbxmxb_fykm_dept.je>0 and billCode in (select billcode from bill_main where billname in (select billname from bill_main where billcode in ({0})))
                    union all
                    select billdept,billuser,fykm,sum(jfje) as jfje,sum(dfje) as dfje,fykmname,fykmcode,fuzhuhesuan,bxzy,pingzhengtype,osdept from (
                        select (select '['+deptCode+']'+deptName from bill_departments where deptCode=(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)) as billDept,
                        (select billUser from bill_main where billCode=bill_ybbxmxb_fykm.billCode) as billUser,
                        '' as fykm,je as jfje,je as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fykmName,
					    (select cwkmCode from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fykmCode,
                        (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fuzhuhesuan,
                        (select top 1 bxzy from bill_ybbxmxb where billCode=bill_ybbxmxb_fykm.billCode) as bxzy,'df' as PingZhengType,(select isnull(forU8id,'') from bill_departments where deptCode=(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)) as OsDept  
                        from bill_ybbxmxb_fykm where billCode in (select billcode from bill_main where billname in (select billname from bill_main where billcode in ({0})))
                    )lsb group by billdept,billuser,fykm,fykmname,fykmcode,fuzhuhesuan,bxzy,pingzhengtype,osdept
		   ) fymx inner join 
			[{1}].{2}.dbo.code code 
			on fymx.fykmcode=code.ccode where code.bend='1'
		) tend where bdept='1'
		group by billdept,billuser,fykm,fykmname,fykmcode,fuzhuhesuan,pingzhengtype,osdept,bcash,bperson,bcus,bsup,bdept,bitem,cass_item,bxzy
        union all
        select  '' as bigclasscode,'' as smallclasscode,'' as xianjinliuxiangmucode,bxzy,sum(jfje) as jfje,sum(dfje) as dfje,'',billuser,fykm,fykmname,fykmcode,fuzhuhesuan,pingzhengtype,'',bcash,bperson,bcus,bsup,bdept,bitem,cass_item from (
	        select fymx.*,bcash,bperson,bcus,bsup,bdept,bitem,cass_item
		        from (
					select
					(select '['+deptCode+']'+deptName from bill_departments where deptCode=bill_ybbxmxb_fykm_dept.deptcode) as billDept,
                    (select billUser from bill_main where billCode=bill_ybbxmxb_fykm.billCode) as billUser,
                    fykm,bill_ybbxmxb_fykm_dept.je as jfje,bill_ybbxmxb_fykm_dept.je as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =bill_ybbxmxb_fykm_dept.deptcode)) as fykmName,
					(select cwkmCode from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =bill_ybbxmxb_fykm_dept.deptcode)) as fykmCode,
                    (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =bill_ybbxmxb_fykm_dept.deptcode)) as fuzhuhesuan,
                    (select top 1 bxzy from bill_ybbxmxb where billCode=bill_ybbxmxb_fykm.billCode) as bxzy,'jf' as PingZhengType,(select isnull(forU8id,'') from bill_departments where deptCode=bill_ybbxmxb_fykm_dept.deptcode) as OsDept  from bill_ybbxmxb_fykm,bill_ybbxmxb_fykm_dept where bill_ybbxmxb_fykm.mxGuid=bill_ybbxmxb_fykm_dept.kmmxGuid and bill_ybbxmxb_fykm_dept.je>0 and billCode in (select billcode from bill_main where billname in (select billname from bill_main where billcode in ({0})))
                    union all
                    select billdept,billuser,fykm,sum(jfje) as jfje,sum(dfje) as dfje,fykmname,fykmcode,fuzhuhesuan,bxzy,pingzhengtype,osdept from (
                        select (select '['+deptCode+']'+deptName from bill_departments where deptCode=(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)) as billDept,
                        (select billUser from bill_main where billCode=bill_ybbxmxb_fykm.billCode) as billUser,
                       '' as fykm,je as jfje,je as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fykmName,
					    (select cwkmCode from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fykmCode,
                        (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fuzhuhesuan,
                        (select top 1 bxzy from bill_ybbxmxb where billCode=bill_ybbxmxb_fykm.billCode) as bxzy,'df' as PingZhengType,(select isnull(forU8id,'') from bill_departments where deptCode=(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)) as OsDept  
                        from bill_ybbxmxb_fykm where billCode in (select billcode from bill_main where billname in (select billname from bill_main where billcode in ({0})))
                    )lsb group by billdept,billuser,fykm,fykmname,fykmcode,fuzhuhesuan,bxzy,pingzhengtype,osdept
		   ) fymx inner join 
			[{1}].{2}.dbo.code code 
			on fymx.fykmcode=code.ccode where code.bend='1'
		) tend where bdept='0'
		group by billuser,fykm,fykmname,fykmcode,fuzhuhesuan,pingzhengtype,bcash,bperson,bcus,bsup,bdept,bitem,cass_item,bxzy";
        strSql = string.Format(strSql, strInBillCode, strlinkdbname, strZhangTaodb);
        //}
        DataTable dtRel = server.GetDataTable(strSql, null);
        this.GridView.DataSource = dtRel;
        this.GridView.DataBind();

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
        string strselectsql = @"select *,cAcc_Id+'|*|'+db_data as tval from (	SELECT    
					                b.cAcc_Id, SUBSTRING(a.name, 8, 3) + b.cAcc_Name + RIGHT(a.name, 4) AS cAcc_Name, b.cAcc_Master,
					                UPPER(a.name) AS db_data,SUBSTRING(UPPER(a.name),12,4) as year
                                    FROM         
                                    [{0}].master.dbo.sysdatabases AS a INNER JOIN
                                    [{0}].UFSystem.dbo.UA_Account AS b ON SUBSTRING(a.name, 8, 3) = b.cAcc_Id
                                    WHERE      (UPPER(a.name) LIKE 'UFDATA%')
                                    ) c order by c.year desc,c.cacc_id asc";
        strselectsql = string.Format(strselectsql, strlinkdbname);
        this.ddlZhangTao.DataSource = server.GetDataTable(strselectsql, null);
        this.ddlZhangTao.DataTextField = "cAcc_Name";
        this.ddlZhangTao.DataValueField = "tval";
        this.ddlZhangTao.DataBind();
        //this.ddlZhangTao.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-选择帐套-", ""));
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
            string strsql = @"select ctext,csign+'|'+cast(isignseq as varchar(10)) as cval from [{0}].{1}.dbo.dsign";
            strsql = string.Format(strsql, strlinkdbname, strZhangTaodb);
            DataTable dtrel = server.GetDataTable(strsql, null);
            ddlPingZhengType.DataSource = dtrel;
            this.ddlPingZhengType.DataTextField = "ctext";
            this.ddlPingZhengType.DataValueField = "cval";
            this.ddlPingZhengType.DataBind();
            this.ddlPingZhengType.SelectedValue = "02|2";
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
    /// 获取所有的现金流项目
    /// </summary>
    /// <returns></returns>
    private DataTable getAllXianJinLiuXiang()
    {
        DataTable dtreturn = new DataTable();
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
        if (strlinkdbname.Equals(""))
        {
            showMessage("没有找到用友系统数据库链接，无法绑定凭证类型", false, "");
            return dtreturn;
        }

        string strZhangTaodb = this.ddlZhangTao.SelectedValue;
        if (string.IsNullOrEmpty(strZhangTaodb))
        {
            return dtreturn;
        }
        strZhangTaodb = strZhangTaodb.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries)[1];
        if (!string.IsNullOrEmpty(strZhangTaodb))
        {
            string strsql = @"select citemcode,citemname  from [{0}].{1}.dbo.fitemss98";
            strsql = string.Format(strsql, strlinkdbname, strZhangTaodb);
            DataTable dtrel = server.GetDataTable(strsql, null);
            for (int i = 0; i < dtrel.Rows.Count; i++)
            {
                string strname = dtrel.Rows[i]["citemname"].ToString();
                dtrel.Rows[i]["citemname"] = i.ToString("00") + strname;
            }
            return dtrel;
        }
        else
        {
            return dtreturn;
        }
    }

    /// <summary>
    /// 获取大类
    /// </summary>
    /// <returns></returns>
    private DataTable getBigClass()
    {
        DataTable dtreturn = new DataTable();
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
        if (strlinkdbname.Equals(""))
        {
            showMessage("没有找到用友系统数据库链接，无法绑定凭证类型", false, "");
            return dtreturn;
        }

        string strZhangTaodb = this.ddlZhangTao.SelectedValue;
        if (string.IsNullOrEmpty(strZhangTaodb))
        {
            return dtreturn;
        }
        strZhangTaodb = strZhangTaodb.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries)[1];
        if (!string.IsNullOrEmpty(strZhangTaodb))
        {
            string strsql = @"select citem_class,citem_name from [{0}].{1}.dbo.fitem";
            strsql = string.Format(strsql, strlinkdbname, strZhangTaodb);
            DataTable dtrel = server.GetDataTable(strsql, null);
            return dtrel;
        }
        else
        {
            return dtreturn;
        }
    }
    /// <summary>
    /// 获取小类
    /// </summary>
    /// <returns></returns>
    private DataTable getSmallClass(string bigclasscode)
    {
        DataTable dtreturn = new DataTable();
        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");
        if (strlinkdbname.Equals(""))
        {
            showMessage("没有找到用友系统数据库链接，无法绑定凭证类型", false, "");
            return dtreturn;
        }

        string strZhangTaodb = this.ddlZhangTao.SelectedValue;
        if (string.IsNullOrEmpty(strZhangTaodb))
        {
            return dtreturn;
        }

        strZhangTaodb = strZhangTaodb.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries)[1];
        if (!string.IsNullOrEmpty(strZhangTaodb))
        {
            string strsql = @"select citemcode,citemname from [{0}].{1}.dbo.fitemss{2} ";
            strsql = string.Format(strsql, strlinkdbname, strZhangTaodb, bigclasscode);
            DataTable dtrel = server.GetDataTable(strsql, null);
            return dtrel;
        }
        else
        {
            return dtreturn;
        }
    }
    protected void bigclasschanged(object sender, EventArgs e) { reload(); }
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
            itemmodel.dfje = this.GridView.Rows[i].Cells[8].Text.Trim();
            itemmodel.jfje = this.GridView.Rows[i].Cells[7].Text.Trim();
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

    }

    /// <summary>
    /// 判断部门id是否是u8系统内的末级部门
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
        string strsql = "select bDepEnd from [{0}].{1}.dbo.Department where cDepCode ='{2}'";
        strsql = string.Format(strsql, strlinkdbname, strZhangTaodb, strid);
        object obj = server.ExecuteScalar(strsql);
        //if (obj == null)
        //{
        //    return false;
        //}
        //else if (string.IsNullOrEmpty(obj.ToString()))
        //{
        //    return false;
        //}
        //else
        //{
        //    return obj.ToString() == "0" ? false : true;
        //}
        if (obj == null)
        {
            return false;
        }
        return (bool)obj;
    }
}
