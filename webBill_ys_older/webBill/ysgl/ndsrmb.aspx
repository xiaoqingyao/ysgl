<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ndsrmb.aspx.cs" Inherits="webBill_ysgl_ndsrmb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>年度收入目标</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
        $(function () {
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[0] != null) {
                    var billCode = $(this).find("td")[0].innerHTML;
                    $("#choosebill").val(billCode);
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" style="margin-left: 5px; margin-top: 5px">
            <tr>
                <td>
                    <table class="baseTable" style="width: 400px;">
                        <tr>
                            <td>财年：</td>
                            <td>
                                <asp:DropDownList ID="drpSelectNd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpSelectNd_SelectedIndexChanged">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>总目标：</td>
                            <td>
                                <asp:TextBox ID="txtje" Width="150" runat="server" CssClass="baseText"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btn_sz" runat="server" Text="保存设置" CssClass="baseButton" OnClick="btn_sz_Click" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="display: none">
                    <div id="divgrid" style="overflow-x: auto; margin-top: 5px">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" Width="400" PageSize="99999">
                            <Columns>
                                <asp:BoundColumn DataField="nd" HeaderText="年度">
                                    <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="myGridItem hiddenbill" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="nd" HeaderText="年度" HeaderStyle-Width="80" ItemStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemCenter" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="zje" HeaderText="年度目标" HeaderStyle-Width="100" ItemStyle-Width="100" DataFormatString="{0:N4}">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        parent.closeAlert('UploadChoose');
    </script>
</body>
</html>
