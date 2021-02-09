<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleConfigDail.aspx.cs" Inherits="SaleBill_SaleConfig_SaleConfigDail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />

    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>
   
    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />
   
        <script type="text/javascript" language="javascript">
        $(function() {
        
        });
       
    </script>

    <style type="text/css">
        .style1
        {
            background-color: #EDEDED;
            width: 81px;
            text-align: center;
        }
    </style>

</head>

<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
     <div id="mange" style="background-color: White; width:100%" >
        <table cellpadding="0" cellspacing="0" width="100%" >
           
            <tr>
                <td align="left" style="text-align: center">
                    <table id="Table1" style="width:100%;" cellpadding="0" cellspacing="0" class="baseTable">
                        <tr>
                            <td class="tableBg" style="width: 127px">
                                名 称：
                           </td>
                           <td colspan="3">
                               <asp:TextBox ID="txtTitle" runat="server" Width="90%" CssClass="baseText"></asp:TextBox>
                                   <div id="Div1" style="color: Red; float: right">
                        *</div>
                               </td>
                              
                       </tr>
                        <tr>
                            <td class="style1">从控制点：
                             </td>
                           <td>
                               <asp:Label ID="lblkzbg" runat="server" Text="生产入库"></asp:Label>
                              <%--  <asp:DropDownList ID="DropDownList1" runat="server" Width="100px">
                                </asp:DropDownList>--%>
                            </td>
                             <td class="style1">到控制点：
                              </td>
                           <td>
                                <asp:DropDownList ID="DropDownList2" runat="server" Width="100px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                             <td class="tableBg" >
                               超限月数：</td>
                             <td>
                                 <asp:DropDownList ID="DropDownList3" runat="server" Width="100px">
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                    <asp:ListItem>4</asp:ListItem>
                                    <asp:ListItem>5</asp:ListItem>
                                    <asp:ListItem>6</asp:ListItem>
                                    <asp:ListItem>7</asp:ListItem>
                                    <asp:ListItem>8</asp:ListItem>
                                    <asp:ListItem>9</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>11</asp:ListItem>
                                    <asp:ListItem>12</asp:ListItem>
                                 </asp:DropDownList>
                               
                            </td>
                          <td style=" text-align:right">
                                 状 态：
                                </td>
                               <td>
                                   <asp:RadioButton ID="redY" runat="server" Text="是" Checked="true" GroupName="a"/>
                                     <asp:RadioButton ID="redN" runat="server" Text="否" GroupName="a" />
                                </td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                             备 注：</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemark" Width="90%"  TextMode="MultiLine" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                       
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="text-align: center; height: 35px;">
                    <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click" />
                    &nbsp;&nbsp;&nbsp;<asp:Button ID="btn_cancel" runat="server" Text="关 闭" CssClass="baseButton"
                        OnClick="btn_cancel_Click" CausesValidation="False" /></td>
            </tr>
        </table>
        <table runat="server" id="tbmsg">
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