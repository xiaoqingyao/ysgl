<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuList.aspx.cs" Inherits="webBill_sysMenu_MenuList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript" language="javascript">
        $(function() {
        $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                parent.helptoggle();
                }
            });
        gudingbiaotounew($("#myGrid"), 380);
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
            $("#Update").click(function() {
                var checkRow = $(".highlight");
                var id = checkRow.find('td')[0].innerHTML;
                var url = 'MenuDetails.aspx?id=' + id;
                var returnValue = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:480px;status:no;scroll:no');
                if (returnValue == undefined || returnValue == "failed") {
                    return false;
                }
                else {
                    document.getElementById("btn_select").click();
                }
            });
        });
        
    function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                <asp:TextBox ID="txtWhere" runat="server"></asp:TextBox>&nbsp;&nbsp;
                <asp:Button ID="btn_select" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_sele_Click" />&nbsp;
                <input id="Update" type="button" value="修改" class="baseButton" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <%-- <asp:GridView EmptyDataText="暂时没有数据"  ID="" runat="server" CssClass = "baseTable"
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="3" Font-Size="9pt"
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                    DataKeyNames="menuid" PageSize="17">
                    <Columns>
                        <asp:BoundField DataField="" HeaderText="" />
                        <asp:BoundField DataField="" HeaderText="" />
                        <asp:BoundField DataField="" HeaderText="" />
                    </Columns>
                </asp:GridView>--%>
                <div style="position: relative; word-warp: break-word; word-break: break-all">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3">
                        <Columns>
                            <asp:BoundColumn DataField="menuid" HeaderText="菜单ID" HeaderStyle-Width="200" ItemStyle-Width="200">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="menuName" HeaderText="菜单名称" HeaderStyle-Width="200" ItemStyle-Width="200">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="showName" HeaderText="显示名称（自定义）" HeaderStyle-Width="200" ItemStyle-Width="200">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
    </table> 
        <script type="text/javascript">
                 parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
