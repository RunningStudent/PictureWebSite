using Picture.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;


namespace PictureWebSite.handler
{
    /// <summary>
    /// ValidateCodeHandler 的摘要说明
    /// </summary>
    public class ValidateCodeHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            ValidateCode vc = new ValidateCode();
            string vcode = vc.CreateValidateCode(4);


            //保存验证码
            context.Session["user_vcode"] = vcode;


            //vc.CreateValidateGraphic(vcode, context);
            vc.CreateValidateGraphic(vcode, context);
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