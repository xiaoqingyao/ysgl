<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cgspDetailDetail.aspx.cs" Inherits="webBill_fysq_cgspDetailDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>采购审批单明细</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
<base target="_self" />

    <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>


    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body style="background-color: #EBF2F5;"> 
    <form id="form1" runat="server">
   
            <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
                <tr>
                    <td style="text-align: center; height: 27px;">
                        <strong><span style="font-size: 12pt">紧急、临时采购审批单明细</span></strong></td>
                </tr>
                <tr>
                    <td style="height: 26px; text-align: center">
                        <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 430px">
                            <tr>
                                <td class="tableBg">
                                    名称</td>
                                <td colspan="2">
                                    <asp:TextBox ID="TextBox1" runat="server" Width="244px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="tableBg">
                                    规格型号</td>
                                <td colspan="2">
                                    <asp:TextBox ID="TextBox2" runat="server" Width="244px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="tableBg">
                                    数量</td>
                                <td colspan="2">
                                    <asp:TextBox ID="TextBox3" runat="server" Width="143px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="tableBg">
                                    单价</td>
                                <td colspan="2">
                                    <asp:TextBox ID="TextBox4" runat="server" Width="143px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="tableBg">
                                    备注</td>
                                <td colspan="2">
                                    <asp:TextBox ID="TextBox5" runat="server" Width="311px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: center; height: 37px;">
                        <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click" />
                        &nbsp;
                        <asp:Button ID="btn_fh" runat="server" Text="取 消" CssClass="baseButton" OnClick="btn_fh_Click" />
                    </td>
                </tr>
                <tr style="display:">
                    <td colspan="6" style="height: 37px; text-align: center">
                        <asp:Label ID="lbl_BillCode" runat="server"></asp:Label></td>
                </tr>
            </table>
    </form>
</body>
</html>
