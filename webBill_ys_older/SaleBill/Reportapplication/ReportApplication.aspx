<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportApplication.aspx.cs"
    Inherits="SaleBill_ReportApplication_ReportApplication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报告申请单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
         $(function(){
        
            //部门选择
            $("#txtreportdept").autocomplete({
                source: availableTagsdt,
                  select: function(event, ui) {
                    var rybh = ui.item.value;
                }
            });
           //报告人
              $("#txtreportname").autocomplete({
                source: availableTagsuser,
                  select: function(event, ui) {
                    var rybh = ui.item.value;
                }
            });
        });
        
        function check()
        {
               
            var reptname = $("#txtreportname").val();
            var reptdept=$("#txtreportdept").val();
            var redate=$("#txtreportdate").val();

            if (reptname.length < 1) {
                alert("报告人不能为空！");
                $("#txtreportname").focus();
                return false;
            }
            if(reptdept.length<1){
                 alert("报告单位不能为空！");
                $("#txtreportdept").focus();
                return false;
            }
             if(redate.length<1){
                 alert("报告日期不能为空！");
                $("#txtreportdate").focus();
                return false;
            }
            else
            {
                return true;
            
            }

        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table class="baseTable" width="95%">
        <tr>
            <td colspan="6" class="billtitle" style="text-align: center">
                报告申请单
            </td>
        </tr>
        <tr>
            <td style="text-align: right"  class="InputLabel">
                报告单号：
            </td>
            <td>
                <asp:Label ID="lblcode" runat="server" Text=""></asp:Label>
            </td>
            <td style="text-align: right">
                报告人：
            </td>
            <td class="baseText">
                <asp:TextBox ID="txtreportname" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                报告单位：
            </td>
            <td  class="baseText">
                <asp:TextBox ID="txtreportdept" runat="server"></asp:TextBox>
            </td>
            <td style="text-align: right">
                报告日期：
            </td>
            <td  class="baseText">
                <asp:TextBox ID="txtreportdate" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                报告说明：
            </td>
            <td colspan="3" class="baseText">
                <asp:TextBox ID="txtreportexplain" runat="server" TextMode="MultiLine" Height="150px" Width="98%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="text-align: center">
                <asp:Button ID="btnsave" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="return check();"
                    OnClick="btnsave_Click" />
                <asp:Button ID="btnclose" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close();" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
