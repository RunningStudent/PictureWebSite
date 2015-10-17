using Picture.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Picture.BLL
{
    public partial class TagImgRelationBLL
    {

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="wheres"></param>
        /// <param name="orderField"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public IEnumerable<TagImgRelationModel> QueryList(int index, int size, string wheres, string orderField, bool isDesc = true)
        {
            return _dao.QueryList(index, size, wheres, orderField, isDesc);
        }

    }
}
