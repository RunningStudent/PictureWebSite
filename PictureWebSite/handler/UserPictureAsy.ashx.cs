using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using PictureWebSite.Model;
using Picture.Utility;


namespace PictureWebSite.handler
{
    /// <summary>
    /// UserUploadPictureAsy 用户中心的图片加载
    /// </summary>
    public class UserPictureAsy : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int loadCount = int.Parse(context.Request["loadCount"]);
            int loadSize = int.Parse(context.Request["loadSize"]);
            int modelType = int.Parse(context.Request["modelType"]);

            //防止非法访问
            if (context.Session["current_user"] == null)
            {
                context.Response.Redirect("../Index.aspx");
            }
            User user = context.Session["current_user"] as User;
            switch (modelType)
            {
                case 0:
                    LoadUserPicture(context, loadCount, loadSize, user);
                    break;
                case 1:
                    LoadCollectionPicture(context, loadCount, loadSize, user);
                    break;
                default:
                    break;
            }


        }


        /// <summary>
        /// 加载用户收藏的图片
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loadCount"></param>
        /// <param name="loadSize"></param>
        /// <param name="user"></param>
        private void LoadCollectionPicture(HttpContext context, int loadCount, int loadSize, User user)
        {
            Picture.BLL.PictureCollectBLL bllCollect = new Picture.BLL.PictureCollectBLL();
            var list = bllCollect.GetCollectWithPictureInfo(loadCount + 1, loadSize, new { CuId = user.UId }, "CollectDate").Select(p => new { imgUrl = CommonHelper.GetSmallImgPath(p.LargeImgPath), uploadDate = p.UploadDate, collectCount = p.CollectCount, pId = p.PId, width = p.Width, height = p.Height });

            //返回数据
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.Write(jss.Serialize(list));
        }



        /// <summary>
        /// 加载用户图片数据返回
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loadCount"></param>
        /// <param name="loadSize"></param>
        /// <param name="user"></param>
        private void LoadUserPicture(HttpContext context, int loadCount, int loadSize, User user)
        {
            //取数据
            Picture.BLL.PictureInfoBLL bllPicture = new Picture.BLL.PictureInfoBLL();
            var list = bllPicture.QueryList(loadCount + 1, loadSize, new { UId = user.UId }, "UploadDate").Select(p => new { imgUrl = CommonHelper.GetSmallImgPath(p.LargeImgPath), uploadDate = p.UploadDate, collectCount = p.CollectCount, pId = p.PId, width = p.Width, height = p.Height });

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