<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectyszj.aspx.cs" Inherits="webBill_select_selectyszj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>选择用款申请单</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <base target="_self" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function acheck(obj) {
            var baseTemp = "";
            var eveTemp = "";
            var flg = 1;
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").each(function (s, i) {
                if ($(this).find("input[type='checkbox']").attr("checked")) {
                    eveTemp = $(this).children().eq(1).html();
                    //alert(eveTemp);
                    eveTemp = eveTemp.substring(0, 6);
                    //alert(eveTemp);
                    if (baseTemp == "") {
                        baseTemp = eveTemp;
                    } else {
                        if (baseTemp != eveTemp) {
                            flg = 0;
                        }
                    }
                }
            });
            if (flg == 0) {
                alert("请选择同一月份的请款单");
                $(obj).attr("checked",false);
            }
        }
    </script>
</head>
<body style="background-color: #E4F5FF;">
    <form id="form1" runat="server">
       
        <div style="float: left; width:99%">

            <div style="margin-top: 5px;">
               
                <asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="确 定" OnClick="Button1_Click" />

                <input id="Button2" type="button" value="取 消" class="baseButton" onclick="javascript: window.close();" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false"  OnItemDataBound="myGrid_ItemDataBound" CssClass="baseTable" ShowFooter="false">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="32" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="false" onclick="acheck(this);" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="单据编号" DataField="billname" ItemStyle-Width="80" HeaderStyle-Width="80" />
                        <asp:BoundField HeaderText="制单日期" DataField="billdate" ItemStyle-Width="100" HeaderStyle-Width="100" DataFormatString="{0:D}" />
                        <asp:BoundField HeaderText="部门" DataField="deptname"   ItemStyle-Width="200" HeaderStyle-Width="200"/>
                        <asp:BoundField HeaderText="金额" DataField="billje" ItemStyle-Width="50" HeaderStyle-Width="50" />    
                         <asp:BoundField HeaderText="" DataField="billcode"  ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill" ItemStyle-Width="50" HeaderStyle-Width="50" />   
                         <asp:BoundField HeaderText="项目" DataField="xm"  ItemStyle-Width="50" HeaderStyle-Width="50" /> 
                          <asp:BoundField HeaderText="制单人" DataField="zdr"  ItemStyle-Width="50" HeaderStyle-Width="50" /> 
                        
                    </Columns>
                </asp:GridView>
            </div>

        </div>

    </form>
</body>
</html>
