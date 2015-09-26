
using PictureWebSite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PictureWebSite.shared
{
    public partial class Layout : System.Web.UI.MasterPage
    {

        public bool isLogin { get; set; }
        public User UserInfo { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["current_user"]!=null)
            {
                isLogin = true;
                UserInfo = Session["current_user"] as User;
            }
        }
    }
}