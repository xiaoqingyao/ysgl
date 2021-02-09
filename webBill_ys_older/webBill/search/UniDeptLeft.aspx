<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UniDeptLeft.aspx.cs" Inherits="webBill_search_UniDeptLeft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>部门选择页面</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <style type="Text/css">
        .Hidden
        {
            overflow-x: hidden;
        }
    </style>
</head>
<body class="Hidden">
    <form id="form1" runat="server">
    <div>
        <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" ExpandDepth="2" EnableViewState="false">
        </asp:TreeView>
    </div>
    </form>
</body>
</html>
