﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ZiChanWeiXiuRiZhiIndex.aspx.cs"
    Inherits="webBill_ZiChanGuanLi_ZiChanWeiXiuRiZhiIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>维修记录</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        var status = "none";

        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                if ($(this).find("td")[0] != null && $(this).find("td")[0].innerHTML != "") {
                    var billCode = $(this).find("td")[0].innerHTML;
                    $("#hd_billCode").val(billCode);
                }
                $.post("../../webBill/MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
                //事件2
                gudingbiaotou();
            });
            //修改
            $("#btn_edit").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行！");
                    return false;
                }

                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("ZiChanWeiXiuRiZhiDetail.aspx?Ctrl=Edit&Code=" + billcode);
            });
            //删除
            $("#btn_delete").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行！");
                    return false;
                }
                var varshbj = checkrow.find("td")[7].innerHTML;

                if (varshbj.length > 6) {
                    alert("已经提交审批不允许删除！");
                    return false;
                }

                if (!confirm("您确定要删除选中的数据吗?")) {
                    return false;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                $("#hdDelCode").val(billcode);
            });
            //部门选择
            $("#txtPaymentDeptCode").autocomplete({
                source: availableTags
            });

            //选择资产
            $("#txtCode").autocomplete({
                source: zcTags
            });
            //选择人员
            $("#txtname").autocomplete({
                source: userTags
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
                    return false;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("ZiChanWeiXiuRiZhiDetail.aspx?Ctrl=look&Code=" + billcode);
            });

        });


        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:900px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("Button6").click();
            }
        }
        function openSplc(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        function openprint(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:1000px;status:no;scroll:yes');
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
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                <asp:Button ID="Button6" runat="server" CssClass="baseButton" OnClick="Button6_Click1"
                    Text="刷 新" />
                <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" onclick="openDetail('ZiChanWeiXiuRiZhiDetail.aspx?Ctrl=Add&par=' + Math.random());" />
                <input type="button" value="修 改" id="btn_edit" class="baseButton" />
                <asp:Button ID="btn_delete" runat="server" Text="删 除" class="baseButton" OnClick="btn_delete_Click" />
                <input type="button" value="详细信息" id="btn_look" class="baseButton" />
            </td>
        </tr>
        <tr id="trSelect" style="display: none;">
            <td align="left">
                <div style="float: left">
                    <table class="baseTable" style="text-align: left;">
                        <tr>
                            <td>
                                资产：
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                维修人：
                            </td>
                            <td>
                                <asp:TextBox ID="txtname" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                维修部门：
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentDeptCode" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                是否提交过审批：
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList1" runat="server" Width="120px">
                                    <asp:ListItem Value="">-全部-</asp:ListItem>
                                    <asp:ListItem Value="1">是</asp:ListItem>
                                    <asp:ListItem Value="0">否</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="2" style="text-align: center">
                                <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
                                <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
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
                <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 1100px; height: 390px;">
                    <asp:DataGrid ID="myGrid" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid"
                        ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="17" Style="table-layout: fixed; word-wrap: break-word"
                        Width="100%">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        <Columns>
                            <asp:BoundColumn DataField="listid" HeaderText="日志编号">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" Wrap="False" CssClass="hiddenbill" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="hiddenbill" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="zcnames" HeaderText="资产">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Left" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="wxname" HeaderText="维修人">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Width="100" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False"
                                    CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Width="100"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" CssClass="myGridItem"
                                    HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="wxbmname" HeaderText="维修部门">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItemCenter" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="WeiXiuJinE" HeaderText="维修金额" DataFormatString="{0:N2}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Width="80" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                    CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Width="80" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" CssClass="myGridItemRight"
                                    HorizontalAlign="Right" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="tbilldate" HeaderText="维修时间" DataFormatString="{0:D}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Width="80" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                    CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Width="80" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" CssClass="myGridItemCenter"
                                    HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="tjsp" HeaderText="是否提交过审批">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Width="80" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                    CssClass="hiddenbill" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Width="80" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" CssClass="hiddenbill"
                                    HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ShenPiDanCode" HeaderText="审批单号">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Width="100" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                    CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Width="100" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" CssClass="myGridItemCenter"
                                    HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="dictypecode" HeaderText="维修类别">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Width="80" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                    CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Width="80" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" CssClass="myGridItemCenter"
                                    HorizontalAlign="Center" />
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
                    runat="server"></asp:Label>条
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hd_billCode" runat="server" />
                <asp:HiddenField ID="hdDelCode" runat="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
