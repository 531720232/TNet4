using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TNet.Extend;
namespace TNet.Script
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DLLRuntimeScope : CSharpRuntimeScope
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settupInfo"></param>
        public DLLRuntimeScope(ScriptSettupInfo settupInfo)
            : base(settupInfo)
        {
        }
        public List<MethodInfo> mains = new List<MethodInfo>();
        List<Assembly> assemblies = new List<Assembly>();

        public override object Execute(string scriptCode, string typeName, params object[] args)
        {
          
          
            if (scriptCode==null)
            {
              
              foreach(var v in assemblies)
                {
                 var type=  v.GetType(typeName);
                    if(type!=null)
                    {
                        return type.CreateInstance(args);
                    }
                }
                foreach (var v in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var type = v.GetType(typeName);
                    if (type != null)
                    {
                        return type.CreateInstance(args);
                    }
                }
            }
            else
            {
                if (typeName == null) { }
              else  if(assemblies.Exists(x=>x.FullName.StartsWith(scriptCode)))
                {
               var b=     assemblies.Find(x => x.FullName.StartsWith(scriptCode));
                    var type = b.GetType(typeName);
                    if (type != null)
                    {
                        return type.CreateInstance(args);
                    }
                }
                else if (AppDomain.CurrentDomain.GetAssemblies().ToList().Exists(x => x.FullName.StartsWith(scriptCode)))
                {
                    var gw = AppDomain.CurrentDomain.GetAssemblies().ToList();
                    var b = gw.Find(x => x.FullName.StartsWith(scriptCode));
                    var type = b.GetType(typeName);
                    if (type != null)
                    {
                        return type.CreateInstance(args);
                    }
                }
            }


            return base.Execute(scriptCode, typeName, args);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            if (_modelAssembly == null)
            {
                
                //启动时清理脚本运行环境
                //    ScriptCompiler.ClearTemp();
            }


            //     List<Assembly> assemblies = new List<Assembly>();


           // InitDll();

            base.Init();

        }
        public void InitDll()
        {
            var dlls = System.IO.Directory.GetFiles(_dllPath, "*.dll", SearchOption.AllDirectories);
            //加载热更dll
            foreach (var dl in dlls)
            {
                var fs_dll = System.IO.File.ReadAllBytes(dl);
                var ab =Assembly.Load(fs_dll);
                //   var cd = AppDomain.CurrentDomain.GetAssemblies();
                //       assemblies.Add(ab);
                assemblies.Add(ab);
                foreach (var a in ab.GetTypes())
                {
                    var objs = (AutoRun)a.GetCustomAttribute(typeof(AutoRun));
                    if (objs != null)
                    {
                        var ojbk = Activator.CreateInstance(a);
                        var method = a.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);
                        mains.Add(method);//     method.Invoke(null, new object[] { null});
                        break;
                    }
                }
                AddWatchPath(_dllPath, "*.dll");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void DoDispose(bool disposing)
        {
            base.DoDispose(disposing);
        }

    }
}
