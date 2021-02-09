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
using Models;
using System.Collections.Generic;
using Bll.UserProperty;
using Bll.TruckType;
using System.Text;

public partial class webBill_truckType_TruckTypeCorrespondDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    T_TruckTypeCorrespondBLL trckbll = new T_TruckTypeCorrespondBLL();
    SysManager smgr = new SysManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
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
        ClientScript.RegisterArrayDeclaration("availableTags", GetcxhAll());
    }


    public void datebind()
    {
        T_TruckTypeCorrespond model = new T_TruckTypeCorrespond();
        string strCarMsg = this.lblCarMsg.Text.Trim();
        if (strCarMsg != "车辆类型" && strCarMsg != "显示未对应记录")
        {
            string strc = this.lblCarMsg.Text.Trim();
            strc = strc.Substring(1, strc.IndexOf("]") - 1).ToString().Trim();
            model.truckTypeCode = strc;
           
          

        }
        else if (strCarMsg.Equals("显示未对应记录"))
        {
            model.IsShowNoCorrespond = "1";
           
        }
        DataTable dt = trckbll.getalltable(model);
        GridView1.DataSource = dt;
        GridView1.DataBind();


    }


    /// <summary>
    /// 初始化控件
    /// </summary>
    private void initControl()
    {
        //内部编号
        IList<string> typelist = new List<string>();
        IList<T_truckType> list = smgr.GetCarType();

        InserTruckTypeTree(treeViewTruckType.Nodes[0], list, typelist);
        //额外添加节点  显示 所有未对应的节点
        TreeNode tnclast = new TreeNode();
        tnclast.Text = "显示未对应记录";
        tnclast.Value = "hasNoCorrespond";
        tnclast.ShowCheckBox = false;
        tnclast.ToolTip = "显示未对应记录";
        treeViewTruckType.Nodes[0].ChildNodes.Add(tnclast);
    }
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
                tnc.ToolTip = dept.IsLastNode;
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
    /// 获取车型号
    /// </summary>
    /// <returns></returns>
    private string GetcxhAll()
    {
        DataSet ds = server.GetDataSet("select distinct cxh from dbo.V_TruckMsg");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["cxh"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    /// <summary>
    /// 当选择内部编号发生改变时
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeViewTruckType_SelectedNodeChanged(object sender, EventArgs e)
    {
        this.hidfieldcartype.Value = this.treeViewTruckType.SelectedNode.ToolTip.ToString();
        this.lblCarMsg.Text = this.treeViewTruckType.SelectedNode.Text;
        datebind();
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string msg = "";
        T_TruckTypeCorrespond model = new T_TruckTypeCorrespond();
        if (txtcartype.Text.Trim() != "")
        {
            model.factTruckType = txtcartype.Text.Trim();
        }
        else
        {
            showMessage("内部编号不能为空！", false, "");
            return;
        }
        if (lblCarMsg.Text.Trim() != "车辆类型" && lblCarMsg.Text.Trim() != "显示未对应记录")
        {
            string strtrcarcode = lblCarMsg.Text.Trim();
            strtrcarcode = strtrcarcode.Substring(1, strtrcarcode.IndexOf("]") - 1).ToString().Trim();
            model.truckTypeCode = strtrcarcode;
        }
        else
        {
            showMessage("车辆类型不能为空！", false, "");
            return;
        }
        string strnbcxh = txtcartype.Text.Trim();
        IList<T_TruckTypeCorrespond> modelTruckTypeCorrespond = trckbll.GetModelListByFactTruckCode(strnbcxh);
        if (modelTruckTypeCorrespond != null && modelTruckTypeCorrespond.Count > 0)
        {
            if (modelTruckTypeCorrespond.Count == 1 && modelTruckTypeCorrespond[0].truckTypeCode != "")
            {
                showMessage("该车型号已经做过对应!", false, "");
                return;
            }
            else
            {
                if (trckbll.DeleteByNbcxh(strnbcxh) < 0)
                {
                    showMessage("在删除未对应的记录时失败！", false, "");
                    return;
                }
            }
        }
        if (model.truckTypeCode != null && model.factTruckType != null && model.factTruckType != "" && model.truckTypeCode != "")
        {
            if (this.hidfieldcartype.Value == "1")
            {
                int row = trckbll.Add(model, out msg);
                if (row > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加成功！');", true);
                    this.txtcartype.Text = "";
                    datebind();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败！');", true);
                    this.txtcartype.Text = "";
                    datebind();
                }
            }
            else
            {
                showMessage("请选择末级编号进行添加！", false, "");
                return;
            }

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败！');", true);
            this.txtcartype.Text = "";
            datebind();
        }



    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Del_Click(object sender, EventArgs e)
    {

        List<T_TruckTypeCorrespond> cxblist = new List<T_TruckTypeCorrespond>();

        T_TruckTypeCorrespond tsandmode = new T_TruckTypeCorrespond();
        string diccode = "";
        int count = 0;
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            CheckBox check = GridView1.Rows[s].FindControl("CheckBox2") as CheckBox;

            string strnids = this.GridView1.Rows[s].Cells[1].Text;
            if (check.Checked == true && strnids != "" && strnids != "&nbsp;")
            {

                diccode += "'" + this.GridView1.Rows[s].Cells[1].Text.Trim() + "',";
                count += 1;

            }
            if (check.Checked == false)
            {

                T_TruckTypeCorrespond model = new T_TruckTypeCorrespond();
                model.list_id = long.Parse(GridView1.Rows[s].Cells[1].Text);
                model.truckTypeCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);
                model.factTruckType = GetGwStr(GridView1.Rows[s].Cells[3].Text);

                cxblist.Add(model);
            }
        }
        if (count != 0)
        {

            diccode = diccode.Substring(0, diccode.Length - 1);

            System.Collections.Generic.List<string> list = new List<string>();

            list.Add("delete from T_TruckTypeCorrespond where list_id in (" + diccode + ")");

            if (server.ExecuteNonQuerysArray(list) > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
                this.txtcartype.Text = "";
            }
        }
     
        datebind();
    }


    /// <summary>
    ///    //判断是不是空格
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
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

}
