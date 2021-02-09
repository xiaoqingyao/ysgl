<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebateDetail.aspx.cs" Inherits="SaleBill_rebate_RebateDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>销售返利修改</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="2" style="height: 35px; text-align: center">
                <strong><span style="font-size: 12pt">返利设置</span></strong>
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 100%">
                    <tr>
                        <td align="left" class="tableBg">
                            费用发生日期
                        </td>
                        <td align="left">
                            <asp:Label ID="lbbgtime" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="tableBg">
                            申请单位:
                        </td>
                        <td align="left">
                            <asp:Label ID="lbdept" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="tableBg">
                            车架号
                        </td>
                        <td align="left">
                            <asp:Label ID="lbcar" runat="server" CssClass="InputLabel" Text=""></asp:Label>
                        </td>
                        <td align="left" class="tableBg">
                            车辆类型
                        </td>
                        <td align="left">
                            <asp:Label ID="lbcartype" runat="server" CssClass="InputLabel" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tableBg">
                            费用科目
                        </td>
                        <td align="left">
                            <asp:Label ID="lbfeetype" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="tableBg">
                            费用控制
                        </td>
                        <td align="left">
                            <asp:Label ID="lbfeecz" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="tableBg">
                            金额
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtfee" runat="server" CssClass="baseText"></asp:TextBox>
                        </td>
                        <td align="left" class="tableBg">
                            类别
                        </td>
                        <td align="left">
                            <asp:Label ID="lbtype" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tableBg">
                            状态
                        </td>
                        <td align="left">
                            <asp:Label ID="lbstatus" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="tableBg">
                            备注
                        </td>
                        <td align="left" colspan="2">
                            <asp:Label ID="lbbz" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="center" colspan="3" style="text-align: center; height: 35px;">
                            <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btn_cancel" runat="server" Text="取 消" CssClass="baseButton" OnClick="btn_cancel_Click"
                                CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:HiddenField ID="HiddenField1" runat="server" />
         <asp:HiddenField ID="HiddenField2" runat="server" />
        
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
    </form>
</body>
</html>
