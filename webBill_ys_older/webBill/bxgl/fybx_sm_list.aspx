<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fybx_sm_list.aspx.cs" Inherits="webBill_bxgl_fybx_sm_list"  EnableViewState="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>费用报销说明</title>
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function openDetail(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:600px;status:no;scroll:no');
            location.replace(location.href);
        }
    </script>

</head>
<body>
    <input id="btn_add" type="button" value="新 增" onclick="openDetail('fykm_sm_details.aspx')"
        style="margin-left: 5px" />
    <form id="form1" runat="server">
    <div id="divshow" runat="server">
    </div>
    </form>
</body>
</html>
