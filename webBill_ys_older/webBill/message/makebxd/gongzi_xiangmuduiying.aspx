<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gongzi_xiangmuduiying.aspx.cs"
    Inherits="webBill_makebxd_gongzi_xiangmuduiying" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>工资项目对应</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function openSelect(Code, obj) {
            var returnValue = window.showModalDialog('gongzi_xiangmuSelect.aspx?yskmCode=' + Code, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:630px;status:no;scroll:yes');
            if (returnValue != undefined) {
                $(obj).parent().children().eq(0).val(returnValue);
            }
        }
        function BackClick() {
            window.location.href = "gongzi_mingxi.aspx";
        }
    </script>

</head>
<body>
    <div>
        <input type="button" class="baseButton" value="返 回" id="btn_back" onclick="BackClick()" />
    </div>
    <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 900px; height: 390px;">
        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
            CssClass="myGrid"  Width="100%" 
            PageSize="17" OnItemDataBound="myGrid_ItemDataBound">
            <Columns>
                <asp:BoundColumn DataField="yskmCode" HeaderText="科目编号" HeaderStyle-Width="100" ItemStyle-Width="100">
                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="yskmMc" HeaderText="科目名称" HeaderStyle-Width="250" ItemStyle-Width="250">
                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="对应关系" HeaderStyle-Width="250" ItemStyle-Width="250">
                    <ItemTemplate>
                        <input id="txt_mingxi" type="text" class="baseText" runat="server" style="width: 65%" />
                        <input id="btnSetMx" type="button" value="设置" class="baseButton" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="False" CssClass="myGridItem" Width="100px" HorizontalAlign="Center" />
                </asp:TemplateColumn>
                <%-- <asp:BoundColumn DataField="kmStatus" HeaderText="状态">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>--%>
            </Columns>
            <PagerStyle Visible="False" />
        </asp:DataGrid>
        <input id="hf_yskmCode" type="hidden" runat="server" value="" />
    </div>
</body>
</html>
