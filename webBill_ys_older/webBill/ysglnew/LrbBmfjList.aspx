<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LrbBmfjList.aspx.cs" Inherits="webBill_ysglnew_LrbBmfjList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <style type="text/css">
        .righttxt
        {
            text-align: right;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            gudingbiaotounew($("#GridView1"), $(window).height() - 100);
            initMainTableClass("<%=GridView1.ClientID%>");
            $("#btn_excel").click(function() {
                window.open("DeptToExcel.aspx");
            });
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=GridView1.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                //    $("#hf_user").val($(".highlight td:eq(0)").html());
            });

            $("#btn_edit").click(function() {

                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var varnd = '<%=Request["nd"] %>'; //年度
                var ysmoney = checkrow.find("td")[3].children[0].value; //预算金额
                var varwfpmoney = $("#hidwfmoney").val(); //未分配金额
                var vardeptcode = checkrow.find("td")[2].innerHTML//部门编号
                vardeptcode = vardeptcode.substring(1, vardeptcode.indexOf("]"));
                var varkmcode = $("#hidkmcode").val(); //科目编号
                var varstatus = checkrow.find("td")[7].innerHTML;

                if (varstatus == "部门确认") {
                    openDetail("LrbBmfjDetails.aspx?nd=" + varnd + "&deptCode=" + vardeptcode + "&yskmcode=" + varkmcode + "&ymoney=" + ysmoney + "&wfpmoney=" + varwfpmoney);

                } else if (varstatus == "未确认") {
                    if (confirm("该科目没有经过部门确认，增加的额度将直接变为状态为部门确认的有效报销额度，是否继续？")) {
                        openDetail("LrbBmfjDetails.aspx?nd=" + varnd + "&deptCode=" + vardeptcode + "&yskmcode=" + varkmcode + "&ymoney=" + ysmoney + "&wfpmoney=" + varwfpmoney);
                    }
                } else {
                    alert("请选择状态为部门确认的记录！");
                    return;
                }
            });
        });
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
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:200px;dialogWidth:480px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                location.replace(location.href);

            }
        }

        //替换非数字
        function replaceNaN(obj) {
            var objval = obj.value;
            if (objval.indexOf("-") == 0) {
                objval = objval.substr(1);
            }
            if (isNaN(objval)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            }
        }
        
        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "-1px 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
        function ImportExcel() {
            if (confirm('导入将会将原来做的对应年度的分解金额冲掉，确定继续吗？')) {
                window.showModalDialog("LrbBmfjList_ImportExcel.aspx", 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:730px;status:no;scroll:no');
            }
        }
    </script>

</head>
<body style="text-align: left">
    <form id="form1" runat="server">
    <table  style="margin-left:5px">
        <tr>
            <td>
                <div style="margin-top: 3px; margin-left: 3px">
                    <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click" />
                    <asp:Button ID="btn_qr" runat="server" Text="确 认" CssClass="baseButton" OnClientClick="return confirm('将会确认所有记录，是否继续？')"
                        OnClick="btn_qr_Click" />
                    <asp:Button ID="Button1" runat="server" Text="通过申报金额" CssClass="baseButton" OnClientClick="return confirm('申报金额将作为预算控制金额并确认，是否继续？')"
                        OnClick="btn_qrsb_Click" />
                    <input class="baseButton" type="button" id="btn_edit" value="增加预算内金额" runat="server" />
                    <asp:Button ID="Button2" runat="server" Text="导出EXCEL" CssClass="baseButton" OnClick="Button2_Click" />
                    <input type="button" id="import" value="导入EXCEL" class="baseButton" onclick="ImportExcel();" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
                </div>
                <div style="margin-top: 3px; margin-left: 3px">
                    预算状态：
                    <asp:DropDownList runat="server" ID="ddlStatus" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                        <asp:ListItem Value="">--全部--</asp:ListItem>
                        <asp:ListItem Value="0">未确认</asp:ListItem>
                        <asp:ListItem Value="1">预算确认</asp:ListItem>
                        <asp:ListItem Value="2">部门确认</asp:ListItem>
                        <asp:ListItem Value="3">部门异议</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;预算项目：
                    <asp:DropDownList runat="server" ID="ddlXm" AutoPostBack="true" OnSelectedIndexChanged="ddlXm_SelectedIndexChanged">
                    </asp:DropDownList>
                    <%--年度--%>
                </div>
                <div style="margin-top: 3px; margin-left: 3px">
                    科目：<asp:Label ID="LaKm" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                    <asp:Label ID="LaNd" runat="server" Text="" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
                    &nbsp; 总金额：<asp:Label ID="LaZje" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                    &nbsp; 未分配金额：<asp:Label ID="Syje" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div style="position: relative; word-warp: break-word; word-break: break-all">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                        Width="920px" ShowFooter="true" OnRowDataBound="GridViewRowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="行号" DataField="" ItemStyle-Width="65" HeaderStyle-Width="65"
                                FooterStyle-Width="65">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                                <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="预算科目" DataField="yskmname" ItemStyle-Width="200" HeaderStyle-Width="200"
                                FooterStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                                <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="部门" DataField="deptname" ItemStyle-Width="200" HeaderStyle-Width="200"
                                FooterStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                                <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="预算控制金额" ItemStyle-Width="130" HeaderStyle-Width="130"
                                FooterStyle-Width="130">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                                <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox1" Width="90%" Text="0.00" runat="server" CssClass="righttxt"
                                        onkeyup="replaceNaN(this);"></asp:TextBox>
                                    <asp:HiddenField ID="hidddept" runat="server" Value='<%#Eval("deptcode") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbeTotalAmount" runat="server" Text="0"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- <asp:BoundField HeaderText="预算申报金额" DataField="">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                <ItemStyle CssClass="myGridItem"    Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                <FooterStyle CssClass="myGridItem"    Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="预算申报金额" ItemStyle-Width="100" HeaderStyle-Width="100"
                                FooterStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                                <FooterStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbleveShenBao"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label runat="server" ID="lbltotalShenBao"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="申报差额" ItemStyle-Width="100" HeaderStyle-Width="100" FooterStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                                <FooterStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="建议说明" DataField="" ItemStyle-Width="100" HeaderStyle-Width="100"
                                FooterStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                                <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="状态" DataField="" ItemStyle-Width="70" HeaderStyle-Width="70"
                                FooterStyle-Width="50">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                                <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="预算编号" DataField="yskmcode" HeaderStyle-CssClass="hiddenbill"
                                ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"></asp:BoundField>
                            <asp:BoundField HeaderText="附件" DataField="" ItemStyle-Width="100" HeaderStyle-Width="100"
                                FooterStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                                <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="附件地址" DataField="" HeaderStyle-CssClass="hiddenbill"
                                ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="hidwfmoney" runat="server" />
                    <asp:HiddenField ID="hidkmcode" runat="server" />
                </div>
            </td>
        </tr>
    </table>  <script type="text/javascript">
                  parent.parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
