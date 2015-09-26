using DS.Web.UCenter.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DS.Web.UCenter;

namespace PictureWebSite.handler
{
    /// <summary>
    /// GetPictureListAsy 的摘要说明
    /// </summary>
    public class GetPictureListAsy : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            int ciShu = int.Parse(context.Request["ciShu"] ?? "0");
            //取数据
            Picture.BLL.PictureMoreInfoBLL pictureInfoBll = new Picture.BLL.PictureMoreInfoBLL();
            //var models = bllPictureInfo.QueryList(ciShu + 1, 20,new {}, "UploadDate");
            var models = pictureInfoBll.GetPictureInfoWithTag(ciShu + 1, 20, null, "UploadDate");

            IUcClient client = new UcClient();

            //构建要发送到前端的数据
            List<Model.Picture> list = new List<Model.Picture>();
            foreach (var item in models)
            {
                list.Add(new Model.Picture()
                {
                    imgUrl = item.LargeImgPath,
                    userName = item.UInfo.UserName,
                    userFace = client.AvatarUrl(item.UInfo.Uid, AvatarSize.Small),//@"assets/img/face/face1.jpg",
                    label = item.Tags.Select(t => t.TagName).ToList(),
                    width = item.Width,
                    height = item.Height,
                    uploadDate = item.UploadDate,
                    title = item.ImgSummary
                });
            }

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