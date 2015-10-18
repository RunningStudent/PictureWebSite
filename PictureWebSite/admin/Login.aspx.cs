using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PictureWebSite.admin
{
    public partial class Login : System.Web.UI.Page
    {

        public string errorMessage { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string username = Request["username"];
                string password = Request["password"];
                if (username=="admin"&&password=="123456")
                {
                    Session["isLogin"] = true;
                    Response.Redirect("Index.aspx");
                }
                else
                {
                    errorMessage = "用户名密码错误";
                }
            }
            else
            {

            }
        }
    }
}