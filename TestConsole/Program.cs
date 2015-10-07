using Gif.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace TestConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<Image> gifList = GetFrames("1.gif");
            Zgke.MyImage.ImageGif _Gif = new Zgke.MyImage.ImageGif(300, 300);
            for (int i = 0; i < gifList.Count; i++)
            {
                _Gif.AddImage(gifList[i], 1, true, Zgke.MyImage.ImageGif.DisposalMethod.NoDisposalMethod);
            }
            _Gif.SaveFile(@"D:/1.gif");
            GetThumbnail("1.gif", 300, 300, @"E:/1.gif");
            Console.WriteLine("结束");
            Console.ReadKey();
        }
        private static int imgH;
        private static int imgW;





        /// <summary>
        /// 裁剪GIF图片
        /// </summary>
        /// <param name="gifFilePath">源文件地址</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        /// <param name="outputPath">输出文件地址</param>
        public static void GetThumbnail(string gifFilePath, int maxWidth, int maxHeight, string outputPath)
        {
            List<Image> gifList = GetFrames(gifFilePath);
            AnimatedGifEncoder ae = new AnimatedGifEncoder();
            ae.SetQuality(1);
            ae.Start();
            ae.SetDelay(delay);    // 延迟间隔
            ae.SetRepeat(0);  //-1:不循环,0:总是循环 播放  
            //我这里是等比缩小（如果原图长大于宽以长为基准缩放,反之以宽为基准缩放)
            Size _newSize = Thumbnail.ResizeImage(gifSize.Width, gifSize.Height, maxWidth, maxHeight);
            ae.SetSize(_newSize.Width, _newSize.Height);

            for (int i = 0, count = gifList.Count; i < count; i++)
            {
                //Image frame = Image.FromFile(gifList[i]);
                ae.AddFrame(gifList[i]);
            }
            ae.Finish();
            ae.Output(outputPath);
        }


        private static Size gifSize;
        private static int delay;
        //解码gif图片
        public static List<Image> GetFrames(string pPath)
        {
            Image gif = Image.FromFile(pPath);
            gifSize = new Size(gif.Width, gif.Height);
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);
            //获取帧数(gif图片可能包含多帧，其它格式图片一般仅一帧)
            int count = gif.GetFrameCount(fd);
            // List<string> gifList = new List<string>();
            List<Image> gifList = new List<Image>();
            //以Jpeg格式保存各帧
            for (int i = 0; i < count; i++)
            {
                gif.SelectActiveFrame(fd, i);
                //gif图片一般每一帧的间隔时间都一样,所以只拿一次图片的时间间隔
                if (i == 0)
                {
                    for (int j = 0; j < gif.PropertyIdList.Length; j++)//遍历帧属性
                    {
                        //如果是延迟时间
                        //可以去MSDNhttps://msdn.microsoft.com/zh-cn/library/system.drawing.imaging.propertyitem.id.aspx
                        //查看其它属性的id值
                        if ((int)gif.PropertyIdList.GetValue(j) == 0x5100)
                        {
                            PropertyItem pItem = (PropertyItem)gif.PropertyItems.GetValue(j);//获取延迟时间属性

                            //倘若这个图有7帧,那么这个pItem.Value是7*4个byte,每个4个byte记录了图片的时间间隔

                            //这里的i已经是0了
                            byte[] delayByte = new byte[4];//延迟时间，以1/100秒为单位
                            delayByte[0] = pItem.Value[i * 4];
                            delayByte[1] = pItem.Value[1 + i * 4];
                            delayByte[2] = pItem.Value[2 + i * 4];
                            delayByte[3] = pItem.Value[3 + i * 4];

                            //截获一个片段的图片间隔保存到byte[]中,最后把它转换成毫秒
                            delay = BitConverter.ToInt32(delayByte, 0) * 10; //乘以10，获取到毫秒
                            break;
                        }
                    }
                }

               // gifList.Add(new Bitmap(gif));//这里可以将每帧图片保存成文件gif.save(,);根据需求
                gifList.Add(Thumbnail.MakeThumbnailImage(new Bitmap(gif), 300, 300));
            }
            gif.Dispose();
            return gifList;
        }



        /// <summary> 

        /// 设置GIF大小 
        /// </summary> 
        /// <param name="path">图片路径</param> 
        /// <param name="width">宽</param> 
        /// <param name="height">高</param> 
        private static void setGifSize(string path, int width, int height)
        {
            Image gif = new Bitmap(width, height);
            Image frame = new Bitmap(width, height);
            Image res = Image.FromFile(path);
            Graphics g = Graphics.FromImage(gif);
            Rectangle rg = new Rectangle(0, 0, width, height);
            Graphics gFrame = Graphics.FromImage(frame);

            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.InterpolationMode = InterpolationMode.Low;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;


            gFrame.CompositingQuality = CompositingQuality.HighSpeed;
            gFrame.SmoothingMode = SmoothingMode.HighSpeed;
            gFrame.InterpolationMode = InterpolationMode.Low;
            gFrame.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            gFrame.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;


            foreach (Guid gd in res.FrameDimensionsList)
            {
                FrameDimension fd = new FrameDimension(gd);

                //因为是缩小GIF文件所以这里要设置为Time，如果是TIFF这里要设置为PAGE，因为GIF以时间分割，TIFF为页分割 
                FrameDimension f = FrameDimension.Time;
                int count = res.GetFrameCount(fd);
                ImageCodecInfo codecInfo = GetEncoder(ImageFormat.Gif);
                System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.SaveFlag;
                EncoderParameters eps = null;

                for (int i = 0; i < count; i++)
                {
                    res.SelectActiveFrame(f, i);
                    if (0 == i)
                    {

                        g.DrawImage(res, rg);

                        eps = new EncoderParameters(1);

                        //第一帧需要设置为MultiFrame 

                        eps.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.MultiFrame);
                        bindProperty(res, gif);
                        gif.Save(@"D:\aaa.gif", codecInfo, eps);
                    }
                    else
                    {

                        gFrame.DrawImage(res, rg);

                        eps = new EncoderParameters(1);

                        //如果是GIF这里设置为FrameDimensionTime，如果为TIFF则设置为FrameDimensionPage 

                        eps.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.FrameDimensionTime);

                        bindProperty(res, frame);
                        gif.SaveAdd(frame, eps);
                    }
                }

                eps = new EncoderParameters(1);
                eps.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.Flush);
                gif.SaveAdd(eps);
            }
        }


        public static void TestGIF(string path, int width, int height)
        {
            var list = GetFrames(path);
            ImageCodecInfo codecInfo = GetEncoder(ImageFormat.Gif);
            EncoderParameters eps = null;
            Image gif = new Bitmap(width, height);
            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.SaveFlag;






            for (int i = 0; i < list.Count(); i++)
            {
                if (i == 0)
                {
                    Image temp = new Bitmap(width, height);

                    Graphics g = Graphics.FromImage(temp);
                    g.DrawImage(list[i], new Rectangle(0, 0, width, height), 0, 0, imgW, imgH, GraphicsUnit.Pixel);
                    eps = new EncoderParameters(1);
                    eps.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.MultiFrame);
                    bindProperty(temp, gif);
                    gif.Save(@"D:\11.gif", codecInfo, eps);
                }
                else
                {

                    Image temp = new Bitmap(width, height);

                    Graphics g = Graphics.FromImage(temp);
                    g.DrawImage(list[i], new Rectangle(0, 0, width, height), 0, 0, imgW, imgH, GraphicsUnit.Pixel);

                    eps = new EncoderParameters(1);
                    eps.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.FrameDimensionTime);

                    gif.SaveAdd(list[i], eps);
                }
            }

        }


        /// <summary> 
        /// 将源图片文件里每一帧的属性设置到新的图片对象里 
        /// </summary> 
        /// <param name="a">源图片帧</param> 
        /// <param name="b">新的图片帧</param> 
        private static void bindProperty(Image a, Image b)
        {

            //这个东西就是每一帧所拥有的属性，可以用GetPropertyItem方法取得这里用为完全复制原有属性所以直接赋值了 

            //顺便说一下这个属性里包含每帧间隔的秒数和透明背景调色板等设置，这里具体那个值对应那个属性大家自己在msdn搜索GetPropertyItem方法说明就有了 

            for (int i = 0; i < a.PropertyItems.Length; i++)
            {
                b.SetPropertyItem(a.PropertyItems[i]);
            }
        }



        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }










    }
}
