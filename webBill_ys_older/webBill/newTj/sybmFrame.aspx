<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sybmFrame.aspx.cs" Inherits="webBill_newTj_sybmFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div style=" width:400px; margin:0 auto;">
        <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
            <tr>
                <td style="height: 125px; text-align: center">
                </td>
            </tr>
            <tr>
                <td style="text-align: center; height: 27px;">
                    <strong><span style="font-size: 12pt">归口费用使用部门统计</span></strong></td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable">
                        
                        <tr>
                            <td class="tableBg">
                                开始时间</td>
                            <td colspan="2" style="width: 257px">
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                截止时间</td>
                            <td colspan="2" style="width: 257px">
                                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                统计单位</td>
                            <td colspan="2" style="width: 257px">
                                <asp:DropDownList ID="drpDept" runat="server">
                                    <asp:ListItem>所有单位</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center">
                    <asp:Button ID="Button1" runat="server" CssClass="baseButton" OnClick="Button1_Click"
                        Text="生成统计表" /></td>
            </tr>
        </table>
        </div>
    </form>
</body>
</html>
