using DS.Web.UCenter;
using DS.Web.UCenter.Client;
using PictureWebSite.handlerHelper;
using PictureWebSite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using Picture.Utility;

namespace PictureWebSite.account
{
    /// <summary>
    /// Register 的摘要说明
    /// </summary>
    public class Register : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            /*
            * 存在问题
            * 
            * 完全可以去掉，登入错误时候返回的提示信息
            * 改为返回错误码，与前端定个规则，1代表表单未填满，2代表密码错误
            * 
            */


            context.Response.ContentType = "text/html";
            Picture.BLL.UserInfoBLL bllUserInfo = new Picture.BLL.UserInfoBLL();
            string verify = context.Request["verify"];
            string username = context.Request["username"];
            string password = context.Request["password"];
            string email = context.Request["email"];

            #region 表单为空校验
            if (string.IsNullOrEmpty(username)
                  | string.IsNullOrEmpty(password)
                  | string.IsNullOrEmpty(verify)
                  | string.IsNullOrEmpty(email)
                )
            {
                AccountHelper.RegisterErrorReturnData("请填写完整", 0, context);
                return;
            }
            #endregion

            #region 邮箱,密码,用户名合法性校验，已注释
            //if (!Regex.IsMatch(email, @"^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$"))
            //{
            //    AccountHelper.RegisterErrorReturnData("邮箱格式错误", 1, context);
            //    return;
            //}

            //if (!Regex.IsMatch(password, @"^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~]{6,20}$"))
            //{
            //    AccountHelper.RegisterErrorReturnData("密码出现非法字符", 2, context);
            //    return;
            //}


            ////长度校验
            //int userNameByteLength = 0;
            //for (int i = 0; i < username.Length; i++)
            //{
            //    if (Regex.IsMatch(username[i].ToString(), @"[^\x00-\xff]"))
            //    {
            //        userNameByteLength += 2;
            //    }
            //    else
            //    {
            //        userNameByteLength++;
            //    }
            //}
            //if (userNameByteLength > 20 || userNameByteLength < 4)
            //{
            //    AccountHelper.RegisterErrorReturnData("用户名过长或过短", 3, context);
            //    return;
            //}
            ////合法性校验
            //if (!Regex.IsMatch(username, @"^[\u4e00-\u9fa5a-zA-Z0-9_]{1,20}$"))
            //{
            //    AccountHelper.RegisterErrorReturnData("用户名非法", 3, context);
            //    return;
            //}
            #endregion

            #region 验证码校验
            var serverVCode = context.Session["user_vcode"];
            if (serverVCode == null)
            {
                AccountHelper.RegisterErrorReturnData("验证码错误", 4, context);
                return;
            }
            //真正的验证码正误判断
            if (serverVCode.ToString().ToUpper() != verify.ToUpper())
            {
                AccountHelper.RegisterErrorReturnData("验证码错误", 4, context);
                return;
            }
            //验证码用完要扔掉
            context.Session["user_vcode"] = null;
            #endregion

            #region Discuz,数据库注册

            IUcClient client = new UcClient();
            UcUserRegister result = client.UserRegister(username, password, email);

            //注册结果处理
            switch (result.Result)
            {
                case RegisterResult.ContainsInvalidWords:
                    AccountHelper.RegisterErrorReturnData("包含不允许注册的词语", 4, context);
                    break;
                case RegisterResult.EmailHasBeenRegistered:
                    AccountHelper.RegisterErrorReturnData("邮箱已经存在", 3, context);
                    break;
                case RegisterResult.EmailNotAllowed:
                    AccountHelper.RegisterErrorReturnData("此邮箱不允许注册", 3, context);
                    break;
                case RegisterResult.IncorrectEmailFormat:
                    AccountHelper.RegisterErrorReturnData("邮箱格式错误", 3, context);
                    break;
                case RegisterResult.Success:
                    //把新用户保存到数据库中
                    Picture.Model.UserInfoModel userModel = new Picture.Model.UserInfoModel();

                    userModel.Uid = result.Uid;
                    userModel.UserStatus = 0;
                    userModel.UserName = username;

                    int insertResult = bllUserInfo.Insert(userModel);
                    if (insertResult <= 0)
                    {
                        AccountHelper.RegisterErrorReturnData("未知错误", 4, context);
                    }
                    break;
                case RegisterResult.UserNameExists:
                    AccountHelper.RegisterErrorReturnData("用户名已经存在", 1, context);
                    break;
                case RegisterResult.UserNameIllegal:
                    AccountHelper.RegisterErrorReturnData("用户名非法", 1, context);
                    break;
                default:
                    break;
            }


            #endregion

            #region 构建数据对象

            //在论坛改了头像后,但是头像的url是不变的,但内容会变,所以添加一个后缀,让浏览器每次都请求头像
            Random r = new Random();

            User testUser = new User()
            {
                UserName = username,
                EMail = email,
                UserFaceMiddle = client.AvatarUrl(result.Uid, AvatarSize.Middle) + "?" + r.Next(100),
                UserFacePathLarge = client.AvatarUrl(result.Uid, AvatarSize.Big) + "?" + r.Next(100),
                UserFacePathSmall = client.AvatarUrl(result.Uid, AvatarSize.Small) + "?" + r.Next(100),
                UId = result.Uid,
                UserStatus = 0
            };

            #endregion

            #region 构建返回信息



            //写入Session,搜索栏右侧的用户信息从Session中获取
            context.Session["current_user"] = testUser;
            var returnData = new
            {
                isRegister = true,
            };
            context.Response.Write(JSONHelper.ToJSONString(returnData));
            return;

            #endregion

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