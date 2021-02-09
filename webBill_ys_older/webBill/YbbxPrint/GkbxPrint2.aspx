<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GkbxPrint2.aspx.cs" Inherits="webBill_YbbxPrint_GkbxPrint2" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <title>费用报销单-打印</title>
    <style media="print">
        .Noprint
        {
            display: none;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var HKEY_Root, HKEY_Path, HKEY_Key;

        HKEY_Root = "HKEY_CURRENT_USER";

        HKEY_Path = "\\Software\\Microsoft\\Internet Explorer\\PageSetup\\";

        var head, foot, top, bottom, left, right;


        function printdiv(div) {

            var newstr = document.getElementById(div).innerHTML;
            document.body.innerHTML = newstr;
            window.print();
        }

        //设置网页打印的页眉页脚和页边距
        window.onload = function() {
            //PageSetup_Null();
        }

        //取得页面打印设置的原参数数据   

        function PageSetup_temp() {

            try {

                var Wsh = new ActiveXObject("WScript.Shell");

                HKEY_Key = "header";

                //取得页眉默认值   

                head = Wsh.RegRead(HKEY_Root + HKEY_Path + HKEY_Key);

                HKEY_Key = "footer";

                //取得页脚默认值   

                foot = Wsh.RegRead(HKEY_Root + HKEY_Path + HKEY_Key);

                HKEY_Key = "margin_bottom";

                //取得下页边距   

                bottom = Wsh.RegRead(HKEY_Root + HKEY_Path + HKEY_Key);

                HKEY_Key = "margin_left";

                //取得左页边距   

                left = Wsh.RegRead(HKEY_Root + HKEY_Path + HKEY_Key);

                HKEY_Key = "margin_right";

                //取得右页边距   

                right = Wsh.RegRead(HKEY_Root + HKEY_Path + HKEY_Key);

                HKEY_Key = "margin_top";

                //取得上页边距   

                top = Wsh.RegRead(HKEY_Root + HKEY_Path + HKEY_Key);

            }

            catch (e) {

                alert("不允许ActiveX控件");

            }

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

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, "3");

            }

            catch (e) {
                alert("对不起，您的浏览器安全级别太高，不允许启用ActiveX控件，请单击‘如何启用ActiveX’按钮获取帮助。");
            }

        }

        //设置网页打印的页眉页脚和页边距为默认值   

        function PageSetup_Default() {

            try {

                var Wsh = new ActiveXObject("WScript.Shell");

                HKEY_Key = "header";

                HKEY_Key = "header";

                //还原页眉   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, head);

                HKEY_Key = "footer";

                //还原页脚   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, foot);

                HKEY_Key = "margin_bottom";

                //还原下页边距   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, bottom);

                HKEY_Key = "margin_left";

                //还原左页边距   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, left);

                HKEY_Key = "margin_right";

                //还原右页边距   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, right);

                HKEY_Key = "margin_top";

                //还原上页边距   

                Wsh.RegWrite(HKEY_Root + HKEY_Path + HKEY_Key, top);

            }

            catch (e) {

                alert("不允许ActiveX控件");

            }

        }



        function printorder() {

            PageSetup_temp(); //取得默认值   

            PageSetup_Null(); //设置页面

            WebBrowser.execwb(6, 6); //打印页面   

            PageSetup_Default(); //还原页面设置

        }

        function onlyprint() {
            try {
                factory.printing.portrait = true; //portrait是指打印方向，设置为true就是纵向，false就是横向。
                document.all.WebBrowser.ExecWB(7, 1);
            } catch (e) {
                alert("对不起，您的浏览器安全级别太高，不允许启用ActiveX控件，请单击‘如何启用ActiveX’按钮获取帮助。");
            }
            //if (!factory.object) {
            //     alert("打印控件没有正确安装!");
            ////      return;
            //} 
            //   factory.printing.portrait = false; //portrait是指打印方向，设置为true就是纵向，false就是横向。
            //   factory.printing.header = "";
            //   factory.printing.footer = "";
            // factory.printing.leftMargin = "77";
            //factory.printing.topMargin = "50";
            //factory.printing.rightMargin = "4.15";
            //factory.printing.bottomMargin = "3.98";
            //factory.DoPrint(true); //设置为false，直接打印
        }
    </script>

</head>
<body>
    <%-- <object id="WebBrowser" height="0" width="0" classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"
        viewastext>
    </object>
    <object id="factory" style="display: none" classid="clsid:1663ed61-23eb-11d2-b92f-008048fdd814"
        codebase="smsx.cab" viewastext>
    </object>--%>
    <div class="Noprint" style="margin-left: 5px; margin-top: 5px;">
        【使用前准备】：设置自定义纸张，该工作只需要操作一次。<br />
        &nbsp;&nbsp;步骤：“文件”=>“打印”=>“首选项”=>“纸张/质量”=>“尺寸”(单击自定义按钮打开自定义尺寸窗口)=>填写名称如“pingzhengzhi”、宽度(119.0)、高度(244.0)=>“保存”<br />
        【打印帮助】：<br />
        <ul>
            <li>单击左上角打印按钮弹出打印参数设置窗口。</li>
            <li>单击首选项。</li>
            <li>在“纸张/质量”标签页的尺寸栏选择之前的自定义的纸张（pingzhengzhi）。</li>
            <li>在“基本”标签页确认打印方向为横向，单击确定。</li>
            <li>单击“打印”。</li>
        </ul>
        <object id="CrystalPrintControl " classid="CLSID:BAEE131D-290A-4541-A50A-8936F159563A "
            codebase="PrintControl.cab " Version= "10,2,0,1078 " viewastext style="display: none" />
        <%-- <input name="Button" onclick="document.all.WebBrowser.ExecWB(1,1)" type="button"
            value="打开">
        <input name="Button" onclick="document.all.WebBrowser.ExecWB(2,1)" type="button"
            value="关闭所有">
        <input name="Button" onclick="document.all.WebBrowser.ExecWB(4,1)" type="button"
            value="另存为">
        <input name="Button" onclick="document.all.WebBrowser.ExecWB(6,1)" type="button" 
            value="打印">
        <input name="Button" onclick="document.all.WebBrowser.ExecWB(6,6)" type="button"
            value="直接打印">--%>
        <%--<input name="Button" onclick="onlyprint();" type="button" value="打 印" class="baseButton" />--%>
        <%--<input name="Button" onclick="WebBrowser.ExecWB(7,1);" type="button" value="打印预览" class="baseButton" />--%>
        <%-- <input name="Button" onclick="document.all.WebBrowser.ExecWB(8,1)" type="button"
            value="页面设置">
        <input name="Button" onclick="document.all.WebBrowser.ExecWB(10,1)" type="button"
            value="属性">
        <input name="Button" onclick="document.all.WebBrowser.ExecWB(17,1)" type="button"
            value="全选">
        <input name="Button" onclick="document.all.WebBrowser.ExecWB(22,1)" type="button"
            value="刷新">
        <input name="Button" onclick="document.all.WebBrowser.ExecWB(45,1)" type="button"
            value="关闭">
        <input name="Button" onclick="printorder();" type="button" value="我的打印">--%>
    </div>
    <form id="form1" runat="server">
    <div id="div_print">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
            DisplayGroupTree="False" HasCrystalLogo="False" HasDrillUpButton="False" HasExportButton="False"
            HasGotoPageButton="False" HasSearchButton="False" HasToggleGroupTreeButton="False"
            HasViewList="False" HasZoomFactorList="False" PrintMode="ActiveX" DisplayToolbar="true"
            HasPrintButton="true" />
    </div>
    </form>
</body>
</html>
