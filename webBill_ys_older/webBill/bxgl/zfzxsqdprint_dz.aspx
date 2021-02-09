<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zfzxsqdprint_dz.aspx.cs" Inherits="webBill_bxgl_zfzxsqdprint_dz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>大智教育转费转校申请单</title>
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

            var billcode = '<%=Request["billcode"]%>';
            //  je = parseFloat(je);

            $("#txt_syjedx").text(cmycurd($("#txt_syje").text()));
            $("#txt_xbjjedx").text(cmycurd($("#txt_xbjje").text()));

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
                        <td colspan="8"><span style="font-size: x-large; font-weight: 600">
                            <asp:Label ID="lbl_tilte" runat="server" Text="大智教育跨区域转费转校申请单"></asp:Label></span></td>
                    </tr>
                    <tr style="height: 40px">
                        <td style='text-align: right;' colspan="7">申请时间：</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txtdate" runat="server" /></td>
                    </tr>
                    <tr>
                        <td style='text-align: right;'><asp:Label ID="lbl_zcfx" runat="server" Text="转出区域分校"></asp:Label>
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="txt_zcqyfx" runat="server"></asp:Label>
                        </td>
                        <td style='text-align: right;'><asp:Label ID="lbl_zrfx" runat="server" Text="转入区域分校"></asp:Label>
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="txt_zrfx" runat="server"></asp:Label>
                        </td>
                        <td style='text-align: right;'>学员姓名
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="txt_xyxm" runat="server" Text=""></asp:Label>
                        </td>
                        <td style='text-align: right;'>年级
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="txt_nianji" runat="server" Text=""></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <td style="text-align: right">原教育教学服务合同费用：</td>
                        <td style='text-align: left;'>
                            <asp:Label runat="server" ID="txt_yxyfdfy"></asp:Label>
                        </td>
                        <td style="text-align: right">原报名课程：</td>
                        <td style='text-align: left;'>
                            <asp:Label runat="server" ID="txt_ybmkc"></asp:Label>
                        </td>
                        <td style="text-align: right">原课程享受优惠：</td>
                        <td colspan="3" style='text-align: left;'>
                            <asp:Label runat="server" ID="txt_ykcxsyh"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: right">已消费课时/次
                        </td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txt_yxfks" runat="server" Width="97%"></asp:Label>
                        </td>
                        <td style="text-align: right">对应课时单价：</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txt_dyksdj" runat="server" Width="95%"></asp:Label>
                        </td>
                        <td style="text-align: right">已消费费用：</td>

                        <td colspan="3" style='text-align: left;'>
                            <asp:Label ID="txt_yxffy" runat="server" Width="95%"></asp:Label>
                        </td>
                    </tr>
                    <tr>

                        <td style="text-align: right">应扣其他费用：</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txt_ykqtfy" runat="server" Width="97%"></asp:Label>
                        </td>
                        <td style="text-align: right">剩余金额：</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txt_syje" runat="server" Width="95%"></asp:Label>
                        </td>
                        <td style="text-align: right">大写：</td>
                        <td colspan="3" style='text-align: left;'>
                            <asp:Label ID="txt_syjedx" Style="text-align: left" runat="server" Width="95%"></asp:Label>
                        </td>


                    </tr>
                    <tr>
                        <td style="text-align: right">转费原因：</td>
                        <td colspan="7" style='text-align: left;'>
                            <asp:Label ID="txtSm" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: right">新报小时/课程：</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txt_xbxs" runat="server" Width="97%"></asp:Label>
                        </td>
                        <td style="text-align: right">须补交金额：</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txt_xbjje" runat="server" Width="95%"></asp:Label>

                        </td>
                        <td style="text-align: right">大写：</td>
                        <td colspan="3" style='text-align: left;'>
                            <asp:Label ID="txt_xbjjedx" runat="server" Width="95%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">家长签字确认：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    </tr>
                    <asp:Literal ID="lalSpl" runat="server"></asp:Literal>


                </table>
            </div>

        </center>
        <div style="margin-left: 290px;">
            <div>备注：</div>
            <div>1.需上传教育教学服务合同、小票/收据/发票及学生卡；</div>
            <div>2.转班费用不退，过期作废；</div>
            <div>3.持此凭证到接收区域/分校办理报名手续，丢失不补办。</div>
        </div>
    </form>
</body>
</html>
