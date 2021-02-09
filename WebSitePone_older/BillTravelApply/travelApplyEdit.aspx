<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travelApplyEdit.aspx.cs"
    Inherits="BillTravelApply_travelApplyEdit" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>出差申请单编辑</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="../js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <link href="../Css/CommenCss.css" rel="stylesheet" type="text/css" />
    <style>
        .div-hs {
            border-top: 1px solid gray;
            border-bottom: 1px solid gray;
            text-align: left;
        }

        .tab-hs {
        }
    </style>



</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false">
        <div>
            <div data-role="header">
                <a data-icon="home" data-ajax="false" onclick="ConfirmReturn('../Index.aspx','单据未保存，确定要返回吗')">返回</a>
                <h1>出差申请单编辑</h1>
                <a href="travelApplyList.aspx" data-role="button" class="ui-btn-right" data-icon="grid"
                    data-ajax="false">单据列表</a>
            </div>
            <div data-role="content">
                <table>
                    <tr>
                        <td class="tdEnenRight" style="width: 90px">申请单号:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_bh" runat="server" ReadOnly="true" BackColor="#FFD9D9D9"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdEnenRight">派遣部门:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_bm" runat="server"></asp:TextBox>
                            <ul id="ul_bm" data-role="listview" data-inset="true" data-theme="c" style="position: absolute; z-index: 99999">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdEnenRight">制单日期:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_billDate" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdEnenRight">申请人:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_billUser" runat="server"></asp:TextBox>
                            <ul id="ul_billUser" data-role="listview" data-inset="true" data-theme="c" style="position: absolute; z-index: 99999"
                                onclick="userChange()">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdEnenRight">所属部门:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_userDept" runat="server" ReadOnly="true" BackColor="#FFD9D9D9"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdEnenRight">预计时间:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_travelDate" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdEnenRight">地点:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_address" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdEnenRight">出差事由:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_reasion" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdEnenRight">日程安排:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_plan" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class='tdEnenRight'>申请交通工具:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_jtgj" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class='tdEnenRight'>是否超标准: 
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlIsbz" runat="server" data-role="slider">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Selected="true" Value="0">否</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="div-hs">
                                <h4>出差人</h4>
                                <asp:HiddenField ID="hfpersons" runat="server" />
                                <table class="tab-hs" id="tab_person" runat="server">
                                    <tr>
                                        <td class="tdborder" colspan="2">
                                            <input type='text' value="" data-role="none" class="myInput" id="txt_user" />
                                            <ul id="ul_user" data-role="listview" data-inset="true" data-theme="c" style="position: absolute; z-index: 99999">
                                        </td>
                                        <td class="tdborder">
                                            <input type="button" data-iconpos="notext" data-icon='plus' onclick='AddRow(this);'>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <div class="div-hs">
                                <h4>预算费用明细</h4>
                                <table class='tab-hs ItemTable' id="tabmx">
                                    <tr>
                                        <td class="tdEnenRight">交通费:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_jtf" runat="server" onkeyup='replaceNaNNum(this)' onchange="SumJe()"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdEnenRight">住宿费:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_zsf" runat="server" onkeyup='replaceNaNNum(this)' onchange="SumJe()"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdEnenRight">业务招待费:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_zdf" runat="server" onkeyup='replaceNaNNum(this)' onchange="SumJe()"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdEnenRight">会议费:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_hyf" runat="server" onkeyup='replaceNaNNum(this)' onchange="SumJe()"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdEnenRight">印刷费:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ysf" runat="server" onkeyup='replaceNaNNum(this)' onchange="SumJe()"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdEnenRight">其他等:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_qt" runat="server" onkeyup='replaceNaNNum(this)' onchange="SumJe()"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdEnenRight">预算总金额:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_zje" runat="server" ReadOnly="true" BackColor="#FFD9D9D9"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button ID="btnSave" runat="server" Text="保存" data-inline="true" OnClick="btnSave_Click"
                                OnClientClick="return Check()" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hffj" runat="server" />
                <asp:HiddenField ID="hftzr" runat="server" />
                <asp:HiddenField ID="hfuser" runat="server" />
            </div>
            <div data-role="footer" data-position="fixed">
                <footer data-role="footer" id="footer">
                    <h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1>
                </footer>
            </div>
        </div>
    </form>

    <script>
        $("#form1").on("pageshow", function (e) {
            var bmObj = $("#txt_bm");
            var bmulObj = $("#ul_bm");
            AutoComplicated(bmObj, bmulObj, "hsbm", "");


            var userObj = $("#txt_user");
            var userulObj = $("#ul_user");
            AutoComplicated(userObj, userulObj, "user", "");

            var billUserObj = $("#txt_billUser");
            var billUserulObj = $("#ul_billUser");
            AutoComplicated(billUserObj, billUserulObj, "user", "");
        });
    </script>

    <script src="../js/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#txt_content").css({ "minHeight": "200px" });
            $("#txt_sm").css({ "minHeight": "100px" });

            var pp = $("#hfpersons").val();
            if (pp != null && pp != "" && pp.length > 0) {
                $("#tab_person").prepend(pp);
                $("#tab_person tr").not(":last").find("td:eq(2)").trigger('create');
                $("#tab_person tr").not(":last").find("input[type='button']").parent().css({ "margin": 0, "padding": 0 });
            }

            userChange();
        });


        function userChange() {
            var v1 = $("#txt_billUser").val();
            if (v1.indexOf("[") != -1) {
                $.get("../Ajax/AutoComplicated.ashx", { type: "userDept", search: v1, otherParm: "" }, function (data, status) {
                    if (status == "success") {
                        if (data == "-1")
                            alert("请输入有效申请人")
                        else
                            $("#txt_userDept").val(data);
                    }
                });
            }
        }
        function Check() {
            var bh = $("#txt_bh").val().length;
            var persons = $("#tab_person tr").not(":last").size();
            if (bh == 0) {
                alert("单号不能为空！");
                return false;
            }
            else if (persons == 0) {
                alert("请填写出差人！");
                return false;
            }
            else {
                var ps = "";
                $("#tab_person tr").not(":last").each(function () {
                    var i = $(this).find("td:eq(0)").text();
                    ps += i + ",";
                });
                if (ps.length > 1)
                    ps = ps.substring(0, ps.length - 1);
                $("#hfuser").val(ps);
                return true;
            }
        }

        function SumJe() {
            var sum = 0;
            $("#tabmx tr").each(function () {
                var item = $(this).find("input").eq(0).val();
                item = ConvertNum(item);
                sum = parseFloat(sum) + parseFloat(item);
            });
            $("#txt_zje").val(sum);
        }


        function AddRow(obj) {
            var tr = $(obj).parentsUntil("tr").parent().eq(0);
            var v = tr.find("input");
            v1 = v.eq(0).val();
            if (v1.length == 0) {
                return;
            }
            $.get("../Ajax/AutoComplicated.ashx", { type: "userDept", search: v1, otherParm: "" }, function (data, status) {
                if (status == "success") {
                    if (data == "-1")
                        alert("请输入有效出差人")
                    else {
                        var v2 = data;
                        tr.before("<tr><td  class='mytdEnenRight tdborder'>" + v1 + "</td><td class='mytdEnenRight tdborder'>" + v2 + "</td><td  class='tdborder'><input type='button' data-iconpos='notext' data-icon='delete' onclick='RemoveRow(this);'/></td></tr>");
                        tr.prev().find("td:eq(2)").trigger('create');
                        tr.find("input[type='text']").val("");
                        tr.prev().find("input[type='button']").eq(0).parent().css({ "margin": 0, "padding": 0 });
                    }
                }
            });
        }

        function RemoveRow(obj) {
            var tr = $(obj).parentsUntil("tr").parent().eq(0);
            tr.remove();
        }
    </script>
</body>
</html>
