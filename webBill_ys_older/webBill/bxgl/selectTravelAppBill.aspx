<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectTravelAppBill.aspx.cs"
    Inherits="selectTravelAppBill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>选择出差申请单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                var billCode = $(this).find("td")[0].innerHTML;
                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            //双击选中   
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").dblclick(function() {
                check();
            });
            //申请人自动加载
            $("#txtAppPersion").autocomplete({
                source: userAll
            });
            //申请单位自动加载
            $("#txtAppDept").autocomplete({
                source: deptAll
            });
            $("#btn_cx").click(function() {
                var status = document.getElementById("trSelect").style.display;
                document.getElementById("trSelect").style.display = status == "none" ? "" : "none";
            });
            $("#btn_qx").click(function() {
                document.getElementById("trSelect").style.display = "none";
            });
        });
        function check() {
            var checkRow = $(".highlight");
            if (checkRow == null) {
                alert("请先选中行！");
                return;
            }
            var billCode = checkRow.find('td')[0].innerHTML;
            var billDept = checkRow.find('td')[1].innerHTML;
            var billUser = checkRow.find('td')[2].innerHTML;
            var billDate = checkRow.find('td')[3].innerHTML;
            var travelType = checkRow.find('td')[4].innerHTML;
            var needAmount = checkRow.find('td')[5].innerHTML;
            var reasion = checkRow.find('td')[6].innerHTML;
            var stepID = checkRow.find('td')[7].innerHTML;
            var strMain = "";
            strMain = "<tr><td>" + billCode + "</td>" + "<td>" + billDept + "</td>" +
             "<td>" + billUser + "</td>" + "<td>" + billDate + "</td>" + "<td>" + travelType + "</td>" +
             "<td>" + needAmount + "</td>" + "<td>" + reasion + "</td>" + "<td>" + stepID + "</td></tr>";
            window.returnValue = strMain;
            window.close();
        }
        function cancle() {
            window.returnValue = "";
            window.close();
        }
        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:700px;dialogWidth:900px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("btn_Sure").click();
            }
        }
        function view() {
            var checkrow = $(".highlight");
            if (checkrow.val() == undefined) {
                alert("请先选择行");
                return;
            }
            var billcode = checkrow.find("td")[0].innerHTML;
            openDetail("../fysq/travelApplicationPrint2.aspx?Ctrl=View&Code=" + billcode);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td>
                <input id="btn_edit" type="button" value="选 择" class="baseButton" onclick="check();" />
                <input id="btn_cancle" type="button" value="取 消" class="baseButton" onclick="cancle();" />
                <input id="btn_view" type="button" value="详细信息" class="baseButton" onclick="view();" />
                <input id="btn_cx" type="button" class="baseButton" value="查询" />
            </td>
        </tr>
        <tr id="trSelect"  style="display: none;">
            <td>
                <table>
                    <tr>
                        <td>单据编号：</td>
                        <td><asp:TextBox ID="txtBillCode" runat="server" Width="120px"></asp:TextBox></td>
                        <td>申请日期从：</td>
                        <td><asp:TextBox ID="txb_sqrqbegin" runat="server" Width="120px"></asp:TextBox></td>
                        <td>至：</td>
                        <td><asp:TextBox ID="txb_sqrqend" runat="server" Width="120px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>人员：</td>
                        <td><asp:TextBox ID="txtAppPersion" runat="server" Width="120px"></asp:TextBox></td>
                        <td> 部门：</td>
                        <td><asp:TextBox ID="txtAppDept" runat="server" Width="120px"></asp:TextBox></td>
                        <td></td><td><asp:Button ID="btn_Sure" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_cx_Click" />
                        <input id="btn_qx" value="取 消" class="baseButton" type="button" /></td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="myGrid" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid"
                    ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"  OnItemDataBound="myGrid_ItemDataBound"
                    AllowPaging="True" PageSize="17" Width="842px">
                    <ItemStyle HorizontalAlign="Center" />
                    <Columns>
                        <%-- <asp:TemplateColumn HeaderText="选择">
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" Width="38px" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:TemplateColumn>--%>
                        <asp:BoundColumn DataField="billCode" HeaderText="单据编号">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="billDept" HeaderText="申请单位">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="billUser" HeaderText="申请人">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="billDate" HeaderText="申请日期" DataFormatString="{0:D}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="travelType" HeaderText="出差类别">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="needAmount" HeaderText="预计总额" DataFormatString="{0:F2}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="reasion" HeaderText="出差事由">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="stepID" HeaderText="单据状态">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle Visible="False" Position="Top" Mode="NumericPages" BorderColor="Black"
                        BorderStyle="Solid" BorderWidth="1px"></PagerStyle>
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
            <td style="height: 30px">
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
