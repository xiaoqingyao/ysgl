<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoanListPrint.aspx.cs" Inherits="SaleBill_BorrowMoney_LoanListPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>预支单</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function Print() {
            window.print();
        }
        function openDetail(deptCode) {
            window.showModalDialog('../../webBill/bxgl/bxDetailFinal.aspx?type=look&billCode=' + deptCode, 'newwindow', 'center:yes;dialogHeight:570px;dialogWidth:970px;status:no;scroll:yes');
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" class="baseTable">
        <tr>
            <td style="text-align: center; height: 26px;" colspan="12">
                <strong><span style="font-size: 12pt">借款单</span></strong>
                <div runat="server" id="divprint" style="float: right; width: auto;">
                    <a href="#" onclick="Print();">[打印]</a></div>
            </td>
        </tr>
        <tr>
            <td style="text-align: center" class="tableBg2">
                借款单位
            </td>
            <td style="text-align: center" class="tableBg2">
                借款单号
            </td>
            <td style="text-align: center" class="tableBg2">
                借款人
            </td>
            <td style="text-align: center" class="tableBg2">
                借款日期
            </td>
            <td style="text-align: center" class="tableBg2">
                可报销科目
            </td>
            <td style="text-align: center" class="tableBg2">
                借款金额
            </td>
            <td style="text-align: center" class="tableBg2">
                状态
            </td>
            <td style="text-align: center" class="tableBg2">
                结算方式
            </td>
            <td style="text-align: center" class="tableBg2">
                审批状态
            </td>
            <td style="text-align: center" class="tableBg2">
                冲减单号
            </td>
            <td style="text-align: center" class="tableBg2">
                经办人
            </td>
            <td style="text-align: center" class="tableBg2">
                经办日期
            </td>
            
        </tr>
        <tr>
            <td style="text-align: center">
                <asp:Label ID="lbldept" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblodercode" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lbloanName" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblloandate" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lbkmcode" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblmoney" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lbljstype" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblSPStatus" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblCjPocode" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblRepsonName" runat="server" Text=""></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblRepsonDate" runat="server" Text=""></asp:Label>
            </td>
             <td style="text-align: center" class="hiddenbill">
                <asp:Label ID="lblycjje" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr runat="server" id="trgride">
            <td colspan="12" style="text-align: center">
                <div style="overflow: auto; width: 100%; height: 250px">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                        ShowHeader="true" Width="100%" EmptyDataText="没有合适的报销单可冲减。">
                        <Columns>
                            <asp:TemplateField HeaderText="选 择" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox2" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="billCode" HeaderText="编号" HeaderStyle-CssClass="hiddenbill"
                                ItemStyle-CssClass="hiddenbill" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="billname" HeaderText="报销单号" HeaderStyle-CssClass="myGridHeader"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="je" HeaderText="金额" HeaderStyle-CssClass="myGridHeader"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="fykmname" HeaderText="费用科目" HeaderStyle-CssClass="myGridHeader"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="billname" HeaderText="报销单号" HeaderStyle-CssClass="hiddenbill"
                                ItemStyle-CssClass="hiddenbill" ItemStyle-HorizontalAlign="Center" />
                         
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr runat="server" id="trcjcz">
            <td colspan="12" style="text-align: center">
                <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click" />&nbsp;
                <asp:Button ID="btn_fh" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close();" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
