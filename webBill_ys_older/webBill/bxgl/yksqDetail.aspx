<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yksqDetail.aspx.cs" Inherits="webBill_bxgl_yksqDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用款审批单打印</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
        .myTable input
        {
        	margin:5px;
        	}
    </style>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script type="text/javascript">
          $(function(){
          $("#txtSqrq").datepicker();
          $("#select_rk").click(function(){
             var url = 'yksqDetaile_selectRkd.aspx?type=m&par=' + Math.random();
            openDetail(url);
          });
          
          
              //部门选择
            $("#txtDept").autocomplete({
                source: availableTagsDept
            });
          
          });         
       

        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else
            {
               var srr=returnValue.split("|");
               $("#txtRkCodes").val(srr[0]);  
               $("#txtJe").val(srr[1]);  
            }
           
        }
        
         function CheckNAN(obj) {
            if (parseFloat($(obj).val()).toString() == "NaN")
                $(obj).val("");
        }
     
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div  style="background-color: #EBF2F5;">
        <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
            <tr>
                <td style="text-align: center; height: 36px;">
                    <strong>
                      用款审批单
                    </strong>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="margin: 0 auto; width: 98%;">
                        <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="100%">
                            <tr>
                                <td   class="tableBg2" style="width: 80px">
                                    经办人：
                                </td>
                                <td >
                                    <asp:TextBox ID="txtJbr" runat="server" ReadOnly="True" CssClass="basetext"></asp:TextBox>
                                </td>
                                      <td class="tableBg2" style="width: 80px">
                                    申请日期：
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtSqrq" runat="server" CssClass="basetext"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                               <td   class="tableBg2">
                                    所在部门：
                                </td>
                                <td  >
                                    <asp:TextBox ID="txtDept" runat="server"></asp:TextBox>
                                </td>
                                 <td   class="tableBg2">
                                    金额：
                                </td>
                                <td  >
                                    <asp:TextBox ID="txtJe" runat="server" onkeyup="CheckNAN(this)" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                              <td   class="tableBg2">
                                    入库单号：
                                </td>
                                <td colspan="3" >
                                    <asp:TextBox ID="txtRkCodes" runat="server" Width="90%" TextMode="MultiLine"></asp:TextBox> 
                                     <input runat="server"  type="button" value="选择"   id="select_rk" style="display:none"/>
                                </td>
                            </tr>
                            <tr>
                                <td   class="tableBg2">
                                   用途：
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtyt" runat="server"  Width="90%"   TextMode="MultiLine" Height="100px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        
                        
                        <div style="text-align:center">   
                        <div>
                             <asp:Label ForeColor="Red" Text="【友情提示】：选完入库单后金额默认为所选单据的总金额，也可以在选择入库单后修改金额。"></asp:Label>
                        </div>   
                        <div style="height:10px"></div>                                            
                            <asp:Button ID="btn_save"  CssClass="baseButton" runat="server" Text="保 存"  OnClick="btn_save_Click"/>
                          <input type="button" onclick="javascript:window.close();" value="关 闭" class="baseButton" />&nbsp;
                        </div>
                    </div>
    </form>
</body>
</html>
