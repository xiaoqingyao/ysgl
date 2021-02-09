<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fyqkListTable_new.aspx.cs"
    Inherits="webBill_search_fyqkListTable_new" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
        $(function() {
            gudingbiaotounew($("#myGrid"), $(window).height()-100);
            initMainTableClass("<%=myGrid.ClientID%>");
        });
        function openDetail(url,date, fykm, kssj, jzsj, deptCode,flowid,dydj,type) {
            window.showModalDialog(''+url+'?date=' + date + '&fykm=' + fykm + '&deptCode=' + deptCode + '&jzsj=' + jzsj + '&kssj=' + kssj+'&flowid='+flowid+'&dydj='+dydj+'&type='+type, 'newwindow', 'center:yes;dialogHeight:470px;dialogWidth:870px;status:no;scroll:yes');
        }
        function gudingbiaotou() {
            var t = document.getElementById("<%=myGrid.ClientID%>");
            var t2 = t.cloneNode(true);
            t2.id = "cloneGridView";
            for (i = t2.rows.length - 1; i > 0; i--) {
                t2.deleteRow(i);
            }
            t.deleteRow(0);
            header.appendChild(t2);
            var mainwidth = document.getElementById("main").style.width;
            mainwidth = mainwidth.substring(0, mainwidth.length - 2);
            mainwidth = mainwidth - 16;
            document.getElementById("header").style.width = mainwidth;
        }
        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
        function dy() {
            var kssj = '<%=Request["kssj"] %>';
            var jzsj = '<%=Request["jzsj"] %>';
            var deptcode = '<%=Request["deptcode"] %>';
            var type = '<%=Request["type"] %>';
            window.location.href = "fyqkListTable_new_dy.aspx?kssj=" + kssj + "&jzsj=" + jzsj + "&deptcode=" + deptcode + "&type=" + type;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 25px">
                <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>&nbsp;
                <asp:Button ID="Button1" runat="server" CssClass="baseButton" OnClick="Button1_Click"
                    Text="返 回" />
                    <input type="button" class="baseButton" value="打印预览" onclick="dy();" />
                <asp:Button ID="Button2" runat="server" Text="导出Excel" CssClass="baseButton" OnClick="Button2_Click" />
                <label style="color: Red">
                    【友情提示】：此报表中的预算金额=年初预算金额+预算内追加金额</label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <div style="position: relative; word-warp: break-word; word-break: break-all">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" PageSize="17" Width="1300"  OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                            <asp:BoundColumn DataField="yskmCode" HeaderText="编号"  ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="yskmMc" HeaderText="名称"  ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="je" HeaderText="预算金额" DataFormatString="{0:N2}"  ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="bxje" HeaderText="决算金额[查]" DataFormatString="{0:N2}" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="zjje" DataFormatString="{0:N2}" HeaderText="追加金额[查]" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="tzcje" DataFormatString="{0:N2}" HeaderText="调整出金额[查]" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="tzrje" DataFormatString="{0:N2}" HeaderText="调整入金额[查]" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="jyje" DataFormatString="{0:N2}" HeaderText="结余金额" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Width="120px" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Width="120px" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="nysje" DataFormatString="{0:N2}" HeaderText="年度预算" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="nzjje" DataFormatString="{0:N2}" HeaderText="年追加" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="nbxje" DataFormatString="{0:N2}" HeaderText="年决算" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="njyje" DataFormatString="{0:N2}" HeaderText="年结余" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr style="display: none;">
            <td align="left">
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
