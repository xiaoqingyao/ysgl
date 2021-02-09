<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webBill.aspx.cs" Inherits="webBill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>费用内控及网上报销系统 v1.0</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="webBill/Resources/Css/StyleSheet.css" type="Text/css" rel="stylesheet" />
    <link href="webBill/Resources/Css/StyleSheet2.css" type="Text/css" rel="stylesheet" />

    <script src="webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="webBill/Resources/jScript/jQuery/jquery-1.4.2.js" type="text/javascript"></script>
    <link rel="shortcut icon" href="favicon.ico" />
    <style type="text/css">
        html
        {
            margin: 0;
            padding: 0;
            min-width: 660px;
            height: 100%;
        }
        body
        {
            margin: 0 auto;
            padding: 0;
            font-size: 12px;
            color: #FFFF00;
            font-family: 宋体;
            min-width: 660px;
            min-height: 660px;
        }
        form, ul, li
        {
            margin: 0 auto;
            padding: 0;
            list-style: none;
        }
        input, textarea
        {
            font-size: 12px;
            font-family: 宋体;
            color: #2c7cbb;
        }
        .loginbg
        {
           
        }
        .in
        {
            padding: 0 0 0 12px;
        }
        .loginput
        {
            border: 1;
            padding: 0;
            margin: 0;
            height: 20px;
            line-height: 18px;
            width: 120px;
        }
        .yzm
        {
            font-size: 18px;
            font-weight: bold;
            line-height: 26px;
            padding-left: 15px;
        }
        .copyright
        {
            color: #91d2f2;
            padding-left: 10px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(function() {//edit by lvcc ^_^
            $("#txtUserCode").focus();
            $("input").mouseover(function() { this.focus(); });
        });
        function setClear() {
            document.getElementById("txtUserCode").value = "";
            document.getElementById("txtUserPwd").value = "";
            document.getElementById("txtCheckCode").value = "";
        }

        function userLoginSucess() {
            var width = screen.width;
            var height = screen.height;

            if (height == 768) height -= 75;
            if (height == 600) height -= 60;


            var szFeatures = "top=0,";
            szFeatures += "left=0,";
            szFeatures += "width=" + width + ",";
            szFeatures += "height=" + height + ",";
            szFeatures += "directories=no,";
            szFeatures += "status=yes,";
            szFeatures += "menubar=no,";

            //    _Default.SetScreenWidth(width);
            //    _Default.SetScreeniHeight(height);
            if (height <= 600) szFeatures += "scrollbars=yes,";
            //else if (height >= 800) szFeatures += "scrollbars=yes,";
            else szFeatures += "scrollbars=no,";

            szFeatures += "resizable=yes"; //channelmode
            window.open("webBill/main/mainFrame.aspx", "", szFeatures);

            //不提示的关闭窗口
            window.opener = "telchina";
            window.close();
        }
    </script>

</head>
<body>
    <form id="Form1" name="form1" runat="server">
    <div style="width:1000px; margin:0 auto;">
        <table style="width:100%;"  cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td style="height: 543px; text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" style="background-image: url(webBill/Resources/loginBg.jpg);width: 964px;height:380px;" runat="server" id="mainTable">
                        <tr>
                            <td style="width: 567px; height: 182px;" valign="top">
                            </td>
                            <td valign="bottom" style="height: 182px">
                                </td>
                        </tr>
                        <tr>
                            <td style="width: 567px">
                            </td>
                            <td style="text-align: left" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="height: 45px; text-align: right" colspan="3">
                                            <span style="font-size: 16pt; font-family: 华文隶书">费用内控及报销系统</span></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px; height: 29px; text-align: center">
                                        </td>
                                        <td style="width: 63px; height: 29px; text-align: center;">
                                            用户名：</td>
                                        <td style="width: 73px; height: 29px;" valign="middle">
                                            <input id="txtUserCode" runat="server" name="txtUserCode" type="text" value="" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px; height: 29px; text-align: center">
                                        </td>
                                        <td style="width: 63px; height: 29px; text-align: center;">
                                            密 码：</td>
                                        <td style="width: 73px; height: 29px;">
                                            <input id="txtUserPwd" runat="server" name="txtUserPwd" type="password" value="" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px; height: 29px; text-align: center">
                                        </td>
                                        <td style="width: 63px; height: 29px; text-align: center;">
                                            验证码：</td>
                                        <td style="width: 73px; height: 29px;">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" style="width: 76px">
                                                        <asp:TextBox ID="txtCheckCode" runat="server" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td align="left">
                                                        <img align="middle"alt="" src="validate.aspx" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1" style="height: 37px; text-align: center">
                                        </td>
                                        <td colspan="2" style="text-align: center; height: 37px;">
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/webBill/Resources/itbn_login.jpg"
                                                OnClick="ImageButton1_Click" />
                                            &nbsp; &nbsp;
                                            <img border="0" onclick="setClear();" alt=""
                                                    src="webBill/Resources/ibtn_cancel.jpg" style="cursor: hand" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </div>
    </form>
</body>
</html>
