<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gkbxList.aspx.cs" Inherits="webBill_search_gkbxList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
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
        var status = "none";
        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            $("#txtDateFrm").datepicker();
            $("#txtDateTo").datepicker();
            //            gudingbiaotounew($("#myGrid"), 300);
            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {

                if ($(this).find("td")[0] != null) {
                    var billCode = $(this).find("td")[0].innerHTML;
                    var billname = $(this).find("td")[1].innerHTML;
                    $("#hd_billCode").val(billCode);
                    $("#hd_billname").val(billname);
                    
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
            //经办人自动加载
            $("#txtJbr").autocomplete({
                source: availableTags
            });
            //科目自动加载
            $("#txtSubject").autocomplete({
                source: subjectTags
            });
        });
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');

        }
        function openLookSpStep(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
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
                    Text="包含下级单位" Checked="True" />
                <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                <asp:Button ID="Button3" runat="server" Text="详细信息" CssClass="baseButton" OnClick="Button3_Click1" />
                <asp:Button ID="Button6" runat="server" CssClass="baseButton" OnClick="Button6_Click1"
                    Text="打印预览" />
                <asp:Button ID="Button2" runat="server" CssClass="baseButton" Text="导出Excel" OnClick="Button2_Click" />
                 <asp:Button ID="btn_exclall" runat="server" CssClass="baseButton" Text="导出Excel(全部)"  Visible="false" OnClick="btn_exclall_Click" />
                <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
            </td>
        </tr>
        <tr id="trSelect" style="display: none; float: left;">
            <td align="left">
                <table class="baseTable" style="text-align: left;">
                    <tr>
                        <td style="text-align: right">
                            日期从：
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateFrm" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            到：
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateTo" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            报销人：
                        </td>
                        <td>
                            <asp:TextBox ID="txtBxr" runat="server" Width="120px"></asp:TextBox>
                        </td>
                         <td style="text-align: right">
                            经办人：
                        </td>
                        <td>
                            <asp:TextBox ID="txtJbr" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            审核状态：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="125px">
                                <asp:ListItem Value="">--全部--</asp:ListItem>
                                <asp:ListItem Value="-1">未提交</asp:ListItem>
                                <asp:ListItem Value="1">审批中</asp:ListItem>
                                <asp:ListItem Value="2">审核通过</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            科目：
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            单据编号：
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillCode" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            是否给付：
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList1" runat="server" Width="125px">
                                <asp:ListItem Value="">--全部--</asp:ListItem>
                                <asp:ListItem Value="1">已给付</asp:ListItem>
                                <asp:ListItem Value="0">未给付</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td  style="text-align: left">
                            <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
                            <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
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
                        CssClass="myGrid" Style="table-layout: fixed; word-wrap: break-word;" Width="1500"
                        OnItemDataBound="myGrid_ItemDataBound" AllowPaging="True" PageSize="17" ShowFooter="true">
                        <Columns>
                            <asp:BoundColumn DataField="billCode" HeaderText="报销单号">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader hiddenbill" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem hiddenbill" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem hiddenbill" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billName" HeaderText="报销单号" ItemStyle-Width="100" HeaderStyle-Width="100"
                                FooterStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader " />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem " />
                                <FooterStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" HeaderText="报销申请日期" DataFormatString="{0:D}"
                                ItemStyle-Width="100" HeaderStyle-Width="100" FooterStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Width="90px" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true"
                                    CssClass="myGridHeader" />
                                <ItemStyle Width="90px" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Width="90px" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true"
                                    CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDept" HeaderText="所属部门" ItemStyle-Width="160" HeaderStyle-Width="160"
                                FooterStyle-Width="160">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                           
                            <asp:BoundColumn DataField="billUser1" HeaderText="制单人" ItemStyle-Width="50" HeaderStyle-Width="50"
                                FooterStyle-Width="50">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" Width="50" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>

                             <asp:BoundColumn DataField="jbr" HeaderText="经办人" ItemStyle-Width="50" HeaderStyle-Width="50"
                                FooterStyle-Width="50">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" Width="50" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                             <asp:BoundColumn DataField="bxr" HeaderText="报销人" ItemStyle-Width="50" HeaderStyle-Width="50"
                                FooterStyle-Width="50">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" Width="50" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billJe" HeaderText="报销总额" DataFormatString="{0:N2}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader"
                                    Width="90" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItemRight" Width="90" />
                                <FooterStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItemRight" Width="90" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="bxzy" HeaderText="摘要">
                                <HeaderStyle Font-Bold="True" Width="200px" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true"
                                    CssClass="myGridHeader" />
                                <ItemStyle Width="200px" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Width="200px" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true"
                                    CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="附加单据">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader"
                                    Width="120" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" Width="120" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem"
                                    Width="120" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepid" HeaderText="状态" ItemStyle-Width="120" HeaderStyle-Width="120"
                                FooterStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="pzcode" HeaderText="凭证号" ItemStyle-Width="100" HeaderStyle-Width="100"
                                FooterStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="sfgf" HeaderText="是否给付">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader"
                                    Width="60" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" Width="60" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem"
                                    Width="60" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="gfje" HeaderText="给付金额">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader"
                                    Width="60" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" Width="60" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem"
                                    Width="60" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="gfr" HeaderText="给付人" ItemStyle-Width="50" HeaderStyle-Width="50"
                                FooterStyle-Width="50">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="gfsj" HeaderText="给付时间">
                                <HeaderStyle Font-Bold="True" Width="150px" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true"
                                    CssClass="myGridHeader" />
                                <ItemStyle Width="150px" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cxr" HeaderText="撤销人" ItemStyle-Width="50" HeaderStyle-Width="50"
                                FooterStyle-Width="50">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cxsj" HeaderText="撤销时间" ItemStyle-Width="100" HeaderStyle-Width="100"
                                FooterStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cxyy" HeaderText="撤销原因" ItemStyle-Width="100" HeaderStyle-Width="100"
                                FooterStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
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
            <td>
                <table>
                    <tr>
                        <td style="height: 10px">
                            审批流程:
                            <asp:HiddenField ID="hd_billCode" runat="server" />
                             <asp:HiddenField ID="hd_billname" runat="server" />
                        </td>
                        <td id="wf">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
