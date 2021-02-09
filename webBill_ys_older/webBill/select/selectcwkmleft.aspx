<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectcwkmleft.aspx.cs" Inherits="webBill_select_selectcwkmleft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <title></title>
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

        <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" ExpandDepth="1">
            <SelectedNodeStyle BackColor="#EBF2F5" />
            <Nodes>
                <asp:TreeNode ImageUrl="~/webBill/Resources/Images/treeView/treeHome.gif" NavigateUrl="selectcwkmlist.aspx?kmCode="
                    Target="list" Text="财务科目" Value="财务科目"></asp:TreeNode>
            </Nodes>
        </asp:TreeView>

    </form>
</body>
</html>
