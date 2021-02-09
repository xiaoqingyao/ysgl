<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UniDeptFrame.aspx.cs" Inherits="webBill_search_UniDeptFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<frameset cols="25%,*">
<frame id="left" name="left" src="UniDeptLeft.aspx?wdheight=<%=Request["wdheight"] %>&listurl=<%=Request["listurl"] %>" />
<frame id="list" name="list" src="<%=Request["listurl"] %>?wdheight=<%=Request["wdheight"] %>&deptCode=" /></frameset>
</html>
