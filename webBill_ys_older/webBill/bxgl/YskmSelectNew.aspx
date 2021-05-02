<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YskmSelectNew.aspx.cs" Inherits="webBill_bxgl_YskmSelectNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>选择预算科目</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <div style="overflow: auto; height: 380px;">
            <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" ExpandDepth="2" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                <Nodes>
                    <asp:TreeNode ImageUrl="../Resources/Images/treeView/treeHome.gif" SelectAction="Select"
                        Text="预算科目" Value=""></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </div>
        <div style="bottom: 5px; position: fixed; width: 100%">
            <div style="width: 180px; margin: 0 auto;">
                <asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="确 定" OnClick="Button1_Click" />
                <%--<input id="Button2" type="button" value="取 消" class="baseButton" onclick="javascript: $('#dialog-confirm',window.parent.document).dialog('close');" />--%>
            </div>
        </div>
    </form>
</body>
</html>
