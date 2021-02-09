<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WeiXiuShenQingList.aspx.cs"
    Inherits="webBill_ZiChanGuanLi_WeiXiuShenQingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>维修申请列表页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">
        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                var row = $(".highlight");
                var id = row.find("td")[0].innerHTML;
                if (id != null && id != undefined && id != "") {
                    $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": id }, function(data, status) {
                        if (status == "success") {
                            $("#wf").html(data);
                        }
                    });
                }
            });
            //刷新
            $("#btn_refresh").click(function() {
                location.replace(location.href);
            });
            //查询
            $("#btnSelect").click(function() {
                 $("#trSelect").toggle();
            });
            //取消
            $("#btn_cancle").click(function() {
                document.getElementById("trSelect").style.display = "none";
            });
            //部门选择
            $("#txtAppPersioncode").autocomplete({
                source: availableTagsPersion
            });
            //人员选择
            $("#txtDeptCode").autocomplete({
                source: availableTagsDept
            });
            //添加
            $("#btn_Add").click(function() {
                var openUrl = "WeiXiuShenQingDetail.aspx?Ctrl=Add&par=" + Math.random();
                var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1000px;status:no;scroll:yes');
                if (returnValue == "yes") {
                    $("#btn_Select").click();
                }
            });
            //修改
            $("#btn_Edit").click(function() {
                var row = $(".highlight");
                var id = row.find("td")[0].innerHTML;
                if (id == null || id == undefined || id == "") { alert("请先选择行！"); return; }
                var openUrl = "WeiXiuShenQingDetail.aspx?Ctrl=Edit&Code=" + id + "&par=" + Math.random();
                var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1000px;status:no;scroll:yes');
                if (returnValue == "yes") {
                    $("#btn_Select").click();
                }
            });
            //详细信息
            $("#btn_View").click(function() {
                var row = $(".highlight");
                var id = row.find("td")[0].innerHTML;
                if (id == null || id == undefined || id == "") { alert("请先选择行！"); return; }
                var openUrl = "WeiXiuShenQingDetail.aspx?Ctrl=View&Code=" + id + "&par=" + Math.random();
                window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1000px;status:no;scroll:yes');
            });
            //删除
            $("#btn_Delete").click(function() {
            var row = $(".highlight");
            var id = row.find("td")[0].innerHTML;
            if (id == null || id == undefined || id == "") { alert("请先选择行！"); return; }
            var zt = row.find("td")[6].innerHTML;
            if (zt != "未提交") {
                alert("该行已提交,不能删除！");
                return;
            }
            if (!confirm("确定要删除该记录吗？")) { return; }
                $.post("../MyAjax/DeleteBill.ashx", { "billCode": id, "type": "wxsq" }, function(data, status) {
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
            //审批提交
            $("#btn_Submint").click(function() {
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
                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": "wxsq", "billtype": "ccsq" }, function(data, status) {
                        if (data == "-1" || data == "-2") {
                            alert("提交失败！");
                        } else if (data == "-3") {
                            alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                        } else {
                            if (status == "success") {
                                alert("提交成功！");
                                checkrow.find("td")[6].innerHTML = data;
                            }
                        }
                    });
                }
            });
            //撤销单据提交
            $("#btn_Revert").click(function() {
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
                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "wxsq", "billtype": "ccsq" }, function(data, status) {
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
        });
        function SelectAll(aControl) {
            var chk = document.getElementById("myGrid").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
            }
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
    </script>

    <style type="text/css">
        .divMain
        {
            margin-left: 5px;
            margin-top: 5px;
        }
    </style>
</head>
<body onload="gudingbiaotou();">
    <form id="form1" runat="server">
    <div class="divMain">
        <div style="height: 25px;">
            <input class="baseButton" id="btn_refresh" value="刷 新" type="button" />
            <input type="button" id="btnSelect" value="查 询" class="baseButton" />
            <input type="button" id="btn_Add" value="添 加" class="baseButton" />
            <input type="button" id="btn_Edit" value="修 改" class="baseButton" />
            <input type="button" id="btn_Delete" value="删 除" class="baseButton" />
            <input type="button" id="btn_View" value="详细信息" class="baseButton" />
            <input type="button" id="btn_Submint" value="提交审核" class="baseButton" />
            <input type="button" id="btn_Revert" value="审批撤销" class="baseButton" />
            <asp:Button runat="server" ID="btnExportExcel" Text="导出Excel" CssClass="baseButton"
                OnClick="btnExportExcel_Click" />
        </div>
        <div id="trSelect" style="display: none;">
            <table class="baseTable" style="margin-left: 0px;">
                <tr>
                    <td style="text-align: right">
                        申请日期从：
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppDateFrm" runat="server" Width="120px" onfocus="setday(this);"></asp:TextBox>
                    </td>
                    <td style="text-align: right">
                        到：
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppDateTo" runat="server" Width="120px" onfocus="setday(this);"></asp:TextBox>
                    </td>
                    <td style="text-align: right">
                        报销人：
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppPersioncode" runat="server" Width="120px"></asp:TextBox>
                    </td>
                    <td style="text-align: right">
                        单位：
                    </td>
                    <td>
                        <asp:TextBox ID="txtDeptCode" runat="server" Width="120px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        申请单号
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillCode" runat="server" Width="120px"></asp:TextBox>
                    </td>
                    <td colspan="6">
                        <asp:Button ID="btn_Select" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_Select_click" />
                        <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <div id="header" style="overflow: hidden;">
            </div>
            <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 1100px; height: 390px;">
                <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    PageSize="17" CssClass="myGrid" Style="table-layout: fixed; word-wrap: break-word;"
                    Width="100%" AllowPaging="True" OnItemDataBound="myGrid_ItemDataBound">
                    <Columns>
                        <asp:BoundColumn DataField="billCode" HeaderText="申请编号">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="True" CssClass=" myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="billUserName" HeaderText="申请人">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="True" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="billDeptName" HeaderText="申请部门">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Width="140" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Width="140" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="BillDate" HeaderText="申请日期" DataFormatString="{0:D}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="BillJe" HeaderText="金额" DataFormatString="{0:N2}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="维修资产">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="stepid" HeaderText="审批状态">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="True" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ShuoMing" HeaderText="说明摘要">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle Visible="False" />
                </asp:DataGrid>
            </div>
        </div>
        <div>
            &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
            <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
            <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
            <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
            第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
            </asp:DropDownList>
            页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
            <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                runat="server"></asp:Label>条
        </div>
    </div>
    &nbsp;&nbsp;审批流程：<span id="wf">
        <asp:Label ID="lblShlc" runat="server"></asp:Label></span>
    </form>
</body>
</html>
