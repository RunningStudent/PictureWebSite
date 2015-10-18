using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;


namespace Picture.Utility
{
    /// <summary>
    /// 获取配置文件,登入登出的操作,对一些Cookie与Session的操作
    /// </summary>
    public static class GlobalSetting
    {
        private static HttpRequestBase Request
        {
            get { return new HttpContextWrapper(HttpContext.Current).Request; }
        }
        private static HttpResponseBase Response
        {
            get { return new HttpContextWrapper(HttpContext.Current).Response; }
        }
        private static HttpSessionStateBase Session
        {
            get { return new HttpContextWrapper(HttpContext.Current).Session; }
        }


        private const string PICTRUESIZE = "PictureSize";
        private static int _pictureSize;

        public static int PictureSize
        {
            get
            {
                if (_pictureSize == 0)
                {
                    _pictureSize = getAppSetting(PICTRUESIZE, 0);
                }
                return _pictureSize;
            }
        }


        private static string getAppSetting(string key, string defaultValue = "")
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch
            {
                return defaultValue;
            }
        }
        private static int getAppSetting(string key, int defaultValue)
        {
            try
            {
                return int.Parse(ConfigurationManager.AppSettings[key]);
            }
            catch
            {
                return defaultValue;
            }
        }





    }
}