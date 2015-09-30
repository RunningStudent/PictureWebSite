using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using PictureWebSite.Model;

namespace PictureWebSite.handler
{
    /// <summary>
    /// UserUploadPictureAsy 的摘要说明
    /// </summary>
    public class UserUploadPictureAsy : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int loadCount = int.Parse(context.Request["loadCount"]);
            int loadSize = int.Parse(context.Request["loadSize"]);
            //防止非法访问
            if (context.Session["current_user"]==null)
            {
                context.Response.Redirect("/Index.aspx");
            }
            User user=context.Session["current_user"] as User;

            //取数据
            Picture.BLL.PictureInfoBLL bllPicture = new Picture.BLL.PictureInfoBLL();
            var list=bllPicture.QueryList(loadCount + 1, loadSize, new { UId = user.UId }, "UploadDate").Select(p => new { imgUrl = p.LargeImgPath ,uploadDate=p.UploadDate,collectCount=p.CollectCount,pId=p.PId,width=p.Width,height=p.Height});
            
            //返回数据
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.Write(jss.Serialize(list));

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