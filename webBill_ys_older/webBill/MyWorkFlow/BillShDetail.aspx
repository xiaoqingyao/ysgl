<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BillShDetail.aspx.cs" Inherits="webBill_MyWorkFlow_BillShDetail" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>审核明细页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" style="text-align: center; margin-top: 5px; margin-left:5px" cellspacing="0" width="100%">
            <tr>
                <td>
                    <div id="divgrid" style="overflow-x: auto; text-align: center">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" AllowSorting="false"
                            AllowPaging="false" CssClass="myGrid" Width="90%" OnRowDataBound="GridView1_RowDataBound">
                            <PagerStyle CssClass="hidden" />
                            <Columns>
                                <asp:TemplateField HeaderText="序号" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1%>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="false" CssClass="myGridHeader hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem hiddenbill" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="flowid" HeaderText="审批类别" ItemStyle-Width="80" HeaderStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" />
                                <asp:BoundField DataField="checkuser" HeaderText="审批人" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem" ItemStyle-Width="100" />
                                <asp:BoundField DataField="wsrdstate" HeaderText="审批状态" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem" ItemStyle-Width="100" />
                                <asp:BoundField DataField="mind" HeaderText="审批意见" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem" ItemStyle-Width="200" />
                                <asp:BoundField DataField="checkdate1" HeaderText="审批时间" HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem" ItemStyle-Width="150" DataFormatString="{0:f}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
