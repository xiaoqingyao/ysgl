<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalepreassDetails.aspx.cs" Inherits="SaleBill_Salepreass_SalepreassDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>销售过程</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
     <base target="_self" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
   
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>
    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />
  
    <script language="javascript" type="text/javascript">
       
        //为用户显示提示信息
        function showMessage(msg) {
            $("#lblMsg").html(msg);
        }
        //取消按钮
        function Canle() {
        window.close();
           
        }
        
        function chage()
        {
            var vname=document.getElementById("txtname").value;
            if (vname==null||vname=="")
            {
                alert("名称不能为空");
               document.getElementById("txtname").focus(); 
                return;
            }
        }
        
    </script>
    <style type="text/css">
        .style1
        {
            width: 118px;
        }
        .style2
        {
            width: 127px;
        }
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
    <div id="mange" style="background-color: White; width:100%" >
        <table id="Table1" style="width:100%;" cellpadding="0" cellspacing="0" class="baseTable" runat="server">
            <tr >
                <td class="style1" style=" text-align:right">
                    编号:
                </td>
                <td id="Td1" style="width:200" runat="server" >
                    <asp:TextBox ID="txtnid" runat="server"  Width="90%" CssClass="baseText" Enabled="false"></asp:TextBox>
                    <div id="yztxtnid" style="color: Red; float: right">
                        *</div>
                </td>
               
            </tr>
          
            <tr>
                <td class="style1" style=" text-align:right">
                    名称:
                </td>
                <td >
                    <asp:TextBox ID="txtname" runat="server"  Width="90%"  CssClass="baseText"  ></asp:TextBox>
                     <div id="Div1" style="color: Red; float: right">
                        *</div>
                </td>
               
               
                
            </tr>
            <tr>
                <td style=" text-align:right">
                   状态:  
                </td>
                <td style="width:500" >
                   <asp:RadioButton ID="redY" runat="server" Text="是" Checked="true" GroupName="a"/>
                     <asp:RadioButton ID="redN" runat="server" Text="否" GroupName="a" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3" style="text-align: center">
                        <asp:Button ID="Button4" runat="server" Text="确  定" CssClass="baseButton" OnClick="Button1_Click"   />
                        <asp:Button ID="Button1" runat="server" Text="取 消" CssClass="baseButton"  OnClientClick="javascript:window.close();"   />

                 </td>
            </tr>
        </table>
       <table>
            <tr>
                <td style=" color:Red;">
                    【友情提示】：<asp:Label runat="server" ID="lblMsg">
                    红色*标示的项为必填项。</asp:label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

