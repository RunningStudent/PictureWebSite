//===========图片详细信息弹出层

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

    //删除图片
    links.eq(2).click(function () {
        Modal.confirm(
        {
            msg: "是否删除图片？"
        })
        .on(function (e) {
            if (e == true) {
                pictureDelete();
            }
        })


    });

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

//图片删除
function pictureDelete() {
    var modal = $("#pictureDetailModal")
    var pId = modal.attr("data-pid");

    $.getJSON("handler/DeletePicture.ashx", { pId: pId }, function (data) {
        if (data.IsDelete == false) {
            switch (data.errorCode) {
                case 1:
                    alert("请先登入");
                    //location.href = "Index.aspx";
                    break;
                case 2:
                    alert("您没有这张图片");
                    break;
                case 3:
                    alert("未知错误");
                    break;
                default:
            }
        } else {
            modal.modal('hide');
            var tarPic = $("#userPictureModel div[data-pid='" + pId + "']");
            var $container = $('.masonry-container');
            $container.masonry('remove', tarPic);
            masonryOn();
        }
    });
}

//当弹出层关闭事件绑定
function modalClose() {
    $("#pictureDetailModal").on('hidden.bs.modal', function (e) {
        commentTxtReset();
        $("#pictureDetailModal .modal-body img").attr("src", "assets/img/detailPictureLoading.jpg");
    })
}

//==================================弹出层结束

//========图片上传弹出层
var lastFile;
var uploader = new plupload.Uploader({ //实例化一个plupload上传对象
    browse_button: 'picPreview',//点击弹出浏览文件对话框的标签
    url: 'handler/PictureUpload.ashx',//图片上传的目标位置
    drop_element: 'picPreview',//图片可拖拽到这标签
    multi_selection: false,//是否允许多选图片
    filters: {
        mime_types: [ //只允许上传图片
          { title: "Image files", extensions: "jpg,gif,png,bmp" },
        ],
        max_file_size: '4000kb', //最大只能上传2000kb的文件
        prevent_duplicates: true //不允许选取重复文件
    },
});
uploader.init(); //初始化
//绑定文件添加进队列事件
uploader.bind('FilesAdded', function (uploader, files) {
    //清空画选框
    var selectContain = $("#imgareaselectContain");
    selectContain.html("");
    if (lastFile != null) {
        uploader.removeFile(lastFile);
    }

    if (files.length > 1) {
        alert("错误");
        return false;
    }
    lastFile = files[0];
    //var file_name = files[0].name; //文件名
    //$("#fileName").html(file_name);
    //图片预览
    previewImage(files[0].getNative());
    //截图框载入

    var img = $("#imghead");
    img.imgAreaSelect({
        selectionColor: 'white',//选择区域的颜色
        x1: 0,//初始区的左上角的x坐标
        y1: 0,//初始区的左上角的y坐标
        x2: 200,//初始区的右下角的x坐标
        y2: 200,//初始区的右下角的y坐标
        selectionOpacity: 0.2,//选择区的透明度
        persistent: true,
        aspectRatio: "1:1",
        handles: true,
        parent: $("#imgareaselectContain")
    });

    //设置画选框容器的大小
    img.load(function () {
        selectContain.css('height', img.css('height'));
        selectContain.css('width', img.css('width'));
        selectContain.find('div').css('position', 'absolute');
    });

});

//图片上传
uploader.bind('FileUploaded', function (uploader, file, responseObject) {
    var data = $.parseJSON(responseObject.response);
    if (file.status == 5) {
        if (data.isUpload == false) {
            switch (data.errorCode) {
                case 1:
                    alert("请先登入")

                    break;
                case 2:
                    alert("图片格式错误");
                    break;
                case 3:
                    alert("图片大小错误");
                    break;
                default:
            }
        } else {
            alert("上传成功");
            lastFile = null;
            $("#picInfo [contenteditable]").css('display', 'block');
            $("#uploadProgress").css('display', 'none');
            picInfoContentReset();
            //刷新页面
            window.location.reload();
        }
    } else if (file.status == 4) {
        //前端的上传失败
        //alert("上传失败");

    }
})

//绑定文件上传进度事件
uploader.bind('UploadProgress', function (uploader, file) {
    //隐藏图片介绍和标签,显示进度条
    var introduceAndTags = $("#picInfo [contenteditable]").css('display', 'none');
    var progress = $("#uploadProgress");
    progress.css('display', 'block');
    var div = progress.find("div");
    div.css('width', file.percent + '%');
    div.text(file.percent + '%');
});

//错误事件
uploader.bind('Error', function (uploader, errObject) {
    console.log(errObject.message + "_______" + errObject.code);
    switch (errObject.code) {
        case -200:
            alert("网络连接错误");
            console.log(errObject.message);
            break;
        case -300:
            alert("选取文件读取失败");
            console.log(errObject.message);
            break;
        case -702:
        case -600:
            alert("选取文件过大");
            console.log(errObject.message);
            break;
        case -601:
            alert("选取文件类型错误");
            console.log(errObject.message);
            break;
        case -602:
            alert("无法选取重复文件");
            console.log(errObject.message);
            break;
        case -700:
            alert("选取文件格式错误");
            console.log(errObject.message);
            break;
        default:
            alert("未知错误");
            console.log(errObject.message);
            break;
    }

});


//上传即将开始
uploader.bind('BeforeUpload', function (uploader, file) {
})

//图片上传预览    IE是用了滤镜。
function previewImage(file) {
    var MAXWIDTH = 260;
    var MAXHEIGHT = 180;
    var div = document.getElementById('picPreview');
    if (file) {
        //清空图片
        div.innerHTML = '<img id=\"imghead\" class="center-block img-thumbnail" style="padding:0px">';
        var img = document.getElementById('imghead');
        var reader = new FileReader();
        reader.onload = function (evt) {
            img.src = evt.target.result;
        }
        reader.readAsDataURL(file);
    }
    else //兼容IE
    {
        var sFilter = 'filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale,src="';
        file.select();
        var src = document.selection.createRange().text;
        div.innerHTML = '<img id=\"imghead\">';
        var img = document.getElementById('imghead');
        img.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').src = src;
        var rect = clacImgZoomParam(MAXWIDTH, MAXHEIGHT, img.offsetWidth, img.offsetHeight);
        status = ('rect:' + rect.top + ',' + rect.left + ',' + rect.width + ',' + rect.height);
        div.innerHTML = "<div id=divhead style='width:" + rect.width + "px;height:" + rect.height + "px;margin-top:" + rect.top + "px;" + sFilter + src + "\"'></div>";
    }
}


//==========图片上传弹出层相关
function introduceFocus(e) {
    var regex = /<span.+?>写下图片的介绍<\/span>/;
    if (regex.test(e.innerHTML)) {
        e.innerHTML = "";
    }

}
function introduceBlur(e) {
    var regex = /^\s{1,}$/;
    if (regex.test(e.innerText) || e.innerText == "") {
        e.innerHTML = "<span style=\"color:gray\">写下图片的介绍</span>";
    }
}

function tagsFocus(e) {
    var regex = /<span.*>\s*输入标签用逗号隔开\s*<\/span>/;
    if (regex.test(e.innerHTML)) {
        e.innerHTML = "";
    }
}
function tagsBlur(e) {
    var regex = /^\s{1,}$/;
    if (regex.test(e.innerText) || e.innerText == "") {
        e.innerHTML = "<span>输入标签用逗号隔开</span>";
    }
}
function uploadClick() {
    if (uploader.files.length == 0) {
        alert("请选择图片");
        return;
    }
    var param=buildAreaSelectParam();
    uploader.setOption("multipart_params", {
        summary: $("#introduce").text(),
        tags: $("#tags").text(),
        X: param.oX,
        Y: param.oY,
        sideLength:param.sideLength
    });
    console.log("s");
    uploader.start();
}


//计算截取点和图片宽度
function buildAreaSelectParam() {
    //获得选取框的数据
    var selectInstance = $("#imghead").imgAreaSelect({ instance: true });
    var selectParams = selectInstance.getSelection();
    //获取原始图片对象
    var viewImg = $("#imghead")[0];
    var originalImg = new Image();
    originalImg.src = viewImg.src;
    var postParams = {};
    postParams.oX = (selectParams.x1 / viewImg.width) * originalImg.width;
    postParams.oY = (selectParams.y1 / viewImg.height) * originalImg.height;
    if (originalImg.height>originalImg.width) {
        postParams.sideLength=(selectParams.width/viewImg.width)*originalImg.width
    } else {
        postParams.sideLength = (selectParams.height / viewImg.height) * originalImg.height
    }
    return postParams;

}


(function uploadModelClose() {
    $("#picUploadModal").on('hidden.bs.modal', function (e) {
        //图片还原
        $("#picPreview").find("img").attr("src", "assets/img/preview.jpg");
        //标签与介绍还原
        picInfoContentReset();
        //清空画选框
        var selectContain = $("#imgareaselectContain");
        selectContain.html("");
    })
})();

function picInfoContentReset() {
    var infos = $("#picInfo [contenteditable]");
    infos[0].innerHTML = "<span style=\"color:gray\">写下图片的介绍</span>";
    infos[1].innerHTML = "<span>输入标签用逗号隔开</span>";
}



//弹出层结束