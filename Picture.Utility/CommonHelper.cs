using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
            //UpLoadPicture/20151007/9f50ecd8f40cad69273a1894b54760c3.jpg
            string imgName = Path.GetFileNameWithoutExtension(largeImgPath);
            string smallImgName = "small_" + imgName + ".jpg";
            string parentDic = largeImgPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
            string finalImgPath = "UpLoadPicture/SmallImg_" + parentDic + "/" + smallImgName;
            return finalImgPath;
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

    }
}
