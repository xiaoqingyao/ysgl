<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChdList_dr.aspx.cs" Inherits="webBill_bxgl_ChdList_dr" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

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
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[7].innerHTML;
                if (zt != "未提交") {
                    alert("该行已提交,不能删除！");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                var dydj = '<%=Request["dydj"] %>';

                openDetail("bxDetailFinal.aspx?type=edit&billCode=" + billcode + "&dydj=04");
                //window.location.href="bxDetailFinal.aspx?type=edit&billCode=" + billcode;
            });
            $("#btn_delete").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[7].innerHTML;
                var lx = zt.substring(zt.length - 2, zt.length);
                var hdYbbxNeedAudit = $("#hdYbbxNeedAudit").val();
                if (zt != "未提交" && lx != "否决" && hdYbbxNeedAudit == "1") {
                    alert("该行已提交,不能删除！");
                    return;
                }

                if (!confirm("您确定要删除选中的单据吗?")) {
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                var type = "ybbx";
                <%--  var dydj = '<%=Request["dydj"] %>';
                if (dydj != undefined && dydj != null && dydj != "") {
                    type = dydj;
                }--%>

                var flowid = $("#hidflowid").val();

                if (flowid != undefined && flowid != null && flowid != "") {
                    type = flowid;
                }

                $.post("../MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": type }, function (data, status) {
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
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                var dydj = '<%=Request["dydj"] %>';

                openDetail("bxDetailFinal.aspx?type=look&billCode=" + billcode + "&dydj=04");
            });
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[0] != null) {
                    var billCode = $(this).find("td")[0].innerHTML;
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
            $("#btn_replace").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[7].innerHTML;
                if (zt == "未提交" || zt == "审批通过") {
                    alert("该单据不能撤销操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    var djtype = "ybbx";
                    var dydj = '<%=Request["dydj"] %>';
                    if (dydj != undefined && dydj != null && dydj != "") {
                        djtype = dydj;
                    }
                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": djtype }, function (data, status) {
                        if (status == "success") {
                            if (data == "1") {
                                alert("撤销成功！");
                                checkrow.find("td")[7].innerHTML = "未提交";
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
            $("#btn_print").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                window.open("../YbbxPrint/YbbxPrint.aspx?billCode=" + billcode);
            });
            $("#btn_summit").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var dept = checkrow.find("td")[11].innerHTML;
                var zt = checkrow.find("td")[7].innerHTML;
                if (zt != "未提交") {
                    alert("单据已提交.不要重复操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    var djtype = "ybbx";
                    var flowid = $("#hidflowid").val();

                    if (flowid != undefined && flowid != null && flowid != "") {
                        djtype = flowid;
                    }
                <%--    var dydj = '<%=Request["dydj"] %>';
                    if (dydj != undefined && dydj != null && dydj != "") {
                        djtype = dydj;
                    }--%>
                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": djtype, "dept": dept }, function (data, status) {
                        if (data == "-1" || data == "-2") {
                            alert("提交失败！");
                        } else if (data == "-3") {
                            alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                        } else {
                            if (status == "success") {
                                alert("提交成功！");
                                checkrow.find("td")[7].innerHTML = data;
                            }
                        }
                    });
                }
            });
        });


        function openDetail(openUrl) {
            //            var returnValue = window.showModelessDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            //            if (returnValue == undefined || returnValue == "") {
            //                return false;
            //            }
            //            //            else {
            //            //                document.getElementById("Button2").click();
            //            //            }

            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("Button2").click();
            }

        }
        function openLookSpStep(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
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

        function OpenAdd() {
            var dydj = '<%=Request["dydj"] %>';

            var url = 'bxDetailFinal.aspx?type=add&par=' + Math.random()+'&dydj=04';
            
            openDetail(url);
        }

        $(function () {
            var dydj = '<%=Request["dydj"] %>';
         
            $("#btn_importEcxel").click(function () {
                window.location.href = "chbxd_ImportExcel.aspx?dydj=04";
                //openDetail("chbxd_ImportExcel.aspx?dydj=04");
            });
        });
            function initWindowHW() {
                //给隐藏域设置窗口的高度
                $("#hdwindowheight").val($(window).height());
                //给gridview表格外部的div设置宽度  宽度为页面宽度
                $("#divgrid").css("width", ($(window).width() - 5));
            }
            $(function () {
                initWindowHW();
                initMainTableClass("<%=myGrid.ClientID%>");
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
                    <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" onclick="OpenAdd();" />
                    <input type="button" value="修 改" id="btn_edit" class="baseButton" />
                    <input type="button" value="删 除" id="btn_delete" class="baseButton" />
                    <input type="button" value="详细信息" id="btn_look" class="baseButton" />
                    <input type="button" value="审批提交" id="btn_summit" class="baseButton" />
                    <input type="button" value="审批撤销" id="btn_replace" class="baseButton" />
                    <input type="button" class="baseButton" id="btn_importEcxel" value="导入" runat="server" />
                    <input type="button" value="打印预览" id="btn_print" class="baseButton" style="display: none" />
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
                                <td colspan="6">
                                    <asp:Button ID="Button2" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
                                    <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" Width="1400px"
                            CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound">
                            <Columns>
                                <asp:BoundColumn DataField="billCode" HeaderText="报销单号">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem hiddenbill" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billName" HeaderText="单据编号" HeaderStyle-Width="120" ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billUserName" HeaderText="报销人" HeaderStyle-Width="120"
                                    ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDate" HeaderText="报销申请日期" DataFormatString="{0:D}"
                                    HeaderStyle-Width="120" ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDept" HeaderText="所属部门" DataFormatString="{0:D}"
                                    HeaderStyle-Width="130" ItemStyle-Width="130">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billJe" HeaderText="报销总额" DataFormatString="{0:N2}" HeaderStyle-Width="120"
                                    ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bxzy" HeaderText="摘要" HeaderStyle-Width="350" ItemStyle-Width="350">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="审批状态" DataField="stepid" HeaderStyle-Width="120" ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="isgk" HeaderText="归口费用" HeaderStyle-Width="120" ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="gkDept" HeaderText="归口部门" HeaderStyle-Width="150" ItemStyle-Width="150">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="驳回理由" HeaderStyle-Width="120" ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader"
                                        Width="100" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Width="100" Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>

                                <asp:BoundColumn DataField="billDept" HeaderText="部门">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem hiddenbill" />
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid>
                        <%-- <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" PageSize="17" Style="table-layout: fixed" Width="100%" OnItemDataBound="myGrid_ItemDataBound"
                        AllowPaging="True">
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>--%>
                        <asp:HiddenField ID="hidflowid" runat="server" />
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
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
