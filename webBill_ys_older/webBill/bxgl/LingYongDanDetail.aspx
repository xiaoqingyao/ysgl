<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LingYongDanDetail.aspx.cs"
    Inherits="webBill_bxgl_LingYongDanDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
    </style>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">
    
    $(function(){
    
      $("#btnAddFykm").click(function(){
          openKm();
      });
    });
    
      function openKm(deptCode, isGk) {
            var kmcode = "";
            var kmArray = new Array();
            $("#tab_fykm tbody tr td:nth-child(1)").each(function(i) {
                kmArray[i] = $(this).html();
            });
            kmcode = kmArray.join();
            var str = window.showModalDialog("YskmSelectNew.aspx?isgk=true&kmcode=" + kmcode+"&dydj=lyd", 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var json = $.parseJSON(str);
                var inner = "";
                var checkArray = new Array();
                for (var i = 0; i < json.length; i++) {
                    if (kmcode.indexOf(json[i].Yscode) < 0) {
                        inner += '<tr >';
                        inner += "<td>" + json[i].Yscode + "</td>";
                        inner += '<td><input type="text" class="baseText ysje" onblur="htjeChange();" value="0.00"  onkeyup="CheckNAN(this)" /></td>';
                        inner += "</tr>";                 
                    }
                }
                $("#tab_fykm tbody").html(inner);
            }
        }
        
        
         function htjeChange() {
            var je = 0;
            $(".ysje").each(function() {
                je += Number($(this).val()) * 100;
            });
            $(".ysse").each(function() {
                je += Number($(this).val()) * 100;
            });
            je = je.toFixed();
            $("#txtHjjeXx").val(je / 100);
          //  $("#txtHjjeDx").val(cmycurd($("#txtHjjeXx").val()));
        }
        
        
        function check()
        {
          var sum=0;
          var kmcode="";
            $("#tab_fykm tbody tr").each(function(){
                val=$(this).find("input").val();
                if(parseFloat(val)>0)
                {
                sum+=parseFloat( val);
                kmcode+=SubCode($(this).find("td:eq(0)").html())+"|"+val+",";
                }
            });

            if(kmcode.length==0)
            {
            alert("请选择归口费用科目");
            return false;
            }
            else if(sum==0)
            {
               alert("请填写金额");
               return false;
            }
            else{  $("#hfyskms").val(kmcode.substring(0),kmcode.length-1);
            return true;
            }
           
        }
        
        
        function CheckNAN(obj) {
            if (parseFloat($(obj).val()).toString() == "NaN")
                $(obj).val("");
        }
        
         $(function(){
         
         $("#txtBxr").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function(data, status) {
                        if (status == "success") {
                            $("#txtDept").val(data);
                        }
                        else {
                            alert("获取部门失败");
                        }
                    });
                }
            });
            $("#txtLysj").datepicker();
         });

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
            <tr>
                <td style="text-align: center; height: 36px;">
                    <strong>领用单 </strong>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="margin: 0 auto; width: 98%;">
                        <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="100%">
                            <tr>
                                <td colspan="2" class="tableBg2" style="width: 80px">
                                    制单人
                                </td>
                                <td colspan="2" style="width: 200px">
                                    <asp:TextBox ID="txtJbr" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="tableBg2">
                                    领用人
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtBxr" runat="server" Width="148px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    领用部门
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDept" runat="server" ReadOnly=true></asp:TextBox>
                           <input id="txtbxdept"  readonly="readonly" style="display: none; width: 1px" type="text" runat="server" />
                                </td>
                                <td class="tableBg2">
                                    领用时间
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtLysj" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    领用说明
                                </td>
                                <td colspan="10">
                                    <asp:TextBox ID="txtBxsm" runat="server" Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    备注
                                </td>
                                <td colspan="10">
                                    <asp:TextBox ID="txtBz" runat="server" TextMode="MultiLine" Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    选择归口费用
                                </td>
                                <td colspan="7">
                                    <input type="button" value="选择归口费用" id="btnAddFykm" runat="server" class="baseButton" />
                                </td>
                            </tr>
                            <tr class="fykmRow">
                                <td colspan="2" class="tableBg2">
                                    合计金额小写
                                </td>
                                <td colspan="7" style="width: 200px">
                                    <input type="text" id="txtHjjeXx" runat="server" readonly="readonly" style="width: 226px;
                                        text-align: right; background-color: #cccccc;" />
                                </td>
                               <%--  <td colspan="5" class="tableBg2" style="width: 200px">
                                    合计金额大写
                                </td>
                               <td colspan="3">
                                    <input type="text" id="txtHjjeDx" runat="server" readonly="readonly" style="background-color: #cccccc" />
                                </td>--%>
                            </tr>
                            <tr>
                                <td colspan="2" class="tableBg2">
                                    费用科目
                                </td>
                                <td colspan="7">
                                    <table id="tab_fykm" class="myTable" style="width: 95%">
                                        <thead class="myGridHeader">
                                            <tr>
                                                <th>
                                                    科目
                                                </th>
                                                <th>
                                                    金额
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody id="body_fykm" runat="server">
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; height: 30px;">
                    <asp:Button ID="btn_save" runat="server" Text="保 存" OnClientClick= "return check()" OnClick="btn_save_click" />
                    <input type="button" onclick="javascript:window.close();" value="关 闭" class="baseButton" />&nbsp;
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hfyskms" runat="server"  Value='' />
    </div>
    </form>
</body>
</html>
