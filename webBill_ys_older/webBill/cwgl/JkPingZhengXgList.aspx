<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JkPingZhengXgList.aspx.cs" Inherits="webBill_cwgl_JkPingZhengXgList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>借款单列表</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"    type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js"type="text/javascript" charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">
        var status = "none";

        $(function() {
            $("#txtLoanDateFrm").datepicker();
            $("#txtLoanDateTo").datepicker();

            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 10));

            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                if ($(this).find("td")[0] != null && $(this).find("td")[0].innerHTML != "") {
                    var billCode = $(this).find("td")[0].innerHTML;
                    $("#hd_billCode").val(billCode);
                }
                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
           
           
            //部门选择
            $("#txtdept").autocomplete({
                source: availableTags
            });
            //人员选择
            $("#txtbilluser").autocomplete({
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
                var id = checkrow.find("td")[9].innerHTML;
                openDetail("../../SaleBill/BorrowMoney/FundHKDetail.aspx?Ctrl=look&Code=" + billcode + "&id=" + id);
            });
             $("#btn_pz").click(function() {

                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行！");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                var id = checkrow.find("td")[9].innerHTML;
                openDetail("../../SaleBill/BorrowMoney/FundHKDetail.aspx?Ctrl=pz&Code=" + billcode + "&id=" + id);
            });
          })
           


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
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 27px; ">
                            <asp:Button ID="Button6" runat="server" CssClass="baseButton" OnClick="Button6_Click1"
                                Text="刷 新" />
                            <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                            <input type="button" value="凭 证" id="btn_pz" runat="server" class="baseButton" />
                            <input type="button" value="详细信息" id="btn_look" class="baseButton" />
                            <asp:Button ID="Button2" runat="server" CssClass="baseButton" Text="导出Excel" OnClick="Button2_Click" />
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
                                        <asp:TextBox ID="txtcode" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td>
                                        借款人：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbilluser" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>                                 
                                    <td colspan="8" style="text-align: left">
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
                        Width="1000" OnItemDataBound="myGrid_ItemDataBound">
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
                            <asp:BoundColumn DataField="loancode" HeaderText="借款单号">
                                <HeaderStyle Font-Italic="False" Width="120" Font-Overline="False" Font-Strikeout="False"
                                    HorizontalAlign="Left" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Width="120" Font-Overline="False" Font-Strikeout="False"
                                    Wrap="False" CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billUser" HeaderText="还款人">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Left"
                                    Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="true"
                                    CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptName" HeaderText="所在部门">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Left"
                                    Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="true"
                                    CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" HeaderText="还款日期">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Left"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                                    CssClass="myGridItem" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="审批状态" DataField="stepID">
                                <HeaderStyle Font-Italic="False" Width="100" Font-Overline="False" Font-Strikeout="False"
                                    HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Width="100" Font-Overline="False" Font-Strikeout="False"
                                    Wrap="true" CssClass="myGridItemCenter" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billJe" HeaderText="还款金额" DataFormatString="{0:N2}">
                                <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Right"
                                    Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="False"
                                    CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ycjmoney" HeaderText="已冲减金额">
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
                            <asp:BoundColumn DataField="listid" HeaderText="序号" HeaderStyle-CssClass="hiddenbill"
                                ItemStyle-CssClass="hiddenbill"></asp:BoundColumn>
                                
                           <asp:BoundColumn DataField="note3" HeaderText="凭证" >
                                   <HeaderStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" HorizontalAlign="Left"
                                    Wrap="true" CssClass="myGridHeader"  Width="110"/>
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Wrap="true"
                                    CssClass="myGridItem" HorizontalAlign="Left" Width="110" />
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
                <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
                </pager:UcfarPager>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
                        </td>
                        <td id="wf">
                        </td>
                    </tr>
                </table>
            </td>
            <asp:HiddenField ID="hd_billCode" runat="server" />
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <input type="hidden" runat="server" id="hdwindowheight" />
        </tr>
    </table>
    </form>
</body>
</html>
