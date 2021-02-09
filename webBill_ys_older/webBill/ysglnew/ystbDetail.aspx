<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ystbDetail.aspx.cs" Inherits="webBill_ysglnew_ystbDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
 <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function() {
            $("#btn_cxysgc").click(function() {
                var Rvnum = openDetail("ystbSelectYsgc.aspx");
                if (Rvnum != undefined && Rvnum != "") {
                    $("#txtysgc").val(Rvnum);
                }
            });
        });
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:yes');
            return returnValue;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="baseDiv" style="margin-top: 3px">
            &nbsp;预算年度：<asp:TextBox ID="txtysgc" runat="server"></asp:TextBox>&nbsp; 
            <input id="btn_cxysgc" type="button" value="选择预算过程"  class="baseButton"/>  &nbsp;
            <asp:Button ID="btn_Tb" runat="server" Text="填报" CssClass="baseButton" OnClick="btn_Tb_Click" />
            &nbsp;
            <asp:Button ID="btn_save" runat="server" Text="保存" CssClass="baseButton" OnClick="btn_save_Click" />
            
            部门：<asp:Label ID="Ladept" runat="server" Text=""></asp:Label>
            <div class="baseDiv">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="baseTable"
                    Width="1400">
                    <Columns>
                     <asp:BoundField  HeaderText="序号" DataField=""/>
                        <asp:BoundField DataField="km" HeaderText="\月份<br/>\<br/> 科目\" HtmlEncode="false" />
                        <%-- <asp:BoundField DataField="January"  HeaderText="一月份<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="一月份<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtJanuary" runat="server" Width="60px" Text='<%#Eval("January") %>'></asp:TextBox>
                                <asp:HiddenField ID="HiddenKmbh" runat="server" Value='<%#Eval("kmbh") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--     <asp:BoundField DataField="February" HeaderText="二月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="二月<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFebruary" runat="server" Width="60px" Text='<%#Eval("February") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--    <asp:BoundField DataField="march" HeaderText="三月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="三月<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtmarch" runat="server" Width="60px" Text='<%#Eval("march") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="" HeaderText="第一季度" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="四月<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtApril" runat="server" Width="60px" Text='<%#Eval("April") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--  <asp:BoundField DataField="May" HeaderText="五月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="五月<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMay" runat="server" Width="60px" Text='<%#Eval("May") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:BoundField DataField="June" HeaderText="六月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="六月<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtJune" runat="server" Width="60px" Text='<%#Eval("June") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="" HeaderText="第二季度" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="七月<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtJuly" runat="server" Width="60px" Text='<%#Eval("July") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--  <asp:BoundField DataField="August" HeaderText="八月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="八月<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAugust" runat="server" Width="60px" Text='<%#Eval("August") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--        <asp:BoundField DataField="September" HeaderText="九月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="九月<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtSeptember" runat="server" Width="60px" Text='<%#Eval("September") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="" HeaderText="第三季度" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="十月<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtOctober" runat="server" Width="60px" Text='<%#Eval("October") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--   <asp:BoundField DataField="November" HeaderText="十一月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="十一月<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtNovember" runat="server" Width="60px" Text='<%#Eval("November") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="十二月<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDecember" runat="server" Width="60px" Text='<%#Eval("December") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="" HeaderText="第四季度" HtmlEncode="false" />
                        <%-- <asp:BoundField DataField="year" HeaderText="年<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="年<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtyear" runat="server" Width="60px" Text='<%#Eval("year") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" CssClass="baseTable"
                    Width="100%">
                    <Columns>
                     <asp:BoundField  HeaderText="序号" DataField=""/>
                        <asp:BoundField DataField="km" HeaderText="\月份<br/>\<br/> 科目\" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="第一季度<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtspring" runat="server" Width="60px" Text='<%#Eval("spring") %>'></asp:TextBox>
                                <asp:HiddenField ID="HiddenKmbh" runat="server" Value='<%#Eval("kmbh") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="第二季度<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtsummer" runat="server" Width="60px" Text='<%#Eval("summer") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="第三季度<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtautumn" runat="server" Width="60px" Text='<%#Eval("autumn") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="第四季度<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtwinter" runat="server" Width="60px" Text='<%#Eval("winter") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="年<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtyear" runat="server" Width="60px" Text='<%#Eval("year") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
