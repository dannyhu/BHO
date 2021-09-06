using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.IO;

namespace BHO
{
    /// <summary>
    /// BizLog4Provider日志提供者
    /// </summary>
    public static class BizLog4Provider
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        static BizLog4Provider()
        {
            WebLoggerInit();
        }

        /// <summary>
        /// Web查询日志
        /// </summary>
        public static log4net.ILog WebLogger { get; private set; }

        /// <summary>
        /// Web查询日志
        /// </summary>
        private static void WebLoggerInit()
        {
            string logger_name = "reg_webApp";

            Hierarchy hierarchy = (Hierarchy)LogManager.CreateRepository(logger_name);

            PatternLayout patternLayout = new PatternLayout
            {
                ConversionPattern = "[%level][%d] %message%newline"
            };
            patternLayout.ActivateOptions();

            TraceAppender tracer = new TraceAppender();
            tracer.Layout = patternLayout;
            tracer.ActivateOptions();

            hierarchy.Root.AddAppender(tracer);

            RollingFileAppender roller = new RollingFileAppender
            {
                LockingModel = new log4net.Appender.FileAppender.MinimalLock(),                  //最小化锁定,支持并发写入
                File = MapPath("/Logs/"),                                                        //文件存储的文件夹位置
                StaticLogFileName = false,                                                       //是否使用静态文件名
                RollingStyle = RollingFileAppender.RollingMode.Date,                             //日志文件名
                DatePattern = "yyyyMMdd\".log\"",                                                //日期格式
                AppendToFile = true,                                                             //是否覆写到文件中
                Layout = patternLayout
            };
            roller.ActivateOptions();

            hierarchy.Root.AddAppender(roller);
            hierarchy.Root.Level = log4net.Core.Level.All;
            hierarchy.Configured = true;

            log4net.Config.XmlConfigurator.Configure(hierarchy, new FileInfo(logger_name));

            WebLogger = log4net.LogManager.GetLogger(hierarchy.Name, logger_name);
        }

        /// <summary>
        /// 获取指定虚拟路径在应用中映射的物理路径
        /// </summary>
        /// <param name="path">虚拟路径,例如:"~/bin"</param>
        /// <returns>返回在应用中映射的物理路径</returns>
        private static string MapPath(string path)
        {
            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }
    }
}
