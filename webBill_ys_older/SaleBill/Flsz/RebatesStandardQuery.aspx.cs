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
using Bll.Sepecial;
using Bll.UserProperty;
using System.Collections.Generic;
using Models;

public partial class SaleBill_Flsz_RebatesStandardQuery : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    RebatesStandardBLL spebll = new RebatesStandardBLL();
    SysManager smgr = new SysManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        { }
        if (!IsPostBack)
        {
            initControl();


            DataTable dt = new DataTable();
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

    }
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
    /// 
    /// </summary>
    public void databind() 
    {

        T_RebatesStandard redsmodel = new T_RebatesStandard();
        if (this.txtbgtime.Text.Trim()!=null&&this.txtbgtime.Text.Trim()!="")
        {
            redsmodel.EffectiveDateFrm = this.txtbgtime.Text.Trim();
            
        }
        if (this.txtedtime.Text.Trim()!=null&&this.txtedtime.Text.Trim()!="")
        {
            redsmodel.EffectiveDateTo = this.txtedtime.Text.Trim();
        }
        if (this.lblDeptMsg.Text.Trim() != null && this.lblDeptMsg.Text.Trim() != "部门")
        {
            string strdeptcode=this.lblDeptMsg.Text.Trim();
            strdeptcode=strdeptcode.Substring(1,strdeptcode.IndexOf("]")-1).Trim();

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
            strfeecode = strfeecode.Substring(1,strfeecode.IndexOf("]")-1).Trim();
            redsmodel.SaleFeeTypeCode = strfeecode;
        }
        redsmodel.Status = "2";
        DataTable dt = spebll.getalltable(redsmodel);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    
    }
    private void initFeeType(string strDeptCode)
    {
        IList<string> kmlist = new List<string>();
        IList<Bill_Yskm> list1 = smgr.GetYskmByDep(strDeptCode);
        SysManager sysMgr = new SysManager();
        sysMgr.SetEndYsbm(list1);
        InserFeeTypeTree(treeViewFeeType.Nodes[0], list1, kmlist);
        this.treeViewFeeType.ExpandAll();
    }

    /// <summary>
    /// 卡车类型选择变化
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeViewTruckType_SelectedNodeChanged(object sender, EventArgs e)
    {
        this.lblCarMsg.Text = this.treeViewTruckType.SelectedNode.Text;
        if (this.txtbgtime.Text.Trim() == null || this.txtbgtime.Text.Trim() == "")
        {
            showMessage("请选择有效日期起！", false, "");

        }
       
        if (this.txtedtime.Text.Trim() == null || this.txtedtime.Text.Trim() == "")
        {
            showMessage("请选择有效日期止！", false, "");

        }
        else
        {
            databind();

        }
     
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
        databind();
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


        //if (this.treeViewFeeType.SelectedNode.ToolTip.Equals("1"))
        //{
            this.lblFeeTypeMsg.Text = this.treeViewFeeType.SelectedNode.Text;
        //}
        //else
        //{
        //    showMessage("请选择具体费用类型！", false, "");
        //    TreeNode tn = this.treeViewFeeType.SelectedNode;
        //    if (tn != null)
        //    {
        //        tn.Selected = false;
        //    }
        //}
        databind();
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

  
    #endregion


    protected void btn_select_Click(object sender, EventArgs e)
    {
      
        databind();
    }
}
