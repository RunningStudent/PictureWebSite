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
    <script src="assets/js/pictureDetail.js"></script>
    <script src="assets/js/lib/BootStrap/js/bootstrap.min.js"></script>
    <script src="assets/js/lib/jquery.validate.min.js"></script>
    <script src="assets/js/lib/messages_cn.js"></script>
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


    </script>
</asp:Content>
