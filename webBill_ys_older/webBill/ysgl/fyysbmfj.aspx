<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fyysbmfj.aspx.cs" Inherits="webBill_ysgl_fyysbmfj" %>

<%--<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>预算部门分解</title>
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
                <td>年度：<asp:DropDownList ID="drpSelectNd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpSelectNd_SelectedIndexChanged">
                </asp:DropDownList>
                    <asp:Button ID="btn_save" runat="server" CssClass="baseButton" Text="保存" OnClick="btn_save_Click" />
                    <asp:Label runat="server" ID="lb_zje" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divgrid" style="overflow-x: auto; margin-top: 5px;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" Width="600" PageSize="99999" ShowFooter="true" OnItemDataBound="myGrid_DataBinding">
                            <Columns>
                                <asp:BoundColumn DataField="deptcode" HeaderText="部门编号" HeaderStyle-Width="100" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" HorizontalAlign="Center" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="deptname" HeaderText="部门名称" HeaderStyle-Width="150" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bl" HeaderText="比例" HeaderStyle-Width="80" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="金额" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight">

                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_fjje" CssClass="myGridItemRight" Text='<%#Eval("xjje") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <%--<asp:BoundColumn DataField="xjje" HeaderText="金额" HeaderStyle-Width="160" ItemStyle-Width="160" DataFormatString="{0:N4}">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>--%>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <%--  <tr>
                <td style="height: 30px">
                    <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
                    </pager:UcfarPager>
                    <input type="hidden" runat="server" id="hdwindowheight" />
                </td>
            </tr>--%>
        </table>
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
