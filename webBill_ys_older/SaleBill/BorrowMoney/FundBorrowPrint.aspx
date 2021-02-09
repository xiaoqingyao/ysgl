<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FundBorrowPrint.aspx.cs"
    Inherits="SaleBill_BorrowMoney_FundBorrowPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>借款单</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <style type="text/css">
        .title
        {
            font-size: 18px;
            font-family: 微软雅黑;
            font-weight: 500;
            text-align: center;
            border: none;
        }
    </style>

    <script type="text/javascript">
        function Print() {
            window.print();
        }

        $(function() {
            $(".baseTable td").addClass("tableBg2");
//            $(".baseTable tr").find("td:even").css({ "text-align": "right","width":"150" });
//            $(".baseTable tr").find("td:odd").css({ "text-align": "left" ,"min-width":"150"});

            $(".baseTable tr").find("td:even").css({ "text-align": "right" });
            $(".baseTable tr").find("td:odd").css({ "text-align": "left" });
            var billCode = $("#lbjkcode").html();
            $.post("../../webBill/MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                if (status == "success") {
                    $("#hfwf").html(data);
                }
            });

            //显示采购审批单的审批过程
            billCode = $("#lblspdh").html();
            $.post("../../webBill/MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                if (status == "success") {
                    $("#cgspwf").html(data);
                }
            });
        });
        

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div runat="server" id="divprint" style="float: right; width: auto;">
            <a href="#" onclick="Print();">[打印]</a></div>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td class="title">
                    <asp:Label ID="lbcompany" runat="server"></asp:Label>借款单
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" width="100%" class="baseTable">
            <tr>
                <td style=" text-align:left;">
                借款人签字：
                </td>
                <td>
                </td>
                <td>
                    单号：
                </td>
                <td>
                   <asp:Label ID="lbjkcode" Font-Size="14px" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    借款日期：
                </td>
                <td>
                    <asp:Label ID="lbjksj" runat="server"></asp:Label>
                </td>
                <td>
                    填报时间：
                </td>
                <td>
                    <asp:Label ID="lbaddtime" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    借款人：
                </td>
                <td>
                    <asp:Label ID="lbloanName" runat="server"></asp:Label>
                </td>
                <td>
                    所在部门：
                </td>
                <td>
                    <asp:Label ID="lbdeptname" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    借款类别：
                </td>
                <td>
                    <asp:Label ID="lbjklb" runat="server"></asp:Label>
                </td>
                <td>
                    借款天数：
                </td>
                <td>
                    <asp:Label ID="lbjkts" runat="server"></asp:Label>(天)
                </td>
            </tr>
            <tr>
                <td>
                    借款金额：
                </td>
                <td>
                    <asp:Label ID="lbmoney" runat="server"></asp:Label>
                </td>
                <td>
                    核定金额：
                </td>
                <td>
                    <asp:Label ID="lbhdje" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="tr1" runat="server">
                <td>
                    借款事由：
                </td>
                <td colspan="3">
                    <asp:Label ID="lbjksy" runat="server"></asp:Label>
                </td>
            </tr>
             <tr id="tr2" runat="server">
                <td>
                    借款备注：
                </td>
                <td colspan="3">
                    <asp:Label ID="lbBz" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    部门经理签字：
                </td>
                <td colspan="3">
                    <asp:Label ID="hfwf" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    分管领导签字：
                </td>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td>
                    财务总监签字：
                </td>
                <td colspan="3">
                </td>
            </tr> 
            
            <tr style="display: none">
                <td colspan="4">
                    <table width="100%">
                        <thead>
                            <tr>
                                <th>
                                    单据编号
                                </th>
                                <th>
                                    单位
                                </th>
                                <th>
                                    人员
                                </th>
                                <th>
                                    日期
                                </th>
                                <th>
                                    类别
                                </th>
                                <th>
                                    金额
                                </th>
                                <th>
                                    说明
                                </th>
                            </tr>
                        </thead>
                        <tbody id="tbody" runat="server">
                        </tbody>
                    </table>
                </td>
            </tr>
            <%--如果有附加的采购审批单 则打印相关信息--%>
            <tr runat="server" id="trCgsp" visible="false">
                <td colspan="4">
                    <table width="100%">
                        <caption>
                            采购审批单</caption>
                        <tr>
                            <td>
                                审批单号：
                            </td>
                            <td>
                                <label id="lblspdh" runat="server">
                                </label>
                            </td>
                            <td>
                                申请部门：
                            </td>
                            <td>
                                <label id="lbldept" runat="server">
                                </label>
                            </td>
                            <td>
                                承办人：
                            </td>
                            <td>
                                <label id="lblcbr" runat="server">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                供应商：
                            </td>
                            <td colspan="5">
                                <label id="lblgys" runat="server">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                开户行：
                            </td>
                            <td colspan="2">
                                <label id="lblkhh" runat="server">
                                </label>
                            </td>
                            <td>
                                银行账号：
                            </td>
                            <td colspan="2">
                                <label id="lblzh" runat="server">
                                </label>
                            </td>
            </tr>
            <tr>
                <td>
                    采购明细：
                </td>
                <td colspan="5">
                    <table width="100%">
                        <thead>
                            <tr>
                                <th>
                                    名称
                                </th>
                                <th>
                                    规格型号
                                </th>
                                <th>
                                    数量
                                </th>
                                <th>
                                    单价
                                </th>
                                <th>
                                    总价
                                </th>
                                <th>
                                    备注
                                </th>
                            </tr>
                        </thead>
                        <tbody runat="server" id="tbCgmx">
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    审批过程：
                </td>
                <td colspan="5" id="cgspwf">
                </td>
            </tr>
        </table>
        </td> </tr> </table>
    </div>
    </form>
</body>
</html>
