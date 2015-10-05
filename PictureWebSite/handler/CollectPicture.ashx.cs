using Picture.Utility;
using PictureWebSite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace PictureWebSite.handler
{
    /// <summary>
    /// CollectPicture 的摘要说明
    /// </summary>
    public class CollectPicture : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            /*
             * 约定
             * 1   用户未登入
             * 2   图片不存在
             * 3   在点击收藏情况下,图片已收藏
             * 4   在删除收藏的情况下,图片已删除收藏
             * 5   未知错误: 图片更新后返回的影响条数不为1
             * 
             */



            context.Response.ContentType = "text/plain";
            int pId = int.Parse(context.Request["pId"]);
            bool isCollect = bool.Parse(context.Request["isCollect"]);

            //用户登入校验
            User user = context.Session["current_user"] as User;
            if (user == null)
            {
                CollectError(context, 1);
                return;
            }

            Picture.BLL.PictureCollectBLL collectBll = new Picture.BLL.PictureCollectBLL();
            Picture.BLL.PictureInfoBLL pictureBll = new Picture.BLL.PictureInfoBLL();

            if (!isCollect)
            {
                //收藏
                //先看图片是否存在
                var pic = pictureBll.QuerySingle(new { PId = pId });
                if (pic==null)
                {
                    CollectError(context, 2);
                    return;
                }

                //是否已经收藏了
                int cCount = collectBll.QueryCount(new
                {
                    PId = pId,
                    __o = "and",
                    CuId = user.UId
                });
                if (cCount > 0)
                {
                    CollectError(context, 3);
                    return;
                }

                //用户已登入,图片存在,图片未收藏

                //添加收藏
                collectBll.Insert(new Picture.Model.PictureCollectModel()
                {
                    CollectDate = DateTime.Now,
                    CuId = user.UId,
                    PId = pId
                });

                pic.CollectCount++;
                //修改图片数据
                if (pictureBll.Update(pic)!=1)
                {
                    CollectError(context, 5);
                    return;
                }

            }
            else
            {
                //取消收藏
                
                //图片是否存在
                var pic = pictureBll.QuerySingle(new { PId = pId });
                if (pic == null)
                {
                    CollectError(context, 2);
                    return;
                }


                //是否已经收藏了
                var collect = collectBll.QuerySingle(new
                {
                    PId=pId,
                    __o="and",
                    CuId=user.UId
                });
                //如果已经取消收藏了
                if (collect==null)
                {
                    CollectError(context, 4);
                    return;
                }

                //用户已登入,图片存在,图片已收藏
                //删除收藏
                collectBll.Delete(collect.CId);
                //修改图片数据
                pic.CollectCount--;
                if (pictureBll.Update(pic) != 1)
                {
                    CollectError(context, 5);
                    return;
                }
            }

            //添加/删除收藏成功
            context.Response.Write(JSONHelper.ToJSONString(new
            {
                isCollect = true
            }));


        }


        /// <summary>
        /// 收藏失败
        /// </summary>
        /// <param name="context"></param>
        /// <param name="errorCode"></param>
        public void CollectError(HttpContext context, int errorCode)
        {
            context.Response.Write(JSONHelper.ToJSONString(new
            {
                isCollect = false,
                errorCode = errorCode
            }));
            return;
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