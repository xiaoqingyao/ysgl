<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deptGkLeft.aspx.cs" Inherits="webBill_Dept_deptGkLeft" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
 <meta http-equiv="X-UA-Compatible" content="IE=8" />
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
    <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" ExpandDepth="2" EnableViewState="false">
        <SelectedNodeStyle BackColor="#EBF2F5" />
    </asp:TreeView>
    </form>
</body>
</html>