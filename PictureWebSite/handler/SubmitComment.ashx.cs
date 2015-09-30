using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;
using PictureWebSite.Model;
using Picture.Utility;

namespace PictureWebSite.handler
{
    /// <summary>
    /// SubmitComment 的摘要说明
    /// </summary>
    public class SubmitComment : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            /*
             * isSubmit
             * 
             * errorCode
             * 1    未登入
             * 2    未知错误 如果被评价的图片不在数据库中
             * 3    评论长度过长
             * 
             * 
             * 存在问题
             * 数据库中设置的是nvarchar250 即保存字符250个,相当于250/2个汉字
             * 那么用户上传的时候要计算所占字符长度(不同编码格式占用的长度不同)
             * Encoding.UTF8.GetChartCount()
             * 
             * 这里我直接设置为125个无论是字母还是汉字,一切以长度为准
             * 
             */


            context.Response.ContentType = "text/plain";
            if (context.Session["current_user"] == null)
            {
                SubmitCommentError(1, context);
                return;
            }

            User user = context.Session["current_user"] as User;
            int pId = int.Parse(context.Request["pId"]);
            string comment = context.Request["comment"];
            if (comment.Length > 125)
            {
                SubmitCommentError(3, context);
                return;
            }


            Picture.BLL.CommentBLL commentBll = new Picture.BLL.CommentBLL();


            int cId=commentBll.Insert(new Picture.Model.CommentModel()
            {
                UId = user.UId,
                Content = context.Server.HtmlEncode(comment),//编个码
                PId = pId,
                PostDate = DateTime.Now
            });
            
            if (cId>0)
            {
                var result = new
                {
                    isSubmit = true,
                    userName=user.UserName,
                    userFace=user.UserFacePathSmall,
                    cId = cId
                };
                context.Response.Write(JSONHelper.ToJSONString(result));
            }
            else
            {
                SubmitCommentError(2, context);
            }


        }

        public void SubmitCommentError(int errorCode, HttpContext context)
        {
            context.Response.Write(JSONHelper.ToJSONString(new
            {
                isSubmit = false,
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