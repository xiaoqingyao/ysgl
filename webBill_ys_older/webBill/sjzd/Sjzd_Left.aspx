<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sjzd_Left.aspx.cs" Inherits="webBill_sjzd_Sjzd_Left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据字典树</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
    <asp:TreeView ID="TreeView1" runat="server" ShowLines="True">
<%--        <Nodes>
            <asp:TreeNode ImageUrl="~/webBill/Resources/Images/treeView/treeHome.gif" Text="数据字典"
                Value="数据字典">
                <asp:TreeNode ImageUrl="~/webBill/Resources/Images/treeView/treeNode.gif" NavigateUrl="sjzdList02.aspx?dicType=02"
                    Target="list" Text="报销明细类型" Value="报销明细类型"></asp:TreeNode>
                <asp:TreeNode ImageUrl="~/webBill/Resources/Images/treeView/treeNode.gif" NavigateUrl="sjzdList.aspx?dicType=03"
                    Target="list" Text="申请类别" Value="申请类别"></asp:TreeNode>
                <asp:TreeNode ImageUrl="~/webBill/Resources/Images/treeView/treeNode.gif" NavigateUrl="sjzdList.aspx?dicType=04"
                    Target="list" Text="采购资金计划类型" Value="采购资金计划类型"></asp:TreeNode>
                <asp:TreeNode ImageUrl="~/webBill/Resources/Images/treeView/treeNode.gif" NavigateUrl="sjzdList.aspx?dicType=05"
                    Target="list" Text="职务类型" Value="职务类型"></asp:TreeNode>
                    <asp:TreeNode ImageUrl="~/webBill/Resources/Images/treeView/treeNode.gif" NavigateUrl="sjzdList.aspx?dicType=06"
                    Target="list" Text="出差类型" Value="出差类型"></asp:TreeNode>
            </asp:TreeNode>
        </Nodes>--%>
    </asp:TreeView>
    </form>
</body>
</html>
