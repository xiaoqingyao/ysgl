<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chailvbxPrint.aspx.cs" Inherits="webBill_YbbxPrint_chailvbxPrint" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>差旅费报销单-打印</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
       <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <style media="print">
        .Noprint
        {
            display: none;
        }
    </style>

    <script type="text/javascript">
        var HKEY_Root, HKEY_Path, HKEY_Key;

        HKEY_Root = "HKEY_CURRENT_USER";

        HKEY_Path = "\\Software\\Microsoft\\Internet Explorer\\PageSetup\\";

        var head, foot, top, bottom, left, right;

        //设置网页打印的页眉页脚和页边距
        window.onload = function() {
            PageSetup_Null();
        }

        //设置网页打印的页眉页脚和页边距   

        function PageSetup_Null() {

            try {

                var Wsh = new ActiveXObject("WScript.Shell");

                HKEY_Key = "header";

                //设置页眉（为空）   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "");

                HKEY_Key = "footer";

                //设置页脚（为空）   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "");

                HKEY_Key = "margin_bottom";

                //设置下页边距（0）   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "");

                HKEY_Key = "margin_left";

                //设置左页边距（0）   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "2");

                HKEY_Key = "margin_right";

                //设置右页边距（0）   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, ""); //0.5

                HKEY_Key = "margin_top";

                //设置上页边距（8）   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "2.8");

            }

            catch (e) {
                alert("对不起，您的浏览器安全级别太高，不允许启用ActiveX控件，请单击‘如何启用ActiveX’按钮获取帮助。");
            }

        }
        function onlyprint() {
            try {
                factory.printing.portrait = true; //portrait是指打印方向，设置为true就是纵向，false就是横向。
                document.all.WebBrowser.ExecWB(7, 1);
            } catch (e) {
                alert("对不起，您的浏览器安全级别太高，不允许启用ActiveX控件，请单击‘如何启用ActiveX’按钮获取帮助。");
            }
        }
    </script>

</head>
<body>
    <object id="WebBrowser" height="0" width="0" classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"
        viewastext>
    </object>
    <div class="Noprint" style="margin-left: 5px; margin-top: 5px;">
    【打印帮助】<br />
    <ul>
        <li>进入页面当浏览器提示是否允许加载Active时单击“是”。</li>
        <li>单击“打印预览”按钮打开预览页面。</li>
        <li>把纵向打印改为横向打印模式（由于浏览器的差异，纵向改为横向时可能会变成多页，您只需要找到第二页然后打印当前页即可）。</li>
        <li>单击打印按钮。</li>
    </ul>
        <input name="Button" onclick="WebBrowser.ExecWB(7,1);" type="button" value="打印预览" 
            class="baseButton" />
    </div>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
            DisplayGroupTree="False" HasCrystalLogo="False" HasDrillUpButton="False" HasExportButton="False"
            HasGotoPageButton="False" HasSearchButton="False" HasToggleGroupTreeButton="False"
            HasViewList="False" HasZoomFactorList="False" PrintMode="Pdf" DisplayToolbar="False"
            HasPrintButton="False" />
    </div>
    </form>
</body>
</html>
