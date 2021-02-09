<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gxxythxxprint_dz.aspx.cs" Inherits="webBill_fysq_gxxythxxprint_dz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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
                        <td colspan="3"><span style="font-size: x-large; font-weight: 600">
                            <asp:Label ID="lbl_tilte" runat="server" Text="关系学员特惠信息表"></asp:Label></span></td>
                    </tr>
                    <tr>
                        <td style="text-align: center">分校
                        </td>
                        <td style="text-align: center">学员姓名
                        </td>
                        <td style="text-align: center">年级
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_fx" Width="100"></asp:Label>
                        </td>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_xyxm" Width="100"></asp:Label>

                        </td>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_nj" Width="100"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">报名课时/小时
                        </td>
                        <td style="text-align: center">应收费
                        </td>
                        <td style="text-align: center">现行优惠
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_bmkc" Width="100"></asp:Label>
                        </td>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_ysf" Width="100"></asp:Label>

                        </td>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_xxyh" Width="100"></asp:Label>

                        </td>

                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: left;">在现行优惠的基础上另申请：优惠 
                        <asp:Label ID="txt_youhui" runat="server"></asp:Label>元；
                        赠送
                        <asp:Label ID="txt_zengsong1" runat="server"></asp:Label>

                            小时/科
                        <br />
                            或  
                        <asp:Label ID="txt_zengsong2" runat="server"></asp:Label>
                            优惠
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">备注：
                        </td>
                        <td colspan="2" style="text-align: left;">
                            <asp:Label ID="txt_beizhu" runat="server" Width="90%"></asp:Label>
                        </td>
                    </tr>
                    <asp:Literal ID="lalSpl" runat="server"></asp:Literal>
                </table>
            </div>
        </center>
    </form>
</body>
</html>
