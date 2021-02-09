<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zhuyuanshouru.aspx.cs" Inherits="webBill_shouru_zhuyuanshouru" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>住院收入生成收入单</title>
 <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="baseDiv">
        部门：<asp:DropDownList ID="ddlDept" runat="server">
        </asp:DropDownList>
        日期：
        <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
        <asp:Button ID="btnMakBxd" runat="server" Text="制单" OnClick="btnMakBxd_OnClick" CssClass="baseButton" />
    </div>
    </form>
</body>

<script type="text/javascript">
    $(function() {
        $("#txtDate").datepicker();
    });
</script>

</html>
