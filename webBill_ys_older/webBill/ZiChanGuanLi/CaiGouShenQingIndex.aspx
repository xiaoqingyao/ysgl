<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CaiGouShenQingIndex.aspx.cs"
    Inherits="ZiChan_ZiChanGuanLi_CaiGouShenQingIndex" %>

<%@ Register Assembly="PaginationControl" Namespace="PaginationControl" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>采购申请</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
        td
        {
            text-align: left;
        }
    </style>
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                 if( $(this).find("td")[0]!=null&& $(this).find("td")[0].innerHTML!="")
                {
                  var billCode = $(this).find("td")[0].innerHTML;
                
                }
              
                if (billCode != "单据编号") {
                    $(this).addClass("highlight");
                    $.post("../../webBill/MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                        if (status == "success") {
                            $("#wf").html(data);
                        }
                    });
                }
            });
            $("#btn_sx").click(function(){
                 location.replace(location.href);
            });
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
                    $.post("../../webBill/MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": "zccgsq", "billtype": "zccgsq" }, function(data, status) {
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
                    $.post("../../webBill/MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "zccgsq", "billtype": "zccgsq" }, function(data, status) {
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
            $("#txtdept").autocomplete({
                source: availableTags
            });
        });
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:1000px;status:no;scroll:yes');
            if (returnValue == "1") {
                location.replace(location.href);
            }
        }
        function Edit() {
            var checkrow = $(".highlight");
            if (checkrow.val() == undefined) {
                alert("请选择行！");
                return;
            }
            else {
                var lx = checkrow.find("td")[4].innerHTML;
                if (lx == "未提交") {
                    var bh = checkrow.find("td")[0].innerHTML;
                    openDetail('CaiGouShenQingDetail.aspx?type=Edit&&bh=' + bh);
                } else {
                alert("该行已经提交，不允许修改！");
                    return false;
                }
            }
        }
        function Look() {
            var checkrow = $(".highlight");
            if (checkrow.val() == undefined) {
                alert("请选择行！");
                return;
            }
            else {
                var bh = checkrow.find("td")[0].innerHTML;
                openDetail('CaiGouShenQingDetail.aspx?type=look&&bh=' + bh);
            }
        }
        function checkDelete() {
            var checkrow = $(".highlight");
            if (checkrow.val() == undefined) {
                alert("请选择行！");
                return false;
            }
            else {
                var lx = checkrow.find("td")[4].innerHTML;
                if (lx == "未提交") {
                    var bh = checkrow.find("td")[0].innerHTML;
                    $("#HiddenBh").val(bh);
                    return confirm("你确定要删除编号为" + bh + "的单据吗？");
                }
                else {
                    alert("该行已经提交,不允许删除！");
                    return false;
                }
            }
        }
        function opendiv() {
            document.getElementById("cxid").style.display = document.getElementById("cxid").style.display == "none" ? "" : "none";
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
    <div>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td>
                    <div class="baseDiv" style="margin-bottom: 3px; margin-top: 3px">
                        <input type="button" value="刷 新" id="btn_sx" class="baseButton" />
                        <input type="button" value="查 询" id="Button2" class="baseButton" onclick="opendiv()" />
                        <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" onclick="openDetail('CaiGouShenQingDetail.aspx?type=Add')" />
                        <input type="button" value="修 改" id="btn_edit" class="baseButton" onclick="Edit()" />
                        <asp:Button ID="btn_del" runat="server" Text="删 除" class="baseButton" OnClientClick="return checkDelete()"
                            OnClick="btn_del_Click" />
                        <input type="button" value="详细信息" id="btn_look" class="baseButton" onclick="Look()" />
                        <input type="button" value="审批提交" id="btn_summit" class="baseButton" />
                        <input type="button" value="审批撤销" id="btn_replace" class="baseButton" />
               <asp:Button ID="Button4" runat="server" CssClass="baseButton" Text="导出Excel" OnClick="Button2_Click" />
                    </div>
                   <div id="cxid" style="display: none; float:left;">
                        <table class="baseTable" style="text-align: left">
                            <tr>
                                <td>
                                    申请单号：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtbh" runat="server" Width="120px"></asp:TextBox>
                                </td>
                                <td>
                                    时间：
                                </td>
                                <td>
                                    <asp:TextBox ID="txb_sqrqbegin" runat="server" Width="120px" onfocus="javascript:setday(this);"></asp:TextBox>至<asp:TextBox
                                        ID="txb_sqrqend" runat="server" Width="120px" onfocus="javascript:setday(this);"></asp:TextBox>
                                </td>
                                <td>
                                    申请单位 ：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdept" runat="server" Width="120px"></asp:TextBox>
                                </td>
                            
                                <td style="text-align: center">
                                    <asp:Button runat="server" Text="确 定" ID="btn_cx" CssClass="baseButton" OnClick="btn_cx_Click" />
                                    <input type="button" value="取 消" id="Button3" class="baseButton" onclick="javascript:document.getElementById('cxid').style.display='none'" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="header">
                    </div>
                    <div class="baseDiv" id="main" style="overflow-y: scroll; margin-top: -1px; width: 1100px;
                        height: 380px;">
                        <asp:DataGrid ID="myGrid" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid"
                            ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                            AllowPaging="True" PageSize="17" Style="table-layout: fixed; word-wrap: break-word;"
                            Width="100%" OnItemDataBound="myGrid_ItemDataBound">
                            <ItemStyle HorizontalAlign="Center" />
                            <Columns>
                               <asp:BoundColumn DataField="billcode" HeaderText="单据编号">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billdept" HeaderText="申请单位">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billuser" HeaderText="申请人">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billdate" HeaderText="申请日期" DataFormatString="{0:D}">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Center" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="审批状态" DataField="stepID" ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" HorizontalAlign="Center" />
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
                    <cc1:paginationtogv id="PaginationToGV1" runat="server" ondatabinding="PaginationToGV1_DataBinding"
                        ongvbind="PaginationToGV1_GvBind" />
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
            </tr>
        </table>
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="HiddenBh" runat="server" />
    </div>
    </form>
</body>
</html>
