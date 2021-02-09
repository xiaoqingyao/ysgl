<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travelApplicationList.aspx.cs"
    Inherits="travelApplicationList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>出差管理单列表页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">

        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            $("#txb_sqrqbegin").datepicker();
            $("#txb_sqrqend").datepicker();

        });

        $(function () {
            //gudingbiaotounew($("#myGrid"), 380);
            // var status = "none";
            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#btn_cx").click(function () {
                $("#trSelect").toggle();
            });
            //修改
            $("#btn_edit").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[7].innerHTML;
                if (zt != "未提交") {
                    alert("该行已提交,不能修改！");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("travelApplicationDetails2.aspx?Ctrl=Edit&Code=" + billcode);
            });
            //填写单据
            $("#btn_AddBGD").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[7].innerHTML;
                if (zt != "审批通过") {
                    alert("该记录还未审核通过！");
                    return;
                }
                var billzt = checkrow.find("td")[8].innerHTML;
                if (billzt != "未附加") {
                    alert("该记录已经附加单据,如需编辑请到出差报告单页表页！");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("travelReportDetail2.aspx?Ctrl=Add&AppCode=" + billcode);
            });
            $("#btn_delete").click(function () {
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
                if (!confirm("您确定要删除选中的单据吗?")) {
                    return;
                }
                if (checkrow.find("td")[0] != null && checkrow.find("td")[0].innerHTML != "") {
                    var billcode = checkrow.find("td")[0].innerHTML;
                }
                $.post("../MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": "ccsq" }, function (data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            alert("删除成功!");
                            $(".highlight").remove();
                        }
                        else {
                            alert("删除失败!");
                        }
                    }
                    else {
                        alert("删除失败!");
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
                openDetail("travelApplicationDetails2.aspx?Ctrl=View&Code=" + billcode);
            });
            //打印预览
            $("#btn_print").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                if (billcode == null || billcode == "") {
                    alert("请先选择行");
                    return;
                }
                window.showModalDialog("travelApplicationPrint2.aspx?Ctrl=View&Code=" + billcode, 'newwindow', 'center:yes;dialogHeight:700px;dialogWidth:1000px;status:no;scroll:yes');
                //                openDetail("travelApplicationPrint.aspx?Ctrl=View&Code=" + billcode);
                //window.open("travelApplicationPrint.aspx?Ctrl=View&Code=" + billcode);
            });
            //打印预览2
            $("#btn_print2").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                if (billcode == null || billcode == "") {
                    alert("请先选择行");
                    return;
                }
                window.showModalDialog("travelApplicationOnlyPrint2.aspx?Ctrl=View&Code=" + billcode, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1000px;status:no;scroll:yes');
                //                openDetail("travelApplicationPrint.aspx?Ctrl=View&Code=" + billcode);
                //window.open("travelApplicationPrint.aspx?Ctrl=View&Code=" + billcode);
            });
            //返回 btn_print2
            $("#to_travelReportList").click(function () {
                window.location.href = "travelReportList.aspx";
            });
            //取消
            $("#btn_cancle").click(function () {
                document.getElementById("trSelect").style.display = "none";
            });

            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[0] != null && $(this).find("td")[0].innerHTML != "") {
                    var billCode = $(this).find("td")[0].innerHTML;
                }
                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function (data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            //费用类别  

            $("#txtAppDept").autocomplete({

                source: availableTagsdept,
                select: function (event, ui) {
                    var rybh = ui.item.value;
                }
            });
            //撤销单据提交
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
                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "ccsq", "billtype": "ccsq" }, function (data, status) {
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
            //审批提交
            $("#btn_summit").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }

                var zt = checkrow.find("td")[7].innerHTML;
                if (zt != "未提交") {
                    alert("单据已提交.不要重复操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    var dept = checkrow.find("td")[11].innerHTML;

                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": "ccsq", "billtype": "ccsq", "dept": dept }, function (data, status) {
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
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:700px;dialogWidth:1000px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                //document.getElementById("Button4").click();
                $("#Button4").click();
            }
        } function openSplc(openUrl) {
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
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td style="height: 30px">
                    <input type="button" value="查 询" id="btn_cx" class="baseButton" />
                    <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" onclick="openDetail('travelApplicationDetails2.aspx?Ctrl=Add&par=' + Math.random());" />
                    <input type="button" value="修 改" id="btn_edit" class="baseButton" runat="server" />
                    <input type="button" value="删 除" id="btn_delete" class="baseButton" runat="server" />
                    <input type="button" value="详细信息" id="btn_look" class="baseButton" runat="server" />
                    <input type="button" value="填写报告单" id="btn_AddBGD" class="baseButton" runat="server" />
                    <input type="button" value="返回" id="to_travelReportList" class="baseButton" runat="server" />
                    <input type="button" value="审批提交" id="btn_summit" class="baseButton" runat="server" />
                    <input type="button" value="审批撤销" id="btn_replace" class="baseButton" runat="server" />
                    <input type="button" value="打印预览" id="btn_print" class="baseButton" runat="server" />
                    <input type="button" value="打印预览(管理单)" id="btn_print2" class="baseButton" runat="server" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                </td>
            </tr>
            <tr id="trSelect" style="display: none;">
                <td>
                    <div style="float: left">
                        <table class="baseTable" style="text-align: left;">
                            <tr>
                                <td>申请日期从：
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_sqrqbegin" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>至:
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_sqrqend" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>单据编号：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBillCode" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>申请部门：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAppDept" runat="server" Width="120px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>报告单状态：
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server">
                                        <asp:ListItem Value="">--全部--</asp:ListItem>
                                        <asp:ListItem Value="1">已附</asp:ListItem>
                                        <asp:ListItem Value="0">未附</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="8" style="text-align: center">
                                    <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_cx_Click" />
                                    <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divgrid" style="position: relative; overflow-x: auto; word-warp: break-word; word-break: break-all">
                        <asp:DataGrid ID="myGrid" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid"
                            ItemStyle-HorizontalAlign="Center" AutoGenerateColumns="False" OnItemDataBound="myGrid_ItemDataBound">
                            <ItemStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:BoundColumn DataField="billCode" HeaderText="单据编号" HeaderStyle-Width="150px"
                                    ItemStyle-Width="150px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="deptName" HeaderText="申请单位" HeaderStyle-Width="100px"
                                    ItemStyle-Width="100px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billUser" HeaderText="申请人" HeaderStyle-Width="100px"
                                    ItemStyle-Width="100px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDate" HeaderText="申请日期" DataFormatString="{0:D}"
                                    HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="travelTypeName" HeaderText="出差类别" HeaderStyle-Width="80px"
                                    ItemStyle-Width="80px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="hiddenbill" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="reasion" HeaderText="出差事由" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="hiddenbill" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="needAmount" HeaderText="预计费用" DataFormatString="{0:N2}"
                                    HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="审批状态" DataField="stepID" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="出差报告单" DataField="BillStatus" HeaderStyle-Width="90px"
                                    ItemStyle-Width="90px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="出差报告单号" DataField="ReportCode" HeaderStyle-Width="150px"
                                    ItemStyle-Width="150px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="驳回理由" DataField="mind" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="部门" DataField="showdeptcode" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem hiddenbill" />
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False" Position="Top" Mode="NumericPages" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px"></PagerStyle>
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
                <td style="height: 10px">
                    <table>
                        <tr>
                            <td>审核流程：
                            </td>
                            <td id="wf">
                                <asp:Label ID="lblShlc" runat="server"></asp:Label>
                            </td>
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
