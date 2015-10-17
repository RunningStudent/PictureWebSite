using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Index;
using System.IO;
using PictureWebSite.Model;

namespace PictureWebSite.handler
{

    /// <summary>
    /// SearchPreview 的摘要说明
    /// </summary>
    public class SearchPreview : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string searchKey = context.Request["wd"];

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
            ScoreDoc[] docs = collector.TopDocs(0,10).scoreDocs;

            //展示数据实体对象集合
            var tagModels = new List<Picture.Model.TagModel>();
            for (int i = 0; i < docs.Length; i++)
            {
                int docId = docs[i].doc;//得到查询结果文档的id（Lucene内部分配的id）
                Document doc = searcher.Doc(docId);//根据文档id来获得文档对象Document


                Picture.Model.TagModel tag = new Picture.Model.TagModel();
                //picture.ImgSummary = doc.Get("summary");
                tag.TagName= Picture.Utility.SearcherHelper.HightLight(searchKey, doc.Get("tag"));
                //book.ContentDescription = doc.Get("content");//未使用高亮
                //搜索关键字高亮显示 使用盘古提供高亮插件
                //book.ContentDescription = Picture.Utility.SplitContent.HightLight(Request.QueryString["SearchKey"], doc.Get("content"));
                tag.TId = Convert.ToInt32(doc.Get("id"));
                tagModels.Add(tag);
            }

            SearchPreviewResult result = new SearchPreviewResult()
            {
                q=searchKey,
                p=false
            };

            foreach (var item in tagModels)
	        {
                result.s.Add(item.TagName);
	        }

            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();

            context.Response.Write(jss.Serialize(result));

        }

        /// <summary>
        /// 从索引库中检索关键字
        /// </summary>
        private void SearchFromIndexData(HttpContext context)
        {

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