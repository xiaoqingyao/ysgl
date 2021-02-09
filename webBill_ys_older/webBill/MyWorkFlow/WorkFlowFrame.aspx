<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkFlowFrame.aspx.cs" Inherits="webBill_MyWorkFlow_WorkFlowFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
      
        <div style="float: left">
            <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
            <asp:TreeView ID="TreeView1" runat="server">
                <Nodes>
                    <asp:TreeNode ImageUrl="~/webBill/Resources/Images/treeView/treeHome.gif" Text="流程类型"></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </div>
        <div>
            <iframe id="right" name="right" frameborder='0' width="80%" height="600px" scrolling="auto"></iframe>
        </div>
    </form>
</body>
</html>
