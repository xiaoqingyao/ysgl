<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TsflDetail.aspx.cs" Inherits="SaleBill_TSFLSZ_TsflDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>特殊返利标准制定</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        //替换非数字
        function replaceNaN(obj) {
            var objval = obj.value;
            if (objval.indexOf("-") == 0) {
                objval = objval.substr(1);
            }
            if (isNaN(objval)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            }
        }
        function openBillDetails() {
            var strBillCode = $("#lbdjmc").text();
            if (strBillCode == "" || strBillCode == undefined) {
                alert("请先选中行！");
                return;
            }
            openDetail("../Salepreass/SpecialRebatesAppDetails.aspx?Ctrl=look&Code=" + strBillCode);
        }
        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:550px;dialogWidth:900px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("Button6").click();
            }
        } 
    </script>

</head>
<body style="background-color: #EDEDED; width: 100%;">
    <form id="form1" runat="server">
    <div style="width: 100%; text-align: center">
        <table cellpadding="0" id="taball" cellspacing="0" width="100%" style="text-align: center">
            <tr>
                <td style="text-align: center; height: 36px; font-size:larger">
                    <strong>特殊返利标准制定 </strong>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="100%">
                          <tr>
                            <td style="text-align: right">
                                申请单号：
                            </td>
                            <td style="text-align: left">
                                <a href="#" style=" color:Blue;" id="aAlert" onclick="openBillDetails();"><asp:Label ID="lbdjmc" runat="server" Text=""></asp:Label></a>
                            </td>
                             <td style="text-align: right">
                                部门：
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lblbm" runat="server" Text=""></asp:Label>
                            </td>
                          
                        </tr>
<%--                        <tr>
                         <td style="text-align: right">
                                车架号：
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lbcjh" runat="server" Text=""></asp:Label>
                            </td>
                             <td style="text-align: right">
                                车辆类型：
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lbmcar" runat="server" Text=""></asp:Label>
                            </td>
                           
                        </tr>--%>
     <%--                   <tr>
                            <td style="text-align: right">
                                正常返利标准：
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lbybfee" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="text-align: right">
                                超出正常标准点数：
                            </td>
                            <td style="text-align: left">
                                <asp:Label ID="lblvesp" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="4">
                                <div style="height: 500px; overflow: scroll">
                                    <asp:DataGrid ID="mygrid" runat="server" CssClass="myGrid" Width="100%" ItemStyle-HorizontalAlign="Center"
                                        AllowSorting="True" AutoGenerateColumns="False">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:BoundColumn DataField="deptName" HeaderText="部门" ItemStyle-HorizontalAlign="Center" >
                                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="truckCode" HeaderText="车架号" ItemStyle-HorizontalAlign="Center" >
                                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="truckTypeName" HeaderText="车辆类型" ItemStyle-HorizontalAlign="Center" >
                                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="yskmName" HeaderText="费用类别" ItemStyle-HorizontalAlign="Center" >
                                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="tName" HeaderText="配置项/销售过程">
                                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="StandardFee" HeaderText="标准金额">
                                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="批复金额">
                                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtpfje" runat="server" onkeyup="replaceNaN(this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 5px;">
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Button ID="btn_test" class="baseButton" runat="server" Text="确 定" OnClick="btn_test_Click" />
                    <asp:Button ID="Button1" class="baseButton" runat="server" Text="取 消" OnClick="btn_cancel_Click" />
                    <asp:HiddenField ID="hdDeptCode" runat="server" Value="" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
