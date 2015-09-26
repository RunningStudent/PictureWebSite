using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PictureWebSite.TestCode
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Picture.BLL.TagImgRelationBLL bllTIR = new Picture.BLL.TagImgRelationBLL();
                string str = Context.Request["testBtn"];
                if (str == "666")
                {
                    int []flag=new int [51];

                    Random r = new Random();
                    int temp = 0;
                    for (int i = 0; i < 50;i++ )
                    {
                        temp = r.Next(1, 51);
                        while (flag[temp]>=3)
                        {
                            temp = r.Next(1, 51);
                        } 
                        flag[temp]++;
                        bllTIR.Insert(new Picture.Model.TagImgRelationModel()
                        {
                             ImgId=temp,
                             TagId=r.Next(1,6)
                        });
                    }

                }



            }
        }
    }
}