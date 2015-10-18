using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace PictureWebSite.admin.handler
{
    /// <summary>
    /// LogOut 的摘要说明
    /// </summary>
    public class LogOut : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Session.Clear();
            context.Response.Redirect("/");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}