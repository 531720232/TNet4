using EcsRx.Infrastructure.Dependencies;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNet.Dependencies
{
    public class NinjectDependencyContainer : IDependencyContainer
    {
        private readonly IKernel _kernel;

        public NinjectDependencyContainer()
        {
            _kernel = new StandardKernel();
        }

        public object NativeContainer => _kernel;

        public void Bind<TFrom, TTo>(BindingConfiguration configuration = null) where TTo : TFrom
        {
            var bindingSetup = _kernel.Bind<TFrom>();

            if (configuration == null)
            {
                bindingSetup.To<TTo>().InSingletonScope();
                return;
            }

            if (configuration.BindInstance != null)
            {
                var instanceBinding = bindingSetup.ToConstant((TFrom)configuration.BindInstance);

                if (configuration.AsSingleton)
                { instanceBinding.InSingletonScope(); }

                return;
            }

            var binding = bindingSetup.To<TFrom>();

            if (configuration.AsSingleton)
            { binding.InSingletonScope(); }

            if (!string.IsNullOrEmpty(configuration.WithName))
            { binding.Named(configuration.WithName); }

            if (configuration.WithConstructorArgs.Count == 0)
            { return; }

            foreach (var constructorArg in configuration.WithConstructorArgs)
            { binding.WithConstructorArgument(constructorArg.Key, constructorArg.Value); }
        }

        public void Bind<T>(BindingConfiguration configuration = null)
        { Bind<T, T>(configuration); }

        public T Resolve<T>(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            { return _kernel.Get<T>(); }

            return _kernel.Get<T>(name);
        }

        public IEnumerable<T> ResolveAll<T>()
        { return _kernel.GetAll<T>(); }

        public void LoadModule<T>() where T : IDependencyModule, new()
        {
            var module = new T();
            module.Setup(this);
        }
    }
}
