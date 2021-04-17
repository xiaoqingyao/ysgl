<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CaiwubuDuiYingRight.aspx.cs" Inherits="webBill_xtsz_CaiwubuDuiYingRight" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">

        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                $("#hf_user").val($(".highlight td:eq(0)").html());
            });
        });
        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
        $(function () {
            initWindowHW();
        });
    </script>

</head>
<div class="baseDiv">
    <body>
        <form id="form1" runat="server" onsubmit="initWindowHW();">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 30px">区域经费申请部门：<asp:Label runat="server" ID="lblCaiwu">未选择</asp:Label>

                    </td>
                </tr>
                <tr>
                    <td style="height: 30px">新增归集部门编号：<asp:TextBox runat="server" ID="deptCode"></asp:TextBox>
                        &nbsp;<asp:Button ID="btn_add" runat="server" Text="添 加" CssClass="baseButton"
                            OnClick="btn_add_Click" />
                         &nbsp;<asp:Button ID="btn_dele" runat="server" Text="删 除" CssClass="baseButton" OnClientClick="return confirm('是否确认删除？');"
                    OnClick="btn_dele_Click" />
                <asp:HiddenField ID="hf_user" runat="server" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divgrid" style="overflow-x: auto;">
                            <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                BorderWidth="1px" CssClass="myGrid" Width="600px">
                                <Columns>
                                    <asp:BoundColumn DataField="deptCode" HeaderText="部门编号" HeaderStyle-Width="100" ItemStyle-Width="100">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="deptName" HeaderText="部门名称" HeaderStyle-Width="250" ItemStyle-Width="250">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="true" />
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle Visible="False" />
                            </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
            <script type="text/javascript">
                parent.parent.closeAlert('UploadChoose');
            </script>
        </form>
    </body>
</div>
</html>
