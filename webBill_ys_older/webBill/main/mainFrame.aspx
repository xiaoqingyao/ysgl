<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mainFrame.aspx.cs" Inherits="main_mainFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>中国重汽(香港)有限公司橡塑件制造部 费用内控及网上报销系统 v1.0</title> <meta http-equiv="X-UA-Compatible" content="IE=8" />
     <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $("body").focus();
        $(this).keypress(function(event) {
            var key = event.keyCode;
            if (key == 13) {
                var url = '../bxgl/bxDetailFinal.aspx?type=add&par=' + Math.random();
                window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            } else if (key == 106) {
                var Url2 = '../../SaleBill/BorrowMoney/LoanListDetails.aspx?Ctrl=Add&par=' + Math.random();
                window.showModalDialog(Url2, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:900px;status:no;scroll:yes');
            }
        });
    </script>
</head>
<frameset rows="80,*" id="mainFrame" framespacing="0" frameborder="0"  >
    <frame id="top" name="top" src="top.aspx" noresize scrolling="no" />
    <frame id="main" name="main" src="MainNew.aspx" noresize scrolling="no"></frame>
</frameset>
</html>
