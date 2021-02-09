<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ysgcDetail.aspx.cs" Inherits="ysgl_ysgcDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算过程</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    
    <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>


    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" class="style1" width="100%">
            <tr>
                <td style="text-align: center; height: 26px;">
                    <strong><span style="font-size: 12pt">预&nbsp;&nbsp;算&nbsp;&nbsp;过&nbsp;&nbsp;程</span></strong></td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable">
                        <tr>
                            <td class="tableBg">
                                过程编号</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtGcbh" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGcbh"
                                    ErrorMessage="过程编号不能为空！">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                过程名称</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtXmmc" runat="server" Width="230px" Enabled="False" 
                                    ReadOnly="True"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtXmmc"
                                    ErrorMessage="过程名称不能为空！">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                预算年度</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtNian" runat="server" Width="60px" Enabled="False" 
                                    ReadOnly="True"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNian"
                                    ErrorMessage="预算年度不能为空！">*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtNian"
                                    ErrorMessage="预算年度输入错误！" MaximumValue="9999" MinimumValue="1900" Type="Integer">*</asp:RangeValidator></td>
                        </tr>
                        <tr id="ysLxDiv" runat="server">
                            <td class="tableBg">
                                预算类型</td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="drp_ysType" runat="server" AutoPostBack="True" 
                                    OnSelectedIndexChanged="drp_ysType_SelectedIndexChanged" Enabled="False">
                                    <asp:ListItem Value="0">年度预算</asp:ListItem>
                                    <asp:ListItem Value="1">季度预算</asp:ListItem>
                                    <asp:ListItem Value="2">月预算</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr style="display:none;" id="ysTypeDiv" runat="server">
                            <td class="tableBg">
                                季度(月份)</td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="drpJd" runat="server" Enabled="False">
                                    <asp:ListItem Value="一">第一季度</asp:ListItem>
                                    <asp:ListItem Value="二">第二季度</asp:ListItem>
                                    <asp:ListItem Value="三">第三季度</asp:ListItem>
                                    <asp:ListItem Value="四">第四季度</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="drpYueFen" runat="server" Enabled="False">
                                    <asp:ListItem Value="1">01月</asp:ListItem>
                                    <asp:ListItem Value="2">02月</asp:ListItem>
                                    <asp:ListItem Value="3">03月</asp:ListItem>
                                    <asp:ListItem Value="4">04月</asp:ListItem>
                                    <asp:ListItem Value="5">05月</asp:ListItem>
                                    <asp:ListItem Value="6">06月</asp:ListItem>
                                    <asp:ListItem Value="7">07月</asp:ListItem>
                                    <asp:ListItem Value="8">08月</asp:ListItem>
                                    <asp:ListItem Value="9">09月</asp:ListItem>
                                    <asp:ListItem Value="10">10月</asp:ListItem>
                                    <asp:ListItem Value="11">11月</asp:ListItem>
                                    <asp:ListItem Value="12">12月</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                开始时间</td>
                            <td style="text-align: left">
                                <input type="text" id="txtKssj" runat="server" onfocus="setday(this);" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtKssj"
                                    ErrorMessage="开始时间不能为空！">*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtKssj"
                                    ErrorMessage="开始时间输入错误！" MaximumValue="9999-12-31" MinimumValue="1900-01-01"
                                    Type="Date">*</asp:RangeValidator></td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                截止时间</td>
                            <td style="text-align: left">
                                <input type="text" id="txtJzsj" runat="server" onfocus="setday(this);" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtJzsj"
                                    ErrorMessage="截止时间不能为空！">*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtJzsj"
                                    ErrorMessage="截止时间输入错误！" MaximumValue="9999-12-31" MinimumValue="1900-01-01"
                                    Type="Date">*</asp:RangeValidator></td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                过程状态</td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownList1" runat="server">
                                    <asp:ListItem Value="0">未开始</asp:ListItem>
                                    <asp:ListItem Value="1">已开始</asp:ListItem>
                                    <asp:ListItem Value="2">已结束</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; height: 30px;">
                    <asp:Button ID="btn_Save" runat="server" OnClick="btn_Save_Click" Text="保 存" CssClass="baseButton" />
                    &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="btn_Cancle" runat="server" CausesValidation="False" OnClick="btn_Cancle_Click"
                        Text="取 消" CssClass="baseButton" /></td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
