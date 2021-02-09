<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zhyscxFrame.aspx.cs" Inherits="webBill_search_zhyscxFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
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
            $("#TextBox1").datepicker();
            $("#TextBox2").datepicker();
            $("#DropDownList1").multiselect({
                header: false,
                noneSelectedText: "请选择一个部门",
                selectedText: function (numChecked, numTotal, checkedItems) {
                    return numChecked + '个部门被选中了';
                }
            });

            $("#drp_fykm").multiselect({
                header: false,
                noneSelectedText: "请选择一个费用科目",
                selectedText: function (numChecked, numTotal, checkedItems) {
                    return numChecked + '个费用科目被选中了';
                }
            });
        });
        function toSearch() {
            var retstr = "";
            $("input[type='checkbox']:checked").each(function () {
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
                        <strong><span style="font-size: 12pt">综合预算汇总</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="height: 26px; text-align: center">
                        <table border="0" cellpadding="0" cellspacing="0" class="myTable">
                            <tr>
                                <td class="tableBg">开始时间
                                </td>
                                <td colspan="2" style="width: 257px">
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">截止时间
                                </td>
                                <td colspan="2" style="width: 257px">
                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">单据类型
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownList2" runat="server">
                                        <asp:ListItem Value="0">全部</asp:ListItem>
                                        <asp:ListItem Value="-1">未提交</asp:ListItem>
                                        <asp:ListItem Value="end">审批通过</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">选择财年
                                </td>
                                <td colspan="2" style="width: 257px">
                                    <asp:DropDownList ID="drpSelectNd" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">费用科目
                                </td>
                                <td colspan="2" style="width: 257px">
                                    <asp:DropDownList ID="drp_fykm" runat="server">
                                        <asp:ListItem>所有科目</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">统计单位
                                </td>
                                <td colspan="2" style="width: 257px">
                                    <asp:DropDownList ID="DropDownList1" runat="server">
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
                        <asp:Button ID="Button2" runat="server" CssClass="baseButton" OnClick="Button2_Click"
                            Text="打印预览" OnClientClick="return showWait();" />
                        <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                        <asp:HiddenField ID="hf_dept" runat="server" />
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
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
