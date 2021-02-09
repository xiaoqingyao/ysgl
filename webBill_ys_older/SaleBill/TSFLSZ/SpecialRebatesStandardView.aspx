<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpecialRebatesStandardView.aspx.cs"
    Inherits="SaleBill_TSFLSZ_SpecialRebatesStandardView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特殊返利查看</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/bxgl/bxDetail.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
    </style>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <script type="text/javascript">
        function openBillDetails() {
            var strBillCode = $("#lbsqcode").text();
            if (strBillCode == "" || strBillCode == undefined) {
                alert("请先选中行！");
                return;
            }
            openDetail("../Salepreass/SpecialRebatesAppDetails.aspx?Ctrl=look&Code=" + strBillCode);
        }
        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:550px;dialogWidth:900px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("Button6").click();
            }
        }
    </script>

</head>
<body style="background-color: #EDEDED;">
    <form id="form1" runat="server">
    <table class="myTable" style="width: 100%">
        <tr>
            <td style="text-align: right">
                申请单号：
            </td>
            <td style="text-align: left">
                <a href="#" style="color: Blue;" id="aAlert" onclick="openBillDetails();">
                    <asp:Label ID="lbsqcode" runat="server" Text=""></asp:Label></a>
            </td>
            <td style="text-align: right">
                部门：
            </td>
            <td style="text-align: left">
                <asp:Label ID="lbdeptcode" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <%--        <tr>
            <td style="text-align: right">
                车架号：
            </td>
            <td style="text-align: left">
                <asp:Label ID="lblcjh" runat="server" Text=""></asp:Label>
            </td>
            <td style="text-align: right">
                车辆类型：
            </td>
            <td style="text-align: left">
                <asp:Label ID="lbcartype" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                正常返利标准：
            </td>
            <td style="text-align: left">
                <asp:Label ID="lbybfee" runat="server" Text=""></asp:Label>
            </td>
            <td style="text-align: right">
                超出正常标准点数：
            </td>
            <td>
                <asp:Label ID="lblvesp" runat="server" Text=""></asp:Label>
            </td>
        </tr>--%>
        <tr>
            <td class=" billtitle" colspan="4" style="width: 100%">
                <div style="height: 540px; overflow: scroll">
                    <asp:DataGrid ID="DataGrid2" runat="server" CellPadding="3" CssClass="myGrid" Width="100%"
                        ItemStyle-HorizontalAlign="Center" AutoGenerateColumns="False" ScrollBars="Both">
                        <ItemStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundColumn DataField="deptName" HeaderText="部门" ItemStyle-HorizontalAlign="Center" >
                                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="truckCode" HeaderText="车架号" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="truckTypeName" HeaderText="车辆类型" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="yskmName" HeaderText="费用类别">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="tName" HeaderText="配置项/销售过程">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="StandardFee" HeaderText="标准金额">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="specialFee" HeaderText="提成费用">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr id="trpf" runat="server">
            <td colspan="4" style="text-align: center">
                <asp:Button ID="btpf" runat="server" Text="确认批复" OnClick="btpf_Click" CssClass="baseButton" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
