<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wkSaver.aspx.cs" Inherits="WorkFlow_wkSaver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>保存流程设置</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../webBill/Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../webBill/Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="inc/httpRequest.js"></script>

    <script language="javascript" type="text/javascript">
        function saveInfo() {
            var opener = window.dialogArguments;

            var FlowXML = opener.document.all.FlowXML;
            var xml = FlowXML.value;

            var val=WorkFlow_wkSaver.saveWorkFlow(xml).value;
            if (val == "") {
                alert('保存成功！');
                self.close();
            }
            else {
                alert(val);
            }
        }

        function cancle() {
            self.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
                <tr>
                    <td style="height: 45px; text-align: center">
                        <strong><span style="font-size: 12pt; color: #ff0000">确认保存当前设置的流程步骤?</span></strong></td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 35px;">
                        <input type="button" class="baseButton" value="确 定" onclick='saveInfo()' onfocus='this.blur()' id="Button1" />&nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<input type="button" class="baseButton" value="取 消" onclick='cancle()' onfocus='this.blur()' /></td>
                </tr>
            </table>
    </form>
</body>
</html>
