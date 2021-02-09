<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkFlowList.aspx.cs" Inherits="webBill_MyWorkFlow_WorkFlowList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery.multiselect.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.multiselect.min.js" type="text/javascript"></script>

    <style type="text/css">
        #sortable {
            list-style-type: none;
            margin: 0;
            padding: 0;
            min-width: 100%;
        }

            #sortable li {
                margin: 0 3px 3px 3px;
                padding: 0.4em;
                padding-left: 1.5em;
                font-size: 1.4em;
                height: 20px;
            }

                #sortable li table {
                    position: absolute;
                    margin-left: 0em;
                }

                #sortable li input {
                    position: absolute;
                    margin-left: -1.5em;
                }

        .highlight {
            background: #EBF2F5;
        }

        .hiddenbill {
            display: none;
        }

        .style1 {
            background-color: #EDEDED;
            width: 100px;
            text-align: center;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $(function () {
            $("#TextBox3").datepicker();
            var uicont = $("#sortable>li").length;
            $("#dialog").dialog({
                autoOpen: false,
                width: 600,
                buttons: {
                    "确定": function () {
                        var sptype = $("#DropDownList1").val() + "[" + $("#DropDownList1 option:selected").text() + "]";
                        var spr = "";
                        if ($("#txt_spr").val() != null) {
                            spr = $("#txt_spr").val();
                        }
                        var sxje = $("#TextBox2").val();

                        var sxrq = $("#TextBox3").val();


                        var bz = $("#TextBox4").val();
                        var checktype = $("#drp_type").val() + "[" + $("#drp_type option:selected").text() + "]";
                        var isKmzhuguan = $("#drp_isKeMuZhuGuan").val() + "[" + $("#drp_isKeMuZhuGuan option:selected").text() + "]";
                        var kmtype = $("#DropDownList2").val() + "[" + $("#DropDownList2 option:selected").text() + "]";

                        if (sptype.indexOf("选择") > 0) {
                            alert("请选择审批类型");
                            return;
                        }
                        if ($("#DropDownList1").val() == "01") {
                            if (spr.length == 0) {
                                alert("审批人不能为空");
                                return;
                            }
                        }


                        if (kmtype.indexOf("不设置") == -1 && sxje.length == 0) {
                            alert("请填写生效金额");
                            return;
                        }

                        uicont++;
                        $("#sortable").append("<li id='li_" + uicont + "' class='ui-state-default'><input type='checkbox' id='chk_" + uicont + "' /><table><tr><td>审批类型:</td><td>" + sptype + "</td><td>审批人:</td><td>" + spr + "</td><td>费用类型：</td><td>" + kmtype + "</td><td>生效金额:</td><td>" + sxje + "</td><td>类型:</td><td>" + checktype + "</td><td>必须科目主管：</td><td>" + isKmzhuguan + "</td><td>生效日期:</td><td>" + sxrq + "</td><td>备注:</td><td>" + bz + "</td></tr></table></li>");
                        $(this).dialog("close");
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#sortable li").click(function () {
                $("#sortable li").each(function () { $(this).removeClass("highlight"); });
                $(this).addClass("highlight");
            });
            $("#sortable").sortable(); //拖拽方法
            $("#sortable").disableSelection();
            $("#btn_insert").click(function () {
                $('#dialog').dialog('open');
            });
            $("#btn_save").click(function () {

                var result = $('#sortable').sortable('toArray', {
                    connected: true,
                    attribute: 'id'
                });
                var test = "";

                var flowid = $("#hd_flowid").val();
                for (var i = 0; i < result.length; i++) {
                    var typecode = $("#" + result[i] + " table tr td")[1].innerHTML;
                    var checkcode = $("#" + result[i] + " table tr td")[3].innerHTML;
                    var kmtype = $("#" + result[i] + " table tr td")[5].innerHTML;
                    var cje = $("#" + result[i] + " table tr td")[7].innerHTML;
                    var checktype = $("#" + result[i] + " table tr td")[9].innerHTML;
                    var isKmZhuguan = $("#" + result[i] + " table tr td")[11].innerHTML;
                    var cdate = $("#" + result[i] + " table tr td")[13].innerHTML;
                    var bz = $("#" + result[i] + " table tr td")[15].innerHTML;
                    test += typecode + "," + checkcode + "," + cje + "," + cdate + "," + bz + "," + checktype + "," + isKmZhuguan + "," + kmtype + "|";
                }


                WorkFlowService.UpdateWorkFlows(test.substring(0, test.length - 1), flowid, UpdateSuccess, UpdateError);
                //WorkFlowService.UpdateWorkFlow(test.substring(0, test.length - 1), flowid, UpdateSuccess, UpdateError);

            });

            $("#btn_delete").click(function () {
                var lid = $("input[type=checkbox]:checked").parent().attr("id");
                $("#sortable li").remove("#" + lid);
            });
            $("#DropDownList1").change(function () {
                var typecode = $(this).val();
                $("#txt_spr").val("");
                if (typecode == "01") {
                    $("#btn_choose").removeClass("hiddenbill");
                    //$("#drp_type").addClass("hiddenbill");
                    //$("#drp_type option").eq(0).attr('selected', 'true');
                }
                else {
                    $("#btn_choose").addClass("hiddenbill");
                    //$("#drp_type").removeClass("hiddenbill");
                }
            });
            $("#btn_choose").click(function () {
                selectry();
            });

            spTypeChange();
            kmTypeChange();
        });

        function UpdateSuccess(result, context, methodName) {

            if (result > 0) {
                alert("保存成功!");
            }
            else {
                alert("保存失败!");
            }
        }
        function UpdateError(error, context, methodName) {
            alert(error);
        }

        function ChangeSuccess(result, context, methodName) {
            $("#txt_spr").val("");
            if (result != "") {
                var json = $.parseJSON(result);
                for (var i = 0; i < json.length; i++) {
                    $("#DropDownList2").append("<option value='" + json[i].ccode + "'>" + json[i].cname + "</option>");
                }
            }
        }

        function ChangeError(error, context, methodName) {
            $("#DropDownList2").empty();
            alert("AJAX失败!");
        }

        function selectry() {
            var str = window.showModalDialog('../select/SelectMoreUserFrame.aspx', 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:750px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                if ($("#txt_spr").val() != "") {
                    $("#txt_spr").val($("#txt_spr").val() + ":" + str);

                } else {
                    $("#txt_spr").val($("#txt_spr").val() + str);

                }
            }
        }

        function kmTypeChange() {
            if ($("#DropDownList2").val() == "") {
                $("#TextBox2").val("");
                $("#TextBox2").attr("readonly", 'true');
            }
            else
                $("#TextBox2").removeAttr("readonly");
        }


        function spTypeChange() {
            if ($("#DropDownList1").val() != "01") {
                $("#txt_spr").val("");
                $("#txt_spr").attr("readonly", 'true');
            }
            else
                $("#txt_spr").removeAttr("readonly");
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="dialog" title="编 辑">
            <table class="myTable">
                <tr>
                    <td class="style1">审批类型:
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server" onchange="spTypeChange()">
                        </asp:DropDownList>
                    </td>
                    <td class="style1">审批人:
                    </td>
                    <td>
                        <input type="text" id="txt_spr" />
                        <input type="button" id="btn_choose" value="选择" class="hiddenbill" />
                    </td>
                </tr>
                <tr>
                    <td class="tableBg">费用类型：
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList2" runat="server" onchange="kmTypeChange()">
                        </asp:DropDownList>
                    </td>
                    <td class="tableBg">生效金额:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tableBg">审批类型:
                    </td>
                    <td>
                        <select id="drp_type">
                            <option value="2">会签</option>
                            <option value="1">单签</option>
                        </select>
                    </td>
                    <td class="tableBg">必须科目主管:
                    </td>
                    <td>
                        <select id="drp_isKeMuZhuGuan">
                            <option value="0">否</option>
                            <option value="1">是</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="tableBg">生效日期:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    </td>
                    <td class="tableBg">备注:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table>
                <tr>
                    <td>
                        <input type="button" class="ui-widget-content ui-corner-all" id="btn_insert" value="新 增" />
                    </td>
                    <td>
                        <input type="button" class="ui-widget-content ui-corner-all" id="btn_delete" value="删 除" />
                    </td>
                    <td>
                        <input id="btn_save" type="button" value="保 存" class="ui-widget-content ui-corner-all" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <ul id="sortable" runat="server">
            </ul>
        </div>
        <div>
            <asp:HiddenField ID="hd_flowid" runat="server" />
            <asp:HiddenField ID="hd_spr" runat="server" />
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Services>
                    <asp:ServiceReference Path="~/webBill/MyWorkFlow/WorkFlowService.asmx" />
                </Services>
            </asp:ScriptManager>


        </div>
    </form>
</body>
</html>
