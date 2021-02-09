<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ysnzjFrame.aspx.cs" Inherits="webBill_search_ysnzjFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算内追加查询</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<frameset cols="25%,*">
<frame id="left" name="left" src="ysnzjLeft.aspx?wdheight=<%=Request["wdheight"] %>" />
<frame id="list" name="list" src="ysnzjList.aspx?wdheight=<%=Request["wdheight"] %>&deptCode=" /></frameset>
</html>
