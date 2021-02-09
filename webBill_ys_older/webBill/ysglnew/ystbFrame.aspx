<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ystbFrame.aspx.cs" Inherits="webBill_ysglnew_ystbFrame" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        $(function () {
            initMainTableClass("<%=myGrid.ClientID%>");
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[1] != null) {
                    var billCode = $(this).find("td")[1].innerHTML;
                    $("#choosebill").val(billCode);
                }
                var xkfx = '<%=Request["xkfx"]%>';
                if (xkfx == "1") {
                    $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode, "xkfx": xkfx }, function (data, status) {
                        if (status == "success") {
                            $("#wf").html(data);
                        }
                    });
                } else {
                    $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function (data, status) {
                        if (status == "success") {
                            $("#wf").html(data);
                        }
                    });
                }

            });
            $("#btn_delete").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[7].innerHTML;
                var lx = zt.substring(zt.length - 2, zt.length);
                if (zt != "未提交" && lx != "否决") {
                    alert("该行已提交,不能删除！");
                    return;
                }
                if (!confirm("您确定要删除选中的单据吗?")) {
                    return;
                }
                var billcode = checkrow.find("td")[1].innerHTML;
                $.post("../MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": "ystb" }, function (data, status) {
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
                var varmyguid = document.getElementById("<%=myGrid.ClientID %>");
                var guidlength = varmyguid.rows.length;
                var ichecked = 0;
                var billcode = "";
                var flowid = '<%= flowid%>';
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
                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": flowid }, function (data, status) {
                        if (data == "-1" || data == "-2") {
                            alert("撤销失败！");
                        } else if (data == "-3") {
                            alert("撤销失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                        } else {
                            if (status == "success") {
                                alert("成功撤销" + ichecked + "条记录！");
                                //location.replace(location.href);
                                $("#Button6").click();
                            }
                        }
                    });
                } else {
                    alert('请勾选状态为‘未提交’的记录。');
                }
                <%--var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[6].innerHTML;
                if (zt == "未提交" || zt == "审批通过") {
                    alert("该单据不能撤销操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    var flowid = '<%= flowid%>';
                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": flowid }, function (data, status) {
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
                }--%>
            });
            //提交按钮
            $("#btn_summit").click(function () {
                var yzmag = $("#Hidtjzt").val();
                var isdz = '<%=Request["isdz"]%>';
                var xkfx = '<%=Request["xkfx"]%>';//是否是新开分校
                // if (xkfx.length == 0) {//如果不是新开分校验证
                if (yzmag.length > 1 && isdz == '1') {
                    alert(yzmag);
                    return;
                }
                // }

                var varmyguid = document.getElementById("<%=myGrid.ClientID %>");
                    var guidlength = varmyguid.rows.length;
                    var ichecked = 0;
                    var billcode = "";
                    var flowid = '<%= flowid%>';
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

                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": flowid }, function (data, status) {
                        if (data == "-1" || data == "-2") {
                            alert("提交失败！");
                        } else if (data == "-3") {
                            alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                        } else {
                            if (status == "success") {
                                alert("成功提交" + ichecked + "条记录！");
                                //location.replace(location.href);
                                $("#Button6").click();
                            }
                        }
                    });
                } else {
                    alert('请勾选要提交的记录。');
                }
                <%--var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[6].innerHTML;
                var dept = checkrow.find("td")[8].innerHTML;

                if (zt != "未提交") {
                    alert("单据已提交.不要重复操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    var flowid = '<%= flowid%>';


                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": flowid, "dept": dept }, function (data, status) {
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
                }--%>

            });
        });

        function editCheck() {
            if ($("#drpSelectNd").val() == "0") {
                alert("请选择年度");
                return false;
            }
            else {

                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var xmcode = "";
                if (checkrow.find("td")[10].innerHTML != undefined && checkrow.find("td")[10].innerHTML != "&nbsp;") {
                    xmcode = checkrow.find("td")[10].innerHTML;
                };
                $("#hidxmcode").val(xmcode);


                var deptcode = $("#LaDept").val();
                var deptcode = $("#LaDept").val();
                var yskmtype = '<%= Request["yskmtype"]%>';
                var tbtype = '<%= Request["tbtype"]%>';
                var limittotal = '<%= Request["limittotal"]%>';
                var jecheckflg = '<%=Request["jecheckflg"]%>';
                var isdz = '<%= Request["isdz"]%>'//是否是大智学校的标记
                var xkfx = '<%=Request["xkfx"]%>';//新开分校填报
                openDetail("cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + $("#drpSelectNd").val() + "&type=ystb" + "&yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal + "&jecheckflg=" + jecheckflg + "&isdz=" + isdz + "&xmcode=" + xmcode + "&xkfx=" + xkfx);
                return false;
            }
        }

        function lookCheck() {
            if ($("#drpSelectNd").val() == "0") {
                alert("请选择年度");
                return false;
            }
            else {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var xmcode = "";
                if (checkrow.find("td")[10].innerHTML != undefined && checkrow.find("td")[10].innerHTML != "&nbsp;") {
                    xmcode = checkrow.find("td")[10].innerHTML;
                };
                $("#hidxmcode").val(xmcode);


                var deptcode = $("#LaDept").val();
                var yskmtype = '<%= Request["yskmtype"]%>';
                var tbtype = '<%= Request["tbtype"]%>';
                var limittotal = '<%= Request["limittotal"]%>';
                var xkfx = '<%=Request["xkfx"]%>';//新开分校填报
                openDetail("cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + $("#drpSelectNd").val() + "&type=ystb&look=look" + "&yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal + "&xmcode=" + xmcode + "&xkfx=" + xkfx);
                return false;
            }
        }
        function openDetail(openUrl) {
            //var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1200px;status:no;scroll:yes');
            //if (returnValue == "1") {
            //    document.getElementById("Button6").click();
            //}
            window.location.href = openUrl;
        }
        function openLookSpStep(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:1000px;status:no;scroll:yes');
        }
        function gotoAdd() {

            if ($("#drpSelectNd").val() == "0") {
                alert("请选择财年年度");
                return false;
            }
            else {
                var deptcode = $("#LaDept").val();
                var yskmtype = '<%= Request["yskmtype"]%>';
                var tbtype = '<%= Request["tbtype"]%>';
                var limittotal = '<%= Request["limittotal"]%>';
                var jecheckflg = '<%=Request["jecheckflg"]%>';
                var isdz = '<%= Request["isdz"]%>';//是否是大智学校的标记
                var xkfx = '<%=Request["xkfx"]%>';//新开分校填报
                var nd = $("#drpSelectNd").val();
                var wdheight = '<%=Request["wdheight"]%>';//页面高度


                if (xkfx == '1') {
                    openDetail("../xmsz/xmList.aspx?deptCode=" + deptcode + "&wdheight=" + wdheight + "&nd=" + nd + "&type=ystb" + "&yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal + "&jecheckflg=" + jecheckflg + "&isdz=" + isdz + "&xkfx=" + xkfx);

                } else {
                    openDetail("cwtbDetail.aspx?deptCode=" + deptcode + "&wdheight=" + wdheight + "&nd=" + nd + "&type=ystb" + "&yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal + "&jecheckflg=" + jecheckflg + "&isdz=" + isdz);

                }


                return false;
            }
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
        <table cellpadding="0" cellspacing="0" width="90%" style="margin-left: 5px">
            <tr>
                <td style="height: 30px">财年：<asp:DropDownList ID="drpSelectNd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpSelectNd_SelectedIndexChanged">
                    <%--  <asp:ListItem Text="未选择" Value="0"></asp:ListItem>--%>
                </asp:DropDownList>
                    部门：<asp:DropDownList runat="server" ID="LaDept" AutoPostBack="True" OnSelectedIndexChanged="LaDept_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Button ID="Button6" runat="server" Text="刷 新" CssClass="baseButton" OnClick="Button6_Click" />
                    <asp:Button ID="Button1" runat="server" Text="增 加" CssClass="baseButton" OnClientClick="return gotoAdd();" />
                    <asp:Button ID="btn_edit" runat="server" Text="修 改" OnClientClick="return editCheck()"
                        CssClass="baseButton" />
                    <input type="button" value="删 除" id="btn_delete" class="baseButton" />
                    <asp:Button ID="btn_look" runat="server" Text="详细信息" OnClientClick="return lookCheck()"
                        CssClass="baseButton" />
                    <input type="button" value="审批提交" id="btn_summit" class="baseButton" />
                    <input type="button" value="审批撤销" id="btn_replace" class="baseButton" /><input type="button"
                        class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" Width="1260" PageSize="17" OnItemDataBound="myGrid_ItemDataBound">
                            <Columns>
                                <asp:TemplateColumn ItemStyle-Width="60" HeaderStyle-Width="60">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                            Text="全选" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="false" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="billCode" HeaderText="单据编号">
                                    <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="myGridItem hiddenbill" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billName" HeaderText="预算过程" HeaderStyle-Width="160" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDept" HeaderText="填报单位" HeaderStyle-Width="160" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billUser" HeaderText="制单人" HeaderStyle-Width="160" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDate" DataFormatString="{0:D}" HeaderText="制单日期"
                                    HeaderStyle-Width="160" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemCenter" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billje" HeaderText="金额" HeaderStyle-Width="160" ItemStyle-Width="160" DataFormatString="{0:N}">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="stepid" HeaderText="审核状态" HeaderStyle-Width="160" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="mind" HeaderText="驳回理由" HeaderStyle-Width="160" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="showbilldept" HeaderText="填报单位" HeaderStyle-Width="160" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem hiddenbill" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="note3" HeaderText="项目编号" HeaderStyle-Width="160" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem hiddenbill" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="xmname" HeaderText="项目名称" HeaderStyle-Width="160" ItemStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader " Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem " Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                            </Columns>
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
                            <td>审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
                            </td>
                            <td id="wf"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="choosebill" runat="server" />
        <asp:HiddenField ID="Hidtjzt" runat="server" />
        <asp:HiddenField ID="hidxmcode" runat="server" />
        <script type="text/javascript">
            function SelectAll(aControl) {
                var chk = document.getElementById("myGrid").getElementsByTagName("input");
                for (var s = 0; s < chk.length; s++) {
                    chk[s].checked = aControl.checked;
                }
            }
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
