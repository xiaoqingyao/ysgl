<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ysList.aspx.cs" Inherits="webBill_search_ysList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.2.4a.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        $(function() {
            initMainTableClass("<%=myGrid.ClientID%>");
            initWindowHW();
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });

            $("#txtKm").autocomplete({
                source: arrFyKm,
                select: function(event, ui) {
                    $("#txtKm").val(ui.item.value);
                }
            });
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                var billCode = "";
                if ($(this).find("td")[1] != null) {
                    billCode = $(this).find("td")[1].innerHTML;
                    $("#hd_billCode").val(billCode);
                }

                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
        });
        function openDetail(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:560px;status:no;scroll:yes');
        }
        function gudingbiaotou() {
            var t = document.getElementById("<%=myGrid.ClientID%>");
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
            var mainwidth = document.getElementById("main").style.width;
            mainwidth = mainwidth.substring(0, mainwidth.length - 2);
            mainwidth = mainwidth - 16;
            document.getElementById("header").style.width = mainwidth;
        }

        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
       


    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 30px">
                <asp:CheckBox ID="chkNextLevel" runat="server" AutoPostBack="True" OnCheckedChanged="chkNextLevel_CheckedChanged"
                    Text="包含下级单位" Checked="True" />
                <asp:DropDownList ID="ddlDate" runat="server" OnSelectedIndexChanged="ddlDate_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
                &nbsp;<asp:Button ID="Button3" runat="server" Text="详细信息" CssClass="baseButton" OnClick="Button3_Click1" />
                
                <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
                <asp:Label ID="lblDept" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 30px"> 
                &nbsp;预算科目：<asp:TextBox ID="txtKm" runat="server"></asp:TextBox>
                <asp:Button ID="btn_Select" runat="server" CssClass="baseButton" Text="根据科目统计" OnClick="btn_CX_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <div id="divgrid" style="overflow-x: auto;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        Width="700px" CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                            <asp:TemplateColumn HeaderText="选择">
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                    Width="38px" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="False" CssClass="myGridItem" Width="38" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="billCode" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billNameCode" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="billName" HeaderText="预算过程" ItemStyle-Width="150" HeaderStyle-Width="150">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billUser" HeaderText="制单人" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" DataFormatString="{0:D}" HeaderText="制单日期"
                                ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billJe" DataFormatString="{0:F2}" HeaderText="预算金额" ItemStyle-Width="100"
                                HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepID_ID" HeaderText="审核状态" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepID_ID" Visible="False"></asp:BoundColumn>
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
        <tr>
            <asp:HiddenField ID="hd_billCode" runat="server" />
            <td id="wf">
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
