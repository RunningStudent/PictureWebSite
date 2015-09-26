using PictureWebSite.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PictureWebSite
{
    public partial class UserIndex : BasePage
    {
        public Model.User CurrentUser{ get; set; }
        public IEnumerable<Picture.Model.PictureInfoModel> List { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUser = Session["current_user"] as Model.User;
            Picture.BLL.PictureInfoBLL bllPictureInfo = new Picture.BLL.PictureInfoBLL();
            List = bllPictureInfo.QueryList(-1, 10, new { UId = CurrentUser.UId }, "UploadDate");

        }
    }
}