<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Saleselect.aspx.cs" Inherits="SaleBill_SaleFee_Saleselect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../webBill/Resources/Css/jquery.multiselect.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery.multiselect.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#drpDept").multiselect({
                header: false,
                noneSelectedText: "请选择一个部门",
                selectedText: function(numChecked, numTotal, checkedItems) {
                    return numChecked + '个部门被选中了';
                }
            });
        });
        function toSearch() {
            var retstr = "";
            $("input[type='checkbox']:checked").each(function() {
                retstr += $(this).val() + "|";
            });
            if (retstr == "") {
                alert("请选择单位");
                return false;
            } else {
                retstr = retstr.substring(0, retstr.length - 1);
                $("#hf_dept").val(retstr);
                return true;
            }
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div style="width: 400px; margin: 0 auto;">
        <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%" border="0">
            <tr>
                <td style="height: 125px; text-align: center">
                </td>
            </tr>
            <tr>
                <td style="text-align: center; height: 27px;">
                    <strong><span style="font-size: 12pt">费用科目预算统计</span></strong>
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="100%">
                        <tr>
                            <td class="tableBg" style="text-align: right">
                                开始时间：
                            </td>
                            <td colspan="2" style="width: 257px">
                                <asp:TextBox ID="txtDateFrm" runat="server" Width="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg" style="text-align: right">
                                截止时间：
                            </td>
                            <td colspan="2" style="width: 257px">
                                <asp:TextBox ID="txtDateTo" runat="server" Width="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                统计单位
                            </td>
                            <td colspan="2" style="width: 100px">
                                <asp:DropDownList ID="drpDept" runat="server">
                                    <asp:ListItem>所有单位</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: right; height: 50px;">
                    <asp:Button ID="Button1" runat="server" CssClass="baseButton" OnClientClick="return toSearch();" OnClick="Button1_Click"
                        Text="生成统计表" />
                         <asp:HiddenField ID="hf_dept" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
