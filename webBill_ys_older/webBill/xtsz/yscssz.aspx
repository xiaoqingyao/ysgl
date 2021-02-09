<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yscssz.aspx.cs" Inherits="webBill_xtsz_yscssz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script language="javascript" type="text/javascript">
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });

            $(".baseTable input:text").datepicker();
        });
        function CheckSave() {
            return confirm("保存将提交设置！本次设置在下一年的预算中生效！你是否提交保存？");
        }
    </script>

</head>
<body style="text-align: left">
    <form id="form1" runat="server">
        <div style="text-align: left">
            <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="return CheckSave()"
                OnClick="btn_save_Click" />
            <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
            <div style="text-align: left; width: 70%">
                <table style="width: 100%; text-align: left" class="baseTable">
                    <tr>
                        <td style="width: 50%">预算类型设置： 年份：<asp:DropDownList ID="drpNd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpNd_SelectedIndexChanged">
                        </asp:DropDownList>
                        </td>
                        <td>费用预算填报方式设置：
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="CheckYear" runat="server" OnCheckedChanged="CheckYear_CheckedChanged"
                                AutoPostBack="true" />启用年度预算<br />
                            <asp:RadioButton ID="CheckSeason" runat="server" GroupName="ysfs" Text="启用季度预算" AutoPostBack="true"
                                OnCheckedChanged="CheckSeason_CheckedChanged" />
                            <asp:RadioButton ID="CheckMonth" runat="server" GroupName="ysfs" Text="启用月度预算" AutoPostBack="true"
                                OnCheckedChanged="CheckMonth_CheckedChanged" />
                        </td>
                        <td valign="top">
                            <div>
                                <asp:RadioButton ID="Checkbmsj" runat="server" GroupName="tbfs" Text="部门汇总填报" /><br />
                                <asp:RadioButton ID="Checkfztb" runat="server" GroupName="tbfs" Text="预算分解填报" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">状态 ：
                        <asp:RadioButton ID="rdbOn" runat="server" GroupName="status" Text="进行中" />
                            <asp:RadioButton ID="rdbClose" runat="server" GroupName="status" Text="关闭(关闭后本年度将无法再进行预算追加和费用报销)" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">预算时间设置：
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div>
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="baseTable"
                                    Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="ksmc" HeaderText="月份/季度/年度" />
                                        <asp:TemplateField HeaderText="开始时间">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtks" runat="server" Text='<%#Eval("kssj") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="jsmc" HeaderText="月份/季度/年度" />
                                        <asp:TemplateField HeaderText="截至时间">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtjs" runat="server" Text='<%#Eval("jssj") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
