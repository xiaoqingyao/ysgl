﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xmDeptFrame.aspx.cs" Inherits="webBill_xmsz_xmDeptFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<frameset cols="25%,*">
<frame id="left" name="left" src="xmDeptLeft.aspx?wdheight=<%=Request["wdheight"] %>"/>
<frame id="list" name="list" src="xmDeptList.aspx?wdheight=<%=Request["wdheight"] %>&deptCode=" /></frameset>
</html>