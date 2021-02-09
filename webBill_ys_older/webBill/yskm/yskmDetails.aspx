<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yskmDetails.aspx.cs" Inherits="webBill_yskm_yskmDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算科目</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.multiselect.min.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        $(function () {
            $("#btn_choose").click(function () {
                selectry();
            });
        });

        function selectry() {
            var str = window.showModalDialog('../select/SelectMoreUserFrame.aspx', 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:750px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                $("#txt_spr").val(str);
                $("#HiddenField1").val(str);
            }
        }
    </script>

    <style type="text/css">
        .style1 {
            background-color: #EDEDED;
            width: 100px;
            text-align: center;
            height: 35px;
        }

        .style2 {
            height: 35px;
        }
    </style>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left" style="height: 35px; text-align: center">
                    <strong><span style="font-size: 12pt">预 &nbsp; 算 &nbsp; 科 &nbsp; 目</span></strong>
                </td>
            </tr>
            <tr>
                <td align="left" style="text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 449px">
                        <tr>
                            <td class="tableBg">科目编号
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txb_kmcode" runat="server" ReadOnly="True" Width="180px"></asp:TextBox>
                                <asp:Button ID="btnAgain" runat="server" Text="生成编号" CssClass="baseButton" OnClick="btnAgain_Click"
                                    Visible="False" CausesValidation="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">科目名称
                            </td>
                            <td colspan="1" align="left">
                                <input id="txb_kmmc" runat="server" style="width: 180px" type="text" />&nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txb_kmmc"
                                ErrorMessage="科目名称不能为空！">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">填报说明
                            </td>
                            <td colspan="1" align="left">
                                <input id="txtTbsm" runat="server" style="width: 180px" type="text" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">填报类型
                            </td>
                            <td colspan="1" align="left" class="style2">
                                <asp:DropDownList ID="DropDownList1" runat="server" Width="100px">
                                    <asp:ListItem Selected="True" Value="01">单位填报</asp:ListItem>
                                    <asp:ListItem Value="02">财务填报</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">科目状态
                            </td>
                            <td colspan="1" align="left">
                                <asp:DropDownList ID="DropDownList2" runat="server" Width="100px">
                                    <asp:ListItem Selected="True" Value="1">正常</asp:ListItem>
                                    <asp:ListItem Value="0">禁用</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">是否可控
                            </td>
                            <td colspan="1" align="left">
                                <asp:DropDownList ID="DropDownList3" runat="server" Width="100px">
                                    <asp:ListItem Value="0">可控费用</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="1">不可控费用</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="tableBg">费用类型
                            </td>
                            <td colspan="1" align="left">
                                <asp:DropDownList ID="DropDownList4" runat="server" Width="100px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">归口费用
                            </td>
                            <td colspan="1" align="left">
                                <asp:RadioButton ID="gkfy" runat="server" Text="否" GroupName="gkfy" Checked="true" />
                                <asp:RadioButton ID="gkfy1" runat="server" Text="是" GroupName="gkfy" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">项目核算
                            </td>
                            <td colspan="1" align="left">
                                <asp:RadioButton ID="xmhs" runat="server" Text="否" GroupName="xmhs" Checked="true" />
                                <asp:RadioButton ID="xmhs1" runat="server" Text="是" GroupName="xmhs" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">部门核算
                            </td>
                            <td colspan="1" align="left">
                                <asp:RadioButton ID="bmhs" runat="server" Text="否" GroupName="bmhs" Checked="true" />
                                <asp:RadioButton ID="bmhs1" runat="server" Text="是" GroupName="bmhs" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">人员核算
                            </td>
                            <td colspan="1" align="left">
                                <asp:RadioButton ID="ryhs" runat="server" Text="否" GroupName="ryhs" Checked="true" />
                                <asp:RadioButton ID="ryhs1" runat="server" Text="是" GroupName="ryhs" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">加减核算
                            </td>
                            <td colspan="1" align="left">
                                <asp:RadioButton ID="rdbJian" runat="server" Text="减项" GroupName="zjhs" Checked="true" />
                                <asp:RadioButton ID="rdbAdd" runat="server" Text="加项" GroupName="zjhs" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">科目主管
                            </td>
                            <td colspan="1" align="left">
                                <input type="text" runat="server" id="txt_spr" style="width: 180px" />
                                <input type="button" class="baseButton" id="btn_choose" value="选择" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tableBg">预算类型
                            </td>
                            <td align="left">
                                <asp:DropDownList runat="server" ID="ddlBill">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">允许科目调整
                            </td>
                            <td colspan="1" align="left">
                                <asp:RadioButton ID="allowTzNo" runat="server" Text="否" GroupName="allowTz" />
                                <asp:RadioButton ID="allowTzYes" runat="server" Text="是" GroupName="allowTz" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">占用预算总额
                            </td>
                            <td colspan="1" align="left">
                                <asp:RadioButton ID="allowZyysYes" runat="server" Text="是" GroupName="Zyys" />
                                <asp:RadioButton ID="allowZyysNo" runat="server" Text="否" GroupName="Zyys" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="text-align: center; height: 35px;">
                    <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click" />
                    &nbsp;&nbsp;&nbsp;<asp:Button ID="btn_cancel" runat="server" Text="取 消" CssClass="baseButton"
                        OnClick="btn_cancel_Click" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" />
    </form>
</body>
</html>
