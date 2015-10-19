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
    /// GetAllExcellent 的摘要说明
    /// </summary>
    public class GetAllExcellent : IHttpHandler, IRequiresSessionState
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
            Picture.BLL.ExcellentPictureBLL excellentBll = new Picture.BLL.ExcellentPictureBLL();
            Picture.BLL.PictureMoreInfoBLL pictureBll = new Picture.BLL.PictureMoreInfoBLL();

            var excellentPicList = excellentBll.QueryList(loadCount + 1, 20, null, "eAddDate", true);
            string excellentPicWhere = BuildWhere(excellentPicList);
            var pictureList = pictureBll.GetPictureInfoWithTagAndUserInfo(loadCount + 1, 20,"UploadDate", excellentPicWhere);

            var resultList = pictureList.Select(c => new
            {
                largeSrc = "../" + c.LargeImgPath,
                smallSrc = "../" + CommonHelper.GetSmallImgPath(c.LargeImgPath),
                isExcellent = true,
                pId=c.PId
            });

            context.Response.Write(JSONHelper.ToJSONString(resultList));
        }

        /// <summary>
        /// 根据优秀图片数据拼接图片where条件
        /// </summary>
        /// <param name="excellentPicList"></param>
        /// <returns></returns>
        private string BuildWhere(IEnumerable<Picture.Model.ExcellentPictureModel> excellentPicList)
        {
            StringBuilder resultStrBuilder = new StringBuilder(" where ");
            foreach (var item in excellentPicList)
            {
                resultStrBuilder.Append(" PId=" + item.ePId + " or ");
            }
            string result = resultStrBuilder.ToString();
            result = result.Substring(0, result.LastIndexOf("or"));
            return result;
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