<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserIndex2.aspx.cs" Inherits="PictureWebSite.UserIndex2" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- 上述3个meta标签*必须*放在最前面，任何其他内容都*必须*跟随其后！ -->
    <title>UserIndex2</title>

    <!-- Bootstrap -->
    <link href="assets/js/lib/BootStrap/css/bootstrap.min.css" rel="stylesheet" />
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="//cdn.bootcss.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="//cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->

    <style type="text/css">
        /*BootStrap默认样式修改*/

        .control-label {
            text-align: left !important;
        }

        /*主体container布局*/
        .container {
            margin-top: 70px;
            /*width: 900px;*/
        }

        @media (min-width:768px) {
            .container {
                width: 500px;
            }
        }

        @media (min-width:992px) {
            .container {
                width: 640px;
            }
        }

        @media (min-width:550px) {
            .container {
                width: 500px;
            }
        }

        @media (min-width:1200px) {
            .container {
                width: 700px;
            }
        }

        .breadcrumb {
            background-color: white;
        }

        .panel {
            border-radius: 0px;
        }

        .thumbnail {
            border-radius: 0px;
        }

        .img-thumbnail {
            border-radius: 0px;
        }


        .modal-content {
            border-radius: 0px;
        }

        /*面包屑导航分隔符*/
        .breadcrumb > li + li::before {
            content: "|";
        }

        /*BootStrap默认样式修改结束*/

        /*用户信息面板*/
        #user {
            margin-top: 50px;
            border-bottom: 1px solid #EDECEC;
        }

            #user .panel {
                box-shadow: none;
            }

            #user h3 a[role=button] {
                color: black;
            }

                #user h3 a[role=button]:link {
                    text-decoration: none;
                }

                #user h3 a[role=button]:focus {
                    outline: none;
                }


        #userFunction .panel {
            box-shadow: none;
        }



        /*标签切换按钮样式*/
        #pictureTabs a:focus {
            outline: none;
        }

        /*home标签页的样式*/
        /*
        #singlePictureModel img {
            width: 100%;
        }

        #singlePictureModel div.col-xs-12 {
            padding-left: 0px;
            padding-right: 0px;
        }*/

        #userPanel .panel {
            border-width: 0px;
            margin: 0px;
        }
        /*home标签页的样式结束*/

        /*#mycamera {
            margin: 0px auto;
        }*/

        /*弹出层样式*/
        .modalFace {
            margin-right: 5px;
        }

            .modalFace img {
                border-radius: 4px;
            }

        .modalUsername {
            margin-bottom: 8px;
            word-wrap: break-word;
            word-break: break-all;
        }

        .modalUploadDate {
            color: #8C8C8C;
        }

            .modalUploadDate span {
                margin-left: 5px;
                display: none;
            }


        .modalComment .row {
            border-bottom: solid 1px #EDECEC;
            padding-bottom: 5px;
            padding-top: 5px;
        }

        .modal-footer > .input-group {
            margin-top: 10px;
            width: 100%;
        }

        .modal-footer > .btn {
            margin-top: 10px;
            width: 90px;
        }

        #pictureDetailModal .modal-dialog {
            margin-bottom: 150px;
        }


        #modalTags .breadcrumb {
            margin-top: 0px;
            margin-bottom: 0px;
        }

        #picLinks {
            padding-left: 0px;
        }

            #picLinks li {
                border-color: white;
            }

            #picLinks li {
                color: #6D8BA7;
            }

        .masonry-container > div {
        }
    </style>

</head>
<body>

    <%--主体容器--%>
    <div class="container">

        <%--用户总面板--%>
        <div class="panel panel-default">
            <div class="panel-body ">

                <%--用户名头像部分--%>
                <div class="row">
                    <div class="panel-group col-xs-8 col-xs-offset-2" id="user" role="tablist" aria-multiselectable="true">
                        <div class="panel">
                            <h3 class="text-center">
                                <a role="button" data-toggle="collapse" data-parent="#user" href="#collapse_UserFace" aria-expanded="true" aria-controls="collapse_UserFace">用户名 </a>
                            </h3>
                        </div>

                        <div id="collapse_UserFace" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                            <div class="panel-body">
                                <%--头像--%>
                                <div>
                                    <img class="img-responsive center-block" src="<%=this.AvatarHref %>" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <%--功能链接--%>
                <div id="userFunction">


                    <ol class="breadcrumb text-center">
                        <li><a data-toggle="collapse" href="#collapse1" aria-expanded="false" aria-controls="collapse1" data-parent="#userPanel">首页</a></li>
                        <li><a data-toggle="collapse" href="#collapse_UserSetting" aria-expanded="false" aria-controls="collapse_UserSetting" data-parent="#userPanel">账号设置</a></li>
                        <li><a data-toggle="collapse" href="#collapse3" aria-expanded="false" aria-controls="collapse3" data-parent="#userPanel">退出登入</a></li>
                    </ol>
                    <%--功能连接对应的内容--%>
                    <div class="panel-group" id="userPanel" role="tablist" aria-multiselectable="true">
                        <div class="panel">
                            <div id="collapse1" class="panel-collapse collapse" role="tabpanel">
                                1
                            </div>
                        </div>

                        <%--账号设置--%>
                        <div class="panel">
                            <div id="collapse_UserSetting" class="panel-group collapse" role="tabpanel">

                                <%--用户设置开始--%>
                                <div class="panel-group" id="UserSettingPanelGroup" role="tablist" aria-multiselectable="true">

                                    <%--邮箱--%>
                                    <div class="panel">
                                        <div class="panel-heading" role="tab" id="UserSetting_headingOne" data-toggle="collapse" data-parent="#UserSettingPanelGroup" aria-expanded="true" aria-controls="UserSetting_Mail">
                                            <h4 class="panel-title text-center">
                                                <a role="button" data-toggle="collapse" data-parent="#UserSettingPanelGroup" href="#UserSetting_Mail" aria-expanded="true" aria-controls="collapseOne">邮箱
                                                </a>
                                            </h4>
                                        </div>

                                        <div id="UserSetting_Mail" class="panel-collapse collapse " role="tabpanel" aria-labelledby="headingOne">
                                            <div class="panel-body">
                                                <%--第一个表单,修改邮箱--%>
                                                <div class="col-sm-11 col-sm-offset-1">
                                                    <form class="form-horizontal" id="mailForm">
                                                        <div class="form-group">
                                                            <label class="col-sm-3 control-label">当前邮箱</label>
                                                            <div class="col-sm-9">
                                                                <p class="form-control-static"><%=this.CurrentUser.EMail %></p>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="newEmail" class="col-sm-3 control-label">新邮箱</label>
                                                            <div class="col-sm-9">
                                                                <input class="form-control" id="newEmail" name="newEmail">
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="password" class="col-sm-3 control-label">登入密码</label>
                                                            <div class="col-sm-9">
                                                                <input type="password" class="form-control" name="password">
                                                            </div>
                                                        </div>



                                                        <div class="form-group">
                                                            <div class="col-sm-offset-3 col-sm-5">
                                                                <button type="submit" class="btn btn-default">保存</button>
                                                            </div>
                                                        </div>

                                                        <div class="col-sm-offset-3 col-sm-9" id="changeMailReturnMessage">
                                                        </div>


                                                    </form>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel">
                                        <div class="panel-heading" role="tab" id="headingThree" data-toggle="collapse" data-parent="#UserSettingPanelGroup" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                                            <h4 class="panel-title text-center">
                                                <a class="collapsed" role="button" data-toggle="collapse" data-parent="#UserSettingPanelGroup" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">密码
                                                </a>
                                            </h4>
                                        </div>

                                        <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                                            <div class="panel-body">

                                                <div class="col-sm-11 col-sm-offset-1">
                                                    <form class="form-horizontal " id="passWordForm">
                                                        <div class="form-group text-left">
                                                            <label for="currentPassword" class="col-sm-3 control-label  ">当前密码</label>
                                                            <div class="col-sm-9">
                                                                <input type="password" class="form-control" id="currentPassword" name="currentPassword">
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="newPassword" class="col-sm-3 control-label">新密码</label>
                                                            <div class="col-sm-9">
                                                                <input type="password" class="form-control" id="newPassword" name="newPassword">
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="newPassword2" class="col-sm-3 control-label">确认密码</label>
                                                            <div class="col-sm-9">
                                                                <input type="password" class="form-control" id="newPassword2" name="newPassword2">
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-offset-3 col-sm-9">
                                                                <button type="submit" class="btn btn-default">保存</button>
                                                            </div>
                                                        </div>

                                                        <div class="col-sm-offset-3 col-sm-9" id="changePassWordReturnMessage">
                                                        </div>
                                                    </form>
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                    <%--头像--%>
                                    <div class="panel">
                                        <div class="panel-heading" role="tab" id="headingFour" data-toggle="collapse" data-parent="#UserSettingPanelGroup" href="#collapseFour" aria-expanded="false" aria-controls="collapseFour">
                                            <h4 class="panel-title text-center">
                                                <a class="collapsed" role="button" data-toggle="collapse" data-parent="#UserSettingPanelGroup" href="#collapseFour" aria-expanded="false" aria-controls="collapseFour">头像
                                                </a>
                                            </h4>
                                        </div>

                                        <div id="collapseFour" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                                            <div class="panel-body center-block">
                                                <%=Avatar %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="panel ">
                            <div id="collapse3" class="panel-collapse collapse" role="tabpanel">
                            </div>
                        </div>
                    </div>


                </div>

                <!-- 标签切换按钮-->
                <ul class="nav nav-tabs pull-right" id="pictureTabs" role="tablist">
                    <%--<li role="presentation" ><a href="#singlePictureModel" aria-controls="singlePictureModel" role="tab" data-toggle="tab">单图</a></li>--%>
                    <li role="presentation" class="active"><a href="#userPictureModel" aria-controls="userPictureModel" role="tab" data-toggle="tab">我的上传</a></li>
                    <li role="presentation"><a href="#userCollectionModel" aria-controls="userCollectionModel" role="tab" data-toggle="tab">我的收藏</a></li>
                </ul>


                <%--<button class="btn btn-primary btn-lg" data-toggle="modal" data-target="#pictureDetailModal">
                    Launch demo modal
                </button>--%>
            </div>
        </div>


        <%--标签导航内容--%>
        <div class="tab-content">


            <div role="tabpanel" class="tab-pane  active " id="userPictureModel">
                <div class="row masonry-container">
                </div>
            </div>


            <div role="tabpanel" class="tab-pane" id="userCollectionModel">
                <div class="row masonry-container">
                </div>
            </div>
        </div>


    </div>


    <%--弹出层--%>
    <!-- Modal -->
    <div class="modal fade" id="pictureDetailModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <div class="row">
                        <div class="col-xs-6 col-md-1 modalFace">
                            <a href="#">
                                <img src="http://localhost:8080/uc_server/avatar.php?uid=6&type=virtual&size=small">
                            </a>
                        </div>
                        <div class="col-xs-6 col-md-9">
                            <div class="modalUsername">
                            </div>
                            <div class="modalUploadDate">
                            </div>
                        </div>
                    </div>

                </div>

                <div class="modal-body row">
                    <div class="col-xs-12 summary"></div>
                    <div class="col-xs-12">
                        <img src="" class="img-thumbnail center-block" style="margin-top: 20px">
                    </div>

                    <div class="col-xs-12" id="modalTags">
                    </div>

                </div>

                <div class="modal-footer">
                    <ol class=" text-left" id="picLinks">
                        <li class="btn btn-default"><i class="glyphicon glyphicon-comment"></i>评论<span></span></li>
                        <li class="btn btn-default"><i class="glyphicon glyphicon-heart"></i>收藏<span></span></li>
                        <li class="btn btn-default"><i class="glyphicon glyphicon-pencil"></i>编辑</li>
                        <li class="btn btn-default"><i class="glyphicon glyphicon-trash"></i>删除</li>
                    </ol>

                    <div class="modalComment">
                        <div class="row">
                            <div class="col-xs-6 col-md-1 modalFace">
                                <a href="#">
                                    <img src="http://localhost:8080/uc_server/avatar.php?uid=6&type=virtual&size=small">
                                </a>
                            </div>
                            <div class="col-xs-6 col-md-10 text-left">
                                <div class="modalUsername">
                                    用户名:66666666666666666666666666666666666666666666666666666666666
                                </div>
                                <div class="modalUploadDate">
                                    2015-9-26 15:57:42<span class="glyphicon glyphicon-remove"></span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6 col-md-1 modalFace">
                                <a href="#">
                                    <img src="http://localhost:8080/uc_server/avatar.php?uid=6&type=virtual&size=small" alt="...">
                                </a>
                            </div>
                            <div class="col-xs-6 col-md-9 text-left">
                                <div class="modalUsername">
                                    用户名:6666666666666666666666
                                </div>
                                <div class="modalUploadDate">
                                    2015-9-26 15:57:42
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6 col-md-1 modalFace">
                                <a href="#">
                                    <img src="http://localhost:8080/uc_server/avatar.php?uid=6&type=virtual&size=small" alt="...">
                                </a>
                            </div>
                            <div class="col-xs-6 col-md-9 text-left">
                                <div class="modalUsername">
                                    用户名:6666666666666666666666
                                </div>
                                <div class="modalUploadDate">
                                    2015-9-26 15:57:42
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="input-group">
                        <textarea id="txtComment" class="form-control " style="resize: none;" rows="1"></textarea>
                    </div>
                    <span class="input-group-btn"></span>
                    <button class="btn btn-primary" id="btnCommentSubmit" style="display: none" type="button">发表</button>

                    <!-- /input-group -->
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->



    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->

    <script src="/assets/js/lib/jquery-1.10.2.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="assets/js/lib/BootStrap/js/bootstrap.min.js"></script>
    <script src="assets/js/lib/Masonry.js"></script>
    <%--<script src="assets/js/lib/imagesLoaded.js"></script>--%>
    <script src="assets/js/lib/dateFormat.js"></script>
    <script type="text/javascript">

        <%--图片id藏在了弹出层的一个叫data-pid的属性中--%>

        //=========全局变量
        //分批次加载图片用到的变量
        //var singlePictureModelCount = 0;
        //var userPictureLoadCount = 0;

        //仅仅用于瀑布流显示,第一次加载是在masonry执行前,但AJAX内的一个方法则会保存
        //因此用一个全局变量来避免
        var isFirstLoad = { 0: true, 1: true };
        var loadComplete = { 0: false, 1: false };

        //=========全局变量结束

        //窗体加载完成时
        $(function () {
            //给各个修改表单添加修改事件
            var mailFrom = $("#mailForm");
            var passWordForm = $("#passWordForm");
            userSetting(mailForm, 1);
            userSetting(passWordForm, 2);
            //图片分批次加载事件绑定
            pictureLoadEvent();

            //点击切换按钮,绑定瀑布流效果
            waterFallFlow();

            //弹出层事件绑定
            modalEvents();


        });


        //弹出层相关事件绑定
        function modalEvents() {
            //评价留言框获得焦点事件绑定
            commentTextFocus();
            //评价上传事件绑定
            commentSubmit();
            //弹出层关闭事件绑定
            modalClose();
            //弹出层图片链接点击事件绑定
            picLinksClickEvent();

        }

        //触发Masonry绑定
        function masonryOn() {
            var $container = $('.masonry-container');
            $container.masonry({
                //columnWidth: 20,
                itemSelector: '.item'
            });
        }

        //点击切换按钮,绑定瀑布流效果
        function waterFallFlow() {
            var $container = $('.masonry-container');
            var $picLinks = $("#picLinks");
            //在切换展现模式的时候调用一次masonry,使图片自动排版
            $('a[data-toggle=tab]').each(function () {
                var $this = $(this);
                $this.on('shown.bs.tab', function (e) {
                    //不同标签页展现不同按钮
                    var lis = $picLinks.find('li');
                    switch (e.currentTarget.innerHTML) {
                        case "我的收藏":
                            console.log(1);
                            lis.eq(2).css("display", "none");
                            lis.eq(3).css("display", "none");
                            break;
                        case "我的上传":
                            console.log(2);
                            lis.eq(2).css("display", "inline");
                            lis.eq(3).css("display", "inline");
                            break;
                        default:
                    }

                    masonryOn();

                });
            });
        }

        //图片加载事件绑定
        function pictureLoadEvent() {
            //确保执行这个方法的时候页面没有无关的，class有tab-pane的标签

            //测试,默认先用单页模式
            //var singlePictureModelObj = $("#singlePictureModel");
            //var userPictureObj = $("#userPictureModel").find("div")[0];

            //首次先加载一次
            //单图模式
            //loadPicture(singlePictureModelCount, 5, singlePictureModelObj, true);
            //singlePictureModelCount++;


            //把加载次数动态的保存在Dom对象中

            var tabs = $(".tab-pane");
            tabs.each(function (i) {
                this.loadCount = 0;
                loadPicture(this.loadCount, 10, this, i);
                this.loadCount++;
            });

            // loadPicture(userPictureLoadCount, 5, userPictureObj, false);
            //userPictureLoadCount++;

            $(window).scroll(function () {
                //console.log($(this).height(), $(document).height(), $(this).scrollTop(), $(document).scrollTop());
                //窗口顶部到页面顶部的距离
                var scrollTop = $(this).scrollTop();
                //整个页面的高度
                var pageHeight = $(document).height();
                //窗口高度
                var clientHeight = $(this).height();
                if (scrollTop + clientHeight > pageHeight - 300) {
                    /*
                    //当到达页面底部时候
                    if (singlePictureModelObj.hasClass("active")) {
                        //单图模式 
                        //loadPicture(singlePictureModelCount, 5, singlePictureModelObj, true);
                        //singlePictureModelCount++;
                       
                    } else {
                        loadPicture(userPictureLoadCount, 5, userPictureObj, false);
                        userPictureLoadCount++;
                    }
                    */
                    tabs.each(function (i) {
                        if ($(this).hasClass("active")) {
                            loadPicture(this.loadCount, 10, this, i);
                            this.loadCount++;
                        }
                    });

                }
            });
        }

        //表单提交事件绑定
        function userSetting(obj, formType) {
            //formType  1 邮箱 2密码
            $(obj).submit(function () {
                var data = $(this).serializeArray();
                data.push({ "name": "formType", "value": formType });
                $.ajax({
                    //===请求地址===
                    url: "/account/UserSetting.ashx",
                    //===请求设置===
                    data: data,//传输数据,可以是字符串,也可以是json
                    success: function (data) {
                        var alertType;
                        switch (data.stateCode) {
                            case 1: alertType = "success";
                                //根据不同的表单对其修改成功情况下的不同动作
                                if (formType == 1) {
                                    var newMail = $("#newEmail").val();
                                    $(obj).find("p").eq(0).text(newMail);
                                }
                                break;
                            case 2: alertType = "info";
                                break;
                            case 3: alertType = "danger";
                                break;
                            default:
                        }
                        //添加提示信息
                        switch (formType) {
                            case 1:
                                $("#changeMailReturnMessage").html("<div class=\"alert alert-" + alertType + " alert-dismissible fade in\" role=\"alert\"> <button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button><strong>" + data.returnMessage + "</strong></div>")
                                break;
                            case 2:
                                $("#changePassWordReturnMessage").html("<div class=\"alert alert-" + alertType + " alert-dismissible fade in\" role=\"alert\"> <button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button><strong>" + data.returnMessage + "</strong></div>")
                                break;
                            default:
                        }
                        //把所有的输入框清空
                        $(".form-control").val("");

                    },
                    type: "post",//请求方法
                    dataType: "json",//返回的数据类型
                    cache: false,//是否使用缓存,(默认: true,dataType为script和jsonp时默认为false)  
                    contentType: "application/x-www-form-urlencoded"//发送信息至服务器时内容编码类型,这里是默认值
                })
                return false;
            })
        }

        //分批次加载图片
        function loadPicture(loadCount, loadSize, obj, modelType) {

            //var $container = $('.masonry-container');
            var $container = $($(obj).find("div")[0]);

            if (loadComplete[modelType] == true) {
                return;
            }

            $.ajax({
                //===请求地址===
                url: "handler/UserPictureAsy.ashx",
                //===请求设置===
                data: { "loadCount": loadCount, "loadSize": loadSize, modelType: modelType },//传输数据,可以是字符串,也可以是json
                success: function (data) {//请求成功时的回调函数,success(data, textStatus, jqXHR)
                    var isFirst = false;
                    //$.each(isFirstLoad, function (i, e) {
                    //    if (e == true) {
                    //        isFirst = true;
                    //    }
                    //});


                    ////不同模式的图片追加
                    //if (isSingleModel) {
                    //    //单图模式
                    //    for (var i in data) {
                    //        $(obj).append("<div class=\"col-xs-12\"><div class=\"thumbnail col-xs-12\"><div class=\"col-xs-10 col-xs-offset-1\" style=\"margin-top: 10px; margin-bottom: 20px\"> <div  style=\"border-bottom: 1px solid #e5e5e5\"><a href=\"#\" class=\"date\">2015-04-28</a></div>  <img src=\"" + data[i].imgUrl + "\" class=\"img-thumbnail\" style=\"margin-top: 20px\" /></div></div>");
                    //    }
                    //} else {
                    //三图模式
                    switch (modelType) {

                        case 0:
                        case 1:
                            pictureJsonDataProcess(data, isFirst, $container, modelType);
                            break;
                        default:

                    }

                },

                type: "post",//请求方法
                dataType: "json",//返回的数据类型
                cache: false,//是否使用缓存,(默认: true,dataType为script和jsonp时默认为false)  
                contentType: "application/x-www-form-urlencoded"//发送信息至服务器时内容编码类型,这里是默认值
            });
        }



        //对加载的图片的JSON数据处理
        function pictureJsonDataProcess(data, isFirst, $container, modelType) {
            if (data.length == 0) {
                loadComplete[modelType] = true;
                return;
            }
            for (var i in data) {
                //加载开始先设置高宽撑住页面
                //图片加载完成在删除高宽,保证屏幕缩小时候的显示
                var newElement = $("<div data-pId=\"" + data[i].pId + "\" class=\"col-sm-4 col-xs-12 item\"><div class=\"thumbnail\"><img style=\"width:" + 193 + "px;height:" + getHeight(data[i].height, data[i].width) + "px\"src=\"" + data[i].imgUrl + "\" /></div></div>");
                var img = newElement.find("img");
                img.load(function () {
                    $(this).removeAttr("style");
                });
                //style=\"height:"+data[i].height+"px;width:"+data[i].width+"px\"
                if (isFirst) {
                    $container.append(newElement);
                } else {
                    $container.append(newElement).masonry('appended', newElement);
                }
                //给新添加的图片添加点击事件
                $(newElement).click(function () {
                    $('#pictureDetailModal').modal('toggle');
                    loadPictureDetail($(this).attr("data-pId"));
                })

            }
            //每次刷完图片都要重新设置下
            //$container.imagesLoaded(function () {
            $container.masonry({
                //columnWidth: 20,
                itemSelector: '.item'
            });
            //});
            isFirstLoad[modelType] = false;
        }

        function getHeight(height, width) {
            return (193 / width) * height;
        }



        //===========弹出层

        //载入图片详细数据到弹出层
        function loadPictureDetail(pId) {
            $.getJSON("handler/LoadPictureDetail.ashx", { pId: pId }, function (data) {
                var modal = $("#pictureDetailModal");
                var header = modal.find(".modal-header");
                var body = modal.find(".modal-body");
                var footer = modal.find(".modal-footer");
                var picLinksLis = $("#picLinks li");
                //把图片的PId藏在弹出层
                modal.attr("data-pId", pId);

                //图片上传者信息
                header.find("img").attr("src", data.userInfo.userFace);//头像
                header.find(".modalUsername").html(data.userInfo.userName);//用户名
                //图片信息
                header.find(".modalUploadDate").html(eval(data.uploadDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)")).pattern("yyyy-MM-dd  hh:mm:ss"));
                body.find(".summary").html(data.summary);
                body.find("img").attr("src", data.url);
                //构建标签
                var ol = $("<ol class=\"breadcrumb\"></ol>");
                for (var i in data.tags) {
                    var li = $("<li><a href=\"#\">" + "#" + data.tags[i].TagName + "</a></li>");
                    ol.append(li);
                }
                //载入标签
                body.find("#modalTags").html("").append(ol);

                //载入收藏数
                if (data.collectCount != 0) {
                    picLinksLis.eq(1).find("span").text(data.collectCount);
                } else {
                    picLinksLis.eq(1).find("span").text("");
                }
                //载入是否收藏
                if (data.isCollect) {
                    picLinksLis.eq(1).find("i").css("color", "red");
                } else {
                    picLinksLis.eq(1).find("i").removeAttr("style");
                }


                //载入评论
                var comment = footer.find(".modalComment");
                comment.html("");
                for (var i in data.commentlist) {
                    //如果这个用户登入了，那么他的评论旁边会显示删除的标记
                    var temp = "<div class=\"row\"><div class=\"col-xs-6 col-md-1 modalFace\"><a href=\"#\"><img src=" + data.commentlist[i].userFace + "></a></div><div class=\"col-xs-6 col-md-10 text-left\"><div class=\"modalUsername\">" + data.commentlist[i].userName + ":" + data.commentlist[i].content + "</div><div class=\"modalUploadDate\">" + eval(data.commentlist[i].postDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)")).pattern("yyyy-MM-dd  hh:mm:ss") + (data.commentlist[i].isMe == true ? "<span class=\"glyphicon glyphicon-remove\" data-cId=\"" + data.commentlist[i].cId + "\"></span>" : "") + "</div></div></div>";
                    comment.append(temp);
                }
                //载入评论数
                if (data.commentlist.length != 0) {
                    picLinksLis.eq(0).find("span").text(data.commentlist.length);
                } else {
                    picLinksLis.eq(0).find("span").text("");
                }

                //删除标记鼠标移动到上面显示
                deleteTagHover();
                //删除标记点击事件
                var obj = $(".modalComment .row span");
                deleteComment(obj);



            });
        }

        //评论文本框得到焦点事件
        function commentTextFocus() {

            var txtComment = $("#txtComment");
            var btnCommentSubmit = $("#btnCommentSubmit");
            txtComment.focus(function () {
                txtComment.attr('rows', '3');
                btnCommentSubmit.css('display', 'inline-block');
            });

        }

        //留言上传件事
        function commentSubmit() {
            $("#btnCommentSubmit").click(function () {
                var comment = $("#txtComment").val();
                var pId = $("#pictureDetailModal").attr("data-pId");
                $.getJSON("/handler/SubmitComment.ashx", {
                    pId: pId,
                    comment: comment
                }, function (data) {
                    if (data.isSubmit == true) {
                        var temp = $("<div class=\"row\"><div class=\"col-xs-6 col-md-1 modalFace\"><a href=\"#\"><img src=" + data.userFace + "></a></div><div class=\"col-xs-6 col-md-10 text-left\"><div class=\"modalUsername\">" + data.userName + ":" + comment + "</div><div class=\"modalUploadDate\">" + new Date().pattern("yyyy-MM-dd  hh:mm:ss") + "<span class=\"glyphicon glyphicon-remove\" data-cId=\"" + data.cId + "\"></span>" + "</div></div></div>");
                        $("#pictureDetailModal .modalComment").prepend(temp);
                        temp.css("display", "none").fadeIn();

                        //评论数增加
                        commentCountChange(true);

                        //鼠标移入移出事件绑定
                        deleteTagHover();
                        //删除事件绑定
                        deleteComment(temp.find("span"));

                    } else {
                        switch (data.errorCode) {
                            case "1":
                                alert("请先登入");
                                //location.href = "/Index.aspx";
                                break;
                            case "2":
                                alert("未知错误");
                                break;
                            case "3":
                                alert("评论过长");
                                break;
                            default:
                        }
                    }
                });
                //清空评论栏
                $("#txtComment").val("");
                //恢复评论栏
                commentTxtReset();
                return false;
            });
        }

        //鼠标移动到某评价那一栏，如果评论者是本人显示出删除标志,和删除标记点击事件
        function deleteTagHover() {
            $(".modalComment .row:has(span)").hover(function () {
                $(this).find("span").css("display", "inline");
            }, function () {
                $(this).find("span").css("display", "none");
            });
        }

        //删除评价事件绑定
        function deleteComment(obj) {
            obj.one("click", function () {
                var $this = $(this);
                var cId = $this.attr("data-cId");
                $.getJSON("handler/DeleteComment.ashx", { cId: cId }, function (data) {
                    if (data.IsDelete == false) {
                        switch (data.errorCode) {
                            case "1":
                                alert("用户未登入");
                                //location.href = "/Index.aspx";
                                break;
                            case "2":
                                alert("该用户无此评论");
                                break;
                            case "3":
                                alert("未知错误");
                                break;
                            default:
                        }
                    } else {
                        var parent = $this.parents(".row");
                        parent.fadeOut();
                        commentCountChange(false);
                    }
                });
            });

        }

        //评价数加1或减1
        function commentCountChange(isAdd) {
            var links = $("#picLinks li");
            var span = links.eq(0).find("span");
            var commentCount = span.html();
            if (isAdd) {
                if (commentCount == "") {
                    commentCount = 1;
                } else {
                    commentCount = parseInt(commentCount);
                    commentCount++;
                }
                span.html(commentCount);

            } else {
                commentCount = parseInt(commentCount);
                commentCount--;
                if (commentCount==0) {
                    span.html("");
                } else {
                    span.html(commentCount);
                }
            }
        }



        //评论框变小，按钮隐藏
        function commentTxtReset() {
            $("#txtComment").attr("rows", "1");
            $("#btnCommentSubmit").css("display", "none");
        }

        //图片下方各按钮点击事件
        function picLinksClickEvent() {
            var links = $("#picLinks li");

            //评论按钮
            links.eq(0).click(function () {
                var txtComment = $("#txtComment");
                txtComment.focus();
            });

            //收藏按钮
            links.eq(1).click(function () {
                var heart = $(this).find("i");
                var isCollect = heart.css("color") == 'rgb(255, 0, 0)';//是否收藏了
                var pId = $("#pictureDetailModal").attr("data-pid");
                //对红心处理的两个函数
                var heartTurnRed = function () {
                    heart.css("color", "red");
                    var collectCount = heart.next().html();
                    if (collectCount == "") {
                        collectCount = 1;
                    } else {
                        collectCount = parseInt(collectCount);
                        collectCount++;
                    }
                    heart.next().html(collectCount);
                };
                var redHeartClear = function () {
                    heart.removeAttr("style");
                    var collectCount = heart.next().html();
                    collectCount = parseInt(collectCount);
                    collectCount--;
                    if (collectCount == 0) {
                        heart.next().html("");
                    } else {
                        heart.next().html(collectCount);
                    }
                };
                $.getJSON("/handler/CollectPicture.ashx", { pId: pId, isCollect: isCollect }, function (data) {
                    if (data.isCollect == false) {
                        switch (data.errorCode) {
                            case 1:
                                alert("请先登入");
                                //location.href = "/Index.aspx ";
                                break;
                            case 2:
                                alert("图片不存在");
                                break;
                            case 3:
                                //爱心变红
                                heartTurnRed();
                                break;
                            case 4:
                                //爱心变不红
                                redHeartClear();
                                break;
                            case 5:
                                alert("未知错误");
                                break;
                            default:
                        }
                    } else {
                        if (!isCollect) {
                            heartTurnRed();
                        } else {
                            redHeartClear();
                        }
                    }

                });
            })



        }

        //当弹出层关闭事件绑定
        function modalClose() {
            $("#pictureDetailModal").on('hidden.bs.modal', function (e) {
                commentTxtReset();
            })
        }

        //============弹出层结束

    </script>
</body>
</html>
