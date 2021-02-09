<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Bmfjjeqr.aspx.cs" Inherits="webBill_ysglnew_Bmfjjeqr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            gudingbiaotounew($("#GridView1"), $(window).height() - 100);
            initMainTableClass("<%=GridView1.ClientID%>");
           <%-- $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").dblclick(function () {
                var nd = $("#<%=drpSelectNd.ClientID %>").val();
                var deptcode = $(this).children().eq(6).html();
                var kmcode = $(this).children().eq(7).html();
                var kmje = $(this).children().eq(3).html();
                if (nd == undefined || nd == '' || deptcode == undefined || deptcode == '' || kmcode == undefined || kmcode == '' || kmje == undefined || kmje == '') {
                    return;
                }
                openUrl = 'ViewFjDeptAmount.aspx?nd=' + nd + '&kmcode=' + kmcode + '&gkdeptcode=' + deptcode + '&kmje=' + kmje;
                window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:650px;dialogWidth:680px;status:no;scroll:auto');
            });--%>
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
        function addAttachment_Click() {
            var checkrow = $(".highlight");
            if (checkrow == null || checkrow == undefined || checkrow.val() == undefined) {
                alert("请先选择行。"); return;
            }
            var deptcode = checkrow.find("td")[6].innerHTML;
            var nd = $("#drpSelectNd").val();
            var kmcode = checkrow.find("td")[2].innerHTML;
            kmcode = kmcode.split(']')[0];
            kmcode = kmcode.substring(1, kmcode.length);
            window.showModalDialog("Bmfjjeqr_AddAttachment.aspx?nd=" + nd + "&deptcode=" + deptcode + "&kmcode=" + kmcode, 'newwindow', 'center:yes;dialogHeight:200px;dialogWidth:750px;status:no;scroll:auto');
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
        function queren() {
            if ($("#" + '<%=GridView1.ClientID%>').html().indexOf("部门确认")!=-1) {
                alert("当前状态已经是部门确认状态，请勿重复确认。");
                return false;
            }
            return confirm('将会确认所有科目的分配金额，确认后将确定本部门的年度预算，是否继续？');
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>
                    <div style="margin-top: 3px;">
                        财年：<asp:DropDownList ID="drpSelectNd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSelectNd_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; 部门：<asp:DropDownList runat="server" ID="LaDept" AutoPostBack="True" OnSelectedIndexChanged="LaDept_SelectedIndexChanged">
                        </asp:DropDownList>
                        <%--<asp:Label ID="LaDept" runat="server" Text="" ForeColor="Red" Font-Bold="true">  OnClientClick="return queren();"</asp:Label>--%>&nbsp;
                    <asp:Button ID="btn_Submit" runat="server" Text="申报提交" CssClass="baseButton" OnClientClick="return confirm('将会提交建议金额至预算主管，确定要继续吗？');"
                        OnClick="btn_Submit_Click" />&nbsp;
                    <asp:Button ID="btn_Approve" runat="server" Text="确认预算" CssClass="baseButton"
                        OnClick="btn_Approve_Click" />
                        <%--状态：<asp:Label ID="LaState" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>--%>
                        <asp:Button ID="Button2" runat="server" Text="导出EXCEL" CssClass="baseButton" OnClick="Button2_Click" />
                        <input type="button" id="addAttachment" runat="server" value="添加附件" class="baseButton" onclick="addAttachment_Click();" />
                        <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                        <span style="color: Red">
                            <asp:Label ID="lblTs" runat="server" Visible="false">【友情提示】：双击行可以查看归口部门预算分解情况。</asp:Label></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="position: relative; word-warp: break-word; word-break: break-all">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                            Width="1060" PageSize="9999" ShowFooter="true" OnRowDataBound="GridView_DataBound">
                            <Columns>
                                <asp:BoundField HeaderText="序号" DataField="" ItemStyle-Width="40" HeaderStyle-Width="40"
                                    FooterStyle-Width="40">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="部门" DataField="deptName" ItemStyle-Width="150" HeaderStyle-Width="150"
                                    FooterStyle-Width="150">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="科目" DataField="kmcode" ItemStyle-Width="170" HeaderStyle-Width="170"
                                    FooterStyle-Width="150">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="预算控制金额" DataField="je" DataFormatString="{0:N2}" ItemStyle-Width="100"
                                    HeaderStyle-Width="100" FooterStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="预算申报金额" ItemStyle-Width="170" HeaderStyle-Width="170"
                                    FooterStyle-Width="170">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItemRight" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtjyje" runat="server" Text='<%#Eval("jyje") %>' CssClass="rightBox"></asp:TextBox>
                                        <asp:HiddenField ID="Hiddxmbh" runat="server" Value='<%#Eval("xmbh") %>' />
                                        <asp:HiddenField ID="Hiddkmbh" runat="server" Value='<%#Eval("kmbh") %>' />
                                    </ItemTemplate>
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="说明" ItemStyle-Width="160" HeaderStyle-Width="160"
                                    FooterStyle-Width="160">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtsm" Width="80%" runat="server" Text='<%#Eval("sm") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="deptcode" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill"
                                    FooterStyle-CssClass="hiddenbill" />
                                <asp:BoundField DataField="kmbh" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill"
                                    FooterStyle-CssClass="hiddenbill" />
                                <asp:BoundField HeaderText="预算金额状态" DataField="status" ItemStyle-Width="100" HeaderStyle-Width="100"
                                    FooterStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="附件" DataField="Attachment" ItemStyle-Width="100" HeaderStyle-Width="100"
                                    FooterStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="true" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        parent.closeAlert('UploadChoose');
    </script>
</body>
</html>
