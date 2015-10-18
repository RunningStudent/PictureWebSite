<%@ Page Title="" Language="C#" MasterPageFile="~/admin/shared/admin.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PictureWebSite.admin.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="middle-box text-center loginscreen  animated fadeInDown">
        <div>
            <div>
                <h1 class="logo-name">FLy</h1>
            </div>
            <h3>欢迎使用图片网后台</h3>
            <div class="form-group">
                <input type="text" class="form-control" name="username" placeholder="用户名" required="required" value="">
            </div>
            <div class="form-group">
                <input type="password" class="form-control" name="password" placeholder="密码">
                <span id="cemail-error" class="help-block m-b-none">
                    <%=errorMessage %>
                </span>
            </div>
            <button type="submit" class="btn btn-primary block full-width m-b">登 录</button>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>

