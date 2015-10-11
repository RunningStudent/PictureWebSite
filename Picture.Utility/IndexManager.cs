using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System.Threading;
using System.IO;
using Lucene.Net.Analysis.PanGu;
using System.Web;

namespace Picture.Utility
{
    /// <summary>
    /// 当用户修改数据库的时候，更新索引
    /// </summary>
    public class IndexManager
    {
        public static readonly IndexManager tagIndex = new IndexManager();
        public static readonly string indexPath = HttpContext.Current.Server.MapPath("IndexData");
        private IndexManager()
        {

        }
        //请求队列 解决索引目录同时操作的并发问题
        private Queue<TagViewMode> tagQueue = new Queue<TagViewMode>();
        /// <summary>
        /// 新增Books表信息时 添加邢增索引请求至队列
        /// </summary>
        /// <param name="books"></param>
        public void Add(Picture.Model.TagModel tag)
        {
            TagViewMode bvm = new TagViewMode();
            bvm.Id = tag.TId;
            bvm.Tag = tag.TagName;
            bvm.IT = IndexType.Insert;
            tagQueue.Enqueue(bvm);
        }
        /// <summary>
        /// 删除Books表信息时 添加删除索引请求至队列
        /// </summary>
        /// <param name="bid"></param>
        public void Del(int bid)
        {
            TagViewMode bvm = new TagViewMode();
            bvm.Id = bid;
            bvm.IT = IndexType.Delete;
            tagQueue.Enqueue(bvm);
        }
        /// <summary>
        /// 修改Books表信息时 添加修改索引(实质上是先删除原有索引 再新增修改后索引)请求至队列
        /// </summary>
        /// <param name="picture"></param>
        public void Mod(Picture.Model.TagModel tag)
        {
            TagViewMode bvm = new TagViewMode();
            bvm.Id = tag.TId;
            bvm.Tag = tag.TagName;
            bvm.IT = IndexType.Modify;
            tagQueue.Enqueue(bvm);
        }


        /// <summary>
        /// 在应用程序启动的时候,调用这个函数
        /// </summary>
        public void StartNewThread()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(QueueToIndex));
        }

        //定义一个线程 将队列中的数据取出来 插入索引库中
        private void QueueToIndex(object para)
        {
            while (true)
            {
                if (tagQueue.Count > 0)
                {
                    CRUDIndex();
                }
                else
                {
                    Thread.Sleep(3000);
                }
            }
        }


        /// <summary>
        /// 更新索引库操作
        /// </summary>
        private void CRUDIndex()
        {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            bool isExist = IndexReader.IndexExists(directory);
            if (isExist)
            {
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);
            while (tagQueue.Count > 0)
            {
                Document document = new Document();
                TagViewMode picture = tagQueue.Dequeue();
                if (picture.IT == IndexType.Insert)
                {
                    document.Add(new Field("id", picture.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("tag", picture.Tag, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    
                    writer.AddDocument(document);
                }
                else if (picture.IT == IndexType.Delete)
                {
                    writer.DeleteDocuments(new Term("id", picture.Id.ToString()));
                }
                else if (picture.IT == IndexType.Modify)
                {
                    //先删除 再新增
                    writer.DeleteDocuments(new Term("id", picture.Id.ToString()));
                    document.Add(new Field("id", picture.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("tag", picture.Tag, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    writer.AddDocument(document);
                }
            }
            writer.Close();
            directory.Close();
        }


    }

    public class TagViewMode
    {
        public int Id
        {
            get;
            set;
        }
        public string Tag 
        {
            get;
            set;
        }
      
        public IndexType IT
        {
            get;
            set;
        }
    }
    //操作类型枚举
    public enum IndexType
    {
        Insert,
        Modify,
        Delete
    }
}
