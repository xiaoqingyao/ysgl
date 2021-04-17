<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CaiwubuDuiYing.aspx.cs" Inherits="webBill_xtsz_CaiwubuDuiYing" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<frameset cols="25%,*">
<frame id="left" name="left" src="CaiwubuDuiYingLeft.aspx?wdheight=<%=Request["wdheight"] %>"/>
<frame id="list" name="list" src="CaiwubuDuiYingRight.aspx?wdheight=<%=Request["wdheight"] %>&deptCode=" /></frameset>
</html>

