using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Xml;
using System.Text;
using Ajax;

public partial class WorkFlow_wkSaver : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Ajax.Utility.RegisterTypeForAjax(typeof(WorkFlow_wkSaver));
    }

    [Ajax.AjaxMethod()]
    public string saveWorkFlow(string xml)
    {
        return (new workFlowLibrary.saveWkXml()).saveXml(xml);
    }
}
