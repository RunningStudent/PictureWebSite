using Picture.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace PictureWebSite
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //分词组件初始化
            string panGuSettingPath = Server.MapPath(@"Dict/PanGu.xml");
            PanGu.Segment.Init(panGuSettingPath);
            //对新增标签的处理
            Picture.Utility.IndexManager.tagIndex.StartNewThread();

            //开启线程日志处理
            LogHelper.logHelper.StartNewThread();


        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            LogHelper.logHelper.AddException(Context.Server.GetLastError());
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}