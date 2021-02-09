﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bxList.aspx.cs" Inherits="webBill_search_bxList" %>
<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报销单查询</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">

        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            $("#txtDateFrm").datepicker();
            $("#txtDateTo").datepicker();
            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");
            // gudingbiaotounew($("#myGrid"), '340');
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                if ($(this).find("td")[0] != null) {
                    var billCode = $(this).find("td")[0].innerHTML;
                    $("#hd_billCode").val(billCode);
                }

                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            //查询
            $("#btnSelect").click(function() {
                $("#trSelect").toggle();
            });
            //取消
            $("#btn_cancle").click(function() {
                document.getElementById("trSelect").style.display = "none";
            });
            //报销人自动加载
            $("#txtBxr").autocomplete({
                source: availableTags
            });
            //科目自动加载
            $("#txtSubject").autocomplete({
                source: subjectTags
            });
            //归口部门
            $("#txtGKDept").autocomplete({
                source: gkDEPTTags
            });
        });
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');

        }
        function openLookSpStep(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        //        function gudingbiaotou() {
        //            var t = document.getElementById("<%=myGrid.ClientID%>");
        //            if(t==null||t.rows.length<1)
        //            {
        //             return;
        //            }
        //            var t2 = t.cloneNode(true);
        //			 t2.id = "cloneGridView";
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
        //        function selectry(openUrl) {
        //            var str = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:750px;status:no;scroll:yes');
        //            if (str != undefined && str != "") {
        //                document.getElementById("txtBxr").value = str;
        //            }
        //        }


        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "-1px 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
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
                    Text="包含下级单位" Checked="True" />&nbsp;<asp:Button ID="Button3" runat="server" Text="详细信息"
                        CssClass="baseButton" OnClick="Button3_Click1" />
                <asp:Button ID="Button6" runat="server" CssClass="baseButton" OnClick="Button6_Click1"
                    Text="打印预览" Visible="false" />
                <input type="button" id="btnSelect" value="查  询" class="baseButton" />&nbsp;<asp:Button
                    ID="Button2" runat="server" CssClass="baseButton" Text="导出Excel" OnClick="btnExport_Click" />
                <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
            </td>
        </tr>
        <tr id="trSelect" style=" display:none">
            <td>
                <table class="baseTable" align="left" style="text-align: left">
                    <tr>
                        <td>
                            日期从：
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateFrm" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            到：
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateTo" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            报销人：
                        </td>
                        <td>
                            <asp:TextBox ID="txtBxr" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            科目：
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            审核状态：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="125px">
                                <asp:ListItem Value="">--全部--</asp:ListItem>
                                <asp:ListItem Value="1">审批中</asp:ListItem>
                                <asp:ListItem Value="2">审核通过</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            归口部门：
                        </td>
                        <td>
                            <asp:TextBox ID="txtGKDept" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td colspan="4">
                            <div style="text-align: center">
                                <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
                                <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divgrid" style="overflow-x: auto; position: relative; word-warp: break-word;
                    word-break: break-all">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" Style="width: 900px" OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                            <asp:BoundColumn DataField="billCode" HeaderText="报销单号" ItemStyle-Width="50" HeaderStyle-Width="50">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem hiddenbill" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="单据编号" HeaderText="单据编号" ItemStyle-Width="60" HeaderStyle-Width="60">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="报销人" HeaderText="报销人" ItemStyle-Width="50" HeaderStyle-Width="50">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="报销日期" HeaderText="报销申请日期" DataFormatString="{0:D}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Width="80" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                    CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Width="80" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="报销单位" HeaderText="所属部门" ItemStyle-Width="80" HeaderStyle-Width="80">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="金额" HeaderText="报销总额" DataFormatString="{0:N2}" ItemStyle-Width="60"
                                HeaderStyle-Width="60">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepid" HeaderText="状态" ItemStyle-Width="60" HeaderStyle-Width="60">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="摘要" HeaderText="摘要" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="gkdept" HeaderText="归口部门" ItemStyle-Width="80" HeaderStyle-Width="80">
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
                <pager:ucfarpager id="ucPager" runat="server" onpagechanged="UcfarPager1_PageChanged">
                </pager:ucfarpager>
                <input type="hidden" runat="server" id="hdwindowheight" />
            </td>
        </tr>
        <tr>
            <asp:HiddenField ID="hd_billCode" runat="server" />
            <td id="wf" style=" height:10px">
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
