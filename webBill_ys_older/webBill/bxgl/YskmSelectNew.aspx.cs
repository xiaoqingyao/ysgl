using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;
using Models;
using System.Text;
using Bll;
using Dal.Bills;

public partial class webBill_bxgl_YskmSelectNew : System.Web.UI.Page
{
    static bool boCheckFlg = true;
    private string strdydj = "02";//对应单据编号默认02 
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        //    return;
        //}
        //else
        //{
        Response.Cache.SetSlidingExpiration(true);
        Response.Cache.SetNoStore();
        if (!IsPostBack)
        {
            //单据对应编号
            object objdydj = Request["dydj"];
            if (objdydj != null && objdydj.ToString() != "")
            {
                strdydj = objdydj.ToString();
            }
            //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + strdydj + "')", true);
            IList<string> kmlist = new List<string>();
            string yskmcode = Request.Params["kmcode"];
            string isgk = Request.Params["isGk"];

            if ((isgk == "true" && Request["dydj"] != "lyd") || Request["flag"] == "s")
            {
                //单选
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type=\"text/javascript\">");
                sb.Append("$(\"input[type='checkbox']\").click(function() {");
                sb.Append("$(\"input[type='checkbox']\").each(function() { $(this).attr(\"checked\", false); });");
                sb.Append("$(this).attr(\"checked\", true); });");
                sb.Append("</script>");
                ClientScript.RegisterStartupScript(this.GetType(), "", sb.ToString());
            }

            if (!string.IsNullOrEmpty(yskmcode))
            {
                string[] arry = yskmcode.Split(',');
                foreach (string temp in arry)
                {
                    kmlist.Add(temp.Split(']')[0].Trim('['));
                }
            }
            string dept = Request.Params["deptcode"];
            if (!string.IsNullOrEmpty(dept))
            {
                dept = dept.Split(']')[0].Trim('[');
            }


            SysManager smgr = new SysManager();
            if (isgk == "true")
            {
                IList<Bill_Yskm> list = new List<Bill_Yskm>();
                list = (!string.IsNullOrEmpty(dept)) ? smgr.GetGkYskmByDep(dept, strdydj) : smgr.GetGkYskmAll();
                InserTree(TreeView1.Nodes[0], list, kmlist);
            }
            else
            {
                IList<Bill_Yskm> list1 = new List<Bill_Yskm>();
                list1 = smgr.GetYskmByDep(dept, strdydj);
                InserTree(TreeView1.Nodes[0], list1, kmlist);
            }
            //}
        }
    }


    private void InserTree(TreeNode tnd, IList<Bill_Yskm> list, IList<string> kmlist)
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
                tnc.SelectAction = TreeNodeSelectAction.None;
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
                InserTree(tnc, list, kmlist);
                tnd.ChildNodes.Add(tnc);
            }
        }
        else
        {
            tnd.ShowCheckBox = true;
        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        IList<string> list = new List<string>();
        ForeachTree(TreeView1.Nodes[0], ref list);
        string billDate = Request.Params["billDate"];
        YsglDal ysDal = new YsglDal();
        DateTime dt = DateTime.Now;
        string strny = "";

        if (list.Count > 0)
        {
            //StringBuilder sb = new StringBuilder();
            IList<JsonRet> retlist = new List<JsonRet>();

            foreach (string km in list)
            {
                JsonRet temp = new JsonRet();
                temp.Yscode = km;

                YsManager ysmgr = new YsManager();

                string deptCode = Request.Params["deptcode"];
                deptCode = string.IsNullOrEmpty(deptCode) ? "" : deptCode.Split(']')[0].Trim('[');
                string kmCode = string.IsNullOrEmpty(km) ? "" : km.Split(']')[0].Trim('[');
                string gcbh = ysmgr.GetYsgcCode(DateTime.Parse(billDate));
                if (!string.IsNullOrEmpty(Request["dydj"]))
                {
                    strdydj = Request["dydj"];
                }
                decimal ysje = ysje = ysmgr.GetYueYs(gcbh, deptCode, kmCode);//预算金额

                string tfsq = "";
                if (!string.IsNullOrEmpty(Request["tfsq"]))
                {
                    tfsq = Request["tfsq"].ToString();
                }

                decimal hfje = 0;
                if (!string.IsNullOrEmpty(tfsq))
                {
                    hfje = ysmgr.GetYueHf_tf(gcbh, deptCode, kmCode, strdydj);//花费金额    
                }
                else
                {
                    hfje = ysmgr.GetYueHf(gcbh, deptCode, kmCode, strdydj);//花费金额    
                }

                ////是否启用销售提成模块
                //bool hasSaleRebate = new ConfigBLL().GetValueByKey("HasSaleRebate").Equals("1");             
                decimal syje = ysje - hfje;
                //decimal tcje = 0;
                //if (hasSaleRebate)
                //{
                //    tcje = ysmgr.getEffectiveSaleRebateAmount(deptCode, kmCode);//提成金额
                //}
                decimal tcje = 0;
                decimal kyje = syje + tcje;
                //sb.Append(" 预算:"+ysje.ToString()+" 剩余:"+syje.ToString());

                //sb.Append("|");
                temp.Ysje = ysje;
                temp.Syje = syje;
                temp.Tcje = tcje;
                temp.Kyje = kyje;
                //是否项目核算
                temp.XiangMuHeSuan = "否";//new Dal.SysDictionary.YskmDal().GetYskmByCode(kmCode).XmHs.Equals("1") ? "是" : "否";

                //部门
                temp.dept = new DepartmentBLL().GetShowNameByCode(deptCode);
                //预算使用比例
                string strbl = ysje == 0 ? "0%" : Math.Round(((ysje - syje) / ysje * 100), 2).ToString() + "%";
                temp.sybl = strbl;
                retlist.Add(temp);
            }
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            string script = jserializer.Serialize(retlist);
            //sb.Remove(sb.Length - 1, 1);
            //string script = sb.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>parent.insertKm('" + script + "'); self.close();</script>");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择科目!');", true);
        }
    }

    private void ForeachTree(TreeNode tnd, ref IList<string> list)
    {
        foreach (TreeNode tnc in tnd.ChildNodes)
        {
            if (tnc.Checked)
            {
                list.Add(tnc.Text);
            }
            ForeachTree(tnc, ref list);
        }
    }


    class JsonRet
    {
        public string Yscode { get; set; }
        public decimal Ysje { get; set; }
        public decimal Syje { get; set; }
        /// <summary>
        /// 销售提成金额  如果T_config没有配置系统使用该功能，则该字段无用
        /// </summary>
        public decimal Tcje { get; set; }
        /// <summary>
        /// 可用金额 预算剩余金额+费用提成金额  如果T_config没有配置系统使用该功能，则该字段无用
        /// </summary>
        public decimal Kyje { get; set; }
        /// <summary>
        /// 项目核算
        /// </summary>
        public string XiangMuHeSuan { get; set; }
        public string dept { get; set; }
        /// <summary>
        /// 预算使用比例
        /// </summary>
        public string sybl { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string je = "0";
        /// <summary>
        /// 税额
        /// </summary>
        public string se = "0";

        /// <summary>
        /// 报销摘要
        /// </summary>
        public string bxzy = "";
        /// <summary>
        /// 报销说明
        /// </summary>
        public string bxsm = "";
    }
    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        CheckOrUnCheckAllNodes(this.TreeView1.Nodes[0], boCheckFlg);
        boCheckFlg = !boCheckFlg;
        this.TreeView1.Nodes[0].Selected = !this.TreeView1.Nodes[0].Selected;
    }
    /// <summary>
    /// 选中或者不选中所有的node
    /// </summary>
    /// <param name="tnd">开始操作的node节点</param>
    /// <param name="boCheck">状态</param>
    private void CheckOrUnCheckAllNodes(TreeNode tnd, bool boCheck)
    {
        foreach (TreeNode node in tnd.ChildNodes)
        {
            if (node.ShowCheckBox == true)
            {
                node.Checked = boCheck;
            }
            CheckOrUnCheckAllNodes(node, boCheck);
        }
    }

}
