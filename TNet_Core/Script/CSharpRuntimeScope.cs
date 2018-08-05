using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using TNet.Collection.Generic;
using TNet.Extend;
using TNet.Log;
using TNet.Security;

namespace TNet.Script
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CSharpRuntimeScope : PythonRuntimeScope
    {
        private string _csharpScriptPath;
        private string _modelScriptPath;
        private const string FileFilter = "*.cs";
        public DictionaryExtend<string, ScriptFileInfo> _modelCodeCache;
        public DictionaryExtend<string, ScriptFileInfo> _csharpCodeCache;
        /// <summary>
        /// Script assembly verion
        /// </summary>
        private static int ScriptVerionId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settupInfo"></param>
        public CSharpRuntimeScope(ScriptSettupInfo settupInfo)
            : base(settupInfo)
        {
        }

        /// <summary>
        /// 是否是Model类型的脚本
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool IsModelScript(string file)
        {
            return file.ToLower().IndexOf(_modelScriptPath.ToLower(), StringComparison.Ordinal) != -1;
        }
      public  string _dllPath;
        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            _modelCodeCache = new DictionaryExtend<string, ScriptFileInfo>();
            _csharpCodeCache = new DictionaryExtend<string, ScriptFileInfo>();
            _modelScriptPath = Path.Combine(SettupInfo.RuntimePath, SettupInfo.ScriptRelativePath, SettupInfo.ModelScriptPath);
            AddWatchPath(_modelScriptPath, FileFilter);

            _dllPath = Path.Combine(SettupInfo.RuntimePath, SettupInfo.ScriptRelativePath, SettupInfo.DLLPath);
            _csharpScriptPath = Path.Combine(SettupInfo.RuntimePath, SettupInfo.ScriptRelativePath, SettupInfo.CSharpScriptPath);
            AddWatchPath(_csharpScriptPath, FileFilter);

           
        //    Load();
            base.Init();
        }

        /// <summary>
        /// Process csharp script.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object ExecuteCSharp(string typeName, params object[] args)
        {
            object result;
            if (CreateInstance(null, typeName, args, out result)) return result;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override object Execute(string scriptCode, string typeName, params object[] args)
        {
            string code = FormatScriptCode(SettupInfo.CSharpScriptPath, scriptCode, ".cs");
            if (_csharpCodeCache.TryGetValue(code, out var ad))
            {
            
                object result;
                if (args.Length == 0)
                {
                    if ((result = Execute(ad.Source, true)) != null)
                        return result;
                }
                if ((result = Execute(ad.Source, true, args)) != null) return result;
            }else
            if(typeName!=null)
            {
               
            }
            return base.Execute(scriptCode, typeName, args);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scriptCode"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public bool InvokeStaticMenthod<T>(string method, string scriptCode = "")
        {
            object methodResult;
            return InvokeStaticMenthod<T, object>(method, out methodResult, scriptCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="method"></param>
        /// <param name="methodResult"></param>
        /// <param name="scriptCode"></param>
        /// <returns></returns>
        public bool InvokeStaticMenthod<T, TR>(string method, out TR methodResult, string scriptCode = "")
        {
            return InvokeStaticMenthod<T, TR>(method, new Object[0], out methodResult, scriptCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="scriptCode"></param>
        /// <param name="method"></param>
        /// <param name="methodArgs"></param>
        /// <param name="methodResult"></param>
        /// <returns></returns>
        public bool InvokeStaticMenthod<T, TR>(string method, Object[] methodArgs, out TR methodResult, string scriptCode = "")
        {
            methodResult = default(TR);
            object result;
            if (InvokeStaticMenthod(scriptCode, typeof(T).FullName, method, methodArgs, out result))
            {
                if (result != null) methodResult = (TR)result;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <param name="typeName"></param>
        /// <param name="method"></param>
        /// <param name="methodArgs"></param>
        /// <param name="methodResult"></param>
        /// <returns></returns>
        public bool InvokeStaticMenthod(string scriptCode, string typeName, string method, Object[] methodArgs, out object methodResult)
        {
            methodResult = null;
            Type type;
            string code = FormatScriptCode(SettupInfo.CSharpScriptPath, scriptCode, ".cs");
            if (!VerifyScriptHashCode(_csharpCodeCache[code].FileName))
            {
                return false;
            }
            code = _csharpCodeCache[code].Source;

            //   string code = _csharpCodeCache[scriptCode].Source;
            object obj;
           obj = Execute(code);
            if(obj!=null)
            {
                methodResult =obj.GetType().InvokeMember(method, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, methodArgs);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <param name="typeName"></param>
        /// <param name="typeArgs"></param>
        /// <param name="method"></param>
        /// <param name="methodArgs"></param>
        public override bool InvokeMenthod(string scriptCode, string typeName, Object[] typeArgs, string method, params Object[] methodArgs)
        {
            string code = FormatScriptCode(SettupInfo.CSharpScriptPath, scriptCode, ".cs");
            if(!VerifyScriptHashCode(_csharpCodeCache[code].FileName))
            {
                return false;
            }
            code = _csharpCodeCache[code].Source;

            object obj;
            obj = Execute(code,true,typeArgs);
            if (obj!=null)
            {
                MethodInfo methodInfo = obj.GetType().GetMethod(method);
                if (methodInfo != null)
                {
                    methodInfo.Invoke(obj, methodArgs);
                    return true;
                }
                return false;
            }
            return base.InvokeMenthod(scriptCode, typeName, typeArgs, method, methodArgs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="refAssemblies"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object ExecuteCSharpSource(string[] sources, string[] refAssemblies, string typeName, params object[] args)
        {
         
          //foreach(var s in sources)
          //  {
          //  var dll=    (System.IO.File.ReadAllText(s));
          //      var ab=ToDll(dll);
          //      AppDomain.CurrentDomain.Load(ab.GetName());
          //      //   assemblies.Add(ab);
          //  }

          
          // ScriptCompiler.CompileSource(sources, refAssemblies, "DynamicCode", SettupInfo.ScriptIsDebug, true);
           foreach(var result in assemblies)
            {
                Type type = null;
                if (string.IsNullOrEmpty(typeName))
                {
                    type = result.GetTypes()[0];
                }
                else
                {
                    type = result.GetType(typeName, false, true);
                }
                if (type != null) return type.CreateInstance(args);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override bool VerifyScriptHashCode(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (string.Compare(ext, ".cs", StringComparison.OrdinalIgnoreCase) != 0)
            {
                return base.VerifyScriptHashCode(fileName);
            }
            string scriptCode = GetScriptCode(fileName);
            if (File.Exists(fileName))
            {
                if (fileName.EndsWith("AssemblyInfo.cs", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                ScriptFileInfo code = null;
                if (_modelCodeCache.ContainsKey(scriptCode))
                {
                    code = _modelCodeCache[scriptCode];
                }
                if (_csharpCodeCache.ContainsKey(scriptCode))
                {
                    code = _csharpCodeCache[scriptCode];
                }
                if (code == null) return false;
                string source = Decode(File.ReadAllText(fileName), ext);
                return code.HashCode == CryptoHelper.ToMd5Hash(source);
            }
            return false;
        }

        public const string cs_namespace = "CS";
        public const string cs_lib = "cs.dll";
        dynamic _csEngine;



        public List<MethodInfo> mains = new List<MethodInfo>();
        List<Assembly> assemblies = new List<Assembly>();
        /// <summary>
        /// Init csharp script.
        /// </summary>
        /// 

        public void InitCsharp()
        {

            Assembly csInterfaceAssembly = null;
            try
            {
                var pa =(SettupInfo.RuntimePrivateBinPath);
                //var fs = System.IO.Directory.GetFiles(pa, "*.dll");
                //foreach (var p in fs)
                //{
                //    AppDomain.CurrentDomain.Load(System.IO.File.ReadAllBytes(p));
                //}
                csInterfaceAssembly = Assembly.LoadFrom(Path.Combine(SettupInfo.RuntimePrivateBinPath, cs_lib));
            
                AppDomain.CurrentDomain.Load(csInterfaceAssembly.GetName());
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading cs library", ex);
            }
            Type type = csInterfaceAssembly.GetType(cs_namespace + ".CSharp", false, true);
            if (type != null)
            {
                _csEngine = type.CreateInstance();
           //     Load();
            //    RegisterMethod();
            }

         //   var dlls = System.IO.Directory.GetFiles(_dllPath, "*.dll", SearchOption.AllDirectories);
         //   //     List<Assembly> assemblies = new List<Assembly>();
         //   //加载热更dll
         //   foreach (var dl in dlls)
         //   {
         //       var fs_dll = System.IO.File.ReadAllBytes(dl);
         //       var ab = AppDomain.CurrentDomain.Load(fs_dll);
         //       var cd = AppDomain.CurrentDomain.GetAssemblies();
         ////       assemblies.Add(ab);
         //       foreach(var a in ab.GetTypes())
         //       {
         //         var objs=  (AutoRun)a.GetCustomAttribute(typeof(AutoRun));
         //        if(objs!=null)
         //           {
         //           var ojbk=    Activator.CreateInstance(a);
         //          var method=     a.GetMethod("Main", BindingFlags.Public|BindingFlags.Static);
         //               mains.Add(method);//     method.Invoke(null, new object[] { null});
         //               break;
         //           }
         //       } 
         //   }


           //_csharpScriptPath;
           if (Directory.Exists(_csharpScriptPath))
           {
                var files = Directory.GetFiles(_csharpScriptPath, FileFilter, SearchOption.AllDirectories);
              
                foreach (var fileName in files)
                {
                    LoadScript(_csharpScriptPath, fileName);
                }
            }
           
           //CompileCsharp();
           //BuildPythonReferenceFile();
        }


        /// <summary>
        /// 
        /// </summary>
        private void Load()
        {
            var pathList = new String[] { _modelScriptPath, _csharpScriptPath };

            foreach (var path in pathList)
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, FileFilter, SearchOption.AllDirectories);
                
                    foreach (var fileName in files)
                    {
                        LoadScript(path, fileName);
                    }
                }
            }
        //    Compile();
            BuildPythonReferenceFile();
        }


        private void Compile()
        {
            CompileModel();
            CompileCsharp();

        }

        private void CompileModel()
        {
            //string assemblyName = string.Format("DynamicScripts.{0}", "Model");
            //var refAssemblyNames = SettupInfo.ReferencedAssemblyNames.ToArray();
            //string[] sources = _modelCodeCache.Select(t =>
            //{
            //    string src = t.Value.Source;
            //    t.Value.Source = null;
            //    //huhu modify reason: Support for model debugging.
            //    return SettupInfo.ScriptIsDebug ? t.Value.FileName : src;
            //}).ToArray();
            //if (sources.Length == 0) return;
            ////加载实体程序集
            //_modelAssembly = ScriptCompiler.InjectionCompile(SettupInfo.RuntimePrivateBinPath, sources, refAssemblyNames, assemblyName, SettupInfo.ScriptIsDebug, false, out _modelAssemblyPath);
            //if (_modelAssembly == null)
            //{
            //    throw new Exception("The model script compile error");
            //}
        }

        private void CompileCsharp()
        {
          
        }

     

        private ScriptFileInfo LoadScript(string scriptPath, string fileName)
        {
            ScriptFileInfo scriptFileInfo = null;
            string scriptCode = GetScriptCode(fileName);
            scriptFileInfo = CreateScriptFile(fileName);
            if (scriptFileInfo != null)
            {
                if (scriptPath == _modelScriptPath)
                {
                    _modelCodeCache[scriptCode] = scriptFileInfo;
                }
                else
                {
                    _csharpCodeCache[scriptCode] = scriptFileInfo;
                }
            }
            return scriptFileInfo;
        }

        /// <summary>
        /// 创建脚本文件信息对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ScriptFileInfo CreateScriptFile(string fileName)
        {
            ScriptFileInfo scriptFileInfo = null;
            if (!File.Exists(fileName))
            {
                return scriptFileInfo;
            }
            string ext = Path.GetExtension(fileName);
            if (string.Compare(ext, ".cs", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string fileCode = GetScriptCode(fileName);
                scriptFileInfo = new CSharpFileInfo(fileCode, fileName);
                scriptFileInfo.Source = Decode(File.ReadAllText(fileName), ext);
                scriptFileInfo.HashCode = CryptoHelper.ToMd5Hash(scriptFileInfo.Source);
            }
            else
            {
                TraceLog.WriteError("Not supported \"{0}\" file type.", fileName);
            }
            return scriptFileInfo;
        }

        private bool CreateInstance(string scriptCode, string typeName, object[] args, out object result)
        {
            result = null;
            Type type;

            if (scriptCode != null)
            {
                string code = FormatScriptCode(SettupInfo.CSharpScriptPath, scriptCode, ".cs");
                if (!VerifyScriptHashCode(_csharpCodeCache[code].FileName))
                {
                    return false;
                }
                code = _csharpCodeCache[code].Source;
                result = Execute(code,true, args);
                return true;
            }
            else
            {
                foreach (var asm in assemblies)
                {
                    type = asm.GetType(typeName);
                    if (type != null)
                    {
                        result = Activator.CreateInstance(type, args);
                    }

                }

            }
            //if (TryParseType(scriptCode, typeName, out type))
            //{
            //    result = type.CreateInstance(args);
            //    return true;
            //}
            return false;
        }

        private bool TryParseType(string scriptCode, string typeName, out Type type)
        {
            type = null;
            if (string.IsNullOrEmpty(scriptCode))
            {
                typeName = typeName ?? "";
                scriptCode = ParseScriptCode(typeName);
            }
            scriptCode = GetScriptCode(scriptCode);
            Assembly assembly = _csharpAssembly;
            ScriptFileInfo scriptInfo = _csharpCodeCache[scriptCode];
            if (scriptInfo == null)
            {
                scriptInfo = _modelCodeCache[scriptCode];
                assembly = _modelAssembly;
            }

            if (scriptInfo != null)
            {
                if (assembly != null)
                {
                    type = assembly.GetType(typeName, false, true);
                    if(type==null)
                    {
                        type = Execute(scriptInfo.Source).GetType();
                    }
                }
                return type != null;
            }
            return false;
        }

        private string ParseScriptCode(string typeName)
        {
            string scriptCode;
            int index = typeName.ToLower().IndexOf(SettupInfo.CSharpScriptPath.ToLower() + ".", StringComparison.Ordinal);
            if (index > -1)
            {
                scriptCode = typeName.Substring(index) + ".cs";
            }
            else
            {
                var arr = typeName.Split(',')[0].Split('.');
                scriptCode = arr[arr.Length - 1] + ".cs";
            }
            return scriptCode;
        }

        /// <summary>
        /// 生成Python引用的头文件
        /// </summary>
        private void BuildPythonReferenceFile()
        {
            if (SettupInfo.DisablePython)
            {
                return;
            }
            StringBuilder pyCode = new StringBuilder();
            pyCode.AppendLine(@"import clr, sys");
       //     pyCode.AppendLine(@"clr.AddReference('ZyGames.Framework.Common')");
            pyCode.AppendLine(@"clr.AddReference('TNet.Framework')");
  
            var assmeblyList = GetAssemblies();
            foreach (var assmebly in assmeblyList)
            {
                if (assmebly == null) continue;

                pyCode.AppendFormat(@"clr.AddReference('{0}')", assmebly.GetName().Name);
                pyCode.AppendLine();
            }

            try
            {
                using (var sw = File.CreateText(SettupInfo.PythonReferenceLibFile))
                {
                    sw.Write(pyCode.ToString());
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("BuildPythonReferenceFile error:{0}", ex);
            }
        }

        public dynamic Execute(string code,bool g,params object[] vs)
        {
            //if(!g)
            //{
            //    return _csEngine.eval.LoadMethod(code);
            //}
         return   _csEngine.DoString(code, vs);
        }
        public  dynamic DoFile(string path, params object[] vs)
        {
            return _csEngine.DoFile(path, vs);
        }
        public System.Reflection.Assembly ToDll(string code)
        {
           
            return _csEngine.ToDll(code);
        }

    }
}
