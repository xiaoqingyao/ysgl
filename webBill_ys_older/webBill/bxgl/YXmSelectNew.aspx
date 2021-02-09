<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YXmSelectNew.aspx.cs" Inherits="webBill_bxgl_YXmSelectNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择项目</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
         <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td>
                <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" ExpandDepth="2">
                    <Nodes>
                        <asp:TreeNode ImageUrl="../Resources/Images/treeView/treeHome.gif" SelectAction="None" Text="部门项目" Value=""></asp:TreeNode>
                    </Nodes>
                </asp:TreeView>
            </td>
        </tr>
        <tr>
            <td style="text-align: center;">
                <asp:Button ID="Button1" runat="server" CssClass="baseButton" 
                    Text="确 定" onclick="Button1_Click" />
                <input id="Button2" type="button" value="取 消" class="baseButton" onclick="javascript:window.close();" />    
                </td>      
        </tr>
    </table>
    </form>
</body>
</html>
