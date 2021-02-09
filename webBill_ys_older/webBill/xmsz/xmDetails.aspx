<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xmDetails.aspx.cs" Inherits="Dept_deptDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>项目信息</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
     
    <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>


    
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
 <style type="text/css">
        .righttxt
        {
            text-align: right;
        }
    </style>

    <script language="javascript" type="text/javascript"> 
    function selectdept(w,a)
    {
        var str=window.showModalDialog(w, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:850px;status:no;scroll:yes');
        if(str!=undefined)
        { 
             document.getElementById(a).value=str;
        }
    }
    
        //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left" style="height: 35px; text-align: center">
                    <strong><span style="font-size: 12pt">项 &nbsp; 目&nbsp; &nbsp;信 &nbsp; 息</span></strong></td>
            </tr>
            <tr>
                <td align="left" style="text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 449px">
                        <tr>
                            <td class="tableBg">
                                项目编号</td>
                            <td>
                                <asp:TextBox ID="txb_deptcode" runat="server" Width="180px"></asp:TextBox>
                                <asp:Button ID="btnAgain" runat="server" Text="生成编号" CssClass="baseButton" OnClick="btnAgain_Click"
                                    Visible="False" CausesValidation="False" /></td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                项目名称</td>
                            <td style="width: 200px">
                                <asp:TextBox ID="txb_deptname" runat="server" Width="180px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txb_deptname"
                                    ErrorMessage="项目名称不能为空！">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                项目部门</td>
                            <td colspan="1">
                                <input id="txb_sjdept" runat="server" readonly="readonly" style="width: 180px" type="text" />
                                <asp:Button ID="btn_seledept" runat="server" Text="选择" CssClass="baseButton" CausesValidation="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                上级项目</td>
                            <td colspan="1"><input id="txtSjxm" runat="server" readonly="readonly" style="width: 180px" type="text" />
                                <asp:Button ID="btn_selectXm" runat="server" Text="选择" CssClass="baseButton" CausesValidation="False" /><input type="button" value="清" class="baseButton" causesvalidation="false" id="Button1" runat="server" onserverclick="Button1_ServerClick"/></td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                项目状态</td>
                            <td colspan="1">
                                <asp:DropDownList ID="DropDownList1" runat="server">
                                    <asp:ListItem Value="1">正常</asp:ListItem>
                                    <asp:ListItem Value="0">停用</asp:ListItem>
                                </asp:DropDownList></td>
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
