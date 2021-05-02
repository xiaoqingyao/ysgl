<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisAgreeToSpecial.aspx.cs" Inherits="webBill_MyWorkFlow_DisAgreeToSpecial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        function closeParentDetail() {
            parent.closeDetail();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Button ID="btn_save" runat="server" Text="确 定" OnClick="btn_save_Click" CssClass="baseButton" />&nbsp;&nbsp;
                  <%--  <input type="button" value="取消" class="baseButton " onclick="Cancel()" />--%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButton ID="rdb_first" runat="server" Text="制单人" AutoPostBack="true" GroupName="dis" OnCheckedChanged="RadioButton1_CheckedChanged" />
                    <asp:RadioButton ID="rdb_special" runat="server" Text="某审批节点" AutoPostBack="true" GroupName="dis" OnCheckedChanged="RadioButton1_CheckedChanged" />
                    <asp:DropDownList ID="ddl_prevLiuCheng" runat="server">
                    </asp:DropDownList>
                </td>

            </tr>
            <tr>
                <td>驳回意见：
                    <asp:TextBox ID="txt_mind" runat="server"  Width="400"  TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

        </table>
        <asp:HiddenField ID="hfRecordid" runat="server" />
    </form>
</body>
</html>
