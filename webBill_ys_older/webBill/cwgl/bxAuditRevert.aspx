<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bxAuditRevert.aspx.cs" Inherits="webBill_cwgl_bxAuditRevert" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<%@ Register Assembly="PaginationControl" Namespace="PaginationControl" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报销单审核驳回</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            initWindowHW();
            initMainTableClass("<%=GridView1.ClientID%>");
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
              
            });
            $("#btn_Rever").click(function() {
                var checkrow = $(".highlight").find('td')[0].innerHTML;
                if (checkrow == "") {
                    alert("请先选中行！");
                } else {
                    $("#hdCode").val(checkrow);
                }
            });
            //查看
            $("#btn_View").click(function() {
                var billcode = $(".highlight td:eq(0)").html();
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                }
                else {
                    $.post("../MyWorkFlow/GetBillType.ashx", { "billcode": billcode }, function(data, status) {
                        if (status == "success") {
                            window.showModalDialog(data, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
                        }
                    });
                }
            });
            $("#btn_refresh").click(function() {
                location.replace(location.href);
            });
        });
        function gudingbiaotou() {
            var t = document.getElementById("<%=GridView1.ClientID%>");
            if (t == null || t.rows.length < 1) {
                return;
            }
            var t2 = t.cloneNode(true);
            t2.id = "cloneGridView";
            for (i = t2.rows.length - 1; i > 0; i--) {
                t2.deleteRow(i);
            }
            t.deleteRow(0);
            header.appendChild(t2);
            var mainwidth = document.getElementById("divgrid").style.width;
            mainwidth = mainwidth.substring(0, mainwidth.length - 2);
            mainwidth = mainwidth - 16;
            document.getElementById("header").style.width = mainwidth;
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
    <div class="baseDiv" style="margin-top: 5px;">
        <input id="btn_refresh" type="button" value="刷 新" class="baseButton" />&nbsp; 单据编号：<asp:TextBox
            ID="txtBillCode" runat="server" CssClass="baseText"></asp:TextBox>&nbsp;<asp:Button
                ID="btn_Select" CssClass="baseButton" runat="server" Text="查 询" OnClick="btn_Select_Click" />&nbsp;
        <input id="btn_View" type="button" value="详细信息" class="baseButton" />&nbsp;
        <asp:Button ID="btn_Rever" runat="server" Text="驳回审核" ToolTip="驳回到未提交状态" OnClick="btn_Rever_Click"
            CssClass="baseButton" />
        <input type="button" class="baseButton" value="帮助" onclick="javascript:parent.helptoggle();" />
    </div>
    <div id="header" style="overflow: hidden; padding-top: 5px;">
    </div>
    <div id="divgrid" class="baseDiv" style="overflow-y: scroll; overflow-x: auto; margin-top: -1px;">
        <asp:GridView ID="GridView1" runat="server" Style="table-layout: fixed" Width="1900"
            AutoGenerateColumns="False" CssClass="myGrid">
            <%--OnRowDataBound="GridView1_RowDataBound"--%>
            <Columns>
                <asp:BoundField DataField="billcode" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill">
                    <HeaderStyle CssClass="hiddenbill"></HeaderStyle>
                    <ItemStyle CssClass="hiddenbill"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="billname" HeaderText="单据编号" ItemStyle-CssClass="myGridItem">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                </asp:BoundField>
                <asp:BoundField HeaderText="单据类型" ItemStyle-CssClass="myGridItem" DataField="billTypeName">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                </asp:BoundField>
                <asp:BoundField HeaderText="制单人" ItemStyle-CssClass="myGridItem" DataField="billuserName">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                </asp:BoundField>
                <asp:BoundField HeaderText="制单时间" ItemStyle-CssClass="myGridItem" DataField="billdate"
                    DataFormatString="{0:d}">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                </asp:BoundField>
                <asp:BoundField HeaderText="备注" ItemStyle-CssClass="myGridItem" DataField="bxzy">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                </asp:BoundField>
                <asp:BoundField HeaderText="摘要说明" ItemStyle-CssClass="myGridItem" DataField="bxsm">
                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                    <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                </asp:BoundField>
            </Columns>
            <HeaderStyle CssClass="myGridHeader" />
        </asp:GridView>
    </div>
    <div style="height: 30px">
        <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
        </pager:UcfarPager>
        <input type="hidden" runat="server" id="hdwindowheight" />
    </div>
    <div>
        <asp:HiddenField ID="hdCode" runat="server" />
    </div>
    </form>
</body>
</html>
