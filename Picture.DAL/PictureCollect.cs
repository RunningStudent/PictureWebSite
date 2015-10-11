using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Picture.Model;
using Picture.Utility;

namespace Picture.DAL
{
    public partial class PictureCollectDAL
    {
        #region 向数据库中添加一条记录 +int Insert(PictureCollectModel model)
        /// <summary>
        /// 向数据库中添加一条记录
        /// </summary>
        /// <param name="model">要添加的实体</param>
        /// <returns>插入数据的ID</returns>
        public int Insert(PictureCollectModel model)
        {
            #region SQL语句
            const string sql = @"
INSERT INTO [dbo].[PictureCollect] (
	[PId]
	,[CuId]
	,[CollectDate]
)
VALUES (
	@PId
	,@CuId
	,@CollectDate
);select @@IDENTITY";
            #endregion
            var res = SqlHelper.ExecuteScalar(sql,
                    new SqlParameter("@PId", model.PId),
                    new SqlParameter("@CuId", model.CuId),
                    new SqlParameter("@CollectDate", model.CollectDate)
                );
            return res == null ? 0 : Convert.ToInt32(res);
        }
        #endregion

        #region 删除一条记录 +int Delete(int cid)
        public int Delete(int cid)
        {
            const string sql = "DELETE FROM [dbo].[PictureCollect] WHERE [CId] = @CId";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@CId", cid));
        }
        #endregion

        #region 根据主键ID更新一条记录 +int Update(PictureCollectModel model)
        /// <summary>
        /// 根据主键ID更新一条记录
        /// </summary>
        /// <param name="model">更新后的实体</param>
        /// <returns>执行结果受影响行数</returns>
        public int Update(PictureCollectModel model)
        {
            #region SQL语句
            const string sql = @"
UPDATE [dbo].[PictureCollect]
SET 
	[PId] = @PId
	,[CuId] = @CuId
	,[CollectDate] = @CollectDate
WHERE [CId] = @CId";
            #endregion
            return SqlHelper.ExecuteNonQuery(sql,
                    new SqlParameter("@CId", model.CId),
                    new SqlParameter("@PId", model.PId),
                    new SqlParameter("@CuId", model.CuId),
                    new SqlParameter("@CollectDate", model.CollectDate)
                );
        }
        #endregion

        #region 分页查询一个集合 +IEnumerable<PictureCollectModel> QueryList(int index, int size, object wheres, string orderField, bool isDesc = true)
        /// <summary>
        /// 分页查询一个集合
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">页大小</param>
        /// <param name="wheres">条件匿名类</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="isDesc">是否降序排序</param>
        /// <returns>实体集合</returns>
        public IEnumerable<PictureCollectModel> QueryList(int index, int size, object wheres, string orderField, bool isDesc = true)
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
            var sql = SqlHelper.GenerateQuerySql("PictureCollect", new[] { "CId", "PId", "CuId", "CollectDate" }, index, size, whereBuilder.ToString(), orderField, isDesc);
            using (var reader = SqlHelper.ExecuteReader(sql, parameters.ToArray()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        yield return SqlHelper.MapEntity<PictureCollectModel>(reader);
                    }
                }
            }
        }
        #endregion

        #region 查询单个模型实体 +PictureCollectModel QuerySingle(object wheres)
        /// <summary>
        /// 查询单个模型实体
        /// </summary>
        /// <param name="wheres">条件匿名类</param>
        /// <returns>实体</returns>
        public PictureCollectModel QuerySingle(object wheres)
        {
            var list = QueryList(1, 1, wheres, null);
            return list != null && list.Any() ? list.FirstOrDefault() : null;
        }
        #endregion

        #region 查询单个模型实体 +PictureCollectModel QuerySingle(int cid)
        /// <summary>
        /// 查询单个模型实体
        /// </summary>
        /// <param name="cid">key</param>
        /// <returns>实体</returns>
        public PictureCollectModel QuerySingle(int cid)
        {
            const string sql = "SELECT TOP 1 [CId], [PId], [CuId], [CollectDate] FROM [dbo].[PictureCollect] WHERE [CId] = @CId";
            using (var reader = SqlHelper.ExecuteReader(sql, new SqlParameter("@CId", cid)))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return SqlHelper.MapEntity<PictureCollectModel>(reader);
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
            var sql = SqlHelper.GenerateQuerySql("PictureCollect", new[] { "COUNT(1)" }, whereBuilder.ToString(), string.Empty);
            var res = SqlHelper.ExecuteScalar(sql, parameters.ToArray());
            return res == null ? 0 : Convert.ToInt32(res);
        }
        #endregion
    }
}