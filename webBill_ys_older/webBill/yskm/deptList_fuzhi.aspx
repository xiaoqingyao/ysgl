<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deptList_fuzhi.aspx.cs" Inherits="webBill_yskm_deptList_fuzhi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" />
     <base target="_self" />
     <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
     <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
     <script type="text/javascript">
         $( function(){
         
          //部门选择
            $("#txtdept").autocomplete({
                source: availableTags
            });
         
         });
         
         function check()
         {
             var dept= $("#txtdept").val();
             if(dept.length==0)
             {
             alert("请选择要复制的部门");
             return false;
             }
             else
                return true;
         }
     </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        要复制的部门名词：<asp:TextBox ID="txtdept" runat="server"></asp:TextBox>
        <asp:Button ID="Button1"  runat="server" Text="确定" OnClick="Button1_click" OnClientClick="check()"  CssClass="baseButton" />
    </div>
    </form>
</body>
</html>
