<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Handover.aspx.cs" Inherits="webBill_xtsz_Handover" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>
    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script>
        $(function () {
            //人员选择
            $("#txtFrom").autocomplete({
                source: avaiusertb
            });
            $("#txtTo").autocomplete({
                source: avaiusertb
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin:10px;">
        将职员：<asp:TextBox runat="server" ID="txtFrom"></asp:TextBox>&nbsp;&nbsp;的审批流交接给：
        <asp:TextBox runat="server" ID="txtTo"></asp:TextBox>
        <asp:Button  runat="server" ID="sure"  OnClick="sure_Click" Text="确 定"/>
    </div>
    </form>
</body>
</html>