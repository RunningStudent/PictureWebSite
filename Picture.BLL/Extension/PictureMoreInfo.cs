using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Picture.Model;

namespace Picture.BLL
{
    /// <summary>
    /// 用于首页展现图片数据（牵扯到多张表）
    /// 共两个方法
    /// GetPictureInfoWithTagAndUserInfo
    /// 用于主页,查询图片信息,图片上传者信息,图片标签信息,图片是否被当前用户收藏信息
    /// GetSinglePictureInfoWithTagAndUserInfo
    /// 用户用户中心查询图片信息,图片上传者信息,图片标签信息,图片是否被当前用户收藏信息
    /// </summary>
    public partial class PictureMoreInfoBLL
    {
        /// <summary>
        /// 用于查图片数据
        /// </summary>
        public Picture.DAL.PictureInfoDAL pictureBll = new Picture.DAL.PictureInfoDAL();

        /// <summary>
        /// 用于查标签数据
        /// </summary>
        public Picture.DAL.Tag tagBll = new Picture.DAL.Tag();

        /// <summary>
        /// 用于查用户数据
        /// </summary>
        public Picture.DAL.UserInfoDAL userInfoBll = new Picture.DAL.UserInfoDAL();


        /// <summary>
        /// 使用存储过程查图片,标签,收藏,用户数据
        /// </summary>
        public Picture.DAL.PictureMoreInfoDAL pictureMoreInfo = new Picture.DAL.PictureMoreInfoDAL();



        public Picture.DAL.PictureCollectDAL pictureCollect = new Picture.DAL.PictureCollectDAL();

        /// <summary>
        /// 获得图片数据，这个数据是有带标签名,和用户信息,查了数据库2*n+1次
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="wheres"></param>
        /// <param name="orderField"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public List<PictureMoreInfoModel> GetPictureInfoWithTagAndUserInfo(int index, int size, string orderField, bool isDesc = true, int currentUid = -1)
        {
            var pictureInfoList = pictureMoreInfo.QueryList(index, size, orderField, isDesc, currentUid).ToList();
            // List<PictureMoreInfoModel> resultList = new List<PictureMoreInfoModel>();
            for (int i = 0; i < pictureInfoList.Count(); i++)
            {
                pictureInfoList[i].Tags = tagBll.GetTagsByImgId(pictureInfoList[i].PId);
            }
            //foreach (var item in pictureInfoList)
            //{
            //    //resultList.Add(new PictureMoreInfoModel()
            //    //{
            //    //    Tags = tagBll.GetTagsByImgId(item.PId),
            //    //    CollectCount = item.CollectCount,
            //    //    Height = item.Height,
            //    //    PId = item.PId,
            //    //    UInfo = userInfoBll.QuerySingle(item.UId),
            //    //    LargeImgPath = item.LargeImgPath,
            //    //    ImgSummary = item.ImgSummary,
            //    //    UploadDate = item.UploadDate,
            //    //    Width = item.Width
            //    //});
            //}
            return pictureInfoList;
        }

        /// <summary>
        /// 根据图片id获取单个图片数据
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public PictureMoreInfoModel GetSinglePictureInfoWithTagAndUserInfo(int pid, int currentUid)
        {

            var pictureModel = pictureBll.QuerySingle(pid);
            var UInfo = userInfoBll.QuerySingle(pictureModel.UId);
            var isCollect = pictureCollect.QueryCount(new { Pid = pid,__o="and", CuId = currentUid });
            return new PictureMoreInfoModel()
            {
                CollectCount = pictureModel.CollectCount,
                Height = pictureModel.Height,
                PId = pictureModel.PId,
                LargeImgPath = pictureModel.LargeImgPath,
                ImgSummary = pictureModel.ImgSummary,
                UploadDate = pictureModel.UploadDate,
                Width = pictureModel.Width,
                Tags = tagBll.GetTagsByImgId(pictureModel.PId),
                UId = UInfo.Uid,
                UserName = UInfo.UserName,
                UserStatus = UInfo.UserStatus,
                isCollect = isCollect > 0 ? 1 : 0
            };
        }


    }
}
