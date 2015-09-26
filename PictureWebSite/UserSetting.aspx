<%@ Page Title="" Language="C#" MasterPageFile="~/shared/Layout.Master" AutoEventWireup="true" CodeBehind="UserSetting.aspx.cs" Inherits="PictureWebSite.UserSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="assets/BootStrap/css/bootstrap.min.css" rel="stylesheet" />

    <style type="text/css">
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



        .main {
            margin-top: 100px;
            box-shadow: 0px 1px 3px rgba(34, 25, 25, 0.4);
        }

        .panel {
        }

        .panel-heading {
            cursor: pointer;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container main">

        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
            <div class="page-header text-center">
                <h2>账号设置</h2>
            </div>

            <div class="panel">
                <div class="panel-heading" role="tab" id="headingOne" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                    <h4 class="panel-title">
                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">邮箱信息
                        </a>
                    </h4>
                </div>

                <div id="collapseOne" class="panel-collapse collapse " role="tabpanel" aria-labelledby="headingOne">
                    <div class="panel-body">
                        <%--第一个表单,修改邮箱--%>
                        <div class="col-sm-6 col-sm-offset-1">
                            <h4>邮箱修改</h4>
                            <br />
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">当前邮箱</label>
                                    <div class="col-sm-9">
                                        <p class="form-control-static">email@example.com</p>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="newEmail" class="col-sm-3 control-label">新邮箱</label>
                                    <div class="col-sm-9">
                                        <input type="password" class="form-control" id="newEmail" name="newEmail">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="password" class="col-sm-3 control-label">登入密码</label>
                                    <div class="col-sm-9">
                                        <input type="password" class="form-control" id="password" name="password">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-offset-2 col-sm-10">
                                        <button type="submit" class="btn btn-default">保存</button>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>  

            <div class="panel">

                <div class="panel-heading" role="tab" id="headingTwo" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-controls="collapseTwo">
                    <h4 class="panel-title">
                        <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">用户名
                        </a>
                    </h4>
                </div>

                <div id="collapseTwo" class="panel-collapse collapse " role="tabpanel" aria-labelledby="headingTwo">
                    <div class="panel-body">
                        <%--第一二个表单,修改用户名--%>
                        <div class="col-sm-6 col-sm-offset-1">
                            <h4>用户名修改</h4>
                            <br />
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label">当前用户名</label>
                                    <div class="col-sm-9">
                                        <p class="form-control-static">啦啦啦啦</p>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="newName" class="col-sm-3 control-label">新用户名</label>
                                    <div class="col-sm-9">
                                        <input type="password" class="form-control" id="newName" name="newName">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-offset-2 col-sm-10">
                                        <button type="submit" class="btn btn-default">保存</button>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel">
                <div class="panel-heading" role="tab" id="headingThree" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                    <h4 class="panel-title ">
                        <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">密码
                        </a>
                    </h4>
                </div>

                <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                    <div class="panel-body">

                        <%--第一二个表单,修改用户名--%>
                        <div class="col-sm-6 col-sm-offset-1">
                            <h4>密码修改</h4>
                            <br />
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <label for="currentPassword" class="col-sm-3 control-label">当前密码</label>
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
                                    <label for="newPassword2" class="col-sm-3 control-label">确认新密码</label>
                                    <div class="col-sm-9">
                                        <input type="password" class="form-control" id="newPassword2" name="newPassword2">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-offset-2 col-sm-10">
                                        <button type="submit" class="btn btn-default">保存</button>
                                    </div>
                                </div>
                            </form>
                        </div>


                    </div>
                </div>
            </div>

            <div class="panel">
                <div class="panel-heading" role="tab" id="headingFour" data-toggle="collapse" data-parent="#accordion" href="#collapseFour" aria-expanded="false" aria-controls="collapseFour">
                    <h4 class="panel-title ">
                        <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFour" aria-expanded="false" aria-controls="collapseFour">头像
                        </a>
                    </h4>
                </div>

                <div id="collapseFour" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                    <div class="panel-body">
                        头像修改
                    </div>
                </div>
            </div>


        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
    <script src="assets/BootStrap/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        //修改导航栏宽度
        var main = $(".main")[0];
        var head = $(".topBar")[0];
        headWidth(main, head);
        $(window).resize(function () {
            headWidth(main, head);
        });



    </script>
</asp:Content>
