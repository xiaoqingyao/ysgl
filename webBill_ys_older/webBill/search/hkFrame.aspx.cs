﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_search_hkFrame : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(),"","window.open('../../webBill.aspx','_top')",true);
        }

    }
}