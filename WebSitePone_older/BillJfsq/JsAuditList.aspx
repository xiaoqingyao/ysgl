<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JsAuditList.aspx.cs" Inherits="BillYs_JsAuditList" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>预算单审核</title>
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
            $.post("../workflow/GetBillType.ashx", { "billcode": code }, function (data, status) {
                if (status == "success") {
                    // alert( data + "&checking=true&flowid="+flowId);
                    $("#open").attr("href", data + "&checking=true&flowid=" + flowId);
                    $("#open").click();
                    //获取单据审批流详细信息
                    $.post("../workflow/GetBillStatus.ashx", { "billCode": code }, function (data, status) {
                        if (status == "success") {
                            $("#wf").html("<div style='border-top:1px solid gray;'>审批流程：" + data + "</div>");
                        }
                    });
                }
            });
        }
        //审核通过
        function Audit(flowId, code, obj) {
            if (confirm("是否确定审核")) {
                var mind = $(obj).siblings("span").find("input").val();
                // alert(mind);
                $.post("../workflow/WorkFlowApprove.ashx", { "billcode": code, "mind": mind, "action": "approve" }, function (data, status) {
                    if (data < 0 && status != "success") {
                        alert("审核失败！");
                    }
                    else {
                        // alert("审核成功")
                        $(obj).parent().parent().parent().hide("slow");
                    }
                });
            }

        }


        //驳回
        function Cancel(flowId, code, obj) {
            if (confirm("是否确定驳回？")) {
                var mind = $(obj).siblings("span").find("input").val();
                $.post("../workflow/WorkFlowApprove.ashx", { "billcode": code, "mind": mind, "action": "disagree" }, function (data, status) {
                    if (data < 0 && status != "success") {
                        alert("驳回失败！");
                    }
                    else {
                        //alert("驳回成功");
                        $(obj).parent().parent().parent().hide("slow");
                    }
                });
            }

        }

        function test() {
            $('.ui-dialog').dialog('close');
            return;
        }


        function AuditChildren() {
            if (confirm("是否确定审核")) {
                var mind = $("#txt_mymind").val();
                test();
                var flowid = $("#hfFlowId").val();
                var code = $("#hfBillCode").val();
                var obj = $("a[title='" + code + "']").eq(0);

                $.post("../workflow/WorkFlowApprove.ashx", { "billcode": code, "mind": mind, "action": "approve" }, function (data, status) {
                    if (data < 0 && status != "success") {
                        alert("审核失败！");
                    }
                    else {
                        // alert("审核成功");
                        $(obj).parent().parent().parent().hide("slow");
                    }
                });
            }

        }

        function CancelChildren() {
            if (confirm("是否确定驳回？")) {
                var mind = $("#txt_mymind").val();
                test();
                var flowid = $("#hfFlowId").val();
                var code = $("#hfBillCode").val();
                var obj = $("a[title='" + code + "']").eq(1);
                $.post("../workflow/WorkFlowApprove.ashx", { "billcode": code, "mind": mind, "action": "disagree" }, function (data, status) {
                    if (data < 0 && status != "success") {
                        alert("驳回失败！");
                    }
                    else {
                        $(obj).parent().parent().parent().hide();
                        // alert("驳回成功");
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
                <a href="../workflow/Audit.aspx" data-icon="back" data-ajax="false">返回</a>
                <h1>
                    <asp:Literal ID="ltrTitle" runat="server" Text="预算审批"></asp:Literal>
                </h1>
            </div>
            <div data-role="content">
                <a id="popup" data-role="button" href="#popdiv" data-rel="popup" data-position-to="origin"
                    style="display: none;">弹出层</a>
                <a href="" data-role="button" data-rel="dialog" data-transition="pop"
                    id="open" style="display: none;">弹出框 </a>
                <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                    <ItemTemplate>
                        <div data-role="collapsible" data-collapsed="false" data-theme="b" class="c-main">
                            <h1>
                                <div class="title">
                                    <div class="title-l">
                                        <asp:Literal ID="Literal1" runat="server" Text='<%#Eval("ysgc") %>'></asp:Literal>
                                    </div>
                                    <div class="title-r">
                                        <a data-role="button" data-inline="true" onclick="View('<%#Eval("flowID") %>','<%#Eval("billCode") %>')">
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
                                    <%#Eval("showdept") %>
                                </div>
                                <div class="c-row-item">
                                    <div>
                                        单据金额：
                                    </div>
                                    ￥
                                <%#Eval("billJe","{0:N2}") %>
                                </div>
                            </div>
                            <div class="c_row_item">
                                <asp:HiddenField ID="hfCode" runat="server" Value='<%#Eval("billCode") %>' />
                                <asp:Label ID="lbmx" runat="server"></asp:Label>
                            </div>

                            <div class="c-option">
                                <span>审批意见：<input type="text" data-role="none" class="myInput" /></span>
                                <a data-role='button' data-inline='true' title='<%#Eval("billCode") %>' onclick="Audit('<%#Eval("flowID") %>','<%#Eval("billCode") %>',this)"
                                    data-theme='d'>
                                    <img src='../images/metro/Check.png' />通过</a> <a data-role="button" data-inline="true"
                                        title='<%#Eval("billCode") %>' onclick="Cancel('<%#Eval("flowID") %>','<%#Eval("billCode") %>',this)"
                                        data-theme="d">
                                        <img src="../images/metro/Unmark-To-Download.png" />驳回</a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div id="lbPageNav" runat="server">
                </div>
                <input id="hfBillCode" type="hidden" />
                <input id="hfFlowId" type="hidden" />
            </div>
            <div data-role="footer" data-position="fixed">
                <footer data-role="footer" id="footer">
                    <h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1>
                </footer>
            </div>
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
