using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace TNet.Log
{
    internal class LogHelper
    {
        private static bool m_isinit;
        private static bool m_logInfoEnable;
        private static bool m_logErrorEnable;
        private static bool m_logWarnEnable;
        private static bool m_logComplementEnable;
        private static bool m_logDubugEnable;
        private static bool m_logFatalEnabled;
        private static NLog.Logger _logger;

        private static ConcurrentDictionary<string, NLog.Logger> m_customLoggers;

        static LogHelper()

        {
            m_customLoggers = new ConcurrentDictionary<string, Logger>();
            m_isinit = false;
            m_logInfoEnable = false;
            m_logErrorEnable = false;
            m_logWarnEnable = false;
            m_logComplementEnable = false;
            m_logDubugEnable = false;
            m_logFatalEnabled = false;
            _logger = LogManager.GetCurrentClassLogger();


            if (!m_isinit)
            {
                m_isinit = true;
                SetConfig();
            }

        }
        public static void SetConfig()
        {
            m_logInfoEnable = _logger.IsInfoEnabled;
            m_logErrorEnable = _logger.IsErrorEnabled;
            m_logWarnEnable = _logger.IsWarnEnabled;
            m_logComplementEnable = _logger.IsTraceEnabled;
            m_logFatalEnabled = _logger.IsFatalEnabled;
            m_logDubugEnable = _logger.IsDebugEnabled;
        }
        public static void WriteInfo(string info)
        {
            if (LogHelper.m_logInfoEnable)
            {
                LogHelper._logger.Info(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteDebug(string info)
        {
            if (LogHelper.m_logDubugEnable)
            {
                LogHelper._logger.Debug(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteError(string info)
        {
            if (LogHelper.m_logErrorEnable)
            {
                LogHelper._logger.Error(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteException(string info, Exception ex)
        {
            if (LogHelper.m_logErrorEnable)
            {
                LogHelper._logger.Error(LogHelper.BuildMessage(info, ex));
            }
        }

        public static void WriteWarn(string info)
        {
            if (LogHelper.m_logWarnEnable)
            {
                LogHelper._logger.Warn(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteWarn(string info, Exception ex)
        {
            if (LogHelper.m_logWarnEnable)
            {
                LogHelper._logger.Warn(LogHelper.BuildMessage(info, ex));
            }
        }
        public static void WriteFatal(string info)
        {
            if (LogHelper.m_logFatalEnabled)
            {
                LogHelper._logger.Fatal(LogHelper.BuildMessage(info));
            }
        }

        public static void WriteComplement(string info)
        {
            WriteTo("", info);
        }
        public static void WriteComplement(string info, Exception ex)
        {
            WriteTo("", info, ex);
        }

        public static void WriteTo(string name, string info, Exception ex = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "Complement";
            }
            var lazy = new Lazy<Logger>(() => LogManager.GetLogger(name));
            Logger customLog = m_customLoggers.GetOrAdd(name, lazy.Value);
            if (customLog != null)
            {
                customLog.Log(LogLevel.Trace, LogHelper.BuildMessage(info, ex));
            }
        }





        public static void WriteLine(string message)
        {
            WriteLine(LogLevel.Info, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public static void WriteLine(LogLevel level, string message)
        {
            _logger.Log(level, message);
        }




        private static string BuildMessage(string info)
        {
            return LogHelper.BuildMessage(info, null);
        }
        private static string BuildMessage(string info, Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
          
            try
            {
                stringBuilder.AppendFormat("Time:{0}-{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), info);
               
                if (ex != null)
                {
                    stringBuilder.AppendFormat("Exception:{0}\r\n", ex.ToString());
                }
            }
            catch (Exception error)
            {
                stringBuilder.AppendLine(info + ", Exception:\r\n" + error);
            }
            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }




    }
}
