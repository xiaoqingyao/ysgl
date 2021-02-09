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
using System.Collections.Generic;
using Bll.UserProperty;
using Models;
using Bll.Sepecial;

public partial class SaleBill_Flsz_NormalRebateStandard : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    RebatesStandardBLL spebll = new RebatesStandardBLL();
    string strDeptCode = "";//部门code
    string strEffectiveDateFrm = "";//有效时间frm
    string strEffectiveDateTo = "";//有效时间to
    string strTruckTypeCode = "";//车辆类型code
    string strSaleFeeTypeCode = "";//费用类型code
    SysManager smgr = new SysManager();
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
                initControl();
                DataTable dt = new DataTable();
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
       
    }
    /// <summary>
    /// 绑定
    /// </summary>
    public void dataBind()
    {

        T_RebatesStandard redsmodel = new T_RebatesStandard();
        if (this.txtbgtime.Text.Trim() != null && this.txtbgtime.Text.Trim() != "")
        {
            redsmodel.EffectiveDateFrm = this.txtbgtime.Text.ToString();
        }
        if (this.txtedtime.Text.Trim() != null && this.txtedtime.Text.Trim() != "")
        {
            redsmodel.EffectiveDateTo = this.txtedtime.Text.Trim();
        }
        if (this.lblDeptMsg.Text.Trim() != null && this.lblDeptMsg.Text.Trim() != "部门")
        {
            string strdeptcode = this.lblDeptMsg.Text.Trim();
            strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1).Trim();
            redsmodel.DeptCode = strdeptcode;
        }
        if (this.lblCarMsg.Text.Trim() != null && this.lblCarMsg.Text.Trim() != "车辆类型")
        {
            string strc = this.lblCarMsg.Text.Trim();
            strc = strc.Substring(1, strc.IndexOf("]") - 1).Trim();
            redsmodel.TruckTypeCode = strc;
        }
        if (lblFeeTypeMsg.Text.Trim() != null && this.lblFeeTypeMsg.Text.Trim() != "费用选择")
        {
            string strfeecode = this.lblFeeTypeMsg.Text.Trim();
            strfeecode = strfeecode.Substring(1, strfeecode.IndexOf("]") - 1).Trim();
            redsmodel.SaleFeeTypeCode = strfeecode;
        }

        DataTable dt = spebll.getalltable(redsmodel);
        IList<T_RebatesStandard> tebatesmodel = new List<T_RebatesStandard>();
        for (int s = 0; s < dt.Rows.Count; s++)
        {
            T_RebatesStandard remodel = new T_RebatesStandard();
            remodel.SaleCountFrm = int.Parse(dt.Rows[s]["SaleCountFrm"].ToString());
            remodel.NID = long.Parse(dt.Rows[s]["NID"].ToString());

            remodel.SaleCountTo = int.Parse(dt.Rows[s]["SaleCountTo"].ToString());
            remodel.ControlItemCode = dt.Rows[s]["feekz"].ToString();
            remodel.Fee = decimal.Parse(dt.Rows[s]["Fee"].ToString());
            remodel.Status = dt.Rows[s]["astatus"].ToString();
            remodel.Remark = dt.Rows[s]["Remark"].ToString();
            remodel.DeptCode = dt.Rows[s]["deptname"].ToString();
            remodel.SaleFeeTypeCode = dt.Rows[s]["feename"].ToString();
            remodel.TruckTypeCode = dt.Rows[s]["caname"].ToString();

            tebatesmodel.Add(remodel);
        }

        GridView1.DataSource = tebatesmodel;
        GridView1.DataBind();



    }
    /// <summary>
    /// 初始化控件
    /// </summary>
    private void initControl()
    {
        //车辆类型
        IList<string> typelist = new List<string>();
        IList<T_truckType> list = smgr.GetCarType();
        InserTruckTypeTree(treeViewTruckType.Nodes[0], list, typelist);
        //单位
        IList<Bill_Departments> listDept = smgr.GetAllDeptsedsel();
        int iDeptCount = listDept.Count;
        if (iDeptCount>0)
        {
            for (int i = 0; i < iDeptCount; i++)
            {
                this.ddlSaleDept.Items.Add(new ListItem("[" + listDept[i].DeptCode + "]" + listDept[i].DeptName, "[" + listDept[i].DeptCode + "]" + listDept[i].DeptName));
            }

            this.lblDeptMsg.Text = this.ddlSaleDept.SelectedItem.Text.Trim();
        }
        

        //项目

        string strsql = @" select '['+Code+']'+PName as EName from dbo.T_SaleProcess union
                select '['+Code+']'+CName as EName from dbo.T_ControlItem 
                ";

        DataTable dtobject = server.RunQueryCmdToTable(strsql);
        for (int i = 0; i < dtobject.Rows.Count; i++)
        {
            drowpobject.Items.Add(dtobject.Rows[i]["EName"].ToString().Trim());
        }
        drowpobject.Items.Insert(0, new ListItem("[Qcfp]期初分配"));

        //费用类别
        if (ddlSaleDept.Items.Count > 0)
        {
            string strDeptCodeVal = this.ddlSaleDept.SelectedValue;
            string strDept = strDeptCodeVal.Substring(1, strDeptCodeVal.IndexOf("]") - 1);
            if (strDept != "")
            {
                initFeeType(strDept);
            }
        }

    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        #region 获取并验证 有效日期from、有效日期to、卡车类型编号、部门编号、费用类型编号不能为空

        string stobj = this.drowpobject.SelectedItem.Text.Trim();
        string strselectItemCode = "";
        strselectItemCode = stobj.Substring(1, stobj.IndexOf("]") - 1);
        string strstnberf = txtnumberfrom.Text;//辆份起
        string strstnbert = txtnumberto.Text;//辆份止
        int liangshuFrm = 0;
        bool boLiangshuFrm = int.TryParse(strstnberf, out liangshuFrm);
        int liangshuto = 0;
        bool boLiangshuTo = int.TryParse(strstnbert, out liangshuto);
        //有效日期from
        if (this.txtbgtime.Text == null || this.txtbgtime.Text == "")
        {
            showMessage("有效日期起不能为空！", false, ""); return;
        }
        //有效日期to
        if (this.txtedtime.Text == null || this.txtedtime.Text == "")
        {
            showMessage("请填入有效的截止日期,该日期大于起始日期！", false, "");
            return;
        }
        strEffectiveDateFrm = this.txtbgtime.Text.Trim();
        strEffectiveDateTo = this.txtedtime.Text.Trim();
        DateTime dtbgtime = DateTime.Parse(strEffectiveDateFrm);
        DateTime dtedtime = DateTime.Parse(strEffectiveDateTo);
        int iCompareRel = DateTime.Compare(dtedtime, dtbgtime);
        if (iCompareRel > 0)
        {
            strEffectiveDateFrm = dtbgtime.ToString("yyyy-MM-dd");
            strEffectiveDateTo = dtedtime.ToString("yyyy-MM-dd");
        }
        else
        {
            showMessage("有效的截止日期必须大于起始日期！", false, "");
        }
        //卡车类型编号
        if (this.lblCarMsg.Text != "" && this.lblCarMsg.Text != "车辆类型" && this.lblCarMsg.Text != null)
        {
            string strcarcode = this.lblCarMsg.Text.Trim();
            strTruckTypeCode = strcarcode.Substring(1, strcarcode.IndexOf("]") - 1).Trim();
        }
        else
        {
            if (!strselectItemCode.Equals("Qcfp"))
            {
                showMessage("车辆类型不能为空！", false, "");
                return;
            }
        }

        if (!boLiangshuFrm)
        {
            if (!strselectItemCode.Equals("Qcfp"))
            {
                showMessage("辆数起输入不合法！", false, "");
                return;
            }
        }

        if (!boLiangshuTo)
        {
            if (!strselectItemCode.Equals("Qcfp"))
            {
                showMessage("辆数起输入不合法！", false, "");
                return;
            }
        }
        if (!strselectItemCode.Equals("Qcfp"))
        {
            if (liangshuFrm >= liangshuto)
            {
                showMessage("辆数起必须小于辆数止！", false, "");
                return;
            }

        }
      
        //部门编号
        if (this.lblDeptMsg.Text != "" && this.lblDeptMsg.Text != "部门" && this.lblDeptMsg.Text != null)
        {
            string strdepcode = this.lblDeptMsg.Text;
            strDeptCode = strdepcode.Substring(1, strdepcode.IndexOf("]") - 1);
        }
        else
        {
            showMessage("部门不能为空！", false, "");
            return;
        }
        //费用类型编号
        if (this.lblFeeTypeMsg.Text != "" && this.lblFeeTypeMsg.Text != "费用选择" && this.lblFeeTypeMsg.Text != null)
        {
            string strfeelcode = this.lblFeeTypeMsg.Text;
            strSaleFeeTypeCode = strfeelcode.Substring(1, strfeelcode.IndexOf("]") - 1);
        }
        else
        {
            showMessage("费用类别不能为空！", false, "");
            return;
        }
        #endregion


        T_RebatesStandard modelRebatesStandard = new T_RebatesStandard();
        modelRebatesStandard.AuditUserCode = "";
        modelRebatesStandard.ControlItemCode = strselectItemCode;
        modelRebatesStandard.DeptCode = strDeptCode;
        modelRebatesStandard.EffectiveDateFrm = strEffectiveDateFrm;
        modelRebatesStandard.EffectiveDateTo = strEffectiveDateTo;
        decimal de = 0;
        bool boFee = decimal.TryParse(this.txtmoney.Text.Trim(), out de);
        if (!boFee)
        {
            showMessage("输入金额不合法！", false, "");
            return;
        }
        modelRebatesStandard.Fee = de;
        modelRebatesStandard.Remark = this.txtbz.Text.Trim();

        modelRebatesStandard.SaleCountFrm = liangshuFrm;

        modelRebatesStandard.SaleCountTo = liangshuto;

        modelRebatesStandard.SaleFeeTypeCode = strSaleFeeTypeCode;
        modelRebatesStandard.Status = "1";
        modelRebatesStandard.TruckTypeCode = strTruckTypeCode;
        string strFlg = strselectItemCode.Substring(0, 2);
        if (strFlg.Equals("SP"))
        {
            modelRebatesStandard.Type = "1";
        }
        else if (strFlg.Equals("CF"))
        {
            modelRebatesStandard.Type = "2";
        }
        else
        {
            modelRebatesStandard.Type = "0";
        }
        string strMsg = "";
        try
        {
            int iRel = spebll.Insert(modelRebatesStandard, out strMsg);
            if (iRel > 0)
            {
                showMessage("添加成功！", false, "");
                dataBind();
                this.txtnumberfrom.Text = "";
                this.txtnumberto.Text = "";
                this.txtmoney.Text = "";
                this.txtbz.Text = "";
            }
            else
            {
                showMessage("添加失败，原因：" + strMsg, false, "");
            }
        }
        catch (Exception ex)
        {
            showMessage(ex.Message, false, "");
        }

    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Del_Click(object sender, EventArgs e)
    {

        List<T_RebatesStandard> cxblist = new List<T_RebatesStandard>();
        RebatesStandardBLL flbill = new RebatesStandardBLL();
        T_RebatesStandard tsandmode = new T_RebatesStandard();
        string diccode = "";
        int count = 0;
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            CheckBox check = GridView1.Rows[s].FindControl("CheckBox2") as CheckBox;

            string strstatus = this.GridView1.Rows[s].Cells[6].Text.Trim();
            string strnids = this.GridView1.Rows[s].Cells[1].Text;
            if (check.Checked == true && strnids != "" && strnids != "&nbsp;")
            {
                if (strstatus == "已批复" && strnids != "" && strnids != "&nbsp;")
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('已批复的记录不能删除！');", true);

                    return;
                }
                else
                {
                    diccode += "'" + this.GridView1.Rows[s].Cells[1].Text.Trim() + "',";
                    count += 1;
                }
            }
            if (check.Checked == false)
            {

                T_RebatesStandard model = new T_RebatesStandard();
                model.NID = long.Parse(GridView1.Rows[s].Cells[1].Text);
                model.SaleCountFrm = int.Parse(GetGwStr(GridView1.Rows[s].Cells[2].Text));
                model.SaleCountTo = int.Parse(GetGwStr(GridView1.Rows[s].Cells[3].Text));
                model.ControlItemCode = GetGwStr(GridView1.Rows[s].Cells[4].Text);
                model.Fee = decimal.Parse(GetGwStr(GridView1.Rows[s].Cells[5].Text));
                model.Status = GetGwStr(GridView1.Rows[s].Cells[6].Text);
                model.DeptCode = GetGwStr(GridView1.Rows[s].Cells[7].Text);
                model.Type = GetGwStr(GridView1.Rows[s].Cells[8].Text);
                model.SaleFeeTypeCode = GetGwStr(GridView1.Rows[s].Cells[9].Text);
                model.Remark = GetGwStr(GridView1.Rows[s].Cells[10].Text);
                cxblist.Add(model);
            }
        }
        if (count != 0)
        {

            diccode = diccode.Substring(0, diccode.Length - 1);

            System.Collections.Generic.List<string> list = new List<string>();

            list.Add("delete from T_RebatesStandard where NID in (" + diccode + ")");

            if (server.ExecuteNonQuerysArray(list) > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
            }
        }
        GridView1.DataSource = cxblist;
        GridView1.DataBind();

    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btn_Save_Click(object sender, EventArgs e)
    //{
    //    string msg = "";
    //    IList<T_RebatesStandard> Ilistmode = getmodel();
    //    if (Ilistmode != null)
    //    {
    //        if (Ilistmode.Count > 0)
    //        {
    //            int iRel = spebll.Insert(Ilistmode, out msg);
    //            if (iRel > 0)
    //            {
    //                //向note表中加记录
    //                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
    //            }
    //            else
    //            {
    //                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败,原因：" + msg + "');", true);
    //            }
    //        }

    //    }
    //    else
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请添加相应的辆数、项目、额度！');", true);

    //    }
    //}

    /// <summary>
    /// 获取model值
    /// </summary>
    /// <param name="isp"></param>
    /// <returns></returns>
    //public IList<T_RebatesStandard> getmodel()
    //{
    //    IList<T_RebatesStandard> list = new List<T_RebatesStandard>();
    //    T_RebatesStandard mode = new T_RebatesStandard();
    //    string strRemark = this.txtbz.Text.Trim();


    //    if (this.txtbgtime.Text != "" && this.txtbgtime.Text != null)
    //    {
    //        strEffectiveDateFrm = this.txtbgtime.Text;

    //    }
    //    else
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('有效日期起不能为空')", true);
    //        return null;
    //    }



    //    if (this.txtedtime.Text != "" && this.txtedtime.Text != null)
    //    {
    //        DateTime dtbgtime = DateTime.Parse(this.txtbgtime.Text.ToString().Trim());
    //        DateTime dtedtime = DateTime.Parse(this.txtedtime.Text.ToString().Trim());
    //        int iCompareRel = DateTime.Compare(dtedtime, dtbgtime);
    //        if (iCompareRel > 0)
    //        {
    //            strEffectiveDateTo = this.txtedtime.Text;
    //        }
    //        else
    //        {
    //            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('有效的截止日期必须大于起始日期')", true);
    //            return null;
    //        }
    //    }
    //    else
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请填入有效的截止日期 该日期大于起始日期')", true);
    //        return null;
    //    }
    //    if (this.lblCarMsg.Text != "" && this.lblCarMsg.Text != "车辆类型" && this.lblCarMsg.Text != null)
    //    {
    //        string strcarcode = this.lblCarMsg.Text.Trim();
    //        strTruckTypeCode = strcarcode.Substring(1, strcarcode.IndexOf("]") - 1).Trim();
    //    }
    //    else
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('车辆类型不能为空')", true);
    //        return null;
    //    }
    //    if (this.lblDeptMsg.Text != "" && this.lblDeptMsg.Text != "部门" && this.lblDeptMsg.Text != null)
    //    {
    //        string strdepcode = this.lblDeptMsg.Text;
    //        strDeptCode = strdepcode.Substring(1, strdepcode.IndexOf("]") - 1);
    //    }
    //    else
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门不能为空！');", true);
    //        return null;
    //    }


    //    if (this.lblFeeTypeMsg.Text != "" && this.lblFeeTypeMsg.Text != "费用选择" && this.lblFeeTypeMsg.Text != null)
    //    {
    //        string strfeelcode = this.lblFeeTypeMsg.Text;
    //        strSaleFeeTypeCode = strfeelcode.Substring(1, strfeelcode.IndexOf("]") - 1);
    //    }
    //    else
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('费用类别不能为空')", true);
    //        return null;
    //    }

    //    int SaleCount = this.GridView1.Rows.Count;
    //    for (int i = 0; i < SaleCount; i++)
    //    {

    //        string stobj = this.GridView1.Rows[i].Cells[4].Text.Trim();
    //        string strselectItemCode = "";
    //        string stnid = "";
    //        if (stobj == "期初分配")
    //        {
    //            strselectItemCode = stobj;
    //        }
    //        else
    //        {
    //            strselectItemCode = stobj.Substring(1, stobj.IndexOf("]") - 1);
    //        }
    //        mode = new T_RebatesStandard();
    //        mode.DeptCode = strDeptCode;
    //        mode.EffectiveDateFrm = strEffectiveDateFrm;
    //        mode.EffectiveDateTo = strEffectiveDateTo;
    //        mode.TruckTypeCode = strTruckTypeCode;
    //        mode.SaleFeeTypeCode = strSaleFeeTypeCode;

    //        mode.Remark = strRemark;
    //        mode.SaleCountFrm = int.Parse(this.GridView1.Rows[i].Cells[2].Text.Trim());
    //        mode.SaleCountTo = int.Parse(this.GridView1.Rows[i].Cells[3].Text.Trim());
    //        mode.Fee = decimal.Parse(this.GridView1.Rows[i].Cells[5].Text.Trim());
    //        mode.Remark = GetGwStr(this.GridView1.Rows[i].Cells[10].Text.Trim());



    //        if (GetGwStr(this.GridView1.Rows[i].Cells[6].Text.Trim()) == "已批复")
    //        {
    //            continue;
    //        }
    //        else
    //        {
    //            mode.Status = "1";
    //        }
    //        string str = GetGwStr(this.GridView1.Rows[i].Cells[4].Text.Trim());

    //        string stri = str.Substring(1, 2);
    //        if (stri == "CF")
    //        {

    //            mode.ControlItemCode = str.Substring(1, str.IndexOf("]") - 1);
    //            mode.Type = "2";
    //        }
    //        else if (stri == "SP")
    //        {
    //            mode.ControlItemCode = str.Substring(1, str.IndexOf("]") - 1);
    //            mode.Type = "1";
    //        }
    //        else
    //        {
    //            mode.ControlItemCode = "期初分配";
    //            mode.Type = "0";

    //        }

    //        mode.Status = "1";
    //        list.Add(mode);

    //    }
    //    return list;


    //}

    public void clear()
    {
        IList<T_RebatesStandard> tplist = new List<T_RebatesStandard>();
        GridView1.DataSource = tplist;
        GridView1.DataBind();
    }

    /// <summary>
    /// 卡车类型选择变化
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeViewTruckType_SelectedNodeChanged(object sender, EventArgs e)
    {
        this.lblCarMsg.Text = this.treeViewTruckType.SelectedNode.Text;
        dataBind();
    }
    /// <summary>
    /// 单位选择变化
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSaleDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strDeptMsg = this.ddlSaleDept.SelectedValue;
        this.lblDeptMsg.Text = strDeptMsg;
        dataBind();
        //根据单位的选择 重新绑定费用类别
        string strDeptCode = strDeptMsg.Substring(1, strDeptMsg.IndexOf("]") - 1);
        this.treeViewFeeType.Nodes[0].ChildNodes.Clear();
        initFeeType(strDeptCode);
        this.treeViewFeeType.ExpandAll();
    }
    /// <summary>
    /// 费用类型变化
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeViewFeeType_SelectedNodeChanged(object sender, EventArgs e)
    {
        //判断下必须先选择了单位 选择费用类型才有意义
        //if (this.lblDeptMsg.Text.Trim().Equals("部门"))
        //{
        //    showMessage("请先选择部门", false, "");
        //    return;
        //}
        if (this.treeViewFeeType.SelectedNode.ToolTip.Equals("1"))
        {
            this.lblFeeTypeMsg.Text = this.treeViewFeeType.SelectedNode.Text;
        }
        else
        {
            showMessage("请选择具体费用类型！", false, "");
            TreeNode tn = this.treeViewFeeType.SelectedNode;
            if (tn != null)
            {
                tn.Selected = false;
            }
        }
        dataBind();
    }


    //判断是不是空格
    protected string GetGwStr(string str)
    {
        if (str == "&nbsp;")
        {
            return "";
        }
        else
        {
            return str;
        }
    }

    #region 私有方法
    /// <summary>
    /// 车辆类型加载
    /// </summary>
    /// <param name="tnd"></param>
    /// <param name="list"></param>
    /// <param name="kmlist"></param>
    private void InserTruckTypeTree(TreeNode tnd, IList<T_truckType> list, IList<string> kmlist)
    {
        var childs = from child in list
                     where child.parentCode.ToString().Trim() == tnd.Value.Trim()
                     select child;
        int sl = childs.Count();
        if (sl > 0)
        {
            foreach (T_truckType dept in childs)
            {
                TreeNode tnc = new TreeNode();

                tnc.Text = "[" + dept.typeCode.ToString() + "]" + dept.typeName;
                tnc.Value = dept.typeCode.ToString();
                tnc.ShowCheckBox = false;

                if (kmlist.Count > 0)
                {
                    int cont = (from temp in kmlist
                                where temp == dept.typeCode.ToString()
                                select temp).Count();
                    if (cont > 0)
                    {
                        tnc.Checked = true;
                    }
                }
                InserTruckTypeTree(tnc, list, kmlist);
                tnd.ChildNodes.Add(tnc);
            }
        }
    }
    /// <summary>
    /// 部门加载
    /// </summary>
    /// <param name="tnd"></param>
    /// <param name="list"></param>
    /// <param name="deplist"></param>
    private void InserDeptTree(TreeNode tnd, IList<Bill_Departments> list, IList<string> deplist)
    {
        var childs = from child in list
                     where child.SjDeptCode == tnd.Value
                     select child;
        int sl = childs.Count();
        if (sl > 0)
        {
            foreach (Bill_Departments dept in childs)
            {
                TreeNode tnc = new TreeNode();

                tnc.Text = "[" + dept.DeptCode + "]" + dept.DeptName;
                tnc.Value = dept.DeptCode;
                //tnc.ShowCheckBox = true;
                if (deplist.Count > 0)
                {
                    int cont = (from temp in deplist
                                where temp == dept.DeptCode
                                select temp).Count();
                    if (cont > 0)
                    {
                        tnc.Checked = true;
                    }
                }
                InserDeptTree(tnc, list, deplist);
                tnd.ChildNodes.Add(tnc);
            }
        }

    }
    /// <summary>
    /// 费用类别加载
    /// </summary>
    /// <param name="tnd"></param>
    /// <param name="list"></param>
    /// <param name="kmlist"></param>
    private void InserFeeTypeTree(TreeNode tnd, IList<Bill_Yskm> list, IList<string> kmlist)
    {
        var childs = from child in list
                     where child.YskmCode.Length == tnd.Value.Length + 2 && child.YskmCode.Substring(0, tnd.Value.Length) == tnd.Value
                     select child;
        int sl = childs.Count();
        if (sl > 0)
        {
            foreach (Bill_Yskm yskm in childs)
            {
                TreeNode tnc = new TreeNode();

                tnc.Text = "[" + yskm.YskmCode + "]" + yskm.YskmMc;
                tnc.Value = yskm.YskmCode;
                tnc.ToolTip = yskm.IsEnd;
                //末级选中
                if (kmlist.Count > 0)
                {
                    int cont = (from temp in kmlist
                                where temp == yskm.YskmCode
                                select temp).Count();
                    if (cont > 0)
                    {
                        tnc.Checked = true;
                    }
                }
                InserFeeTypeTree(tnc, list, kmlist);
                tnd.ChildNodes.Add(tnc);
            }
        }
        else
        {
            //tnd.ShowCheckBox = true;
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

    /// <summary>
    /// 通过部门编号未选择费用的控件赋值
    /// </summary>
    /// <param name="strDeptCode"></param>
    private void initFeeType(string strDeptCode)
    {
        IList<string> kmlist = new List<string>();
        IList<Bill_Yskm> list1 = smgr.GetYskmByDep(strDeptCode);
        SysManager sysMgr = new SysManager();
        sysMgr.SetEndYsbm(list1);
        InserFeeTypeTree(treeViewFeeType.Nodes[0], list1, kmlist);
        this.treeViewFeeType.ExpandAll();
    }
    #endregion
}
