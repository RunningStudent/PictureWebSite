using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PictureWebSite.Model
{
    /// <summary>
    /// 用于展示图片瀑布流用的Model
    /// </summary>
    public class Picture
    {
        /*
         *  id = i + 1,
                    imgUrl = @"/assets/img/pic/" + (i+1) + ".jpg",
                    title = "此处为图片" + (1 + i) + "的描述,很长很长很长很长很长很长很长很长很长很长很长很长很长很长很长很长,长得不得了的长得不得了的长得不得了的长得不得了的长得不得了的长得不得了的长得不得了的长得不得了的长得不得了的长得不得了的长得不得了的长得不得了的的描述",
                    userName = "我是用户名",
                    userFace = @"assets/img/face/face1.jpg",
                    uploadDate = new DateTime(2010, 8, 4),
                    label = new string[] { "标签1", "标签2", "标签3", "标签4" },
                    width = r.Next(600, 700),
                    height = r.Next(700, 900)
         * /
        */

        public int id { get; set; }
        public string imgUrl { get; set; }
        public string userName { get; set; }
        public string userFace { get; set; }
        public string title { get; set; }
        public DateTime uploadDate { get; set; }
        public List<string> label { get; set; }
        public int width { get; set; }
        public int height { get; set; }



    }
}