using Picture.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Picture.BLL
{
    public partial class PictureCollectBLL
    {
        /// <summary>
        /// 获得用户收藏的图片
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="wheres"></param>
        /// <param name="orderField"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public IEnumerable<PictureCollectionMoreModel> GetCollectWithPictureInfo(int index, int size, object wheres, string orderField, bool isDesc = true)
        {
            return _dao.GetCollectWithPictureInfo(index, size, wheres, orderField, isDesc);
        }
    }
}
