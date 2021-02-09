<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PingZhengBuLu.aspx.cs" Inherits="webBill_cwgl_PingZhengBuLu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改凭证</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
     <script src="../Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .tdLeft
        {
            text-align: right;
        }
    </style>
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div class="baseDiv">
        <table width="90%" border="0" class="baseTable">
            <tr>
                <td class="tdLeft">
                    凭证日期：
                </td>
                <td>
                    <asp:TextBox ID="txtDate" runat="server" Width="98%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdLeft">帐套：</td>
                <td>
                    <asp:DropDownList ID="ddlZhangTao" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdLeft">
                    凭证号：
                </td>
                <td>
                    <asp:TextBox ID="txtPingZhengHao" runat="server" Width="98%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:right">
                    <asp:Button ID="btn_Save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_Save_Click"/>
                    <input type="button" value="取 消" class="baseButton" onclick="javascript:window.close();" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
