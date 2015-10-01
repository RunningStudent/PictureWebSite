using Picture.Model;
using Picture.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Picture.DAL
{
    public partial class PictureCollectDAL
    {
        /// <summary>
        /// 分页查询一个集合
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">页大小</param>
        /// <param name="wheres">条件匿名类</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="isDesc">是否降序排序</param>
        /// <returns>实体集合</returns>
        public IEnumerable<PictureCollectionMoreModel> GetCollectWithPictureInfo(int index, int size, object wheres, string orderField, bool isDesc = true)
        {
            var parameters = new List<SqlParameter>();
            var whereBuilder = new System.Text.StringBuilder();
            if (wheres != null)
            {
                var props = wheres.GetType().GetProperties();
                foreach (var prop in props)
                {
                    if (prop.Name.Equals("__o", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // 操作符
                        whereBuilder.AppendFormat(" {0} ", prop.GetValue(wheres, null).ToString());
                    }
                    else
                    {
                        var val = prop.GetValue(wheres, null).ToString();
                        whereBuilder.AppendFormat(" [{0}] = @{0} ", prop.Name);
                        parameters.Add(new SqlParameter("@" + prop.Name, val));
                    }
                }
            }

            string sql = null;

            if (index == 1)
            {
                sql = string.Format(@"select top {0} 
                                        pc.CId,
                                        pc.CollectDate,
                                        pc.PId,
                                        pc.Cuid,
                                        p.CollectCount,
                                        p.Height,
                                        p.Width,
                                        p.ImgSummary,
                                        p.LargeImgPath,
                                        p.[UId],
                                        p.UploadDate from 
                                        PictureCollect as pc join PictureInfo as P on pc.PId=p.PId 
                                        {1}
                                        {2}
                                        {3}
                                        ",
                                         size,
                                          wheres == null ? String.Empty : " where " + whereBuilder.ToString(),
                                          string.IsNullOrEmpty(orderField) ? string.Empty : "order by " + orderField,
                                          isDesc && !string.IsNullOrEmpty(orderField) ? "desc" : string.Empty);
            }
            else if (index < 0)
            {
                sql = string.Format(@"select 
                                    pc.CId,
                                    pc.CollectDate,
                                    pc.PId,
                                    pc.Cuid,
                                    p.CollectCount,
                                    p.Height,
                                    p.Width,
                                    p.ImgSummary,
                                    p.LargeImgPath,
                                    p.[UId],
                                    p.UploadDate from 
                                    PictureCollect as pc join PictureInfo as P on pc.PId=p.PId 
                                    {1}
                                    {2}
                                    {3}", wheres == null ? String.Empty : " where " + whereBuilder.ToString(),
                                     string.IsNullOrEmpty(orderField) ? string.Empty : "order by " + orderField,
                                     isDesc && !string.IsNullOrEmpty(orderField) ? "desc" : string.Empty);

            }
            else
            {
                if (string.IsNullOrEmpty(orderField))
                {
                    throw new ArgumentNullException("orderField");
                }

                sql = string.Format(@"select * from
                                    (select ROW_NUMBER() 
                                    over
                                    (order by {0} {1}) 
                                    as num,
                                    pc.CId,
                                    pc.CollectDate,
                                    pc.PId,
                                    pc.Cuid,
                                    p.CollectCount,
                                    p.Height,
                                    p.Width,
                                    p.ImgSummary,
                                    p.LargeImgPath,
                                    p.[UId],
                                    p.UploadDate
                                    from PictureCollect as pc join PictureInfo as P on pc.PId=p.PId {2} 
                                    ) tbl
                                    where tbl.num between ({3}-1)*{4}+1 and {3}*{4}",
                                    orderField,
                                    isDesc ? "desc" : string.Empty,
                                    wheres == null ? String.Empty : " where " + whereBuilder.ToString(),
                                    index,
                                    size);
            }



            using (var reader = SqlHelper.ExecuteReader(sql, parameters.ToArray()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        yield return SqlHelper.MapEntity<PictureCollectionMoreModel>(reader);
                    }
                }
            }
        }



    }
}
