using System.Collections.Generic;
using System.Linq;
using EcsRx.Collections;
using EcsRx.Events;
using EcsRx.Executor;
using EcsRx.Executor.Handlers;
using EcsRx.Extensions;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Modules;
using EcsRx.Infrastructure.Plugins;
using EcsRx.Systems;
using EcsRx.Views.Systems;

namespace EcsRx.Infrastructure
{
    public abstract class EcsRxApplication : IEcsRxApplication
    {
        public ISystemExecutor SystemExecutor { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IEntityCollectionManager EntityCollectionManager { get; private set; }
        public List<IEcsRxPlugin> Plugins { get; }

        [System.Obsolete]
        protected abstract IDependencyContainer DependencyContainer { get; }

        protected EcsRxApplication()
        {
            Plugins = new List<IEcsRxPlugin>();
        }

        public virtual void StartApplication()
        {
            RegisterModules();
            ApplicationStarting();
            RegisterAllPluginDependencies();
            SetupAllPluginSystems();
            ApplicationStarted();
        }

        protected virtual void RegisterModules()
        {
        //   DependencyContainer.LoadModule<FrameworkModule>();
            var mess = new EcsRx.Reactive.MessageBroker();
         // DependencyContainer.Resolve<ISystemExecutor>();
            EventSystem = new EventSystem(mess);// DependencyContainer.Resolve<IEventSystem>();

            var factory = new Entities.DefaultEntityFactory(EventSystem);
            var factory2 = new DefaultEntityCollectionFactory(factory, EventSystem);
            var ob = new Groups.Observable.DefaultObservableObservableGroupFactory(EventSystem);
            EntityCollectionManager = new EntityCollectionManager(EventSystem, factory2, ob);
            var handler = new List<IConventionalSystemHandler>();
            handler.Add(new ReactToEntitySystemHandler(EntityCollectionManager));
            handler.Add(new ReactToGroupSystemHandler(EntityCollectionManager));
            handler.Add(new ReactToDataSystemHandler(EntityCollectionManager));
            handler.Add(new ManualSystemHandler(EntityCollectionManager));
            handler.Add(new SetupSystemHandler(EntityCollectionManager));
            handler.Add(new TeardownSystemHandler(EntityCollectionManager));


        


          //  var def = new EntityCollection();
           // DependencyContainer.Resolve<IEntityCollectionManager>();
        }

        protected virtual void ApplicationStarting() { }
        protected abstract void ApplicationStarted();

        protected virtual void RegisterAllPluginDependencies()
        { Plugins.ForEachRun(x => x.SetupDependencies(DependencyContainer)); }

        protected virtual void SetupAllPluginSystems()
        {
            Plugins.SelectMany(x => x.GetSystemsForRegistration(DependencyContainer))
                .ForEachRun(x => SystemExecutor.AddSystem(x));
        }

        protected void RegisterPlugin(IEcsRxPlugin plugin)
        { Plugins.Add(plugin); }
        
        protected virtual void RegisterAllBoundSystems()
        {

            //var allSystems =DependencyContainer.ResolveAll<ISystem>();

            //var orderedSystems = allSystems
            //    .OrderByDescending(x => x is ViewResolverSystem)
            //    .ThenByDescending(x => x is ISetupSystem);
            
            //orderedSystems.ForEachRun(SystemExecutor.AddSystem);
        }

        protected virtual void RegisterSystem<T>() where T : ISystem
        {
            var system = DependencyContainer.Resolve<T>();
            SystemExecutor.AddSystem(system);
        }
    }
}