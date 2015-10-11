using DS.Web.UCenter.Client;
using PictureWebSite.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DS.Web.UCenter;

namespace PictureWebSite
{
    public partial class UserIndex2 : BasePage
    {
        public Model.User CurrentUser { get; set; }
        public IEnumerable<Picture.Model.PictureInfoModel> List { get; set; }
        public string Avatar { get; set; }
        public string AvatarHref { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUser = Session["current_user"] as Model.User;
            ////获得图片数据
            //Picture.BLL.PictureInfoBLL bllPictureInfo = new Picture.BLL.PictureInfoBLL();
            //List = bllPictureInfo.QueryList(-1, 10, new { UId = CurrentUser.UId }, "UploadDate");

            //获得修改头像
            IUcClient client = new UcClient();
            Avatar = client.Avatar(CurrentUser.UId);
            Avatar = Avatar.Replace(@"id=""mycamera""", @"id=""mycamera"" class=""center-block""");

            AvatarHref = client.AvatarUrl(CurrentUser.UId, AvatarSize.Big, AvatarType.Virtual);


        }
    }
}