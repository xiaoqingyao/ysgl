<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cbzxList.aspx.cs" Inherits="webBill_cwgl_cbzxList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>成本中心</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        $(function() {
            initMainTableClass("<%=myGrid.ClientID%>");
        });
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:420px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("btn_sele").click();
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                <input type="button" value="增 加" class="baseButton" onclick="openDetail('cbzxDetail.aspx?type=add');" />
                &nbsp;<asp:Button ID="btn_edit" runat="server" Text="修 改" CssClass="baseButton" OnClick="btn_edit_Click" />
                &nbsp;<asp:Button ID="btn_del" runat="server" Text="删 除" CssClass="baseButton" OnClientClick="rreturn confirm('您要删除记录吗，是否继续？')"
                    OnClick="btn_del_Click" />
                &nbsp;<asp:TextBox ID="txb_where" runat="server"></asp:TextBox>
                <asp:Button ID="btn_sele" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_sele_Click" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Width="394px" CssClass="myGrid" AllowPaging="True" PageSize="17">
                    <Columns>
                        <asp:TemplateColumn HeaderText="选择">
                            <ItemTemplate>
                                &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                Width="38px" />
                            <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="zxCode" HeaderText="成本中心编号">
                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle CssClass="myGridItem"  Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="zxName" HeaderText="成本中心名称">
                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle CssClass="myGridItem"  Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle Visible="False" />
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="height: 30px">
                &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                </asp:DropDownList>
                页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                    runat="server"></asp:Label>条
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
