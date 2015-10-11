<%@ Page Title="" Language="C#" MasterPageFile="~/shared/Layout.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="PictureWebSite.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="assets/css/index.css" rel="stylesheet" />
    <link href="assets/css/reset.css" rel="stylesheet" />


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <%--  --%>
	<div id="waterfallFlow" class="warpper"></div>
	<div id="loader"></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
    <script src="assets/js/lib/jquery.lazyload.js"></script>
    <script src="assets/js/function.js"></script>
    
    <script src="assets/js/newMask3.js"></script>
    <script src="assets/js/lib/dateFormat.js"></script>
    <script src="assets/js/waterfallFlow.js"></script>
    <script src="assets/js/1.js"></script>

</asp:Content>
