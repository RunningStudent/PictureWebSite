using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using PictureWebSite.handlerHelper;
using System.Text.RegularExpressions;
using PictureWebSite.Model;
using DS.Web.UCenter.Client;
using DS.Web.UCenter;
using Picture.Utility;
using Picture.Model;
namespace PictureWebSite.account
{
    /// <summary>
    /// Login 的摘要说明
    /// 功能:
    /// 表单为空校验
    /// 验证码校验
    /// 登入(伪登入)
    /// 信息封装(登入成功,失败是不同套数据)
    /// 
    /// 待加功能
    /// 真登入
    /// 用户名密码合法性校验
    /// 记住密码
    /// 
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            /*
             * 存在问题
             * 
             * 完全可以去掉，登入错误时候返回的提示信息
             * 改为返回错误码，与前端定个规则，1代表表单未填满，2代表密码错误
             * 
             * 登入部分是否可以不用自己写的Enum，简化那个部分的代码
             * 
             * 
             * 未完成
             * 
             * 校验用户是否为禁言状态
             * 
             */

            context.Response.ContentType = "text/html";
            string verify = context.Request["verify"];
            string username = context.Request["username"];
            string password = context.Request["password"];

            #region 表单为空校验
            if (string.IsNullOrEmpty(username)
                  | string.IsNullOrEmpty(password)
                  | string.IsNullOrEmpty(verify))
            {
                AccountHelper.LoginErrorReturnData("请填写完整", 0, context);
                return;
            }
            #endregion

            #region 用户名合法性校验,已经注释

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
            //if (userNameByteLength > 20 || userNameByteLength < 2)
            //{
            //    AccountHelper.LoginErrorReturnData("用户名过长或过短", 3, context);
            //    return;
            //}
            ////合法性校验
            //if (!Regex.IsMatch(username, @"^[\u4e00-\u9fa5a-zA-Z0-9_]{1,20}$"))
            //{
            //    AccountHelper.LoginErrorReturnData("用户名非法", 3, context);
            //    return;
            //}


            #endregion

            #region 验证码校验
            //使用非法手段,当验证码未生成变请求这个网站时,验证码未空,所以要判断
            var serverVCode = context.Session["user_vcode"];


            if (serverVCode == null)
            {
                AccountHelper.LoginErrorReturnData("验证码错误", 3, context);
                return;
            }
            //真正的验证码正误判断
            if (serverVCode.ToString().ToUpper() != verify.ToUpper())
            {
                AccountHelper.LoginErrorReturnData("验证码错误", 3, context);
                return;
            }

            //验证码用完要扔掉
            context.Session["user_vcode"] = null;
            #endregion

            #region 登入部分

            IUcClient client = new UcClient();
            UcUserLogin result = client.UserLogin(username, password);
            Picture.Model.Enums.LoginResult loginResult = Picture.Model.Enums.LoginResult.未知错误;
            Picture.BLL.UserInfoBLL bllUserInfo = new Picture.BLL.UserInfoBLL();


            //登入结果获取
            switch (result.Result)
            {
                case LoginResult.NotExist:
                    loginResult = Picture.Model.Enums.LoginResult.用户名不存在;
                    break;
                case LoginResult.PassWordError:
                    loginResult = Picture.Model.Enums.LoginResult.密码错误;
                    break;
                case LoginResult.QuestionError:
                    break;
                case LoginResult.Success:
                    loginResult = Picture.Model.Enums.LoginResult.登录成功;
                    //如果论坛有这个用户，而图片网没有
                    if (bllUserInfo.QueryCount(new { Uid = result.Uid }) <= 0)
                    {
                        Picture.Model.UserInfoModel model = new Picture.Model.UserInfoModel()
                        {
                            UserStatus = 0,
                            Uid = result.Uid,
                            UserName = result.UserName
                        };
                        //保存失败
                        if (bllUserInfo.Insert(model) <= 0)
                        {
                            loginResult = Picture.Model.Enums.LoginResult.未知错误;
                        }
                    }
                    break;
                default:
                    break;
            }

            #region 查看用户状态
            UserInfoModel userInfo = bllUserInfo.QuerySingle(result.Uid);
            if (userInfo.UserStatus == 1)
            {
                loginResult = Picture.Model.Enums.LoginResult.用户已被冻结;
            }



            #endregion


            //对结果进行相应的处理
            switch (loginResult)
            {
                case Picture.Model.Enums.LoginResult.用户名不存在:
                    AccountHelper.LoginErrorReturnData("用户名不存在", 1, context);
                    return;
                case Picture.Model.Enums.LoginResult.密码错误:
                    AccountHelper.LoginErrorReturnData("密码错误", 2, context);
                    return;
                case Picture.Model.Enums.LoginResult.用户已被冻结:
                //AccountHelper.LoginErrorReturnData("用户已被冻结", 1, context);
                //return;
                case Picture.Model.Enums.LoginResult.登录成功:

                    Random r = new Random();
                    User user = new User()
                    {
                        EMail = result.Mail,
                        UserName = username,
                        UserFaceMiddle = client.AvatarUrl(result.Uid, AvatarSize.Middle) + "?" + r.Next(100),
                        UserFacePathLarge = client.AvatarUrl(result.Uid, AvatarSize.Big) + "?" + r.Next(100),
                        UserFacePathSmall = client.AvatarUrl(result.Uid, AvatarSize.Small) + "?" + r.Next(100),
                        UId = result.Uid,
                        UserStatus = userInfo.UserStatus
                    };
                    //写入Session,搜索栏右侧的用户信息从Session中获取
                    context.Session["current_user"] = user;
                    break;
                case Picture.Model.Enums.LoginResult.未知错误:
                    AccountHelper.LoginErrorReturnData("未知错误", 3, context);
                    break;

                default:
                    break;
            }
            #endregion

            #region 返回信息构建


            //登入成功
            var data = new
            {
                isLogined = true,
            };
            context.Response.Write(JSONHelper.ToJSONString(data));
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