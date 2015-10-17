<%@ Page Title="" Language="C#" MasterPageFile="~/shared/Layout.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="PictureWebSite.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="assets/js/lib/BootStrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="assets/css/index.css" rel="stylesheet" />
    <link href="assets/css/reset.css" rel="stylesheet" />

    <style type="text/css">
        /*BootStrap全局样式修改*/
        .breadcrumb {
            background-color: white;
        }

        .modal-content {
            border-radius: 0px;
        }

        /*面包屑导航分隔符*/
        .breadcrumb > li + li::before {
            content: "|";
        }

        /*样式覆盖*/
        #signin * {
            box-sizing: content-box;
            -webkit-box-sizing: content-box;
            -moz-box-sizing: content-box;
        }

        #signin input[type=checkbox], input[type=radio] {
            margin: 0;
            line-height: normal;
        }

        /*BootStrap全局样式修改结束*/
        /*弹出层样式*/
        .summary{
            margin-bottom:30px;
        }
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

        #modalTags{
            margin-top:25px;
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

        .modalUploadDate span {
            cursor: pointer;
        }

       
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <%--  --%>
    <div id="waterfallFlow" class="warpper"></div>
    <div id="loader"></div>

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
                        <img src="assets/img/detailPictureLoading.jpg" class="img-thumbnail center-block" style="margin-top: 20px">
                    </div>

                    <div class="col-xs-12" id="modalTags">
                    </div>

                </div>

                <div class="modal-footer">
                    <ol class=" text-left" id="picLinks">
                        <li class="btn btn-default"><i class="glyphicon glyphicon-comment"></i>评论<span></span></li>
                        <li class="btn btn-default"><i class="glyphicon glyphicon-heart"></i>收藏<span></span></li>
                        <%-- <li class="btn btn-default"><i class="glyphicon glyphicon-pencil"></i>编辑</li>--%>
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


    


   



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
    <script src="assets/js/lib/jquery.lazyload.js"></script>
    <script src="assets/js/function.js"></script>
    <script src="assets/js/newMask.js"></script>
    <script src="assets/js/lib/dateFormat.js"></script>
    <script src="assets/js/waterfallFlow.js"></script>
    <script src="assets/js/1.js"></script>

    <script src="assets/js/lib/BootStrap/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function modalEventBing() {
            //评价留言框获得焦点事件绑定
            commentTextFocus();
            //评价上传事件绑定
            commentSubmit();
            //弹出层关闭事件绑定
            modalClose();
            //弹出层图片链接点击事件绑定
            picLinksClickEvent();
            //点击出现图片上传弹出层事件
            plusBtnEven();



        }

        //点击图片执行图片详细信息弹出层相关函数
        function pictureClick(pId) {
            $('#pictureDetailModal').modal('toggle');
            loadPictureDetail(pId);
        }

        function plusBtnEven() {
            $("#plus").click(function () {
                var upLoadModal = $("#picUploadModal");
                upLoadModal.modal();
                    
            });
        }

        //===========图片上传
       


        //===========弹出层

        //载入图片详细数据到弹出层
        function loadPictureDetail(pId) {
            $.getJSON("handler/LoadPictureDetail.ashx", { pId: pId }, function (data) {
                var modal = $("#pictureDetailModal");
                var header = modal.find(".modal-header");
                var body = modal.find(".modal-body");
                var footer = modal.find(".modal-footer");
                var picLinksLis = $("#picLinks li");
                var img = body.find("img");
                //把图片的PId藏在弹出层
                modal.attr("data-pId", pId);

                //图片上传者信息
                header.find("img").attr("src", data.userInfo.userFace);//头像
                header.find(".modalUsername").html(data.userInfo.userName);//用户名
                //图片信息
                header.find(".modalUploadDate").html(eval(data.uploadDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)")).pattern("yyyy-MM-dd  hh:mm:ss"));
                body.find(".summary").html(data.summary);

                //清空图片路径,使就图片不会遗留下来
                img.attr("src", data.url);
                img.css('height', data.imgHeight);
                img.load(function () {
                    $(this).removeAttr("style");
                })


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
                $.getJSON("handler/SubmitComment.ashx", {
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
                            case 1:
                                alert("请先登入");
                                //location.href = "Index.aspx";
                                break;
                            case 2:
                                alert("未知错误");
                                break;
                            case 3:
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
                            case 1:
                                alert("用户未登入");
                                //location.href = "Index.aspx";
                                break;
                            case 2:
                                alert("该用户无此评论");
                                break;
                            case 3:
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
                if (commentCount == 0) {
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
                picturCollectProcess(this);
            })
        }


        //图片收藏处理
        function picturCollectProcess(link) {
            var heart = $(link).find("i");
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
            $.getJSON("handler/CollectPicture.ashx", { pId: pId, isCollect: isCollect }, function (data) {
                if (data.isCollect == false) {
                    switch (data.errorCode) {
                        case 1:
                            alert("请先登入");
                            //location.href = "Index.aspx ";
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
        }


        //当弹出层关闭事件绑定
        function modalClose() {
            $("#pictureDetailModal").on('hidden.bs.modal', function (e) {
                commentTxtReset();
                //把图片变成白板
                $("#pictureDetailModal .modal-body img").attr("src", "assets/img/detailPictureLoading.jpg")
            })
        }

        //============弹出层结束


    </script>
</asp:Content>
