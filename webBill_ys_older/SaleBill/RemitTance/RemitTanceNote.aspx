<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RemitTanceNote.aspx.cs" Inherits="SaleBill_RemitTance_RemitTanceNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>回款记录</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>


    <script src="../../webBill/Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js"
        type="text/javascript"></script>
 

    <script type="text/javascript">
        $(function() {
            $("#txtOperateDateFrm").datepicker();
        $("#txtOperateDateTo").datepicker();

        });
        //查询
        function openselect() {
            document.getElementById("trSelect").style.display = document.getElementById("trSelect").style.display == "none" ? "" : "none";
        }
        //隐藏查询框
        function cancle() {
            document.getElementById("trSelect").style.display = "none";
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                <div class="baseDiv" style="margin-bottom: 3px; margin-top: 3px">
                    <input type="button" id="btrefresh" value="刷 新" class="baseButton" />
                    <input id="btn_cancle" type="button" value="查 询" class="baseButton" onclick="openselect();" />
                </div>
            </td>
        </tr>
        <tr id="trSelect" style="display: none">
            <td>
                <table class="baseTable">
                    <tr>
                        <td style="text-align:right">
                            销售分公司：
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyName" runat="server" Width="120px" ></asp:TextBox>
                        </td>
                        <td style="text-align:right">
                            经销商：
                        </td>
                        <td>
                            <asp:TextBox ID="txtJXSName" runat="server" Width="120px" ></asp:TextBox>
                        </td>
                        <td style="text-align:right">
                            操作人：
                        </td>
                        <td>
                            <asp:TextBox ID="txtOperater" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td style="text-align:right">
                            经办人：
                        </td>
                        <td>
                            <asp:TextBox ID="txtJingBanRen" runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right">
                            操作日期起：
                        </td>
                        <td>
                            <asp:TextBox ID="txtOperateDateFrm" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td style="text-align:right">
                            操作日期止：
                        </td>
                        <td>
                            <asp:TextBox ID="txtOperateDateTo" runat="server" Width="120px" ></asp:TextBox>
                        </td>
                        <td style="text-align:right">
                            回款金额起：
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmoumtFrm" runat="server" Width="120px"  Text="0"></asp:TextBox>
                        </td>
                        <td style="text-align:right">
                            回款金额止：
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmountTo" runat="server" Width="120px" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right">
                            经办日期起：
                        </td>
                        <td>
                            <asp:TextBox ID="txtJingBanRiQiQ" runat="server" Width="120px" onfocus="setday(this);"></asp:TextBox>
                        </td>
                        <td style="text-align:right">
                            经办日期止：
                        </td>
                        <td>
                            <asp:TextBox ID="txtJingBanRiQiZhi" runat="server" Width="120px" onfocus="setday(this);"></asp:TextBox>
                        </td>
                        <td style="text-align:right">
                            回款形式：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlHuiKuanLeiBie" runat="server" Width="120px" >
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" style="text-align: center">
                            <asp:Button runat="server" Text="确 定" ID="btn_cx" CssClass="baseButton" OnClick="btn_cx_Click" />
                            <input id="Button1" type="button" value="取 消" class="baseButton" onclick="cancle();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div class="baseDiv">
                    <asp:GridView ID="GridView1" runat="server" CssClass="myGrid" AutoGenerateColumns="false">
                        <HeaderStyle CssClass="myGridHeader" />
                        <RowStyle CssClass="myGridItem" />
                        <Columns>
                            <asp:BoundField DataField="ssfgs" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill" />
                            <asp:BoundField DataField="ssfgsMc" HeaderText="所属分公司" />
                            <asp:BoundField DataField="dwdm" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill" />
                            <asp:BoundField DataField="dwmc" HeaderText="经销商" />
                            <asp:BoundField DataField="hklbmc" HeaderText="回款形式" />
                            <asp:BoundField DataField="hkje" HeaderText="回款金额" />
                            <asp:BoundField DataField="oper" HeaderText="操作人" />
                            <asp:BoundField DataField="operdate" HeaderText="操作日期" />
                            <asp:BoundField DataField="jbr" HeaderText="经办人" />
                            <asp:BoundField DataField="jbrq" HeaderText="经办日期" />
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
