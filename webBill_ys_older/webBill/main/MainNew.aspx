<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainNew.aspx.cs" Inherits="webBill_main_MainNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        body, td, .p1, .p2, .i {
            font-family: arial;
        }

        body {
            margin: 6px 0 0 0;
            background-color: #fff;
            color: #000;
        }

        table {
            border: 0;
        }

        #cal {
            width: 324px;
            border: 1px solid #c3d9ff;
            font-size: 12px;
            margin: 1px 1px 0 1px;
        }

            #cal #top {
                height: 19px;
                line-height: 19px;
                background: #e7eef8;
                color: #003784;
                padding-left: 5px;
            }

                #cal #top select {
                    font-size: 12px;
                }

                #cal #top input {
                    padding: 0;
                }

            #cal ul#wk {
                margin: 0;
                padding: 0;
                height: 25px;
            }

                #cal ul#wk li {
                    float: left;
                    width: 45px;
                    text-align: center;
                    line-height: 25px;
                    list-style: none;
                }

                    #cal ul#wk li b {
                        font-weight: normal;
                        color: #c60b02;
                    }

            #cal #cm {
                clear: left;
                border-top: 1px solid #ddd;
                border-bottom: 1px dotted #ddd;
                position: relative;
            }

                #cal #cm .cell {
                    position: absolute;
                    width: 30px;
                    height: 30px;
                    text-align: center;
                    margin: 0 0 0 5px;
                }

                    #cal #cm .cell .so {
                        font: bold 12px arial;
                    }

            #cal #bm {
                text-align: right;
                height: 1px;
                line-height: 1px;
                padding: 0 1px 0 0;
            }

            #cal #fd {
                display: none;
                position: absolute;
                border: 1px solid #dddddf;
                background: #feffcd;
                padding: 10px;
                line-height: 21px;
                width: 150px;
            }

                #cal #fd b {
                    font-weight: normal;
                    color: #c60a00;
                }

        .hiddenbill {
            display: none;
        }

        .highlight {
            background: #CCE8CF;
        }
    </style>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/Main.css" rel="stylesheet" type="text/css" />
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <script type="text/javascript">
        function openWindow(url, width, height, title) {
            $("#prodcutDetailSrc").attr("src", url);
            $("#dialog-confirm").dialog('close');
            $("#dialog-confirm").dialog(
                {
                    modal: true,// 创建模式对话框
                    autoOpen: true,//是否自动打开
                    height: height, //高度
                    width: 1000, //宽度
                    title: title,
                    title_html: true,
                    buttons: {
                    }
                }
            );
            $("#dialog-confirm").dialog("option", "width", width);
            $("#dialog-confirm").dialog("option", "height", height);
        }
        function closeDetail() {
            $("#dialog-confirm").dialog('close');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hd_link" runat="server" />
        <input type="hidden" id="hdnowshowname" />
        <div id="left" runat="server">
        </div>
        <div id='btn_menuclost' style="float: left; width: 5px; margin-top: -100px">
            <img id="imgswich" style="margin-top: 300px;" onclick="menuClose()" src="../images/switch_left.gif"
                alt="隐藏菜单" />
        </div>
        <div id="right" style="width: 83%">
            <ul>
                <li><a id="a_desk" href="#tabs-1">桌面</a></li>
            </ul>
            <div id="tabs-1">
                <iframe src="Desk.aspx" width="100%" height="100%" scrolling="auto" frameborder="0"></iframe>
            </div>
        </div>
        <div style="float: left; height: 100%; width: 10%; border: 1px solid #c3d9ff; padding: 1px; display: none"
            id="help">
            <div style="float: left; width: 5px; margin-top: -100px;">
                <img id="img1" style="margin-top: 300px;" onclick="helptoggle()" src="../images/switch_right.gif"
                    alt="隐藏菜单" />
            </div>
            <div style="float: left; word-wrap: break-word; padding: 0px; width: 90%; border: 1px solid bule">
                <div style="height: 21px; background-color: #70A8D2; line-height: 21px; text-align: center; font-family: @微软雅黑">
                    帮 助 (H)
                </div>
                <div id="helpcontent" style="word-wrap: break-word; overflow: auto;">
                </div>
            </div>
        </div>
        <div id="dialog-confirm" style="display: none; overflow: hidden;">
            <iframe frameborder="no" border="0" marginwidth="0" marginheight="0" id="prodcutDetailSrc" scrolling="auto" width="100%" height="100%"></iframe>
        </div>
    </form>
    <script type="text/javascript" language="javascript" charset="gb2312">
        $(function () {
            $("#left").accordion({
                fillSpace: true
            });

            $tabs = $("#right").tabs({
                tabTemplate: "<li><a href='#{href}'>#{label}</a> <span class='ui-icon ui-icon-close'></span></li>",
                add: function (event, ui) {
                    var link = $("#hd_link").val();
                    //在这里为每一个页面传一个页面高度的参数
                    if (link.indexOf('?') > -1) {
                        link += "&wdheight=" + ($(window).height() - 30);
                    } else {
                        link += "?wdheight=" + ($(window).height() - 30);
                    }
                    $(ui.panel).append("<iframe src='" + link + "' width='100%' height='95%' frameborder='0'  scrolling='auto'></iframe>");
                },
                select: function (event, ui) {
                    if (ui.index == 0) {
                        //GetDesk();
                    }
                    //切换tab的时候动态显示帮助信息
                    var menuname = ui.tab.innerHTML;
                    $("#hdnowshowname").val(menuname);
                    showhelpmsg();
                }
            });

            $("#right span.ui-icon-close").live("click", function () {
                var index = $("li", $tabs).index($(this).parent());
                $tabs.tabs("remove", index);
            });
            $(".addTabs").live("click", function () {
                var named = $(this).attr("linkname");

                createNewTab(named, $(this).attr("datalink"))
            });
            $("#cal").mouseleave(function () {
                $("div[name='showDiv']").css("display", "none");
            });

            $("#cal").find(".baseButton").live("click", function () {
                var text = $("#cal").find(".baseText").val();
                var date = this.checkDate;
                $.post("../MyAjax/NoteSave.ashx", { "tempDate": date, "text": text }, function (data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            //                            alert("添加成功");
                        }
                        else {
                            alert(data);
                        }
                    }
                    else {
                        alert("添加失败");
                    }
                });
            });


        });
        function createNewTab(named, url) {
            $("#hd_link").val(url);
            var bj = 0;
            $("#right ul li a").each(function (i) {
                if ($(this).html() == named) {
                    $tabs.tabs({ selected: i });
                    bj = 1;
                }
            });
            if (bj == 0) {
                $tabs.tabs("add", "tab-1", named, 1);
                $tabs.tabs({ selected: 1 });
                var tabcont = $("#right").tabs('length');
                if (tabcont > 6) {
                    $tabs.tabs("remove", tabcont - 1);
                }
                UploadComplete();
            }
            //将菜单名称放到隐藏域
            $("#hdnowshowname").val(named);
            showhelpmsg();
        }
        function menuClose() {
            if (document.getElementById("left").style.display == "none") {
                document.getElementById("left").style.display = ""
                document.getElementById("right").style.width = "83%";
                $("#imgswich").attr("src", "../images/switch_left.gif");
            }
            else {
                document.getElementById("left").style.display = "none"
                document.getElementById("right").style.width = "98%";
                $("#imgswich").attr("src", "../images/switch_right.gif");
            }

        }

        function helptoggle() {
            if (document.getElementById("help").style.display == "none") {
                document.getElementById("right").style.width = "73%";
            } else {
                document.getElementById("right").style.width = "83%";
            }
            $("#help").toggle();
            showhelpmsg();
        }
        function showhelpmsg() {
            var showname = $("#hdnowshowname").val();
            if (showname != '' && showname != null && showname != undefined) {
                $.post("helpHandler.ashx", { "menuname": escape(showname) }, function (data, status) {
                    if (data != null && data != undefined) {
                        var h = $("#help").height();
                        $("#helpcontent").height(h);
                        $("#helpcontent").html(data);
                    }
                });
            }
        }
        function UploadComplete() {

            if (document.getElementById("UploadChoose")) {
                return;
            }
            //showCover();
            //控件宽
            var aw = 240;
            //控件高
            var ah = 60;

            //计算控件水平位置
            var al = ($(window).width() - aw) / 2;
            //计算控件垂直位置
            var at = (screen.height - ah) / 5;
            //内容管理
            var title = '';
            var icon = 'indi.gif';
            var cardID = '0';
            //输出提示框
            var div = document.createElement("div");
            div.id = "UploadChoose";
            div.innerHTML = '\
    <div style="background-color:#FFFFFF;position:absolute;top:' + at + 'px;left:' + al + 'px;border:1px solid #8CBEDA;text-align:center;">\
        <div style="width:' + aw + 'px;height:' + ah + 'px;line-height:20px;border:2px solid #D6E7F2;">\
        <div style="clear:both;background-color:#0099AA;line-height:25px;font-weight:bold;color:#FFFFFF;font-size:12px;padding-left:10px">'+ title + '</div>\
        <div style="float:left;width:30px;padding-left:10px;padding-top:15px;"><img src="../Resources/Images/Loading/'+ icon + '" alert="Cardo" /></div>\
       <div style="float:left;width:160px;padding-top:20px;padding-left:10px;color:#37a;font-size:12px;font-weight:bold;">数据加载中，请稍后…</div>\
    <div style="clear:both;text-align:center;margin-top:10px;padding-bottom:10px">\
        </div>\
    </div>\
    </div>';
            document.body.appendChild(div);
            setTimeout('closeAlert("UploadChoose")', 1000);
        }
        //    //关闭等待条
        function closeAlert(alertid) {
            if (typeof (eval('document.all.' + alertid)) != 'undefined') {
                document.getElementById(alertid).outerHTML = '';
                closeCover();
            }
        }

        function closeCover() {
            if (document.getElementById('AlexCoverV1_0')) {
                document.getElementById('AlexCoverV1_0').style.display = 'none';
                //DispalySelect(1);
            }
        }

    </script>

</body>
</html>
