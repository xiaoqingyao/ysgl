using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class webBill_sjzd_Sjzd_Left : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
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
                TreeNode tNode = new TreeNode("数据字典", "00");
                tNode.NavigateUrl = "sjzdList.aspx?wdheight="+Convert.ToString(Request["wdheight"])+"&dicType=";
                tNode.Target = "list";
                tNode.ImageUrl = "~/webBill/Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);
                DataDicDataBind(tNode, "sjzdList.aspx", "list", "&wdheight=" + Convert.ToString(Request["wdheight"]), false, "../Resources/images/treeview/");
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pNode">根目录</param>
    /// <param name="url">NavigateUrl 目标url</param>
    /// <param name="target">目标</param>
    /// <param name="otherParameter">其它参数</param>
    /// <param name="showChk">是否显示选中的checkbox</param>
    /// <param name="officeImgUrl">图片url</param>
    protected void DataDicDataBind(TreeNode pNode, string url, string target, string otherParameter, bool showChk, string officeImgUrl)
    {
        string str_sql = "select dictype,diccode,dicname from bill_dataDic where dictype='00' order by diccode asc";
        DataSet ds_dataDic = server.GetDataSet(str_sql);
        for (int i = 0; i < ds_dataDic.Tables[0].Rows.Count; i++)
        {
            TreeNode tNode = new TreeNode();
            tNode.Text = "[" + ds_dataDic.Tables[0].Rows[i]["diccode"].ToString().Trim() + "]" + ds_dataDic.Tables[0].Rows[i]["dicname"].ToString().Trim();
            tNode.Value = ds_dataDic.Tables[0].Rows[i]["diccode"].ToString().Trim();

            if (url != "")
            {
                tNode.NavigateUrl = url + "?dicType=" + tNode.Value + otherParameter;
                if (tNode.Value.Equals("02") || tNode.Value.Equals("21") || tNode.Value.Equals("22") || tNode.Value.Equals("23") || tNode.Value.Equals("24") || tNode.Value.Equals("02") || tNode.Value.Equals("10"))//如果是报销明细类型 则用sjzdList02或帐套号
                {
                    tNode.NavigateUrl = "sjzdList02.aspx?dicType=" + tNode.Value + otherParameter;
                }
                tNode.Target = target;
            }
            tNode.ShowCheckBox = showChk;
            if (officeImgUrl != "")
            {
                tNode.ImageUrl = officeImgUrl + "treeNode.gif";
            } 
            pNode.ChildNodes.Add(tNode); 
        }
    }
}
