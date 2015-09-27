using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Picture.Model;
using System.Data.SqlClient;
using Picture.Utility;

namespace Picture.DAL
{
    public class CommentMoreDAL
    {
        /*
        public IEnumerable<CommentMoreModel> QueryList(int index, int size, object wheres, string orderField, bool isDesc = true)
        {

        }*/



        /// <summary>
        /// 查询所有评论数据
        /// </summary>
        /// <param name="wheres"></param>
        /// <param name="orderField"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public IEnumerable<CommentMoreModel> QueryALL(object wheres, string orderField, bool isDesc=true)
        {
            //条件构造
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

            var sqlStr = new StringBuilder();
            //条件构造
            sqlStr.Append(@"select * from Comment as c left join UserInfo as u on c.UId=u.Uid");
            if (wheres != null)
            {
                sqlStr.Append(" where c." + whereBuilder.ToString());
            }
            //排序条件构造
            if (orderField!=null)
            {
                sqlStr.Append(" order by c." + orderField);
            }
            //是否降序
            if (isDesc)
            {
                sqlStr.Append(" desc");
            }

            using (var dataReader = SqlHelper.ExecuteReader(sqlStr.ToString(), parameters.ToArray()))
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        yield return SqlHelper.MapEntity<CommentMoreModel>(dataReader);
                    }
                }
            }
          


        }
    }
}
