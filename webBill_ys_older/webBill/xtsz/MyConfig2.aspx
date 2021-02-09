<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyConfig2.aspx.cs" Inherits="webBill_xtsz_MyConfig2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
     $(function(){
      $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                  parent.helptoggle();
                }
            });
            
            $(".item").mouseover(function(e){
             var c=  $("#rdbopenHelp").get(0).checked;
           if(c)
           {
                var xx = e.originalEvent.x || e.originalEvent.layerX || 0; 
                var yy = e.originalEvent.y || e.originalEvent.layerY || 0; 
                $('#item-help').css({top: yy, left: xx});//注意这是用css的top和left属性来控制div的。
                var type=$(this).find("input[type='hidden']").val();
                $.post("../MyAjax/GetConfigHelp.ashx",{type:type},function(data,status){
                    if(status=="success"&&data!="")
                    {
                     $("#item-help").html(data);
                     $("#item-help").show();
                    }
                });
               
             
            }
            }).mouseout(function(){
            $("#item-help").hide();
            });
     });
      
        
        
         //刷新页面     
         function Refresh() {
             location.replace(location.href);
         }

     function CloseWithParam(file, fileName, type) {  
      if (type == "logo") {
                document.getElementById("<%=lb0.ClientID %>").innerText = file;
                alert("上传成功！");
                $("#f0").val(file);
                $("fj0").val(fileName);

            }       
            if (type == "f1") {
                document.getElementById("<%=lb1.ClientID %>").innerText = file;
                alert("上传成功！");
                $("#f1").val(file);
                $("fj1").val(fileName);
            }
            
             if (type == "f2") {
                document.getElementById("<%=lb2.ClientID %>").innerText = file;
                alert("上传成功！");
                $("#f2").val(file);
                $("fj2").val(fileName);

            }
             if (type == "f3") {
                document.getElementById("<%=lb3.ClientID %>").innerText = file;
                alert("上传成功！");
                $("#f3").val(file);
                $("fj3").val(fileName);

            }
        }

        
        
      
    </script>

    <style>
        *
        {
            font-size: 13px;
        }
        .item
        {
            width: 600px;
            line-height: 25px;
            height: 25px;
            display: block;
            float: left;
            border: 1px solid LightGray;
            background-color: LightYellow;
            margin: 5px;
        }
        .item-l
        {
            display: block;
            width: 70%;
            float: left;
            font-weight:400px;
        }
        .item-r
        {
            display: block;
            width: 28%;
            float: left;
        }
        .item table
        {
            background-color: #E0EEE0;
            border: 1px solid black;
        }
        .item-help
        {
            border: 2px solid blue;
            height: auto;
            width: 300px;
            position: absolute;
            overflow-y: auto;
            font-size: 14px;
            padding: 15px;
            background-color: White;
        }
        .title
        {
            margin: 0px;
            padding: 0px;
            font-family: 宋体;
            font-family: 900;
            display: block;
            text-align: left;
            line-height: 30px;
            height: 30px;
            margin: 0px;
            padding: 0px;
            display: block;
            widows: 300px;
            font-size: 16px;
        }
        .main-container
        {
            margin: 5px;
        }
        .item2
        {
            width: 98%;
            height: auto;
            background-color: LightYellow;
        }
        .item-l2
        {
            width: 300px;
        }
        .isopen
        {
            display: inline;
            font-family: 500;
            padding: 5px;
             padding-left: 5px;
            border: 1px solid LightGray;
            background-color: LightYellow;
            margin:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 10px;">    
            <input id="btn_reFresh" type="button" value="刷  新" class="baseButton" onclick="Refresh();" />
            <asp:Button ID="btn_save" runat="server" Text="保  存" CssClass="baseButton" OnClick="btn_save_Click" />
            <input type="button" class="baseButton" value="帮助" onclick="javascript:parent.helptoggle();" />
    </div>
   <div class="isopen">
        设置助手:
        <input id="rdbopenHelp" type="radio" name="idopen"  />打开
        <input id="rdbcloseHelp" type="radio" name="idopen" checked="checked"/>关闭
    </div><label  style="color:Red">友情提示:关闭设置助手后，鼠标移到配置项上将不再显示设置提示信息</label>
    <div class="main-container">
        <h3 class="title">
            启用设置</h3>
        <div class="item">
            <input type="hidden" value="HasBudgetControl" />
            <div class="item-l">
                是否启用预算管理模块
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdb_BudgetControl_yes" runat="server" Checked="true" Text="需要"
                    GroupName="IsNeedBudgetControl" /><asp:RadioButton ID="rdb_BudgetControl_no" runat="server"
                        Text="不需要" GroupName="IsNeedBudgetControl" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="UseGKFJ" />
            <div class="item-l">
                是否启用归口继续分解
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdbIsGoOnGkYes" runat="server" Checked="true" Text="需要" GroupName="IsGoOnGk" /><asp:RadioButton
                    ID="rdbIsGoOnGkNo" runat="server" Text="不需要" GroupName="IsGoOnGk" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="TaxSwitch" />
            <div class="item-l">
                一般报销单税额是否进总金额
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdbIsInAllYes" runat="server" Checked="true" Text="是" GroupName="IsInAll" /><asp:RadioButton
                    ID="rdbIsInAllNo" runat="server" Text="否" GroupName="IsInAll" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="pingzhengbygkorsy" />
            <div class="item-l">
                凭证是记到使用部门上 还是归口部门上
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdbBM" runat="server" Checked="true" Text="使用部门" GroupName="GkOrShiYong" /><asp:RadioButton
                    ID="rdbGK" runat="server" Text="归口部门" GroupName="GkOrShiYong" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="ISControlPoint" />
            <div class="item-l">
                是否控制点数 1为控制 0或不设置都是不控制
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdbIsCPYes" runat="server" Checked="true" Text="是" GroupName="IsControlPoint" /><asp:RadioButton
                    ID="rdbIsCPNo" runat="server" Text="否" GroupName="IsControlPoint" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="HasDog" />
            <div class="item-l">
                是否检测到狗
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdbCheckDogYes" runat="server" Checked="true" Text="是" GroupName="IsCheckDog" /><asp:RadioButton
                    ID="rdbCheckDogNo" runat="server" Text="否" GroupName="IsCheckDog" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="AllowTzUodoYs" />
            <div class="item-l">
                预算调整时目标预算过程必须是已开启预算
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdbAllowTzUodoYes" runat="server" Checked="true" Text="是" GroupName="AllowTzUodo" /><asp:RadioButton
                    ID="rdbAllowTzUodoNo" runat="server" Text="否" GroupName="AllowTzUodo" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="HsDeptIsLast" />
            <div class="item-l">
                核算部门是否必须是末级部门
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdbHsDeptIsLastYes" runat="server" Text="是" GroupName="HsDeptIsLast" />
                <asp:RadioButton ID="rdbHsDeptIsLastNo" runat="server" Text="否" GroupName="HsDeptIsLast"
                    Checked="true" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="YbbxHsxmMode" />
            <div class="item-l">
                报销单核算项目模式
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdbHsxmLowModel" runat="server" Text="简单" GroupName="YbbxHsxmMode" />
                <asp:RadioButton ID="rdbHsxmHighModel" runat="server" Text="高级" GroupName="YbbxHsxmMode"
                    Checked="true" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="HasBGSQ" />
            <div class="item-l">
                是否启用报告申请单模块
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdb_BGSQ_yes" runat="server" Checked="true" Text="需要" GroupName="IsNeedBGSQ" />
                <asp:RadioButton ID="rdb_BGSQ_no" runat="server" Text="不需要" GroupName="IsNeedBGSQ" /></div>
        </div> 
    </div>
    <div style="clear: both;">
    </div>
    <div class="main-container">
        <h4 class="title">
            费用申请启用设置</h4>
        <div class="item">
            <input type="hidden" value="HasCGSP" />
            <div class="item-l">
                是否启用采购审批单模块
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdb_CGSP_yes" runat="server" Checked="true" Text="需要" GroupName="IsNeedCGSP" />
                <asp:RadioButton ID="rdb_CGSP_no" runat="server" Text="不需要" GroupName="IsNeedCGSP" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="HasXMZF" />
            <div class="item-l">
                是否启用项目支付申请单模块
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdb_XMZF_yes" runat="server" Checked="true" Text="需要" GroupName="IsNeedXMZF" />
                <asp:RadioButton ID="rdb_XMZF_no" runat="server" Text="不需要" GroupName="IsNeedXMZF" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="HasCCSQ" />
            <div class="item-l">
                是否启用出差申请单模块
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdb_CCSQ_yes" runat="server" Checked="true" Text="需要" GroupName="IsNeedCCSQ" />
                <asp:RadioButton ID="rdb_CCSQ_no" runat="server" Text="不需要" GroupName="IsNeedCCSQ" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="HasSaleRebate" />
            <div class="item-l">
                是否启用费用提成该模块
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdbIsSaleRebateYes" runat="server" Checked="true" Text="需要"
                    GroupName="IsSaleRebate" />
                <asp:RadioButton ID="rdbIsSaleRebateNo" runat="server" Text="不需要" GroupName="IsSaleRebate" /></div>
        </div>
        <div class="item">
            <input type="hidden" value="IsRollCtrl" />
            <div class="item-l">
                是否开启滚动费用控制
            </div>
            <div class="item-r">
                <asp:RadioButton ID="rdbRollNo" runat="server" Checked="true" Text="否" GroupName="IsRoll" />
                <asp:RadioButton ID="rdbRollYes" runat="server" Text="是" GroupName="IsRoll" /></div>
        </div>
        
          <div class="item">
            <input type="hidden" value="HasBGSQ" />
            <div class="item-l">
                一般报销单的报销说明是否是必填项
            </div>
            <div class="item-r">
                <asp:RadioButton ID="IsYbbxSmMustYes" runat="server" Text="是" GroupName="IsYbbxSmMust" />
                <asp:RadioButton ID="IsYbbxSmMustNo" runat="server" Text="否" GroupName="IsYbbxSmMust"  Checked="true"/></div>
        </div>
         <div class="item">
            <input type="hidden" value="HasBGSQ" />
            <div class="item-l">
                报销摘要是否作为凭证摘要
            </div>
            <div class="item-r">
                <asp:RadioButton ID="BxzyAsPzzyYes" runat="server"  Text="是" GroupName="BxzyAsPzzy" />
                <asp:RadioButton ID="BxzyAsPzzyNo" runat="server" Text="否" GroupName="BxzyAsPzzy" Checked="true"/></div>
        </div>
    </div>
    <div style="clear: both;">
    </div>
    <div class="main-container">
        <h4 class="title">
            地址配置
        </h4>
        <div class="item item2">
            <input type="hidden" value="EnterForBxURL" />
            <div class="item-l item-l2">
                在桌面上回车 打开的报销单的地址
            </div>
            <div class="item-r ">
                <asp:TextBox ID="txtEnterForBxURL" runat="server" CssClass="baseText" Width="500px"></asp:TextBox></div>
        </div>
        <div class="item item2">
            <input type="hidden" value="ToNcURL" />
            <div class="item-l item-l2">
                nc系统的平台路径
            </div>
            <div class="item-r">
                <asp:TextBox ID="txtToNcURL" runat="server" CssClass="baseText" Width="500px"></asp:TextBox></div>
        </div>
        <div class="item item2">
            <input type="hidden" value="pingzhengdetailurl" />
            <div class="item-l item-l2">
                凭证制作页面的地址
            </div>
            <div class="item-r ">
                <asp:TextBox ID="txtpingzhengdetailurl" runat="server" CssClass="baseText" Width="500px"></asp:TextBox></div>
        </div>
        <div class="item item2">
            <input type="hidden" value="pingzhengdblinkname" />
            <div class="item-l item-l2">
                凭证数据库连接名称
            </div>
            <div class="item-r">
                <asp:TextBox ID="txtpingzhengdblinkname" runat="server" CssClass="baseText" Width="500px"></asp:TextBox></div>
        </div>
        <div class="item item2">
            <input type="hidden" value="ErrorMsg" />
            <div class="item-l item-l2">
                程序错误提示信息
            </div>
            <div class="item-r">
                <asp:TextBox ID="txt_ErrorMsg" runat="server" CssClass="baseText" Width="500px"></asp:TextBox></div>
        </div>
    </div>     
     <div style="clear: both;">
    </div>
    <div class="main-container">
        <h4 class="title">
            用款单配置
        </h4>
        <div class="item item2">
            <input type="hidden" value="ykspIP" />
            <div class="item-l item-l2">
                 用款审批远程数据库地址
            </div>
            <div class="item-r ">
                <asp:TextBox ID="ykspIP" runat="server" CssClass="baseText" Width="500px"></asp:TextBox></div>
        </div>
        <div class="item item2">
            <input type="hidden" value="ykspDbName" />
            <div class="item-l item-l2">
                用款审批数据库名
            </div>
            <div class="item-r">
                <asp:TextBox ID="ykspDbName" runat="server" CssClass="baseText" Width="500px"></asp:TextBox></div>
        </div>
        <div class="item item2">
            <input type="hidden" value="ykspStructureName" />
            <div class="item-l item-l2">
               用款审批架构名
            </div>
            <div class="item-r ">
                <asp:TextBox ID="ykspStructureName" runat="server" CssClass="baseText" Width="500px"></asp:TextBox></div>
        </div>
        <div class="item item2">
            <input type="hidden" value="ykspTbName" />
            <div class="item-l item-l2">
                 用款审批-主表表名
            </div>
            <div class="item-r">
                <asp:TextBox ID="ykspTbName" runat="server" CssClass="baseText" Width="500px"></asp:TextBox></div>
        </div>
          <div class="item item2">
            <input type="hidden" value="ykspTbName" />
            <div class="item-l item-l2">
                 用款审批-子表表名
            </div>
            <div class="item-r">
                <asp:TextBox ID="ykspTbNames" runat="server" CssClass="baseText" Width="500px"></asp:TextBox></div>
        </div>
    </div>
    
    <div style="clear: both;">
    </div>
    <div class="main-container">
        <h4 class="title">
            系统显示信息配置</h4>
        <div class="item item2">
            <input type="hidden" value="CompanyName" />
            <div class="item-l item-l2">
                企业名称：
            </div>
            <div class="item-r ">
                <asp:TextBox ID="txt_CompanyName" runat="server" CssClass="baseText" Width="500px"></asp:TextBox></div>
        </div>
        <div class="item item2">
            <input type="hidden" value="CompanyLogo" />
            <div class="item-l item-l2">
                企业Logo：
            </div>
            <div class="item-r " runat="server" id="td3" colspan="5" style="height: 100%" valign="top">
                <iframe id="Iframe4" name="addPicture" src="../../SaleBill/Upload/Upload.aspx?Type=logo&UseName=false"
                    width="98%" runat="server" scrolling="no" height="30px" frameborder="0" style="border: 0px solid #f0f0f0;">
                </iframe>
                <asp:Label ID="lb0" runat="server" Text="" Width="100%"></asp:Label>
                <asp:Label runat="server" ID="lbfj0" />
                <asp:HiddenField ID="f0" runat="server" />
                <asp:HiddenField ID="fj0" runat="server" />
            </div>
        </div>
        <div class="item item2">
            <input type="hidden" value="LoginImg11" />
            <div class="item-l item-l2">
                登录页面图片一：
            </div>
            <div class="item-r " runat="server" id="tdiframe" style="height: 100%" valign="top">
                <iframe id="Iframe2" name="addPicture" src="../../SaleBill/Upload/Upload.aspx?Type=f1&UseName=false"
                    width="98%" runat="server" scrolling="no" height="30px" frameborder="0" style="border: 0px solid #f0f0f0;">
                </iframe>
                <asp:Label ID="lb1" runat="server" Text="" Width="100%"></asp:Label>
                <asp:Label runat="server" ID="lbfj1" />
                <asp:HiddenField ID="f1" runat="server" />
                <asp:HiddenField ID="fj1" runat="server" />
            </div>
        </div>
        <div class="item item2">
            <input type="hidden" value="LoginImg12" />
            <div class="item-l item-l2">
                登录页面图片二：
            </div>
            <div class="item-r " runat="server" id="td1" style="height: 100%" valign="top">
                <iframe id="Iframe1" name="addPicture" src="../../SaleBill/Upload/Upload.aspx?Type=f2&UseName=false"
                    width="98%" runat="server" scrolling="no" height="30px" frameborder="0" style="border: 0px solid #f0f0f0;">
                </iframe>
                <asp:Label ID="lb2" runat="server" Text="" Width="100%"></asp:Label>
                <asp:Label runat="server" ID="lbfj2" />
                <asp:HiddenField ID="f2" runat="server" />
                <asp:HiddenField ID="fj2" runat="server" />
            </div>
        </div>
        <div class="item item2">
            <input type="hidden" value="LoginImg13" />
            <div class="item-l item-l2">
                登录页面图片三：
            </div>
            <div class="item-r " runat="server" id="td2" style="height: 100%" valign="top">
                <iframe id="Iframe3" name="addPicture" src="../../SaleBill/Upload/Upload.aspx?Type=f3&UseName=false"
                    width="98%" runat="server" scrolling="no" height="30px" frameborder="0" style="border: 0px solid #f0f0f0;">
                </iframe>
                <asp:Label ID="lb3" runat="server" Text="" Width="100%"></asp:Label>
                <asp:Label runat="server" ID="lbfj3" />
                <asp:HiddenField ID="f3" runat="server" />
                <asp:HiddenField ID="fj3" runat="server" />
            </div>
        </div>
    </div>
    <div class="item-help" id="item-help" style="display: none;">
    </div>
    </form>
    <script type="text/javascript">
        parent.closeAlert('UploadChoose');
        </script>
</body>
</html>
