<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeskMsgDetails.aspx.cs" Inherits="webBill_Desktop_DeskMsgDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>技术支持</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left" style="height: 35px; text-align: center">
                        <strong><span style="font-size: 12pt">技&nbsp; 术&nbsp; &nbsp;支 &nbsp; 持</span></strong></td>
                </tr>
                <tr>
                    <td align="left" style="text-align: center">
                        <asp:TextBox ID="txtMes" runat="server" style="width:100%" Rows="10" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="text-align: center">
                        <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btn_cancel" runat="server" Text="取 消" CssClass="baseButton" OnClick="btn_cancel_Click" />
                    </td>
                </tr>
            </table>
        </form>
    </center>
</body>
</html>
