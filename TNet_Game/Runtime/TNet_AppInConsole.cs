using System;
using System.Collections.Generic;
using System.Text;
using TNet.Log;

namespace TNet.Runtime
{
    /// <summary>
    /// 控制台启动服务器 ECS框架 APP
    /// </summary>
  public  class TNServer_AppInConsole:TNServer_App
    {

        public bool IsStoped { get; private set; }

        public TNServer_AppInConsole()
        {
            Console.CancelKeyPress += Console_CancelKeyPress; ;
        }
        protected override void ApplicationStarted()
        {
         
            base.ApplicationStarted();

            Start();
           
        }
        protected override void ApplicationStarting()
        {
          
            base.ApplicationStarting();
        }
        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            try
            {
                IsStoped = true;
                Stop();
                TraceLog.WriteLine("{0} Server has canceled!", DateTime.Now.ToString("HH:mm:ss"));
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnCancelKeyPress error:{1}", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
           
            OnInit();
         
            if (!OnStart())
            {
                RunWait().Wait();
                return;
            }
            Run();
            if (!IsStoped)
            {
                OnStop();
            }
        }
    }
}
