<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chushihua.aspx.cs" Inherits="webBill_xtsz_chushihua" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td style="height: 366px; text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 268px">
                        <tr>
                            <td style="height: 35px; text-align: center">
                                <span style="color: #ff0000"><strong>系统初始化会清空所有业务数据,请谨慎操作！</strong></span>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 35px; text-align: center;">
                                <asp:Button ID="btnRedirect" runat="server" Text="系 统 初 始 化" CssClass="baseButton" OnClientClick="return confirm('该操作会清除系统所有数据,是否继续？');" OnClick="btnRedirect_Click" Font-Bold="True" Font-Size="X-Large" ForeColor="Red" />
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
