<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ybbxCx.aspx.cs" Inherits="webBill_cwgl_ybbxCx" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>一般报销给付撤销</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
        function openDetail(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
        }
        function openLookSpStep(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }


        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            //  gudingbiaotounew($("#myGrid"), 380);
            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
              
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
        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                <asp:Button ID="Button1" runat="server" Text="撤销给付" CssClass="baseButton" OnClientClick="return confirm('给付后不能恢复，是否继续？');"
                    OnClick="Button1_Click" />&nbsp;
                <asp:Button ID="Button2" runat="server" CssClass="baseButton" OnClick="Button2_Click"
                    Text="审核过程" />
                <asp:Button ID="Button3" runat="server" Text="详细信息" CssClass="baseButton" OnClick="Button3_Click1" />&nbsp;<asp:TextBox
                    ID="TextBox1" runat="server"></asp:TextBox>
                <asp:Button ID="Button4" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button4_Click" />
                <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
            </td>
        </tr>
        <tr>
            <td>
                <div style="position: relative; overflow-x: auto; word-warp: break-word; word-break: break-all">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        Width="1250px" CssClass="myGrid" AllowPaging="false">
                        <Columns>
                            <asp:TemplateColumn HeaderText="选择" HeaderStyle-Width="40" ItemStyle-Width="40">
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="billCode" HeaderText="报销单号" Visible='False'>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billUser" HeaderText="报销人" HeaderStyle-Width="120" ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                    Width="65" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" Width="65" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" HeaderText="报销申请日期" DataFormatString="{0:D}"
                                HeaderStyle-Width="120" ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDept" HeaderText="所属部门" DataFormatString="{0:D}"
                                HeaderStyle-Width="120" ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billJe" HeaderText="报销总额" DataFormatString="{0:N2}" HeaderStyle-Width="120"
                                ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepID" HeaderText="状态" HeaderStyle-Width="120" ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepID_ID" HeaderText="status" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="sfgf" HeaderText="是否给付" HeaderStyle-Width="120" ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                    Width="70" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" Width="70" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="gfr" HeaderText="给付人" HeaderStyle-Width="120" ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="gfsj" HeaderText="给付时间" HeaderStyle-Width="120" ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="撤销原因" HeaderStyle-Width="120" ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Wrap="true" Width="100px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="billName" HeaderText="单据编号" HeaderStyle-Width="120" ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 30px">
                <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
                </pager:UcfarPager>
                <input type="hidden" runat="server" id="hdwindowheight" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
