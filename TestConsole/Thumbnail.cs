using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;


namespace TestConsole
{
    public class Thumbnail
    {

        #region 制作缩略图
        /**/
        /// <summary>
        /// 制作缩略图(如果原图长大于宽以长为基准缩放,反之以宽為基准缩放放)
        /// </summary>
        /// <param name="fileName">原图路径</param>
        /// <param name="newFileName">新图路径</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        public static void MakeThumbnailImage(string fileName, string newFileName, int maxWidth, int maxHeight)
        {
            Image original = Image.FromFile(fileName);
            Size _newSize = ResizeImage(original.Width, original.Height, maxWidth, maxHeight);
            MakeImage(fileName, newFileName, _newSize, original);
        }

        /// <summary>
        /// 制作缩略图(如果原图长大于宽以长为基准缩放,反之以宽為基准缩放放)
        /// </summary>
        /// <param name="filePath">图片地址</param>
        /// <param name="maxWidth">缩放图片最大宽度</param>
        /// <param name="maxHeight">缩放图片最大高度</param>
        /// <returns></returns>
        public static Image MakeThumbnailImage(string filePath, int maxWidth, int maxHeight)
        {
            Image original = Image.FromFile(filePath);
            Size _newSize = ResizeImage(original.Width, original.Height, maxWidth, maxHeight);
            return MakeImage(_newSize, original);
        }


        public static Image MakeThumbnailImage(Image original, int maxWidth, int maxHeight)
        {
            Size _newSize = ResizeImage(original.Width, original.Height, maxWidth, maxHeight);
            return MakeImage(_newSize, original);
        }


        /// <summary>
        /// 制作缩略图(一切以宽度为基准缩放)
        /// </summary>
        /// <param name="fileName">原图路径</param>
        /// <param name="newFileName">新图路径</param>
        /// <param name="maxWidth">最大宽度</param>
        public static void MakeThumbnailImage(string fileName, string newFileName, int maxWidth)
        {
            Image original = Image.FromFile(fileName);
            Size _newSize = ResizeImage(original.Width, original.Height, maxWidth);
            MakeImage(fileName, newFileName, _newSize, original);
        }
        #endregion


        #region 缩放图片
        /// <summary>
        /// 等比缩放图片
        /// </summary>
        /// <param name="size">缩放后大小</param>
        /// <param name="image">原图片对象</param>
        /// <returns>返回缩放后的图片对象</returns>
        private static Image MakeImage(Size size, Image image)
        {
            Image displayImage = new Bitmap(image, size);
            Graphics g = Graphics.FromImage(displayImage);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            return displayImage;
        }


        private static void MakeImage(string fileName, string newFileName, Size size, Image image)
        {
            Image displayImage = new Bitmap(image, size);
            Graphics g = Graphics.FromImage(displayImage);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            try
            {
                Image tempImage = displayImage;
                image.Dispose();
                tempImage.Save(newFileName, GetCodecInfo("image/" + GetFormat(fileName).ToString().ToLower()), encoderParams);

            }
            catch { }
            finally
            {
                image.Dispose();

            }
        }



        #endregion


        #region 计算尺寸
        /// <summary>
        /// 计算新尺寸
        /// </summary>
        /// <param name="width">原始宽度</param>
        /// <param name="height">原始高度</param>
        /// <param name="maxWidth">最大新宽度</param>
        /// <param name="maxHeight">最大新高度</param>
        /// <returns></returns>
        public static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
        {
            decimal MAX_WIDTH = (decimal)maxWidth;
            decimal MAX_HEIGHT = (decimal)maxHeight;
            decimal ASPECT_RATIO = MAX_WIDTH / MAX_HEIGHT;//最大长宽比

            int newWidth, newHeight;

            decimal originalWidth = (decimal)width;
            decimal originalHeight = (decimal)height;

            //如果源图的长或者宽大于设定的就开始计算新的长宽
            if (originalWidth > MAX_WIDTH || originalHeight > MAX_HEIGHT)
            {
                decimal factor;
                // determine the largest factor 
                //如果源图的宽长比大于设定的,说明这个图过宽
                if (originalWidth / originalHeight > ASPECT_RATIO)
                {
                    factor = originalWidth / MAX_WIDTH;
                    //设置图片的宽度为设定的最大宽度
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    //高度根据比例缩小
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
                else
                {
                    factor = originalHeight / MAX_HEIGHT;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
            }
            else
            {
                //如果长宽没有超过设定的值
                newWidth = width;
                newHeight = height;
            }

            return new Size(newWidth, newHeight);

        }

        /// <summary>
        /// 计算新尺寸,只算宽度
        /// </summary>
        /// <param name="width">原始宽度</param>
        /// <param name="height">原始高度</param>
        /// <param name="maxWidth">最大新宽度</param>
        /// <returns></returns>
        public static Size ResizeImage(int width, int height, int maxWidth)
        {
            int newWidth, newHeight;
            decimal MAX_WIDTH = (decimal)maxWidth;
            decimal originalWidth = (decimal)width;
            decimal originalHeight = (decimal)height;
            if (originalWidth > MAX_WIDTH)
            {
                decimal factor;
                factor = originalWidth / MAX_WIDTH;
                newWidth = Convert.ToInt32(originalWidth / factor);
                newHeight = Convert.ToInt32(originalHeight / factor);
            }
            else
            {
                newWidth = width;
                newHeight = height;
            }
            return new Size(newWidth, newHeight);
        }

        #endregion
        




        /**/
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="image">Image 对象</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="ici">指定格式的编解码参数</param>
        private static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
        {
            //设置 原图片 对象的 EncoderParameters 对象
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)100));
            image.Save(savePath, ici, parameters);
            parameters.Dispose();
        }

        /**/
        /// <summary>
        /// 获取图像编码解码器的所有相关信息
        /// </summary>
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
        /// <returns>返回图像编码解码器的所有相关信息</returns>
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }


        /**/
        /// <summary>
        /// 得到图片格式
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns></returns>
        public static ImageFormat GetFormat(string name)
        {

            string ext = name.Substring(name.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }



        /**/
        /// <summary>
        /// 自定義縮放圖(縮略圖時以圖頂端為起點)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="newFileName"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        public static void MakeCustomImage(string fileName, string newFileName, int newWidth, int newHeight)
        {
            Image image = Image.FromFile(fileName);

            int i = 0;
            int width = image.Width;
            int height = image.Height;
            if (width > height)
            {
                i = height;
            }
            else
            {
                i = width;
            }
            Bitmap b = new Bitmap(newWidth, newHeight);


            try
            {
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.High;
                g.SmoothingMode = SmoothingMode.HighQuality;

                //清除整个绘图面并以透明背景色填充
                g.Clear(Color.Transparent);
                if (width < height)
                {
                    g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight), new Rectangle((width - newWidth) / 2, 0, newWidth, newHeight), GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight), new Rectangle((width - newWidth) / 2, 0, newWidth, newHeight), GraphicsUnit.Pixel);
                }

                image.Dispose();
                SaveImage(b, newFileName, GetCodecInfo("image/" + GetFormat(fileName).ToString().ToLower()));
            }
            finally
            {
                image.Dispose();
                b.Dispose();
            }

        }

        public static void MakeCustomImage(Image image, string fileName, string newFileName, int newWidth, int newHeight)
        {
            //Image image = Image.FromFile(fileName);

            int i = 0;
            int width = image.Width;
            int height = image.Height;
            if (width > height)
            {
                i = height;
            }
            else
            {
                i = width;
            }
            Bitmap b = new Bitmap(newWidth, newHeight);


            try
            {
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.High;
                g.SmoothingMode = SmoothingMode.HighQuality;

                //清除整个绘图面并以透明背景色填充
                g.Clear(Color.Transparent);
                if (width < height)
                {
                    g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight), new Rectangle((width - newWidth) / 2, 0, newWidth, newHeight), GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
                }

                image.Dispose();
                SaveImage(b, newFileName, GetCodecInfo("image/" + GetFormat(fileName).ToString().ToLower()));
            }
            finally
            {
                image.Dispose();
                b.Dispose();
            }

        }


        public static byte[] ResizeImageFile(byte[] imageFile, int targetSizeW, int targetSizeH)
        {
            System.Drawing.Image original = System.Drawing.Image.FromStream(new MemoryStream(imageFile));
            int targetH, targetW;
            targetW = targetSizeW;
            targetH = (int)(original.Height * ((float)targetSizeW / (float)original.Width));
            if (targetH > targetSizeH)
            {
                targetH = targetSizeH;
                targetW = (int)(original.Width * ((float)targetSizeH / (float)original.Height));
            }
            if (targetSizeW < (int)original.Width || targetSizeH < (int)original.Height)
            {
                System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(new MemoryStream(imageFile));
                // Create a new blank canvas.  The resized image will be drawn on this canvas.
                Bitmap bmPhoto = new Bitmap(targetW, targetH, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(72, 72);
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, targetW, targetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);
                // Save out to memory and then to a file.  We dispose of all objects to make sure the files don't stay locked.
                MemoryStream mm = new MemoryStream();
                bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);
                original.Dispose();
                imgPhoto.Dispose();
                bmPhoto.Dispose();
                grPhoto.Dispose();
                return mm.GetBuffer();
            }
            else
            {
                return imageFile;
            }
        }

    }


}
