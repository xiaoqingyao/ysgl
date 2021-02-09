<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jsczqxList2.aspx.cs" Inherits="webBill_xtsz_jsczqxList2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
         $("#<%=GridView1.ClientID%> tr").removeClass("highlight");
         $(this).addClass("highlight");
     });
    });
 function submitData(oCheckbox) {
     var code = oCheckbox.name.substr(13, 2);
     var gvList = document.getElementById("<%=GridView1.ClientID %>");
       var yj;
       yj = gvList.rows[code - 2].cells[1].innerText;
       for (i = 1; i < gvList.rows.length; i++) {
           if (gvList.rows[i].cells[1].innerText > 2) {
               if (gvList.rows[i].cells[1].innerText.substr(0, 2) == yj) {
                   gvList.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
               }
           }
       }
   }
   function SelectAll(aControl) {
       var chk = document.getElementById("GridView1").getElementsByTagName("input");
       for (var s = 0; s < chk.length; s++) {
           chk[s].checked = aControl.checked;
       }
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
      document.getElementById("header").appendChild(t2);
      var mainwidth = document.getElementById("main").style.width;
      mainwidth = mainwidth.substring(0, mainwidth.length - 2);
      mainwidth = mainwidth - 16;
      document.getElementById("header").style.width = mainwidth;
  }


    </script>

</head>
<body style="text-align: left" onload="gudingbiaotou();">
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                        &nbsp;<asp:Button ID="Button1" runat="server" Font-Size="9pt" Text="保存设置" CssClass="baseButton"
                            OnClick="Button1_Click" />
                        &nbsp; 
                        <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="header">
                        </div>
                        <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 800px; height: 420px;">
                            <asp:GridView EmptyDataText="暂时没有数据" ID="GridView1" runat="server" AllowSorting="True"
                                AutoGenerateColumns="False" CellPadding="0" Font-Size="9pt" BackColor="White"
                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" OnRowDataBound="GridView1_RowDataBound"
                                CssClass="myGrid" Style="table-layout: fixed" Width="100%">
                                <HeaderStyle CssClass=" myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <RowStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" Height="30" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="45" HeaderStyle-Width="45">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                                Text="全选" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="false" onclick="javascript:submitData(this);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="menuid" HeaderText="菜单编号" ItemStyle-Wrap="true" ItemStyle-Width="90" HeaderStyle-Width="90" />
                                    <asp:BoundField DataField="menuname" HeaderText="菜单名称" HtmlEncode="False" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="menusm" HeaderText="菜单说明" ItemStyle-Wrap="true" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <script type="text/javascript">
            parent.parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
