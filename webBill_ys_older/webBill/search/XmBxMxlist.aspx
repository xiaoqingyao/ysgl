<%@ Page Language="C#" AutoEventWireup="true" CodeFile="XmBxMxlist.aspx.cs" Inherits="webBill_search_XmBxMxlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <style type="text/css">
        .NoBreak
        {
            white-space: pre;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script type="text/javascript" language="javascript" charset="utf-8">
        $(function() {
            $("#txt_beg").datepicker();
            $("#txt_end").datepicker();
        });
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label runat="server" ID="lbmasge" ForeColor="Red"></asp:Label>
    </div>
    <div>
        <asp:GridView ID="GridView1" runat="server" CssClass="myGrid" AutoGenerateColumns="False"
            Width="100%" OnRowDataBound="GridView1_RowDataBound" ShowFooter="true">
            <Columns>
                <asp:BoundField DataField="fykmName" HeaderText="费用科目" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="je" HeaderText="金额" ItemStyle-CssClass="myGridItem" ItemStyle-Wrap="true" DataFormatString="{0:N}"/>
                <asp:BoundField DataField="billName" HeaderText="单据编号[查]" ItemStyle-CssClass="myGridItem"
                    ItemStyle-Wrap="true" />
                <asp:BoundField DataField="billCode" HeaderText="单据编号" ItemStyle-CssClass="hiddenbill"
                    HeaderStyle-CssClass="hiddenbill" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="deptcodename" HeaderText="部门" ItemStyle-CssClass="myGridItem"
                    ItemStyle-Wrap="true" />
                <asp:BoundField DataField="xmname" HeaderText="项目" ItemStyle-CssClass="myGridItem"
                    ItemStyle-Wrap="true" />
                     <asp:BoundField DataField="flowidname" HeaderText="报销类型" ItemStyle-CssClass="myGridItem"
                    ItemStyle-Wrap="true" />
            </Columns>
            <HeaderStyle CssClass="myGridHeader" />
            <RowStyle CssClass="myGridItem" />
            <FooterStyle CssClass="myGridItem />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
