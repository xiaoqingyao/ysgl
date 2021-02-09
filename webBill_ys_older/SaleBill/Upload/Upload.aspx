<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Upload.aspx.cs" Inherits="SaleBill_Upload_Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>选择图片</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../../App_Themes/Themes.css" rel="stylesheet" type="text/css" />
   
    <link rel="stylesheet" type="text/css" id="themesMainCss" href="" />

    <script src="../../AScripts/Common.js" type="text/javascript"></script>

    <script src="../../AScripts/Themes.js" type="text/javascript"></script>

   <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery1.32.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <script src="../../webBill/fysq/js/JScript.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
    function selectFile()
    {
        
        var file =document.getElementById('<%=txtFile.ClientID %>').value;
        var fileName = document.getElementById('<%=txtFileNames.ClientID %>').value;
       
	    parent.CloseWithParam(file,fileName,'<%=_strType %>');

	   document.getElementById('<%=txtFileNames.ClientID %>').value = '';
	   document.getElementById('<%=txtFile.ClientID %>').value = '';
    }
    function document.oncontextmenu() {return false};
    </script>

</head>
<body style="margin: 0px;">
    <form id="form1" runat="server" enctype="multipart/form-data" target="_self">
        <asp:HiddenField ID="cFilesPath" Value="../../Uploads/tsflment/" runat="server" />
        <asp:HiddenField ID="cImagePath" Value="../Uploads/Image/" runat="server" />
        <asp:HiddenField ID="cProductPath" Value="../Uploads/ProductImage/" runat="server" />
        <asp:HiddenField ID="cMediaPath" Value="../Uploads/Media/" runat="server" />
        <asp:HiddenField ID="cFlashPath" Value="../Uploads/Flash/" runat="server" />
        <asp:HiddenField ID="cPhotoPath" Value="../Uploads/PersonPho/" runat="server" />
        <div>
            <table height="30" cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td id="trComment"  runat="server">
                        <asp:TextBox ID="txtFileNames"  runat="server" Width="100px" Height="30" ></asp:TextBox>
                        <asp:TextBox ID="txtFile" runat="server" Style="border: 0px; display:none; "></asp:TextBox>
                    </td>
                    <td valign="middle" style=" width:99%">
                        <asp:FileUpload ID="ctrlPicture" name="ctrlPicture" CssClass="baseButton" runat="server" Height="20px" Width="80%"/>
                        <asp:Button ID="btnAcc" runat="server" Text="上 传" OnClick="btnAcc_Click"  CssClass="baseButton" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
