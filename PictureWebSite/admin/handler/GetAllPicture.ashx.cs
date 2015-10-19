using Picture.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace PictureWebSite.admin.handler
{
    /// <summary>
    /// GetAllPicture 的摘要说明
    /// </summary>
    public class GetAllPicture : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Session["isLogin"] == null)
            {
                context.Response.Write("error");
                return;
            }
            int loadCount = int.Parse(context.Request["loadCount"]);
            
            Picture.BLL.PictureMoreInfoBLL pictureBll = new Picture.BLL.PictureMoreInfoBLL();
            Picture.BLL.ExcellentPictureBLL excellentBll=new Picture.BLL.ExcellentPictureBLL();


            var pictureList = pictureBll.GetPictureInfoWithTagAndUserInfo(loadCount + 1, 20, "UploadDate");

            var resultList = pictureList.Select(c => new
            {
                largeSrc = "../" + c.LargeImgPath,
                smallSrc = "../" + CommonHelper.GetSmallImgPath(c.LargeImgPath),
                isExcellent = excellentBll.QueryCount(new {ePId=c.PId })>0,
                pId = c.PId
            });

            context.Response.Write(JSONHelper.ToJSONString(resultList));

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