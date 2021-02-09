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
/// GuidHelper 的摘要说明
/// </summary>
public class GuidHelper
{
    public GuidHelper()
    {
    }
    public string getNewGuid()
    {
        System.Guid guid = System.Guid.NewGuid();
        return guid.ToString().ToUpper();
    }
}