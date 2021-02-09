<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE>
<html>
<head runat="server">
    <title>行信全面预算</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link rel="apple-touch-icon" href="ic_launcher.png">
    <link href="js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <link href="Css/CommenCss.css" rel="stylesheet" type="text/css" />

    <script src="js/Common.js" type="text/javascript"></script>


    <script>
     $(function(){
      $("input[type='text']").parent().css("margin","0px");
     });
    </script>

</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false"
    data-position="fixed">
    <div id="pageone">
        <div data-role="header">
            <h1>
                行信全面预算登录</h1>
        </div>
        <div data-role="content">
            <div align="left">
                用户名</div>
            <div align="left">
                <input id="txtUserCode" runat="server" name="txtUserCode2" type="text" value="" onmouseover="objfocus(this);" />
            </div>
            <div align="left">
                密 &nbsp;码</div>
            <div align="left">
                <input id="txtUserPwd" runat="server" name="txtUserPwd" type="password" value=""
                    onmouseover="objfocus(this);" /></div>
            <div runat="server" id="trValidate" style="padding-bottom: 3px;">
                <div align="left">
                    验证码</div>
                <div style="float: left;">
                    <asp:TextBox ID="txtCheckCode" runat="server" onmouseover="objfocus(this);" data-inline="true"
                        Width="80px"></asp:TextBox>
                </div>
                <div style="padding-top: 10px;">
                    <img id="imgYz" align="middle" alt="看不清？换一张..." src="validate.aspx" height="33px"
                        style="width: 78px; padding-left: 2px; margin-left: 1px;" onclick="this.src='validate.aspx?a='+new Date" />
                </div>
            </div>
              <div style="padding-top: 10px;"></div>
            <div align="left" style="padding-top: 10px; display:none;">
                <input id="CheckBox1" name="CheckBox1" type="checkbox" data-role='none' runat="server"  />下次自动登录
            </div>
            <div align="center">
                <asp:Button ID="Button1" runat="server" Text="登 录" data-role="button" OnClick="Button1_Click" /><%--data-inline="true"--%>
                <%--<input id="Reset1" type="reset" value="重 置" data-inline="true" />--%>
            </div>
            <div style="margin-bottom: 0px">
                <label id="msg" runat="server" style="color: Red">
                </label>
            </div>
            <asp:HiddenField runat="server" ID="hfIsEnableCookie" />
        </div>
        <div data-role="footer" data-position="fixed">
            <footer data-role="footer" id="footer"><h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1></footer>
        </div>
    </div>
    </form>

    <script>
    
    $("#hfIsEnableCookie").val(navigator.cookieEnabled);  
    
    
    </script>

</body>
</html>
