<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ybbxSelect.aspx.cs" Inherits="Phone_ybbxSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>一般报销查询</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                if ($(this).find("td")[0] != null) {
                    var billCode = $(this).find("td")[0].innerHTML;
                }
            });
            
        });
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("Button2").click();
            }
        }
       
    </script>

</head>
<body">
    <form id="form1" runat="server">
    <div style="float: left">
        <table class="baseTable" style="text-align: left;">
            <tr>
                <td>
                    申请日期从：
                </td>
                <td>
                    <asp:TextBox ID="txtLoanDateFrm" runat="server" Width="120px"></asp:TextBox>
                </td>
                <td>
                    到：
                </td>
                <td>
                    <asp:TextBox ID="txtLoanDateTo" runat="server" Width="120px"></asp:TextBox>
                </td>
                <td>
                    单据编号：
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="Button2" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
            CssClass="myGrid" Style="table-layout: fixed" Width="90%" AllowPaging="True"
            PageSize="10" OnItemDataBound="myGrid_ItemDataBound">
            <Columns>
                <asp:BoundColumn DataField="billCode" HeaderText="报销单号">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="true" CssClass="myGridItem hiddenbill" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="billName" HeaderText="单据编号">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Width="120" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Width="120" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="billUserName" HeaderText="报销人">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="billDate" HeaderText="报销申请日期" DataFormatString="{0:D}">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Width="150" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                        CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Width="150" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="billDept" HeaderText="所属部门" DataFormatString="{0:D}">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="billJe" HeaderText="报销总额" DataFormatString="{0:F2}">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="bxzy" HeaderText="摘要">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="审批状态" DataField="stepid">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="isgk" HeaderText="归口费用">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="gkDept" HeaderText="归口部门">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                </asp:BoundColumn>
            </Columns>
            <PagerStyle Visible="False" />
        </asp:DataGrid>
    </div>
    <div>
       &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                </asp:DropDownList>
                页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                    runat="server"></asp:Label>条
    </div>
    <div>
        审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
        <asp:HiddenField ID="hdYbbxNeedAudit" runat="server" />
    </div>
    </form>
</body>
</html>
