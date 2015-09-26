using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Picture.Model;
using Picture.Utility;

namespace Picture.DAL
{
    public partial class PictureInfoDAL
    {
        #region 向数据库中添加一条记录 +int Insert(PictureInfoModel model)
        /// <summary>
        /// 向数据库中添加一条记录
        /// </summary>
        /// <param name="model">要添加的实体</param>
        /// <returns>插入数据的ID</returns>
        public int Insert(PictureInfoModel model)
        {
            #region SQL语句
            const string sql = @"
INSERT INTO [dbo].[PictureInfo] (
	[LargeImgPath]
	,[UploadDate]
	,[Width]
	,[Height]
	,[ImgSummary]
	,[CollectCount]
	,[UId]
)
VALUES (
	@LargeImgPath
	,@UploadDate
	,@Width
	,@Height
	,@ImgSummary
	,@CollectCount
	,@UId
);select @@IDENTITY";
            #endregion
            var res = SqlHelper.ExecuteScalar(sql,
                    new SqlParameter("@LargeImgPath", model.LargeImgPath),
                    new SqlParameter("@UploadDate", model.UploadDate),
                    new SqlParameter("@Width", model.Width),
                    new SqlParameter("@Height", model.Height),
                    new SqlParameter("@ImgSummary", model.ImgSummary),
                    new SqlParameter("@CollectCount", model.CollectCount),
                    new SqlParameter("@UId", model.UId)
                );
            return res == null ? 0 : Convert.ToInt32(res);
        }
        #endregion

        #region 删除一条记录 +int Delete(int pid)
        public int Delete(int pid)
        {
            const string sql = "DELETE FROM [dbo].[PictureInfo] WHERE [PId] = @PId";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@PId", pid));
        }
        #endregion

        #region 根据主键ID更新一条记录 +int Update(PictureInfoModel model)
        /// <summary>
        /// 根据主键ID更新一条记录
        /// </summary>
        /// <param name="model">更新后的实体</param>
        /// <returns>执行结果受影响行数</returns>
        public int Update(PictureInfoModel model)
        {
            #region SQL语句
            const string sql = @"
UPDATE [dbo].[PictureInfo]
SET 
	[LargeImgPath] = @LargeImgPath
	,[UploadDate] = @UploadDate
	,[Width] = @Width
	,[Height] = @Height
	,[ImgSummary] = @ImgSummary
	,[CollectCount] = @CollectCount
	,[UId] = @UId
WHERE [PId] = @PId";
            #endregion
            return SqlHelper.ExecuteNonQuery(sql,
                    new SqlParameter("@PId", model.PId),
                    new SqlParameter("@LargeImgPath", model.LargeImgPath),
                    new SqlParameter("@UploadDate", model.UploadDate),
                    new SqlParameter("@Width", model.Width),
                    new SqlParameter("@Height", model.Height),
                    new SqlParameter("@ImgSummary", model.ImgSummary),
                    new SqlParameter("@CollectCount", model.CollectCount),
                    new SqlParameter("@UId", model.UId)
                );
        }
        #endregion

        #region 分页查询一个集合 +IEnumerable<PictureInfoModel> QueryList(int index, int size, object wheres, string orderField, bool isDesc = true)
        /// <summary>
        /// 分页查询一个集合
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">页大小</param>
        /// <param name="wheres">条件匿名类</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="isDesc">是否降序排序</param>
        /// <returns>实体集合</returns>
        public IEnumerable<PictureInfoModel> QueryList(int index, int size, object wheres, string orderField, bool isDesc = true)
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
            var sql = SqlHelper.GenerateQuerySql("PictureInfo", new[] { "PId", "LargeImgPath", "UploadDate", "Width", "Height", "ImgSummary", "CollectCount", "UId" }, index, size, whereBuilder.ToString(), orderField, isDesc);
            using (var reader = SqlHelper.ExecuteReader(sql, parameters.ToArray()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        yield return SqlHelper.MapEntity<PictureInfoModel>(reader);
                    }
                }
            }
        }
        #endregion

        #region 查询单个模型实体 +PictureInfoModel QuerySingle(object wheres)
        /// <summary>
        /// 查询单个模型实体
        /// </summary>
        /// <param name="wheres">条件匿名类</param>
        /// <returns>实体</returns>
        public PictureInfoModel QuerySingle(object wheres)
        {
            var list = QueryList(1, 1, wheres, null);
            return list != null && list.Any() ? list.FirstOrDefault() : null;
        }
        #endregion

        #region 查询单个模型实体 +PictureInfoModel QuerySingle(int pid)
        /// <summary>
        /// 查询单个模型实体
        /// </summary>
        /// <param name="pid">key</param>
        /// <returns>实体</returns>
        public PictureInfoModel QuerySingle(int pid)
        {
            const string sql = "SELECT TOP 1 [PId], [LargeImgPath], [UploadDate], [Width], [Height], [ImgSummary], [CollectCount], [UId] FROM [dbo].[PictureInfo] WHERE [PId] = @PId";
            using (var reader = SqlHelper.ExecuteReader(sql, new SqlParameter("@PId", pid)))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return SqlHelper.MapEntity<PictureInfoModel>(reader);
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
            var sql = SqlHelper.GenerateQuerySql("PictureInfo", new[] { "COUNT(1)" }, whereBuilder.ToString(), string.Empty);
            var res = SqlHelper.ExecuteScalar(sql, parameters.ToArray());
            return res == null ? 0 : Convert.ToInt32(res);
        }
        #endregion
    }
}