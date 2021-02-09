<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleDeptFeelist.aspx.cs"
    Inherits="SaleBill_SaleFee_SaleDeptFeelist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>部门费用统计</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
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
                var billCode = $(this).find("td")[3].innerHTML;
                $("#hd_kmCode").val(billCode);
                $.post("../../webBill/MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });


            //取消
            $("#btn_cancle").click(function() {
                document.getElementById("trSelect").style.display = "none";
            });

        });


        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:350px;dialogWidth:900px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                location.replace(location.href);
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
        //显示返利明细
        function showRebateDetails() {
            var vDateFrm = $("#hdDateFrm").val();
            var vDateTo = $("#hdDateTo").val();
            var vDeptCode = $("#hdDeptCode").val();
            var vkmCode = $("#hd_kmCode").val();
            var url = "../rebate/RebateList.aspx?Ctrl=Select&DateFrm=" + vDateFrm + "&DateTo=" + vDateTo + "&DeptCode=[" + vDeptCode + "]&kmCode=" + vkmCode + ""
            openDetail(url);
        }
        //显示报销明细
        function showSpendDetails() {
            var vDateFrm = $("#hdDateFrm").val();
            var vDateTo = $("#hdDateTo").val();
            var vDeptCode = $("#hdDeptCode").val();
            var vkmCode = $("#hd_kmCode").val();
            var url = "../Flsz/SaleFeeSpendNoteQuery.aspx?Ctrl=Select&DateFrm=" + vDateFrm + "&DateTo=" + vDateTo + "&DeptCode=[" + vDeptCode + "]&kmCode=" + vkmCode + ""
            openDetail(url);
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                销售费用统计
                <asp:Label ID="lblfee" runat="server" ForeColor="Red"></asp:Label>
                <input type="button" value="返利明细" onclick="showRebateDetails();" class="baseButton" />
                <input type="button" value="报销明细" onclick="showSpendDetails();" class="baseButton" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Width="98%" CssClass="myGrid" AllowPaging="True" OnItemDataBound="myGridItemDataBound"
                    PageSize="20" ShowFooter="true">
                    <Columns>
                        <asp:BoundColumn DataField="deptCode" HeaderText="单位编号">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="hiddenbill" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="hiddenbill" />
                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="hiddenbill" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="deptcodename" HeaderText="单位名称" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="yskmCode" HeaderText="费用编号" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="hiddenbill " />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="hiddenbill" />
                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="hiddenbill" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="yskmName" HeaderText="费用名称" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="NianChuFenPeiAmount" HeaderText="年初分配总额" DataFormatString="{0:N2}"
                            ItemStyle-HorizontalAlign="Right">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="RebateFeeAmount" HeaderText="返利总额" DataFormatString="{0:N2}"
                            ItemStyle-HorizontalAlign="Right">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="SpendFeeAmount" HeaderText="报销总额" DataFormatString="{0:N2}"
                            ItemStyle-HorizontalAlign="Right">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="SurplusFeeAmount" HeaderText="可支配总额" DataFormatString="{0:N2}"
                            ItemStyle-HorizontalAlign="Right">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="isMJ" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill">
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle Visible="False" />
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="height: 30px">
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
            <asp:HiddenField ID="hd_kmCode" runat="server" />
            <asp:HiddenField ID="hdDateFrm" runat="server" />
            <asp:HiddenField ID="hdDateTo" runat="server" />
            <asp:HiddenField ID="hdDeptCode" runat="server" />
        </tr>
    </table>
    </form>
</body>
</html>
