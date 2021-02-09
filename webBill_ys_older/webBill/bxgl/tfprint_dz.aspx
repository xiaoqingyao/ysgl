<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tfprint_dz.aspx.cs" Inherits="webBill_bxgl_tfprint_dz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>退费单打印</title>
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
                <table class="baseTableInput" style="text-align: center; border: 1px;" width="70%">
                    <tr>
                        <td colspan="6"><span style="font-size: x-large; font-weight: 600">大智学校学员退费申请及审批单</span></td>
                    </tr>
                    <tr style="height: 40px">
                        <td style='text-align: right;' colspan="5">受理时间：</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="lblykrq" runat="server" Text="" /></td>
                    </tr>
                    <tr>
                        <td style='text-align: center;'>所在分校
                        </td>
                        <td style='text-align: center;'>学员姓名
                        </td>
                        <td style='text-align: center;'>所在班级
                        </td>
                        <td style='text-align: center;'>协议编号
                        </td>
                        <td style='text-align: center;' colspan="2">签单时间
                        </td>

                    </tr>
                    <tr>
                        <td style='text-align: center;'>
                            <asp:Label runat="server" ID="txt_szfx"></asp:Label>
                        </td>
                        <td style='text-align: center;'>
                            <asp:Label runat="server" ID="txt_xyxm"></asp:Label>
                        </td>
                        <td style='text-align: center;'>
                            <asp:Label runat="server" ID="txt_sznj"></asp:Label>
                        </td>
                        <td style='text-align: center;'>
                            <asp:Label runat="server" ID="txt_xybh"></asp:Label>
                        </td>
                        <td style='text-align: center;' colspan="2">
                            <asp:Label runat="server" ID="txt_qdsj"></asp:Label>
                        </td>

                    </tr>

                    <tr>
                        <td style="text-align: right">协议中学员监<br />
                            护人银行账号
                        </td>

                        <td colspan="5">
                            <div style="text-align: left; border-bottom: 1px solid">
                                <asp:Label ID="lbl_khyh" runat="server"></asp:Label>
                            </div>

                            <div style="text-align: left">
                                <asp:Label ID="lbl_yhzh" runat="server"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>

                        <td style='text-align: center;'>协议辅导费用
                        </td>
                        <td style='text-align: center;'>已消费课时
                        </td>
                        <td style='text-align: center;'>对应课时单价
                        </td>
                        <td style='text-align: center;'>已消费费用
                        </td>
                        <td style='text-align: center;' colspan="2">应扣其他费用
                        </td>


                    </tr>
                    <tr>
                        <td style='text-align: center;'>
                            <asp:Label runat="server" ID="lbl_xyfdfy"></asp:Label>
                        </td>
                        <td style='text-align: center;'>
                            <asp:Label runat="server" ID="lbl_yxfks"></asp:Label>
                        </td>
                        <td style='text-align: center;'>
                            <asp:Label runat="server" ID="lbl_dyksdj"></asp:Label>
                        </td>
                        <td style='text-align: center;'>
                            <asp:Label runat="server" ID="lbl_yxffy"></asp:Label>
                        </td>
                        <td style='text-align: center;' colspan="2">
                            <asp:Label runat="server" ID="lbl_ykqtfy"></asp:Label>
                        </td>

                    </tr>
                    <tr style="height: 40px">
                        <td style="text-align: right">应退费用<br />
                            金额
                        </td>
                        <td colspan="5" style='text-align: left;'>
                            <div style="text-align: left; border-bottom: 1px solid">
                                小写:<asp:Label ID="lblxiaoxie" runat="server" />
                            </div>
                            <div>
                                大写:<asp:Label ID="lbldaxie" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr style="height: 40px;">
                        <td>费用项目</td>
                        <td>结算金额</td>
                        <td>费用项目</td>
                        <td>结算金额</td>
                        <td>费用项目</td>
                        <td>结算金额</td>
                    </tr>
                    <asp:Literal ID="lalFyxm" runat="server"></asp:Literal>
                    <asp:Literal ID="lalSpl" runat="server"></asp:Literal>
                    <tr>
                        <td style="text-align: right">备注</td>
                        <td colspan="5" style="text-align: left">
                            <asp:Label runat="server" ID="lblZy"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="6" align="right">报销人签字：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    </tr>
                </table>
            </div>
        </center>

    </form>
</body>
</html>
