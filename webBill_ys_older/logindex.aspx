<%@ Page Language="C#" AutoEventWireup="true" CodeFile="logindex.aspx.cs" Inherits="logindex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算管理系统</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<style type="text/css">
    .content {
        position: absolute;
        left: 15%;
        top: 35%;
        margin-left: -200px; /* 400的一半 */
        margin-top: -100px; /* 200的一半 */
    }

    body {
        text-align: center;
        color: #333333;
        background: #F6FAFD url(images/new_niu1_02.gif) repeat-x left top;
    }

    .STYLE3 {
        font-size: 12px;
    }

    input {
        font-family: Verdana,Arial,Helvetica,sans-serif;
        font-size: 12px;
        padding: 2px 2px 2px 2px;
    }

    .formstyle {
        width: 150px;
        background: #ffffff;
        padding: 4px 2px 4px 2px;
        font-size: 16px;
        height: 20px;
        border: 1px solid #C0C0C0;
    }

    .container, .container * {
        margin: 0;
        padding: 0;
    }

    .container {
        width: 408px;
        height: 168px;
        overflow: hidden;
        position: relative;
    }

    .slider {
        position: absolute;
    }

        .slider li {
            list-style: none;
            display: inline;
        }

        .slider img {
            width: 408px;
            height: 168px;
            display: block;
        }

    .slider2 {
        width: 2000px;
    }

        .slider2 li {
            float: left;
        }

    .num {
        position: absolute;
        right: 5px;
        bottom: 5px;
    }

        .num li {
            float: left;
            color: #FF7300;
            text-align: center;
            line-height: 16px;
            width: 16px;
            height: 16px;
            font-family: Arial;
            font-size: 12px;
            cursor: pointer;
            overflow: hidden;
            margin: 3px 1px;
            border: 1px solid #FF7300;
            background-color: #fff;
        }

            .num li.on {
                color: #fff;
                line-height: 21px;
                width: 21px;
                height: 21px;
                font-size: 16px;
                margin: 0 1px;
                border: 0;
                background-color: #FF7300;
                font-weight: bold;
            }

    .container, .container * {
        margin: 0;
        padding: 0;
    }

    .container {
        width: 408px;
        height: 168px;
        overflow: hidden;
        position: relative;
    }

    .slider {
        position: absolute;
    }

        .slider li {
            list-style: none;
            display: inline;
        }

        .slider img {
            width: 408px;
            height: 168px;
            display: block;
        }

    .slider2 {
        width: 2000px;
    }

        .slider2 li {
            float: left;
        }

    .num {
        position: absolute;
        right: 5px;
        bottom: 5px;
    }

        .num li {
            float: left;
            color: #FF7300;
            text-align: center;
            line-height: 16px;
            width: 16px;
            height: 16px;
            font-family: Arial;
            font-size: 12px;
            cursor: pointer;
            overflow: hidden;
            margin: 3px 1px;
            border: 1px solid #FF7300;
            background-color: #fff;
        }

            .num li.on {
                color: #fff;
                line-height: 21px;
                width: 21px;
                height: 21px;
                font-size: 16px;
                margin: 0 1px;
                border: 0;
                background-color: #FF7300;
                font-weight: bold;
            }

    .tt {
        height: 320px;
        width: 470px;
    }
</style>

<script src="webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

<script language="javascript" type="text/javascript">
    $(function () {
        // changeImg();
        //alert($("#txtUserCode"));
        //alert($("#txtUserCode").val()); //null
        //alert($("#txtUserCode").text());//报错
        document.getElementById("txtUserCode").focus();
        //if (!window.ActiveXObject) {
        //    alert("尊敬的用户您好，系统检测到您使用非IE系列浏览器登录，为了保证您有良好使用体验，强烈建议您使用IE系列浏览器。");
        //}



    });

    function setClear() {
        document.getElementById("txtUserCode").value = "";
        document.getElementById("txtUserPwd").value = "";
        document.getElementById("txtCheckCode").value = "";
    }
    function userLoginSucess() {
        var width = screen.width;
        var height = screen.height;

        if (height == 768) height -= 75;
        if (height == 600) height -= 60;


        var szFeatures = "top=0,";
        szFeatures += "left=0,";
        szFeatures += "width=" + width + ",";
        szFeatures += "height=" + height + ",";
        szFeatures += "directories=no,";
        szFeatures += "status=yes,";
        szFeatures += "menubar=no,";

        //    _Default.SetScreenWidth(width);
        //    _Default.SetScreeniHeight(height);
        if (height <= 600) szFeatures += "scrollbars=yes,";
            //else if (height >= 800) szFeatures += "scrollbars=yes,";
        else szFeatures += "scrollbars=no,";

        szFeatures += "resizable=yes"; //channelmode
        window.open("webBill/main/mainFrame.aspx", "", szFeatures);

        //不提示的关闭窗口
        window.opener = "telchina";
        window.close();
    }
    function Clos() {
        if (confirm("你要退出本系统吗？")) {
            window.close();
        }
    }
    function objfocus(obj) {
        obj.focus();
    }
    function changeImg() {
        var url = "validate.aspx?" + Math.round();
        document.getElementById('imgYz').src = "";
        document.getElementById('imgYz').src = url;
    }
</script>

<body id="bodyid" runat="server">
    <form id="form1" runat="server" style="text-align: center">
        <div class="content" style="width: 100%">
            <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center" style="text-align: center">
                        <table width="778" border="0" align="center" style="text-align: center" cellpadding="0"
                            cellspacing="8" bgcolor="#FFFFFF">
                            <tr>
                                <td colspan="2">
                                    <div class="container" id="idTransformView2" style="height: 320px; width: 470px">
                                        <ul class="slider slider2" id="idSlider2">
                                            <li>
                                                <img id="img01" runat="server" style="width: 470px; height: 320px"></li>
                                            <li>
                                                <img id="img02" runat="server" style="width: 470px; height: 320px" /></li>
                                            <li>
                                                <img id="img03" runat="server" style="width: 470px; height: 320px" /></li>
                                        </ul>
                                        <ul class="num" id="idNum2">
                                            <li>1</li>
                                            <li>2</li>
                                            <li>3</li>
                                        </ul>
                                    </div>
                                </td>
                                <td>
                                    <div style="width: 264px; height: 320px; border-width: 1px; border: solid 1px #CCCCCC; margin-bottom: 0px">
                                        <div width="80%" style="border: #FFFFFF; text-align: center; margin-bottom: 0px">
                                            <div style="font-size: 20px; margin-top: 10px; margin-bottom: 15px; font-family: 微软雅黑; font-weight: bold; text-align: center; color: Black; height: 20px">
                                                <%=Strcp%>
                                            </div>
                                            <div style="font-size: 20px; margin-left: 15px; font-weight: bold; text-align: center; font-family: 微软雅黑;">
                                                <%=Strobjectname %>
                                            </div>
                                            <table width="42%" border="0" cellpadding="1" cellspacing="0">


                                                <tr>
                                                    <td>
                                                        <div align="center" class="STYLE3" style="width: 70px;">
                                                            用户名:
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div align="left" class="STYLE3">
                                                            <input id="txtUserCode" class="formstyle" runat="server" name="txtUserCode2" type="text"
                                                                value="" />
                                                            <%--    onmouseover="objfocus(this);"--%>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div align="center" class="STYLE3">
                                                            密 &nbsp;码:
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div align="left" class="STYLE3">
                                                            <input id="txtUserPwd" runat="server" class="formstyle" name="txtUserPwd" type="password"
                                                                value="" />
                                                            <%--   onmouseover="objfocus(this);"--%>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr style="display: <%=ShowYanZhengMa%>">
                                                    <td>
                                                        <div align="center" class="STYLE3">
                                                            验证码:
                                                        </div>
                                                    </td>
                                                    <td colspan="2">
                                                        <div style="float: left; margin-top: 2px">
                                                            <asp:TextBox ID="txtCheckCode" runat="server" CssClass="formstyle" Width="60px" onmouseover="objfocus(this);"></asp:TextBox>
                                                        </div>
                                                        <div style="float: left; margin-top: 3px; text-align: right; margin-left: 10px;">
                                                            <img id="imgYz" align="middle" alt="看不清，请单击该验证码" src="validate.aspx" height="30px"
                                                                style="width: 78px" onclick="changeImg();" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" height="60px" colspan="2">
                                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="images/loginBttton.jpg"
                                                            Width="120" OnClick="ImageButton1_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div align="left" class="STYLE3">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;本系统支持IE6以上的IE系列主流浏览器，单击<a href="help.htm">查看</a>如何将IE设为默认浏览器。
                                                            &nbsp;&nbsp;&nbsp;&nbsp;<a href="erweima.html">手机扫码登录</a>
                                                        </div>

                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <%--  <div style="width:780px; margin:0 auto; padding:0px; padding-top:5px; height:30px;">
                          <div style="float:right; color:Black; font-size:12px; font-weight:bold; width:300px;">
                            <label style="font-weight:normal">济南行信科技有限公司</label>
                         </div>
                     </div>--%>
                    </td>
                </tr>
            </table>
        </div>
        <script type="text/javascript">
            var $ = function (id) {
                return "string" == typeof id ? document.getElementById(id) : id;
            };

            var Class = {
                create: function () {
                    return function () {
                        this.initialize.apply(this, arguments);
                    }
                }
            }

            Object.extend = function (destination, source) {
                for (var property in source) {
                    destination[property] = source[property];
                }
                return destination;
            }

            var TransformView = Class.create();
            TransformView.prototype = {
                //容器对象,滑动对象,切换参数,切换数量
                initialize: function (container, slider, parameter, count, options) {
                    if (parameter <= 0 || count <= 0) return;
                    var oContainer = $(container), oSlider = $(slider), oThis = this;

                    this.Index = 0; //当前索引

                    this._timer = null; //定时器
                    this._slider = oSlider; //滑动对象
                    this._parameter = parameter; //切换参数
                    this._count = count || 0; //切换数量
                    this._target = 0; //目标参数

                    this.SetOptions(options);

                    this.Up = !!this.options.Up;
                    this.Step = Math.abs(this.options.Step);
                    this.Time = Math.abs(this.options.Time);
                    this.Auto = !!this.options.Auto;
                    this.Pause = Math.abs(this.options.Pause);
                    this.onStart = this.options.onStart;
                    this.onFinish = this.options.onFinish;

                    oContainer.style.overflow = "hidden";
                    oContainer.style.position = "relative";

                    oSlider.style.position = "absolute";
                    oSlider.style.top = oSlider.style.left = 0;
                },
                //设置默认属性
                SetOptions: function (options) {
                    this.options = {//默认值
                        Up: true, //是否向上(否则向左)
                        Step: 10, //滑动变化率
                        Time: 30, //滑动延时
                        Auto: true, //是否自动转换
                        Pause: 2000, //停顿时间(Auto为true时有效)
                        onStart: function () { }, //开始转换时执行
                        onFinish: function () { } //完成转换时执行
                    };
                    Object.extend(this.options, options || {});
                },
                //开始切换设置
                Start: function () {
                    if (this.Index < 0) {
                        this.Index = this._count - 1;
                    } else if (this.Index >= this._count) { this.Index = 0; }

                    this._target = -1 * this._parameter * this.Index;
                    this.onStart();
                    this.Move();
                },
                //移动
                Move: function () {
                    clearTimeout(this._timer);
                    var oThis = this, style = this.Up ? "top" : "left", iNow = parseInt(this._slider.style[style]) || 0, iStep = this.GetStep(this._target, iNow);

                    if (iStep != 0) {
                        this._slider.style[style] = (iNow + iStep) + "px";
                        this._timer = setTimeout(function () { oThis.Move(); }, this.Time);
                    } else {
                        this._slider.style[style] = this._target + "px";
                        this.onFinish();
                        if (this.Auto) { this._timer = setTimeout(function () { oThis.Index++; oThis.Start(); }, this.Pause); }
                    }
                },
                //获取步长
                GetStep: function (iTarget, iNow) {
                    var iStep = (iTarget - iNow) / this.Step;
                    if (iStep == 0) return 0;
                    if (Math.abs(iStep) < 1) return (iStep > 0 ? 1 : -1);
                    return iStep;
                },
                //停止
                Stop: function (iTarget, iNow) {
                    clearTimeout(this._timer);
                    this._slider.style[this.Up ? "top" : "left"] = this._target + "px";
                }
            };
            window.onload = function () {
                function Each(list, fun) {
                    for (var i = 0, len = list.length; i < len; i++) { fun(list[i], i); }
                };
                var objs2 = $("idNum2").getElementsByTagName("li");

                var tv2 = new TransformView("idTransformView2", "idSlider2", 470, 3, {
                    onStart: function () { Each(objs2, function (o, i) { o.className = tv2.Index == i ? "on" : ""; }) }, //按钮样式
                    Up: false
                });
                tv2.Start();
                Each(objs2, function (o, i) {
                    o.onmouseover = function () {
                        o.className = "on";
                        tv2.Auto = false;
                        tv2.Index = i;
                        tv2.Start();
                    }
                    o.onmouseout = function () {
                        o.className = "";
                        tv2.Auto = true;
                        tv2.Start();
                    }
                })
            }


        </script>

    </form>
</body>
</html>
