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

public partial class ClearData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    /// <summary>
    /// 确定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnYes_Click(object sender, EventArgs e)
    {
        sqlHelper.sqlHelper sqlHelper = new sqlHelper.sqlHelper();
        
        string strFlg = this.rdbYW.Checked == true ? "1" : "0";//数据表中 1是业务数据 0 基础数据
        DataTable dtRel = sqlHelper.GetDataSet("select tbName,note1 from tab_Message where 1=0 and tbType='" + strFlg + "' and tbStatus!='D'").Tables[0];
        if (dtRel==null||dtRel.Rows.Count<=0)
        {
            Response.Write("没找到要删除的记录！");
            return;
        }
        int iRelCount = dtRel.Rows.Count;
        for (int i = 0; i < iRelCount; i++)
        {
  
            string strDelSql = string.Format("delete from {0} where 1=1 {1} ", dtRel.Rows[i][0],dtRel.Rows[i]["note1"]);
            try
            {
                sqlHelper.ExecuteNonQuery(strDelSql);
            }
            catch (Exception)
            {
                Response.Write("删除表"+dtRel.Rows[0][0]+"时出现错误！");
            }
        }
        string strMsg = strFlg=="1"?"业务数据":"基础数据";
        Response.Write("删除" + strMsg + "成功！");
    }
}
