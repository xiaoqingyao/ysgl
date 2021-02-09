<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ybbxList.aspx.cs" Inherits="bxd_ybbxList"
    EnableViewState="true" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>一般报销列表页</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="../js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <link href="../Css/CommenCss.css" rel="stylesheet" type="text/css" />

    <script src="../js/Common.js" type="text/javascript"></script>

    <style>
        .div-yskm {
            margin-top: 10px;
            border-bottom: 2px solid red;
        }

        .tab-yskm {
            color: Blue;
            font-size: 16px;
        }

        .div-hs {
            margin-top: 10px;
            border-bottom: 1px solid gray;
            text-align: left;
        }

        .tab-hs {
        }
    </style>

    <script>

        $(function () {
            $(".title-r").click(function (event) {
                event.stopPropagation();
            });

            $(".c-option").find("input[type='text']").css({ "width": $(".c-option").width() - 100 });
        });

        //查看
        function View(flowId, code) {

            $("#hfFlowId").val(flowId);
            $("#hfBillCode").val(code);

            //window.location.href="../bxd/ybbxView.aspx";
            $.post("../workflow/GetBillType.ashx", { "billcode": code, "isdz": "1" }, function (data, status) {
                if (status == "success") {
                    $("#open").attr("href", "ybbxView.aspx?billCode=" + code + "&type=View" + "&flowid=" + flowId + "&isdz=1");
                    $("#open").click();
                    //获取单据审批流详细信息
                    $.post("../workflow/GetBillStatus.ashx", { "billCode": code, "isdz": "1" }, function (data, status) {
                        if (status == "success") {
                            $("#wf").html("<div style='border-top:1px solid gray;'>审批流程：" + data + "</div>");
                        }
                    });
                }
            });
        }

        function test() {
            $('.ui-dialog').dialog('close');
            return;
        }


        function Submit(flowid, code, dept, obj) {
            var isdz = '<%=Request["isdz"]%>';
            //if (isdz == "1") {
            //    flowid = "gkbx";
            //}

            $.post("../workflow/WorkFlowSummit.ashx", { "billcode": code, "flowid": flowid, "isdz": isdz }, function (data, status) {
                if (data == "-1" || data == "-2") {
                    alert("提交失败！");
                }
                else if (data == "-3") {
                    alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");;
                }
                else {
                    if (status == "success") {
                       // alert("提交成功！");
                        $(obj).hide();
                        $(obj).siblings().hide();
                        window.location.href = window.location.href;
                        $.post("../Ajax/GetCurWorkState.ashx", { "code": code }, function (data, status) {
                            if (status == "success")
                                $(obj).parent().parent().prev("div").find("lable").text("审核状态：" + data);
                        });
                    }
                }
            });
        }
        //详细页提交
        function SubmitChildren() {
            CloseChidren();
            var flowid = $("#hfFlowId").val();
            var code = $("#hfBillCode").val();
            var dept = $("#hfdept").val();
            var obj = $("a[title='" + code + "']").eq(1);
            var isdz = '<%=Request["isdz"]%>';


            $.post("../workflow/WorkFlowSummit.ashx", { "billcode": code, "flowid": flowid, "isdz": isdz }, function (data, status) {
                if (data == "-1" || data == "-2") {
                    alert("提交失败！");
                }
                else if (data == "-3") {
                    alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                }
                else {
                    if (status == "success") {
                       // alert("提交成功！");
                        $(obj).hide();
                        $(obj).siblings().hide();
                        window.location.href = window.location.href;
                        $.post("../Ajax/GetCurWorkState.ashx", { "code": code }, function (data, status) {
                            if (status == "success")
                                $(obj).parent().parent().prev("div").find("lable").text("审核状态：" + data);
                        });
                    }
                }
            });
        }

        function AddMx(flowid, code) {


            window.location.href = "ybbxEditItem.aspx?billCode=" + code + "&type=addmx";
        }
        //详细撤销审核
        function RevorkChildren() {
            CloseChidren();
            var flowid = $("#hfFlowId").val();
            var code = $("#hfBillCode").val();
            var obj = $("a[title='" + code + "']").eq(1);
            $.post("../workflow/WorkFlowReplace.ashx", { "billcode": code, "flowid": "gkbx" }, function (data, status) {
                if (status == "success") {
                    if (data == "1") {
                       // alert("撤销成功！");
                        $(obj).parent().hide();
                        $(obj).parent().prev("div").find("lable").text("审核状态：未提交");
                        $(obj).parent().parent().parent().next("div").show("slow");
                        window.location.href = window.location.href;
                    }
                    else
                        alert("单据以进入审批，不能撤销");
                }
                else
                    alert("失败");
            });
        }

        function RevokeCheck(obj, flowid, code) {
            $.post("../workflow/WorkFlowReplace.ashx", { "billcode": code, "flowid": "gkbx" }, function (data, status) {
                if (status == "success") {
                    if (data == "1") {
                      //  alert("撤销成功！");
                        $(obj).parent().hide();
                        $(obj).parent().prev("div").find("lable").text("审核状态：未提交");
                        $(obj).parent().parent().parent().next("div").show("slow");
                    }
                    else
                        alert("单据以进入审批，不能撤销");
                }
                else
                    alert("失败");
            });
        }

        //详细页删除
        function DeleteChildren() {
            if (confirm("您确定要删除吗")) {
                CloseChidren();
                var flowid = $("#hfFlowId").val();
                var code = $("#hfBillCode").val();

                var obj = $("a[title='" + code + "']").eq(1);
                var isdz = '<%=Request["isdz"]%>';
                if (isdz == "1") {
                    flowid = "gkbx";

                }

                $.post("../workflow/DeleteBill.ashx", { "billCode": code, "type": flowid }, function (data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            //alert("删除成功!");
                            $(obj).parent().parent().parent().parent().hide("slow");
                        }
                        else {
                            alert("删除失败1!");
                        }
                    }
                    else {
                        alert("删除失败2!");
                    }
                });
            }
        }
        function Delete(flowId, code, obj) {
            if (confirm("您确定要删除吗?")) {
                var isdz = '<%=Request["isdz"]%>';
                if (isdz == "1") {
                    flowId = "gkbx";
                }
              
                $.post("../workflow/DeleteBill.ashx", { "billCode": code, "type": flowId }, function (data, status) {
                    if (status == "success") {
                        if (data == "1") {
                           // alert("删除成功!");
                            $(obj).parent().parent().parent().parent().hide("slow");
                            window.location.href = window.location.href;
                        }
                        else {
                            alert("删除失败1!");
                        }
                    }
                    else {
                        alert("删除失败2!");
                    }
                });
            }

        }

    </script>

</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false">
        <div>
            <div data-role="header">
                <a href="../Index.aspx" data-icon="home" data-ajax="false">主页</a><%--data-rel="back"--%>
                <h1>我的报销单</h1>
                <div style="display: none">
                    <a href="ybbxEditMain.aspx" data-role="button" class="ui-btn-right" data-icon="plus"
                        data-ajax="false">制单</a>
                </div>

            </div>
            <div data-role="content">
                <a id="popup" data-role="button" href="#popdiv" data-rel="popup" data-position-to="origin"
                    style="display: none;">弹出层</a> <a href="ybbxView.aspx" id="open" data-role="button"
                        data-inline="true" data-rel="dialog" data-transition="pop" style="display: none;">Open dialog</a>
                <table>
                    <tr>
                        <td style="width: 50px;">时间：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTime" runat="server" AutoPostBack="True" EnableViewState="true"
                                OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                                <asp:ListItem Value="1">当月</asp:ListItem>
                                <asp:ListItem Value="2">上月</asp:ListItem>
                                <asp:ListItem Value="3">近三个月</asp:ListItem>
                                <asp:ListItem Value="4">全部</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                <ItemTemplate>
                    <div data-role="collapsible" data-collapsed="false" data-theme="b" class="c-main">
                        <h1>
                            <div class="title">
                                <div class="title-l">
                                    <asp:Literal ID="Literal1" runat="server" Text='<%#Eval("billName") %>'></asp:Literal>
                                </div>
                                <div class="title-r">
                                    <a data-role="button" data-inline="true" onclick="View('<%#Eval("flowID") %>','<%#Eval("billname") %>')"
                                        data-ajax="false">
                                        <img src="../images/metro/View-Small-Icons.png" />详细信息</a>
                                </div>
                            </div>
                        </h1>
                        <div class="c-row">
                            <div class="c-row-item">
                                <div>
                                    报销人：
                                </div>
                                <%#Eval("billUser") %>
                            </div>
                            <div class="c-row-item">
                                <div>
                                    制单时间：
                                </div>
                                <%#Eval("billDate")%>
                            </div>
                            <div class="c-row-item">
                                <div>
                                    部门：
                                </div>
                                <%#Eval("billDept") %>
                            </div>
                            <div class="c-row-item">
                                <div>
                                    单据金额：
                                </div>
                                <%#Eval("billJe","{0:N2}") %>￥
                            </div>
                            <div class="c-row-item">
                                <div>
                                    报销摘要：
                                </div>
                                <%#Eval("bxzy") %>
                            </div>
                        </div>
                        <div class="c_row_item">
                            <asp:HiddenField ID="hfCode" runat="server" Value='<%#Eval("billname") %>' />
                            <asp:HiddenField ID="hfname" runat="server" Value='<%#Eval("billname") %>' />
                            <asp:HiddenField ID="hiddatetime" runat="server" Value='<%#Eval("billDate") %>' />
                            <asp:HiddenField ID="hidbillDept" runat="server" Value='<%#Eval("billDept") %>' />
                            <asp:Label ID="lbmx" runat="server"></asp:Label>
                        </div>
                        <div class="c-option" id="optionDiv" runat="server" data-role="controlgroup" data-type="horizontal"
                            data-inline="true">
                            <a data-role="button" data-inline="true" title='<%#Eval("billname") %>' onclick="Submit('<%#Eval("flowID") %>','<%#Eval("billname") %>',this)"
                                data-theme="d">
                                <img src='../images/metro/Road-Right.png' />提 交
                            </a>
                            <a>

                                <%--  <a data-role="button" data-inline="true"
                                    title='<%#Eval("billname") %>' onclick="AddMx('<%#Eval("flowID") %>','<%#Eval("billname") %>')"
                                    data-theme="d">
                                    <img src="../images/metro/Add-New.png" />明 细
                            </a>--%>
                                <a data-role='button' data-inline='true'
                                    data-theme='d' onclick="Delete('<%#Eval("flowID") %>','<%#Eval("billname") %>',this)">
                                    <img src='../images/metro/Delete.png' />删 除
                                </a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <div id="lbPageNav" runat="server">
            </div>
            <input id="hfBillCode" type="hidden" />
            <input id="hfFlowId" type="hidden" />
            <input id="hfdept" type="hidden" />
        </div>
        <div data-role="footer" data-position="fixed">
            <footer data-role="footer" id="footer">
                <h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1>
            </footer>
        </div>
        <div id="popdiv" class="display" data-role="popup" data-overlay-theme="a" data-theme="c"
            class="ui-content">
            操作成功！
        </div>
        <div id="divMenu">
            <a href="#">
                <img src="../images/toTop.jpg" border="0" /></a>
        </div>

        <script src="../js/backtop.js" type="text/javascript"></script>
    </form>
</body>
</html>
