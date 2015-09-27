using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Picture.Utility;
using DS.Web.UCenter.Client;
using DS.Web.UCenter;

namespace PictureWebSite.handler
{
    /// <summary>
    /// 载入数据到弹出层
    /// </summary>
    public class LoadPictureDetail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pId = int.Parse(context.Request["pId"]);
            Picture.BLL.CommentMore commentBll = new Picture.BLL.CommentMore();
            Picture.BLL.PictureMoreInfoBLL pictureInfoBll = new Picture.BLL.PictureMoreInfoBLL();


            IUcClient client = new UcClient();

            //构建评论信息
            var commentlist = commentBll.GetCommentWithUserInfo(new { PId = pId }, "PostDate").Select(c => new
            {
                userFace = client.AvatarUrl(c.UId, AvatarSize.Small),
                content = c.Content,
                postDate = c.PostDate,
                userName = c.UserName
            });

            //获得图片信息
            var pictureInfo = pictureInfoBll.GetSinglePictureInfoWithTagAndUserInfo(pId);


            //把数据整合并返回
            var result = new
            {
                url = pictureInfo.LargeImgPath,
                uploadDate = pictureInfo.UploadDate,
                tags = pictureInfo.Tags,
                summary=pictureInfo.ImgSummary,
                userInfo = new
                {
                    userName = pictureInfo.UInfo.UserName,
                    userStatus = pictureInfo.UInfo.UserStatus,
                    userFace = client.AvatarUrl(pictureInfo.UInfo.Uid, AvatarSize.Small)
                },
                commentlist = commentlist
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