<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fyqkListTable_new.aspx.cs"
    Inherits="webBill_tjbb_dz_fyqkListTable_new" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
        $(function () {
            gudingbiaotounew($("#myGrid"), $(window).height() - 200);
            initMainTableClass("<%=myGrid.ClientID%>");
        });
        function openDetail(url, date, fykm, kssj, jzsj, deptCode, flowid, dydj, type) {
            window.showModalDialog('' + url + '?date=' + date + '&fykm=' + fykm + '&deptCode=' + deptCode + '&jzsj=' + jzsj + '&kssj=' + kssj + '&flowid=' + flowid + '&dydj=' + dydj + '&type=' + type, 'newwindow', 'center:yes;dialogHeight:470px;dialogWidth:870px;status:no;scroll:yes');
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

    </script>
    <style type="text/css">
        .heji {
            background-color:#6293BB;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%" >
            <tr>
                <td style="height: 25px">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>&nbsp;
                <asp:Button ID="Button1" runat="server" CssClass="baseButton" OnClick="Button1_Click"
                    Text="返 回" />

                    <asp:Button ID="Button2" runat="server" Text="导出Excel" CssClass="baseButton" OnClick="Button2_Click" />
                    <label style="color: Red">
                        【友情提示】：此报表中的预算金额=年初预算金额+预算内追加金额</label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div style="position: relative; word-warp: break-word; word-break: break-all">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" Width="1700" OnItemDataBound="myGrid_ItemDataBound">
                            <Columns>
                                <asp:BoundColumn DataField="yskmCode" HeaderText="编号" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yskmMc" HeaderText="名称" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="je" HeaderText="年初预算" DataFormatString="{0:N2}" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                  <asp:BoundColumn DataField="bqys" HeaderText="本期预算" DataFormatString="{0:N2}" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bxje" HeaderText="本期决算" DataFormatString="{0:N2}" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                 <asp:BoundColumn DataField="yszxl" HeaderText="预算执行率" DataFormatString="{0:N2}" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="zysyje" HeaderText="本期占用金额" DataFormatString="{0:N2}" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="zjje" DataFormatString="{0:N2}" HeaderText="追加预算" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="tzcje" DataFormatString="{0:N2}" HeaderText="预算调整出" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="tzrje" DataFormatString="{0:N2}" HeaderText="预算调整入" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="jyje" DataFormatString="{0:N2}" HeaderText="本期结余(差异)" ItemStyle-Width="100" HeaderStyle-Width="100">
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
                                <asp:BoundColumn DataField="nbxje" DataFormatString="{0:N2}" HeaderText="年度决算" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <%--<asp:BoundColumn DataField="nzyje" HeaderText="年占用金额" DataFormatString="{0:N2}" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>--%>
                                <asp:BoundColumn DataField="nzysyje" DataFormatString="{0:N2}" HeaderText="年占用金额" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="njyje" DataFormatString="{0:N2}" HeaderText="年结余(差异)" ItemStyle-Width="100" HeaderStyle-Width="100">
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
            <tr>
                <td>
                    <div>
                        报表说明：
                      
                        <ul>
                            <%--  <li>本年度预算：财年年度预算+项目年度预算</li> 本期预算: 查询条件的时间段内财年月份的预算额度+项目预算合计--%>
                            <li>本期预算： 年初预算+调整入+追加-调整出 </li> 
                            <li>本期决算：查询条件的时间段内的决算金额合计，决算金额以新财年费用报销单为准</li>
                            <li>追加预算：查询条件的时间段内的追加金额合计</li>
                            <li>预算调整出：查询条件的时间段内调整出金额合计</li>
                            <li>预算调整入：查询条件的时间段内调整入的金额合计</li>
                            <li>本期结余(差异)：查询条件时间段内本期预算-本期决算金额
                            </li>
                            <li>年度预算：财年年度预算+项目年度预算</li>
                            <li>年追加：财年年度追加总金额
                            </li>
                            <li>年度决算：财年年度的决算金额合计，决算金额以新财年费用报销单为准
                            </li>
                            <li>年结余(差异)：本年度预算-本年度决算</li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
