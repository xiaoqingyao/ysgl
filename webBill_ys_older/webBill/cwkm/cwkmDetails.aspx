<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cwkmDetails.aspx.cs" Inherits="cwkm_cwkmDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>财务科目</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript" language="javascript">
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
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" style="height: 35px; text-align: center">
                <strong><span style="font-size: 12pt">财 &nbsp; 务 &nbsp; 科 &nbsp; 目</span></strong>
            </td>
        </tr>
        <tr>
            <td align="left" style="text-align: center">
                <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 449px">
                    <tr>
                        <td class="tableBg">
                            科目编号
                        </td>
                        <td>
                            <asp:TextBox ID="txb_kmcode" runat="server" ReadOnly="True"></asp:TextBox>
                            <asp:Button ID="btnAgain" runat="server" Text="生成编号" CssClass="baseButton" OnClick="btnAgain_Click"
                                Visible="False" CausesValidation="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            科目代码
                        </td>
                        <td style="width: 200px">
                            <asp:TextBox ID="txb_kmbm" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txb_kmbm"
                                ErrorMessage="科目代码不能为空！">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            科目名称
                        </td>
                        <td colspan="1">
                            <input id="txb_kmmc" runat="server" style="width: 282px" type="text" />&nbsp;
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txb_kmmc"
                                ErrorMessage="科目名称不能为空！">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            核算项目一
                        </td>
                        <td colspan="1">
                            <input id="Text1" runat="server" style="width: 282px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            核算项目二
                        </td>
                        <td colspan="1">
                            <input id="Text2" runat="server" style="width: 282px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            核算项目三
                        </td>
                        <td colspan="1">
                            <input id="Text3" runat="server" style="width: 282px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            核算项目四
                        </td>
                        <td colspan="1">
                            <input id="Text4" runat="server" style="width: 282px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            核算项目五
                        </td>
                        <td colspan="1">
                            <input id="Text5" runat="server" style="width: 282px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            显示名称
                        </td>
                        <td colspan="1">
                            <input id="txtXianShiMc" runat="server" style="width: 282px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            科目类型
                        </td>
                        <td colspan="1">
                            <input id="txtType" runat="server" style="width: 282px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            方向
                        </td>
                        <td colspan="1">
                            <input id="txtFangxiang" runat="server" style="width: 282px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            级次
                        </td>
                        <td colspan="1">
                            <input id="txtJiCi" runat="server" style="width: 282px"  onkeyup="replaceNaN(this);"  type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            辅助核算
                        </td>
                        <td colspan="1">
                            <input id="txtFuZhuHeSuan" runat="server" style="width: 282px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            是否封存
                        </td>
                        <td colspan="1">
                            <asp:DropDownList ID="txtShiFouFengCun" runat="server" Width="95%">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>
                            </asp:DropDownList>
                           <%-- <input id="txtShiFouFengCun" runat="server" style="width: 282px" type="text" />--%>
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
