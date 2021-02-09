<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dbSelectYsgcs.aspx.cs" Inherits="webBill_select_dbSelectYsgcs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算过程(多选)</title>
    <base  target="_self"/>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
     $(function(){
            $("#tabData tr td:eq(0)").width(50);
            $("#tabData tr td:eq(1)").width(250);
            
             $("#btn_select").click(function(){
                var chk = $("#tabData input[type='checkbox']:checked");
                if (chk.length == 0) {
                    alert("请选择预算过程");
                    return false;
                }
                else {
                    return true;
                }
            });
            
             $("#btn_cancle").click(function(){
               window.self.close();
            });
            
            
            gudingbiaotounew($("#tabData"), 380);
        });
    
     
      function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
        
        
        function SelectAll(aControl) {
            var chk = document.getElementById("tabData").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
            }
        }
       
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="optionDiv" style="margin:5px">
        <asp:DropDownList ID="ddlNd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNd_SelectedIndexChanged">
        </asp:DropDownList>
        <asp:Button ID="btn_select" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_select_Click" />
        <input id="btn_cancle" type="button" value="取 消" class="baseButton" />
    </div>
    <div id="dataDiv" style="margin: 5px">
        <div style="position: relative; word-warp: break-word; word-break: break-all;">
            <table  class="baseTable" id="tabData" style="margin-left:0px" >
                <tr>
                    <th style="width: 50px">
                        <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                            Text="全选" />
                    </th>
                    <th style="width: 250px">
                        预算过程
                    </th>
                </tr>
                <asp:Repeater ID="rptYsgc" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ckYsgc" runat="server" />
                            </td>
                            <td>
                            <asp:Literal ID="ltlYsgc" runat="server" Text='<%#Eval("xmmc")%>'></asp:Literal>
                                <asp:HiddenField ID="hfYsgc" runat="server" Value='<%#Eval("gcbh") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
