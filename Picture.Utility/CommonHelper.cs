using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Picture.Utility
{
    public static class CommonHelper
    {


        /// <summary>
        /// 根据大图路径获得小图路径
        /// </summary>
        /// <param name="largeImgPath"></param>
        /// <returns></returns>
        public static string GetSmallImgPath(string largeImgPath)
        {
            //UpLoadPicture/20151006/7.jpg
            //UpLoadPicture/SmallImg_20151006/smal_7.jpg
            string imgName = Path.GetFileNameWithoutExtension(largeImgPath);
            string smallImgName = "small_" + imgName + ".jpg";
            string parentDic = largeImgPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
            string finalImgPath = "/UpLoadPicture/SmallImg_" + parentDic + "/" + smallImgName;
            return finalImgPath;
        }
    }
}
