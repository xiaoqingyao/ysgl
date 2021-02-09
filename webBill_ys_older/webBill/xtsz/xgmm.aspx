<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xgmm.aspx.cs" Inherits="xtsz_xgmm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td style="text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 323px">
                        <tr>
                            <td style="height: 31px; text-align: center">
                                <strong><span style="font-size: 12pt">修 改 密 码</span></strong></td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <table border="0" cellpadding="0" cellspacing="0" class="myTable">
                                    <tr>
                                        <td class="tableBg">原密码</td>
                                        <td style="width: 100px; height: 15px">
                                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="tableBg">新密码</td>
                                        <td>
                                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="tableBg">确认密码</td>
                                        <td>
                                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 35px; text-align: center;">
                                <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click" />
                                <asp:Button ID="btnRedirect" runat="server" Text="修改他人密码" CssClass="baseButton" OnClick="btnRedirect_Click" Visible="False" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
