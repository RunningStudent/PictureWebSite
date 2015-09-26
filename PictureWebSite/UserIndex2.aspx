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

        @media (max-width:550px) {
            #secondShow {
                display: none;
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


        .modal-content{
            border-radius:0px;
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
        #singlePictureModel img {
            width: 100%;
        }

        #singlePictureModel div.col-xs-12 {
            padding-left: 0px;
            padding-right: 0px;
        }

        #userPanel .panel {
            border-width: 0px;
            margin: 0px;
        }


        #mycamera {
            margin: 0px auto;
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
                        <div class="panel ">
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
                    <li role="presentation" class="active"><a href="#singlePictureModel" aria-controls="singlePictureModel" role="tab" data-toggle="tab">单图展示</a></li>
                    <li role="presentation" id="secondShow"><a href="#threePictureModel" aria-controls="threePictureModel" role="tab" data-toggle="tab">三图模式</a></li>
                </ul>


                <button class="btn btn-primary btn-lg" data-toggle="modal" data-target="#myModal">
                    Launch demo modal
                </button>
            </div>
        </div>


        <%--标签导航内容--%>
        <div class="tab-content">
            <!--单图片显示-->
            <div role="tabpanel" class="tab-pane active" id="singlePictureModel">

                <%--                <%foreach (var item in this.List)
                  {%>
                <div class="col-xs-12 ">
                    <div class="thumbnail">
                        <img src="<%=item.LargeImgPath %>" />
                    </div>
                </div>
                <%} %>--%>


                <%--<%foreach (var item in this.List)
                  {%>

                <div class="thumbnail col-xs-12 ">
                    <div class="col-xs-10 col-xs-offset-1" style="margin-top: 10px; margin-bottom: 20px">
                        <div class="info" style="border-bottom: 1px solid #e5e5e5">
                            <a href="#" class="date">2015-04-28</a>
                        </div>
                        <img src="<%=item.LargeImgPath %>" class="img-thumbnail" style="margin-top: 20px" />
                    </div>
                </div>
                <%} %>--%>

            </div>

            <%--三图片显示--%>
            <div role="tabpanel" class="tab-pane" id="threePictureModel">
                <div class="row masonry-container">
                    <%--<%foreach (var item in this.List)
                      {%>
                    <div class="col-sm-4 col-xs-12 item">
                        <div class="thumbnail">
                            <img src="<%=item.LargeImgPath %>" />
                            
                        </div>
                    </div>
                    <%} %>--%>
                </div>
            </div>

        </div>


    </div>


    <%--弹出层--%>
    <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel">Modal title</h4>
                </div>

                <div class="modal-body">
                    
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
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
    <script src="assets/js/lib/imagesLoaded.js"></script>
    <script type="text/javascript">



        //=========全局变量
        //分批次加载图片用到的变量
        var singlePictureModelCount = 0;
        var threePicutreModelCount = 0;
        //仅仅用于瀑布流显示,三图模式第一次加载是在masonry执行前,但AJAX内的一个方法则会保存
        //因此用一个全局变量来避免
        var isFirstLoad = true;

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

        });


        //点击切换按钮,绑定瀑布流效果
        function waterFallFlow() {
            var $container = $('.masonry-container');
            //在切换展现模式的时候调用一次masonry,使图片自动排版
            $('a[data-toggle=tab]').each(function () {
                var $this = $(this);
                $this.on('shown.bs.tab', function () {
                    $container.imagesLoaded(function () {
                        $container.masonry({
                            columnWidth: '.item',
                            itemSelector: '.item'
                        });
                    });
                });
            });
        }



        //图片加载事件绑定
        function pictureLoadEvent() {

            //测试,默认先用单页模式
            var singlePictureModelObj = $("#singlePictureModel");
            var threePictureModelObj = $("#threePictureModel").find("div")[0];

            //首次先加载一次

            /*
            //单图模式
            loadPicture(singlePictureModelCount, 5, singlePictureModelObj, true);
            singlePictureModelCount++;
            */

            loadPicture(threePicutreModelCount, 5, threePictureModelObj, false);
            threePicutreModelCount++;

            $(window).scroll(function () {
                //console.log($(this).height(), $(document).height(), $(this).scrollTop(), $(document).scrollTop());
                //窗口顶部到页面顶部的距离
                var scrollTop = $(this).scrollTop();
                //整个页面的高度
                var pageHeight = $(document).height();
                //窗口高度
                var clientHeight = $(this).height();
                if (scrollTop + clientHeight > pageHeight - 300) {
                    //当到达页面底部时候
                    if (singlePictureModelObj.hasClass("active")) {
                        /*单图模式
                        loadPicture(singlePictureModelCount, 5, singlePictureModelObj, true);
                        singlePictureModelCount++;
                        */
                    } else {
                        loadPicture(threePicutreModelCount, 5, threePictureModelObj, false);
                        threePicutreModelCount++;
                    }
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
        function loadPicture(loadCount, loadSize, obj, isSingleModel) {
            $.ajax({
                //===请求地址===
                url: "handler/UserUploadPictureAsy.ashx",
                //===请求设置===
                data: { "loadCount": loadCount, "loadSize": loadSize },//传输数据,可以是字符串,也可以是json
                success: function (data) {//请求成功时的回调函数,success(data, textStatus, jqXHR)

                    //不同模式的图片追加
                    if (isSingleModel) {
                        //单图模式

                        for (var i in data) {
                            $(obj).append("<div class=\"col-xs-12\"><div class=\"thumbnail col-xs-12\"><div class=\"col-xs-10 col-xs-offset-1\" style=\"margin-top: 10px; margin-bottom: 20px\"> <div  style=\"border-bottom: 1px solid #e5e5e5\"><a href=\"#\" class=\"date\">2015-04-28</a></div>  <img src=\"" + data[i].imgUrl + "\" class=\"img-thumbnail\" style=\"margin-top: 20px\" /></div></div>");

                        }
                    } else {
                        //三图模式

                        var $container = $('.masonry-container');
                        for (var i in data) {

                            var newElement = $("<div class=\"col-sm-4 col-xs-12 item\"><div class=\"thumbnail\"><img src=\"" +
                                data[i].imgUrl + "\" /></div></div>");
                            if (isFirstLoad) {
                                $container.append(newElement);
                            } else {
                                $container.append(newElement).masonry('appended', newElement);
                            }

                        }
                        //每次刷完图片都要重新设置下
                        $container.imagesLoaded(function () {
                            $container.masonry({
                                columnWidth: '.item',
                                itemSelector: '.item'
                            });
                        });
                        isFirstLoad = false;
                    }
                },
                type: "post",//请求方法
                dataType: "json",//返回的数据类型
                cache: false,//是否使用缓存,(默认: true,dataType为script和jsonp时默认为false)  
                contentType: "application/x-www-form-urlencoded"//发送信息至服务器时内容编码类型,这里是默认值
            });

        }


    </script>
</body>
</html>
