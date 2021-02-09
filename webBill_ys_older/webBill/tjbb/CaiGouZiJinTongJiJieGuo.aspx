<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CaiGouZiJinTongJiJieGuo.aspx.cs" Inherits="webBill_tjbb_CaiGouZiJinTongJiJieGuo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>采购资金统计</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:650px;dialogWidth:900px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                location.replace(location.href);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
       <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                采购资金统计
                <asp:Label ID="lblfee" runat="server" ForeColor="Red"></asp:Label>
                <asp:Button ID="btnReturn" runat="server" Text=" 返 回 " CssClass="baseButton" OnClick="btnReturn_Click" />
                <asp:Button ID="btnExport" runat="server" CssClass="baseButton" Text="导出Excel" OnClick="btnExport_Click" Visible="false" />
            </td>
        </tr>
        <tr>
            <td>
                <div id="header" >
                </div>
                <div id="main" style=" overflow-y:scroll; margin-top:-1px; width:900px; height:400px;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" PageSize="17" onitemdatabound="myGrid_ItemDataBound" Style="table-layout: fixed" Width="100%">
                        <Columns>
                            <asp:BoundColumn DataField="gysbh" HeaderText="" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill"/>
                            <asp:BoundColumn DataField="gysmc" HeaderText="供应商[查]" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False"  CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true"  CssClass="myGridItemRight"/>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="totalje" HeaderText="支付总额" DataFormatString="{0:N2}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
