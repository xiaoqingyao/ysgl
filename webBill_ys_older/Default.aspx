<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <script src="webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

     <link href="webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" />
    <script src="webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"></script>
     <script>
         $(function () {
             alert("");
             $("#dialog").dialog({
                 buttons: { "Ok": function () { $(this).dialog("close"); } }
             });
             
         });
     </script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
