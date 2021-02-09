<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ZijinShenqingprint_dz.aspx.cs" Inherits="webBill_bxgl_ZijinShenqingprint_dz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>区域经费用款申请单</title>
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

            $("#txtjedx").text(cmycurd($("#txtje").text()));

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
                        <td colspan="6"><span style="font-size: x-large; font-weight: 600">区域经费用款申请单</span></td>
                    </tr>
                    <tr style="height: 40px">
                        <td style='text-align: right;' colspan="5">申请时间</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txtdate" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style='text-align: right;'>用款部门
                        </td>
                        <td style="text-align: left">
                            <asp:Label ID="txtdept" runat="server" OnTextChanged="txt_yksj_TextChanged" AutoPostBack="true"></asp:Label>

                        </td>
                        <td style="text-align: right">用款日期
                        </td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txt_yksj" runat="server" OnTextChanged="txt_yksj_TextChanged" AutoPostBack="true"></asp:Label>
                        </td>
                        <td style="text-align: right">申请人</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txtsqr" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style='text-align: right;'>款项用途</td>
                        <td colspan="2" style='text-align: left;'>
                            <asp:Label ID="TextBox1" runat="server" Width="97%"></asp:Label>
                        </td>
                        <td style='text-align: right;'>用款方式</td>
                        <td colspan="2" style='text-align: left;'>
                            <asp:Label ID="txt_ykfs" runat="server" Width="90%">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style='text-align: right;'>款项说明</td>
                        <td colspan="5" style='text-align: left;'>
                            <asp:Label ID="txtSm" runat="server" Width="97%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style='text-align: right;'>款项金额（大写）</td>
                        <td colspan="3" style='text-align: left;'>
                            <asp:Label ID="txtjedx" runat="server" Width="97%"></asp:Label></td>
                        <td style="text-align: right">款项金额（小写）</td>
                        <td style='text-align: left;'>
                            <asp:Label ID="txtje" runat="server" Width="95%"></asp:Label></td>
                    </tr>

                    <asp:Literal ID="lalSpl" runat="server"></asp:Literal>
                    <tr>
                        <td colspan="6" align="right">申请人现金签收确认：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    </tr>
                </table>
            </div>

        </center>

    </form>
</body>
</html>
