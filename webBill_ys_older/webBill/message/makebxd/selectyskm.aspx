<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectyskm.aspx.cs" Inherits="webBill_makebxd_selectyskm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>选择预算科目...</title>

    <script type="text/javascript">
        function OnTreeNodeChecked() {
            var element = window.event.srcElement;
            var endStr = element.title;
            window.returnValue = endStr;
            window.close();
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" ExpandDepth="2">
            <SelectedNodeStyle BackColor="#EBF2F5" />
            <NodeStyle Font-Size="Small"  />
            <Nodes>
                <asp:TreeNode ImageUrl="~/webBill/Resources/Images/treeView/treeHome.gif" Text="预算科目"
                    Value="预算科目" Expanded="true"></asp:TreeNode>
            </Nodes>
        </asp:TreeView>
    </div>
    </form>
</body>
</html>
