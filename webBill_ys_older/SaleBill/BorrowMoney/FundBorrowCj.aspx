<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FundBorrowCj.aspx.cs" Inherits="SaleBill_BorrowMoney_FundBorrowCj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>借款单冲减</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js"
        type="text/javascript" charset="UTF-8"></script>

    <script type="text/javascript">
$(function(){
  $("#main th").addClass("myGridHeader");
  $("#main td").addClass("myGridItem").css({"text-align":"center"});
  
});

  
       function SingleSelect(aControl) {
            var chk = document.getElementById("GridView1").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = false;
            }
            
               aControl.checked =true;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id='main' width="100%" class="baseTable">
            <tr>
                <td style="text-align: center; height: 26px;" colspan="8">
                    <strong><span style="font-size: 12pt">借款冲减
                        <asp:Label ID="txtlb" runat="server"></asp:Label></span></strong>
                </td>
            </tr>
            <tr>
                <th>
                    借款人
                </th>
                <th>
                    借款金额
                </th>
                <th>已冲减</th>
                <th>
                    填报时间
                </th>
                <th>
                    借款类别
                </th>
                <th>
                    借款部门
                </th>
                <th>
                    借款天数
                </th>
                <th>
                    借款日期
                </th>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="txtloanName" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="txtmoney" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="txtycj" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="txtaddtime" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbjkcode" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="txtdeptname" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="txtjkts" runat="server"></asp:Label>
                    (天)
                </td>
                <td>
                    <asp:Label ID="txtjksj" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <div runat="server" id="trgride" style="text-align: center">
            <div style="overflow: auto; width: 100%; max-height: 250px">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                    ShowHeader="true" Width="100%" EmptyDataText="没有合适的报销单可冲减。">
                    <RowStyle CssClass="myGridItem" />
                    <HeaderStyle CssClass="myGridHeader" />
                    <Columns>
                        <asp:TemplateField HeaderText="选 择">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox2" runat="server"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="billCode" HeaderText="编号" HeaderStyle-CssClass="hiddenbill"
                            ItemStyle-CssClass="hiddenbill" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="billname" HeaderText="报销单号" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="je" HeaderText="金额" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="fykmname" HeaderText="费用科目" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="billname" HeaderText="报销单号" HeaderStyle-CssClass="hiddenbill"
                            ItemStyle-CssClass="hiddenbill" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div runat="server" id="trcjcz" style="text-align: center">
            <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click" />&nbsp;
            <asp:Button ID="btn_fh" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close();" />
        </div>
    </div>
    </form>
</body>
</html>
