<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ystzFrame2.aspx.cs" Inherits="webBill_ystz_ystzFrame2" %>

<%@ Register Assembly="PaginationControl" Namespace="PaginationControl" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
    $(function(){
    $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                parent.helptoggle();
                }
            });
      $("#<%=GridView2.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=GridView2.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
    });
            function openDetail(openUrl) {
                var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
                if (returnValue == undefined || returnValue == "") {
                    return false;
                }
                else {
                    document.getElementById("Button4").click();
                }
            }
            var rowId = 0;
            $(function() {
                $("#TextBox1").autocomplete({
                    source: availableTags
                });
                $("#TextBox3").autocomplete({
                    source: availableTags
                });
                $("#TextBox5").autocomplete({
                    source: availablekm
                });
                $("#TextBox6").autocomplete({
                    source: availablekm
                });
                $('#tabs').tabs();
                $('#dialog').dialog({
                    autoOpen: false,
                    width: 600,
                    buttons: {
                        "确定": function() {
                            var json = "";
                            json += "yf:1,bl:" + $("#tb_1").val() + "|";
                            json += "yf:2,bl:" + $("#tb_2").val() + "|";
                            json += "yf:3,bl:" + $("#tb_3").val() + "|";
                            json += "yf:4,bl:" + $("#tb_4").val() + "|";
                            json += "yf:5,bl:" + $("#tb_5").val() + "|";
                            json += "yf:6,bl:" + $("#tb_6").val() + "|";
                            json += "yf:7,bl:" + $("#tb_7").val() + "|";
                            json += "yf:8,bl:" + $("#tb_8").val() + "|";
                            json += "yf:9,bl:" + $("#tb_9").val() + "|";
                            json += "yf:10,bl:" + $("#tb_10").val() + "|";
                            json += "yf:11,bl:" + $("#tb_11").val() + "|";
                            json += "yf:12,bl:" + $("#tb_12").val();

                            var gridView = document.getElementById("<%=GridView1.ClientID%>");
                            var kmbh = gridView.rows[rowId].cells[2].innerHTML;
                            var bmbh = gridView.rows[rowId].cells[4].innerHTML;
                            var je = gridView.rows[rowId].cells[6].getElementsByTagName("INPUT")[0].value;
                            var test = 0;
                            for (var i = 1; i <= 12; i++) {
                                test += parseFloat($("#tb_" + i.toString()).val()) * 1000000;
                            }
                            if (test != 1000000) {
                                alert(test);
                            }
                            else {
                                ystzServiece.UpdateYsbl(json, bmbh, kmbh, je, OnSuccess2, OnFailed2);
                            }
                        },
                        "取消": function() {
                            $(this).dialog("close");
                        }
                    }
                });
            });

            function editClickY(obj) {
                var yRow = obj.name.split("$")[1].substring(3, 5) - 1;
                var gridView = document.getElementById("<%=GridView2.ClientID%>");
                var gcbh = gridView.rows[yRow].cells[1].innerHTML;
                var kmbh = gridView.rows[yRow].cells[3].innerHTML;
                var bmbh = gridView.rows[yRow].cells[5].innerHTML;
                var je = gridView.rows[yRow].cells[7].getElementsByTagName("INPUT")[0].value;
                ystzServiece.UpdateYYsje(gcbh, bmbh, kmbh, je, OnSuccess3, OnFailed3);
            }
            function editClick(obj) {
                rowId = obj.name.split("$")[1].substring(3, 5) - 1;                
                $('#dialog').dialog('open');
                ystzServiece.GetYsbl(OnSuccess, OnFailed);
                return false;
            }

            function OnSuccess2(result, context, methodName) {
                if (result < 0) {
                    alert("请输入正确的分配比率！");
                } else {
                    alert("保存成功");
                    $('#dialog').dialog('close');
                }
            }

            function OnSuccess3(result, context, methodName) {
                if (result < 0) {
                    alert("保存失败!");
                } else {
                    alert("保存成功");
                }
            }
            
            function OnSuccess(result, context, methodName) {
                var json = $.parseJSON(result);
                for (var i = 0; i < json.length; i++) {
                    switch (json[i].yf) {
                        case '1': $("#tb_1").val(json[i].bl); break;
                        case '2': $("#tb_2").val(json[i].bl); break;
                        case '3': $("#tb_3").val(json[i].bl); break;
                        case '4': $("#tb_4").val(json[i].bl); break;
                        case '5': $("#tb_5").val(json[i].bl); break;
                        case '6': $("#tb_6").val(json[i].bl); break;
                        case '7': $("#tb_7").val(json[i].bl); break;
                        case '8': $("#tb_8").val(json[i].bl); break;
                        case '9': $("#tb_9").val(json[i].bl); break;
                        case '10': $("#tb_10").val(json[i].bl); break;
                        case '11': $("#tb_11").val(json[i].bl); break;
                        case '12': $("#tb_12").val(json[i].bl); break;
                    }
                }
            }
            
            
            function OnFailed(error, context, methodName) {

            }
            function OnFailed2(error, context, methodName) {

            }
            function OnFailed3(error, context, methodName) {

            } 
        function gudingbiaotou() {
            var t = document.getElementById("<%=GridView1.ClientID%>");
            if (t == null || t.rows.length < 1) {
                return;
            }
            var t2 = t.cloneNode(true);
			 t2.id = "cloneGridView";
            for (i = t2.rows.length - 1; i > 0; i--) {
                t2.deleteRow(i);
            }
            t.deleteRow(0);
            header.appendChild(t2);
            var mainwidth = document.getElementById("main").style.width;
            mainwidth = mainwidth.substring(0, mainwidth.length - 2);
            mainwidth = mainwidth - 16;
            document.getElementById("header").style.width = mainwidth;
        }

    </script>

    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<body onload="gudingbiaotou();">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/webBill/ystz/ystzServiece.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="dialog" title="预算分配">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <table class="myTable">
                        <tr>
                            <td class="tableBg2">
                                一月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_1" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                二月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_2" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                三月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_3" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                四月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_4" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                五月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_5" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                六月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_6" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                七月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_7" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                八月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_8" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                九月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_9" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                十月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_10" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                十一月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_11" CssClass="baseText" runat="server"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                十二月:
                            </td>
                            <td>
                                <asp:TextBox ID="tb_12" CssClass="baseText" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">年度预算调整</a></li>
            <li><a href="#tabs-2">月预算调整</a></li>
        </ul>
        <div id="tabs-1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                部门:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                科目:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="Button4" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button4_Click" />
                                  <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                            </td>
                        </tr>
                    </table>
                    <div id="header" style="overflow: hidden;">
                    </div>
                    <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 1000px; height: 320px;">
                        <asp:GridView ID="GridView1" Style="table-layout: fixed" Width="100%" runat="server"
                            AutoGenerateColumns="False" CssClass="ui-widget ui-widget-content">
                            <Columns>
                                <asp:TemplateField HeaderText="选择">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="xmmc" HeaderText="项目名称"  ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="yskmcode" HeaderText="科目编号" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="yskmmc" HeaderText="科目名称" ItemStyle-Wrap="true" />
                                <asp:BoundField DataField="deptcode" HeaderText="部门编号" ItemStyle-Wrap="true"/>
                                <asp:BoundField DataField="deptname" HeaderText="部门名称" ItemStyle-Wrap="true"/>
                                <asp:TemplateField HeaderText="预算金额">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="baseText" Text='<%# Bind("ysje") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="修改">
                                    <ItemTemplate>
                                        <asp:Button ID="Button1" runat="server" Text="修 改" CssClass="baseButton" OnClientClick="return editClick(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="ui-widget-header" />
                        </asp:GridView>
                    </div>
                    <p>
                        <cc1:PaginationToGV ID="PaginationToGV1" runat="server" OnGvBind="PaginationToGV1_GvBind" />
                    </p>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="tabs-2">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                部门:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                科目:
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="Button2" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button2_Click" />
                                  <input type="button" class="baseButton" value="帮助" onclick="javascript:parent.helptoggle();" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="ui-widget ui-widget-content">
                        <Columns>
                            <asp:TemplateField HeaderText="选择">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox2" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="gcbh" HeaderText="项目编号" />
                            <asp:BoundField DataField="xmmc" HeaderText="项目名称" />
                            <asp:BoundField DataField="yskmcode" HeaderText="科目编号" />
                            <asp:BoundField DataField="yskmmc" HeaderText="科目名称" />
                            <asp:BoundField DataField="deptcode" HeaderText="部门编号" />
                            <asp:BoundField DataField="deptname" HeaderText="部门名称" />
                            <asp:TemplateField HeaderText="预算金额">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("ysje") %>' CssClass="baseText"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="Button5" runat="server" Text="修 改" CssClass="baseButton" OnClientClick="return editClickY(this)" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="ui-widget-header" />
                    </asp:GridView>
                    <p>
                        <cc1:PaginationToGV ID="PaginationToGV2" runat="server" OnGvBind="PaginationToGV2_GvBind" />
                    </p>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
