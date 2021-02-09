<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BenefitproDetails.aspx.cs" Inherits="webBill_ysglnew_BenefitproDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>项目档案</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />

    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />


    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script type="text/javascript">
        function check() {
            var vProName = document.getElementById("txb_kmmc").value;
            if (vProName == undefined || vProName == "") {
                alert("项目名称不能为空！");
                return false;
            }
            return true;
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left" style="height: 35px; text-align: center">
                    <strong><span style="font-size: 12pt">
                        <asp:Label ID="lbl_mc" runat="server"></asp:Label>
                    </span></strong></td>
            </tr>
            <tr>
                <td align="left" style="text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 400px">
                        <tr runat="server" id="tr_xmbh">
                            <td class="tableBg" style="text-align: right">项目编号:</td>
                            <td>
                                <asp:TextBox ID="txb_kmcode" runat="server" Style="width: 80%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="tr_xmmc" runat="server">
                            <td class="tableBg" style="text-align: right">项目名称:</td>
                            <td colspan="1">
                                <input id="txb_kmmc" runat="server" style="width: 80%" type="text" /></td>
                        </tr>
                        <tr id="tr_nd" runat="server">
                            <td class="tableBg" style="text-align: right">财年:</td>
                            <td colspan="1">
                                <asp:DropDownList ID="drpNd" runat="server" AutoPostBack="True"  Style="width: 80%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <%--<tr>
                          <td class="tableBg" style=" text-align:right">
                               年度:</td>
                             <td colspan="1">
                                 <asp:TextBox ID="txt_annual" runat="server" Width="80%"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <tr id="tr_jsfs" runat="server">
                            <td class="tableBg" style="text-align: right">计算方式:</td>
                            <td colspan="1">
                                <asp:DropDownList ID="DropDownList1" runat="server" Style="width: 80%">
                                    <asp:ListItem Selected="True" Value="加">加项</asp:ListItem>
                                    <asp:ListItem Value="减">减项</asp:ListItem>
                                    <asp:ListItem Value="不计算">不计算</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr id="tr_lufs" runat="server">
                            <td class="tableBg" style="text-align: right">录入方式:</td>
                            <td colspan="1">
                                <asp:DropDownList ID="DropDownList3" runat="server" Style="width: 80%">
                                    <asp:ListItem Value="直接录入">直接录入</asp:ListItem>
                                    <asp:ListItem Value="明细汇总" Selected="True">明细汇总</asp:ListItem>
                                    <asp:ListItem Value="本表计算">本表计算</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr id="tr_pxh" runat="server">
                            <td class="tableBg" style="text-align: right">排序号:</td>
                            <td colspan="1">
                                <input id="sortcode" runat="server" style="width: 80%" type="text" /></td>
                        </tr>
                        <tr id="tr_yslx" runat="server">
                            <td class="tableBg" style="text-align: right">预算类型:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_yslx"  Style="width: 80%">
                                    <asp:ListItem Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="tr_xmzt" runat="server">
                            <td class="tableBg" style="text-align: right">项目状态:</td>
                            <td colspan="1">
                                <asp:DropDownList ID="DropDownList2" runat="server"  Style="width: 80%">
                                    <asp:ListItem Selected="True" Value="1">正常</asp:ListItem>
                                    <asp:ListItem Value="0">禁用</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>

                        <tr id="tr_zjr" runat="server">
                            <td class="tableBg" style="text-align: right">增加人:</td>
                            <td colspan="1">
                                <asp:Label ID="adduser" runat="server" Text=""  Style="width: 80%"></asp:Label>
                            </td>
                        </tr>

                        <tr id="tr_zjsj" runat="server">
                            <td class="tableBg" style="text-align: right">增加时间:</td>
                            <td colspan="1">
                                <asp:Label ID="adddate" runat="server" Text=""  Style="width: 80%"></asp:Label>
                            </td>
                        </tr>

                        <tr id="tr_xgr" runat="server">
                            <td class="tableBg" style="text-align: right">修改人:</td>
                            <td colspan="1">
                                <asp:Label ID="modifyuser" runat="server" Text=""  Style="width: 80%"></asp:Label>
                            </td>
                        </tr>


                        <tr id="tr_xgsj" runat="server">
                            <td class="tableBg" style="text-align: right">修改时间:</td>
                            <td colspan="1">
                                <asp:Label ID="modifydate" runat="server" Text=""  Style="width: 80%"></asp:Label>
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="text-align: center; height: 35px;">
                    <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="return check();" OnClick="btn_save_Click" />
                    &nbsp;&nbsp;&nbsp;<input id="btn_cancle2" value="取 消" onclick="javascript: window.close()" class="baseButton" type="button" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" />
    </form>
</body>
</html>
