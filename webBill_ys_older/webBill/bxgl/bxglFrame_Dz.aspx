<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bxglFrame_Dz.aspx.cs" Inherits="webBill_bxgl_bxglFrame_Dz" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>归口报销列表页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>
    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
        var status = "none";
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            $("#txtLoanDateFrm").datepicker();
            $("#txtLoanDateTo").datepicker();
            $("#btn_edit").click(function () {
                var dydj = '<%=Request["dydj"] %>';
                var djlx = '<%=Request["djlx"]%>';
                var djmxlx = '<%=Request["djmxlx"]%>';//单据明细类型  就是报销明细类型 是否占用预算是否冲减预算的选项
                var isDZ = '<%=Request["isDZ"]%>';
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[2].innerHTML;
                var zt = checkrow.find("td")[8].innerHTML;
                if (zt != "未提交") {
                    alert("该行已提交,不能修改！");
                    return;
                }
                var url = "bxDetailForDz.aspx?type=edit&billCode=" + billcode;
                if (dydj != undefined && dydj != null && dydj != "") {
                    url += '&dydj=' + dydj;
                }
                if (djlx != undefined && djlx != null && djlx != "") {
                    url += '&djlx=' + djlx;
                }
                if (djmxlx != undefined && djmxlx != null && djmxlx != "") {
                    url += '&djmxlx=' + djmxlx;
                }
                if (isDZ != undefined && isDZ != null && isDZ != "") {
                    url += '&isDZ=' + isDZ;
                }
                openDetail(url);
            });
            $("#btn_delete").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[8].innerHTML;
                var lx = zt.substring(zt.length - 2, zt.length);
                var hdYbbxNeedAudit = $("#hdYbbxNeedAudit").val();
                if (zt != "未提交" && lx != "否决" && hdYbbxNeedAudit == "1") {
                    alert("该行已提交,不能删除！");
                    return;
                }

                if (!confirm("您确定要删除选中的单据吗?")) {
                    return;
                }


                var billcode = checkrow.find("td")[2].innerHTML;

                $.post("../MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": "gkbx" }, function (data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            alert("删除成功!");
                            $(".highlight").remove();
                        }
                        else {
                            alert("删除失败1!");
                        }
                    }
                    else {
                        alert("删除失败2!");
                    }
                });
            });
            $("#btn_look").click(function () {
                var dydj = '<%=Request["dydj"] %>';
                var djlx = '<%=Request["djlx"]%>';
                var djmxlx = '<%=Request["djmxlx"]%>';//单据明细类型  就是报销明细类型 是否占用预算是否冲减预算的选项
                var isDZ = '<%=Request["isDZ"]%>';
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[2].innerHTML;
                var url = 'bxDetailForDz.aspx?type=look&billCode=' + billcode;
                if (dydj != undefined && dydj != null && dydj != "") {
                    url += '&dydj=' + dydj;
                }
                if (djlx != undefined && djlx != null && djlx != "") {
                    url += '&djlx=' + djlx;
                }
                if (djmxlx != undefined && djmxlx != null && djmxlx != "") {
                    url += '&djmxlx=' + djmxlx;
                }
                if (isDZ != undefined && isDZ != null && isDZ != "") {
                    url += '&isDZ=' + isDZ;
                }
                openDetail(url);
            });
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[1] != null) {
                    var billCode = $(this).find("td")[2].innerHTML;
                }

                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function (data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            //查询
            $("#btnSelect").click(function () {
                $("#trSelect").toggle();
            });
            //从用友导入存货报销单
            $("#btn_drxgj").click(function () {
                window.location.href = "chbxd_ImportExcel.aspx?dydj=04";
                //openDetail("chbxd_ImportExcel.aspx?dydj=04");
            });
            //取消
            $("#btn_cancle").click(function () {
                document.getElementById("trSelect").style.display = "none";
            });
            //部门选择
            $("#txtLoanDeptCode").autocomplete({
                source: availableTags
            });
            //人员选择
            $("#txtloannamecode").autocomplete({
                source: avaiusertb
            });
            //撤销
            $("#btn_replace").click(function () {
                var checkrow = $(".highlight");//单行提交
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[8].innerHTML;
                if (zt == "未提交" || zt == "审批通过") {
                    alert("该单据不能撤销操作!");
                } else {
                    var billcode = checkrow.find("td")[2].innerHTML;
                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "gkbx" }, function (data, status) {
                        //alert(data);
                        if (status == "success") {
                            if (data == "1") {
                                alert("撤销成功！");
                                checkrow.find("td")[8].innerHTML = "未提交";
                            }
                            else {
                                alert("单据以进入审批，不能撤销");
                            }
                        }
                        else {
                            alert("失败");
                        }
                    });
                }
            });
            //打印预览
            $("#btn_print").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[2].innerHTML;
                var je = 0;
                je = checkrow.find("td")[6].innerHTML;
                var dydj = '<%=Request["dydj"]%>';
                var isDZ = '<%=Request["isDZ"]%>';
                if (dydj.length > 0) {
                    if (dydj == "06" & isDZ == "1") {

                        window.open("bxdprint_dz_fybx.aspx?billCode=" + billcode + "&dydj=" + dydj + "&je=" + je);

                    }
                    if (dydj == "02" & isDZ == "1") {
                        window.open("bxdprint_dz.aspx?billCode=" + billcode + "&dydj=" + dydj + "&je=" + je);
                    }

                } else {
                    window.open("../YbbxPrint/YbbxPrint.aspx?billCode=" + billcode);

                }
            });
            //打印预览
            $("#btn_print2").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[2].innerHTML;
                window.open("../YbbxPrint/GkbxPrint2.aspx?billName=" + billcode);
            });
            //审核详细页
            $("#btn_sh").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[2].innerHTML;
                openDetail("../MyWorkFlow/BillShDetail.aspx?billCode=" + billcode);
            });
            //提交
            $("#btn_summit").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[8].innerHTML;
                var djtype = "ybbx";
                var flowid = $("#hidflowid").val();

                if (flowid != undefined && flowid != null && flowid != "") {
                    djtype = flowid;
                }
                if (zt != "未提交") {
                    alert("单据已提交.不要重复操作!");
                } else {
                    var billcode = checkrow.find("td")[2].innerHTML;
                    //因为一般处理程序处理的方式同市立医院的gkbx 所以传的gkbx
                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": djtype, "isdz": "1" }, function (data, status) {
                        if (data == "-1" || data == "-2") {
                            alert("提交失败！");
                        } else if (data == "-3") {
                            alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                        } else {
                            if (status == "success") {
                                alert("提交成功！");
                                checkrow.find("td")[8].innerHTML = data;
                            }
                        }
                    });
                }
            });
        });


        function openDetail(openUrl) {
            //var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:700px;dialogWidth:990px;status:no;scroll:yes');
            //if (returnValue == undefined || returnValue == "") {
            //    return false;
            //}
            //else {
            //    document.getElementById("btnRefresh").click();
            //}
            $("#prodcutDetailSrc").attr("src", openUrl);
            $("#dialog-confirm").dialog(
                {
                    modal: true,             // 创建模式对话框
                    autoOpen: true,//是否自动打开
                    height: 700, //高度
                    width: 990, //宽度
                    //title: "新窗体",
                    title_html: true,
                    buttons: {
                        //"Ok": function () {
                        //    $("#btnRefresh").click();
                        //    $(this).dialog('close');
                        //},
                        //"Cancel": function () { $(this).dialog('close'); return false; }
                    }
                }
            );

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
        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "-1px 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
        function add() {
            var dydj = '<%=Request["dydj"] %>';
            var djlx = '<%=Request["djlx"]%>';
            var djmxlx = '<%=Request["djmxlx"]%>';//单据明细类型  就是报销明细类型 是否占用预算是否冲减预算的选项
            var isDZ = '<%=Request["isDZ"]%>';
            var url = 'bxDetailForDz.aspx?type=add&par=' + Math.random();
            if (dydj != undefined && dydj != null && dydj != "") {
                url += '&dydj=' + dydj;
            }
            if (djlx != undefined && djlx != null && djlx != "") {
                url += '&djlx=' + djlx;
            }
            if (djmxlx != undefined && djmxlx != null && djmxlx != "") {
                url += '&djmxlx=' + djmxlx;
            }
            if (isDZ != undefined && isDZ != null && isDZ != "") {
                url += '&isDZ=' + isDZ;
            }
            openDetail(url);

        }
        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
        $(function () {
            var dydj = '<%=Request["dydj"] %>';
            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#btn_Srbg").click(function () {
                window.location.href = "../shouru/srd_dz.aspx";
            });
        });



    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 30px">
                    <input id="btnRefresh" type="button" class="baseButton" value="刷 新" onclick="javascript: location.replace(location.href);" />
                    <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                    <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" onclick="add();" />
                    <input type="button" class="baseButton" id="btn_Srbg" value="从校管家批量导入" runat="server" />
                    <input type="button" value="修 改" id="btn_edit" class="baseButton" />
                    <input type="button" value="删 除" id="btn_delete" class="baseButton" />
                    <input type="button" value="详细信息" id="btn_look" class="baseButton" />
                    <input type="button" value="审批提交" id="btn_summit" class="baseButton" />
                    <input type="button" value="审批撤销" id="btn_replace" class="baseButton" />
                    <input type="button" value="打印预览" runat="server" id="btn_print" class="baseButton" />
                    <input type="button" value="从用友批量导入" id="btn_drxgj" class="baseButton" />
                    <input type="button" class="baseButton" id="btn_importEcxel" value="收入报告单Excel导入" runat="server" visible="false" />

                    <input type="button" class="baseButton" id="btn_sh" value="查看审核详细信息" style="display: none" />
                    <asp:Button ID="btn_Export" runat="server" Text="导出Excel" OnClick="btn_Export_Click" CssClass="baseButton" />
                    <asp:Button ID="Button3" runat="server" Text="导出Excel2" OnClick="Button3_Click" CssClass="baseButton" />
                    <%--<input type="button" value="打印预览（方式2）" id="btn_print2" class="baseButton" />--%>
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                </td>
            </tr>
            <tr id="trSelect" style="display: none;">
                <td align="left" colspan="3">
                    <div style="float: left">
                        <table class="baseTable" style="text-align: left;">
                            <tr>
                                <td>申请日期从：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLoanDateFrm" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>到：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLoanDateTo" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>报销人：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtloannamecode" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>单位：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLoanDeptCode" runat="server" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>单据编号：
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>审批状态:
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlstatus" Width="122px">
                                        <asp:ListItem Value="">全部</asp:ListItem>
                                        <asp:ListItem Value="end">审批通过</asp:ListItem>
                                        <asp:ListItem Value="-1">未提交/审批中</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="4">
                                    <asp:Button ID="Button2" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
                                    <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound">
                            <Columns>
                                <asp:BoundColumn DataField="" HeaderText="报销单号" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem hiddenbill" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="" HeaderText="报销单号" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem hiddenbill" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billName" HeaderText="单据编号" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billUserName" HeaderText="制单人" ItemStyle-Width="100"
                                    HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDate" HeaderText="单据日期" DataFormatString="{0:D}"
                                    ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Width="120" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="false"
                                        CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Width="120" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" CssClass="myGridItem"
                                        HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDept" HeaderText="部门"
                                    ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billJe" HeaderText="单据总额" DataFormatString="{0:F2}" ItemStyle-Width="100"
                                    HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItemRight" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bxsm" HeaderText="实际情况说明" ItemStyle-Width="350" HeaderStyle-Width="350">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="审批状态" DataField="stepid" ItemStyle-Width="100" HeaderStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="驳回理由" DataField="">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
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
                    <asp:HiddenField ID="hidflowid" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                    <table>
                        <tr>
                            <td>审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
                                <asp:HiddenField ID="hdYbbxNeedAudit" runat="server" />
                            </td>
                            <td id="wf"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div id="dialog-confirm" style="display: none; overflow: hidden;">
            <iframe frameborder="no" border="0" marginwidth="0" marginheight="0" id="prodcutDetailSrc" scrolling="no" width="100%" height="100%"></iframe>
        </div>
    </form>

    <script type="text/javascript">
        function SelectAll(aControl) {
            var chk = document.getElementById("myGrid").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
            }
        }
    </script>

</body>


</html>
