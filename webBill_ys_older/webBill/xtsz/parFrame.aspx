<%@ Page Language="C#" AutoEventWireup="true" CodeFile="parFrame.aspx.cs" Inherits="webBill_xtsz_parFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script type="text/javascript" language="javascript">
        function RemoveHTML(  ) 
        { 
          
       
            var regEx = /<[^>]*>/g; 
            var strText=document.getElementById("TextBox35").value;
              alert(strText);
          var entstr=  strText.replace(regEx, ""); 
          document.getElementById("TextBox35").value=entstr;
          alert(entstr);
        } 

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" style="width: 100%" border="0">
            <tr>
                <td style="height: 27px; text-align: center">
                    <strong><span style="font-size: 12pt">系 统 运 行 参 数 设 置</span></strong></td>
            </tr>
            <tr>
                <td>
                    <table class="baseTable">
                        <tr>
                            <th style="width: 67px">
                                分类</th>
                            <th style="width: 127px">
                                参数名称</th>
                            <th style="width: 117px">
                                参数值</th>
                            <th style="width: 67px">
                                分类</th>
                            <th style="width: 127px">
                                参数名称</th>
                            <th style="width: 117px">
                                参数值</th>
                            <th style="width: 67px">
                                分类</th>
                            <th style="width: 127px">
                                参数分类</th>
                            <th style="width: 117px">
                                参数值</th>
                        </tr>
                        <tr>
                            <td colspan="3" rowspan="2">
                                重点说明：年度预算、第一季度预算以及1月份预算起止时间,<br />
                                如以01开头,则表示本年填报,否则,按照上年相应日期处理。</td>
                            <td rowspan="12" style="text-align: center">
                                <strong><span style="font-size: 12pt; color: #ff0000">月<br />
                                    <br />
                                    预<br />
                                    <br />
                                    算<br />
                                    <br />
                                    起<br />
                                    <br />
                                    止<br />
                                    <br />
                                    时<br />
                                    <br />
                                    间</span></strong></td>
                            <td style="width: 127px">
                                1月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox11" runat="server" Width="126px"></asp:TextBox></td>
                            <td rowspan="12" style="width: 67px; text-align: center">
                                <span style="font-size: 12pt; color: #ff0000"><strong>月<br />
                                    <br />
                                    预<br />
                                    <br />
                                    算<br />
                                    <br />
                                    起<br />
                                    <br />
                                    止<br />
                                    <br />
                                    时<br />
                                    <br />
                                    间</strong></span></td>
                            <td style="width: 127px">
                                7月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox23" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 127px">
                                1月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox12" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                7月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox24" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="width: 67px; text-align: center">
                                <strong><span style="font-size: 12pt"><span style="color: #ff0000">年度<br />
                                </span><span style="color: #ff0000">预算</span></span></strong></td>
                            <td rowspan="1" style="width: 127px">
                                年度预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox1" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                2月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox13" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                8月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox25" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td rowspan="1" style="width: 127px">
                                年度预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox2" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                2月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox14" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                8月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox26" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td rowspan="8" style="width: 67px; text-align: center;">
                                &nbsp;<span style="font-size: 12pt; color: #ff0000"><strong>季<br />
                                    <br />
                                    度<br />
                                    <br />
                                    预<br />
                                    <br />
                                    算</strong></span></td>
                            <td rowspan="1" style="width: 127px">
                                第1季度预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox3" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                3月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox15" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                9月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox27" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td rowspan="1" style="width: 127px">
                                第1季度预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox4" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                3月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox16" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                9月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox28" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td rowspan="1" style="width: 127px">
                                第2季度预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox5" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                4月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox17" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                10月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox29" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td rowspan="1" style="width: 127px">
                                第2季度预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox6" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                4月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox18" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                10月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox30" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td rowspan="1" style="width: 127px">
                                第3季度预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox7" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                5月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox19" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                11月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox31" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td rowspan="1" style="width: 127px">
                                第3季度预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox8" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                5月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox20" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                11月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox32" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td rowspan="1" style="width: 127px">
                                &nbsp;第4季度预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox9" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                6月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox21" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                12月份预算开始时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox33" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td rowspan="1" style="width: 127px">
                                &nbsp;第4季度预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox10" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                6月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox22" runat="server" Width="126px"></asp:TextBox></td>
                            <td style="width: 127px">
                                12月份预算截止时间</td>
                            <td style="width: 117px">
                                <asp:TextBox ID="TextBox34" runat="server" Width="126px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="9" rowspan="1" style="height: 7px; text-align: center">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" rowspan="1" style="text-align: center">
                                <span style="font-size: 12pt; color: #ff0000"><strong>技术支持提示信息</strong></span></td>
                            <td colspan="7">
                                <asp:TextBox ID="TextBox35" runat="server" Width="814px"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Button ID="Button1" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="RemoveHTML()" OnClick="Button1_Click" /></td>
            </tr>
        </table>
    </form>
</body>
</html>
