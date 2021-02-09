<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PoMoreList.aspx.cs" Inherits="webBill_main_PoMoreList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>我的单据</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
   <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript" charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">
      
      var status = "none";
        $(function() {
            $("#btn_edit").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[2].innerHTML;
                if (zt != "未提交") {
                    alert("该行已提交,不能删除！");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("bxDetailFinal.aspx?type=edit&billCode=" + billcode);
            });
         
           
            $("#<%=myGrid.ClientID%> tr:gt(0)").dblclick(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                if ($(this).find("td")[0] != null) {
                    var billCode = $(this).find("td")[0].innerHTML;
                  
                }
                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
               // alert($(this).find("td:eq(6)").html().replace("amp;",""));
                openDetail($(this).find("td:eq(6)").html().replace("amp;",""));
            });
         
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                if ($(this).find("td")[0] != null) {
                    var billCode = $(this).find("td")[0].innerHTML;
                }

                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });

            $("#btn_replace").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var zt = checkrow.find("td")[2].innerHTML;
                if (zt == "未提交" || zt == "审批通过") {
                    alert("该单据不能撤销操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    $.post("../MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "ybbx" }, function(data, status) {
                        if (status == "success") {
                            if (data == "1") {
                                alert("撤销成功！");
                                checkrow.find("td")[2].innerHTML = "未提交";
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
                var zt = checkrow.find("td")[2].innerHTML;
                if (zt != "未提交") {
                    alert("单据已提交.不要重复操作!");
                } else {
                    var billcode = checkrow.find("td")[0].innerHTML;
                    $.post("../MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": "ybbx" }, function(data, status) {
                        if (data == "-1" || data == "-2") {
                            alert("提交失败！");
                        } else if (data == "-3") {
                            alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
                        } else {
                            if (status == "success") {
                                alert("提交成功！");
                                checkrow.find("td")[2].innerHTML = data;
                            }
                        }
                    });
                }
            });
        });
    
    
        function openDetail(openUrl)
        {
            var returnValue=window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            if(returnValue==undefined||returnValue=="")
            {
                return false;
            }
            else
            {
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
          $(function() {
        $("#txb_sqrqbegin").datepicker();
        $("#txb_sqrqend").datepicker();
       
        });
    </script>

</head>
<body onload="gudingbiaotou()">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr >
            <td style="height: 27px">
              <%--  <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>--%>
              单据类型：<asp:DropDownList ID="ddltype" runat="server"  Width="100px" 
                    AutoPostBack="True" onselectedindexchanged="ddltype_SelectedIndexChanged"> </asp:DropDownList>
              日期从：<asp:TextBox ID="txb_sqrqbegin" runat="server"   Width="80px"></asp:TextBox>至<asp:TextBox ID="txb_sqrqend" runat="server" Width="80px"></asp:TextBox>
                <asp:Button ID="Button2" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button2_Click" />
                <input type="button" value="审批提交" id="btn_summit" class="baseButton" />&nbsp;
                <input type="button" value="审批撤销" id="btn_replace" class="baseButton" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <div id="header" style="overflow: hidden;">
                </div>
                <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 900px; height: 420px;">
                   <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" Style="table-layout: fixed" Width="100%"  AllowPaging="True" PageSize="17" OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                            <asp:BoundColumn DataField="billCode" HeaderText="单据编号[查]">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem hiddenbill" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billType" HeaderText="单据类型[查]">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="单据状态" DataField="stepid">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="单据类型" DataField="flowID">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader  hiddenbill" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem  hiddenbill" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="单据日期" DataField="billdate">
                                 <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="报销摘要" DataField="bxzy">
                                 <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill">
                            <ItemTemplate>
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                            </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                     <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
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
            <td style="height: 10px">
                <table>
                    <tr>
                        <td>
                            审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdYbbxNeedAudit" runat="server" />
                        </td>
                        <td id="wf">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    </form>
</body>
</html>
