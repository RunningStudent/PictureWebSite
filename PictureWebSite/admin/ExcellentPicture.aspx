<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcellentPicture.aspx.cs" Inherits="PictureWebSite.admin.ExcellentPicture" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/bootstrap.min.css?v=3.4.0" rel="stylesheet">
    <link href="css/font-awesome.min.css?v=4.3.0" rel="stylesheet">
    <link href="js/plugins/fancybox/jquery.fancybox.css" rel="stylesheet">
    <link href="css/animate.min.css" rel="stylesheet">
    <link href="css/style.min.css?v=3.2.0" rel="stylesheet">
</head>
<body class="gray-bg">
    <div class="wrapper wrapper-content">
        <div class="row">
            <div class="col-sm-12">
                <div class="ibox float-e-margins">
                    <div class="ibox-title">
                        <div class="ibox-tools">
                            <a class="collapse-link">
                                <i class="fa fa-chevron-up"></i>
                            </a>
                            <a class="dropdown-toggle" data-toggle="dropdown" href="basic_gallery.html#">
                                <i class="fa fa-wrench"></i>
                            </a>
                            <ul class="dropdown-menu dropdown-user">
                                <li><a href="basic_gallery.html#">选项1</a>
                                </li>
                                <li><a href="basic_gallery.html#">选项2</a>
                                </li>
                            </ul>
                            <a class="close-link">
                                <i class="fa fa-times"></i>
                            </a>
                        </div>
                    </div>

                    <div class="ibox-content" >

                       <%-- <%foreach (var item in list)
                          {%>
                        <a class="fancybox" data-fancybox-group="gallery" href="<%="../"+item.LargeImgPath %>">
                            <img src="<%="../"+Picture.Utility.CommonHelper.GetSmallImgPath(item.LargeImgPath) %>" />
                        </a>
                        <%} %>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!-- 全局js -->
    <script src="js/jquery-2.1.1.min.js"></script>
    <script src="js/bootstrap.min.js?v=3.4.0"></script>



    <!-- Peity -->
    <script src="js/plugins/peity/jquery.peity.min.js"></script>



    <!-- Fancy box -->
    <script src="js/plugins/fancybox/jquery.fancybox.js"></script>
    <script src="js/plugins/fancybox/jquery.fancybox.pack.js"></script>

    <script>
        $(document).ready(function () {
            picBox();
            pictureLoad();
            loadCount++;
            scrollLoadEvent();

        })
        var loadCount = 0;
        function scrollLoadEvent() {
            $(window).scroll(function () {
                //console.log($(this).height(), $(document).height(), $(this).scrollTop(), $(document).scrollTop());
                //窗口顶部到页面顶部的距离
                var scrollTop = $(this).scrollTop();
                //整个页面的高度
                var pageHeight = $(document).height();
                //窗口高度
                var clientHeight = $(this).height();
                if (scrollTop + clientHeight > pageHeight - 300) {
                    pictureLoad();
                    loadCount++;
                }
            })
        }

        function pictureLoad() {
            var content = $(".ibox-content");
            $.getJSON("handler/GetAllExcellent.ashx", { loadCount: loadCount }, function (data) {
                for (var i in data) {
                    var newElement = $('<a class="fancybox"  data-fancybox-group="gallery" href="' + data[i].largeSrc + '?' + data[i].isExcellent + '&'+data[i].pId+'"><img src="' + data[i].smallSrc + '" /></a>');
                    content.append(newElement);
                }

            });
            picBox();
        }

        function picBox() {
            $('.fancybox').fancybox({
                padding: 0,
                margin: 5,
                nextEffect: 'fade',
                prevEffect: 'none',
                autoWidth: true,
                autoHeight: true,
                autoResize: false,
                scrolling: "visible",
                afterLoad: function () {
                    var href = this.href;
                    var params = href.substring(href.lastIndexOf('?') + 1, href.length).split('&');
                    var isExcellent = params[0];
                    var pId = params[1];
                    var color = "blue";

                    if (isExcellent == "true") {
                        color = "red";
                    }
                    $.extend(this, {
                        type: 'html',
                        content: '<div class="fancybox-image" id="picModel" data-pId=' + pId + ' style="background-image:url(' + this.href + '); background-size: cover; background-position:50% 50%;background-repeat:no-repeat;height:100%;width:100%;" /></div><button class="pull-right" style="color:' + color + '" type="button" class="btn btn-default">赞</button>'
                    });
                    setTimeout(btnZanClickEvent, 500);
                }
            });
        }

        function btnZanClickEvent() {
            $("button.pull-right").click(function () {
                var pId = $("#picModel").attr('data-pId');
                var isExcellent = $(this).css('color') == 'rgb(255, 0, 0)';
                var turnRed = function () {
                    $("button.pull-right").css('color', 'red');
                }
                var turnBlue = function () {
                    $("button.pull-right").css('color', 'blue');
                }

                $.get("handler/ToggleExcellent.ashx", { pId: pId, isExcellent: isExcellent }, function (data) {
                    console.log(data)
                    if (isExcellent) {
                        if (data) {
                            turnBlue();
                        } else {
                            alert("取消赞失败");
                        }
                    } else {
                        if (data) {
                            turnRed();
                        } else {
                            alert("赞失败");
                        }
                    }
                });
            });
        }
    </script>


</body>
</html>