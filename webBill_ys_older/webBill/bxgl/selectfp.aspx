<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectfp.aspx.cs" Inherits="webBill_bxgl_selectfp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
 <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script language="javascript" type="text/javascript">
        $(function () {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
            //双击选中
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").dblclick(function () {
                check();
            });
        });
        function cancle() {
            window.returnValue = "";
            window.close();
        }
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
            var aa = "";
            var bb = "";
            var cc = "";
           
            var strMain = "";
            strMain = "<tr><td>" + billCode + "</td>" + "<td>" + billDept + "</td>" +
             "<td>" + billUser + "</td>" + "<td>" + billDate + "</td>" + "<td>" + aa + "</td><td>" + travelType + "</td><td>"+bb+"</td><td>"+cc+"</td>" +
             "</tr>";

          
            window.returnValue = strMain;
            window.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td>
                <input id="btn_edit" type="button" value="选 择" class="baseButton" onclick="check();" />&nbsp;&nbsp;
                <input id="btn_cancle" type="button" value="取 消" class="baseButton" onclick="cancle();" />&nbsp;
                申请日期从：<asp:TextBox ID="txb_sqrqbegin" runat="server" Width="80px"></asp:TextBox>至<asp:TextBox
                    ID="txb_sqrqend" runat="server" Width="80px"></asp:TextBox>
                采购单号：<asp:TextBox ID="txtcode" runat="server" Width="80px"></asp:TextBox>
                <asp:Button runat="server" Text="查询" ID="btn_cx" CssClass="baseButton" OnClick="btn_cx_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="myGrid" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid"
                    ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                    AllowPaging="True" PageSize="17" Width="842px">
                    <ItemStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundColumn DataField="billCode" HeaderText="单号" HeaderStyle-CssClass="myGridHeader"
                            ItemStyle-CssClass="myGridItem"></asp:BoundColumn>
                        <asp:BoundColumn DataField="deptname" HeaderText="部门">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="fpusername" HeaderText="申请人">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="fprq" HeaderText="发票日期" DataFormatString="{0:D}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:BoundColumn>
                       
                        <asp:BoundColumn DataField="billJe" HeaderText="发票总额" DataFormatString="{0:F2}">
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                        </asp:BoundColumn>                       
                        <asp:BoundColumn DataField="bz" HeaderText="备注">
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
            <td style="height: 30px">
                审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
