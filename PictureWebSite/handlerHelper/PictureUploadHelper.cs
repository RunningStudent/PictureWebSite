using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Picture.Utility;
namespace PictureWebSite.handlerHelper
{
    public class PictureUploadHelper
    {

        public static void PictureUploadError(HttpContext context, int errorCode)
        {
            var returnData = new
            {
                isUpload = false,
                errorCode = errorCode
            };
            context.Response.Write(JSONHelper.ToJSONString(returnData));
            context.Response.End();
        }

    }
}