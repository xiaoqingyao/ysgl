<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deptYsTjResult.aspx.cs" Inherits="webBill_tjbb_deptYsTjResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>����Ԥ��ͳ��</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        $(function () {
            gudingbiaotounew($("#myGrid"), $(window).height()-100);
            initMainTableClass("<%=myGrid.ClientID%>");
        });
        function openDetail(url, date, dept, kssj, jzsj, deptCode, stepid, flowid,dydj,type) {
           
            window.showModalDialog(' ' + url + '?date=' + date + '&fykm=' + dept + '&deptCode=' + deptCode + '&jzsj=' + jzsj + '&kssj=' + kssj + '&stepid=' + stepid + '&flowid=' + flowid + '&dydj=' + dydj + '&type=' + type, 'newwindow', 'center:yes;dialogHeight:470px;dialogWidth:870px;status:no;scroll:yes');
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
            obj.css("margin", "-1px 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
        function dy() {
            var kssj = '<%=Request["kssj"] %>';
            var jzsj = '<%=Request["jzsj"] %>';
            var deptcode = '<%=Request["deptcode"] %>';
            var type = '<%=Request["type"] %>';
            window.location.href = "deptYsTjResult_dy.aspx?kssj=" + kssj + "&jzsj=" + jzsj + "&deptcode=" + deptcode + "&djzt=" + type;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 25px">

                    <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>&nbsp;<asp:Button
                        ID="Button1" runat="server" CssClass="baseButton" OnClick="Button1_Click" Text="�� ��" />
                    <input type="button" class="baseButton" value="��ӡԤ��" onclick="dy();" />
                    <asp:Button ID="Button2" runat="server" CssClass="baseButton" Text="����Excel" OnClick="Button2_Click" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div style="position: relative; word-warp: break-word; word-break: break-all">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" PageSize="17" OnItemDataBound="myGrid_ItemDataBound" Width="1200px">
                            <Columns>
                                <asp:BoundColumn DataField="bmbh" HeaderText="���ű��" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bmmc" HeaderText="��������" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bndys" HeaderText="�����Ԥ��" DataFormatString="{0:N2}" ItemStyle-Width="100"
                                    HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bqys" HeaderText="����Ԥ��[��]" DataFormatString="{0:N2}" ItemStyle-Width="100"
                                    HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bqjsje" HeaderText="���ھ�����[��]" DataFormatString="{0:N2}"
                                    ItemStyle-Width="120" HeaderStyle-Width="120">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bndyssyje" HeaderText="�����Ԥ��ʣ����" DataFormatString="{0:N2}"
                                    ItemStyle-Width="110" HeaderStyle-Width="110">
                                    <HeaderStyle CssClass="myGridHeader" Width="120px" Font-Bold="True" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="true" />
                                    <ItemStyle CssClass="myGridItemRight" Width="120px" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="dqjssyje" HeaderText="���ھ���ʣ����" DataFormatString="{0:N2}"
                                    ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Width="120px" Font-Bold="True" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="true" />
                                    <ItemStyle CssClass="myGridItemRight" Width="120px" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="sndys" HeaderText="�����Ԥ��" DataFormatString="{0:N2}" ItemStyle-Width="100"
                                    HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="sndbqys" HeaderText="����ȱ���Ԥ����" DataFormatString="{0:N2}"
                                    ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Width="120px" Font-Bold="True" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="true" />
                                    <ItemStyle CssClass="myGridItemRight" Width="120px" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="sndbqjsje" HeaderText="����ȱ��ھ�����" DataFormatString="{0:N2}"
                                    ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Width="120px" Font-Bold="True" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="true" />
                                    <ItemStyle CssClass="myGridItemRight" Width="120px" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bmbh" HeaderText="���ű��" Visible="False">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="date" HeaderText="ͳ��ʱ��" DataFormatString="{0:d}" Visible="False">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
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
