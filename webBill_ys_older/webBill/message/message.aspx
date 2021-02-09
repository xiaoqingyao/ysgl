<%@ Page Language="C#" AutoEventWireup="true" CodeFile="message.aspx.cs" Inherits="webBill_message_message" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>友情提示</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <table style="text-align: center" cellpadding="0" cellspacing="0" width="100%" class="myTable">
        <tr>
            <td align="left" style="height: 35px; text-align: center">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: center" align="center">
                <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="100%">
                    <tr>
                        <td class="tableBg">
                            发布人
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Label" Width="127px"></asp:Label>
                        </td>
                        <td class="tableBg">
                            发布日期
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Label" Width="127px"></asp:Label>
                        </td>
                        <td class="tableBg">
                            阅读次数
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Label" Width="127px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg" colspan="6">
                            信息内容
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                            <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" Height="350px" DefaultLanguage="zh-cn">
                            </FCKeditorV2:FCKeditor>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            附件：
                        </td>
                        <td colspan="5">
                        <%-- <iframe id="Iframe2" name="addPicture" src="../../SaleBill/Upload/Upload.aspx?Type=file2&UseName=false"
                                width="98%" runat="server" scrolling="no" height="30px" frameborder="0" style="border: 0px solid #f0f0f0;">
                            </iframe>--%>
                         <asp:Label ID="TextBox3" runat="server" Text="" Width="100%"></asp:Label>
                            <asp:Label runat="server" ID="lbfj" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" style="text-align: center; height: 27px;">
                <asp:Button ID="btn_cancel" runat="server" Text="关 闭" CssClass="baseButton" Visible="false"
                    OnClick="btn_cancel_Click" CausesValidation="False" />
                     <asp:HiddenField ID="HiddenField2" runat="server" />
                <asp:HiddenField ID="Hidfileurlfj" runat="server" />
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
    </form>
</body>
</html>
