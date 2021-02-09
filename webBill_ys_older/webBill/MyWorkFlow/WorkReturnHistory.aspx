<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkReturnHistory.aspx.cs" Inherits="webBill_MyWorkFlow_WorkReturnHistory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>
    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <title>驳回记录</title>
    <script type="text/javascript">
        $(function () {
            $("#begtime").datepicker();
            $("#endtime").datepicker();
        });
        function openDetail(url) {
            window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:470px;dialogWidth:1070px;status:no;scroll:yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="baseDiv">
            <table>
                <tr>
                    <td>驳回人：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlShenPiRen" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShenPiRen_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>单据类型：
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShenPiRen_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                     <td>单据编号：
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_billname"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        驳回开始时间：
                    </td>
                    <td>
                       <asp:TextBox runat="server" ID="begtime"></asp:TextBox>
                    </td>
                    <td>驳回截止时间：</td>
                    <td>
                        <asp:TextBox runat="server" ID="endtime"></asp:TextBox>
                    </td>
                   <td colspan="2">
                        <asp:Button ID="btn_cx" runat="server" CssClass="baseButton" OnClick="btn_cx_Click" Text="查询" />
                    </td>
                </tr>
             
            </table>



        </div>
        <div class="baseDiv">

            <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                CssClass="myGrid" Width="1000" OnItemDataBound="myGrid_ItemDataBound">
                <Columns>

                    <asp:BoundColumn DataField="mainbillcode" HeaderText="单据编号(单击详细信息)" ItemStyle-Width="100" HeaderStyle-Width="100">
                        <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItemRight hiddenbill" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="billname" HeaderText="单据编号(单击详细信息)" ItemStyle-Width="100" HeaderStyle-Width="100">
                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="djlx" HeaderText="单据类型" ItemStyle-Width="100" HeaderStyle-Width="100">
                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="usercode" HeaderText="驳回人" ItemStyle-Width="100" HeaderStyle-Width="100">
                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="dt" HeaderText="驳回时间" ItemStyle-Width="100" HeaderStyle-Width="100">
                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="mind" HeaderText="驳回意见" ItemStyle-Width="100" HeaderStyle-Width="100">
                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="flowid" HeaderText="flowid" ItemStyle-Width="100" HeaderStyle-Width="100">
                        <HeaderStyle CssClass="myGridHeader  hiddenbill" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItemRight hiddenbill" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="billDept" HeaderText="billDept" ItemStyle-Width="100" HeaderStyle-Width="100">
                        <HeaderStyle CssClass="myGridHeader  hiddenbill" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle CssClass="myGridItemRight hiddenbill" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    </asp:BoundColumn>


                </Columns>
                <PagerStyle Visible="False" />
            </asp:DataGrid>

        </div>
    </form>
</body>
</html>
