using Picture.Model;
using Picture.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Picture.DAL
{
    public partial class PictureMoreInfoDAL
    {
     

        /// <summary>
        /// 分页查询一个集合
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">页大小</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="isDesc">是否降序排序</param>
        /// <param name="currentUId">当前用户的id,如果这个用户未登入填-1</param>
        /// <returns>实体集合</returns>
        public IEnumerable<PictureMoreInfoModel> QueryList(int index, int size, string orderField, bool isDesc = true, int currentUId = -1)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@index",index),
                new SqlParameter("@size",size),
                new SqlParameter("@orderby",orderField),
                new SqlParameter("@isDesc",isDesc?"1":"0"),
                new SqlParameter("@currentUId",currentUId)
            };

            var sql = "usp_GetDetailPicture";

            using (var reader = SqlHelper.ExecuteReader(sql, CommandType.StoredProcedure, parameters.ToArray()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        yield return SqlHelper.MapEntity<PictureMoreInfoModel>(reader);
                    }
                    reader.Close();
                }
                else
                {
                    reader.Close();
                }
            }
        }
    }
}
