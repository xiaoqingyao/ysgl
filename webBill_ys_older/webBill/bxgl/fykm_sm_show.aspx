<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fykm_sm_show.aspx.cs" Inherits="webBill_bxgl_fykm_sm_show" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>费用科目选择提示</title>
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <span style="color: Red; font-size: small;">[友情提示]：为帮助大家高效填写报销单据，请先阅读费用科目的报销适用情况。</span>
    <div id="divshow" runat="server">
    </div>
    <div style="float: right">
        <input type="button" class="baseButton" value="关 闭" onclick="javascript:self.close();"
            style="margin-right: 5px" /></div>
    </form>
</body>
</html>
