<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeptDuiYingEdit.aspx.cs"
    Inherits="webBill_pingzheng_DeptDuiYingEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改对应系统部门名称</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="baseTable" width="100%">
            <tr>
                <td style="text-align:right">
                    本系统名称：
                </td>
                <td>
                    <asp:Label ID="lblOlderName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td  style="text-align:right">
                    最新帐套部门名称：
                </td>
                <td>
                    <asp:TextBox ID="txtDeptName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right" style="text-align:right">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_OnClick" Text="保 存" CssClass="baseButton" />
                    <input id="btnCancle" type="button" onclick="javascript:window.close();" value="取 消" class="baseButton" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
