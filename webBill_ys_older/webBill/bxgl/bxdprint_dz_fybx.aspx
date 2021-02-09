<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bxdprint_dz_fybx.aspx.cs" Inherits="webBill_bxgl_bxdprint_dz_fybx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>费用报销单打印</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            var je = '<%=je%>';
            var billcode = '<%=Request["billcode"]%>';
            je = parseFloat(je);
            $("#lblxiaoxie").text("￥" + je + "元");

            $("#lbldaxie").text(cmycurd(je));
            $("#lblhjxx").text("￥" + je + "元");
            $("#lblhjdx").text(cmycurd($("#lblxiaoxie").text()));

            $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billcode }, function (data, status) {
                if (status == "success") {
                    $("#wf").html(data);
                }
            });
        });
    </script>
    <style type="text/css">
        .baseTableInput td {
            padding: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <center>
            <div>
                <div style="height: 10px;"></div>
                <table class="baseTableInput" style="text-align: center; border: 1px;" width="80%">
                    <tr>
                        <td colspan="6" ><span style="font-size: x-large; font-weight: 600">费 用 报 销 单</span></td>
                    </tr>
                    <tr style="height: 40px">
                        <td style='text-align: right;'>报销日期</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="lblykrq" runat="server" Text="" /></td>
                        <td style='text-align: right;'>报销单类型</td>
                        <td style='text-align: left;'>
                            <asp:Label runat="server" ID="lblbxdlx"></asp:Label>
                        </td>
                        <td style='text-align: right;'>附件</td>
                        <td style="text-align: left; border-left: 0">
                            <asp:Label ID="lbl_fjs" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style='text-align: right;'>开户银行
                        </td>
                        <td style="text-align: left; border-left: 0">
                            <asp:Label ID="lbl_khyh" runat="server"></asp:Label>
                        </td>
                        <td style='text-align: right;'>银行账号
                        </td>
                        <td style="text-align: left; border-left: 0">
                            <asp:Label ID="lbl_yhzh" runat="server"></asp:Label>
                        </td>
                        <td style='text-align: right;'>收款单位
                        </td>
                        <td style="text-align: left; border-left: 0">
                            <asp:Label ID="lbl_skdw" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 40px">
                        <td style="text-align: right">发票项目</td>
                        <td colspan="5" style="text-align: left">
                            <asp:Label runat="server" ID="lblZy"></asp:Label></td>
                    </tr>
                   <%--2016-06-29 辛青要求将费用项目明细隐藏掉--%>
                    <tr style="height: 40px; display:none">
                        <td>费用项目</td>
                        <td>结算金额</td>
                        <td>费用项目</td>
                        <td>结算金额</td>
                        <td>费用项目</td>
                        <td>结算金额</td>
                    </tr>
                    <asp:Literal ID="lalFyxm" runat="server"  Visible="false"></asp:Literal>
                    <tr style="height: 40px">
                        <td style="text-align: right">报销金额（大写）
                        </td>
                        <td colspan="2" style='text-align: left;'>
                            <asp:Label ID="lbldaxie" runat="server" />
                        </td>
                        <td style="text-align: right">报销金额合计（小写）  
                        </td>
                        <td colspan="2" style='text-align: left;'>
                            <asp:Label ID="lblxiaoxie" runat="server" />
                        </td>

                    </tr>
                    <asp:Literal ID="lalSpl" runat="server"></asp:Literal>
                    <tr style="height: 40px">
                        <td colspan="6" align="right">报销人签字：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    </tr>
                    <%--<tr id="tr_kmmx" runat="server">
                        <td colspan="7" style="width: 100%">

                            <table style="width: 100%; border: none">
                                <tr>
                                    <td colspan="2">
                                        <asp:Repeater ID="Repeater1" runat="server">
                                            <HeaderTemplate>
                                                <table style="width: 100%; border: 0">
                                                    <tr>
                                                        <td>费用项目</td>
                                                        <td>结算单位</td>
                                                        <td>结算日期</td>
                                                        <td>结算金额</td>
                                                    </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%#Eval("km") %>
                                                    </td>
                                                    <td>
                                                        <%#Eval("deptname") %>
                                                    </td>
                                                    <td>
                                                        <%#Eval("billDate") %>
                                                    </td>
                                                    <td>
                                                        <%#Eval("je") %>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </td>
                                    <td rowspan="3" style="border-left: 0; border-right: 0; border-bottom: 0; border-top: 0">
                                        <asp:Literal ID="ltl_sqzt" runat="server"></asp:Literal>
                                    </td>
                                </tr>

                                <tr>

                                    <td style="text-align: left">报销金额合计（小写）  
                                    </td>
                                    <td>
                                        <asp:Label ID="lblxiaoxie" runat="server" />
                                    </td>
                                </tr>
                                <tr>

                                    <td style="text-align: left">报销金额（大写）
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldaxie" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>--%>
                </table>
            </div>
        </center>

    </form>
</body>
</html>
