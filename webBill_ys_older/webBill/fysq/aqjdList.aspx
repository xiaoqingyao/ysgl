<%@ Page Language="C#" AutoEventWireup="true" CodeFile="aqjdList.aspx.cs" Inherits="webBill_fysq_aqjdList" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>请假单列表页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script language="javascript" type="Text/javascript">
        $(function() {
            gudingbiaotounew($("#myGrid"), 380);
           
            //修改
            $("#btn_edit").click(function() {
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
                openDetail("aqjdDetail.aspx?Ctrl=Update&Code=" + billcode);
            });
            
            $("#btn_delete").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[4].innerHTML;
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
                $.post("../MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": "ccsq" }, function(data, status) {
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
            $("#btn_look").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("aqjdDetail.aspx?Ctrl=View&Code=" + billcode);
            });
            //打印预览
            $("#btn_print").click(function() {
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
            $("#btn_print2").click(function() {
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
            $("#to_List").click(function() {
                window.location.href = "aqjdList.aspx";
            });
            //取消
            $("#btn_cancle").click(function() {
                document.getElementById("trSelect").style.display = "none";
            });

            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                if ($(this).find("td")[0] != null && $(this).find("td")[0].innerHTML != "") {
                    var billCode = $(this).find("td")[0].innerHTML;

                }
                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
           
            //撤销单据提交
            $("#btn_replace").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[4].innerHTML;
                if (zt == "未提交" || zt == "审批通过") {
                    alert("该单据不能撤销操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "qjd", "billtype": "qjd" }, function(data, status) {
                        if (status == "success") {
                            if (data == "1") {
                                alert("撤销成功！");
                                checkrow.find("td")[4].innerHTML = "未提交";
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
            $("#btn_summit").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[4].innerHTML;
                if (zt != "未提交") {
                    alert("单据已提交.不要重复操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": "qjd", "billtype": "qjd" }, function(data, status) {
                        if (data == "-1" || data == "-2") {
                            alert("提交失败！");
                        } else if (data == "-3") {
                            alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                        } else {
                            if (status == "success") {
                                alert("提交成功！");
                                checkrow.find("td")[4].innerHTML = data;
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
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
    </script>

</head>
<body>
    <form id="form2" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td style="height: 27px">
             <input type="button" value="刷新" id="to_List" class="baseButton" runat="server" />
                <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" onclick="openDetail('aqjdDetail.aspx?Ctrl=Add&par=' + Math.random());" />
                <input type="button" value="修 改" id="btn_edit" class="baseButton" runat="server" />
                <input type="button" value="删 除" id="btn_delete" class="baseButton" runat="server" />
                <input type="button" value="详细信息" id="btn_look" class="baseButton" runat="server" />
                <input type="button" value="审批提交" id="btn_summit" class="baseButton" runat="server" />
                <input type="button" value="审批撤销" id="btn_replace" class="baseButton" runat="server" />
                <input type="button" value="打印预览" id="btn_print" class="baseButton" runat="server" />
                <input type="button" value="打印预览(管理单)" id="btn_print2" class="baseButton" runat="server" />
            </td>
        </tr>
       
        <tr>
            <td>
             <div id="header" style="overflow: hidden;">
                </div>
                <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 1100px; height: 380px;">
                    <asp:DataGrid ID="myGrid" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid"
                        ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="17" Style="table-layout: fixed; word-wrap:break-word;" Width="100%" OnItemDataBound="myGrid_ItemDataBound">
                        <ItemStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundColumn DataField="billCode" HeaderText="单据编号">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptName" HeaderText="填报单位">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billUser" HeaderText="报告人">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" HeaderText="报告日期" DataFormatString="{0:D}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="审批状态" DataField="stepID">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" Position="Top" Mode="NumericPages" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px"></PagerStyle>
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
                &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                </asp:DropDownList>
                页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            审核流程：
                        </td>
                        <td id="wf">
                            <asp:Label ID="lblShlc" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
