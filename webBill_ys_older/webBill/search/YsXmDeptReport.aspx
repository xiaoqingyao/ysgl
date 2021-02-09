<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YsXmDeptReport.aspx.cs" Inherits="webBill_search_YsXmDeptReport" %>

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
        $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                 parent.helptoggle();
                }
            });
            $("#txt_beg").datepicker();
            $("#txt_end").datepicker();
        });

        function openDetail(begtime, endtime, deptCode, xmcode) {
            window.showModalDialog('XmBxMxlist.aspx?begtime=' + begtime + '&endtime=' + endtime + '&deptcode=' + deptCode + '&xmcode=' + xmcode, 'newwindow', 'center:yes;dialogHeight:470px;dialogWidth:870px;status:no;scroll:yes');
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btn_excel" runat="server" Text="导出excel" CssClass="baseButton" OnClick="btn_excel_Click" />
        开始时间:
        <asp:TextBox ID="txt_beg" runat="server"></asp:TextBox>
        结束时间:
        <asp:TextBox ID="txt_end" runat="server"></asp:TextBox>
        <asp:Button ID="btn_find" runat="server" Text="查询" CssClass="baseButton" OnClick="btn_find_Click" />
          <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
    </div>
    <div>
        <asp:GridView ID="GridView1" runat="server" CssClass="myGrid" OnDataBound="myGrid_ItemDataBound"
            AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound">
            <Columns>
                <asp:BoundField DataField="bmmc" HeaderText="部门[查]" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="xmbh" HeaderText="项目编号[查]" ItemStyle-CssClass="myGridItem"
                    ItemStyle-Wrap="true" />
                <asp:BoundField DataField="xmmc" HeaderText="项目名称" ItemStyle-CssClass="myGridItem"
                    ItemStyle-Wrap="true" />
                <asp:BoundField DataField="ysje" HeaderText="预算金额" ItemStyle-CssClass="myGridItemRight" />
                <asp:BoundField DataField="bxje" HeaderText="报销金额" ItemStyle-CssClass="myGridItemRight" />
            </Columns>
            <HeaderStyle CssClass="myGridHeader" />
            <RowStyle CssClass="myGridItem" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
