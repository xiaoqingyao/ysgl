﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FundBorrowHK.aspx.cs" Inherits="SaleBill_BorrowMoney_FundBorrowHK" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>借款单冲减</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js"
        type="text/javascript" charset="UTF-8"></script>

    <script type="text/javascript">
    $(function(){
    var tr=$("#main tr:gt(0)");
      tr.find("td").addClass("myGridItem");
      tr.find("td:even").css({"text-align":"right","width":"60"});
      tr.find("td:odd").css({"text-align":"right","width":"120"});
      
    });
   //替换非数字
  function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }
        //非空验证
   function checkisNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("不能为空！");
                obj.focus();
                return false;
            };
        }
        
        
    function Check()
    {
     var  je=$("#hfje").val(); 
     var  ycj=$("#hfycj").val(); 
     var  hkje=$("#txt_hkje").val(); 
     if(parseFloat(hkje)==0)
     {
     alert("还款金额不能为0");
     return false;
     }
    var zj=parseFloat(hkje)+parseFloat(ycj)-parseFloat(je);
     if(parseFloat(zj)>0)
         return  confirm("还款金额超出"+zj+",您确定要还款吗");
    else
        return true;
    }
    </script>
    <style type="text/css">
     .title
        {
            font-size: 18px;
            font-family: 微软雅黑;
            font-weight: 500;
            text-align: center;
            border: none;   
            width:100%;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id='main' width="100%" class="baseTable">
            <tr>
                <td  class="title" colspan="6">
                    还款  <asp:Label ID="lbjkcode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    借款人：
                </td>
                <td>
                    <asp:Label ID="txtloanName" runat="server"></asp:Label>
                </td>
                <td>
                    借款部门：
                </td>
                <td>
                    <asp:Label ID="txtdeptname" runat="server"></asp:Label>
                </td>
                <td>
                    借款类别：
                </td>
                <td>
                    <asp:Label ID="txtlb" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    借款天数：
                </td>
                <td>
                    <asp:Label ID="txtjkts" runat="server"></asp:Label>
                    (天)
                </td>
                <td>
                    借款日期：
                </td>
                <td>
                    <asp:Label ID="txtjksj" runat="server"></asp:Label>
                </td>
                <td>
                    填报时间：
                </td>
                <td>
                    <asp:Label ID="txtaddtime" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    借款金额：
                </td>
                <td>
                    <asp:Label ID="txtmoney" runat="server"></asp:Label>
                </td>
                <td>
                    已冲减金额：
                </td>
                <td>
                    <asp:Label ID="lbycj" runat="server"></asp:Label>
                </td>
                <td>
                    还款金额：
                </td>
                <td>
                    <asp:TextBox ID="txt_hkje" runat="server" onblur="checkisNaN(this);" onkeyup="replaceNaN(this);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>还款原因：</td>
                <td colspan="5">
                    <input type="text" runat="server" id="txtReasion" style="width:95%" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hfje" runat="server" Value="0" />
         <asp:HiddenField ID="hfycj" runat="server" Value="0" />
        <div runat="server" id="trgride" style="text-align: center">
            <div style="overflow: auto; width: 100%; max-height: 250px">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="baseTable"
                    ShowHeader="true" Width="100%" EmptyDataText="没有冲减/还款记录" onrowdatabound="GridView1_RowDataBound">
                    <RowStyle CssClass="myGridItem" />
                    <HeaderStyle CssClass="myGridHeader" />
                    <Columns>
                        <asp:BoundField DataField="loancode" HeaderText="单号"  ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="je" HeaderText="金额" ItemStyle-HorizontalAlign="Center" /> 
                        <asp:BoundField DataField="ltype" HeaderText="还款类型" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ldate" HeaderText="还款日期"  />
                        <asp:BoundField DataField="billCode" HeaderText="报销单号" ItemStyle-HorizontalAlign="Center" />
                         <asp:BoundField DataField="note1" HeaderText="审批状态" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div runat="server" id="trcjcz" style="text-align: center">
            <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click"  OnClientClick="return Check()"/>&nbsp;
            <asp:Button ID="btn_fh" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close();" />
        </div>
    </div>
    </form>
</body>
</html>
