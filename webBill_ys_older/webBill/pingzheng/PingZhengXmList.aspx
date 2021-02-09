<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PingZhengXmList.aspx.cs"
    Inherits="webBill_pingzheng_PingZhengXmList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title> 
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
        $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                   parent.helptoggle();
                }
            });
            $("#<%=tlm.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=tlm.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                try {
                    var billCode = $(this).find("td")[2].innerHTML;
                    $("#hdDelCode").val(billCode);
                     $("#hf_xmcode").val($(this).find("td")[7].innerHTML);
                } catch (e) { }
            });
            //刷新
            $("#btn_refresh").click(function() {
                location.replace(location.href);
            });
            //添加
            $("#btn_add").click(function() {
            
               var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    openDetail("PingZhengXmDetail.aspx?Ctrl=Add");
                }
               else
               {
                var code = checkrow.find('td')[6].innerHTML;
                 openDetail("PingZhengXmDetail.aspx?Ctrl=Add&Code="+code);
               }
                
                $("#btn_refresh").click();
            });
            //删除
            $("#btn_delete").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return false;
                }
                if (!window.confirm("如果该类型下有子节点将导致删除不成功，确认要继续吗？")) { return false; }
                var code = checkrow.find('td')[6].innerHTML;
                $("#hdDelCode").val(code);
            });
            //修改
            $("#btn_update").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return false;
                }
                var code = checkrow.find('td')[6].innerHTML;
                openDetail("PingZhengXmDetail.aspx?Ctrl=Edit&Code=" + code);
                $("#btn_refresh").click();
            });
        });
        //打开明细页 
        function openDetail(url) {
            var newUrl = url + "&" + Math.random();
            var returnValue = window.showModalDialog(newUrl, 'newwindow', 'center:yes;dialogHeight:350px;dialogWidth:550px;status:no;scroll:yes');
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
                       <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TreeListView runat="server" ID="tlm" NodeColumnIndex="1" ParentKey="parentCode"
                        ColumnKey="xmCode" CssClass="myGrid" Width="100%" AutoGenerateColumns="false"
                        ExpendDepth="5" EmptyDataText="暂无数据">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <Columns>
                            <asp:LineNumberField HeaderText="No." ItemStyle-Width="20" HeaderStyle-Width="20">
                            </asp:LineNumberField>
                        
                            <asp:BoundField DataField="xmCode" HeaderText="类型编码" />
                            <asp:BoundField DataField="xmName" HeaderText="类型名称" />
                            <asp:BoundField DataField="parentCode" HeaderText="上级编号" />
                             <asp:BoundField DataField="parentName" HeaderText="上级名称" />
                            <asp:BoundField DataField="isDefaultShow" HeaderText="是否默认" />
                                <asp:BoundField  DataField="list_id" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill"/>
                                 <asp:BoundField  DataField="xmCode" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill"/>
                        </Columns>
                    </asp:TreeListView>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdDelCode" runat="server" />
     <asp:HiddenField ID="hf_xmcode" runat="server" />
    </form>
</body>
</html>
