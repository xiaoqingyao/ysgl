<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YstzDetailNew.aspx.cs" Inherits="webBill_ysgl_YstzDetailNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery.multiselect.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.multiselect.min.js" type="text/javascript"></script>

    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <style type="text/css">
        .style1
        {
            background-color: #EDEDED;
        }
    </style>

    <script type="text/javascript">
        //        $(function() {
        //            $.post("../MyAjax/GetBudgetByKm.ashx", { "dept": "000009", "km": "0102", "type": "edit", "billCode": "9E278153-3870-4DD9-8C82-15321584B27F" }, function(data, status) {
        //                if (status == "success") {
        //                    $("#tb_ys").html(data);
        //                }
        //                else {
        //                    alert("获取源预算失败");
        //                }
        //            });
        //        });
        $(function() {
            $("#drp_yskm").multiselect({
                header: false,
                noneSelectedText: "请选择一个科目",
                selectedText: function(numChecked, numTotal, checkedItems) {
                    return numChecked + '个科目被选中了';
                }
            });
            //选择科目到列表页
            $("#btn_getYs").click(function() {
                bindystz();

            });
            $("#btn_save").click(function() {
                var json = saveMake();
                if (json == "") {
                    alert("请填写相关明细金额！");
                    return;
                }
                $.post("../MyAjax/YstzBillSave.ashx", json, function(data, status) {
                    if (status == "success") {
                        if (data == "0") {
                            alert("保存成功!");
                            window.returnValue = "success";
                            window.close();
                        }
                        else if (data == "-1") {
                            alert("保存失败1");
                        }
                        else if (data == "-2") {
                            alert("保存失败2");
                        }
                        else if (data == "-0") {
                            alert("填报总金额为0，保存失败!");
                        }
                        else {
                            alert("保存失败3,第" + data + "行超支了!");
                        }
                    }
                    else {
                        alert("调整预算失败");
                    }
                });
            });
        });

        function saveMake() {
            var savStr = "";
            $(".ysje").each(function() {
                var je = $(this).val();
                if (je != "") {
                    var km = $(this).parent().parent().find("td")[0].innerHTML;
                    km = km.split("]")[0].substring(1);
                    var gcbh = $(this).parent().parent().find("td")[1].innerHTML;
                    gcbh = gcbh.split("]")[0].substring(1);
                    savStr += '{"gcbh":"' + gcbh + '","kmbh":"' + km + '","je":"' + je + '"},';
                }
            });
            if (savStr.length < 1) {
                savStr = "";
            }
            else {
                var billcode = $("#hf_billcode").val();
                var zy = $("#txt_zy").val();
                var tzbm = $("#txt_dept").val();
                var forgcbh = $("#txt_source").val();
                savStr = '{"list":[' + savStr.substring(0, savStr.length - 1) + '],"billcode":"' + billcode + '","zy":"' + zy + '","tzbm":"' + tzbm.substring(1, tzbm.indexOf(']')) + '","forgcbh":"' + forgcbh + '"}';
            }
            return savStr;
        }
        //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }
        function bind() {
            bindystz();
        }

        function bindystz() {
            var retstr = "";
            var mbysgc = $("#txt_source").val();

            $("input[type='checkbox']:checked").each(function() {
                retstr += $(this).val() + "|";
            });
            if (retstr == "") {
                alert("请选择科目");
            } else {
                retstr = retstr.substring(0, retstr.length - 1);
                var dept = $("#txt_dept").val().split("]")[0].substring(1);
                $.post("../MyAjax/GetBudgetByKm.ashx", { "dept": dept, "km": retstr, "type": "add", "mbysgc": mbysgc }, function(data, status) {
                    if (status == "success") {
                        $("#tb_ys").html(data);
                    }
                    else {
                        alert("获取源预算失败");
                    }
                });
            }
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div style="width: 800px; margin: 0 auto;">
        <table class="myTable" cellpadding="0" width="100%">
            <tr>
                <td colspan="4" class="billtitle">
                    预算调整单
                </td>
            </tr>
            <tr>
                <td class="tableBg2">
                    制单人:
                </td>
                <td>
                    <asp:TextBox ID="txt_zdr" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
                <td class="tableBg2">
                    目标预算过程:
                </td>
                <td>
                    <%--<asp:TextBox ID="txt_source" runat="server" ReadOnly="true" Width="90%"></asp:TextBox>--%>
                    <asp:DropDownList runat="server" ID="txt_source" Width="90%" onchange="bind();">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tableBg2">
                    调整部门:
                </td>
                <td>
                    <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
                <td class="tableBg2">
                    预算科目:
                </td>
                <td>
                    <asp:DropDownList ID="drp_yskm" runat="server">
                    </asp:DropDownList>
                    <input type="button" value="确定" class="baseButton" id="btn_getYs" runat="server" />
                    <asp:Label ID="lb_yskm" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableBg2">
                    摘要:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txt_zy" runat="server" Width="90%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <table width="100%" class="myGrid">
                        <thead class="myGridHeader">
                            <tr>
                                <td>
                                    科目
                                </td>
                                <td>
                                    源预算过程
                                </td>
                                <td>
                                    预算金额
                                </td>
                                <td>
                                    花费金额
                                </td>
                                <td>
                                    占用金额
                                </td>
                                <td>
                                    可调整金额
                                </td>
                                <td>
                                    调整金额
                                </td>
                            </tr>
                        </thead>
                        <tbody id="tb_ys" runat="server">
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: center">
                    <input id="btn_save" type="button" value="确 定" class="baseButton" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="Button2" type="button" value="取 消" class="baseButton" onclick="javascript:window.close();" />
                    <asp:HiddenField ID="hf_billcode" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
