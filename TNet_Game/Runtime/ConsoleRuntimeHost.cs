using System;
using System.Collections.Generic;
using System.Text;
using TNet.Log;

namespace TNet.Runtime
{
    /// <summary>
    /// Console runtime host
    /// </summary>
    public class ConsoleRuntimeHost : RuntimeHost
    {
        private bool isStop;

        /// <summary>
        /// 
        /// </summary>
        public bool IsStoped { get { return isStop; } }

        /// <summary>
        /// init
        /// </summary>
        public ConsoleRuntimeHost()
        {
            Console.CancelKeyPress += OnCancelKeyPress;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            SetColor(ConsoleColor.DarkYellow);
            OnInit();
            SetColor(currentForeColor);
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

        private void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            try
            {
                isStop = true;
                Stop();
                TraceLog.WriteLine("{0} Server has canceled!", DateTime.Now.ToString("HH:mm:ss"));
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnCancelKeyPress error:{1}", ex);
            }
        }

        private void SetColor(ConsoleColor color)
        {
            try
            {
                Console.ForegroundColor = color;
            }
            catch { }
        }
    }
}
