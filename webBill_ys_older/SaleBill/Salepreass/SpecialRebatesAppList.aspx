﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpecialRebatesAppList.aspx.cs"
    Inherits="SaleBill_Salepreass_SpecialRebatesAppList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特殊返利申请报告</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        var status = "none";

        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                 if( $(this).find("td")[0]!=null&& $(this).find("td")[0].innerHTML!="")
                {
                     var billCode = $(this).find("td")[0].innerHTML;
                $("#hd_billCode").val(billCode);
                }
               
                $.post("../../webBill/MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            //修改
             $("#btn_edit").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行！");
                    return;
                }
                var zt = checkrow.find("td")[4].innerHTML;
                if (zt != "未提交") {
                    alert("该行已提交,不能修改！");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("SpecialRebatesAppDetails.aspx?Ctrl=Edit&Code=" + billcode);
            });
            //删除
              $("#btn_delete").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行！");
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
                var billcode = checkrow.find("td")[0].innerHTML;
                $.post("../../webBill/MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": "tzcfld" }, function(data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            alert("删除成功！");
                            $(".highlight").remove();
                        }
                        else {
                            alert("删除失败！");
                        }
                    }
                    else {
                        alert("删除失败!");
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
             //详细信息
          $("#btn_look").click(function() {
               
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行！");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("SpecialRebatesAppDetails.aspx?Ctrl=look&Code=" + billcode);
            });
            
            
             //打印
          $("#btn_print").click(function() {
               
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行！");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openprint("SpecialRebateprint.aspx?Ctrl=print&Code=" + billcode);
            });
               //撤销单据提交
            $("#btn_replace").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行！");
                    return;
                }
                var zt = checkrow.find("td")[4].innerHTML;
                if (zt == "未提交" || zt == "审批通过") {
                    alert("该单据不能撤销操作！");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    $.post("../../webBill/MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "tsfl", "billtype": "tzcfld" }, function(data, status) {
                        if (status == "success") {
                            if (data == "1") {
                                alert("撤销成功！");
                                checkrow.find("td")[4].innerHTML = "未提交";
                            }
                            else {
                                alert("单据以进入审批，不能撤销！");
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
                    alert("请先选择行！");
                    return;
                }
                var zt = checkrow.find("td")[4].innerHTML;
                if (zt != "未提交") {
                    alert("单据已提交.不要重复操作！");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    $.post("../../webBill/MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": "tsfl", "billtype": "tzcfld" }, function(data, status) {
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
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:550px;dialogWidth:900px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("Button6").click();
            }
        } function openSplc(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        function openprint(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:900px;status:no;scroll:yes');
        }
        function openLookSpStep(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
  function gudingbiaotou() {
            var t = document.getElementById("<%=myGrid.ClientID%>");
              if(t==null||t.rows.length<1){
                return;
            }
            var t2 = t.cloneNode(true);
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

</head>
<body onload="gudingbiaotou();">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                <asp:Button ID="Button6" runat="server" CssClass="baseButton" OnClick="Button6_Click1"
                    Text="刷 新" />
                <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" onclick="openDetail('SpecialRebatesAppDetails.aspx?Ctrl=Add&par=' + Math.random());" />
                <input type="button" value="修 改" id="btn_edit" class="baseButton" />
                <input type="button" value="删 除" id="btn_delete" class="baseButton" />
                <input type="button" value="详细信息" id="btn_look" class="baseButton" />
                <input type="button" value="审批提交" id="btn_summit" class="baseButton" />
                <input type="button" value="审批撤销" id="btn_replace" class="baseButton" />
                <input type="button" value="打印预览" id="btn_print" class="baseButton" />
            </td>
        </tr>
        <tr id="trSelect" style="display: none;">
            <td align="left">
                <div style="float: left;">
                    <table class="baseTable" style="text-align: left;">
                        <tr>
                            <td>
                                申请日期从：
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
                                车架号：
                            </td>
                            <td>
                                <asp:TextBox ID="txtcarcode" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                申请单号：
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <%--<td>
                            有效期始：
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            有效期末：
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" Width="120px"></asp:TextBox>
                        </td>--%>
                        </tr>
                        <tr>
                            <td>
                                单据状态：
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlBillStatus" Width="124px">
                                    <asp:ListItem Value="">--全部--</asp:ListItem>
                                    <asp:ListItem Value="end">审核通过</asp:ListItem>
                                    <asp:ListItem Value="-1" Selected="True">未提交</asp:ListItem>
                                    <asp:ListItem Value="0">审核中</asp:ListItem>
                                    <asp:ListItem Value="1">审核驳回</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="6" style="text-align: center">
                                <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
                                <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                            </td>
                        </tr>
                </div>
    </table>
    </td> </tr>
    <tr>
        <td>
            <div id="header">
            </div>
            <div class="baseDiv" id="main" style="overflow-y: scroll; margin-top: -1px; width: 1100px;
                height: 380px;">
                <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Style="table-layout: fixed; word-wrap: break-word;" Width="100%" CssClass="myGrid"
                    OnItemDataBound="myGrid_ItemDataBound" AllowPaging="True" PageSize="20">
                    <Columns>
                        <asp:BoundColumn DataField="billCode" HeaderText="申请单号">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="deptName" HeaderText="申请部门">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="底盘号">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Left" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="billDate" HeaderText="申请日期" DataFormatString="{0:D}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemCenter" HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="审批状态" DataField="stepID">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="true" CssClass="myGridItemCenter" HorizontalAlign="Center" />
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle Visible="False" />
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
                runat="server"></asp:Label>条
        </td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td style="height: 10px">
                        审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
                    </td>
                    <td id="wf">
                    </td>
                </tr>
            </table>
        </td>
        <asp:HiddenField ID="hd_billCode" runat="server" />
    </tr>
    </table>
    </form>
</body>
</html>
