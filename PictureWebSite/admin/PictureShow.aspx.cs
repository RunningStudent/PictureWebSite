using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PictureWebSite.admin.code;

namespace PictureWebSite.admin
{
    public partial class PictureShow : BasePage
    {
        public IEnumerable<Picture.Model.PictureInfoModel> list { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Picture.BLL.PictureInfoBLL bll = new Picture.BLL.PictureInfoBLL();
            list=bll.QueryList(-1, 0, null, "UploadDate", true);
        }
    }
}