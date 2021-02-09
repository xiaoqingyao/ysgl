<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PassworkEdit.aspx.cs" Inherits="webBill_main_PassworkEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <title>密码修改</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <style type="text/css">
        div
        {
            width: 300px;
            margin: 0 auto;
        }
        td
        {
            text-align: center;
        }
    </style>

    <script type="text/javascript">
        function checkpwd() {
            var newpwd = document.getElementById("TextBox2").value;
            var checkpwd = document.getElementById("TextBox3").value;
            if (newpwd == checkpwd) {
                return true;
            }
            else {
                alert("两次输入的新密码不相等!");
                return false;
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table class="baseTable" width="90%">
        <tr>
            <td  style="width: 150px; text-align: right">
                人员编号:
            </td>
            <td style="width: 150px; text-align: left">
                <asp:TextBox ID="TextBox4" runat="server" CssClass="baseText"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td  style="width: 150px; text-align: right">
                原 密 码:
            </td>
            <td style="width: 150px; text-align: left">
                <asp:TextBox ID="TextBox1" runat="server" TextMode="Password" CssClass="baseText"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td  style="width: 150px; text-align: right">
                新 密 码:
            </td>
            <td style="width: 150px; text-align: left">
                <asp:TextBox ID="TextBox2" runat="server" TextMode="Password" CssClass="baseText"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td  style="width: 150px; text-align: right">
                新密码确认:
            </td>
            <td style="width: 150px; text-align: left">
                <asp:TextBox ID="TextBox3" runat="server" TextMode="Password" CssClass="baseText"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: right">
                <asp:Button ID="Button1" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button1_Click"
                    OnClientClick=" return checkpwd();" />
                <input id="Button2" type="button" value="取 消" class="baseButton" onclick="javascript:window.close();" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
