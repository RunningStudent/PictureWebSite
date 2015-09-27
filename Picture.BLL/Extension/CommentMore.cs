using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Picture.DAL;
using Picture.Model;

namespace Picture.BLL
{
    public class CommentMore
    {
        private CommentMoreDAL commentDal = new CommentMoreDAL();

        /// <summary>
        /// 查询所有评论数据
        /// </summary>
        /// <param name="wheres"></param>
        /// <param name="orderField"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public IEnumerable<CommentMoreModel> GetCommentWithUserInfo(object wheres, string orderField, bool isDesc=true)
        {
            return commentDal.QueryALL(wheres, orderField, isDesc);
        } 

    }
}
