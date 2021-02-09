<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YSDatesDeptTongji.aspx.cs" Inherits="webBill_tjbb_YSDatesDeptTongji" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>日期部门单选查询（查询3）</title>
<link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery.multiselect.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.multiselect.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script type="text/javascript">
        $(function() {
        $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                parent.helptoggle();
                }
            });
        $("#TextBox1").datepicker();
        $("#TextBox2").datepicker();
           
        });
        function toSearch() {
        var t1=$("#TextBox1").val().length;
            var t2=$("#TextBox2").val().length;

            if (t1==0) {
                alert("请填写查询时间");
                $("#TextBox1").focus();
                return false;
            } 
            else if(t2==0)
            {
            alert("请填写查询时间");
                $("#TextBox2").focus();
            }
            else {
                return true;
            }
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div style="width: 400px; margin: 0 auto;">
        <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
            <tr>
                <td style="height: 125px; text-align: center">
                </td>
            </tr>
            <tr>
                <td style="text-align: center; height: 27px;">
                    <strong><span style="font-size: 12pt">部门预算统计</span></strong>
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable">
                        <tr>
                            <td class="tableBg">
                                开始时间
                            </td>
                            <td colspan="2" style="width: 257px">
                                <asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                截止时间
                            </td>
                            <td colspan="2" style="width: 257px">
                                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                       
                        <tr>
                            <td class="tableBg">
                                统计单位
                            </td>
                            <td colspan="2" style="width: 257px">
                                <asp:DropDownList ID="drpDept" runat="server">
                                    <asp:ListItem>所有单位</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center">
                    <asp:Button ID="Button1" runat="server" CssClass="baseButton" OnClick="Button1_Click"
                        Text="生成统计表" OnClientClick="return toSearch();" />
                          <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                    <asp:HiddenField ID="hf_dept" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
