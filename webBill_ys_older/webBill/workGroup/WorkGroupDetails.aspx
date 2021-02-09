<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkGroupDetails.aspx.cs"
    Inherits="webBill_workGroup_WorkGroupDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户角色</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="2" style="height: 35px; text-align: center">
                <strong><span style="font-size: 12pt">角&nbsp; 色&nbsp; &nbsp;信 &nbsp; 息</span></strong>
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 380px">
                    <tr>
                        <td align="left" class="tableBg">
                            角色编号
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtGroupID" runat="server" Width="150px"></asp:TextBox><asp:Button
                                ID="btnAgain" runat="server" Text="生成编号" CssClass="baseButton" OnClick="btnAgain_Click"
                                Visible="False" CausesValidation="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tableBg">
                            角色名称
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txb_groupname" runat="server" Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txb_groupname"
                                ErrorMessage="角色名称不能为空！">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="text-align: center; height: 35px;">
                <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_cancel" runat="server" Text="取 消" CssClass="baseButton" OnClick="btn_cancel_Click"
                    CausesValidation="False" />
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
    </form>
</body>
</html>
