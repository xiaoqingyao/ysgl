<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cwtbList.aspx.cs" Inherits="webBill_ysglnew_cwtbList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        $(function() {
        $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
          $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                if($(this).find("td")[0]!=null)
                {
                    var billCode = $(this).find("td")[0].innerHTML;
                $("#choosebill").val(billCode);
                }
            
                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            $("#btn_delete").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[6].innerHTML;
                if (zt != "未提交") {
                    alert("该行已提交,不能删除！");
                    return;
                }
                if (!confirm("您确定要删除选中的单据吗?")) {
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                $.post("../MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": "cwtb" }, function(data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            alert("删除成功!");
                            $(".highlight").remove();
                        }
                        else {
                            alert("删除失败1!");
                        }
                    }
                    else {
                        alert("删除失败2!");
                    }
                });
            });
            $("#btn_replace").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[6].innerHTML;
                if (zt == "未提交" || zt == "审批通过") {
                    alert("该单据不能撤销操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "ys" }, function(data, status) {
                        if (status == "success") {
                            if (data == "1") {
                                alert("提交成功！");
                                checkrow.find("td")[6].innerHTML = "未提交";
                            }
                            else {
                                alert("单据以进入审批，不能撤销");
                            }
                        }
                        else {
                            alert("失败");
                        }
                    });
                }
            });
            $("#btn_summit").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[6].innerHTML;
                if (zt != "未提交") {
                    alert("单据已提交.不要重复操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": "ys" }, function(data, status) {
                        if (status == "success") {
                            if (data == "-1") {
                                alert("预算过程缺少财务填报部分,不能提交！");
                            }
                            else if (data == "-2") {
                                alert("预算过程缺少部门填报部分,不能提交！");
                            }else if (data == "-3") {
                            alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                             } 
                            else {
                                alert("提交成功！");
                                checkrow.find("td")[6].innerHTML = data;
                            }
                        }
                        else {
                            alert("失败");
                        }
                    });
                }
            });
        });
        function editCheck() {
            var deptcode = '<%=Request["deptCode"] %>';
            if ($("#drpSelectNd").val() == "0") {
                alert("请选择年度");
                return false;
            }
            else {
                var returnValue = window.showModalDialog("cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + $("#drpSelectNd").val() + "&type=cwtb", 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:1200px;status:no;scroll:yes');
                if (returnValue == "1") {
                    $("#Button6").click();
                }
                return false;
            }
        }

        function lookCheck() {
            var deptcode = '<%=Request["deptCode"] %>';
            if ($("#drpSelectNd").val() == "0") {
                alert("请选择年度");
                return false;
            }
            else {
                var returnValue = window.showModalDialog("cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + $("#drpSelectNd").val() + "&type=cwtb&look=look", 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:1200px;status:no;scroll:yes');
                return false;
            }
        }
        
        function openDetail(openUrl)
        {
            var returnValue=window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:900px;status:no;scroll:yes');
            if(returnValue=="success")
            {
                document.getElementById("Button6").click();
            }
        }
        function openLookSpStep(openUrl)
        {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        function OpenAdd() {
            var deptcode = '<%=Request["deptCode"] %>';
            if ($("#drpSelectNd").val() == "0") {
                alert("请选择年度");
                return false;
            }
            else {
                var returnValue = window.showModalDialog("cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + $("#drpSelectNd").val()+"&type=cwtb", 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:1200px;status:no;scroll:yes');
                if (returnValue == "1") {
                    $("#Button6").click();
                }
                return false;
            }
        }
          function gudingbiaotou() {
            var t = document.getElementById("<%=myGrid.ClientID%>");
              if(t==null||t.rows.length<1){
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

</head>
<body onload="gudingbiaotou();">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" style="margin-left:5px">
        <tr>
            <td style="height: 27px">
                预算年度：
                <asp:DropDownList ID="drpSelectNd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSelectNd_SelectedIndexChanged">
                    <asp:ListItem Value="0" Text="未选择"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;<asp:Button ID="Button1" runat="server" Text="增 加" CssClass="baseButton" OnClick="Button1_Click"
                    OnClientClick=" return OpenAdd()" />&nbsp;
                <asp:Button ID="btn_edit" runat="server" Text="修 改" OnClientClick="return editCheck()"
                    OnClick="btn_edit_Click" CssClass="baseButton" />&nbsp;
                <input type="button" value="删 除" id="btn_delete" class="baseButton" />&nbsp;
                <asp:Button ID="btn_look" runat="server" Text="完整信息" OnClientClick="return lookCheck()"
                    OnClick="btn_look_Click" CssClass="baseButton" />
                &nbsp;
                <input type="button" value="审批提交" id="btn_summit" class="baseButton" />&nbsp;
                <input type="button" value="审批撤销" id="btn_replace" class="baseButton" />&nbsp;
                <asp:Button ID="Button6" runat="server" Text="刷新数据" CssClass="baseButton" OnClick="Button6_Click" />
                 <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
            </td>
        </tr>
        <tr>
            <td>
                <div id="header" style="overflow: hidden;">
                </div>
                <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 850px; height: 360px;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" Style="table-layout: fixed" Width="100%" PageSize="17" OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                            <asp:BoundColumn DataField="billCode" HeaderText="单据编号">
                                <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem hiddenbill" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billName" HeaderText="预算过程">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True"  Width="150" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Width="150" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDept" HeaderText="填报单位">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billUser" HeaderText="制单人">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" DataFormatString="{0:D}" HeaderText="制单日期">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billje" HeaderText="金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepid" HeaderText="审批状态">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                             <asp:BoundColumn DataField="mind" HeaderText="驳回理由">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 30px">
                &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                </asp:DropDownList>
                页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                    runat="server"></asp:Label>条
            </td>
        </tr>
        <tr>
            <td style="height: 30px">
                <table>
                    <tr>
                        <td>
                            审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
                        </td>
                        <td id="wf">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="choosebill" runat="server" />
    </form>
</body>
</html>
