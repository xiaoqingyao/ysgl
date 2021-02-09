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

public partial class webBill_tjbb_dz_SelectYskm : System.Web.UI.Page
{
    static bool boCheckFlg = true;
    private string strdydj = "";//对应单据编号默认02 
    private string forchanchu = "";//是否默认价值投入产出表
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {

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
            object objforchanchu = Request["forchanchu"];
            if (objforchanchu != null && objforchanchu.ToString() != "")
            {
                forchanchu = objforchanchu.ToString();
            }

            SysManager smgr = new SysManager();
            IList<string> kmlist = new List<string>();
            if (forchanchu.Equals("1"))//如果对应投入产出报表 则自动选中上次选择的内容
            {
                string strkm = server.GetCellValue("select top 1 yskm from bill_tcsql");
                if (!string.IsNullOrEmpty(strkm))
                {
                    kmlist = strkm.Split(new string[] { "," }, StringSplitOptions.None);
                }
            }
            IList<Bill_Yskm> list1 = new List<Bill_Yskm>();
            list1 = smgr.GetYskmByDep("", strdydj);
            InserTree(TreeView1.Nodes[0], list1, kmlist);
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
        YsglDal ysDal = new YsglDal();
        DateTime dt = DateTime.Now;
        if (list.Count > 0)
        {
            IList<JsonRet> retlist = new List<JsonRet>();

            foreach (string km in list)
            {
                JsonRet temp = new JsonRet();
                temp.Yscode = km;

                YsManager ysmgr = new YsManager();

                string kmCode = string.IsNullOrEmpty(km) ? "" : km.Split(']')[0].Trim('[');
                if (!string.IsNullOrEmpty(Request["dydj"]))
                {
                    strdydj = Request["dydj"];
                }
                retlist.Add(temp);
            }
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            string script = jserializer.Serialize(retlist);
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>window.returnValue='" + script + "'; self.close();</script>");
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
