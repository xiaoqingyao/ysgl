<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShiYongZhuangKuangList.aspx.cs" Inherits="ZiChan_ZiChanGuanLi_ShiYongZhuangKuangList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>使用状况列表页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
 <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
   
        $(function() {
      
           $("#<%=tlm.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=tlm.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                var billCode = $(this).find("td")[2].innerHTML;
                $("#hdDelCode").val(billCode);
               
               });
            //刷新
            $("#btn_refresh").click(function() {
                location.replace(location.href);
            });
            //添加
            $("#btn_add").click(function() {
                var checkRow = $(".highlight");
                if(checkRow.length==0)
                {
                     openDetail("syzkDetail.aspx?Ctrl=Add");
                }
               else{
                    var code = checkRow.find('td')[2].innerHTML;
                    openDetail("syzkDetail.aspx?Ctrl=Add&Code=" + code);
                } 
            });
            //删除
            $("#btn_delete").click(function() {
             var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return false;
                }
               
                if (!window.confirm("如果该类型下有子节点将导致删除不成功，确认要继续吗？")) { return false; }
                
                var code = checkrow.find('td')[2].innerHTML;
                $("#hdDelCode").val(code);
            });
            //修改
            $("#btn_update").click(function() {
             var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return false;
                }
                var code = checkrow.find('td')[2].innerHTML;
                openDetail("syzkDetail.aspx?Ctrl=Edit&Code=" + code);
            });

        });
        //打开明细页 
        function openDetail(url) {
            var newUrl = url+ "&" + Math.random();
            var returnValue = window.showModalDialog(newUrl, 'newwindow', 'center:yes;dialogHeight:350px;dialogWidth:850px;status:no;scroll:yes');
            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("btn_refresh").click();
            }
        }
       
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="98%">
            <tr>
                <td>
                    <input type="button" value="刷 新" id="btn_refresh" class="baseButton" />
                    <input type="button" value="添 加" id="btn_add" class="baseButton" />
                    <input type="button" value="修 改" id="btn_update" class="baseButton" />
                    <asp:Button ID="btn_delete" runat="server" Text="删 除" CssClass="baseButton" OnClick="btn_delete_Click" />
                    <asp:Button ID="Button1" runat="server" Text="导出EXCEL" CssClass="baseButton" Visible="false"
            onclick="Button1_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TreeListView runat="server" ID="tlm" NodeColumnIndex="1" ParentKey="ParentCode"
                        ColumnKey="ZhuangKuangCode" CssClass="myGrid" Width="100%" AutoGenerateColumns="false" runat="server"  
                        ExpendDepth="5">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <Columns>
                            <asp:LineNumberField HeaderText="No." ItemStyle-Width="20" HeaderStyle-Width="20">
                            </asp:LineNumberField>
                            <asp:BoundField DataField="ZhuangKuangName" HeaderText="类型名称" />
                            <asp:BoundField DataField="ZhuangKuangCode" HeaderText="类型编码" />
                            <asp:BoundField DataField="ParentCode" HeaderText="上级编号" />
                           
                                <asp:BoundField DataField="BeiZhu" HeaderText="备注" ItemStyle-Width="200"
                                HeaderStyle-Width="200" />
                          
                        </Columns>
                    </asp:TreeListView>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdDelCode" runat="server" />
    </form>
</body>
</html>
