using DS.Web.UCenter.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DS.Web.UCenter;
using System.Web.SessionState;
using PictureWebSite.Model;
using Picture.Utility;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Search;
using System.IO;
using Lucene.Net.Documents;
using System.Text;


namespace PictureWebSite.handler
{
    /// <summary>
    /// GetPictureListAsy 的摘要说明
    /// </summary>
    public class GetPictureListAsy : IHttpHandler, IRequiresSessionState
    {
        private IUcClient client = new UcClient();
        Picture.BLL.PictureMoreInfoBLL pictureInfoBll = new Picture.BLL.PictureMoreInfoBLL();


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            int ciShu = int.Parse(context.Request["ciShu"] ?? "0");
            string seacherKey = context.Request["seacherKey"];

            List<Model.Picture> list = null;
            if (string.IsNullOrEmpty(seacherKey))
            {
                //如果搜索词为空,普通的加载图片
                list = GetAllPicture(context, ciShu, 20);
            }
            else
            {
                //搜索图片模式
                list = SearchPicture(context, ciShu, seacherKey, 20);
            }

            context.Response.Write(JSONHelper.ToJSONString(list));
        }

        /// <summary>
        /// 根据请求次数分页查询图片数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ciShu"></param>
        /// <returns></returns>
        private List<Model.Picture> GetAllPicture(HttpContext context, int ciShu, int size)
        {
            //取数据
            //var models = bllPictureInfo.QueryList(ciShu + 1, 20,new {}, "UploadDate");
            User user = context.Session["current_user"] as User;
            var models = pictureInfoBll.GetPictureInfoWithTagAndUserInfo(ciShu + 1, size, "UploadDate", "", true, user != null ? user.UId : -1); //pictureInfoBll.GetPictureInfoWithTagAndUserInfo(ciShu + 1, 20, null, "UploadDate");

            List<Model.Picture> list = BuildResult(models);
            return list;
        }


        private List<Model.Picture> SearchPicture(HttpContext context, int ciShu, string searchKey, int size)
        {
           
            User user = context.Session["current_user"] as User;

            //获得标签id
            List<int> tids = GetTagIds(context, searchKey);
            List<int> pids=GetPictureIds(ciShu,size,tids,searchKey);

            //if (tids.Count<1)
            //{
            //    string where="";
            //    //开始模糊查询
            //     var models = pictureInfoBll.GetPictureInfoWithTagAndUserInfo(ciShu + 1, size, "UploadDate", where, true, user != null ? user.UId : -1);

            //}
            
            StringBuilder whereBuilder = new StringBuilder(" where ") ;
            if (tids.Count>0)
            {
                foreach (var item in pids)
                {
                    whereBuilder.Append(" pid=" + item + " or ");
                }
                string temp = whereBuilder.ToString();
                whereBuilder.Clear().Append(temp.Substring(0, temp.LastIndexOf("or")));
            }
            else
            {
                whereBuilder.Append(" pid=-1");
            }
            string where = whereBuilder.ToString();
            var models = pictureInfoBll.GetPictureInfoWithTagAndUserInfo(ciShu + 1, size, "UploadDate", where, true, user != null ? user.UId : -1);
            return BuildResult(models);
        }


        /// <summary>
        /// 从索引库中找到要找的数据id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="searchKey"></param>
        /// <param name="ciShu"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private List<int> GetTagIds(HttpContext context, string searchKey)
        {
            string indexPath = context.Server.MapPath("../IndexData");
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            //搜索条件
            PhraseQuery query = new PhraseQuery();
            //把用户输入的关键字进行分词
            foreach (string word in Picture.Utility.SearcherHelper.SplitWords(searchKey))
            {
                query.Add(new Term("tag", word));
            }
            //query.Add(new Term("content", "C#"));//多个查询条件时 为且的关系
            query.SetSlop(100); //指定关键词相隔最大距离

            //TopScoreDocCollector盛放查询结果的容器
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);
            searcher.Search(query, null, collector);//根据query查询条件进行查询，查询结果放入collector容器
            //TopDocs 指定0到GetTotalHits() 即所有查询结果中的文档 如果TopDocs(20,10)则意味着获取第20-30之间文档内容 达到分页的效果


            ScoreDoc[] docs = collector.TopDocs(0, collector.GetTotalHits()).scoreDocs;

            List<int> results = new List<int>();

            for (int i = 0; i < docs.Length; i++)
            {
                int docId = docs[i].doc;//得到查询结果文档的id（Lucene内部分配的id）
                Document doc = searcher.Doc(docId);//根据文档id来获得文档对象Document
                results.Add(int.Parse(doc.Get("id")));
            }

            return results;
        }


        /// <summary>
        /// 根据标签id获得图片id
        /// </summary>
        /// <param name="ciShu"></param>
        /// <param name="size"></param>
        /// <param name="tids"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        private List<int> GetPictureIds(int ciShu, int size, List<int> tids, string searchKey)
        {
            Picture.BLL.TagImgRelationBLL tIrbll = new Picture.BLL.TagImgRelationBLL();
            List<int> resultList = new List<int>();

            if (tids.Count > 0)
            {
                //用标签id查图片数据

                //构建where条件
                StringBuilder sb = new StringBuilder(" where ");
                foreach (var item in tids)
                {
                    sb.Append(" TagId=" + item + " or ");
                }
                string result = sb.ToString();
                result = result.Substring(0, result.LastIndexOf("or"));


                //获得tagImgRelation对象
                var tagImgRList = tIrbll.QueryList(ciShu + 1, size, result, "UploadDate", true);

                //获得图片id同时去重复
                foreach (var item in tagImgRList)
                {
                    if (!resultList.Contains(item.ImgId))
                    {
                        resultList.Add(item.ImgId);
                    }
                }
            }

            return resultList;


        }

       



        /// <summary>
        /// 构建返回数据
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private List<Model.Picture> BuildResult(List<Picture.Model.PictureMoreInfoModel> models)
        {
            //构建要发送到前端的数据
            List<Model.Picture> list = new List<Model.Picture>();
            foreach (var item in models)
            {
                list.Add(new Model.Picture()
                {
                    id = item.PId,
                    imgUrl = CommonHelper.GetSmallImgPath(item.LargeImgPath),
                    userName = item.UserName,
                    userFace = client.AvatarUrl(item.UId, AvatarSize.Small),
                    label = item.Tags.Select(t => t.TagName).ToList(),
                    width = item.Width,
                    height = item.Height,
                    uploadDate = item.UploadDate,
                    title = item.ImgSummary,
                    isCollect = item.isCollect > 0 ? 1 : 0,
                    isGif = item.LargeImgPath.Contains(".gif")
                });
            }
            return list;
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}