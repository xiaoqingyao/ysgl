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
using Dal.SysDictionary;

public partial class webBill_bxgl_YskmSelectNew : System.Web.UI.Page
{
    static bool boCheckFlg = true;
    private string strdydj = "02";//对应单据编号默认02 
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

            YskmDal yskmDal = new YskmDal();
            IList<Bill_Yskm> list = new List<Bill_Yskm>();
            list = yskmDal.GetYskmBydjlx("01");//获取收入的预算科目
            InserTree(TreeView1.Nodes[0], list, kmlist);
            TreeView1.ExpandAll();
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

        if (list.Count > 0)
        {
            //StringBuilder sb = new StringBuilder();
            IList<JsonRet> retlist = new List<JsonRet>();

            foreach (string km in list)
            {
                JsonRet temp = new JsonRet();
                temp.Yscode = km;
                YsManager ysmgr = new YsManager();             
                string kmCode = string.IsNullOrEmpty(km) ? "" : km.Split(']')[0].Trim('[');
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
