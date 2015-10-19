using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Picture.BLL;
using Picture.Utility;

namespace PictureWebSite.admin.handler
{
    /// <summary>
    /// ToggleExcellent 的摘要说明
    /// </summary>
    public class ToggleExcellent : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Session["isLogin"] == null)
            {
                context.Response.Write("error");
                return;
            }
            int pid = int.Parse(context.Request["pId"]);
            bool isExcellent = bool.Parse(context.Request["isExcellent"]);
            ExcellentPictureBLL excellentBll = new ExcellentPictureBLL();


            int effecRowCount = -1;
            if (isExcellent)
            {
                int eId = excellentBll.QuerySingle(new { ePid = pid }).eId;
                effecRowCount = excellentBll.Delete(eId);
            }
            else
            {
                //其实这里返回的新增数据的主键id
                effecRowCount = excellentBll.Insert(new Picture.Model.ExcellentPictureModel()
                 {
                     ePId = pid,
                     eAddDate = DateTime.Now
                 });
            }
            if (effecRowCount > 0)
            {
                context.Response.Write(true);
            }
            else
            {
                context.Response.Write(false);
            }

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