using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using PictureWebSite.Model;
using Picture.BLL;
using Picture.Utility;

namespace PictureWebSite.handler
{
    /// <summary>
    /// DeleteComment 的摘要说明
    /// </summary>
    public class DeleteComment : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            /*
             * 
             * ErrorCode
             * 1   用户未登入
             * 2   该用户并无此评论
             * 3   未知错误
             * 
             * 
             */
            context.Response.ContentType = "text/plain";
            int cId = int.Parse(context.Request["cId"]);

            CommentBLL commentBll = new CommentBLL();
            
            //用户是否登入校验
            User user=context.Session["current_user"] as User;
            if (user==null)
            {
                DeleteCommentError(1, context);
                return;
            }

            //判断该评论是否属于该用户
            if (commentBll.QueryCount(new { UId=user.UId,__o="and",cId=cId})<1)
            {
                DeleteCommentError(2, context);
                return;
            }
            if (commentBll.Delete(cId) < 1)
            {
                DeleteCommentError(3, context);
                return;
            }
            else
            {
                context.Response.Write(JSONHelper.ToJSONString(new
                {
                    IsDelete = true
                }));
            }

        }

        public void DeleteCommentError(int errorCode, HttpContext context)
        {
            context.Response.Write(JSONHelper.ToJSONString(new
            {
                IsDelete = false,
                errorCode = errorCode
            }));
            context.Response.End();
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