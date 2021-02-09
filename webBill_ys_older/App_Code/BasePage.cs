using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
///BasePage 的摘要说明
/// </summary>
public class BasePage : System.Web.UI.Page
{
    public BasePage()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 返回一个数组  数组中的第一个元素代表rowfrm 第二个 rowto 第三个根据页面高度计算的每页显示的行数
    /// </summary>
    /// <param name="pageIndex">页码（第几页）</param>
    /// <param name="wdHeight">窗口的高度（post传过来的也就是回发才会有效）</param>
    /// <param name="wdHeight">除了数据表格之外的页面元素的高度和 </param>
    /// <returns></returns>
    protected int[] ComputeRow(int pageIndex, int wdHeight, int otherHeight)
    {
        //页面的高度
        int iwindowheight = 0;
        //如果是非回发，也就是从左侧的菜单树打开 这个时候在从左侧打开的时候已经获取了页面的高度 用get传值的方法给页面传值 告诉后台
        if (!IsPostBack)
        {
            //获取get传来的页面高度
            object objWdHeight = Request["wdheight"];
            if (!(objWdHeight != null && int.TryParse(objWdHeight.ToString(), out iwindowheight) && iwindowheight > 0))
            {
                iwindowheight = 400;
            }
        }
        else
        {
            iwindowheight = wdHeight;
        }
        //减去按钮和下面分页控件的
        iwindowheight = iwindowheight - otherHeight;
        //计算每页显示的行数
        int iEveCount = 1;//(iwindowheight / 31) - 1;//每页显示条数
        if (iwindowheight > 0 && (iwindowheight / 28) > 1)
        {
            iEveCount = (iwindowheight / 28) - 1;
        }

        int[] ret = new int[3];
        int pagRows = Convert.ToInt32(iEveCount);
        if (pageIndex <= 0)
        {
            pageIndex = 1;
        }
        //计算rowfrm
        ret[0] = (pageIndex - 1) * pagRows;
        //计算rowto
        ret[1] = pageIndex * pagRows;
        //每页的行数
        ret[2] = iEveCount;
        return ret;
    }
    public static string SubString(string longStr)
    {

        try
        {
            string result = "";
            if (!string.IsNullOrEmpty(longStr) && longStr.Length > 1 && longStr.IndexOf("[") != -1 && longStr.IndexOf("]") != -1)
            {
                int i = longStr.LastIndexOf("]");
                result = longStr.Substring(1, i - 1);
            }
            else
            {
                result = longStr;
            }
            return result;
        }
        catch (Exception e)
        {

            throw e;
        }
    }


    /// <summary>
    /// '[code]'name 中截取出code
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string CutVal(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            try
            {
                return str.Substring(1, str.IndexOf(']') - 1);
            }
            catch (Exception)
            {
                return str;
            }
        }
        else
            return "";

    }

    /// <summary>
    /// '[code]'name 中截取出name
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string GetName(string str)
    {
        try
        {
            return str.Substring(str.IndexOf(']') + 1);
        }
        catch (Exception)
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
    public void showMessage(string strMsg, bool isExit, string strReturnVal)
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
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    /// <param name="isExit">提示后是否退出</param>
    /// <param name="strReturnVal">返回值</param>
    /// <param name="appendSqlContent">增加的js</param>
    public void showMessage(string strMsg, bool isExit, string strReturnVal, string appendJsContent)
    {
        string strScript = "alert('" + strMsg + "');";
        if (!string.IsNullOrEmpty(appendJsContent))
        {
            strScript += appendJsContent;
        }
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
