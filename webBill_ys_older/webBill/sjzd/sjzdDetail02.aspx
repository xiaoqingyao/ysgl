<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sjzdDetail02.aspx.cs" Inherits="webBill_sjzd_sjzdDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据字典</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
                <strong><span style="font-size: 12pt">数&nbsp; 据&nbsp; 字&nbsp; 典&nbsp; 信&nbsp; 息</span></strong>
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 380px">
                    <tr>
                        <td align="right" class="tableBg">
                            字典编号
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_diccode" runat="server" Width="136px"></asp:TextBox>
                            <asp:Button ID="btnAgain" runat="server" CssClass="baseButton" OnClick="btnAgain_Click"
                                Text="生成编号" Visible="False" CausesValidation="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tableBg">
                            名称
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txb_dicname" runat="server" Width="136px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txb_dicname"
                                ErrorMessage="名称不能为空！">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trChongJianYs" runat="server">
                        <td align="right" class="tableBg">
                            冲减预算
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="DropDownList1" runat="server"  Width="138px">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trKongZhiYs" runat="server">
                        <td align="right" class="tableBg">
                            控制预算
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="DropDownList2" runat="server"  Width="138px">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trFuJiadj" runat="server">
                        <td align="right" class="tableBg">
                            是否附单据
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="DropDownList4" runat="server"  Width="138px">
                                <asp:ListItem Value="0">否</asp:ListItem>
                                <asp:ListItem Value="1">是</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trReceiver" runat="server">
                        <td align="right"  class="tableBg">
                            默认获取编号
                        </td>
                        <td>
                            <asp:TextBox ID="txtReceiver" runat="server"  Width="136px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trShiFouMoRen" runat="server">
                        <td align="right"  class="tableBg">
                            是否默认
                        </td>
                        <td>
                         <asp:DropDownList ID="ddlShiFouMoRen" runat="server"  Width="138px">
                                    <asp:ListItem Value="0">否</asp:ListItem>
                                    <asp:ListItem Value="1">是</asp:ListItem>
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tableBg">
                            所属字典
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddl_dictype" runat="server" Enabled="False"  Width="138px">
                            </asp:DropDownList>
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
