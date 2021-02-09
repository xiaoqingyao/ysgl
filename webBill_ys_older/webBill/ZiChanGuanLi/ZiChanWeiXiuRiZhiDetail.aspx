<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ZiChanWeiXiuRiZhiDetail.aspx.cs"
    Inherits="webBill_ZiChanGuanLi_ZiChanWeiXiuRiZhiDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>维护记录编辑页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
          
             //部门选择
              $("#txtwxdeptname").autocomplete({
                source: availableTags
            });
             //选择资产
              $("#txtzccode").autocomplete({
                source: zcTags
            });
//            //选择人员
//              $("#txtwxname").autocomplete({
//                source: userTags
//            });
            
              $("#txtwxname").autocomplete({
                source: userTags,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function(data, status) {
                        if (status == "success") {
                            $("#txtwxdeptname").val(data);
                        }
                        else {
                            alert("获取部门失败");
                        }
                    });
                }
            });
          });
        function check()
        {
               
            var reptname = $("#txtzccode").val();
            if (reptname.length < 1) {
                alert("资产编号不能为空！");
                $("#txtreportname").focus();
                return false;
            }
              var reptmoney = $("#txtwxmoney").val();
            if (reptmoney.length < 1) {
                alert("金额不能为空！");
                $("#txtreportname").focus();
                return false;
            }
            else
            {
                return true;
            
            }

        }
        //替换非数字
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
            <td colspan="6" class="billtitle" style="text-align: center; height: 26px; font-family: @华文宋体; font-size: medium;
                    font-weight: 800; background-color:#EDEDED;">
                维护记录
            </td>
        </tr>
        <tr>
            <td style="text-align: right" class="InputLabel">
                资产编号：
            </td>
            <td>
                <asp:TextBox ID="txtzccode" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: right"  class="InputLabel">
                维修人：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtwxname" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: right"  class="InputLabel">
                维修部门：
            </td>
            <td class="baseText">
            
                <asp:TextBox ID="txtwxdeptname"  ReadOnly="true" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right"  class="InputLabel">
                维修金额：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtwxmoney" runat="server" onkeyup="replaceNaN(this);"></asp:TextBox>
            </td>
            <td style="text-align: right"  class="InputLabel">
                维修类别：
            </td>
            <td class="baseText">
                <asp:DropDownList ID="ddlSalewxlb" runat="server" Width="152" >
                </asp:DropDownList>
            </td>
            <td colspan="2">
            </td>

            
        </tr>
        <tr>
            <td style="text-align: right"  class="InputLabel">
                备注：
            </td>
            <td colspan="5" class="baseText">
                <asp:TextBox ID="txtreportexplain" runat="server" TextMode="MultiLine" Height="250px"
                    Width="98%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="6" style="text-align: center">
                <asp:Button ID="btnsave" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="return check();"
                    OnClick="btnsave_Click" />
                <asp:Button ID="btnclose" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close();" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
