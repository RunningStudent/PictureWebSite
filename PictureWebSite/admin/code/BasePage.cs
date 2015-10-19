using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PictureWebSite.admin.code
{
    public class BasePage : System.Web.UI.Page
    {
        /// <summary>
        /// 特定写法!!!页面初始化时候干的事,这个代码会在编译时与管道事件绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Page_Init(object sender, EventArgs e)
        {
            Object obj=Session["isLogin"];
            bool? isLogin=obj as bool?;
            #region 判断是否登入
            if (isLogin != true)
            {
                Response.ContentType = "text/html";
                Response.Redirect("Login.aspx");
                Response.Write(@"<script type=""text/javascript"">alert(""请登入"")</script>");
            }
            #endregion
        }
    }
}