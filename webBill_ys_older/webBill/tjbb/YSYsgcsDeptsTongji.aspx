<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YSYsgcsDeptsTongji.aspx.cs"
    Inherits="webBill_tjbb_YSYsgcsDeptsTongji" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>部门多选部门多选页（查询一）</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery.multiselect.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.multiselect.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
        $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                parent.helptoggle();
                }
            });
     
            $("#drpDept").multiselect({
                header: false,
                noneSelectedText: "请选择一个部门",
                selectedText: function(numChecked, numTotal, checkedItems) {
                    return numChecked + '个部门被选中了';
                }
            });
            
               $("#ddlYsgc").multiselect({
                header: false,
                noneSelectedText: "请选择预算过程",
                selectedText: function(numChecked, numTotal, checkedItems) {
                    return numChecked + '个预算过程被选中了';
                }
            });
        });
        
        
         function toSearch() {
            var deptstr = "";
            var ysgcstr="";
            $("#drpDept option:selected").each(function(){
         deptstr+=$(this).val()+"|";
            });
              $("#ddlYsgc option:selected").each(function(){
         ysgcstr+=$(this).val()+"|";
            });
        //$("input[name='multiselect_drpDept']:checked").each(function() {
        // deptstr += $(this).val() + "|";  });
            if (deptstr == "") {
                alert("请选择单位");
                return false;
            } else {
                deptstr = deptstr.substring(0, deptstr.length - 1);
                $("#hf_dept").val(deptstr);
            }
            
             if (ysgcstr == "") {
                alert("请选择预算过程");
                return false;
            } else {
                ysgcstr = ysgcstr.substring(0, ysgcstr.length - 1);
                $("#hf_ysgc").val(ysgcstr);
                return true;
            }
        }
        
        
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div style="width: 400px; margin: 0 auto;">
        <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
            <tr>
                <td style="height: 125px; text-align: center">
                </td>
            </tr>
            <tr>
                <td style="text-align: center; height: 27px;">
                    <strong><span style="font-size: 12pt">部门预算过程统计</span></strong>
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable">
                        <tr>
                            <td class="tableBg">
                                年度
                            </td>
                            <td colspan="2" style="width: 257px">
                                <asp:DropDownList ID="ddlNd" runat="server" OnSelectedIndexChanged="ddlNd_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                统计单位
                            </td>
                            <td colspan="2" style="width: 257px">
                                <asp:DropDownList ID="drpDept" runat="server" multiple="multiple" >
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                预算过程
                            </td>
                            <td colspan="2" style="width: 257px">
                                    <asp:DropDownList ID="ddlYsgc" runat="server" multiple="multiple">
                                    </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center">
                    <asp:Button ID="Button1" runat="server" CssClass="baseButton" OnClick="Button1_Click"
                        Text="生成统计表" OnClientClick="return toSearch();" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                    <asp:HiddenField ID="hf_dept" runat="server" />
                    <asp:HiddenField ID="hf_ysgc" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
