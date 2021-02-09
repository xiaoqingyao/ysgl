<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cwtbhz_List.aspx.cs" Inherits="webBill_ysglnew_Cwtbhz_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <style type="text/css">
        .unEdit {
            background-color: #dedede;
        }
    </style>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#btn_fh").click(function () {
                var dept = '<%=Request["deptCode"]%>';
                var nian = '<%=Request["nian"]%>';
                window.history.go(-1);
                //window.location.href = "cwtbDetail.aspx?deptCode=" + dept + "&nd=" + nian;
            });
            $("#btn_zt").click(function () {

                var deptcode = '<%=Request["deptCode"]%>';

                var nd = '<%=Request["nian"]%>';


                window.showModalDialog("gkdept_zt_list.aspx?deptcode=" + deptcode + "&nian=" + nd, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:500px;status:no;scroll:yes');
                //openDetail("gkdept_zt_list.aspx?deptcode=" + deptcode + "&nian=" + nd);
            });

        });

        //function EnbleTxt() {

        //    $("body td[class='unEdit'] input").attr("readonly", "readonly");
        //    $("body tr[class='unEdit'] td input").attr("readonly", "readonly");

        //}
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <center>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_masege" runat="server" Font-Size="18"></asp:Label>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <div>
            <input type="button" id="btn_fh" value="返  回" class="baseButton" />
            <input type="button" id="btn_zt" value="查看分校预算状态" class="baseButton" />
            <asp:Button ID="btn_excel" runat="server" Text="导出excel" CssClass="baseButton" OnClick="btn_excel_Click" />
        </div>
        <div style="margin-top: 5px; margin-left: 5px">
            <span style="color: red">【友情提示】：“分校汇总数”为分校填报的并审批通过了的预算填报金额汇总。</span>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" ShowFooter="true" CssClass="myGrid" OnRowDataBound="GridView1_RowDataBound"
                Width="2300px" OnRowCreated="GridView1_RowCreated">
                <Columns>
                    <asp:BoundField HeaderText="序号" DataField="xh" ItemStyle-Width="32" HeaderStyle-Width="30" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem" />
                    <asp:BoundField DataField="km" HeaderText="月份\科目" HtmlEncode="false" ItemStyle-Width="100"
                        HeaderStyle-Width="100" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem" />
                    <asp:BoundField DataField="year" HeaderText="年预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="yearYsnZj" HeaderText="归口部门年预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="January" HeaderText="一月份预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="JanuaryYsnZj" HeaderText="归口部门一月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="February" HeaderText="二月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="FebruaryYsnZj" HeaderText="归口部门二月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="march" HeaderText="三月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="marchYsnZj" HeaderText="归口部门三月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="April" HeaderText="四月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="AprilYsnZj" HeaderText="归口部门四月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="May" HeaderText="五月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="MayYsnZj" HeaderText="归口部门五月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="June" HeaderText="六月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="JuneYsnZj" HeaderText="归口部门六月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="July" HeaderText="七月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="JulyYsnZj" HeaderText="归口部门七月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="August" HeaderText="八月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="AugustYsnZj" HeaderText="归口部门八月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="September" HeaderText="九月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="SeptemberYsnZj" HeaderText="归口部门九月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="October" HeaderText="十月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="OctoberYsnZj" HeaderText="归口部门十月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="November" HeaderText="十一月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="NovemberYsnZj" HeaderText="归口部门十一预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="December" HeaderText="十二月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="DecemberYsnZj" HeaderText="归口部门十二月预算" HtmlEncode="false" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:TemplateField ItemStyle-CssClass="basehidden" HeaderStyle-CssClass="basehidden" FooterStyle-CssClass="basehidden">
                        <ItemTemplate>
                            <asp:HiddenField ID="HiddenKmbh" runat="server" Value='<%#Eval("kmbh") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
