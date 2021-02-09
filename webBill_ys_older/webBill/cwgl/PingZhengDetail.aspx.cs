using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using System.Xml;

public partial class webBill_cwgl_PingZhengDetail : System.Web.UI.Page
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
            ClientScript.RegisterArrayDeclaration("availableTags", getAllOsDept());
            if (!IsPostBack)
            {
                this.BindDataGrid();
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
        int iBillCount = 0;
        if (!int.TryParse(strBillCount, out iBillCount))
        {
            showMessage("附加单据数必须为阿拉伯数字！", false, "");
            return;
        }
        string strZhangTaoMsg = this.ddlZhangTao.SelectedValue;
        if (string.IsNullOrEmpty(strZhangTaoMsg))
        {
            showMessage("必须选择帐套！", false, "");
            return;
        }
        string[] arrZhangTaoMsg = strZhangTaoMsg.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries);
        if (arrZhangTaoMsg.Length < 2)
        {
            showMessage("帐套绑定有误，请联系管理员解决！", false, ""); return;
        }
        string strZhangTaoCode = arrZhangTaoMsg[0].Trim();//帐套号
        string strSender = arrZhangTaoMsg[1].Trim();//接受编号
        string strZhangTaoName = this.ddlZhangTao.SelectedItem.Text.Trim();//帐套名字
        if (string.IsNullOrEmpty(strZhangTaoName))
        {
            showMessage("该帐套没有对应的单位，请到数据字段的“10”下修改！", false, "");
            return;
        }
        string strNewid = new GuidHelper().getNewGuid();

        string strPingZhengType = "记账凭证";
        //生成xml文件
        StringBuilder sbMain = new StringBuilder("<?xml version='1.0' encoding='gb2312' standalone='no'?>");
        sbMain.Append("<ufinterface billtype='gl' codeexchanged='y' docid='989898989898' proc='add' receiver='2000' roottag='voucher' sender='" + strSender + "'>");
        sbMain.Append("<voucher id='" + strNewid + "'>");
        #region head
        string strBillDate = this.txtDate.Text.Trim();
        if (strBillDate.Equals(""))
        {
            showMessage("单据时间不能为空！", false, "");
            return;
        }
        DateTime dt;
        if (!DateTime.TryParse(strBillDate, out dt))
        {
            showMessage("单据时间不合法！", false, "");
            return;
        }
        sbMain.Append("<voucher_head>");
        sbMain.Append("<company>" + strZhangTaoName + "</company>");
        sbMain.Append("<voucher_type>" + strPingZhengType + "</voucher_type>");
        sbMain.Append("<fiscal_year>" + dt.Year.ToString() + "</fiscal_year>");
        sbMain.Append("<accounting_period>" + dt.Month.ToString("00") + "</accounting_period>");
        sbMain.Append("<voucher_id>0</voucher_id>");
        sbMain.Append("<attachment_number>" + strBillCount + "</attachment_number>");
        sbMain.Append("<date>" + strBillDate + "</date>");
        sbMain.Append("<enter>" + new Bll.UserProperty.UserMessage(Session["userCode"].ToString()).Users.UserName + "</enter>");
        sbMain.Append("<cashier></cashier>");
        sbMain.Append("<signature>Y</signature>");
        sbMain.Append("<checker></checker>");
        sbMain.Append("<posting_date></posting_date>");
        sbMain.Append("<posting_person></posting_person>");
        sbMain.Append("<voucher_making_system>总账</voucher_making_system>");
        sbMain.Append("<memo1></memo1>");
        sbMain.Append("<memo2></memo2>");
        sbMain.Append("<reserve1></reserve1>");
        sbMain.Append("<reserve2></reserve2>");
        sbMain.Append("<revokeflag></revokeflag>");
        sbMain.Append("</voucher_head>");
        #endregion end voucher_head
        #region 生成body
        string strBodyEnd = "";
        int iGridViewRows = this.GridView.Rows.Count;
        for (int i = 0; i < iGridViewRows; i++)
        {
            StringBuilder sbEveBody = new StringBuilder("<entry>");
            sbEveBody.Append("<entry_id>" + (i + 1).ToString() + "</entry_id>");
            string strKmCode = "";
            TextBox txtmxkm = this.GridView.Rows[i].Cells[4].FindControl("txtMingXiKemu") as TextBox;
            if (txtmxkm != null)
            {
                strKmCode = txtmxkm.Text.Trim();
                strKmCode = strKmCode.Equals("&nbsp;") ? "" : strKmCode;
            }
            if (strKmCode.Equals(""))
            {
                showMessage("明细科目不能为空，请先到部门预算科目处添加对应或者直接选择！", false, "");
                return;
            }
            if (strKmCode.IndexOf("]") > -1)
            {
                strKmCode = strKmCode.Substring(1, strKmCode.IndexOf(']') - 1);
            }
            string strKmabstract = "";
            TextBox txtZhaiYao = this.GridView.Rows[i].Cells[3].FindControl("txtZhaiYao") as TextBox;
            if (txtZhaiYao != null)
            {
                strKmabstract = txtZhaiYao.Text.Trim();
            }

            sbEveBody.Append("<account_code>" + strKmCode + "</account_code>");
            sbEveBody.Append("<abstract>" + strKmabstract + "</abstract>");
            sbEveBody.Append("<settlement></settlement>");
            sbEveBody.Append("<document_id></document_id>");
            sbEveBody.Append("<document_date></document_date>");
            sbEveBody.Append("<currency>人民币</currency>");
            sbEveBody.Append("<unit_price>0</unit_price>");
            sbEveBody.Append("<exchange_rate1>0</exchange_rate1>");
            sbEveBody.Append("<exchange_rate2>1</exchange_rate2>");
            sbEveBody.Append("<debit_quantity>0</debit_quantity>");
            //借方金额
            string strJie = ((TextBox)this.GridView.Rows[i].Cells[8].FindControl("txtjfje")).Text.Trim(); //this.GridView.Rows[i].Cells[7].Text;
            double dbJie = 0;
            if (strJie.Equals(""))
            {
                dbJie = 0;
            }
            else if (!double.TryParse(strJie, out dbJie))
            {
                showMessage("行" + i + "借方金额不合法！", false, "");
                return;
            }
            else { }
            sbEveBody.Append("<primary_debit_amount>" + strJie + "</primary_debit_amount>");//借方1
            sbEveBody.Append("<secondary_debit_amount>0.00000000</secondary_debit_amount>");//借方2
            sbEveBody.Append("<secondary_debit_amount>0</secondary_debit_amount>");
            sbEveBody.Append("<natural_debit_currency>" + strJie + "</natural_debit_currency>");
            sbEveBody.Append("<credit_quantity>0</credit_quantity>");
            //贷方金额
            string strDai = ((TextBox)this.GridView.Rows[i].Cells[9].FindControl("txtdfje")).Text.Trim(); //this.GridView.Rows[i].Cells[8].Text;
            double dbDai = 0;
            if (strDai.Equals(""))
            {
                dbDai = 0;
            }
            else if (!double.TryParse(strDai, out dbDai))
            {
                showMessage("行" + i + "贷方金额不合法！", false, "");
                return;
            }
            else { }
            sbEveBody.Append("<primary_credit_amount>" + strDai + "</primary_credit_amount>");//贷方1
            sbEveBody.Append("<secondary_credit_amount>0</secondary_credit_amount>");//贷方2
            sbEveBody.Append("<natural_credit_currency>" + strDai + "</natural_credit_currency>");
            sbEveBody.Append("<bill_type></bill_type>");
            sbEveBody.Append("<bill_id></bill_id>");
            sbEveBody.Append("<bill_date></bill_date>");
            #region 辅助核算
            string strFuZhuHeSuanEndString = "";
            //1.下拉框需要选择的
            DropDownList ddlFuZhuHeSuan = this.GridView.Rows[i].Cells[5].FindControl("ddlFuZhuHeSuan") as DropDownList;
            if (ddlFuZhuHeSuan != null && ddlFuZhuHeSuan.Items.Count > 0)
            {
                strFuZhuHeSuanEndString += string.Format("<item name=\"{0}\">{1}</item>", ddlFuZhuHeSuan.SelectedItem.Value, ddlFuZhuHeSuan.SelectedItem.Text);
            }
            //其它的都去读默认的
            string strAllFuzhu = this.GridView.Rows[i].Cells[10].Text.Trim();
            strAllFuzhu = strAllFuzhu.Equals("&nbsp;") ? "" : strAllFuzhu;
            if (!strAllFuzhu.Equals(""))
            {
                string[] arrAllFuzhu = strAllFuzhu.Split(new char[] { '[' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < arrAllFuzhu.Length; j++)
                {
                    string strEveFuZhuName = arrAllFuzhu[j].Substring(0, arrAllFuzhu[j].Length - 1);
                    if (strEveFuZhuName.Equals(ddlFuZhuHeSuan.SelectedValue))
                    {
                        continue;
                    }
                    if (strEveFuZhuName.Equals("人员档案") || strEveFuZhuName.Equals("部门档案"))
                    {   //人员档案和部门档案去读取人员和档案基础信息表
                        if (strEveFuZhuName.Equals("人员档案"))
                        {
                            string strUserCode = this.GridView.Rows[i].Cells[11].Text;
                            if (!strUserCode.Equals(""))
                            {
                                Bll.UserProperty.UserMessage usermsg = new Bll.UserProperty.UserMessage(strUserCode);
                                strFuZhuHeSuanEndString += string.Format("<item name=\"{0}\">{1}</item>", "人员档案", usermsg.Users.UserName);
                            }
                        }
                        else
                        {
                            TextBox txtDept = this.GridView.Rows[i].Cells[7].FindControl("txtForDept") as TextBox;
                            if (txtDept == null || string.IsNullOrEmpty(txtDept.Text))
                            {
                                showMessage("第" + i + 1 + "行的财务科目的辅助核算含有“部门档案”，请务必选择对应财务系统的部门的名称！，", false, "");
                                return;
                            }
                            strFuZhuHeSuanEndString += string.Format("<item name=\"{0}\">{1}</item>", "部门档案", txtDept.Text.Trim());
                        }
                    }
                    else
                    {
                        //客商辅助核算等 去基础信息 项目核算中读取

                        string strEveFuZhuValue = new Bll.PingZheng.PingZheng_XMBLL().GetChildByName(strEveFuZhuName).xmName;
                        strFuZhuHeSuanEndString += string.Format("<item name=\"{0}\">{1}</item>", strEveFuZhuName, strEveFuZhuValue);
                    }
                }
                strFuZhuHeSuanEndString = string.Format("<auxiliary_accounting>{0}</auxiliary_accounting>", strFuZhuHeSuanEndString);

            }
            #endregion
            sbEveBody.Append(strFuZhuHeSuanEndString);
            sbEveBody.Append("<detail></detail>");
            sbEveBody.Append("</entry>");
            strBodyEnd += sbEveBody.ToString();
        }
        sbMain.Append(string.Format("<voucher_body>{0}</voucher_body>", strBodyEnd));
        #endregion
        sbMain.Append("</voucher>");
        sbMain.Append("</ufinterface>");
        //发送
        string strResult = "";
        try
        {
            strResult = sendToNC(sbMain.ToString(), strZhangTaoCode);

        }
        catch (Exception ex)
        {
            this.lbeMsg.Text = "错误信息：" + ex.Message;
            this.btnSave.Enabled = false;
            return;
        }
        string strPingZhengHao = "";
        string strRelMsg = strResult.Substring(strResult.IndexOf("<resultdescription>") + 19, strResult.IndexOf("</resultdescription>") - strResult.IndexOf("<resultdescription>") - 19);
        strRelMsg = strRelMsg.Replace("\r\n", "");
        string strPingZheng = strResult.Substring(strResult.IndexOf("<content>") + 9, strResult.IndexOf("</content>") - strResult.IndexOf("<content>") - 9);
        int index = strPingZheng.LastIndexOf('-');
        if (index == -1)
        {
            lbeMsg.Text = "操作结果：处理不成功，信息：" + strRelMsg;
            // showMessage("将不能为单据保存凭证号！", true, "");
            return;
        }
        strPingZhengHao = strPingZheng.Substring(index + 1);
        //修改bill_ybbxmxb
        string[] arrCode = strCode.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries);
        string strInBillCode = "";
        for (int i = 0; i < arrCode.Length; i++)
        {
            strInBillCode += string.Format("'{0}',", arrCode[i]);
        }
        strInBillCode = strInBillCode.Substring(0, strInBillCode.Length - 1);
        if (strPingZhengHao.Equals(""))
        {
            return;
        }
        try
        {
            if (new Bll.newysgl.bill_ysmxbBll().SetPingZheng(strInBillCode, strPingZhengHao, strZhangTaoCode, strBillDate) > 0)
            {
                // showMessage("处理成功，同时报销单处理成功！", true, "");
                lbeMsg.Text = "操作结果：处理成功，凭证号为" + strPingZhengHao;
            }
            else
            {
                //showMessage("凭证保存成功，但报销单处理失败，可以自动将凭证号保存或联系管理员解决！", true, "");
                lbeMsg.Text = "操作结果：凭证保存成功，但报销单处理失败，可以自动将凭证号保存或联系管理员解决！";
            }
        }
        catch (Exception ex)
        {
            lbeMsg.Text = "保存失败，原因：" + ex.Message;
        }
        btnSave.Enabled = false;
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
            itemmodel.billDept = this.GridView.Rows[i].Cells[6].Text.Trim();
            itemmodel.dfje = ((TextBox)this.GridView.Rows[i].Cells[9].FindControl("txtdfje")).Text.Trim(); //this.GridView.Rows[i].Cells[8].Text.Trim();
            itemmodel.jfje = ((TextBox)this.GridView.Rows[i].Cells[8].FindControl("txtjfje")).Text.Trim(); //this.GridView.Rows[i].Cells[7].Text.Trim();
            TextBox txtMxkm = this.GridView.Rows[i].Cells[4].FindControl("txtMingXiKemu") as TextBox;
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
            itemmodel.fuzhuhesuan = this.GridView.Rows[i].Cells[10].Text.Trim().Equals("&nbsp;") ? "" : this.GridView.Rows[i].Cells[10].Text.Trim();
            itemmodel.PingZhengType = this.GridView.Rows[i].Cells[1].Text.Trim();
            itemmodel.bxzy = strZhaiYao;
            itemmodel.billUser = this.GridView.Rows[i].Cells[11].Text.Trim();
            TextBox txtOsDept = this.GridView.Rows[i].Cells[7].FindControl("txtForDept") as TextBox;
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

    /// <summary>
    /// 重新加载gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void reLoad_OnClick(object sender, EventArgs e)
    {
        this.GridView.DataSource = getItems();
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
            string strPingZhengType = e.Row.Cells[1].Text.Trim();
            //if (strPingZhengType.Equals("df"))
            //{
            //    //e.Row.Cells[7].Text = "0";
            //    ((TextBox)e.Row.Cells[7].FindControl("txtjfje")).Text = "0";
            //}
            //else
            //{
            //    //e.Row.Cells[8].Text = "0";
            //    ((TextBox)e.Row.Cells[8].FindControl("txtdfje")).Text = "0";
            //}

            //显示摘要到textbox
            string strZY = e.Row.Cells[2].Text.Trim();
            System.Web.UI.WebControls.TextBox textbox = e.Row.Cells[3].FindControl("txtZhaiYao") as System.Web.UI.WebControls.TextBox;
            if (textbox != null)
            {
                textbox.Text = strZY;
            }
            //合计行
            string strJie = ((TextBox)e.Row.Cells[8].FindControl("txtjfje")).Text.Trim(); //e.Row.Cells[7].Text.Trim();
            double dbJie = 0;
            if (double.TryParse(strJie, out dbJie))
            {
                dbjfjeZong += dbJie;
            }
            string strDai = ((TextBox)e.Row.Cells[9].FindControl("txtdfje")).Text.Trim(); //e.Row.Cells[8].Text.Trim();
            double dbDai = 0;
            if (double.TryParse(strDai, out dbDai))
            {
                dbdfjeZong += dbDai;
            }
            //辅助核算赋值
            string strFuzhu = e.Row.Cells[10].Text.Trim();
            if (!strFuzhu.Equals("") && !strFuzhu.Equals("&nbsp;"))
            {
                string[] arrFuzhu = strFuzhu.Split(new char[] { '[' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arrFuzhu.Length; i++)
                {
                    string strEveFuzhu = arrFuzhu[i].Substring(0, arrFuzhu[i].Length - 1);
                    //因为部门档案和人员档案跟其它辅助核算不同（直接去部门和人员基础表里去读取） 所以这里还要区别于下拉列表和其它的辅助项目  
                    if (new Bll.PingZheng.PingZheng_XMBLL().GetModelByName(strEveFuzhu) == null && !strEveFuzhu.Equals("部门档案") && !strEveFuzhu.Equals("人员档案"))
                    {
                        isHasFuZhuHasNoCount = true;
                        //添加到辅助核算维护页面
                        string strMsg = "";
                        Models.bill_pingzheng_xmModel pingzheng_FuZhuModel = new Models.bill_pingzheng_xmModel();
                        pingzheng_FuZhuModel.isDefault = "0";
                        pingzheng_FuZhuModel.parentCode = "0";
                        pingzheng_FuZhuModel.parentName = "";
                        pingzheng_FuZhuModel.Status = "1";
                        pingzheng_FuZhuModel.xmCode = new Dal.SysDictionary.DataDicDal().GetYbbxBillName("N", DateTime.Now.ToString("yyyyMMdd"), 1, 4);
                        pingzheng_FuZhuModel.xmName = strEveFuzhu;
                        if (new Bll.PingZheng.PingZheng_XMBLL().Add(pingzheng_FuZhuModel, out strMsg) <= 0)
                        {
                            showMessage("没有找到辅助项目：" + strEveFuzhu + "，同时系统自动添加失败，请手动到辅助项目下添加数据或联系管理员解决！", false, "");
                            return;
                        }
                        else
                        {
                            showMessage("没有找到辅助项目：" + strEveFuzhu + "系统已经自动添加到凭证项目中，请手动到凭证项目页面为该项目添加子节点以作说明！", false, "");
                            return;
                        }
                    }
                    if (!(strEveFuzhu.Equals("客商辅助核算") || strEveFuzhu.Equals("部门档案") || strEveFuzhu.Equals("人员档案")) && new Bll.PingZheng.PingZheng_XMBLL().GetChildByName(strEveFuzhu) == null)
                    {
                        showMessage("辅助项目：" + strEveFuzhu + "没有子节点，请手动到凭证项目页面为该项目添加子节点以作说明！", false, "");
                        return;
                    }
                    if (!(strEveFuzhu.Equals("客商辅助核算") || strEveFuzhu.Equals("部门档案") || strEveFuzhu.Equals("人员档案")))//客商辅助核算其实跟其他项目一样区别于部门、人员档案去读取核算项目基础数据部门设置的子项  但是由于它只有一种子项 不像现金流项目一样 有多个项目可以选择 所以就在这个地方不再绑定 直接去读取了
                    {
                        IList<Models.bill_pingzheng_xmModel> lstFuZhuHeSuan = new List<Models.bill_pingzheng_xmModel>();
                        lstFuZhuHeSuan = new Bll.PingZheng.PingZheng_XMBLL().GetChildsByName(strEveFuzhu);
                        if (lstFuZhuHeSuan != null && lstFuZhuHeSuan.Count > 0)
                        {
                            DropDownList ddlZhaiYao = e.Row.Cells[5].FindControl("ddlFuZhuHeSuan") as DropDownList;
                            ddlZhaiYao.DataSource = lstFuZhuHeSuan;
                            ddlZhaiYao.DataTextField = "xmName";
                            ddlZhaiYao.DataValueField = "parentName";
                            ddlZhaiYao.DataBind();
                        }
                    }
                }
            }
            //默认核算部门
            string strNowDeptName = e.Row.Cells[6].Text.Trim();
            strNowDeptName = strNowDeptName.Equals("&nbsp;") ? "" : strNowDeptName;
            e.Row.Cells[6].Text = strNowDeptName;
            if (!strNowDeptName.Equals(""))
            {
                strNowDeptName = strNowDeptName.Substring(strNowDeptName.IndexOf(']') + 1);
                //bool bofinddept = false;
                TextBox txtDept = e.Row.Cells[7].FindControl("txtForDept") as TextBox;
                //for (int i = 0; i < lstOsDept.Count; i++)
                //{
                //    if (lstOsDept[i].ToString().Equals(strNowDeptName))
                //    {
                //        if (txtDept != null)
                //        {
                //            txtDept.Text = strNowDeptName;
                //            bofinddept = true;
                //            break;
                //        }
                //    }
                //}
                string strsql = "select OSDeptName from bill_pingzheng_bumenduiying where Note1='" + strNowDeptName + "'";
                string strenddeptname = server.GetCellValue(strsql);
                if (!string.IsNullOrEmpty(strenddeptname))
                {
                    txtDept.Text = strenddeptname;
                }
                else
                {
                    txtDept.Text = strNowDeptName;
                }
            }
            if (IsPostBack)
            {
                //绑定对应系统核算部门（跟上面的默认核算部门顺序不能颠倒）
                string strdeptname = e.Row.Cells[12].Text.Trim();
                strdeptname = strdeptname.Equals("&nbsp;") ? "" : strdeptname;
                TextBox txtDept2 = e.Row.Cells[7].FindControl("txtForDept") as TextBox;
                if (txtDept2 != null)
                {
                    txtDept2.Text = strdeptname;
                }
            }
            //显示明细科目(费用科目对应贷方财务科目)
            string strMxKm = e.Row.Cells[13].Text.Trim();
            strMxKm = strMxKm.Equals("&nbsp;") ? "" : strMxKm;
            TextBox txtMxkm = e.Row.Cells[4].FindControl("txtMingXiKemu") as TextBox;
            if (txtMxkm != null)
            {
                txtMxkm.Text = strMxKm;
                //如果是税额 默认明细科目
                if (strPingZhengType.Equals("se"))
                {
                    txtMxkm.Text = server.GetCellValue("select '['+cwkmcode+']'+cwkmmc from bill_cwkm where cwkmcode='22210101'");
                }
            }
        }
        else if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.Footer)
        {
            e.Row.Cells[3].Text = "合计：";
            e.Row.Cells[3].Style.Add("text-align", "right");
            e.Row.Cells[8].Text = dbdfjeZong.ToString("N");
            e.Row.Cells[8].Style.Add("text-align", "right");
            e.Row.Cells[9].Text = dbjfjeZong.ToString("N");
            e.Row.Cells[9].Style.Add("text-align", "right");
            if (isHasFuZhuHasNoCount)
            {
                showMessage("系统已自动添加记录中未维护的辅助项目到辅助项目，请先为其添加下级项目作为项目说明，否则单据将保存失败！", true, "");
            }
        }
    }

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
    /// 添加一行
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_addrow_Click(object sender, EventArgs e)
    {
        List<itemModel> lstItems = getItems();
        if (lstItems.Count > 0)
        {
            lstItems.Add(new itemModel() { bxzy = lstItems[0].bxzy, billDept = lstItems[0].billDept, OsDept = lstItems[0].OsDept, jfje = "0", dfje = "0", billUser = lstItems[0].billUser });
        }
        this.GridView.DataSource = lstItems;
        this.GridView.DataBind();
    }
    /// <summary>
    /// 拆分一行
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_cfh_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<itemModel> lstItemModel = new System.Collections.Generic.List<itemModel>();

        int iCount = this.GridView.Rows.Count;
        List<itemModel> lstItems = getItems();
        for (int i = 0; i < iCount; i++)
        {
            //获取删除flg
            CheckBox ck = (CheckBox)this.GridView.Rows[i].Cells[0].FindControl("CheckBox1");
            //如果前台加了删除标记  直接忽略
            if (ck.Checked==false)
            {
                continue;
            }

            itemModel itemmodel = new itemModel();
            itemmodel.billDept = this.GridView.Rows[i].Cells[6].Text.Trim();
            itemmodel.dfje = ((TextBox)this.GridView.Rows[i].Cells[9].FindControl("txtdfje")).Text.Trim();// this.GridView.Rows[i].Cells[8].Text.Trim();
            itemmodel.jfje = ((TextBox)this.GridView.Rows[i].Cells[8].FindControl("txtjfje")).Text.Trim();
            TextBox txtMxkm = this.GridView.Rows[i].Cells[4].FindControl("txtMingXiKemu") as TextBox;
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
            itemmodel.PingZhengType = this.GridView.Rows[i].Cells[1].Text.Trim();
            itemmodel.bxzy = this.GridView.Rows[i].Cells[2].Text.Trim().Equals("&nbsp;") ? "" : this.GridView.Rows[i].Cells[2].Text.Trim();
            itemmodel.billUser = this.GridView.Rows[i].Cells[11].Text.Trim();
            TextBox txtOsDept = this.GridView.Rows[i].Cells[7].FindControl("txtForDept") as TextBox;
            if (txtOsDept != null)
            {
                itemmodel.OsDept = txtOsDept.Text.Trim();
            }
            lstItemModel.Add(itemmodel);
            if (lstItems.Count > 0)
            {
                lstItems.Add
                    (new itemModel() { bxzy = itemmodel.bxzy, fykmName = itemmodel.fykmName,fuzhuhesuan=itemmodel.fuzhuhesuan, billDept = itemmodel.billDept, OsDept = itemmodel.OsDept, jfje = "0", dfje = "0", billUser = itemmodel.billUser }
                    );
            }
        }

        //
        //if (lstItems.Count > 0)
        //{
        //    lstItems.Add
        //        (new itemModel() { bxzy = lstItems[0].bxzy, billDept = lstItems[0].billDept, OsDept = lstItems[0].OsDept, jfje = "0", dfje = "0", billUser = lstItems[0].billUser }
        //        );
        //}
        //if (lstItems.Count > 0)
        //{
        //    lstItems.Add
        //        (new itemModel() 
        //           { bxzy = lstItems[0].bxzy, billDept = lstItems[0].billDept, OsDept = lstItems[0].OsDept, jfje = "0", dfje = "0", billUser = lstItems[0].billUser }
        //        );
        //}
        this.GridView.DataSource = lstItems;
        this.GridView.DataBind();


    }
    ///// <summary>
    ///// 还款
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void btn_Return_Click(object sender, EventArgs e) {

    //}

    private List<itemModel> getItems()
    {
        System.Collections.Generic.List<itemModel> lstItemModel = new System.Collections.Generic.List<itemModel>();
        int iCount = this.GridView.Rows.Count;
        for (int i = 0; i < iCount; i++)
        {
            //获取删除flg
            string strflg = ((TextBox)this.GridView.Rows[i].Cells[14].FindControl("delFlg")).Text;
            //如果前台加了删除标记  直接忽略
            if (strflg.Equals("1"))
            {
                continue;
            }

            itemModel itemmodel = new itemModel();
            itemmodel.billDept = this.GridView.Rows[i].Cells[6].Text.Trim();
            itemmodel.dfje = ((TextBox)this.GridView.Rows[i].Cells[9].FindControl("txtdfje")).Text.Trim();// this.GridView.Rows[i].Cells[8].Text.Trim();
            itemmodel.jfje = ((TextBox)this.GridView.Rows[i].Cells[8].FindControl("txtjfje")).Text.Trim();
            TextBox txtMxkm = this.GridView.Rows[i].Cells[4].FindControl("txtMingXiKemu") as TextBox;
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
            itemmodel.PingZhengType = this.GridView.Rows[i].Cells[1].Text.Trim();
            itemmodel.bxzy = this.GridView.Rows[i].Cells[2].Text.Trim().Equals("&nbsp;") ? "" : this.GridView.Rows[i].Cells[2].Text.Trim();
            itemmodel.billUser = this.GridView.Rows[i].Cells[11].Text.Trim();
            TextBox txtOsDept = this.GridView.Rows[i].Cells[7].FindControl("txtForDept") as TextBox;
            if (txtOsDept != null)
            {
                itemmodel.OsDept = txtOsDept.Text.Trim();
            }
            lstItemModel.Add(itemmodel);
        }
        return lstItemModel;
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
        bindZhangTao();
        //绑定明细
        string[] arrCode = strCode.Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries);
        string strInBillCode = "";
        for (int i = 0; i < arrCode.Length; i++)
        {
            strInBillCode += string.Format("'{0}',", arrCode[i]);
        }
        strInBillCode = strInBillCode.Substring(0, strInBillCode.Length - 1);
        string strSql = "";
        string strflg = new Bll.ConfigBLL().GetValueByKey("pingzhengbygkorsy");
        if (strflg.Equals("1"))//使用部门
        {
            //像素公司的情况  使用nc系统   核算到使用部门  摘要用预算科目 select top 1 bxzy from bill_ybbxmxb where billcode=fykm.billcode
            strSql = @"					select 
	                    (select '['+deptCode+']'+deptName from bill_departments where deptCode=dept.deptcode) as billDept,
	                    (select billUser from bill_main where billCode=fykm.billCode) as billUser,
	                    fykm,dept.je as jfje,0 as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=fykm.fykm and  deptCode =dept.deptcode)) as fykmName,
	                    (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=fykm.fykm and deptCode =dept.deptcode)) as fuzhuhesuan,
	                    (select cwkmMc from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=fykm.fykm  and deptCode =dept.deptcode)) as bxzy,'jf' as PingZhengType,'' as OsDept 
	                    from bill_ybbxmxb_fykm fykm,bill_ybbxmxb_fykm_dept dept where fykm.mxGuid=dept.kmmxGuid  and dept.je<>0
		                    and fykm.billCode in ({0})
	                    union all
                        select	(select '['+deptCode+']'+deptName from bill_departments where deptCode=(select top 1 dept.deptcode from bill_ybbxmxb_fykm_dept dept where  fykm.mxGuid=dept.kmmxGuid)) as billDept,
	                    '' as billUser,
	                    fykm,fykm.se as jfje,0 as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode='22210101') as fykmName,
	                   (select FuZhuHeSuan from bill_cwkm where cwkmCode='22210101') as fuzhuhesuan,
	                    '税额' as bxzy,'se' as PingZhengType,'' as OsDept
	                    from bill_ybbxmxb_fykm fykm
	                    where fykm.billCode in ({0}) and fykm.se>0
	                    union all
                         select 
	                    '' as billDept,
	                    (select billUser from bill_main where billCode=fykm.billCode) as billUser,
	                    fykm,0 as jfje,(fykm.je+fykm.se) as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where   yskmCode=fykm.fykm and deptCode =(select top 1 deptcode from bill_ybbxmxb_fykm_dept where kmmxGuid=fykm.mxGuid and je<>0 ))) as fykmName,
	                    (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=fykm.fykm  and deptCode =(select top 1 deptcode from bill_ybbxmxb_fykm_dept where kmmxGuid=fykm.mxGuid and je<>0))) as fuzhuhesuan,
	                    (select cwkmMc from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=fykm.fykm  and  deptCode =(select top 1 deptcode from bill_ybbxmxb_fykm_dept fykmdept where fykmdept.kmmxGuid=fykm.mxGuid))) as bxzy,'df' as PingZhengType,'' as OsDept 
	                    from bill_ybbxmxb_fykm fykm
		                    where fykm.billCode in ({0}) 
                    ";
        }
        else
        {
            strSql = @"
                    select (select '['+deptCode+']'+deptName from bill_departments where deptCode=(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)) as billDept,
                    (select billUser from bill_main where billCode=bill_ybbxmxb_fykm.billCode) as billUser,
                    fykm,je as jfje,0 as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm  and  deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fykmName,
                    (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 jfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm and  deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fuzhuhesuan,
                    (select top 1 bxzy from bill_ybbxmxb where billCode=bill_ybbxmxb_fykm.billCode) as bxzy,'jf' as PingZhengType,'' as OsDept from bill_ybbxmxb_fykm where billCode in ({0})
                    union all
                    select (select '['+deptCode+']'+deptName from bill_departments where deptCode=(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode)) as billDept,
                    (select billUser from bill_main where billCode=bill_ybbxmxb_fykm.billCode) as billUser,
                    fykm,0 as jfje,je as dfje,(select '['+cwkmCode+']'+cwkmMc from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm   and  deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fykmName,
                    (select FuZhuHeSuan from bill_cwkm where cwkmCode=(select top 1 dfkmcode1 from bill_yskm_dept where yskmCode=bill_ybbxmxb_fykm.fykm  and deptCode =(select gkdept from bill_main where billCode=bill_ybbxmxb_fykm.billCode))) as fuzhuhesuan,
                    (select top 1 bxzy from bill_ybbxmxb where billCode=bill_ybbxmxb_fykm.billCode) as bxzy,'df' as PingZhengType,'' as OsDept  from bill_ybbxmxb_fykm where billCode in ({0})
                     ";
        }

        strSql = string.Format(strSql, strInBillCode);
        DataTable dtRel = server.GetDataTable(strSql, null);
        this.GridView.DataSource = dtRel;
        this.GridView.DataBind();
    }

    /// <summary>
    /// 将组建好的xml发送的nc数据平台
    /// </summary>
    /// <param name="strXML"></param>
    /// <param name="strReceiver"></param>
    /// <returns></returns>
    private string sendToNC(string strXML, string strReceiver)
    {
        string strToUrl = new Bll.ConfigBLL().GetValueByKey("ToNcURL");
        if (strToUrl.Equals(""))
        {
            throw new Exception("未配置nc系统url，请联系管理员解决！");
        }
        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strToUrl + strReceiver + "");

        byte[] xmlByte = System.Text.Encoding.Default.GetBytes(strXML);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentType = "text/XML";//SOAP
        request.ContentLength = xmlByte.Length;

        // 发送XML(需要考虑编码集问题)
        System.IO.Stream requestStream = request.GetRequestStream();
        requestStream.Write(xmlByte, 0, xmlByte.Length);

        //接收结果
        System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
        System.IO.Stream responseStream = response.GetResponseStream();
        byte[] result = new byte[20000];
        responseStream.Read(result, 0, result.Length);
        //返回结果转换字符串(需要考虑编码集问题)
        String resultStr = System.Text.Encoding.UTF8.GetString(result);

        requestStream.Close();
        responseStream.Close();
        return resultStr;
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
        this.ddlZhangTao.DataSource = server.GetDataTable("select dicCode+'|*|'+cjys as dicCode,dicName from bill_dataDic where dicType='10'", null);
        this.ddlZhangTao.DataTextField = "dicName";
        this.ddlZhangTao.DataValueField = "dicCode";
        this.ddlZhangTao.DataBind();
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

    /// <summary>
    /// 获取所有目标系统的部门
    /// </summary>
    /// <returns></returns>
    private string getAllOsDept()
    {
        DataSet ds = server.GetDataSet("select OSDeptName as deptName from bill_pingzheng_bumenduiying");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["deptName"]));
            arry.Append("',");
            lstOsDept.Add(dr["deptName"].ToString());
        }
        string script = "";
        if (arry.Length > 0)
        {
            script = arry.ToString().Substring(0, arry.Length - 1);
        }
        return script;
    }

    #endregion

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
    }

}
