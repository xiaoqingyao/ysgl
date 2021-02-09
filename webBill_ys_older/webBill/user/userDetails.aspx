<%@ Page Language="C#" AutoEventWireup="true" CodeFile="userDetails.aspx.cs" Inherits="user_userDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>人员信息</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
     
    <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>


    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript"> 
    function selectDept(url) {
        var str=window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:850px;status:no;scroll:yes');
        if(str!=undefined)
        { 
             document.getElementById("txtDept").value=str;
        }
    }
    function selectPosition(url) {
        var str = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:850px;status:no;scroll:yes');
        if (str != undefined) {
            document.getElementById("txtPosition").value = str;
        }
    }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left" style="height: 35px; text-align: center">
                    <strong><span style="font-size: 12pt">人&nbsp; 员&nbsp; &nbsp;信 &nbsp; 息</span></strong></td>
            </tr>
            <tr>
                <td align="left" style="text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 400px">
                        <tr>
                            <td class="tableBg">
                                人员编号</td>
                            <td style="width: 200px">
                                <asp:TextBox ID="txb_usercode" runat="server" Width="157px"></asp:TextBox>
                                <asp:Button ID="btnAgain" runat="server" CausesValidation="False" CssClass="baseButton"
                                    OnClick="btnAgain_Click" Text="生成编号" Visible="False" /></td></tr><tr>
                            <td class="tableBg">
                                人员姓名</td> 
                            <td style="width: 200px">
                                <asp:TextBox ID="txb_username" runat="server" Width="157px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txb_username"
                                    ErrorMessage="人员姓名不能为空！">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                职务
                            </td>
                            <td>
                                <input id="txtPosition" runat="server" type="text" style="width: 157px" readonly="readOnly" />
                                <input id="btn_selectPosition" type="button" value="选择" class="baseButton" onclick="selectPosition('../select/PositionList.aspx');" />
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPosition"
                                    ErrorMessage="职务信息不能为空！">*</asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                角色</td>
                            <td>
                                <asp:DropDownList ID="ddl_group" runat="server" Width="158px">
                                </asp:DropDownList>
                            </td></tr><tr>
                            <td class="tableBg">
                                所在部门</td>
                            <td>
                                <input id="txtDept" runat="server" type="text" style="width: 157px" readonly="readOnly" />
                                <asp:Button ID="btn_seldept" runat="server" Text="选择" CssClass="baseButton" CausesValidation="False" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDept"
                                    ErrorMessage="所在部门不能为空！">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                人员状态</td>
                            <td style="width: 200px">
                                <asp:RadioButton ID="rdoUserStatus1" runat="server" Checked="True" GroupName="userStatus"
                                    Text="正常" />
                                <asp:RadioButton ID="rdoUserStatus2" runat="server" GroupName="userStatus" Text="禁用" /></td></tr><tr>
                            <td class="tableBg">
                                是否是管理员
                            </td>
                            <td style="width: 200px">
                                <asp:RadioButton ID="rdoIsSystem0" runat="server" Checked="True" GroupName="isSystem"
                                    Text="否" />
                                <asp:RadioButton ID="rdoIsSystem1" runat="server" GroupName="isSystem" Text="是" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="text-align: center; height: 35px;">
                    <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click" />
                    &nbsp;&nbsp;&nbsp;<input class="baseButton" type="button" onclick="javascript:window.close();" value="取 消" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" />
    </form>
</body>
</html>
