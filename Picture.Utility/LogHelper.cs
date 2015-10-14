using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Picture.Utility
{
    public class LogHelper
    {
        private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");
        public static readonly LogHelper logHelper = new LogHelper();//单例模式

        private LogHelper() 
        { 
            log4net.Config.XmlConfigurator.Configure();//初始化Log4Net
        }

        /// <summary>
        /// 异常队列
        /// </summary>
        private Queue<Exception> errors = new Queue<Exception>();


        /// <summary>
        /// 添加异常到异常队列
        /// </summary>
        /// <param name="e"></param>
        public void AddException(Exception e)
        {
            errors.Enqueue(e);
        }

        /// <summary>
        /// 处理队列中的异常
        /// </summary>
        private void ProcessLogs(object uselessParam)
        {
            while (true)
            {
                if (errors.Count>0)
                {
                    Exception e=errors.Dequeue();
                    logerror.Error(e);
                }
                else
                {
                    Thread.Sleep(3000);
                }
            }
        }

        /// <summary>
        /// 在应用程序启动的时候,调用这个函数
        /// </summary>
        public void StartNewThread()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessLogs));
        }



    }
}
