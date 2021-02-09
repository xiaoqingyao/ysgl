<%@ Page Language="C#" AutoEventWireup="true" CodeFile="changePwd.aspx.cs" Inherits="pwd_changePwd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <title></title>
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="../js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <link href="../Css/CommenCss.css" rel="stylesheet" type="text/css" />

    <script src="../js/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        function fanh() {
            window.location.href = '../Index.aspx';
        }
        function Check() {
            var ysmm = $("#ysmm").val();
            if (ysmm.length == 0) {
                alert("对不起，原密码不能为空");
                return false;
            }
            var xmm = $("#xmm").val();
            //新密码不能为空
            if (xmm.length == 0) {
                alert("对不起，新密码不能为空");
                return false;
            }
            //检查密码是否一致

            var cfmm = $("#cfmm").val();
            if (xmm != cfmm) {
                alert("两次密码输入不一致，请重新输入");
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div data-role="header">
            <a href="../Index.aspx" data-icon="home" data-role="button" data-ajax="false">主页</a>
            <h1>密码修改</h1>
        </div>
        <div data-role="content">
            <table style="width:60%">
                <tr>
                    <td style=" text-align:right">用户名:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="yhm" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style=" text-align:right">原密码:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="ysmm" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style=" text-align:right">新密码:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="xmm" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style=" text-align:right">新密码确认:&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="cfmm" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td  style="text-align: center;">
                        <asp:Button ID="btnSave" runat="server" Text="保存" data-inline="true" OnClick="btnSave_Click"
                            OnClientClick="return Check()" />
                        <input type="button" id="fanhui" onclick="fanh();" data-inline="true" value="返回" />
                    </td>
                </tr>
            </table>
        </div>
        <div data-role="footer" data-position="fixed">
            <footer data-role="footer" id="footer">
                <h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1>
            </footer>
        </div>
    </form>
</body>
</html>
