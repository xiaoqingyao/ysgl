<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectMoreUserFrame.aspx.cs" Inherits="webBill_select_SelectMoreUserFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>选择人员</title> 
    <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<frameset cols="25%,*" scrolling=auto>
<frame id="left" name="left" src="MoreUserLeft.aspx" />
<frame id="list" name="list" src="MoreUserList.aspx?deptCode=" /></frameset>
</html>