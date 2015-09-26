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
    public partial class Tag
    {
        public List<TagModel> GetTagsByImgId(int id)
        {
            /*
             select TagName from [dbo].TagImgRelation as TIR 
            inner join [dbo].Tag as T on TIR.TagId=T.TId
            where TIR.ImgId=42
             */
            const string sql = @"
            select TagName from [dbo].TagImgRelation as TIR 
            inner join [dbo].Tag as T on TIR.TagId=T.TId
            where TIR.ImgId=@Id
            ";
            SqlParameter para = new SqlParameter("@Id", id);
            SqlDataReader reader = SqlHelper.ExecuteReader(sql, CommandType.Text, para);
            List<TagModel> result = new List<TagModel>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result.Add(SqlHelper.MapEntity<TagModel>(reader));
                }
            }
            reader.Close();
            return result;
        }
    }
}
