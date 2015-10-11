using DS.Web.UCenter.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace PictureWebSite.account
{
    /// <summary>
    /// Logout 的摘要说明
    /// </summary>
    public class Logout : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Session["current_user"] = null;
            context.Response.Redirect("../index.aspx");
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