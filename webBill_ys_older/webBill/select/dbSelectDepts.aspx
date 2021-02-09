<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dbSelectDepts.aspx.cs" Inherits="webBill_select_dbSelectDepts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>部门多选</title>
    <base target="_self" />
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#tabData tr td:eq(0)").width(50);
            $("#tabData tr td:eq(1)").width(250);

            var type = '<%=Request["type"] %>';

            $("#btn_select").click(function () {
                var chk = $("#tabData input[type='checkbox']:checked");
                if (chk.length == 0) {
                    alert("请选择部门");
                    return false;
                }
                else {
                    return true;
                }
            });

            $("#btn_cancle").click(function () {
                window.self.close();


            });

            if (type == "s") {
                var td = $("#tabData tr:eq(0)").find("th").first();
                td.html("");
                td.html("选择");
            }

           // gudingbiaotounew($("#tabData"), 380);
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



        function SingleSelect(aControl) {
            var type = '<%=Request["type"] %>';
            if (type == "m")
                return false;
            var chk = document.getElementById("tabData").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = false;
            }

            aControl.checked = true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="optionDiv" style="margin: 5px">
            <asp:TextBox ID="txt_search" runat="server"></asp:TextBox>
            <asp:Button ID="btn_search" runat="server" Text="查询" CssClass="baseButton" OnClick="btn_searc_Click" />
            <asp:Button ID="btn_select" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_select_Click" />
            <input id="btn_cancle" type="button" value="取 消" class="baseButton" />
        </div>
        <%--<div id="dataDiv" style="margin: 5px">
            <div style="position: relative; word-warp: break-word; word-break: break-all;">--%>
        <div>
                <table class="baseTable" id="tabData" style="margin-left: 0px">
                    <tr>
                        <th style="width: 50px">
                            <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                Text="全选" />
                        </th>
                        <th style="width: 250px">部门
                        </th>
                    </tr>
                    <asp:Repeater ID="rptYsgc" runat="server" OnItemDataBound="rptYsgc_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckDept" runat="server" onclick="javascript:SingleSelect(this);" />
                                </td>
                                <td>
                                    <asp:Label ID="kongge" runat="server" Text=""></asp:Label>
                                    <asp:Literal ID="ltlDept" runat="server" Text='<%#Eval("deptName")%>'></asp:Literal>
                                    <asp:HiddenField ID="hfDept" runat="server" Value='<%#Eval("deptCode") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
          
        </div>
    </form>
</body>
</html>
