<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zcfzb_set.aspx.cs" Inherits="webBill_tjbb_dz_zcfzb_set" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        input {
            color: expression(this.type=="text"? width:500px);
        }

            input[type="text"] {
                width: 100px;
            }
    </style>
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script>
        $(function () {
            $("input[type='button']").attr("class", "baseButton");
        });
        function selectyskm(obj) {
            var str = window.showModalDialog("SelectYskm.aspx", 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str == null || str.length == 0) {
                return;
            }
            var json = $.parseJSON(str);
            var yskm = "";
            var yskmcode = "";
            for (var i = 0; i < json.length; i++) {
                if (json[i].Yscode != "") {
                    yskmcode += json[i].Yscode.substring(1, json[i].Yscode.indexOf("]")) + ",";
                    yskm += json[i].Yscode + ";";
                }

            }
            $(obj).parent().prev().children().eq(0).val(yskmcode);
            $(obj).parent().prev().prev().children().eq(0).val(yskmcode);
        }
        function setZt_Dept() {
            window.showModalDialog("dz_zt_dept_dy.aspx", 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:500px;status:no;scroll:yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="baseTable">
                <tr><td colspan="12">资产负债表设置</td></tr>
                <tr>
                    <td>账套：</td>
                    <td colspan="9">

                        <asp:DropDownList ID="ddlZt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlZt_SelectedIndexChanged">
                        </asp:DropDownList>
                        <input type="button" value="设置账套与部门对应关系"  onclick="setZt_Dept();"/>
                        <asp:Button ID="btn_Save" runat="server" Text="保 存" OnClick="btn_Save_Click" CssClass="baseButton" />
                    </td>
                    <td colspan="2">单位：元</td>

                </tr>
                <tr>
                    <td>资产</td>
                    <td>行次</td>
                    <td>年初数</td>
                    <td>预算期末数</td>
                    <td>实际期末数</td>
                    <td>汇总科目</td>
                    <td>负债和所有者权益</td>
                    <td>行次</td>
                    <td>年初数</td>
                    <td>预算期末数</td>
                    <td>实际期末数</td>
                    <td>汇总科目</td>
                </tr>
                <tr>
                    <td colspan="6">流动资产：</td>
                    <td colspan="6">流动负债：</td>
                </tr>
                <tr>
                    <td>货币资金</td>
                    <td>1</td>
                    <td>
                        <asp:TextBox ID="nc_1" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_1" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_1" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>短期借款</td>
                    <td>33</td>
                    <td>
                        <asp:TextBox ID="nc_33" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_33" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_33" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>短期投资</td>
                    <td>2</td>
                    <td>
                        <asp:TextBox ID="nc_2" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_2" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_2" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>应付票据</td>
                    <td>34</td>
                    <td>
                        <asp:TextBox ID="nc_34" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_34" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_34" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>应收票据</td>
                    <td>3</td>
                    <td>
                        <asp:TextBox ID="nc_3" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_3" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_3" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>应付账款</td>
                    <td>35</td>
                    <td>
                        <asp:TextBox ID="nc_35" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_35" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_35" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>应收股利</td>
                    <td>4</td>
                    <td>
                        <asp:TextBox ID="nc_4" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_4" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_4" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>预收账款</td>
                    <td>36</td>
                    <td>
                        <asp:TextBox ID="nc_36" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_36" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_36" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" />
                    </td>
                </tr>
                <tr>
                    <td>应收利息</td>
                    <td>5</td>
                    <td>
                        <asp:TextBox ID="nc_5" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_5" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_5" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>应付工资</td>
                    <td>37</td>
                    <td>
                        <asp:TextBox ID="nc_37" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_37" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_37" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>应收账款</td>
                    <td>6</td>
                    <td>
                        <asp:TextBox ID="nc_6" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_6" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_6" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>应付福利费</td>
                    <td>38</td>
                    <td>
                        <asp:TextBox ID="nc_38" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_38" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_38" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>其他应收款</td>
                    <td>7</td>
                    <td>
                        <asp:TextBox ID="nc_7" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_7" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_7" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>应付股利</td>
                    <td>39</td>
                    <td>
                        <asp:TextBox ID="nc_39" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_39" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_39" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>预付账款</td>
                    <td>8</td>
                    <td>
                        <asp:TextBox ID="nc_8" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_8" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_8" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>应交税金</td>
                    <td>40</td>
                    <td>
                        <asp:TextBox ID="nc_40" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_40" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_40" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>应收补贴款</td>
                    <td>9</td>
                    <td>
                        <asp:TextBox ID="nc_9" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_9" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_9" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>其它应交款</td>
                    <td>41</td>
                    <td>
                        <asp:TextBox ID="nc_41" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_41" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_41" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>存货</td>
                    <td>10</td>
                    <td>
                        <asp:TextBox ID="nc_10" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_10" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_10" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>其它应付款</td>
                    <td>42</td>
                    <td>
                        <asp:TextBox ID="nc_42" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_42" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_42" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>待摊费用</td>
                    <td>11</td>
                    <td>
                        <asp:TextBox ID="nc_11" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_11" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_11" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>预提费用</td>
                    <td>43</td>
                    <td>
                        <asp:TextBox ID="nc_43" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_43" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_43" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>一年内到期的长期债权投资</td>
                    <td>12</td>
                    <td>
                        <asp:TextBox ID="nc_12" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_12" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_12" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>预计负债</td>
                    <td>44</td>
                    <td>
                        <asp:TextBox ID="nc_44" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_44" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_44" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>其它流动资产</td>
                    <td>13</td>
                    <td>
                        <asp:TextBox ID="nc_13" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_13" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_13" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>一年到内的长期收益</td>
                    <td>45</td>
                    <td>
                        <asp:TextBox ID="nc_45" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_45" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_45" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>流动资产合计</td>
                    <td>14</td>
                     <td>
                        <asp:TextBox ID="nc_14" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_14" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_14" runat="server" /></td>
                    <td></td>
                    <td>其它流动负债</td>
                    <td>46</td>
                    <td>
                        <asp:TextBox ID="nc_46" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_46" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_46" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td colspan="12">长期投资：</td>
                </tr>
                <tr>
                    <td>长期股权投资</td>
                    <td>15</td>
                    <td>
                        <asp:TextBox ID="nc_15" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_15" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_15" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>流动负债合计</td>
                    <td>47</td>
                   <td>
                        <asp:TextBox ID="nc_47" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_47" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_47" runat="server" /></td>
                    <td></td>
                </tr>
                <tr>
                    <td>长期债权投资</td>
                    <td>16</td>
                    <td>
                        <asp:TextBox ID="nc_16" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_16" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_16" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td colspan="6">长期负债：</td>
                </tr>
                <tr>
                    <td>长期投资合计</td>
                    <td>17</td>
                    <td colspan="4"></td>
                    <td>长期借款</td>
                    <td>48</td>
                    <td>
                        <asp:TextBox ID="nc_48" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_48" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_48" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td colspan="6">固定资产：</td>
                    <td>应付债券</td>
                    <td>49</td>
                    <td>
                        <asp:TextBox ID="nc_49" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_49" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_49" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>固定资产原价</td>
                    <td>18</td>
                    <td>
                        <asp:TextBox ID="nc_18" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_18" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_18" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>长期应付款</td>
                    <td>50</td>
                    <td>
                        <asp:TextBox ID="nc_50" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_50" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_50" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>减：累计折旧</td>
                    <td>19</td>
                    <td>
                        <asp:TextBox ID="nc_19" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_19" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_19" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>专项应付款</td>
                    <td>51</td>
                    <td>
                        <asp:TextBox ID="nc_51" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_51" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_51" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>固定资产净值</td>
                    <td>20</td>
                    <td>
                        <asp:TextBox ID="nc_20" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_20" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_20" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>其他长期负债</td>
                    <td>52</td>
                    <td>
                        <asp:TextBox ID="nc_52" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_52" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_52" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>减：固定资产减值准备</td>
                    <td>21</td>
                    <td>
                        <asp:TextBox ID="nc_21" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_21" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_21" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>长期负债合计</td>
                    <td>53</td>
                    <td>
                        <asp:TextBox ID="nc_53" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_53" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_53" runat="server" /></td>
                    <td></td>
                </tr>
                <tr>
                    <td>固定资产净值</td>
                    <td>22</td>
                    <td>
                        <asp:TextBox ID="nc_22" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_22" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_22" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>递延税项：</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>工程物资</td>
                    <td>23</td>
                    <td>
                        <asp:TextBox ID="nc_23" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_23" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_23" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>递延税款贷项</td>
                    <td>54</td>
                    <td>
                        <asp:TextBox ID="nc_54" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_54" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_54" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>在建工程</td>
                    <td>24</td>
                    <td>
                        <asp:TextBox ID="nc_24" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_24" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_24" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>负债合计</td>
                    <td>55</td>
                    <td>
                        <asp:TextBox ID="nc_55" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_55" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_55" runat="server" /></td>
                    <td></td>
                </tr>
                <tr>
                    <td>固定资产清理</td>
                    <td>25</td>
                    <td>
                        <asp:TextBox ID="nc_25" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_25" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_25" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td colspan="6"></td>
                </tr>
                <tr>
                    <td>固定资产合计</td>
                    <td>26</td>
                    <td>
                        <asp:TextBox ID="nc_26" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_26" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_26" runat="server" /></td>
                    <td></td>
                    <td colspan="6">所有者权益（或股东权益）：</td>
                </tr>
                <tr>
                    <td colspan="6">无形资产及其他资产：</td>
                    <td>实收资本</td>
                    <td>56</td>
                    <td>
                        <asp:TextBox ID="nc_56" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_56" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_56" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>无形资产</td>
                    <td>27</td>
                    <td>
                        <asp:TextBox ID="nc_27" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_27" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_27" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>减：已归还投资</td>
                    <td>57</td>
                    <td>
                        <asp:TextBox ID="nc_57" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_57" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_57" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>长期待摊费用</td>
                    <td>28</td>
                    <td>
                        <asp:TextBox ID="nc_28" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_28" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_28" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>实收资本（或股本）</td>
                    <td>58</td>
                    <td>
                        <asp:TextBox ID="nc_58" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_58" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_58" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>其他长期资产</td>
                    <td>29</td>
                    <td>
                        <asp:TextBox ID="nc_29" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_29" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_29" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>资本公积</td>
                    <td>59</td>
                    <td>
                        <asp:TextBox ID="nc_59" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_59" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_59" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>无形资产及其他资产合计</td>
                    <td>30</td>
                    <td>
                        <asp:TextBox ID="nc_30" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_30" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_30" runat="server" /></td>
                    <td></td>
                    <td>盈余公积</td>
                    <td>60</td>
                    <td>
                        <asp:TextBox ID="nc_60" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_60" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_60" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td colspan="6"></td>
                    <td>其中：法定</td>
                    <td>61</td>
                    <td>
                        <asp:TextBox ID="nc_61" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_61" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_61" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td colspan="6">递延税项：</td>
                    <td>未分配利润</td>
                    <td>62</td>
                    <td>
                        <asp:TextBox ID="nc_62" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_62" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_62" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>递延税项借项</td>
                    <td>31</td>
                    <td>
                        <asp:TextBox ID="nc_31" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_31" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_31" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>所有者权益</td>
                    <td>63</td>
                    <td>
                        <asp:TextBox ID="nc_63" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_63" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_63" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
                <tr>
                    <td>资产总计</td>
                    <td>32</td>
                    <td>
                        <asp:TextBox ID="nc_32" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_32" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_32" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                    <td>负债和所有者权益</td>
                    <td>64</td>
                    <td>
                        <asp:TextBox ID="nc_64" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmys_64" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="qmjs_64" runat="server" /></td>
                    <td>
                        <input type="button" value="设置" onclick="selectyskm(this);" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
