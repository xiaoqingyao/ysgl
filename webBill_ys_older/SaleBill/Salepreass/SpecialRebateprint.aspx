<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpecialRebateprint.aspx.cs"
    Inherits="SaleBill_Salepreass_SpecialRebateprint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特殊返利申请单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function Print() {
            window.print();
        }
    </script>

</head>
<body>
    <center>
        <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="90%" border="0">
            <tr>
                <td style="text-align: center; height: 26px;" colspan="6">
                    <strong><span style="font-size: 12pt">特殊返利申请单</span></strong>
                    <div style="float: right; width: auto;">
                        <a href="#" onclick="Print();">[打印]</a></div>
                </td>
            </tr>
            <tr style="display: none">
                <td style="text-align: right">
                    申请部门：
                </td>
                <td colspan="2" style="width: 55%">
                    <asp:Label ID="lbeDept" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center" colspan="6">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="myTable">
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                申请日期：
                            </td>
                            <td>
                                <asp:Label ID="txtAppDate" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                报告单号：
                            </td>
                            <td>
                                <asp:Label ID="lbeBillCode" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                申请部门：
                            </td>
                            <td>
                                <asp:Label ID="lbdept" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                辆数：
                            </td>
                            <td>
                                <asp:Label ID="txtWorkPlan" runat="server" Width="98%"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr3" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                此报告执行有效时间：
                            </td>
                            <td>
                                <asp:Label ID="txtbgtime" runat="server" Style="width: 98%"></asp:Label>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                截止日期：
                            </td>
                            <td>
                                <asp:Label ID="txtendtime" runat="server" Style="width: 98%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                车辆情况：
                            </td>
                            <td colspan="3">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="选择" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox2" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Note1" HeaderText="订单号" HeaderStyle-CssClass="myGridHeader" />
                                        <asp:BoundField DataField="TruckCode" HeaderText="车架号" HeaderStyle-CssClass="myGridHeader" />
                                        <asp:BoundField DataField="StandardSaleAmount" HeaderText="正常返利" HeaderStyle-CssClass="myGridHeader" />
                                        <asp:BoundField DataField="ExceedStandardPoint" HeaderText="超出返利" HeaderStyle-CssClass="myGridHeader" />
                                        <asp:BoundField DataField="Explain" HeaderText="申请原因" HeaderStyle-CssClass="myGridHeader" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="display: none">
                <td colspan="6" style="text-align: center; height: 37px;">
                    <asp:Button ID="btn_fh" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close();" />
                </td>
            </tr>
        </table>
        </form>
    </center>
</body>
</html>
