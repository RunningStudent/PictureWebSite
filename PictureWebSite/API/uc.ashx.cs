using DS.Web.UCenter.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PictureWebSite.API
{
    /// <summary>
    /// uc 的摘要说明
    /// </summary>
    public class uc :UcApiBase
    {

       
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public override ApiReturn DeleteUser(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn GetCredit(int uid, int credit)
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.UcCreditSettingReturns GetCreditSettings()
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.UcTagReturns GetTag(string tagName)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn RenameUser(int uid, string oldUserName, string newUserName)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn SynLogin(int uid)
        {
            //throw new NotImplementedException();
            return ApiReturn.Failed;
        }

        public override ApiReturn SynLogout()
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateApps(DS.Web.UCenter.UcApps apps)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateBadWords(DS.Web.UCenter.UcBadWords badWords)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateClient(DS.Web.UCenter.UcClientSetting client)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateCredit(int uid, int credit, int amount)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateCreditSettings(DS.Web.UCenter.UcCreditSettings creditSettings)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateHosts(DS.Web.UCenter.UcHosts hosts)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdatePw(string userName, string passWord)
        {
            throw new NotImplementedException();
        }
    }
}