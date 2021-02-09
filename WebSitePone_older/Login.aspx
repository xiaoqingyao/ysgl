<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE>
<html>
<head runat="server">
    <title>行信全面预算</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link rel="apple-touch-icon" href="ic_launcher.png">
    <style>
        .aa {
            padding: 0px;
        }
    </style>

    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="js/Common.js" type="text/javascript"></script>

    <link href="Css/Login.css" rel="stylesheet" />
    <script>
        $(function () {
            $("input[type='text']").parent().css("margin", "0px");
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="pageone">
            <div id="header" class="ui-header ui-bar-a">
                <h1 class="ui-title">大智教育集团预算与报销系统</h1>
            </div>
            <div data-id="content">
                <div id="cd-login" class="is-selected">
                    <!-- 登录表单 -->
                    <div class="cd-form">
                        <p class="fieldset">
                            <label class="image-replace cd-username" for="signin-username">用户名</label>

                            <input class="full-width2 has-padding has-border" id="txtUserCode" runat="server" name="txtUserCode2" placeholder="输入用户名">
                        </p>
                        <!-- 用户名end -->

                        <p class="fieldset">
                            <label class="image-replace cd-password" for="signin-password">密码</label>
                            <input class="full-width2 has-padding has-border" id="txtUserPwd" runat="server" name="txtUserPwd" type="password" placeholder="输入密码">
                        </p>
                        <!-- 密码end -->
                        <div runat="server" id="trValidate" style="padding-bottom: 3px;">
                            <table style="width: 80%">
                                <tr>
                                    <td>
                                        <asp:TextBox placeholder="验证码" CssClass="full-width2 has-padding has-border aa" ID="txtCheckCode" runat="server" Width="100%"></asp:TextBox>
                                    </td>
                                    <td style="width: 80px;">
                                        <div style="padding-top: 10px;">
                                            <img id="imgYz" align="middle" alt="看不清？换一张..." src="validate.aspx" height="33px"
                                                style="width: 78px; padding-left: 2px; margin-left: 1px;" onclick="this.src='validate.aspx?a='+new Date" />
                                        </div>

                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="display: none;">
                                <input id="CheckBox1" name="CheckBox1" runat="server" type="checkbox">
                                <label for="remember-me">记住登录状态</label>                          
                        </div>

                        <p class="fieldset">
                            <label id="msg" runat="server" style="color: Red"></label>
                        </p>

                        <p class="fieldset">
                            <asp:Button ID="Button1" runat="server" Text="登 录" CssClass="full-width2" data-role="button" OnClick="Button1_Click" />
                        </p>
                    </div>
                </div>

                <asp:HiddenField runat="server" ID="hfIsEnableCookie" />
            </div>
            <div id="footer" class="ui-footer ui-bar-a">
                <h1 class="ui-title">©2015<a href="http://www.jnhxsoft.com/" class="ui-link">行信科技</a> 全面预算-移动版</h1>
            </div>
        </div>
    </form>

    <script>

        $("#hfIsEnableCookie").val(navigator.cookieEnabled);


    </script>

</body>
</html>
