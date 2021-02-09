<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClearData.aspx.cs" Inherits="ClearData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>清除业务/逻辑数据页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />

    <script src="webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
        });
        function yes() {
            var rdb = $('input:radio:checked').val();
            if (rdb == "rdbYW") {
                return confirm('您确定要删除全部业务数据吗？')

            }
            else if (rdb == "rdbJC") {
                return confirm('您确定要删除全部基础数据吗？')
            }
            else {
                return false;
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:RadioButton ID="rdbYW" runat="server" Text="清空业务数据" GroupName="clearItem" Checked="true" />
        <asp:RadioButton ID="rdbJC" runat="server" Text="清空基础数据" GroupName="clearItem" />
        <br />
        <asp:Button ID="btnYes" runat="server" Text="执行" OnClientClick="return  yes()" OnClick="btnYes_Click" />
        <input type="button" class="baseButton" value="帮助" onclick="javascript:parent.helptoggle();" />
    </div>
    </form>
</body>
</html>
