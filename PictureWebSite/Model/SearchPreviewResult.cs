using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PictureWebSite.Model
{
    public class SearchPreviewResult
    {
        public string q { get; set; }
        public bool p { get; set; }
        public List<string> s = new List<string>();
    }
}