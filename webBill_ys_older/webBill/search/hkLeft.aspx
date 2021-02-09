<%@ Page Language="C#" AutoEventWireup="true" CodeFile="hkLeft.aspx.cs" Inherits="webBill_search_hkLeft" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>还款单列表树</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <style type="Text/css">
        .Hidden {
            overflow-x: hidden;
        }
    </style>
</head>
<body class="Hidden">
    <form id="form1" runat="server">

        <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" EnableViewState="false">
        </asp:TreeView>

    </form>
</body>
</html>
