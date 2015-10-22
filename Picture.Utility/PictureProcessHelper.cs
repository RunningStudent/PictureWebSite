using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Picture.Utility
{
    public class PictureProcessHelper
    {

        /// <summary>
        /// 获得图片的长宽
        /// </summary>
        /// <param name="picturePath">图片路径</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public static void GetPictureShap(string picturePath,out int width, out int height)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(picturePath);
            width = img.Width;
            height = img.Height;
            img.Dispose();
        }

        public static void MakeThumbnail(Image originalImage, string thumbnailPath, int width, int height, string mode)
        {
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置画板质量
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
             new Rectangle(x, y, ow, oh),
             GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }



        ///<summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径（物理路径）</param> 
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式,HW:指定高宽缩放(可能变形),W:指定宽，高按比例,即宽度为指定高度自动适配,H:指定高，宽按比例,Cut:指定高宽裁减（不变形）</param>     
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            MakeThumbnail(originalImage, thumbnailPath, width, height, mode);
        }


        /// <summary>
        /// 裁剪图片,返回图片对象
        /// </summary>
        /// <param name="srcImgPath"></param>
        /// <param name="tarImgPath"></param>
        /// <param name="originalX"></param>
        /// <param name="originalY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image CutPhoto(String srcImgPath, int originalX, int originalY, int width, int height)
        {
            Bitmap tarImg = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(tarImg);
            Bitmap srcImg = new Bitmap(srcImgPath);
            g.DrawImage(srcImg, new Rectangle(0, 0, width, height), new Rectangle(originalX, originalY, width, height), GraphicsUnit.Pixel);
            g.Dispose();
            srcImg.Dispose();
            return tarImg;

        }
    }
}
