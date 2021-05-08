<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Desk.aspx.cs" Inherits="webBill_main_Desk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/layui/css/layui.css" rel="stylesheet" />
    <script src="../Resources/layui/layui.js" type="text/javascript"></script>
    <link href="../Resources/layuiadmin/layuiadmin.css" rel="stylesheet" />
    <link href="../Resources/layuiadmin/xadmin.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $(".layui-col-xs3").live("click", function () {
                var named = $(this).attr("linkname");
                var width = $(this).attr("windoww");
                var height = $(this).attr("windowh");

                var datalink = $(this).attr("datalink");
                if (named == "预算调整" || named == "其他调整" || named == "预算追加") {
                    parent.createNewTab(named, $(this).attr("datalink"));
                } else {
                    parent.openWindow(datalink, width, height, named);
                }
            });
            $("#needTodo a").live("click", function () {
                var datalink = $(this).attr("datalink");
                var name = $(this).find("h3").eq(0).html();
                parent.createNewTab(name, datalink);
            });
        });
    </script>
</head>
<body>
    <div class="layui-fluid">
        <div class="layui-row layui-col-space15">
            <div class="layui-col-md8">
                <div class="layui-row layui-col-space15">
                    <div class="layui-col-md12">
                        <div class="layui-card">
                            <div class="layui-card-header">快捷方式</div>
                            <div class="layui-card-body">
                                <div class="layui-carousel layadmin-carousel layadmin-shortcut">
                                    <ul class="layui-row layui-col-space10">
                                        <li class="layui-col-xs3" linkname="借款申请单" datalink="../bxgl/bxDetailForDz.aspx?type=add&dydj=02&djmxlx=01&isDZ=1" windoww="1000" windowh="700">
                                            <a>
                                                <i class="layui-icon layui-icon-add-1"></i>
                                                <cite>借款单</cite>
                                            </a>
                                        </li>
                                        <li class="layui-col-xs3" linkname="报销单" datalink="../bxgl/bxDetailForDz.aspx?type=add&par=0.23071922305615455&dydj=06&djmxlx=01&isDZ=1" windoww="1000" windowh="700">
                                            <a>
                                                <i class="layui-icon layui-icon-form"></i>
                                                <cite>报销单</cite>
                                            </a>
                                        </li>
                                        <li class="layui-col-xs3" linkname="退费申请" datalink="../bxgl/jkDetailForDz.aspx?type=add&par=0.23071922305615455&dydj=06&djmxlx=01&isDZ=1" windoww="1000" windowh="700">
                                            <a>
                                                <i class="layui-icon layui-icon-survey"></i>
                                                <cite>退费申请</cite>
                                            </a>
                                        </li>
                                        <li class="layui-col-xs3" linkname="退费申请" datalink="../bxgl/ZijinShenqingDetails.aspx?ctrl=add" windoww="1000" windowh="700">
                                            <a layadmin-event="im">
                                                <i class="layui-icon layui-icon-rmb"></i>
                                                <cite>经费申请</cite>
                                            </a>
                                        </li>
                                        <li class="layui-col-xs3" linkname="预算调整" datalink="../ysgl/ystz.aspx?isdz=1" windoww="1000" windowh="700">
                                            <a>
                                                <i class="layui-icon layui-icon-slider"></i>
                                                <cite>预算调整</cite>
                                            </a>
                                        </li>
                                        <li class="layui-col-xs3" linkname="其他调整" datalink="../ysgl/ystz_qt_dz.aspx" windoww="1000" windowh="700">
                                            <a>
                                                <i class="layui-icon layui-icon-senior"></i>
                                                <cite>其他调整</cite>
                                            </a>
                                        </li>
                                        <li class="layui-col-xs3" linkname="预算追加" datalink="../ysgl/ystz.aspx?isdz=1" windoww="1000" windowh="700">
                                            <a lay-href="user/user/list.html">
                                                <i class="layui-icon layui-icon-templeate-1"></i>
                                                <cite>预算追加</cite>
                                            </a>
                                        </li>

                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--<div class="layui-col-md6">
                            <div class="layui-card">
                                <div class="layui-card-header">当月指标</div>
                                <div class="layui-card-body">
                                    <table class="layui-table">
                                        <thead>
                                            <tr>
                                                <th>科目
                                                </th>
                                                <th>预算
                                                </th>
                                                <th>决算
                                                </th>
                                                <th>剩余
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>差旅费</td>
                                                <td>30000</td>
                                                <td>20000</td>
                                                <td>10000</td>
                                            </tr> 
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>--%>
                </div>
                <div class="layui-col-md12">
                    <div class="layui-card">
                        <div class="layui-card-header">我的单据</div>
                        <div class="layui-card-body">
                            <table class="layui-table">
                                <thead>
                                    <tr>
                                        <th>类型
                                        </th>
                                        <th>摘要
                                        </th>
                                        <th>日期
                                        </th>
                                        <th>进度
                                        </th>
                                    </tr>
                                </thead>
                                <tbody id="mybill">
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <%--<div class="layui-card">
                                <div class="layui-tab layui-tab-brief layadmin-latestData">
                                    <ul class="layui-tab-title">
                                        <li class="layui-this">今日热搜</li>
                                        <li>今日热帖</li>
                                    </ul>
                                    <div class="layui-tab-content">
                                        <div class="layui-tab-item layui-show">
                                            <table id="LAY-index-topSearch"></table>
                                        </div>
                                        <div class="layui-tab-item">
                                            <table id="LAY-index-topCard"></table>
                                        </div>
                                    </div>
                                </div>
                            </div>--%>
                </div>
                <%--<div class="layui-row layui-col-space15">
                        <div class="layui-col-md12">
                            <div class="layui-card">
                                <div class="layui-card-header">效果报告</div>
                                <div class="layui-card-body layadmin-takerates">
                                    <div class="layui-progress" lay-showpercent="yes">
                                        <h3>转化率（日同比 28% <span class="layui-edge layui-edge-top" lay-tips="增长" lay-offset="-15"></span>）</h3>
                                        <div class="layui-progress-bar" lay-percent="65%"></div>
                                    </div>
                                    <div class="layui-progress" lay-showpercent="yes">
                                        <h3>签到率（日同比 11% <span class="layui-edge layui-edge-bottom" lay-tips="下降" lay-offset="-15"></span>）</h3>
                                        <div class="layui-progress-bar" lay-percent="32%"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>--%>
            </div>

            <div class="layui-col-md4">
                <div class="layui-card">
                    <div class="layui-card-header">待办事项</div>
                    <div class="layui-card-body layui-text">
                        <div class="layui-carousel layadmin-backlog">
                            <ul class="layui-row layui-col-space10" id="needTodo">
                            </ul>
                        </div>
                    </div>
                </div>

                <%-- 

                    <div class="layui-card">
                        <div class="layui-card-header">实时监控</div>
                        <div class="layui-card-body layadmin-takerates">
                            <div class="layui-progress" lay-showpercent="yes">
                                <h3>CPU使用率</h3>
                                <div class="layui-progress-bar" lay-percent="58%"></div>
                            </div>
                            <div class="layui-progress" lay-showpercent="yes">
                                <h3>内存占用率</h3>
                                <div class="layui-progress-bar layui-bg-red" lay-percent="90%"></div>
                            </div>
                        </div>
                    </div>

                    <div class="layui-card">
                        <div class="layui-card-header">产品动态</div>
                        <div class="layui-card-body">
                            <div class="layui-carousel layadmin-carousel layadmin-news" data-autoplay="true" data-anim="fade" lay-filter="news">
                                <div carousel-item>
                                    <div><a href="http://fly.layui.com/docs/2/" target="_blank" class="layui-bg-red">layuiAdmin 快速上手文档</a></div>
                                    <div><a href="http://fly.layui.com/vipclub/list/layuiadmin/" target="_blank" class="layui-bg-green">layuiAdmin 会员讨论专区</a></div>
                                    <div><a href="http://www.layui.com/admin/#get" target="_blank" class="layui-bg-blue">获得 layui 官方后台模板系统</a></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="layui-card">
                        <div class="layui-card-header">
                            作者心语
            <i class="layui-icon layui-icon-tips" lay-tips="要支持的噢" lay-offset="5"></i>
                        </div>
                        <div class="layui-card-body layui-text layadmin-text">
                            <p>一直以来，layui 秉承无偿开源的初心，虔诚致力于服务各层次前后端 Web 开发者，在商业横飞的当今时代，这一信念从未动摇。即便身单力薄，仍然重拾决心，埋头造轮，以尽可能地填补产品本身的缺口。</p>
                            <p>在过去的一段的时间，我一直在寻求持久之道，已维持你眼前所见的一切。而 layuiAdmin 是我们尝试解决的手段之一。我相信真正有爱于 layui 生态的你，定然不会错过这一拥抱吧。</p>
                            <p>子曰：君子不用防，小人防不住。请务必通过官网正规渠道，获得 <a href="http://www.layui.com/admin/" target="_blank">layuiAdmin</a>！</p>
                            <p>—— 贤心（<a href="http://www.layui.com/" target="_blank">layui.com</a>）</p>
                        </div>
                    </div>--%>
            </div>
            <div class="layui-col-md4">
                <div class="layui-card">
                    <div class="layui-card-header">微信机器人</div>
                    <div class="layui-card-body layui-text">
                        建设中，功能开放后可通过微信聊天实现单据审批。
                    </div>
                </div>
            </div>
            <div class="layui-col-md4">
                <div class="layui-card">
                    <div class="layui-card-header">我要吐槽</div>
                    <div class="layui-card-body layui-text">
                        <div class="layui-carousel layadmin-backlog">
                            <form class="layui-form" action="" lay-filter="example">
                                <div class="layui-form-item">
                                    <label class="layui-form-label">类型</label>
                                    <div class="layui-input-block">
                                        <select name="type" lay-filter="type">
                                            <option value="0">操作习惯优化</option>
                                            <option value="1">系统性能提升</option>
                                            <option value="2">新增功能</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="layui-form-item">
                                    <label class="layui-form-label">功能模块</label>
                                    <div class="layui-input-block">
                                        <input id="title" type="text" name="title" lay-verify="title" autocomplete="off" placeholder="请输入标题" class="layui-input">
                                    </div>
                                </div>
                                <div class="layui-form-item layui-form-text">
                                    <label class="layui-form-label">详细说明</label>
                                    <div class="layui-input-block">
                                        <textarea id="desc" placeholder="请输入内容" class="layui-textarea" name="desc"></textarea>
                                    </div>
                                </div>
                                <div class="layui-form-item layui-form-text">

                                    <div class="layui-input-block">
                                        十分感谢您的意见和建议，我们会尽力为您提供更好的软件使用体验。
                                    </div>
                                </div>
                                <div class="layui-form-item">
                                    <div class="layui-input-block">
                                        <button type="submit" class="layui-btn layui-btn-normal" lay-submit="" lay-filter="demo1">提交</button>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>
    </div>
    <script type="text/javascript">
        layui.use(['form', 'layedit', 'laydate'], function () {
            var form = layui.form;
            form.on('submit(demo1)', function (data) {
                var data = form.val('example');

                $.post("../MyAjax/Proposal.ashx", JSON.stringify(data), function (data, status) {
                    if (status == "success") {
                        $("#title").val('');
                        $("#desc").val('');
                        alert("提交成功！");
                    }
                });
                return false;
            });
        });
        $(function () {
            //待办事项
            $.post("../MyAjax/GetDeskUndo.ashx", "", function (data, status) {
                if (status == "success") {
                    $("#needTodo").html(data);
                }
            });
            $.post("../MyAjax/MyBills.ashx", "", function (data, status) {
                if (status == "success") {
                    $("#mybill").html(data);
                }
            });
        });

    </script>
</body>
</html>
