<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yszjList.aspx.cs" Inherits="webBill_search_yszjList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">
        $(function() {
        $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
        initMainTableClass("<%=myGrid.ClientID%>");
        initWindowHW();

        $("#txtDateFrm").datepicker();
        $("#txtDateTo").datepicker();

            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                var billCode = "";
                if ($(this).find("td")[1] != null) {
                    billCode = $(this).find("td")[1].innerHTML;
                }
                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            $("#txtKm").autocomplete({
                source: arrFyKm,
                select: function(event, ui) {
                    $("#txtKm").val(ui.item.value);
                }
            });
            //报销人自动加载
            $("#txtBxr").autocomplete({
                source: availableTags
            });
            //科目自动加载
            $("#txtSubject").autocomplete({
                source: subjectTags
            });
            //查询
            $("#btnSelect").click(function() {
                var status = document.getElementById("trSelect").style.display;
                document.getElementById("trSelect").style.display = status == "block" ? "none" : "block";
            });
            //取消
            $("#btn_cancle").click(function() {
                document.getElementById("trSelect").style.display = "none";
            });

        });
        
        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
        function openDetail(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:550px;status:no;scroll:yew');
        }
        //        function gudingbiaotou() {
        //            var t = document.getElementById("<%=myGrid.ClientID%>");
        //            var t2 = t.cloneNode(true);
        //            t2.id = "cloneGridView";
        //            for (i = t2.rows.length - 1; i > 0; i--) {
        //                t2.deleteRow(i);
        //            }
        //            t.deleteRow(0);
        //            header.appendChild(t2);
        //            var mainwidth = document.getElementById("main").style.width;
        //            mainwidth = mainwidth.substring(0, mainwidth.length - 2);
        //            mainwidth = mainwidth - 16;
        //            document.getElementById("header").style.width = mainwidth;
        //        }
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
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 30px">
                <asp:CheckBox ID="chkNextLevel" runat="server" AutoPostBack="True" OnCheckedChanged="chkNextLevel_CheckedChanged"
                    Text="包含下级单位" Checked="True" />
                <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                <asp:Button ID="Button3" runat="server" Text="详细信息" CssClass="baseButton" OnClick="Button3_Click" />
                  <asp:Button ID="Button2" runat="server" CssClass="baseButton" Text="导出Excel" OnClick="Button2_Click" />
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
        <tr id="trSelect" style="display: none; float: left; width: 900px">
            <td align="left">
                <table class="baseTable" style="text-align: left;">
                    <tr>
                        <td>
                            日期从：
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateFrm" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            到：
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateTo" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            报销人：
                        </td>
                        <td>
                            <asp:TextBox ID="txtBxr" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            审核状态：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem Value="">--全部--</asp:ListItem>
                                <asp:ListItem Value="-1">未提交</asp:ListItem>
                                <asp:ListItem Value="1">审批中</asp:ListItem>
                                <asp:ListItem Value="2">审核通过</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            科目：
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            单据编号：
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillCode" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            单据来源：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBillFrm" runat="server">
                                <asp:ListItem Value="">--全部--</asp:ListItem>
                                <asp:ListItem Value="1" Selected="True">录入请求</asp:ListItem>
                                <asp:ListItem Value="2">系统生成</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2" style="text-align: left">
                            <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
                            <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divgrid" style="position: relative; word-warp: break-word; word-break: break-all;
                    overflow-x: auto;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        Width="600px" CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                            <asp:TemplateColumn HeaderText="选择" ItemStyle-Width="40">
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                    Width="40" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="False" CssClass="myGridItem" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="billCode" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billNameCode" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="billName" HeaderText="预算过程" ItemStyle-Width="120" HeaderStyle-Width="120">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
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
                            <asp:BoundColumn DataField="billJe" DataFormatString="{0:N2}" HeaderText="追加金额" ItemStyle-Width="100"
                                HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepid" HeaderText="完成审核" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                              <asp:BoundColumn DataField="billDept" HeaderText="部门" ItemStyle-Width="180" HeaderStyle-Width="180">
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
        <tr>
            <td style="height: 30px">
                <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
                </pager:UcfarPager>
                <input type="hidden" runat="server" id="hdwindowheight" />
            </td>
        </tr>
        <tr>
            <td id="wf">
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
