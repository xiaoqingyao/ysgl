<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PositionList.aspx.cs" Inherits="PositionList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
        });
        function cancle() {
            window.returnValue = "";
            window.close();
        }
        function sureCheck() {
            var checkrow = $(".highlight");
            try{
                var code = checkrow.find('td')[0].innerHTML;
                var name = checkrow.find('td')[1].innerHTML;
                window.returnValue = "[" + code + "]" + name; window.close();
            }catch(e){
                alert("请先选中行！");
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                &nbsp;&nbsp;<input type="button" id="btn_select" onclick="sureCheck();" value="确定选择"  class="baseButton" onclick="sureCheck();"/>
                &nbsp;
                <input type="button" id="Button1" onclick="cancle();" value="关 闭"  class="baseButton" onclick="cancle();"/>
                
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="myGrid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CellPadding="3" CssClass="myGrid" PageSize="8" Width="612px">
                    <Columns>
                        <%--<asp:TemplateColumn HeaderText="选择">
                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Width="35px"
                                Wrap="False" />
                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            <ItemTemplate>
                                &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" onclick="alert();"/>
                            </ItemTemplate>
                        </asp:TemplateColumn>--%>
                        <asp:BoundColumn DataField="dicCode" HeaderText="职务编号">
                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="dicName" HeaderText="名称">
                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle Visible="False" />
                </asp:DataGrid>&nbsp;
            </td>
        </tr>
        <tr>
            <td style="height: 30px">
                &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton><asp:LinkButton
                    ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton><asp:LinkButton
                        ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton><asp:LinkButton
                            ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>第<asp:DropDownList
                                ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
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
