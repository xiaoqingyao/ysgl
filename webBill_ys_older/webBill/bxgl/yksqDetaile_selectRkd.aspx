<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yksqDetaile_selectRkd.aspx.cs"
    Inherits="webBill_bxgl_yksqDetaile_selectRkd" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用款审批单 选择入库单（可以多选）</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">  
        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
  
          function SelectAll(aControl) {
            var chk = document.getElementById("<%=myGrid.ClientID%>").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
            }
        }

        function SingleSelect(aControl) {
            if ("<%=type%>" == "s") {
                var chk = document.getElementById("<%=myGrid.ClientID%>").getElementsByTagName("input");
                for (var s = 0; s < chk.length; s++) {
                    chk[s].checked = false;
                }
                aControl.checked = true;
            }
        }

        $(function () {
        
         $("#txtLoanDateFrm").datepicker();
            $("#txtLoanDateTo").datepicker();
            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");
            
            
            if ("<%=type%>" == "s") {
                var td = $("#<%=myGrid.ClientID%> tr:eq(0)").find("th:eq(1)");
                td.html("");
                td.html("选择");
                $("#Muti").hide();
            }

            $("#btn_cancel").click(function () {
                parent.WindowClose()
            });

            ChangeHeight();
        });


        function ChangeHeight()
        {
           
            $("#<%=Label1.ClientID%>").height($("#<%=Label1.ClientID%>")[0].scrollHeight);
            if ($("#<%=Label1.ClientID%>").height() < 20) {
                $("#<%=Label1.ClientID%>").height(20)
            }
        }

    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="margin: 5px; text-align: left;">
                <div id="Muti" style="margin: 5px;">
                    <div style="margin-bottom: 5px;">
                        已选总金额：
                        <asp:TextBox ID="txtje" runat="server" Width="50%"></asp:TextBox>
                         <asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="清除选择" OnClick="Button1_Click" />
                    </div>
                    已选择单号：
                    <asp:TextBox ID="Label1" runat="server" TextMode="MultiLine" Width="80%" Height="20"></asp:TextBox>
                </div>
                <table class="baseTable" style="margin-left: 0px;">
                    <tr>
                        <td>
                            单号:
                        </td>
                        <td>
                            <input type="text" class="baseInput" runat="server" id="txtCode" />
                        </td>
                        <td>
                            制单时间从:
                        </td>
                        <td>
                            <input type="text" class="baseInput" runat="server" id="txtLoanDateFrm" />
                        </td>
                        <td>
                            到:
                        </td>
                        <td>
                            <input type="text" class="baseInput" runat="server" id="txtLoanDateTo" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btn_query" CssClass="baseButton" Text="查 询" OnClick="btn_query_Click" />
                        </td>
                        
                        <td>
                            <asp:Button ID="btn_SaveChoose" runat="server" CssClass="baseButton" Text="确 定" OnClick="btn_SaveChoose_Click" />
                        </td>
                        <td style="display: none;">
                            <input id="btn_cancel" type="button" value="取 消" class="baseButton" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divgrid" style="overflow-x: auto;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid">
                        <PagerStyle Visible="False" />
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="false" CssClass="myGridItem" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                        Text="" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckDept" runat="server" onclick="javascript:SingleSelect(this);" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="ID" HeaderText="编号" HeaderStyle-Width="150" ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="je" HeaderText="单据金额" HeaderStyle-Width="150" ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="false" CssClass="myGridItem hiddenbill" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cBusType" HeaderText="业务类型" HeaderStyle-Width="150" ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cSource" HeaderText="单据来源" HeaderStyle-Width="150" ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="je" HeaderText="单据金额" DataFormatString="{0:N}" HeaderStyle-Width="150"
                                ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="dDate" HeaderText="制单时间" HeaderStyle-Width="150" ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cMaker" HeaderText="制单人" DataFormatString="{0:F2}" HeaderStyle-Width="150"
                                ItemStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="False" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                        </Columns>
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
