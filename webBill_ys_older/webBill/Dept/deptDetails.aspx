<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deptDetails.aspx.cs" Inherits="Dept_deptDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>部门信息</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function selectdept(w, a) {
            var str = window.showModalDialog(w, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:850px;status:no;scroll:yes');
            if (str != undefined) {
                document.getElementById(a).value = str;
            }
        }
        function selectTianJianID() {
            var rel = window.showModalDialog('selectTianJianDept.aspx', 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:850px;status:no;scroll:yes');
            if (rel != undefined) {
                $("#txtForTianJian").val(rel);
            }

        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left" style="height: 35px; text-align: center">
                    <strong><span style="font-size: 12pt">部 &nbsp; 门&nbsp; &nbsp;信 &nbsp; 息</span></strong>
                </td>
            </tr>
            <tr>
                <td align="left" style="text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 449px">
                        <tr>
                            <td class="tableBg">部门编号
                            </td>
                            <td>
                                <asp:TextBox ID="txb_deptcode" runat="server"></asp:TextBox>
                                <asp:Button ID="btnAgain" runat="server" Text="生成编号" CssClass="baseButton" OnClick="btnAgain_Click"
                                    Visible="False" CausesValidation="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">部门名称
                            </td>
                            <td style="width: 200px">
                                <asp:TextBox ID="txb_deptname" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txb_deptname"
                                    ErrorMessage="部门名称不能为空！">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">上级部门
                            </td>
                            <td colspan="1">
                                <input id="txb_sjdept" runat="server" readonly="readonly" type="text" />
                                <asp:Button ID="btn_seledept" runat="server" Text="选" CssClass="baseButton" CausesValidation="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">是否归口
                            </td>
                            <td style="width: 200px">
                                <asp:RadioButton ID="isGkY" runat="server" Text="是" GroupName="Isgk" />
                                <asp:RadioButton ID="isGkN" runat="server" Text="否" GroupName="Isgk" />
                            </td>
                        </tr>
                        <tr runat="server" id="trisSale">
                            <td class="tableBg">是否销售公司
                            </td>
                            <td style="width: 200px">
                                <asp:RadioButton ID="isIsSellY" runat="server" Text="是" GroupName="IsSe" />
                                <asp:RadioButton ID="isIsSellN" runat="server" Text="否" GroupName="IsSe" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">部门状态
                            </td>
                            <td style="width: 200px">
                                <asp:RadioButton ID="RadioButton1" runat="server" Text="正常" Checked="True" GroupName="status" />
                                <asp:RadioButton ID="RadioButton2" runat="server" Text="禁用" GroupName="status" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">部门简码
                            </td>
                            <td style="width: 200px">
                                <asp:TextBox runat="server" ID="txtJianma" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">对应用友系统编号
                            </td>
                            <td style="width: 200px">
                                <asp:TextBox runat="server" ID="txtForU8id" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">对应天健编号
                            </td>
                            <td style="width: 200px">
                                <asp:TextBox runat="server" ID="txtForTianJian" Text="" CssClass="baseTextReadOnly" />
                                <input type="button" value="选" class="baseButton" onclick="selectTianJianID();" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">是否占用财年预算
                            </td>
                            <td style="width: 200px">
                                <asp:RadioButton ID="Yiskzys" Checked="true" runat="server" Text="是" GroupName="Iskzys" />
                                <asp:RadioButton ID="Niskzys" runat="server" Text="否" GroupName="Iskzys" />
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
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" />
    </form>
</body>
</html>
