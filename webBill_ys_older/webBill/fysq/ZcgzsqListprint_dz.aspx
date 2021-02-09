<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ZcgzsqListprint_dz.aspx.cs" Inherits="webBill_fysq_ZcgzsqListprint_dz" %>

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
                        <td colspan="7"><span style="font-size: x-large; font-weight: 600">
                            <asp:Label ID="lbl_tilte" runat="server" Text="新建总部固定资产申购单"></asp:Label></span></td>
                    </tr>
                    <tr style="height: 40px">
                        <td style='text-align: right;' colspan="2">编号：</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txt_bh" runat="server" /></td>
                        <td style='text-align: right;' colspan="3">申请时间：</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txtdate" runat="server" /></td>
                    </tr>
                    <tr>
                        <td style="text-align: center">物品名称
                        </td>
                        <td style="text-align: center">规格数量
                        </td>
                        <td style="text-align: center">用途
                        </td>
                        <td style="text-align: center">使用部门
                        </td>
                        <td style="text-align: center">需用日期
                        </td>
                        <td style="text-align: center">估计价值
                        </td>
                        <td style="text-align: center">购置备注
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_wpmc" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:Label>

                        </td>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_ggsl" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:Label>

                        </td>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_yt" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:Label>

                        </td>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_sybm" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:Label>

                        </td>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_xyrq" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:Label>

                        </td>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_gjjz" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:Label>

                        </td>
                        <td style="text-align: center;">
                            <asp:Label runat="server" ID="txt_gzbz" Width="100" Height="50" BorderColor=" #FBF7CB"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="text-align: left;">共申请办公用品 
                                    <asp:Label ID="txt_sqjs" runat="server" BorderColor=" #FBF7CB"></asp:Label>
                            件&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        总金额
                                    <asp:Label ID="txt_zje" runat="server" BorderColor=" #FBF7CB"></asp:Label>
                            元
                        </td>
                    </tr>
                    <asp:Literal ID="lalSpl" runat="server"></asp:Literal>
                </table>
            </div>
        </center>
    </form>
</body>
</html>
