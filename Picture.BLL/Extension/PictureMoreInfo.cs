using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Picture.Model;

namespace Picture.BLL
{
    /// <summary>
    /// 用于首页展现图片数据（牵扯到多张表）
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
        /// 获得图片数据，这个数据是有带标签名,和用户信息,查了数据库2*n+1次
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="wheres"></param>
        /// <param name="orderField"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public List<PictureMoreInfoModel> GetPictureInfoWithTag(int index, int size, object wheres, string orderField, bool isDesc = true)
        {
            var pictureInfoList = pictureBll.QueryList(index, size, wheres, orderField, isDesc);
            List<PictureMoreInfoModel> resultList = new List<PictureMoreInfoModel>();


            foreach (var item in pictureInfoList)
            {
                resultList.Add(new PictureMoreInfoModel()
                {
                    Tags = tagBll.GetTagsByImgId(item.PId),
                    CollectCount = item.CollectCount,
                    Height = item.Height,
                    PId = item.PId,
                    UInfo=userInfoBll.QuerySingle(item.UId),
                    LargeImgPath = item.LargeImgPath,
                    ImgSummary = item.ImgSummary,
                    UploadDate = item.UploadDate,
                    Width = item.Width
                });
            }
            return resultList;
        }

    }
}
