<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xmFrame.aspx.cs" Inherits="webBill_select_xmFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>选择项目</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<frameset cols="25%,*">
<frame id="left" name="left" src="xmLeft.aspx?deptCode=<%=deptCode %>"></frame>"/>
<frame id="list" name="list" src="xmList.aspx?deptCode=<%=deptCode %>&xmCode=" /></frameset>
</html>
