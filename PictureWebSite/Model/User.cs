using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PictureWebSite.Model
{
    public class User
    {
        public int UId { get; set; }
        public string EMail { get; set; }
        public string UserName { get; set; }
        public string UserFacePathLarge { get; set; }
        public string UserFacePathSmall { get; set; }
        public string UserFaceMiddle { get; set; }
        public int UserStatus { get; set; }
    }
}