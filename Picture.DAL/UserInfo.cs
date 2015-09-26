using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Picture.Model;
using Picture.Utility;

namespace Picture.DAL
{
    public partial class UserInfoDAL
    {
        #region 向数据库中添加一条记录 +int Insert(UserInfoModel model)
        /// <summary>
        /// 向数据库中添加一条记录
        /// </summary>
        /// <param name="model">要添加的实体</param>
        /// <returns>插入数据的ID</returns>
        public int Insert(UserInfoModel model)
        {
            #region SQL语句
            const string sql = @"
INSERT INTO [dbo].[UserInfo] (
	[Uid]
	,[UserStatus]
	,[UserName]
)  output inserted.Uid
VALUES (
	@Uid
	,@UserStatus
	,@UserName
);select @@IDENTITY";
            #endregion
            var res = SqlHelper.ExecuteScalar(sql,
                    new SqlParameter("@Uid", model.Uid),
                    new SqlParameter("@UserStatus", model.UserStatus),
                    new SqlParameter("@UserName", model.UserName)
                );
            return res == null ? 0 : Convert.ToInt32(res);
        }
        #endregion

        #region 删除一条记录 +int Delete(int uid)
        public int Delete(int uid)
        {
            const string sql = "DELETE FROM [dbo].[UserInfo] WHERE [Uid] = @Uid";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@Uid", uid));
        }
        #endregion

        #region 根据主键ID更新一条记录 +int Update(UserInfoModel model)
        /// <summary>
        /// 根据主键ID更新一条记录
        /// </summary>
        /// <param name="model">更新后的实体</param>
        /// <returns>执行结果受影响行数</returns>
        public int Update(UserInfoModel model)
        {
            #region SQL语句
            const string sql = @"
UPDATE [dbo].[UserInfo]
SET 
	[Uid] = @Uid
	,[UserStatus] = @UserStatus
	,[UserName] = @UserName
WHERE [Uid] = @Uid";
            #endregion
            return SqlHelper.ExecuteNonQuery(sql,
                    new SqlParameter("@Uid", model.Uid),
                    new SqlParameter("@UserStatus", model.UserStatus),
                    new SqlParameter("@UserName", model.UserName)
                );
        }
        #endregion

        #region 分页查询一个集合 +IEnumerable<UserInfoModel> QueryList(int index, int size, object wheres, string orderField, bool isDesc = true)
        /// <summary>
        /// 分页查询一个集合
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">页大小</param>
        /// <param name="wheres">条件匿名类</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="isDesc">是否降序排序</param>
        /// <returns>实体集合</returns>
        public IEnumerable<UserInfoModel> QueryList(int index, int size, object wheres, string orderField, bool isDesc = true)
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
            var sql = SqlHelper.GenerateQuerySql("UserInfo", new[] { "Uid", "UserStatus", "UserName" }, index, size, whereBuilder.ToString(), orderField, isDesc);
            using (var reader = SqlHelper.ExecuteReader(sql, parameters.ToArray()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        yield return SqlHelper.MapEntity<UserInfoModel>(reader);
                    }
                }
            }
        }
        #endregion

        #region 查询单个模型实体 +UserInfoModel QuerySingle(object wheres)
        /// <summary>
        /// 查询单个模型实体
        /// </summary>
        /// <param name="wheres">条件匿名类</param>
        /// <returns>实体</returns>
        public UserInfoModel QuerySingle(object wheres)
        {
            var list = QueryList(1, 1, wheres, null);
            return list != null && list.Any() ? list.FirstOrDefault() : null;
        }
        #endregion

        #region 查询单个模型实体 +UserInfoModel QuerySingle(int uid)
        /// <summary>
        /// 查询单个模型实体
        /// </summary>
        /// <param name="uid">key</param>
        /// <returns>实体</returns>
        public UserInfoModel QuerySingle(int uid)
        {
            const string sql = "SELECT TOP 1 [Uid], [UserStatus], [UserName] FROM [dbo].[UserInfo] WHERE [Uid] = @Uid";
            using (var reader = SqlHelper.ExecuteReader(sql, new SqlParameter("@Uid", uid)))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return SqlHelper.MapEntity<UserInfoModel>(reader);
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
            var sql = SqlHelper.GenerateQuerySql("UserInfo", new[] { "COUNT(1)" }, whereBuilder.ToString(), string.Empty);
            var res = SqlHelper.ExecuteScalar(sql, parameters.ToArray());
            return res == null ? 0 : Convert.ToInt32(res);
        }
        #endregion
    }
}