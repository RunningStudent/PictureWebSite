using DS.Web.UCenter.Client;
using PictureWebSite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using DS.Web.UCenter;
using Picture.Utility;

namespace PictureWebSite.account
{
    /// <summary>
    /// UserSetting 的摘要说明
    /// </summary>
    public class UserSetting : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //如果这个用户已经因长时间不操作自动登出，跳转到首页
            if (context.Session["current_user"] == null)
            {
                context.Response.Redirect("Index.aspx");
                return;
            }

            string formType = context.Request["formType"];
            User user = context.Session["current_user"] as User;


            IUcClient client = new UcClient();
            UcUserEdit result;

            //不同表单的处理1代表邮箱修改，2代表密码修改
            if (formType == "1")
            {
                string newEmail = context.Request["newEmail"];
                string mailChange_password = context.Request["password"];
                result = client.UserEdit(user.UserName, mailChange_password, "", newEmail);
                ChangeResultProcess(context, 1, newEmail, user, result);
            }
            else
            {
                string newPassword = context.Request["newPassword"];
                string currentPassword = context.Request["currentPassword"];
                result = client.UserEdit(user.UserName, currentPassword, newPassword, "");
                ChangeResultProcess(context, 2, "", user, result);
            }

            








            // ChangeResultProcess(context, newEmail, user, result);

        }

        /// <summary>
        /// 处理登入UCenter登入结果
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <param name="formType">修改的表单类型</param>
        /// <param name="data">修改的数据，这里这个数据只用于修改保存在Session中的Emai信息</param>
        /// <param name="user">用户的信息，用于修改有再保存到Session中</param>
        /// <param name="result">UCenter的返回结果</param>
        private  void ChangeResultProcess(HttpContext context, int formType, string data, User user, DS.Web.UCenter.UcUserEdit result)
        {
            switch (result.Result)
            {
                case DS.Web.UCenter.UserEditResult.DoNotEdited:
                    UserSettingChangeReturnData(2, "未做任何修改", context);
                    break;
                case DS.Web.UCenter.UserEditResult.EditedNothing:
                    UserSettingChangeReturnData(2, "未做任何修改", context);
                    break;
                case DS.Web.UCenter.UserEditResult.EmailHasBeenRegistered:
                    UserSettingChangeReturnData(3, "邮箱已被注册了", context);
                    break;
                case DS.Web.UCenter.UserEditResult.EmailNotAllowed:
                    UserSettingChangeReturnData(3, "该邮箱不允许注册", context);
                    break;
                case DS.Web.UCenter.UserEditResult.IncorrectEmailFormat:
                    UserSettingChangeReturnData(3, "邮箱格式错误", context);
                    break;
                case DS.Web.UCenter.UserEditResult.PassWordError:
                    UserSettingChangeReturnData(3, "密码错误", context);
                    break;
                case DS.Web.UCenter.UserEditResult.Success:
                    if (formType==1)
                    { 
                        //修改保存在Session中的数据，免得刷新后出现就得数据
                        user.EMail = data;
                        context.Session["current_user"] = user;
                        UserSettingChangeReturnData(1, "修改成功", context);
                    }
                    else
                    {
                        UserSettingChangeReturnData(1, "修改成功", context);
                    }
                    break;
                case DS.Web.UCenter.UserEditResult.UserIsProtected:
                    UserSettingChangeReturnData(3, "该用户受保护无法修改", context);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 修改邮箱失败返回错误信息
        /// </summary>
        /// <param name="stateCode">显示在前端的警告栏样式，1为success，2为info，3为danger</param>
        /// <param name="context"></param>
        public  void UserSettingChangeReturnData(int stateCode, string returnMessage, HttpContext context)
        {
            var data = new
            {
                stateCode = stateCode,
                returnMessage = returnMessage
            };

            context.Response.Write(JSONHelper.ToJSONString(data));
            context.Response.End();

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