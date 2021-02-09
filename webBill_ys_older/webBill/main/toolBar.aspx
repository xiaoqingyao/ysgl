<%@ Page Language="C#" AutoEventWireup="true" CodeFile="toolBar.aspx.cs" Inherits="main_toolBar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />

    <script language="javascript" type="text/javascript">
        
    function SetTimeDiaplay() {
		    var Timer = new Date()
		    var year = Timer.getFullYear()
		    var month = Timer.getMonth() + 1
		    var day = Timer.getDate()
		    var week = Timer.getDay()
		    var hours = Timer.getHours()
		    var minutes = Timer.getMinutes()
		    var seconds = Timer.getSeconds()
		    if (hours == 0)
		        hours = 12
		    if (minutes <= 9)
		        minutes = "0" + minutes
		    if (seconds <= 9)
		        seconds = "0" + seconds

		    if (week == 0)
		    { week = "日" }
		    if (week == 1)
		    { week = "一" }
		    if (week == 2)
		    { week = "二" }
		    if (week == 3)
		    { week = "三" }
		    if (week == 4)
		    { week = "四" }
		    if (week == 5)
		    { week = "五" }
		    if (week == 6)
		    { week = "六" }

		    myclock = year + "年" + month + "月" + day + "日    " + hours + ":" + minutes + ":" + seconds + "  星期" + week

		    document.getElementById("lbl_currentTime").innerHTML = myclock;
		    setTimeout("SetTimeDiaplay()", 1000)
		}
		
		function exitApplication()
		{
		    top.close();
		}
    </script>

    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/jScript/Ext/resources/css/ext-all.css" type="text/css" rel="Stylesheet" />
    <style type="text/css">
        .style2
        {
            width: 13px;
        }
    </style>
</head>
<body style="background-image: url(../Resources/images/toolBarBG.bmp);" onload="SetTimeDiaplay();">
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 26px">
            <tr>
                
                <td class="style2">
                </td>
                <td>
                    <a href="../Desktop/Desktop.aspx" target="main">【显示桌面】</a> <span id="lbl_currentTime"></span>&nbsp;&nbsp; 当前人员：<span id="lblCurrentUser" runat="server"></span>&nbsp;&nbsp; <%--当前操作：<span id="lblCurrentOperate">桌面</span>--%></td><td style="text-align:right;"><a href="../../webBill.aspx" target=_top>【重新登录】</a>&nbsp;&nbsp;<a href=# onclick="exitApplication();">【退出】</a>&nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>
