using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Picture.Utility;

namespace PictureWebSite.handlerHelper
{
    public static class AccountHelper
    {
        /// <summary>
        /// 登入失败,返回错误信息
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="context"></param>
        public static void LoginErrorReturnData(string errorMessage,int place , HttpContext context)
        {
            var data = new
            {
                isLogined = false,
                errorMessage = errorMessage,
                place=place
            };
            context.Response.Write(JSONHelper.ToJSONString(data));
            context.Response.End();
        }

        /// <summary>
        /// 注册失败,返回错误信息
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="context"></param>
        public static void RegisterErrorReturnData(string errorMessage,int place, HttpContext context)
        {
            var data = new
            {
                isRegister = false,
                errorMessage = errorMessage,
                place=place
            };

            context.Response.Write(JSONHelper.ToJSONString(data));
            context.Response.End();
        }


        /// <summary>
        /// 修改邮箱失败返回错误信息
        /// </summary>
        /// <param name="stateCode">显示在前端的警告栏样式，1为success，2为info，3为danger</param>
        /// <param name="context"></param>
        public static void UserSettingChangeReturnData(int stateCode,string returnMessage, HttpContext context)
        {
            var data=new 
            {
                stateCode=stateCode,
                returnMessage = returnMessage
            };

            context.Response.Write(JSONHelper.ToJSONString(data));
            context.Response.End();

        }


    }
}