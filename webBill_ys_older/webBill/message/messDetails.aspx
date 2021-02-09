<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeFile="messDetails.aspx.cs"
    Inherits="message_messDetails" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>消息发布</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        $(function() {
        // $("#txtendtime").datepicker();
            $("#btn_choose").click(function() {
                selectry();
            });
        });
        function selectry() {
            var str = window.showModalDialog('../select/SelectMoreUserFrame.aspx', 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:750px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var oldstr=$("#txt_spr").val();
                var newstr=oldstr+str;
                $("#txt_spr").val(newstr);
            }
        }
          function CloseWithParam(file, fileName, type) {
             
            if (type == "file2") {
                document.getElementById("<%=TextBox3.ClientID %>").innerText = file;

                alert("上传成功！");
                $("#HiddenField2").val(file);
                $("Hidfileurlfj").val(fileName);

            }
        }
    </script>

    <style type="text/css">
        .style1
        {
            background-color: #EDEDED;
            width: 81px;
            text-align: center;
        }
    </style>
</head>
<body style="background-color: #EBF2F5;">
    <form id="form2" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" style="height: 35px; text-align: center">
                <strong><span style="font-size: 12pt">信 &nbsp; 息 &nbsp; 发 &nbsp; 布</span></strong>
            </td>
        </tr>
        <tr>
            <td  style="text-align: center" >
                <table border="0"   class="myTable"  align="center">
                    <tr>
                        <td class="tableBg" style="width: 127px">
                            标题
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtTitle" runat="server" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg" style="width: 127px">
                            发布人
                        </td>
                        <td>
                            <asp:TextBox ID="txtWriter" runat="server"></asp:TextBox>
                        </td>
                        <td class="style1">
                            发布日期
                        </td>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                        </td>
                        <td class="tableBg">
                            阅读次数
                        </td>
                        <td>
                            <asp:TextBox ID="txtReadTimes" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            发布类型
                        </td>
                        <td>
                            <asp:DropDownList ID="Drtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="change">
                                <asp:ListItem Value="新闻">新闻</asp:ListItem>
                                <asp:ListItem Value="通知">通知</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style1" id="tdnmae" runat="server">
                            通知人
                        </td>
                        <td id="tdnamec" runat="server">
                            <input type="text" id="txt_spr" runat="server" />
                            <input type="button" id="btn_choose" class="baseButton" runat="server" value="选择" />
                        </td>
                        <td class="tableBg" id="tdendtime" runat="server">
                            通知有效期
                        </td>
                        <td runat="server" id="td2endtime">
                            <asp:TextBox ID="txtendtime" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg" colspan="6">
                            信息内容
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="height: 100%" id="msgContent" runat="server" valign="top">
                            <%--        <asp:TextBox ID="FCKeditor1" runat="server"  TextMode="MultiLine" Height="300" Width="99%"></asp:TextBox>--%>
                            <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" DefaultLanguage="zh-cn" SkinPath="../../fckEditor/editor/skins/mac/">
                            </FCKeditorV2:FCKeditor>
                        </td>
                    </tr>
                    <tr>
                       <td class="tableBg2"  style="text-align: right" runat="server" id="tdfj">
                                附件：
                            </td>
                  
                         <td runat="server" id="tdiframe" colspan="5" style="height: 100%"  runat="server" valign="top">
                            <iframe id="Iframe2" name="addPicture" src="../../SaleBill/Upload/Upload.aspx?Type=file2&UseName=false"
                                width="98%" runat="server" scrolling="no" height="30px" frameborder="0" style="border: 0px solid #f0f0f0;">
                            </iframe>
                            <asp:Label ID="TextBox3" runat="server" Text="" Width="100%"></asp:Label>
                            <asp:Label runat="server" ID="lbfj" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" style="text-align: center; height: 35px;">
                <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click" />
                &nbsp;&nbsp;&nbsp;<asp:Button ID="btn_cancel" runat="server" Text="关 闭" CssClass="baseButton"
                    OnClick="btn_cancel_Click" CausesValidation="False" />
                <asp:HiddenField ID="HiddenField2" runat="server" />
                <asp:HiddenField ID="Hidfileurlfj" runat="server" />
            </td>
        </tr>
    </table>
    <%--   <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
            ShowSummary="False" />--%>
    </form>
</body>
</html>
