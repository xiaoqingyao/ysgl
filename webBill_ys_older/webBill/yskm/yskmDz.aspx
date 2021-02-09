<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yskmDz.aspx.cs" Inherits="yskm_yskmDz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算科目对应</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 607px; height: 27px">
                    选择归集方式:
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem Value="0">归集到指定单位</asp:ListItem>
                        <asp:ListItem Value="1">部分归集</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 297px; width: 607px;">
                    <iframe id="detail" name="detail" width="100%" height="100%"></iframe>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
