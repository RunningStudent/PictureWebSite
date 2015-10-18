using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Picture.Model;
using Picture.Utility;

namespace Picture.DAL
{
    public partial class ExcellentPictureDAL
    {
        #region 向数据库中添加一条记录 +int Insert(ExcellentPictureModel model)
        /// <summary>
        /// 向数据库中添加一条记录
        /// </summary>
        /// <param name="model">要添加的实体</param>
        /// <returns>插入数据的ID</returns>
        public int Insert(ExcellentPictureModel model)
        {
            #region SQL语句
            const string sql = @"
INSERT INTO [dbo].[ExcellentPicture] (
	[ePId]
	,[eAddDate]
)
VALUES (
	@ePId
	,@eAddDate
);select @@IDENTITY";
            #endregion
            var res = SqlHelper.ExecuteScalar(sql,
                    new SqlParameter("@ePId", model.ePId),
                    new SqlParameter("@eAddDate", model.eAddDate)
                );
            return res == null ? 0 : Convert.ToInt32(res);
        }
        #endregion

        #region 删除一条记录 +int Delete(int eid)
        public int Delete(int eid)
        {
            const string sql = "DELETE FROM [dbo].[ExcellentPicture] WHERE [eId] = @eId";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@eId", eid));
        }
        #endregion

        #region 根据主键ID更新一条记录 +int Update(ExcellentPictureModel model)
        /// <summary>
        /// 根据主键ID更新一条记录
        /// </summary>
        /// <param name="model">更新后的实体</param>
        /// <returns>执行结果受影响行数</returns>
        public int Update(ExcellentPictureModel model)
        {
            #region SQL语句
            const string sql = @"
UPDATE [dbo].[ExcellentPicture]
SET 
	[ePId] = @ePId
	,[eAddDate] = @eAddDate
WHERE [eId] = @eId";
            #endregion
            return SqlHelper.ExecuteNonQuery(sql,
                    new SqlParameter("@eId", model.eId),
                    new SqlParameter("@ePId", model.ePId),
                    new SqlParameter("@eAddDate", model.eAddDate)
                );
        }
        #endregion

        #region 分页查询一个集合 +IEnumerable<ExcellentPictureModel> QueryList(int index, int size, object wheres, string orderField, bool isDesc = true)
        /// <summary>
        /// 分页查询一个集合
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">页大小</param>
        /// <param name="wheres">条件匿名类</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="isDesc">是否降序排序</param>
        /// <returns>实体集合</returns>
        public IEnumerable<ExcellentPictureModel> QueryList(int index, int size, object wheres, string orderField, bool isDesc = true)
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
            var sql = SqlHelper.GenerateQuerySql("ExcellentPicture", new[] { "eId", "ePId", "eAddDate" }, index, size, whereBuilder.ToString(), orderField, isDesc);
            using (var reader = SqlHelper.ExecuteReader(sql, parameters.ToArray()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        yield return SqlHelper.MapEntity<ExcellentPictureModel>(reader);
                    }
                }
            }
        }
        #endregion

        #region 查询单个模型实体 +ExcellentPictureModel QuerySingle(object wheres)
        /// <summary>
        /// 查询单个模型实体
        /// </summary>
        /// <param name="wheres">条件匿名类</param>
        /// <returns>实体</returns>
        public ExcellentPictureModel QuerySingle(object wheres)
        {
            var list = QueryList(1, 1, wheres, null);
            return list != null && list.Any() ? list.FirstOrDefault() : null;
        }
        #endregion

        #region 查询单个模型实体 +ExcellentPictureModel QuerySingle(int eid)
        /// <summary>
        /// 查询单个模型实体
        /// </summary>
        /// <param name="eid">key</param>
        /// <returns>实体</returns>
        public ExcellentPictureModel QuerySingle(int eid)
        {
            const string sql = "SELECT TOP 1 [eId], [ePId], [eAddDate] FROM [dbo].[ExcellentPicture] WHERE [eId] = @eId";
            using (var reader = SqlHelper.ExecuteReader(sql, new SqlParameter("@eId", eid)))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return SqlHelper.MapEntity<ExcellentPictureModel>(reader);
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region 查询条数 +int QueryCount(object wheres)
        /// <summary>
        /// 查询条数
        /// </summary>
        /// <param name="wheres">查询条件</param>
        /// <returns>条数</returns>
        public int QueryCount(object wheres)
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
            var sql = SqlHelper.GenerateQuerySql("ExcellentPicture", new[] { "COUNT(1)" }, whereBuilder.ToString(), string.Empty);
            var res = SqlHelper.ExecuteScalar(sql, parameters.ToArray());
            return res == null ? 0 : Convert.ToInt32(res);
        }
        #endregion
    }
}