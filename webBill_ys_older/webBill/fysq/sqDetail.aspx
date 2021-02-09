<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sqDetail.aspx.cs" Inherits="fysq_sqDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta   http-equiv= "pragma "   content= "no-cache " /> 
<meta   http-equiv= "Cache-Control "   content= "no-cache,must-revalidate " /> 
<meta   http-equiv= "Expires "   content= "0 " />   
    <title>费用申请详细信息</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
<base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>
    <script language="javascript" type="text/javascript" src="js/Jscript.js"></script>

    <script language="javascript" type="text/javascript">
        //删除附件
        function deleteFj(obj,fjurl,fjGuid)
        {
            if(confirm("确认删除该附件？")==false)
            {
                return;
            }
            
            var returnValue=fysq_sqDetail.DelteFj(fjurl,fjGuid).value;
            if(returnValue==true)
            {
                $(obj).parent().parent().remove();
            }
            else
            {
                alert('附件删除失败！');
            }
        }
        function openDetail(openUrl)
        {
            var returnValue=window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:850px;status:no;scroll:no');
            if(returnValue==undefined||returnValue=="")
            {
                return false;
            }
            else
            {
                document.getElementById("txt_jkr").value=returnValue;
            }
        }
        //是否借款
        function changeJkStatus()
        {
            if($("#rad_sfjks").attr("checked")==true)//借款
            {
                $(".sfjk").each(function()
                {
                    $(this).css("display","");
                });
            }
            else
            {
                $(".sfjk").each(function()
                {
                    $(this).css("display","none");
                });
            }
        }
        //保存判断
        function checkMess()
        { 
             if(document.getElementById("txt_billDate").value=="")
            {
                alert("借款日期不能为空！");
                return false;
            }
            if(document.getElementById("txt_jkr").value=="")
            {
                alert("借款申请人不能为空！");
                return false;
            }
            if(document.getElementById("txt_billJe").value=="")
            {
                alert("借款金额不能为空！");
                return false;
            }
            if(document.getElementById("rad_sfjks").checked==true)
            { 
               var vjk= Conv_sz("txt_billJe");
               var vhj= math_js(new Array("txt_xj","txt_zp","txt_hk"));
               if(vjk!=vhj)
               {
                    alert("借款金额必须等于借款明细合计！");
                    return false;
               }
            }
            
            return true;
        }
    </script>

</head>
<body style="background-color: #EBF2F5;" onload="set_Dxje('txt_billJe','txt_billjeDx');">
    <center>
        <form id="form1" runat="server">
            &nbsp;<table cellpadding="0" cellspacing="0" class="style1" width="100%">
                <tr>
                    <td style="text-align: center; height: 26px;">
                        <strong><span style="font-size: 12pt">费&nbsp; 用&nbsp; 申&nbsp; 请&nbsp; 详&nbsp; 细&nbsp;
                            信&nbsp; 息</span></strong></td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <table border="0" cellpadding="0" cellspacing="0" class="myTable" style="width: 803px">
                            <tr>
                                <td class="tableBg">
                                    经办人</td>
                                <td colspan="2">
                                    <asp:TextBox ID="txt_billUser" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="tableBg">
                                    申请日期</td>
                                <td>
                                    <asp:TextBox ID="txt_billDate" runat="server"  style="width: 164px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">
                                    借款申请人
                                </td>
                                <td colspan="2">
                                    <input id="txt_jkr" runat="server" readonly="readonly" type="text" /><asp:Button
                                        ID="btn_sqr" runat="server" Text="选择" CssClass="baseButton" /></td>
                                <td class="tableBg">
                                    借款登记类型
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_jkdjlx" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">
                                    借款金额
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txt_billJe" runat="server" onblur="clearNoNum(this,'Y');set_Dxje('txt_billJe','txt_billjeDx');"></asp:TextBox>元
                                </td>
                                <td class="tableBg">
                                    申请金额(大写)
                                </td>
                                <td>
                                    <input id="txt_billjeDx" type="text" runat="server" readonly="readonly" style="width: 164px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">
                                    申请摘要
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txt_sqzy" runat="server" TextMode="MultiLine" Width="588px" Rows="2"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg">
                                    申请备注</td>
                                <td colspan="4">
                                    <asp:TextBox ID="txt_sqbz" runat="server" TextMode="MultiLine" Width="588px" Rows="2"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg" rowspan="">
                                    是否借款</td>
                                <td colspan="4">
                                    <asp:RadioButton ID="rad_sfjkf" runat="server" Checked="True" Text="不借款" GroupName="sfjk" />
                                    <asp:RadioButton ID="rad_sfjks" runat="server" Text="借款" GroupName="sfjk" /></td>
                            </tr>
                            <tr class="sfjk" style="display:none;">
                                <td class="tableBg" rowspan="4">
                                    借款明细</td>
                                <td class="tableBg">
                                    现金</td>
                                <td>
                                    <asp:TextBox ID="txt_xj" runat="server" onblur="clearNoNum(this,'Y');"></asp:TextBox></td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr class="sfjk" style="display:none;">
                                <td class="tableBg">
                                    支票</td>
                                <td>
                                    <asp:TextBox ID="txt_zp" runat="server" onblur="clearNoNum(this,'Y');"></asp:TextBox></td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr class="sfjk" style="display:none;">
                                <td class="tableBg">
                                    汇款</td>
                                <td>
                                    <asp:TextBox ID="txt_hk" runat="server" onblur="clearNoNum(this,'Y');"></asp:TextBox></td>
                                <td class="tableBg">
                                    单位名称</td>
                                <td>
                                    <asp:TextBox ID="txt_dwmc" runat="server" Width="200px"></asp:TextBox></td>
                                
                            </tr>
                            <tr class="sfjk" style="display:none;">
                                <td class="tableBg">
                                    开户行</td>
                                <td>
                                    <asp:TextBox ID="txt_khh" runat="server" Width="186px"></asp:TextBox></td>
                                <td class="tableBg">
                                    银行账户</td>
                                <td>
                                    <asp:TextBox ID="txt_yhzh" runat="server" Width="200px"></asp:TextBox></td>
                                
                            </tr>
                            <tr>
                                <td class="tableBg" rowspan="999999999">
                                    费用附件</td>
                                <td colspan="4">
                                    
                                        &nbsp;<asp:FileUpload ID="upLoadFiles" runat="server" CssClass="baseButton" Width="415px" />
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="上传文件" CssClass="baseButton" />
                                    
                                </td>
                            </tr>
                                <div id="fjList" runat="server">
                                </div> 
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: center; height: 37px;">
                        <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="return checkMess(); " OnClick="btn_bc_Click" />
                        &nbsp;
                        <asp:Button ID="btn_fh" runat="server" Text="取 消" CssClass="baseButton" OnClick="btn_fh_Click" />
                    </td>
                </tr>
                <tr style="display:none">
                    <td colspan="6" style="height: 37px; text-align: center">
                        <asp:Label ID="lbl_BillCode" runat="server"></asp:Label></td>
                </tr>
            </table>
        </form>
    </center>
</body>
</html>
