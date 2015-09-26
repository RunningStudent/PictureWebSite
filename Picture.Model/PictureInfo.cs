using System;
using System.Collections.Generic;
namespace Picture.Model
{
    public class PictureInfoModel
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
        public int UId { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public List<TagModel> Tags { get; set; }

    }
}

