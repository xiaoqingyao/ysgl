<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectBillToRebate.aspx.cs"
    Inherits="SaleBill_select_SelectBillToRebate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择销售过程单据</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script language="javascript" type="text/javascript" src="../../webBill/Resources/jScript/calender.js"></script>

    <script language="javascript" type="text/javascript" src="../../webBill/Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                var billCode = $(this).find("td")[1].innerHTML;
                $.post("../../webBill/MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            //刷新
            $("#btrefresh").click(function() {
                location.replace(location.href);
            });
            //申请单位自动加载
            $("#txtAppDept").autocomplete({
                source: deptAll
            });
        });
        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:700px;dialogWidth:900px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("btn_cx").click();
            }
        }
        function view() {

            var checkrow = $(".highlight");
            if (checkrow.val() == undefined) {
                alert("请先选择行！");
                return;
            }
            var billcode = checkrow.find("td")[1].innerHTML;

            if (billcode == "" || billcode == undefined) { alert("请先选择行！"); return }

            var type = billcode.substring(0, 4);

            if (type == "kpsq") {
                openDetail("../kpsq/KpsqDetails.aspx?type=look&bh=" + billcode);
            }
        }
        function confirmBilling() {
            var checkrow = $(".highlight");
            if (checkrow.val() == undefined) {
                alert("请先选择行！");
                return false;
            }
            var billcode = checkrow.find("td")[1].innerHTML;
            var billtime = checkrow.find("td")[7].innerHTML;
            if (billtime.length >= 10) {
                alert("该记录已开票,不能重复开票！");
                return false;
            } else {
                if (confirm("确认开票？")) {
                    document.getElementById("divOver").style.visibility = "visible";
                    return true;
                } else { return false; }
            }
        }
        function unconfirmBilling() {
            var checkrow = $(".highlight");
            if (checkrow.val() == undefined) {
                alert("请先选择行！");
                return false;
            }
            var billcode = checkrow.find("td")[1].innerHTML;
            var billtime = checkrow.find("td")[7].innerHTML;
            if (billtime.length < 10) {
                alert("该记录未开票,不能取消开票！");
                return false;
            } else {
                if (!confirm("将会禁用该车辆的所有返利明细并且该过程是不可逆的，确定继续？")) {
                    return false;
                } else {
                    return true;
                }
            }
        }
        function SelectAll(aControl) {
            var chk = document.getElementById("myGrid").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
            }
        }
        //查询
        function openselect() {
            document.getElementById("trSelect").style.display = document.getElementById("trSelect").style.display == "none" ? "" : "none";
        }
        //隐藏查询框
        function cancle() {
            document.getElementById("trSelect").style.display = "none";
        }
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                location.replace(location.href);

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
            document.getElementById("header").appendChild(t2);
            var mainwidth = document.getElementById("main").style.width;
            mainwidth = mainwidth.substring(0, mainwidth.length - 2);
            mainwidth = mainwidth - 16;
            document.getElementById("header").style.width = mainwidth;
        }

        function hiddendiv() {

        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td>
                <div class="baseDiv" style="margin-bottom: 3px; margin-top: 3px">
                    <input type="button" id="btrefresh" value="刷 新" class="baseButton" />
                    <input id="btn_cancle" type="button" value="查 询" class="baseButton" onclick="openselect();" />
                    <input id="btn_view" type="button" value="详细信息" class="baseButton" onclick="view();" />
                    <asp:Button runat="server" ID="btn_ConfirmBilling" Text="确认开票" CssClass="baseButton"
                        OnClientClick="return confirmBilling();" OnClick="Billing_click" />
                        <asp:Button runat="server" ID="btn_unBilling" Text="退 票" CssClass="hiddenbill"
                        OnClientClick="return unconfirmBilling();" OnClick="btn_unBilling_click" />
                    <%--                    <input id="btn_ConfirmBilling" type="button" value="确认开票" class="baseButton" onclick="confirmBilling();" />
--%>
                </div>
            </td>
        </tr>
        <tr id="trSelect" style="display: none">
            <td>
                <table class="baseTable">
                    <tr>
                        <td>
                            申请日期从：
                        </td>
                        <td>
                            <asp:TextBox ID="txb_sqrqbegin" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            至
                        </td>
                        <td>
                            <asp:TextBox ID="txb_sqrqend" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            部门：
                        </td>
                        <td>
                            <asp:TextBox ID="txtAppDept" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            确认状态：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem Value="">--全部--</asp:ListItem>
                                <asp:ListItem Value="0">--未确认--</asp:ListItem>
                                <asp:ListItem Value="1">--已确认--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            单号：
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillCode" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            车架号：
                        </td>
                        <td>
                            <asp:TextBox ID="txtCarCode" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td colspan="4" style="text-align: center">
                            <asp:Button runat="server" Text="确 定" ID="btn_cx" CssClass="baseButton" OnClick="btn_cx_Click" />
                            <input id="Button1" type="button" value="取 消" class="baseButton" onclick="cancle();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="header">
                </div>
                <div class="baseDiv" id="main" style="overflow-y: scroll; margin-top: -1px; width: 1100px;
                    height: 400px;">
                    <asp:DataGrid ID="myGrid" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid"
                        ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                        Style="table-layout: fixed" Width="100%" AllowPaging="True" PageSize="17">
                        <ItemStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateColumn ItemStyle-Width="60" HeaderStyle-Width="60">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                    Width="60px" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                        Text="全选" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="false" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="billCode" HeaderText="单据号" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TruckCode" HeaderText="车架号" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptName" HeaderText="申请单位" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" HeaderText="申请日期" DataFormatString="{0:D}"
                                ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="fph" HeaderText="发票号" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="fprq" HeaderText="开票日期" DataFormatString="{0:D}" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="BillingDate" HeaderText="确认开票日期" DataFormatString="{0:D}"
                                ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
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
            <td style="height: 10px">
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
        </tr>
    </table>
    <div id="divOver" runat="server" style="z-index: 1200; left: 30%; width: 160; cursor: wait;
        position: absolute; top: 25%; height: 100; visibility: hidden;">
        <table style="width: 17%; height: 10%;">
            <tr>
                <td>
                    <table style="width: 316px; height: 135px;">
                        <tr align="center" valign="middle">
                            <td>
                                <img src="../../webBill/Resources/Images/Loading/pressbar2.gif" alt="" /><br />
                                <b>正在处理中，请稍后....<br />
                                </b>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
