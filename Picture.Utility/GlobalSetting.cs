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

        /*
        #region Unite,登入与登出的操作,会修改Session与Cookie
        /// <summary>
        /// 登入,修改Cookie与Session
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stayOnline"></param>
        public static void Login(User user, bool stayOnline = false)
        {
            //Session
            Logined = true;
            Uid = user.Uid;
            Authority = (UserAuthority)user.Authority;
            UserName = user.UserName;

            //Cookie
            StayOnline = stayOnline;
            if (stayOnline) CookieUid = user.Uid;
        }

        /// <summary>
        /// 登出,修改Cookie与Session
        /// </summary>
        public static void Logout()
        {
            Session.Clear();
            StayOnline = false;
            CookieUid = 0;
        }

        #endregion
         
         
         
         #region Cookie操作,这里只有对uid和stay_online的操作
        private const string KEY_UID = "uid";
        private const string KEY_STAY_ONLINE = "stay_online";
        public static int CookieUid
        {
            get
            {
                int result;
                return int.TryParse(UcUtility.AuthCodeDecode(Cookie.Get(KEY_UID)), out result) ? result : 0;
            }
            set { Cookie.Set(KEY_UID, UcUtility.AuthCodeEncode(value.ToString())); }
        }
        private static bool StayOnline
        {
            get
            {
                bool result;
                return bool.TryParse(Cookie.Get(KEY_STAY_ONLINE), out result) ? result : true;
            }
            set { Cookie.Set(KEY_STAY_ONLINE, value.ToString()); }
        }
        #endregion
         
         
         
         
         
           #region Session操作
        private const string UID = "uid";
        private const string USERNAME = "username";
        private const string AUTHORITY = "authority";
        private const string LOGINED = "logined";
        private const string AUTHOR = "authoer";
        private const string IPV6 = "ipv6";
        /// <summary>
        /// Session获得/设置用户角色
        /// </summary>
        public static UserAuthority Authority
        {
            get
            {
                return Session[AUTHORITY] == null ? UserAuthority.游客 : (UserAuthority)Session[AUTHORITY];
            }
            set
            {
                Session[AUTHORITY] = value;
            }
        }

        /// <summary>
        /// Session获得/设置UID
        /// </summary>
        public static int Uid
        {
            get
            {
                return Session[UID] == null ? 0 : (int)Session[UID];
            }
            set
            {
                Session[UID] = value;
            }
        }

        /// <summary>
        /// Session获得/设置用户名
        /// </summary>
        public static string UserName
        {
            get
            {
                return Session[USERNAME] == null ? "" : Session[USERNAME].ToString();
            }
            set { Session[USERNAME] = value; }
        }

        /// <summary>
        /// Session判断是否为管理员
        /// </summary>
        public static bool IsAdmin
        {
            get
            {
                if (Session[AUTHORITY] == null) return false;
                var authority = (UserAuthority)Session[AUTHORITY];
                if (authority != UserAuthority.超级管理员 && authority != UserAuthority.管理员) return false;
                return true;
            }
        }

        /// <summary>
        /// Session获得/设置登入状态
        /// </summary>
        public static bool Logined
        {
            get { return Session[LOGINED] == null ? false : (bool)Session[LOGINED]; }
            set { Session[LOGINED] = value; }
        }

        /// <summary>
        ///  Session获得/设置作者
        /// </summary>
        public static User Author
        {
            get { return (User)Session[AUTHOR]; }
            set { Session[AUTHOR] = value; }
        }

        /// <summary>
        /// Session判断是否通过IPV6登入
        /// </summary>
        public static bool Ipv6
        {
            get
            {
                if (Session[IPV6] == null)
                {
                    string pattern;
                    try
                    {
                        pattern = ConfigurationManager.AppSettings["IPV6Regex"];
                    }
                    catch
                    {
                        pattern = "";
                    }
                    var reg = new Regex(pattern);
                    Session[IPV6] = Request.Url != null && reg.Match(Request.Url.ToString()).Success;
                }
                return (bool)Session[IPV6];
            }
        }
        #endregion
         */


        
    }
}