<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkFlowFrame_Dept.aspx.cs" Inherits="webBill_MyWorkFlow_WorkFlowFrame_Dept" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 5px; margin-left: 5px; width: 95%">

            <div>
                <asp:Label ID="lb_msg" runat="server" Text="【友情提示】：先选择部门，再选择单据类型，设置完审批流程后，单击“保存”按钮保存设置。"></asp:Label>

            </div>

            <table>

                <tr>
                    <td style="text-align: left;">部门选择：
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="ddlBilldept" OnSelectedIndexChanged="ddlBilldept_TextChanged" runat="server" AutoPostBack="true"></asp:DropDownList>
                    </td>
                 
                </tr>
            </table>
        </div>
        <div style="float: left; width: 17%">
            <%--  <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />--%>
            <asp:TreeView ID="TreeView1" runat="server" Style="margin-right: 41px">
                <Nodes>
                    <asp:TreeNode ImageUrl="../Resources/Images/treeView/treeHome.gif" Text="流程类型"></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
            <asp:HiddenField ID="Hidflowid" runat="server" />
        </div>

        <div>
            <iframe id="right" name="right" frameborder='0' width="60%" height="600" scrolling="auto"></iframe>
        </div>
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
          
        </script>
    </form>
</body>
</html>

