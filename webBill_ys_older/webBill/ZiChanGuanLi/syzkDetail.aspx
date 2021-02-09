<%@ Page Language="C#" AutoEventWireup="true" CodeFile="syzkDetail.aspx.cs" Inherits="ZiChan_ZiChanGuanLi_syzkDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>使用状况编辑页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
<base target="_self" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            //取消
            $("#btn_Cancle").click(function() {
                window.returnValue = "";
                self.close();
            });
            $("#txtParent").autocomplete({
                source: availableTags
            });
        });
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
         //非空验证
         function checkEdit() {
             var needAmount = $("#txtName").val();
           
            if (needAmount.length < 1) {
                alert("名称不能为空！");
                $("#txtName").focus();
                return false;
            } 
            else {
                return true;
            }
        }
        
     
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div class="baseDiv" style="text-align: center">
        <table border="0" width="99%" class="baseTable">
            <tr>
                <td style="text-align: right; width: 20%;" class="tableBg2" >
                    状况编号：
                </td>
                <td>
                    <asp:TextBox ID="txtCode" runat="server" CssClass="baseText" Width="95%"  ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right" class="tableBg2" > 
                    状况名称：
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" CssClass="baseText" Width="95%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right" class="tableBg2" >
                    上级类型：
                </td>
                <td>
                    <span style="width: 95%">
                        <asp:TextBox ID="txtParent" runat="server" CssClass="baseText" Width="70%"></asp:TextBox>
                        <asp:CheckBox ID="chbRoot" runat="server" Text="设为根节点" />
                    </span>
                </td>
            </tr>
             <tr>
                <td style="text-align: right" class="tableBg2" >
                    备注：
                </td>
                <td>
                    <asp:TextBox ID="txtbeizhu" runat="server"  CssClass="baseText" Width="95%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="tableBg2" >
                    <div style="text-align: center">
                        <asp:Button ID="btn_Yes" runat="server" Text="确 定" OnClientClick="return checkEdit();"  OnClick="btn_Yes_Click" CssClass="baseButton" />
                        <input type="button" value="取 消" id="btn_Cancle" class="baseButton" /></div>
                    <asp:HiddenField ID="HiddenFieldCode" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

