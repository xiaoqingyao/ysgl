<%@ Page Language="C#" AutoEventWireup="true" Inherits="webBill_tjbb_dz_zhYsTj" CodeFile="zhYsTj.aspx.cs"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>综合预算统计</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery.multiselect.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.multiselect.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script type="text/javascript">
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            $("#TextBox1").datepicker();
            $("#TextBox2").datepicker();
            $("#btnSr").click(function () {
                window.showModalDialog("bill_bmsrysList.aspx", 'newwindow', 'center:yes;dialogHeight:7000px;dialogWidth:800px;status:no;scroll:yes');
            });
            $("#btnFykm").click(function () {
                var str = window.showModalDialog("SelectYskm.aspx?dydj=02&forchanchu=1", 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
                if (str==null||str.length == 0) {
                    return;
                }
                var json = $.parseJSON(str);
                var yskm = "";
                var yskmcode = "";
                for (var i = 0; i < json.length; i++) {
                    if (json[i].Yscode != "") {
                        yskmcode += json[i].Yscode.substring(1, json[i].Yscode.indexOf("]")) + ",";
                        yskm += json[i].Yscode + ";";
                    }

                }
                $("#txtyskm").text(yskm);
                $("#hf_yskm").val(yskmcode);
                //alert(yskm);

            });
            $("#btndept").click(function () {
                var str = window.showModalDialog("Selectdept.aspx", 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
                if (str == null || str.length == 0) {
                    return;
                }
                var json = $.parseJSON(str);
                var ysdept = "";
                var deptcode = "";
                for (var i = 0; i < json.length; i++) {
                    if (json[i].deptcode != "") {
                        deptcode += json[i].deptcode.substring(1, json[i].deptcode.indexOf("]")) + ",";
                        ysdept += json[i].deptcode + ";";
                    }

                }
                $("#lbl_dept").text(ysdept);
                $("#hf_dept").val(deptcode);
            });

        });
        function toSearch() {
            var retstr = $("#hf_dept").val();
            var retstrkm = $("#hf_yskm").val();

            if (retstr == "") {
                alert("请选择单位");
                return false;
            }
            if (retstrkm == "") {
                alert("请选择科目");
                return false;
            }

        }

        function showWait() {
            document.getElementById("divOver").style.visibility = "visible";
        }


    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <div style="width: 400px; margin: 0 auto;">
            <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
                <tr>
                    <td style="height: 125px; text-align: center"></td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 27px;">
                        <strong><span style="font-size: 12pt">投入产出报表设置</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="height: 26px; text-align: center">
                        <table border="0" cellpadding="0" cellspacing="0" class="myTable">
                                 <tr>
                                <td class="tableBg">财年
                                </td>
                                <td colspan="2" style="width: 257px">
                                    <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                    <asp:DropDownList ID="drpyear" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="drpyear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">开始月份
                                </td>
                                <td colspan="2" style="width: 257px">
                                    <asp:DropDownList ID="bgintime" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">截止月份
                                </td>
                                <td colspan="2" style="width: 257px">
                                    <asp:TextBox ID="TextBox2" runat="server" Visible="false"></asp:TextBox>
                                    <asp:DropDownList ID="endtime" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">统计科目
                                </td>
                                <td>
                                    <asp:Label ID="txtyskm" runat="server"></asp:Label>

                                    <input type="button" value="选择科目" id="btnFykm" runat="server" class="baseButton" />

                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">统计单位
                                </td>
                                <td colspan="2" style="width: 257px">

                                    <asp:Label ID="lbl_dept" runat="server"></asp:Label>
                                    <input type="button" value="选择部门" id="btndept" runat="server" class="baseButton" />

                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">收入情况
                                </td>
                                <td colspan="2" style="width: 257px">

                                    <input type="button" value="打开录入页面" id="btnSr" runat="server" class="baseButton" />

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 26px; text-align: center">
                        <asp:Button ID="Button1" runat="server" CssClass="baseButton" OnClick="Button1_Click"
                            Text="生成统计表" OnClientClick="return toSearch();" />
                        <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                        <asp:HiddenField ID="hf_dept" runat="server" />
                        <asp:HiddenField ID="hf_yskm" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divOver" runat="server" style="z-index: 1200; left: 30%; width: 160; cursor: wait; position: absolute; top: 25%; height: 100; visibility: hidden;">
            <table style="width: 17%; height: 10%;">
                <tr>
                    <td>
                        <table style="width: 316px; height: 135px;">
                            <tr align="center" valign="middle">
                                <td>
                                    <img src="../../webBill/Resources/Images/Loading/pressbar2.gif" alt="" /><br />
                                    <b>正在处理中，请耐心等候....<br />
                                    </b>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
