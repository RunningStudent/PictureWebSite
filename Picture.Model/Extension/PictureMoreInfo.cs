using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Picture.Model
{
    public class PictureMoreInfoModel
    {  
        /// <summary>
        /// PId
        /// </summary>
        public int PId { get; set; }
        /// <summary>
        /// LargeImgPath
        /// </summary>
        public string LargeImgPath { get; set; }
        /// <summary>
        /// UploadDate
        /// </summary>
        public DateTime UploadDate { get; set; }
        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// ImgSummary
        /// </summary>
        public string ImgSummary { get; set; }
        /// <summary>
        /// CollectCount
        /// </summary>
        public int CollectCount { get; set; }
        /// <summary>
        /// UId
        /// </summary>
        public UserInfoModel UInfo { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public List<TagModel> Tags { get; set; }


    }
}
