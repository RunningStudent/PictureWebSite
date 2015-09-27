using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Picture.Model
{
    /// <summary>
    /// 带用户信息的评论实体类
    /// </summary>
    public class CommentMoreModel
    {
        /// <summary>
        /// CId
        /// </summary>
        public int CId { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// UId
        /// </summary>
        public int UId { get; set; }
        /// <summary>
        /// PId
        /// </summary>
        public int PId { get; set; }
        /// <summary>
        /// PostDate
        /// </summary>
        public DateTime PostDate { get; set; }

        //用户信息
        public int Status { get; set; }
        public String UserName { get; set; }



    }
}
