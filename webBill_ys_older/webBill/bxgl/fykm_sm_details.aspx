<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fykm_sm_details.aspx.cs"
    Inherits="webBill_bxgl_fykm_sm_details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>费用报销说明详细页</title>
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #txtsm
        {
            height: 117px;
            width: 449px;
        }
        #txtfykm
        {
            width: 452px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="baseTable">
            <tr>
                <td>
                    费用科目：
                </td>
                <td>
                    <input runat="server" type="text" id="txtfykm" name="txtfykm" />
                </td>
            </tr>
            <tr>
                <td>
                    报销说明：
                </td>
                <td>
                    <textarea runat="server" id="txtsm" name="txtsm"></textarea>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right">
                    <input type="submit" class="baseButton" value="保 存" />&nbsp;
                    <input type="button" class="baseButton" value="取 消" onclick="javascript:self.close();" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
