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
        /// 修改表信息时 添加修改索引(实质上是先删除原有索引 再新增修改后索引)请求至队列
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
                    IndexReader reader = IndexReader.Open(directory, true);
                    Term indexTerm = new Term("id", picture.Tag);
                    TermDocs docs = reader.TermDocs(indexTerm);
                    //查看是否有这条索引
                    if (docs.Next())
                    {
                        document.Add(new Field("id", picture.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("tag", picture.Tag, Field.Store.YES, Field.Index.ANALYZED,
                                               Field.TermVector.WITH_POSITIONS_OFFSETS));
                        writer.AddDocument(document);
                    }
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


        /// <summary>
        /// 创建索引
        /// </summary>
        public static void CreateIndexByData(IEnumerable<Picture.Model.TagModel> tagModels)
        {
            //删除原来的索引
            if (System.IO.Directory.Exists(indexPath))
            {
                System.IO.Directory.Delete(indexPath, true);

            }

            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            //IndexReader:对索引库进行读取的类
            bool isExist = IndexReader.IndexExists(directory); //是否存在索引库文件夹以及索引库特征文件
            if (isExist)
            {
                //如果索引目录被锁定（比如索引过程中程序异常退出或另一进程在操作索引库），则解锁
                //Q:存在问题 如果一个用户正在对索引库写操作 此时是上锁的 而另一个用户过来操作时 将锁解开了 于是产生冲突 --解决方法后续
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }

            //创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)
            //补充:使用IndexWriter打开directory时会自动对索引库文件上锁
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);


            //--------------------------------遍历数据源 将数据转换成为文档对象 存入索引库
            foreach (var tag in tagModels)
            {
                Document document = new Document(); //new一篇文档对象 --一条记录对应索引库中的一个文档

                //向文档中添加字段  Add(字段,值,是否保存字段原始值,是否针对该列创建索引)
                document.Add(new Field("id", tag.TId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));//--所有字段的值都将以字符串类型保存 因为索引库只存储字符串类型数据

                //Field.Store:表示是否保存字段原值。指定Field.Store.YES的字段在检索时才能用document.Get取出原值  //Field.Index.NOT_ANALYZED:指定不按照分词后的结果保存--是否按分词后结果保存取决于是否对该列内容进行模糊查询


                document.Add(new Field("tag", tag.TagName, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

                //Field.Index.ANALYZED:指定文章内容按照分词后结果保存 否则无法实现后续的模糊查询 
                //WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词 还保存词之间的距离

                //document.Add(new Field("content", book.ContentDescription, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                writer.AddDocument(document); //文档写入索引库
            }
            writer.Close();//会自动解锁
            directory.Close(); //不要忘了Close，否则索引结果搜不到
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
