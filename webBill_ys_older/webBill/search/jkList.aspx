<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jkList.aspx.cs" Inherits="webBill_search_jkList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js"
        type="text/javascript" charset="UTF-8"></script>

    <style type="text/css">
        .linqi
        {
            background-color: #ffff88;
        }
        .chaoqi
        {
            background-color: #ff8877;
        }
        .chaoqiwanbi
        {
            background-color: #FCF0C1;
        }
        .hidden
        {
            display: none;
        }
    </style>

    <script language="javascript" type="Text/javascript">
        var status = "none";

        $(function() {
            $("#txtLoanDateFrm").datepicker();
            $("#txtLoanDateTo").datepicker();
            $("#txtRepsonDateFrom").datepicker();
            $("#txtRepsonDateTo").datepicker();
            initWindowHW();

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
            });


            //人员选择
            $("#txtRepsonCode").autocomplete({
                source: avaiusertb
            });
            $("#txtloannamecode").autocomplete({
                source: avaiusertb
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
                //                <a href="../../SaleBill/BorrowMoney/FundBorrowDetail.aspx">../../SaleBill/BorrowMoney/FundBorrowDetail.aspx</a>
                openDetail("../../SaleBill/BorrowMoney/FundBorrowDetail.aspx?Ctrl=look&Code=" + billcode);
            });
            $("#btn_back").click(function() {
                window.location.href = "FundHkList.aspx";
            });
            if ($("#ddlStatus").val() == '临期超期未还款') { document.getElementById('tianshu').style.display = 'block'; } else { document.getElementById('tianshu').style.display = 'none' }
        });


        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:900px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("Button6").click();
            }
        } function openSplc(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        function openprint(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:1000px;status:no;scroll:yes');
        }
        function openLookSpStep(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
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
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 27px">
                            <asp:Button ID="Button6" runat="server" CssClass="baseButton" OnClick="Button6_Click1"
                                Text="刷 新" />
                            <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                            <input type="button" value="详细信息" id="btn_look" class="baseButton" />
                            <asp:Button ID="Button1" runat="server" Text="导出Excel" OnClick="ToExcel_click" CssClass="baseButton" />
                            <input type="button" value="返 回" id="btn_back" runat="server" class="baseButton"
                                visible="false" />
                            <asp:Label runat="server" ForeColor="Red" ID="lblDept"></asp:Label>
                            <asp:Label runat="server" ForeColor="Red" ID="lbl_jkje"></asp:Label>
                            <asp:Label runat="server" ForeColor="Red" ID="lbl_yhje"></asp:Label>
                            <asp:Label runat="server" ForeColor="Red" ID="lbl_whje"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trSelect" style="display: none;">
                        <td align="left">
                            <table style="padding-top: 4px; border-collapse: separate; border: 1px solid #1855C6;
                                margin-right: auto; margin-top: 5px; margin-bottom: 5px;">
                                <tr>
                                    <td>
                                        借款日期从：
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
                                        申请单号：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOrderCode" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        经办日期从：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRepsonDateFrom" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td>
                                        到：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRepsonDateTo" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td>
                                        经办人：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRepsonCode" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        借款人：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtloannamecode" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td>
                                        冲减状态：
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DrCJstatus" runat="server" Width="120px">
                                            <asp:ListItem Value="">全部</asp:ListItem>
                                            <asp:ListItem Value="1">借款</asp:ListItem>
                                            <asp:ListItem Value="2">结算完毕</asp:ListItem>
                                            <asp:ListItem Value="3">冲减中</asp:ListItem>
                                            <asp:ListItem Value="4">借款+冲减中</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        审批状态：
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DrSPstatus" runat="server" Width="120px">
                                            <asp:ListItem Value="">全部</asp:ListItem>
                                            <asp:ListItem Value="-1">未提交+审批中</asp:ListItem>
                                            <asp:ListItem Value="end">审核通过</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        状态：
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlStatus" onchange="if(this.value=='临期超期未还款'){document.getElementById('tianshu').style.display='block';}else {document.getElementById('tianshu').style.display='none'}">
                                            <asp:ListItem Value="全部">--全部--</asp:ListItem>
                                            <asp:ListItem Value="超期未还款">超期未还款</asp:ListItem>
                                            <asp:ListItem Value="临期超期未还款">临期超期未还款</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <span id="tianshu" style="display: none">临期天数：<asp:TextBox runat="server" ID="txtTianShu"
                                            Width="120px"></asp:TextBox></span>
                                    </td>
                                    <td colspan="2" style="text-align: left">
                                        <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
                                        <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divgrid" style="overflow-x: auto;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CssClass="myGrid"
                        Width="1300" OnItemDataBound="myGrid_ItemDataBound" ShowFooter="true">
                        <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Center"
                            Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                            CssClass="myGridItem" />
                        <Columns>
                            <asp:BoundColumn DataField="billCode" HeaderText="申请单号">
                                <HeaderStyle Font-Italic="False" Width="120" Font-Overline="False" Font-Strikeout="False"
                                    HorizontalAlign="Left" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Width="120" Font-Overline="False" Font-Strikeout="False"
                                    Wrap="False" CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ResponsibleCode" HeaderText="借款人">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Left"
                                    Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="true"
                                    CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptName" HeaderText="部门">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Left"
                                    Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="true"
                                    CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="loandate" HeaderText="借款日期">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Left"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                                    CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="jksj" HeaderText="借款天数">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Center"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                                    CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billJe" HeaderText="借款金额" DataFormatString="{0:N2}">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Right"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                                    CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="loanStatus" HeaderText="状态">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Center"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                                    CssClass="myGridItem" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="loanType" HeaderText="结算方式">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Center"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                                    CssClass="myGridItemRight" HorizontalAlign="Right" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="审批状态" DataField="stepID">
                                <HeaderStyle Font-Italic="False" Width="100" Font-Overline="False" Font-Strikeout="False"
                                    HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Width="100" Font-Overline="False" Font-Strikeout="False"
                                    Wrap="true" CssClass="myGridItemCenter" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Note3" HeaderText="已冲减金额">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Center"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="true"
                                    CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="wcjmoney" HeaderText="未冲减金额">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Center"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="true"
                                    CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="note6" HeaderText="借款类型">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Center"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="true"
                                    CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="jbname" HeaderText="经办人">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Center"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="true"
                                    CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Respondate" HeaderText="经办日期">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Center"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                                    CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="chaoqidays" HeaderText="超出日期">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Center"
                                    Wrap="False" CssClass="hidden" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                                    CssClass="hidden" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="chaoqidays_wanbi" HeaderText="已经还款完毕的超出日期">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Center"
                                    Wrap="False" CssClass="hidden" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                                    CssClass="hidden" HorizontalAlign="Left" />
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
            <asp:HiddenField ID="hd_billCode" runat="server" />
            <td id="wf">
            </td>
        </tr>
        <tr>
            <td>
                颜色提示：<span style="background-color: #ffff88;">距离还款时间还剩3天</span> <span style="background-color: #ff8877;">
                    超期未还款</span> <span style="background-color: #FCF0C1;">超期还款完毕</span>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
