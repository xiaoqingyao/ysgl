<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yszxqk_jz_select.aspx.cs" Inherits="webBill_search_yszxqk_jz_select" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算执行情况-截至月度</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
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
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            $("#DropDownList1").multiselect({
                header: false,
                noneSelectedText: "请选择一个部门",
                selectedText: function (numChecked, numTotal, checkedItems) {
                    return numChecked + '个部门被选中了';
                }
            });
        });
        function toSearch() {
            var retstr = "";
            $("input[type='checkbox']:checked").each(function () {
                retstr += $(this).val() + ",";
            });
            if (retstr == "") {
                alert("请选择单位");
                return false;
            } else {
                retstr = retstr.substring(0, retstr.length - 1);
                $("#hf_dept").val(retstr);
                showWait();
                return true;
            }
        }

        function showWait() {
            document.getElementById("divOver").style.visibility = "visible";
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <div style="width: 400px; margin: 0 auto;">
            <table cellpadding="0" id="taball" cellspacing="0" width="100%" style="margin: 0 auto">
                <tr>
                    <td style="height: 125px; text-align: center"></td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 27px;">
                        <strong><span style="font-size: 12pt">部门预算执行情况</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="height: 26px; text-align: center">
                        <table border="0" cellpadding="0" cellspacing="0" class="myTable">
                            <tr>
                                <td class="tableBg">年度
                                </td>
                                <td colspan="2" style="width: 257px">
                                    <asp:DropDownList ID="ddlNd" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">月度
                                </td>
                                <td colspan="2" style="width: 257px">
                                    <asp:DropDownList ID="ddlYd" runat="server">
                                        <asp:ListItem Value="1">01</asp:ListItem>
                                        <asp:ListItem Value="2">02</asp:ListItem>
                                        <asp:ListItem Value="3">03</asp:ListItem>
                                        <asp:ListItem Value="4">04</asp:ListItem>
                                        <asp:ListItem Value="5">05</asp:ListItem>
                                        <asp:ListItem Value="6">06</asp:ListItem>
                                        <asp:ListItem Value="7">07</asp:ListItem>
                                        <asp:ListItem Value="8">08</asp:ListItem>
                                        <asp:ListItem Value="9">09</asp:ListItem>
                                        <asp:ListItem Value="10">10</asp:ListItem>
                                        <asp:ListItem Value="11">11</asp:ListItem>
                                        <asp:ListItem Value="12">12</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td class="tableBg">统计单位
                                </td>
                                <td colspan="2" style="width: 257px">
                                    <asp:DropDownList ID="DropDownList1" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <tr>
                            <td style="height: 26px; text-align: center">
                                <asp:Button ID="Button1" runat="server" CssClass="baseButton" OnClick="Button1_Click"
                                    Text="生成统计表" OnClientClick="return toSearch();" />
                                <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                                <asp:HiddenField ID="hf_dept" runat="server" />
                            </td>
                        </tr>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divOver" runat="server" style="z-index: 1200; left: 30%; width: 160; cursor: wait; position: absolute; top: 25%; height: 100; visibility: hidden;">
            <table style="width: 17%; height: 10%;">
                <tr>
                    <td>
                        <table style="width: 316px; height: 135px;">
                            <tr align="center" valign="middle">
                                <td>
                                    <img src="../../webBill/Resources/Images/Loading/pressbar2.gif" alt="" /><br />
                                    <b>正在处理中，请耐心等候....<br />
                                    </b>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
    <script type="text/javascript">
        parent.closeAlert('UploadChoose');
    </script>
</body>
</html>
