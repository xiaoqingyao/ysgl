<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dbSelectYskm.aspx.cs" Inherits="webBill_select_dbSelectYskm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>科目单选</title>
    <base target="_self" />
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script type="text/javascript">
     $(function(){
            $("#tabData tr td:eq(0)").width(50);
            $("#tabData tr td:eq(1)").width(250);
            $("#btn_refresh").click(function(){
            location.reload();
            });
                        
             $("#btn_cancle").click(function(){
               window.self.close();
            });
            
            $("#btn_reset").click(function(){
            var chk = document.getElementById("tabData").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = false;
            }
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
        
        
       function SingleSelect(aControl) {
             var type='<%=Request["type"] %>';
             if(type=="m")
             return false;
            var chk = document.getElementById("tabData").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = false;
            }
            
               aControl.checked =true;
        }
       
       
        $(function() {
            $("#txt_findYskm").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                    var chk = $("#tabData input[type='checkbox']:checked");
                    var type='<%=Request["type"] %>';
                    if(type=="s")
                    {
                    for (var s = 0; s < chk.length; s++) {
                        chk[s].checked = false;
                    }
                    }

                    var lbs = $("#tabData label");
                    for (var s = 0; s < lbs.length; s++) {
                        if (lbs[s].innerText == rybh) {
                            $(lbs[s]).parent().prev().find("input[type='checkbox']").attr("checked", "true").focus().select();
                           break;

                        }
                    }
                }
            });
            
            
            $("#btn_select").click(function(){
             var chk = $("#tabData input[type='checkbox']:checked");
             var type='<%=Request["type"] %>';
             if(chk.length==0)
             {
             alert("请选择预算科目");
             return false;
             }  
             else if(chk.length>1&&type=="s")
             {
             alert("最多选择一个预算科目");
             return false;
             }
             else
             {
            return true;
             }
            });
        });
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="optionDiv" style="margin: 5px">
      <input id="btn_reset" type="button" value="重 选" class="baseButton" />
        快速检索<asp:TextBox ID="txt_findYskm" runat="server"></asp:TextBox>
        <asp:Button ID="btn_select" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_select_Click" />
        <input id="btn_cancle" type="button" value="取 消" class="baseButton" />
    </div>
    <div id="dataDiv" style="margin: 5px">
        <div style="position: relative; word-warp: break-word; word-break: break-all;">
            <table class="baseTable" id="tabData" style="margin-left: 0px">
                <tr>
                    <th style="width: 50px">
                        选择
                    </th>
                    <th style="width: 250px">
                        部门
                    </th>
                </tr>
                <asp:Repeater ID="rptYsgc" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ckYskm" runat="server" onclick="javascript:SingleSelect(this);" />
                            </td>
                            <td>
                                <label style="display: none;">
                                    <%#Eval("yskmMc")%></label>
                                <asp:Literal ID="ltlYskm" runat="server" Text='<%#Eval("yskmMc")%>'></asp:Literal>
                                <asp:HiddenField ID="hfyskm" runat="server" Value='<%#Eval("yskmCode") %>' />
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
