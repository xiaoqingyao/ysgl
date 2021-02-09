<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ZiChan_JiluDetail.aspx.cs"
    Inherits="ZiChan_ZiChanGuanLi_ZiChan_JiluDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>资产卡片编辑</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script language="javascript" type="Text/javascript">

       $(function() {
           //部门
            $("#txtShiYongBuMenCode").autocomplete({
                source: deptAll
            });
            $("#txtCaiGouBuMenCode").autocomplete({
                source: deptAll
            });
            //资产类别
             $("#txtleibiecode").autocomplete({
                source: availableTagszclb
             });
            //增减方式
            $("#txtZengJianFangShiCode").autocomplete({
                source: availableTagszjfs
            });
            //使用状况
           $("#txtShiYongZhuangKuangCode").autocomplete({
                source: availableTagssyzk
            });
        });
        function check()
        {
               
            var reptname = $("#txtreportname").val();
            if (reptname.length < 1) {
                alert("资产名称不能为空！");
                $("#txtreportname").focus();
                return false;
            }
            var vartxtleibiecode = $("#txtleibiecode").val();
            if (vartxtleibiecode.length < 1) {
                alert("资产类别不能为空！");
                $("#txtleibiecode").focus();
                return false;
            }
             var vartxtZengJianFangShiCode = $("#txtZengJianFangShiCode").val();
            if (vartxtZengJianFangShiCode.length < 1) {
                alert("增减方式不能为空！");
                $("#txtZengJianFangShiCode").focus();
                return false;
            }
             var vartxtYuanZhi = $("#txtYuanZhi").val();
            if (vartxtYuanZhi.length < 1) {
                alert("原值不能为空！");
                $("#txtYuanZhi").focus();
                return false;
            }
            else
            {
                return true;
            
            }

        }
        //替换非数字
        function replaceNaNf(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }
        function replaceNaN(obj) {
            var objval = obj.value;
            if (objval.indexOf("-") == 0) {
                objval = objval.substr(1);
            }
            if (isNaN(objval)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            }
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <table class="baseTable" width="95%" style="background-color: #EBF2F5;">
        <tr>
            <td colspan="6" class="billtitle" style="text-align: center">
                资产卡片
            </td>
        </tr>
        <tr>
            <td style="text-align: right" class="InputLabel">
                资产编号：
            </td>
            <td>
                <asp:Label ID="lblcode" runat="server" Text=""></asp:Label>
            </td>
            <td style="text-align: right" class="InputLabel">
                资产名称：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtreportname" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: right" class="InputLabel">
                资产类别：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtleibiecode" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right" class="InputLabel">
                增减方式：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtZengJianFangShiCode" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: right" class="InputLabel">
                使用状况：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtShiYongZhuangKuangCode" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: right" class="InputLabel">
                规格型号：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtGuiGeXingHao" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right" class="InputLabel">
                使用期限(月)：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtShiYongQiXian" onkeyup="replaceNaN(this);" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: right" class="InputLabel">
                使用部门：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtShiYongBuMenCode" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: right" class="InputLabel">
                原值：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtYuanZhi" onkeyup="replaceNaN(this);" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right" class="InputLabel">
                开始使用期：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtQiYongDate" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: right" class="InputLabel">
                采购部门：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtCaiGouBuMenCode" runat="server"></asp:TextBox>
            </td>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td style="text-align: right" class="InputLabel">位置：</td>
            <td class="baseText" colspan="5">
                <asp:TextBox ID="txtWeiZhi" runat="server" TextMode="MultiLine" Height="50px" Width="98%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right" class="InputLabel">
                备注：
            </td>
            <td colspan="5" class="baseText">
                <asp:TextBox ID="txtreportexplain" runat="server" TextMode="MultiLine" Height="100px"
                    Width="98%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="6" style="text-align: center;background-color: #EBF2F5;">
                <asp:Button ID="btnsave" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="return check();"
                    OnClick="btnsave_Click" />
                <asp:Button ID="btnclose" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close();" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
