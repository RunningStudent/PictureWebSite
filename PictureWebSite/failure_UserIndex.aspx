<%@ Page Title="" Language="C#" MasterPageFile="~/shared/Layout.Master" AutoEventWireup="true" CodeBehind="failure_UserIndex.aspx.cs" Inherits="PictureWebSite.failure_UserIndex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="assets/js/lib/BootStrap/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #ECEBE9;
        }

        .userPanel {
            margin-top: 150px;
            background-color: white;
        }

            .userPanel .thumbnail {
                position: relative;
                top: -30px;
                border-width: 0px;
            }

        .usePFristRow a.btn {
            float: right;
            margin-right: 20px;
            margin-top: 20px;
        }

        .usePFristRow img {
            width: 100%;
        }

        .main {
            margin-top: 50px;
        }

        /*导航栏微调*/
        #head .classifyDiv {
            right: -100px;
        }
        /*Boostrap全局属性覆盖*/
        .h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6 {
            font-weight: bold;
        }

        #signin * {
            box-sizing: content-box;
            -webkit-box-sizing: content-box;
        }

        button, input, select, textarea {
            font-size: medium;
        }

        .input-group #searchTxt {
            z-index: inherit;
        }

        .input-group button.btn.btn-default {
            z-index: inherit;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="container userPanel" id="userPanel">
        <div class="row usePFristRow">
            <div class="col-sm-2 col-xs-4">
                <!--用户头像-->
                <div class="thumbnail">
                    <%-- <img src="" />--%>
                    <img src="<%=CurrentUser.UserFacePathLarge %>" class="img-responsive" alt="Responsive image">
                    <div class="caption">
                        <div class="nav">
                        </div>
                    </div>
                </div>
            </div>
            <a href=""class="btn btn-default">用户设置</a>
         <%--   <button class="btn btn-default" type="submit">用户设置</button>--%>
        </div>


        <div class="row">
            <div class="col-sm-5">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs" id="myTabs" role="tablist">
                    <li role="presentation" class="active"><a href="#home" aria-controls="home" role="tab" data-toggle="tab">我的图片</a></li>
                    <li role="presentation"><a href="#profile" aria-controls="profile" role="tab" data-toggle="tab">我的评论</a></li>
                    <li role="presentation"><a href="#messages" aria-controls="messages" role="tab" data-toggle="tab">我的收藏</a></li>
                    <li role="presentation"><a href="#settings" aria-controls="settings" role="tab" data-toggle="tab">没想好</a></li>
                </ul>
            </div>

            <%--搜索栏--%>
            <div class="col-sm-5 col-sm-offset-2">
                <div class="input-group">
                    <input type="text" class="form-control" id="searchTxt" placeholder="搜索我的图片">
                    <span class="input-group-btn">
                        <button class="btn btn-default" type="button"><span class="glyphicon glyphicon-search" aria-hidden="true"></span></button>
                    </span>
                </div>
            </div>
        </div>
    </div>

    <div class="container main">
        <div>

            <!-- Tab panes -->
            <div class="tab-content">

                <!--第一个标签内容-->
                <div role="tabpanel" class="tab-pane active" id="home">
                    <div class="row masonry-container">
                        <%foreach (var item in this.List)
                          {%>
                        <div class="col-sm-3 col-xs-12 item">
                            <div class="thumbnail">
                                <img src="<%=item.LargeImgPath %>" />
                                <div class="caption">
                                    <p>
                                        <a href="#" class="btn btn-primary" role="button">Button</a>
                                        <a href="#" class="btn btn-default" role="button">Button</a>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <%} %>
                    </div>
                </div>

                <div role="tabpanel" class="tab-pane" id="profile">空</div>
                <div role="tabpanel" class="tab-pane" id="messages"></div>
                <div role="tabpanel" class="tab-pane" id="settings"></div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
    <script src="assets/js/lib/BootStrap/js/bootstrap.min.js"></script>
    <script src="assets/js/lib/Masonry.js"></script>
    <script src="assets/js/lib/imagesLoaded.js"></script>

    <script type="text/javascript">
        //修改导航栏宽度
        var main = document.getElementById("userPanel");
        var head = $(".topBar")[0];
        headWidth(main, head);
        $(window).resize(function () {
            headWidth(main, head);
        });
        //图片加载失败时的处理
        var $container = $('.masonry-container');
        $container.imagesLoaded(function () {
            $container.masonry({
                columnWidth: '.item',
                itemSelector: '.item'
            });
        });
        //瀑布流
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
    </script>
</asp:Content>
