<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travelclaimsFrame.aspx.cs"
    Inherits="webBill_bxgl_travelclaimsFrame" %>


<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>旅差费报销单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>
<script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
        var status = "none";
        $(function() {
        $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                parent.helptoggle();
                }
            });
            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#btn_edit").click(function() {
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
                var billcode = checkrow.find("td")[1].innerHTML;
                var dydj = "02";
                var rqdydj = '<%=Request["dydj"] %>';
                if (rqdydj != null && rqdydj != undefined && rqdydj != '') {
                    dydj = rqdydj;
                }
                openDetail("clbxDetailForGK.aspx?type=edit&billCode=" + billcode + "&dydj=" + dydj);
            });
            $("#btn_delete").click(function() {
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
                var billcode = checkrow.find("td")[1].innerHTML;
                $.post("../MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": "gkbx" }, function(data, status) {
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
            $("#btn_look").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[1].innerHTML;
                openDetail("clbxDetailForGK.aspx?type=look&billCode=" + billcode);
            });
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                if ($(this).find("td")[0] != null) {
                    var billCode = $(this).find("td")[1].innerHTML;
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
            //部门选择
            $("#txtLoanDeptCode").autocomplete({
                source: availableTags
            });
            //人员选择
            $("#txtloannamecode").autocomplete({
                source: avaiusertb
            });
            //撤销
            $("#btn_replace").click(function() {
                var varmyguid = document.getElementById("<%=myGrid.ClientID %>");
                var guidlength = varmyguid.rows.length;
                var ichecked = 0;
                var billcode = "";
                //将选中的记录的code组合成 code,code2,code3,的格式
                for (var i = 0; i < guidlength; i++) {
                    if (varmyguid.rows[i].cells[0].getElementsByTagName("input")[0].checked) {
                        var evebillcode = varmyguid.rows[i].cells[1].innerHTML;
                        if (evebillcode.length > 6 && evebillcode != null && evebillcode != undefined) {
                            billcode += evebillcode + ",";
                            ichecked++;
                        }
                    }
                }
                //如果选中的记录大于等于1  就提交给处理程序  当然处理程序也有所改动 详见代码
                if (ichecked >= 1) {
                    billcode = billcode.substring(0, billcode.length - 1);
                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "gkbx" }, function(data, status) {
                        if (data == "-1" || data == "-2") {
                            alert("提交失败！");
                        } else if (data == "-3") {
                            alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                        } else {
                            if (status == "success") {
                                alert("成功撤销" + ichecked + "条记录！");
                                location.replace(location.href);
                            }
                        }
                    });
                } else {
                    alert('请勾选要提交的记录。');
                }
                //                var checkrow = $(".highlight");
                //                if (checkrow.val() == undefined) {
                //                    alert("请先选择行");
                //                    return;
                //                }
                //                var zt = checkrow.find("td")[7].innerHTML;
                //                if (zt == "未提交" || zt == "审批通过") {
                //                    alert("该单据不能撤销操作!");
                //                } else {
                //                    var billcode = checkrow.find("td")[1].innerHTML;
                //                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "gkbx" }, function(data, status) {
                //                        if (status == "success") {
                //                            if (data == "1") {
                //                                alert("撤销成功！");
                //                                checkrow.find("td")[7].innerHTML = "未提交";
                //                            }
                //                            else {
                //                                alert("单据以进入审批，不能撤销");
                //                            }
                //                        }
                //                        else {
                //                            alert("失败");
                //                        }
                //                    });
                //                }
            });
            //打印预览
            $("#btn_print").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[1].innerHTML;
                window.open("../YbbxPrint/chailvbxPrint.aspx?billName=" + billcode);
            });
            //提交
            $("#btn_summit").click(function() {
                var varmyguid = document.getElementById("<%=myGrid.ClientID %>");
                var guidlength = varmyguid.rows.length;
                var ichecked = 0;
                var billcode = "";
                //将选中的记录的code组合成 code,code2,code3,的格式
                for (var i = 0; i < guidlength; i++) {
                    if (varmyguid.rows[i].cells[0].getElementsByTagName("input")[0].checked) {
                        var evebillcode = varmyguid.rows[i].cells[1].innerHTML;
                        var zt = varmyguid.rows[i].cells[7].innerHTML;
                        if (zt != '未提交') {
                            continue;
                        }
                        if (evebillcode.length > 6 && evebillcode != null && evebillcode != undefined) {
                            billcode += evebillcode + ",";
                            ichecked++;
                        }
                    }
                }
                //如果选中的记录大于等于1  就提交给处理程序  当然处理程序也有所改动 详见代码
                if (ichecked >= 1) {
                    billcode = billcode.substring(0, billcode.length - 1);

                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": "gkbx" }, function(data, status) {
                        if (data == "-1" || data == "-2") {
                            alert("提交失败！");
                        } else if (data == "-3") {
                            alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                        } else {
                            if (status == "success") {
                                alert("成功提交" + ichecked + "条记录！");
                                location.replace(location.href);
                            }
                        }
                    });
                } else {
                    alert('请勾选要提交的记录。');
                }
                //                var checkrow = $(".highlight");
                //                if (checkrow.val() == undefined) {
                //                    alert("请先选择行");
                //                    return;
                //                }
                //                var zt = checkrow.find("td")[7].innerHTML;
                //                if (zt != "未提交") {
                //                    alert("单据已提交.不要重复操作!");
                //                } else {
                //                    var billcode = checkrow.find("td")[1].innerHTML;
                //                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": "gkbx" }, function(data, status) {
                //                        if (data == "-1" || data == "-2") {
                //                            alert("提交失败！");
                //                        } else if (data == "-3") {
                //                            alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                //                        } else {
                //                            if (status == "success") {
                //                                alert("提交成功！");
                //                                checkrow.find("td")[7].innerHTML = data;
                //                            }
                //                        }
                //                    });
                //                }
            });
        });


        function openDetail(openUrl) {
            var returnValue = window.showModelessDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');

            //            if (returnValue == undefined || returnValue == "") {
            //                return false;
            //            }
            //            else {
            //                document.getElementById("Button2").click();
            //            }
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
            var dydj = "04";
            var rqdydj = '<%=Request["dydj"] %>';
            if (rqdydj != null && rqdydj != undefined && rqdydj != '') {
                dydj = rqdydj;
            }
            window.showModelessDialog('clbxDetailForGK.aspx?type=add&par=' + Math.random() + "&dydj=" + dydj, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
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
    <table cellpadding="0" cellspacing="0" >
        <tr>
            <td style="height: 30px">
                <input id="btnRefresh" type="button" class="baseButton" value="刷 新" onclick="javascript:location.replace(location.href);" />
                <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" onclick="add();" />
                <input type="button" value="修 改" id="btn_edit" class="baseButton" />
                <input type="button" value="删 除" id="btn_delete" class="baseButton" />
                <input type="button" value="详细信息" id="btn_look" class="baseButton" />
                <input type="button" value="审批提交" id="btn_summit" class="baseButton" />
                <input type="button" value="审批撤销" id="btn_replace" class="baseButton" />
                <input type="button" value="打印预览" id="btn_print" class="baseButton" />
                  <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
            </td>
        </tr>
        <tr id="trSelect" style="display: none;">
            <td align="left" colspan="3">
                <div style="float: left">
                    <table class="baseTable" style="text-align: left;">
                        <tr>
                            <td>
                                申请日期从：
                            </td>
                            <td>
                                <asp:TextBox ID="txtLoanDateFrm" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                到：
                            </td>
                            <td>
                                <asp:TextBox ID="txtLoanDateTo" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                报销人：
                            </td>
                            <td>
                                <asp:TextBox ID="txtloannamecode" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                单位：
                            </td>
                            <td>
                                <asp:TextBox ID="txtLoanDeptCode" runat="server" Width="120px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                单据编号：
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                审批状态:
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
            <td>
                <div id="divgrid" style="overflow-x: auto;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                            <asp:TemplateColumn ItemStyle-Width="50" HeaderStyle-Width="50">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                        Text="全选" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="false" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="billName" HeaderText="单据编号" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billUserName" HeaderText="制单人" ItemStyle-Width="100"
                                HeaderStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" HeaderText="申请日期" DataFormatString="{0:D}"
                                ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Width="120" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="false"
                                    CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Width="120" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDept" HeaderText="所属部门" DataFormatString="{0:D}"
                                ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billJe" HeaderText="报销总额" DataFormatString="{0:F2}" ItemStyle-Width="100"
                                HeaderStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="false" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="bxsm" HeaderText="出差说明" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="审批状态" DataField="stepid" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="isgk" HeaderText="归口费用" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <%--<asp:BoundColumn DataField="gkDept" HeaderText="归口部门">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>--%>
                            <asp:BoundColumn  HeaderText="审批人" ItemStyle-Width="250" HeaderStyle-Width="250">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                            </asp:BoundColumn>
                             <asp:BoundColumn DataField="mind" HeaderText="驳回理由" ItemStyle-Width="150" HeaderStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                <ItemStyle  Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
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
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
                <table>
                    <tr>
                        <td>
                            审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdYbbxNeedAudit" runat="server" />
                        </td>
                        <td id="wf">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
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
