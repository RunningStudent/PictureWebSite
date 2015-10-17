using Picture.Model;
using Picture.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Picture.DAL
{
    public partial class TagImgRelationDAL
    {
        public IEnumerable<TagImgRelationModel> QueryList(int index, int size, string wheres, string orderField, bool isDesc = true)
        {
            string sql = null;
            if (index == 1)
            {
                sql = string.Format(@"select top {0} 
                                    RId,TagId,ImgId
                                    from 
                                    TagImgRelation as ti join PictureInfo as p on ti.ImgId=p.PId
                                    {1} {2} {3}", size,
                                                wheres == null ? String.Empty : wheres,
                                                string.IsNullOrEmpty(orderField) ? string.Empty : " order by " + orderField,
                                                isDesc && !string.IsNullOrEmpty(orderField) ? " desc " : string.Empty);
            }
            else if (index < 0)
            {
                sql = string.Format(@"select 
                                    RId,TagId,ImgId 
                                    from 
                                    TagImgRelation as ti join PictureInfo as p on ti.ImgId=p.PId
                                    {1}
                                    {2}
                                    {3}", wheres == null ? String.Empty : wheres,
                                          string.IsNullOrEmpty(orderField) ? string.Empty : " order by " + orderField,
                                          isDesc && !string.IsNullOrEmpty(orderField) ? " desc " : string.Empty);

            }
            else
            {
                if (string.IsNullOrEmpty(orderField))
                {
                    throw new ArgumentNullException("没有orderField");
                }

                sql = string.Format(@"select * from
                                    (select ROW_NUMBER() 
                                    over
                                    (order by {0} {1}) 
                                    as num,
                                    RId,TagId,ImgId
                                    from 
                                    TagImgRelation as ti join PictureInfo as p on ti.ImgId=p.PId 
                                    {2} 
                                    ) tbl
                                    where tbl.num between ({3}-1)*{4}+1 and {3}*{4}",
                                    orderField,
                                    isDesc ? "desc" : string.Empty,
                                    wheres == null ? String.Empty : wheres,
                                    index,
                                    size);
            }



            using (var reader = SqlHelper.ExecuteReader(sql))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        yield return SqlHelper.MapEntity<TagImgRelationModel>(reader);
                    }
                }
            }


        }
    }
}
