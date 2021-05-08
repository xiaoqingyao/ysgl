<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ystz_qt_dz.aspx.cs" Inherits="webBill_ysgl_ystz_qt_dz" %>
<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>其他退费调整-大智</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <style type="text/css">
        .hiddenbill {
            display: none;
        }
    </style>

    <script language="javascript" type="Text/javascript">

        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[0] != null) {
                    var billCode = $(this).find("td")[0].innerHTML;
                    $("#hd_billCode").val(billCode);
                }

                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function (data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
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
                var vardeptcode = $("#LaDept").val();
                var isdz = '<%=Request["isdz"]%>';

                window.location.href = "tf_ystzDetail_qt_dz.aspx?type=look&deptcode=" + vardeptcode + "&billCode=" + billcode;


            });
            $("#btn_delete").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var hdYstzNeedAudit = $("#hd_YstzNeedAudit").val();
                var zt = checkrow.find("td")[6].innerHTML;
                if (zt != "未提交" && !(zt == "审批通过" && hdYstzNeedAudit == "0")) {
                    alert("该行已提交,不能删除！");
                    return;
                }
                if (!confirm("您确定要删除选中的单据吗?")) {
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                $.post("../MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": "ystz" }, function (data, status) {
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
            $("#btn_replace").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[6].innerHTML;
                if (zt == "未提交" || zt == "审批通过") {
                    alert("该单据不能撤销操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "ystz" }, function (data, status) {
                        if (status == "success") {
                            if (data == "1") {
                                alert("撤销成功！");
                                checkrow.find("td")[6].innerHTML = "未提交";
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
            $("#btn_summit").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[6].innerHTML;
                if (zt != "未提交") {
                    alert("单据已提交.不要重复操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": "ystz" }, function (data, status) {
                        if (status == "success") {
                            if (data == "-1") {
                                alert("预算过程缺少财务填报部分,不能提交！");
                            }
                            else if (data == "-2") {
                                alert("预算过程缺少部门填报部分,不能提交！");
                            } else if (data == "-3") {
                                alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                            }
                            else {
                                alert("提交成功！");
                                checkrow.find("td")[6].innerHTML = data;
                            }
                        }
                        else {
                            alert("失败");
                        }
                    });
                }
            });
            $("#btn_insert").click(function () {
                var isdz = '<%=Request["isdz"]%>';
                var vardeptcode = $("#LaDept").val();
                window.location.href = "tf_ystzDetail_qt_dz.aspx?type=add&deptcode=" + vardeptcode;
            });
            $("#btn_edit").click(function () {
                var isdz = '<%=Request["isdz"]%>';
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[6].innerHTML;
                if (zt != "未提交") {
                    alert("该行已提交！");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                var vardeptcode = $("#LaDept").val();
                var url = "";

                window.location.href = "tf_ystzDetail_qt_dz.aspx?type=edit&billCode=" + billcode + "&deptcode=" + vardeptcode;


            });
        });
        //function editCheck() {
        //    var checkrow = $(".highlight");
        //    if (checkrow.val() == undefined) {
        //        alert("请先选择行");
        //        return false;
        //    }
        //    $("#choosebill").val(checkrow.find("td")[0].innerHTML);
        //    var zt = checkrow.find("td")[4].innerHTML;
        //    if (zt != "未提交") {
        //        alert("该行已提交！");
        //        return false;
        //    }
        //    return true;
        //}


        function openDetail(option, DetailUrl) {
            if (DetailUrl == "YstzDetailNew.aspx") {
                var returnValue = window.showModalDialog(DetailUrl + option, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:900px;status:no;scroll:yes');
                if (returnValue == "success") {
                    document.getElementById("Button6").click();
                }
            } else if (DetailUrl == "yd_ystzDetail.aspx") {
                window.location.href = DetailUrl + option;
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


        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
        $(function () {
            initWindowHW();
        });

    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 30px">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red">预算调整</asp:Label>&nbsp; 部门：<asp:DropDownList
                        runat="server" ID="LaDept" AutoPostBack="True" OnSelectedIndexChanged="LaDept_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;
                    审批状态：
                     <asp:DropDownList runat="server" ID="ddlstatus" Width="122px" AutoPostBack="True" OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged">
                         <asp:ListItem Value="">全部</asp:ListItem>
                         <asp:ListItem Value="end">审批通过</asp:ListItem>
                         <asp:ListItem Value="-1">未提交/审批中</asp:ListItem>
                     </asp:DropDownList>
                    &nbsp;<asp:Button ID="Button6" runat="server" Text="刷 新" CssClass="baseButton" OnClick="Button6_Click" />
                    <input id="btn_insert" value="新 增" class="baseButton" type="button" />
                    <input id="btn_edit" value="修 改" class=" hiddenbill" type="button" />
                    <%--<asp:Button ID="Button2" runat="server" Text="修 改" CssClass="baseButton" OnClick="Button2_Click" OnClientClick=" return editCheck();" />--%>
                    <input type="button" value="删 除" id="btn_delete" class="baseButton" />
                    <input type="button" value="详细信息" id="btn_look" class="baseButton" />
                    <input type="button" value="审批提交" id="btn_summit" class="baseButton" />
                    <input type="button" value="审批撤销" id="btn_replace" class="baseButton" />
                    <asp:Button ID="btn_Export" runat="server" Text="导出Excel" OnClick="btn_Export_Click" CssClass="baseButton" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" Width="1100px" PageSize="17" OnItemDataBound="myGrid_ItemDataBound">
                            <Columns>
                                <asp:BoundColumn DataField="billCode" HeaderText="单据编号">
                                    <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="myGridItem hiddenbill" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDept" HeaderText="填报单位" HeaderStyle-Width="150" ItemStyle-Width="150">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billUser" HeaderText="制单人" HeaderStyle-Width="150" ItemStyle-Width="150">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="gcmc" HeaderText="目标预算过程" HeaderStyle-Width="150" ItemStyle-Width="150">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billJe" HeaderText="调整金额" DataFormatString="{0:N2}" ItemStyle-Width="100"
                                    HeaderStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDate" DataFormatString="{0:D}" HeaderText="制单日期" HeaderStyle-Width="150" ItemStyle-Width="150">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="stepid" HeaderText="审核状态" HeaderStyle-Width="150" ItemStyle-Width="150">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billName2" HeaderText="摘要" ItemStyle-Width="120" HeaderStyle-Width="120">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="dydjname" HeaderText="调整类型" ItemStyle-Width="120" HeaderStyle-Width="120">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemCenter" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="mind" HeaderText="驳回理由" ItemStyle-Width="120" HeaderStyle-Width="120">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>

                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
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
                                <asp:HiddenField ID="hd_billCode" runat="server" />
                                <asp:HiddenField ID="hd_YstzNeedAudit" runat="server" />
                            </td>
                            <td id="wf"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        parent.closeAlert('UploadChoose');
    </script>
</body>
</html>

