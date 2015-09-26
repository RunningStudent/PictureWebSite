using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Picture.Utility
{
    public class JSONHelper
    {
        public static string ToJSONString(object obj)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            return jss.Serialize(obj);
        }
        public static T JSON2Obj<T>(string json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            return jss.Deserialize<T>(json);
        }
    }
}
