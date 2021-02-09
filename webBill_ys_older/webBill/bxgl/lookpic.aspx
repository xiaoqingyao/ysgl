<%@ Page Language="C#" AutoEventWireup="true" CodeFile="lookpic.aspx.cs" Inherits="webBill_bxgl_lookpic" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>查看附件图片</title>
</head>
<body>
    <form id="form1" runat="server">

        <table>
            <tr>
                <th  colspan="2">
                    <asp:Label ID="lbl_billname" runat="server"></asp:Label></th>
            </tr>
            <tr>
                <td  style="text-align:right"">
                    摘要：
                </td>
                <td style="text-align:left">
                    <asp:Label ID="lbl_zy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr /> 
                </td>
            </tr>
            <tr>
                <td colspan="2">
            
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
