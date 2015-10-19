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



        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            //并不知道如何用参数获得文件只能用[0]的方式，反正就一张图
            var file = context.Request.Files[0];
            string tags = context.Request["tags"];
            string pictreSummary = context.Server.HtmlEncode(context.Request["summary"]);

            List<String> Tags = new List<String>();
            foreach (var item in tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
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

            //文件类型校验
            if (!FileExtCheck(file))
            {
                PictureUploadError(context, 2);
            }

            //文件大小校验
            if (file.ContentLength > GlobalSetting.PictureSize)
            {
                PictureUploadError(context, 3);
                return;
            }

            #region 保存文件

            //保存到磁盘
            string bigImgRelativePath;
            string bigImgPath;
            string smallImgPath;
            GetSavePath(context, file, out bigImgRelativePath, out bigImgPath, out smallImgPath);
            //保存
            file.SaveAs(bigImgPath);
            //缩略图保存
            MakeSmallPicture(bigImgPath, smallImgPath);
           

            #region 保存到数据库

            //保存标签到数据库
            List<Picture.Model.TagModel> tagModelList = TagProcess(hsTag);

            //更新索引
            foreach (var item in tagModelList)
            {
                IndexManager.tagIndex.Add(item);
            }
            int pId = PictureInfoProcess(pictreSummary, user, bigImgRelativePath, bigImgPath);
            TagImgRelationProcess(tagModelList, pId);

            #endregion

            #endregion


            context.Response.Write(JSONHelper.ToJSONString(new { isUpload = true }));

        }


        /// <summary>
        /// 制作缩略图
        /// </summary>
        /// <param name="bigImgPath"></param>
        /// <param name="smallImgPath"></param>
        private void MakeSmallPicture(string bigImgPath, string smallImgPath)
        {
            PictureProcessHelper.MakeThumbnail(bigImgPath, smallImgPath, 400, 0, "W");
        }


        /// <summary>
        /// 保存标签与图片关系到TagImgRelation表中
        /// </summary>
        /// <param name="tagModelList"></param>
        /// <param name="pId"></param>
        private  void TagImgRelationProcess(List<Picture.Model.TagModel> tagModelList, int pId)
        {
            Picture.BLL.TagImgRelationBLL bllTIR = new Picture.BLL.TagImgRelationBLL();
            foreach (var item in tagModelList)
            {
                bllTIR.Insert(new Picture.Model.TagImgRelationModel()
                {
                    ImgId = pId,
                    TagId = item.TId
                });
            }
        }


        /// <summary>
        /// 保存图片数据到PictureInfo表中
        /// </summary>
        /// <param name="pictreSummary"></param>
        /// <param name="user"></param>
        /// <param name="bigImgRelativePath"></param>
        /// <param name="bigImgPath"></param>
        /// <returns></returns>
        private  int PictureInfoProcess(string pictreSummary, User user, string bigImgRelativePath, string bigImgPath)
        {
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
            return pId;
        }


        /// <summary>
        /// 获取标签对象,判断标签是否已经存在了，不存在则创建,返回该图的标签对象集合
        /// </summary>
        /// <param name="hsTag"></param>
        /// <returns></returns>
        private  List<Picture.Model.TagModel> TagProcess(HashSet<string> hsTag)
        {
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
            return tagModelList;
        }

        /// <summary>
        /// 获得大小图路径
        /// </summary>
        /// <param name="context"></param>
        /// <param name="file"></param>
        /// <param name="bigImgRelativePath"></param>
        /// <param name="bigImgPath"></param>
        /// <param name="smallImgPath"></param>
        private  void GetSavePath(HttpContext context, HttpPostedFile file, out string bigImgRelativePath, out string bigImgPath, out string smallImgPath)
        {
            //大小图所在的文件夹的相对路径
            string bigRelativePathDir = BIG_IMAGE_SAVE_PATH + DateTime.Now.ToString("yyyyMMdd");
            string smallRelativePathDir = BIG_IMAGE_SAVE_PATH + "SmallImg_" + DateTime.Now.ToString("yyyyMMdd");

            //大,小图所在的文件夹绝对路径
            string bigPathDir = context.Request.MapPath("../" + bigRelativePathDir);
            string smallPathDir = context.Request.MapPath("../" + smallRelativePathDir);

            //创建文件夹
            GreateDirectory(bigPathDir);
            GreateDirectory(smallPathDir);

            //算出文件的MD5
            string fileMD5Name = CommonHelper.GetMD5FromFile(file.InputStream);
            string fileExt = Path.GetExtension(file.FileName);

            //大,小图所在的相对路径
            bigImgRelativePath = bigRelativePathDir + "/" + fileMD5Name + fileExt;
            string smallImgRelativePath = smallRelativePathDir + "/" + "small_" + fileMD5Name + ".jpg";

            //大,小图所在的绝对路径
            bigImgPath = context.Request.MapPath("../" + bigImgRelativePath);
            smallImgPath = context.Request.MapPath("../" + smallImgRelativePath);
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        private  void GreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 检测图片后缀名
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool FileExtCheck(HttpPostedFile file)
        {
            string fileName = file.FileName;
            string extName = Path.GetExtension(fileName);
            if (!(extName == ".jpeg" || extName == ".jpg" || extName == ".bmp" || extName == ".png" || extName == ".gif"))
            {
                return false;

            }
            return true;
        }


        /// <summary>
        /// 上传文件错误处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="errorCode"></param>
        private void PictureUploadError(HttpContext context, int errorCode)
        {
            var returnData = new
            {
                isUpload = false,
                errorCode = errorCode
            };
            context.Response.Write(JSONHelper.ToJSONString(returnData));

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