using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// cwkm 的摘要说明
/// </summary>
public class cwkm : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public cwkm()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public void BindCwkm(TreeNode pNode, string url, string target, string imgUrl)
    {
        DataSet temp = server.GetDataSet("select * from bill_cwkm");
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["cwkmCode"].ToString().Trim();
            if (kmCode.Length == 4)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["cwkmCode"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["cwkmMc"].ToString().Trim(), kmCode);
                if (url != "")
                {
                    tNode.NavigateUrl = url + "&kmCode=" + tNode.Value;
                    tNode.Target = target;
                }
                if (imgUrl != "")
                {
                    tNode.ImageUrl = imgUrl;
                }
                pNode.ChildNodes.Add(tNode);
                this.BindCwkm2(tNode, temp, url, target, imgUrl);
            }
        }
    }
    private void BindCwkm2(TreeNode pNode, DataSet temp, string url, string target, string imgUrl)
    {
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["cwkmCode"].ToString().Trim();
            if (kmCode.Substring(0, kmCode.Length - 2) == pNode.Value
                && kmCode.Length == pNode.Value.Length + 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["cwkmCode"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["cwkmMc"].ToString().Trim(), kmCode);
                if (url != "")
                {
                    tNode.NavigateUrl = url + "&kmCode=" + tNode.Value;
                    tNode.Target = target;
                }
                if (imgUrl != "")
                {
                    tNode.ImageUrl = imgUrl;
                }
                pNode.ChildNodes.Add(tNode);
                this.BindCwkm2(tNode, temp, url, target, imgUrl);
            }
        }
   }
}
