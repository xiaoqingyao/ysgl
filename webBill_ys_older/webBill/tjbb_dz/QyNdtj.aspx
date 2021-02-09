<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QyNdtj.aspx.cs" Inherits="webBill_tjbb_dz_QyNdtj" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>职能部门占预算</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />

    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script>
        $(function () {

            $("#<%=DataGrid1.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                $("#<%=DataGrid1.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });

            //GridView1.colums[0].Frozen = true;
            // gudingbiaotounew($("#GridView1"), $(window).height()-180 );
        });

    </script>
    <style type="text/css">
        .fixCol {
            LEFT: expression(this.offsetParent.scrollLeft);
            POSITION: relative;
        }
    </style>

</head>
<body>
    <form id="form2" runat="server">
        <table style="width: 90%">
            <tr>
                <td>财年：<asp:DropDownList ID="drpSelectNd" runat="server">
                </asp:DropDownList>
                    开始月份
                    <asp:DropDownList ID="bgintime" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="TextBox2" runat="server" Visible="false"></asp:TextBox>
                    <asp:DropDownList ID="endtime" runat="server"></asp:DropDownList>
                    <asp:Button ID="Button1" runat="server" CssClass="baseButton" OnClick="Button1_Click"
                        Text="生成统计表" />
                    <asp:Button runat="server" ID="export" Text="导出excel" OnClick="btn_excel_Click" CssClass="baseButton" />
                </td>
            </tr>
            <tr>
                <td>

                    <asp:DataGrid ID="DataGrid1" runat="server" CssClass="myGrid" UseAccessibleHeader="True" AutoGenerateColumns="False"
                        OnItemDataBound="GridView1_ItemDataBound">
                        <Columns>
                            <asp:BoundColumn DataField="部门类别" HeaderText="部门类别" HeaderStyle-Width="80" ItemStyle-Width="80">
                                <HeaderStyle CssClass="fixCol"></HeaderStyle>
                                <ItemStyle CssClass="fixCol"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="集团预算" HeaderText="集团预算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="集团决算" HeaderText="集团决算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="青岛预算" HeaderText="青岛预算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="青岛决算" HeaderText="青岛决算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="淄博预算" HeaderText="淄博预算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="淄博决算" HeaderText="淄博决算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="泰安预算" HeaderText="泰安预算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="泰安决算" HeaderText="泰安决算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="济宁预算" HeaderText="济宁预算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" HorizontalAlign="Right" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="济宁决算" HeaderText="济宁决算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" HorizontalAlign="Right" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>

                            <asp:BoundColumn DataField="临沂预算" HeaderText="临沂预算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" HorizontalAlign="Right" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="临沂决算" HeaderText="临沂决算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" HorizontalAlign="Right" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>

                            <asp:BoundColumn DataField="济南预算" HeaderText="济南预算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" HorizontalAlign="Right" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="济南决算" HeaderText="济南决算" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" HorizontalAlign="Right" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>

                            <asp:BoundColumn DataField="预算合计" HeaderText="预算合计" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="决算合计" HeaderText="决算合计" HeaderStyle-Width="150" ItemStyle-Width="150" DataFormatString="{0:N2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>

                        </Columns>
                    </asp:DataGrid>

                </td>
            </tr>
        </table>
    </form>
</body>
</html>
