using Picture.Utility;
using PictureWebSite.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace PictureWebSite.handler
{
    /// <summary>
    /// DeletePicture 的摘要说明
    /// </summary>
    public class DeletePicture : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            /*
             *  与前端的规定
             *  1     未登入
             *  2     该用户没有这张图片
             *  3     数据库问题,图片未删除
             * 
             */

            context.Response.ContentType = "text/plain";
            int pId = int.Parse(context.Request["pId"]);
            User user = context.Session["current_user"] as User;
            //校验是否登入
            if (user == null)
            {
                DeletePictureError(context, 1);
                return;
            }

            Picture.BLL.PictureInfoBLL picturebll = new Picture.BLL.PictureInfoBLL();
            //校验该用户是否有这张图片
            Picture.Model.PictureInfoModel picture=picturebll.QuerySingle(new { PId = pId, __o = "and", UId = user.UId });
            if ( picture==null)
            {
                DeletePictureError(context, 2);
                return;
            }

            //删除图片表图片
            if (picturebll.Delete(pId) <= 0)
            {
                DeletePictureError(context, 3);
                return;
            }
            else
            {
                //大图路径
                string picPath = context.Request.MapPath("../"+picture.LargeImgPath);
                //小图路径
                string smallPath =context.Request.MapPath("../"+CommonHelper.GetSmallImgPath(picture.LargeImgPath));
                if (File.Exists(picPath))
                {
                    File.Delete(picPath);
                }
                if (File.Exists(smallPath))
                {
                    File.Delete(smallPath);
                }

            }
            context.Response.Write(JSONHelper.ToJSONString(new { isDelete = true }));
        }


        public void DeletePictureError(HttpContext context, int errorCode)
        {
            context.Response.Write(JSONHelper.ToJSONString(new{
                isDelete = false,
                errorCode = errorCode
            }));
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