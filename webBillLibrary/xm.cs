using System;
using System.Collections.Generic;

using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace webBillLibrary
{
    public class xm : System.Web.UI.Page
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        /// <summary>
        /// 绑定单位的一级部门
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="otherParameter"></param>
        /// <param name="showChk"></param>
        /// <param name="OrgImgUrl"></param>
        /// <param name="ImgUrl"></param>
        /// <param name="deptStatus">格式： '1','2','0'</param>
        public void BindXmTree(TreeNode pNode, string url, string target, string otherParameter, bool showChk, string ImgUrl, string xmStatus, string xmDept)
        {
            string sql = "select * from bill_xm where isnull(sjxm,'')='' and xmDept='" + xmDept + "'";

            DataSet officeDataSet = server.GetDataSet(sql);

            for (int i = 0; i <= officeDataSet.Tables[0].Rows.Count - 1; i++)
            {
                TreeNode tNode = new TreeNode();
                tNode.Text = "[" + officeDataSet.Tables[0].Rows[i]["xmCode"].ToString().Trim() + "]" + officeDataSet.Tables[0].Rows[i]["xmName"].ToString().Trim();
                tNode.Value = officeDataSet.Tables[0].Rows[i]["xmCode"].ToString().Trim();

                if (url != "")
                {
                    tNode.NavigateUrl = url + "?xmCode=" + tNode.Value + otherParameter;
                    tNode.Target = target;
                }
                tNode.ShowCheckBox = showChk;
                if (ImgUrl != "")
                {
                    tNode.ImageUrl = ImgUrl + "office.gif";
                }
                pNode.ChildNodes.Add(tNode);
                this.BindXmTreeNextLevel(tNode, url, target, otherParameter, showChk, ImgUrl, xmStatus, xmDept);

            }
        }

        /// <summary>
        /// 递归绑定下级部门
        /// </summary>
        /// <param name="orgCode"></param>
        /// <param name="pNode"></param>
        /// <param name="officeDataSet"></param>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="otherParameter"></param>
        /// <param name="showChk"></param>
        /// <param name="OrgImgUrl"></param>
        /// <param name="ImgUrl"></param>
        /// <param name="deptStatus">格式： '1','2','0'</param>
        public void BindXmTreeNextLevel(TreeNode pNode, string url, string target, string otherParameter, bool showChk, string ImgUrl, string xmStatus, string xmDept)
        {
            string sql = "select * from bill_xm where sjxm='" + pNode.Value + "'";
            DataSet officeDataSet = server.GetDataSet(sql);

            for (int i = 0; i <= officeDataSet.Tables[0].Rows.Count - 1; i++)
            {
                TreeNode tNode = new TreeNode();
                tNode.Text = "[" + officeDataSet.Tables[0].Rows[i]["xmcode"].ToString().Trim() + "]" + officeDataSet.Tables[0].Rows[i]["xmname"].ToString().Trim();
                tNode.Value = officeDataSet.Tables[0].Rows[i]["xmCode"].ToString().Trim();

                if (url != "")
                {
                    tNode.NavigateUrl = url + "?xmCode=" + tNode.Value + otherParameter;
                    tNode.Target = target;
                }
                tNode.ShowCheckBox = showChk;
                if (ImgUrl != "")
                {
                    tNode.ImageUrl = ImgUrl + "office.gif";
                }
                pNode.ChildNodes.Add(tNode);
                this.BindXmTreeNextLevel(tNode, url, target, otherParameter, showChk, ImgUrl, xmStatus, xmDept);

            }
        }
    }
}
