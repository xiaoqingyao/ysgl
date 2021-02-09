<%@ Page Language="C#" AutoEventWireup="true" CodeFile="hkFrame.aspx.cs" Inherits="webBill_search_hkFrame" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<frameset cols="25%,*">
<frame id="left" name="left" src="hkLeft.aspx?wdheight=<%=Request["wdheight"] %>" />
<frame id="list" name="list" src="hkList.aspx?wdheight=<%=Request["wdheight"] %>&deptCode=" /></frameset>
</html>
