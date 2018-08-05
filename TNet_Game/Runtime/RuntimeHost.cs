using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using TNet.Log;
using TNet.Script;

namespace TNet.Runtime
{
    /// <summary>
    /// Runtime host service
    /// </summary>
    public class RuntimeHost
    {

        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        /// <summary>
     
        private ZoneSetting _setting;

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnInit()
        {
            try
            {
                _setting = new ZoneSetting();
                var osbit = GetOsBit();
                var platform = GetRunPlatform();
                TraceLog.WriteLine(string.Format(GameZone.char_d,
                    Assembly.GetExecutingAssembly().GetName().Version,
                    osbit,
                    platform,
                    _setting.ProductCode,
                    _setting.ProductServerId));
            }
            catch
            {
            }
        }

        /// <summary>
        /// Process start logic init
        /// </summary>
        /// <returns></returns>
        public virtual bool OnStart()
        {
            try
            {
                GameZone.Start(_setting);
                return true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteLine("{0} Server failed to start error:{1}", DateTime.Now.ToString("HH:mm:ss"), ex.Message);
                TraceLog.WriteError("OnInit error:{0}", ex);
                TraceLog.WriteLine("# Server exit command \"Ctrl+C\" or \"Ctrl+Break\".");
            }
            return false;
        }

        private int GetOsBit()
        {
            try
            {
                return Environment.Is64BitProcess ? 64 : 32;
            }
            catch (Exception)
            {
                return 32;
            }
        }
        private string GetRunPlatform()
        {
            try
            {
                return Environment.OSVersion.Platform.ToString();
            }
            catch (Exception)
            {
                return "Unknow";
            }
        }
        /// <summary>
        /// Run
        /// </summary>
        public void Run()
        {
            try
            {
                RunAsync().Wait();
            }
            finally
            {
                runCompleteEvent.Set();
            }

        }

        /// <summary>
        /// Proccess stop logic
        /// </summary>
        public virtual void OnStop()
        {
            try
            {
                TraceLog.WriteLine("{0} Server is stopping, please wait.", DateTime.Now.ToString("HH:mm:ss"));
                ScriptEngines.StopMainProgram();
                GameZone.WaitStop().Wait();
                TraceLog.WriteLine("{0} Server has stoped successfully!", DateTime.Now.ToString("HH:mm:ss"));
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnStop error:{0}", ex);
            }
        }

        /// <summary>
        /// Set stop
        /// </summary>
        public void Stop()
        {
            GameZone.IsCanceled = true;
            WaitRunComplated();
            OnStop();
        }

        /// <summary>
        /// 
        /// </summary>
        public void WaitRunComplated()
        {
            runCompleteEvent.WaitOne();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual async System.Threading.Tasks.Task RunAsync()
        {
            try
            {
                if (ScriptEngines.RunMainProgram())
                {
                    TraceLog.WriteLine("{0} Server has started successfully!", DateTime.Now.ToString("HH:mm:ss"));
                    TraceLog.WriteLine("# Server is listening...");
                }
                else
                {
                    TraceLog.WriteLine("{0} Server failed to start!", DateTime.Now.ToString("HH:mm:ss"));
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteLine("{0} Server failed to start error:{1}", DateTime.Now.ToString("HH:mm:ss"), ex.Message);
                TraceLog.WriteError("RunMain error:{0}", ex);
            }
            finally
            {
                TraceLog.WriteLine("# Server exit command \"Ctrl+C\" or \"Ctrl+Break\".");
            }

            await RunWait();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected async System.Threading.Tasks.Task RunWait()
        {
            while (!GameZone.IsCanceled)
            {
                await System.Threading.Tasks.Task.Delay(1000);
            }
        }
    }
}
