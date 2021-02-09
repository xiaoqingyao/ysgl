<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bxdprint_dz.aspx.cs" Inherits="webBill_bxgl_bxdprint_dz" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用款申请单打印</title>
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
            var je = '<%= je%>';
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
            padding:5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <center>
            <div>
                <div style="height: 10px;"></div>
                <table class="baseTableInput" style="text-align: center; border: 1px; padding: 3px;" width="70%">
                    <tr>
                        <td colspan="8" style="font-size: x-large; font-weight: 600">借 款 申 请 单</td>
                    </tr>
                    <tr>
                        <td colspan="3"></td>
                        <td colspan="0" style="text-align: right; border-right: 0;">
                            申请单号
                         </td>
                        <td style="text-align:left;" colspan="2"><%=Request["billCode"] %></td>
                        <td  style="text-align: right">申请时间</td>
                        <td style="text-align:left; "> <asp:Label ID="lblsqrq" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td style="text-align: right">借款部门</td>
                        <td colspan="2" style="text-align: left">
                            <asp:Label ID="lblDept" runat="server" Text="" /></td>
                        <td style="text-align: right">借款日期</td>
                        <td style="text-align: left" colspan="2">
                            <asp:Label ID="lblykrq" runat="server" Text="" /></td>
                        <td style="text-align: right">借款人</td>
                        <td style="text-align: left">
                            <asp:Label ID="lblsqr" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td style="text-align: right">发票项目</td>
                        <td colspan="2" style="text-align: left">
                            <asp:Label ID="lblkxyt" runat="server" Text="" /></td>
                        <td style="text-align: right">借款方式</td>
                        <td colspan="4" style="text-align: left">
                            <asp:Label ID="lblykfs" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: right">款项金额（大写）</td>
                        <td colspan="4" style="text-align: left">
                            <asp:Label ID="lbldaxie" runat="server" /></td>
                        <td style="text-align: right">款项金额（小写）</td>
                        <td style="text-align: left" colspan="2">
                            <asp:Label ID="lblxiaoxie" runat="server" /></td>
                    </tr>
                    <tr>
                        <td style='text-align: right;'>开户银行
                        </td>
                        <td style="text-align: left; border-left: 0">
                            <asp:Label ID="lbl_khyh" runat="server"></asp:Label>
                        </td>
                        <td style='text-align: right;'>银行账号
                        </td>
                        <td colspan="2" style="text-align: left; border-left: 0">
                            <asp:Label ID="lbl_yhzh" runat="server"></asp:Label>
                        </td>
                        <td style='text-align: right;'>收款单位
                        </td>
                        <td colspan="2" style="text-align: left; border-left: 0">
                            <asp:Label ID="lbl_skdw" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_kmmx" runat="server" style="display: none">
                        <td colspan="7" style="width: 80%">
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
                                    <tr>
                                        <td>报销金额合计
                                        </td>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="lblhjxx"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>报销金额(大写)
                                        </td>
                                        <td colspan="3">
                                            <asp:Label runat="server" ID="lblhjdx"></asp:Label>
                                        </td>
                                    </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    <asp:Literal ID="ltl_sqzt" runat="server"></asp:Literal>
                    <tr>
                        <td colspan="8" style="text-align: left">申请人现金签收确认：
                        </td>
                    </tr>
                    <%--id="wf" colspan="6"--%>
                </table>
            </div>
        </center>

    </form>
</body>
</html>
