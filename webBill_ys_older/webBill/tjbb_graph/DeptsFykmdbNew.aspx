<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeptsFykmdbNew.aspx.cs" Inherits="webBill_tjbb_graph_DeptsFykmdbNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>各部门同费用科目对比</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../FusionCharts/FusionCharts.js" type="text/javascript"></script>

    <script type="text/javascript">
    
   $(function(){
       
    
        $("#btn_BuildGraph").bind("click",function(){
            var ysgc=$("#txt_ysgc").val().length;
            var dept=$("#txt_dept").val().length;
            var yskm=$("#txt_yskm").val().length;
            if(ysgc==0)
            {
                alert("请选择预算过程");
                return false;
            }
            else if(dept==0)
            {
                alert("请选择部门");
                return false;
            }
            else if(yskm==0)
            {
                alert("请选择预算科目");
                return false;
            }
            else {
                 return true;
            }
        });
        
        //选择预算过程
        $("#btn_ysgc").click(function(){
        var r= $("#hfYsgc").val();
        Open("../select/dbSelectYsgcs.aspx?r="+r,"ysgc");
        });
        
        //选择部门
        $("#btn_dept").click(function(){
        var r= $("#hfDept").val();
        var count=parseFloat($("#hfYskmCount").val());
        var t;
        if(count<=0||count==1)//如果预算过程多选 部门就只能单选
            t="m";
        else 
            t="s";
        Open("../select/dbSelectDepts.aspx?r="+r+"&type="+t,"dept");
        });
        
        //选择预算科目
        $("#btn_yskm").click(function(){
        var r= $("#hfYskm").val();
        var count=parseFloat($("#hfDeptCount").val());
        var t;
        if(count<=0||count==1)//如果部门多选 预算过程就只能单选
            t="m";
        else 
            t="s";
        Open("../select/dbSelectYskm.aspx?r="+r+"&type="+t,"yskm");
        });
        
   });
   
   
    function Check() {

            var chk = $("#hfYsgc").val().length;
            var chk1 = $("#hfDept").val().length;
            var chk2 =$("#hfYskm").val().length;
            if (chk.length == 0) {
                alert("请选择预算过程");
                return false;
            }
            else if (chk1.length == 0) {
                alert("请选择部门");
                return false;
            }
            else if (chk2.length == 0) {
                alert("请选择预算科目");
                return false;
            }
            else {
                return true;
            }
        }



    function Open(openUrl,myKey) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:450px;dialogWidth:400px;status:no;scroll:none');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
              var jsonRel = $.parseJSON(returnValue);
             if(myKey=="ysgc")
            {
            var obj1=$("#hfYsgc");
            var obj2=$("#txt_ysgc");
            var obj3=$("#hfYsgcCount");
              SetValues(jsonRel,obj1,obj2,obj3);
            }
            else if(myKey=="dept")
            {
             var obj1=$("#hfDept");
            var obj2=$("#txt_dept");
            var obj3=$("#hfDeptCount");
            SetValues(jsonRel,obj1,obj2,obj3);
            }
            else if(myKey=="yskm")
            {
            var obj1=$("#hfYskm");
            var obj2=$("#txt_yskm");
            var obj3=$("#hfYskmCount");
            SetValues(jsonRel,obj1,obj2,obj3);
            //var obj1= $("#hfYskm").val(jsonRel.code);
            //$("#txt_yskm").val(jsonRel.name);
            }
            else {}
        }
   
   
       function SetValues(jsonRel,obj1,obj2,obj3)
       {
          var innerString = "";
          var namesstring = "";
          for (var i = 0; i < jsonRel.length; i++) {
                  innerString += jsonRel[i].code + ",";
                  namesstring +=jsonRel[i].name + ",";
             } 
          if (innerString.length-1>0)
           {
              obj1.val(innerString.substring(0, innerString.length - 1));
              obj2.val(namesstring.substring(0, namesstring.length - 1));
              obj3.val(jsonRel.length);
          }     
       }
    </script>

    <style type="text/css">
        .w
        {
            width: 750px;
        }
        .s
        {
            width: 85%;
        }
        .r
        {
            text-align: right;
            width: 80px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="OptionDiv" style="margin: 10px">
        <label style="color: Red">
            友情提示：预算过程可以多选，部门和预算科目不能同时多选。</label>
        <table class="baseTable" style="margin-left: 0px;">
            <tr>
                <td class="r">
                    预算过程:
                </td>
                <td class="w">
                    <input id="txt_ysgc" type="text" runat="server" class="s"  readonly="readonly"/>
                    <asp:HiddenField ID="hfYsgc" runat="server" />
                    <asp:HiddenField ID="hfYsgcCount" runat="server" Value="" />
                    <input id="btn_ysgc" type="button" value="选 择" />
                </td>
            </tr>
            <tr>
                <td class="r">
                    公司部门:
                </td>
                <td class="w">
                    <input id="txt_dept" type="text" runat="server" class="s"  readonly="readonly" />
                    <asp:HiddenField ID="hfDept" runat="server" />
                    <asp:HiddenField ID="hfDeptCount" runat="server" Value="0" />
                    <input id="btn_dept" type="button" value="选 择" />
                </td>
            </tr>
            <tr>
                <td class="r">
                    预算科目:
                </td>
                <td class="w">
                    <input id="txt_yskm" type="text" runat="server" class="s"  readonly="readonly"/>
                    <asp:HiddenField ID="hfYskm" runat="server" />
                    <asp:HiddenField ID="hfYskmCount" runat="server" Value="0" />
                    <input id="btn_yskm" type="button" value="选 择" />
                </td>
            </tr>
            <tr>
                <td class="r">
                    生成图表:
                </td>
                <td class="w">
               
                    <asp:RadioButton ID="rdbzhuxing" runat="server" Text="柱形图" GroupName="tuxingType"
                        Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdbshanxing" runat="server" Text="扇形图" GroupName="tuxingType" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdbzhexian" runat="server" Text="折线图" GroupName="tuxingType" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_BuildGraph" runat="server" Text="生成图表" OnClick="Button1_Click"
                        OnClientClick="return Check()" />
                </td>
            </tr>
        </table>
    </div>
    <div id="DataDiv"  style="margin-left: 0px;">
        <%=FusionHTML %>
    </div>
    </form>
</body>
</html>
