<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PanGUTest.aspx.cs" Inherits="PictureWebSite.PanGUTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            请输入搜索关键字:
        <input type="text" name="SearchKey" value="" />
            <input type="submit" name="btnSearch" value="一哈哈" />
            <input type="submit" name="btnCreate" value="创建索引" />
            <br />
            <ul>
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <li><a href='#'>
                            <%# Eval("ImgSummary") %></a></li>
                    </ItemTemplate>
                </asp:Repeater>


            </ul>
        </div>
    </form>
</body>
</html>
