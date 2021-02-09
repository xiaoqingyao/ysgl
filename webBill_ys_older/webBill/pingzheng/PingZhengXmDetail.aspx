<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PingZhengXmDetail.aspx.cs"
    Inherits="webBill_pingzheng_PingZhengXmDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>凭证项目类型明细页面</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            //取消
            $("#btn_Cancle").click(function() {
                window.returnValue = "";
                self.close();
            });
            $("#txtParent").autocomplete({
                source: availableTags
            });
        });
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div class="baseDiv" style="text-align: center">
        <table width="95%" class="baseTable">
            <tr>
                <td style="text-align: right">
                    编号：
                </td>
                <td>
                    <asp:TextBox ID="txtCode" runat="server" CssClass="baseText" Width="75%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    名称：
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" CssClass="baseText" Width="75%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">上级类型：</td>
                <td>
                    <asp:TextBox ID="txtParent" runat="server" CssClass="baseText" Width="75%"></asp:TextBox>
                    <asp:CheckBox ID="ChIsRootNode" runat="server" Text="设为根节点"/>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">是否为默认项目：</td>
                <td><asp:CheckBox ID="cbIsDefault" runat="server" Text="设为默认"/></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    状态：
                </td>
                <td>
                    <asp:RadioButtonList ID="rdStatus" runat="server" RepeatDirection="Horizontal" CssClass="baseRadio">
                        <asp:ListItem Value="1" Selected="True">正常</asp:ListItem>
                        <asp:ListItem Value="0">禁用</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">项目大类编码：</td>
                <td>
                    <asp:TextBox ID="txtcitem_class" runat="server" CssClass="baseText" Width="75%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">项目编码：</td>
                <td>
                    <asp:TextBox ID="txtcitem_id" runat="server" CssClass="baseText" Width="75%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <div style="text-align:center"><asp:Button ID="btn_Yes" runat="server" Text="确 定" 
                        OnClick="btn_Yes_Click" CssClass="baseButton" />
                    <input type="button" value="取 消" id="btn_Cancle" class="baseButton" /></div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
