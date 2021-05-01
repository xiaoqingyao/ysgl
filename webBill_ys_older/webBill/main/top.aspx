<%@ Page Language="C#" AutoEventWireup="true" CodeFile="top.aspx.cs" Inherits="main_top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Resources/Css/Top.css" rel="stylesheet" type="text/css" />
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="logo" style=" padding-top:15px">
        <img src="../Resources/Images/xxkj.gif" alt="" style="height: 40px; width:200px"  runat="server" id="img_logo"/>
    </div>
    <div id="toolBar">
        <span>登录人员:</span><span id="userName" runat="server"></span> <span>部门:</span><span
            id="deptName" runat="server"></span> <a href="#" onclick="javascript:top.close();">【退出】</a>
        <a href="../../webBill.aspx" target="_top">【重新登录】</a> <a href="#" onclick="javascript:var returnValue=window.showModalDialog('PassworkEdit.aspx', 'newwindow', 'center:yes;dialogHeight:180px;dialogWidth:500px;status:no;scroll:yes');">
            【修改密码】</a>
    </div>
    <div id="weather">
        <ul>
            <li id="sp_first" runat="server"></li>
            <li id="sp_next" runat="server"></li>
            <li id="sp_last" runat="server"></li>
        </ul>
    </div>
    </form>
</body>
</html>
