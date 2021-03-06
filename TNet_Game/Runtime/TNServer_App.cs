﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using EcsRx.Infrastructure.Dependencies;
using Microsoft.AspNetCore.Hosting;
using TNet.Cache;
using TNet.Cache.Generic;
using TNet.Log;
using TNet.Script;

namespace TNet.Runtime
{
   
    /// <summary>
    /// TNet
    /// </summary>
    public class TNServer_App : EcsRx.Infrastructure.EcsRxApplication
    {

        [Obsolete]
        protected override IDependencyContainer DependencyContainer => throw new NotImplementedException();


        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        /// <summary>
        /// 数据化游戏场景
        /// </summary>
        public EcsRx.Entities.IEntity Scene { get;private set; }
      
        private EcsRx.Collections.IEntityCollection cs;
        /// <summary>
        /// 全局变量集合
        /// </summary>
        public  ContextCacheSet<CacheItem> Global
        {
            get;
            private set;
        }

        /// <summary>
        /// 单例
        /// </summary>
        public static TNServer_App Instance { get; private set; }
        System.Runtime.Caching.ObjectCache obj = System.Runtime.Caching.MemoryCache.Default;
        protected override void ApplicationStarted()
        {
         

        }
        protected override void ApplicationStarting()
        {
            base.ApplicationStarting();
            Instance = this;
               cs= base.EntityCollectionManager.GetCollection();
            Scene=    cs.CreateEntity();


            //    var name=  Scene.AddComponent<Coms.NameShowed>();

            //    Scene.AddComponent<Coms.TNBehaviourTest>();
            //    name.Name = "名将三国的游戏场景数据实现";
            // Scene.AddChildren(cs.CreateEntity());
            //    //  cs.CreateEntity();

            //    System.Runtime.Caching.CacheItemPolicy ca = new System.Runtime.Caching.CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddSeconds(2f) };
            //    obj.Set("key", Guid.NewGuid().ToString(), ca);



        }
        private ZoneSetting _setting;
        /// <summary>
        /// 
        /// </summary>
        public virtual void OnInit()
        {
            try
            {
                _setting = new ZoneSetting();
                var osbit = GameZone.bit;
                var platform =GameZone.ps;
                TraceLog.WriteLine(string.Format(GameZone.char_d,
                    Assembly.GetExecutingAssembly().GetName().FullName.Split(',')[0],
                    osbit,
                    platform,
                    Assembly.GetExecutingAssembly().GetName().FullName.Split(',')[1],
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
                  //  TraceLog.WriteLine(Lang.Language.Instance.ServerBusy);
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
              //  var obj = Script.ScriptEngines.Execute("Game.Script.Action2017", "Game.Script.Action2017", new TNet.Service.ActionGetter(null,null));
                Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(null).UseStartup<Web.Startup>().Start("http://127.0.0.1:666");
                //  var obj = Script.ScriptEngines.Execute("code", "Game.Script.Action2017");
                //   TNet.Cache.GameDataCacheSet.ContextCacheSet<TNet.Cache.Generic.CacheItem> ca=new ContextCacheSet<CacheItem>("item");
                PersonalCacheStruct<TNet.Com.Model.ExGiftNoviceCard> personal=new PersonalCacheStruct<Com.Model.ExGiftNoviceCard>();
                //    ca.TryAdd("fyindex", new CacheItem() { Item = "fafff" ,IsInCache=true});

                var tabl = new Model.SchemaTable();
                tabl.Keys = new string[] {"xsk" };
                tabl.EntityName = "fyindex";
                tabl.StorageType = Model.StorageType.ReadWriteRedis;
             
                TNet.Model.EntitySchemaSet.AddSchema(typeof(TNet.Com.Model.ExGiftNoviceCard), tabl);
                personal.LoadFrom((x) => { return true; });
                // var b = new Com.Model.ExGiftNoviceCard { CardNo = "fff" };
                //           var rr=     personal.Count;
      var rrr=          personal.Find("1",(x)=> {return x.CardNo == "fff"; });
                var rwrr = personal.FindKey("1", "xsk");
                var kr = personal.ChildrenItem[0].GetItem() as TNet.Cache.Generic.CacheCollection;// ("xsk",(x)=> { return x.CardNo == "fff"; });
                                                                                                  //              personal.Update(true);
                var krr = kr.TryGetValue("xsk", out TNet.Com.Model.ExGiftNoviceCard sw);
                // personal.Update
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
