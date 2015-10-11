using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PictureWebSite.Code;
using Picture.Utility;
using System.Web.SessionState;
using PictureWebSite.Model;
using System.Collections;
using System.Text;
using System.Security.Cryptography;

namespace PictureWebSite.handler
{
    /// <summary>
    /// PictureUpload 的摘要说明
    /// </summary>
    public class PictureUpload : IHttpHandler, IRequiresSessionState
    {
        /*
         * 与前端定个规则
         * errorCode:
         * 1     用户未登入
         * 2     图片格式错误
         * 3     图片大小错误
         * 4     该用户已经被禁止上传图片
         * 
         * 
         *  图片标签保存部分未校验是否保存成功
         *  
         */

        /// <summary>
        /// 大图路径
        /// </summary>
        private const string BIG_IMAGE_SAVE_PATH = "UpLoadPicture/";
        //这里保存到数据库的路径其为字符串,故要以页面位置为准写入图片路径
        //而保存图片需要绝对路径,这里会以该ashx文件找路径


        /// <summary>
        /// 图片大小限制
        /// </summary>
        private const int IMAGE_SIZE_LIMIT = 1024 * 1024 * 2;

        public void ProcessRequest(HttpContext context)
        {
            //并不知道如何用参数获得文件只能用索引的方式，反正就一张图
            context.Response.ContentType = "text/plain";
            var file = context.Request.Files[0];
            string tags = context.Request["tags"];
            string pictreSummary = context.Server.HtmlEncode(context.Request["summary"]);



            List<String> Tags = new List<String>();

            foreach (var item in tags.Split(new char[] { ',' }))
            {
                Tags.Add(item.Trim());
            }

            //去除重复的标签
            HashSet<string> hsTag = new HashSet<string>(Tags);



            #region 用户登入校验
            if (context.Session["current_user"] == null)
            {
                PictureUploadError(context, 1);
                return;
            }

            User user = context.Session["current_user"] as User;

            #endregion

            #region 用户是否被禁止上传图片
            if (user.UserStatus == 1)
            {
                PictureUploadError(context, 4);
                return;
            }
            #endregion

            #region 文件类型校验
            string fileName = file.FileName;
            string extName = Path.GetExtension(fileName);
            if (!(extName == ".jpeg" || extName == ".jpg" || extName == ".bmp" || extName == ".png" || extName == ".gif"))
            {
                PictureUploadError(context, 2);
                return;
            }
            #endregion

            #region 文件大小校验
            if (file.ContentLength > IMAGE_SIZE_LIMIT)
            {
                PictureUploadError(context, 3);
                return;
            }
            #endregion

            #region 保存文件

            #region  保存到磁盘
            //大小图所在的文件夹的相对路径
            string bigRelativePathDir = BIG_IMAGE_SAVE_PATH + DateTime.Now.ToString("yyyyMMdd");
            string smallRelativePathDir = BIG_IMAGE_SAVE_PATH + "SmallImg_" + DateTime.Now.ToString("yyyyMMdd");


            //大,小图所在的文件夹绝对路径
            string bigPathDir = context.Request.MapPath("../" + bigRelativePathDir);
            string smallPathDir = context.Request.MapPath("../" + smallRelativePathDir);
            //创建文件夹
            if (!Directory.Exists(bigPathDir))
            {
                Directory.CreateDirectory(bigPathDir);
                Directory.CreateDirectory(smallPathDir);
            }

            //算出文件的MD5
            string fileMD5Name = GetMD5FromFile(file.InputStream);
            string fileExt = Path.GetExtension(file.FileName);

            //大,小图所在的相对路径
            string bigImgRelativePath = bigRelativePathDir + "/" + fileMD5Name + fileExt;
            string smallImgRelativePath = smallRelativePathDir + "/" + "small_" + fileMD5Name + ".jpg";

            //大,小图所在的绝对路径
            string bigImgPath = context.Request.MapPath("../" + bigImgRelativePath);
            string smallImgPath = context.Request.MapPath("../" + smallImgRelativePath);




            //保存
            file.SaveAs(bigImgPath);


            PictureProcessHelper.MakeThumbnail(bigImgPath, smallImgPath, 400, 0, "W");

            #endregion

            #region 保存到数据库



            #region 获取标签对象,判断标签是否已经存在了，不存在则创建
            Picture.BLL.TagBLL tagBll = new Picture.BLL.TagBLL();
            List<Picture.Model.TagModel> tagModelList = new List<Picture.Model.TagModel>();
            Picture.Model.TagModel tempModel = null;

            foreach (var item in hsTag)
            {
                tempModel = tagBll.QuerySingle(new { TagName = item });
                if (tempModel == null)
                {
                    int tagId = tagBll.Insert(new Picture.Model.TagModel()
                    {
                        TagName = item
                    });
                    tagModelList.Add(new Picture.Model.TagModel()
                    {
                        TagName = item,
                        TId = tagId
                    });
                }
                else
                {
                    tagModelList.Add(tempModel);
                }
                tempModel = null;
            }

            #endregion

            #region  更新索引
            foreach (var item in tagModelList)
            {
                IndexManager.tagIndex.Add(item);
            }

            #endregion

            #region 保存图片




            Picture.BLL.PictureInfoBLL pictureBll = new Picture.BLL.PictureInfoBLL();

            int width = 0;
            int height = 0;
            PictureProcessHelper.GetPictureShap(bigImgPath, out width, out height);

            Picture.Model.PictureInfoModel model = new Picture.Model.PictureInfoModel()
            {
                CollectCount = 0,
                Height = height,
                Width = width,
                ImgSummary = pictreSummary,
                LargeImgPath = bigImgRelativePath,
                UId = user.UId,
                UploadDate = DateTime.Now
            };

            int pId = pictureBll.Insert(model);
            #endregion

            #region 保存图片与标签的关系
            Picture.BLL.TagImgRelationBLL bllTIR = new Picture.BLL.TagImgRelationBLL();

            foreach (var item in tagModelList)
            {
                bllTIR.Insert(new Picture.Model.TagImgRelationModel()
                {
                    ImgId = pId,
                    TagId = item.TId
                });
            }

            #endregion

            #endregion

            #endregion

            context.Response.Write(JSONHelper.ToJSONString(new { isUpload = true }));

        }


        private void PictureUploadError(HttpContext context, int errorCode)
        {
            var returnData = new
            {
                isUpload = false,
                errorCode = errorCode
            };
            context.Response.Write(JSONHelper.ToJSONString(returnData));

        }


        /// <summary>
        /// 生成文件MD5
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMD5FromFile(Stream sr)
        {
            using (MD5 md = MD5.Create())
            {
                //传入文件的流给MD5处理(循环读取什么的)
                byte[] md5byte = md.ComputeHash(sr);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < md5byte.Length; i++)
                {
                    //x代表转化成16进制,x2代表生成的16进制保留两位
                    //如果为大写X,生成16进制也是大写
                    sb.Append(md5byte[i].ToString("x2"));
                }
                return sb.ToString();
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