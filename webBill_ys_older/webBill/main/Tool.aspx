<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tool.aspx.cs" Inherits="webBill_main_Tool" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Resources/Css/Tool.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function exitApplication() {
            top.close();
        }
    </script>
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<body>
    <form id="form1" runat="server">
    <div class="main">
        <span>登陆人员:</span><span id="userName" runat="server"></span>
        <span>部门:</span><span id="deptName" runat="server"></span>
        <a href="../../webBill.aspx" target="_top">【重新登录】</a>
        <a href="#" onclick="exitApplication();">【退出】</a>
    </div>
    </form>
</body>
</html>
