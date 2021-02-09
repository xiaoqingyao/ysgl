<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cwtbSmFj.aspx.cs" Inherits="webBill_ysgl_cwtbSmFj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算科目 说明附件</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
       
        <table border=0 cellpadding=0 cellspacing=0 width="100%">
            <tr>
                <td style="height: 17px">
                </td>
            </tr>
            <tr>
                <td style="text-align:center;"> <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="440">
            <tr>
                <td class="tableBg">选择附件
                </td><td><input type="file" runat="server" id="upLoadFiles" style="width: 281px" class="baseButton" />
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="上 传" OnClick="Button1_Click" /></td>
            </tr>
        </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
