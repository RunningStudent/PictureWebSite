using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Picture.Utility;
using DS.Web.UCenter.Client;
using DS.Web.UCenter;
using System.Web.SessionState;
using PictureWebSite.Model;
using Picture.Model;

namespace PictureWebSite.handler
{
    /// <summary>
    /// 载入数据到弹出层
    /// </summary>
    public class LoadPictureDetail : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pId = int.Parse(context.Request["pId"]);
            Picture.BLL.CommentMore commentBll = new Picture.BLL.CommentMore();
            Picture.BLL.PictureMoreInfoBLL pictureInfoBll = new Picture.BLL.PictureMoreInfoBLL();


            IUcClient client = new UcClient();

            User user = context.Session["current_user"] as User;

            object commentlist = null;
            //构建评论信息
            if (user != null)
            {
                commentlist = commentBll.GetCommentWithUserInfo(new { PId = pId }, "PostDate").Select(c => new
                {
                    userFace = client.AvatarUrl(c.UId, AvatarSize.Small),
                    content = c.Content,
                    postDate = c.PostDate,
                    userName = c.UserName,
                    cId = c.CId,
                    isMe = c.UId == user.UId ? true : false
                });
            }
            else
            {
                commentlist = commentBll.GetCommentWithUserInfo(new { PId = pId }, "PostDate").Select(c => new
               {
                   userFace = client.AvatarUrl(c.UId, AvatarSize.Small),
                   content = c.Content,
                   postDate = c.PostDate,
                   cId = c.CId,
                   userName = c.UserName,
               });
            }


            //获得图片信息
            var pictureInfo = pictureInfoBll.GetSinglePictureInfoWithTagAndUserInfo(pId, user==null?-1:user.UId);


            //把数据整合并返回
            var result = new
            {
                url = pictureInfo.LargeImgPath,
                imgHeight=pictureInfo.Height,
                uploadDate = pictureInfo.UploadDate,
                tags = pictureInfo.Tags,
                summary = pictureInfo.ImgSummary,
                collectCount=pictureInfo.CollectCount,
                userInfo = new
                {
                    //userName = pictureInfo.UInfo.UserName,
                    //userStatus = pictureInfo.UInfo.UserStatus,
                    //userFace = client.AvatarUrl(pictureInfo.UInfo.Uid, AvatarSize.Small)
                    userName = pictureInfo.UserName,
                    userStatus = pictureInfo.UserStatus,
                    userFace = client.AvatarUrl(pictureInfo.UId, AvatarSize.Small)
                },
                commentlist = commentlist,
                isCollect = pictureInfo.isCollect
            };
            context.Response.Write(JSONHelper.ToJSONString(result));


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