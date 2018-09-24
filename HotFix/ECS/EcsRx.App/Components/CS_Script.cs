
using EcsRx.Entities;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mjsg
{
    public class CS_Script : EcsRx.Components.IComponent
    {
        public string cs_path = "/script/cs/";
        public string py_path = "/script/py/";
        public IEntity Entity { get; set; }
        public CS_Script()
        {
          
            var fs = System.IO.Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory + py_path, "*.py", System.IO.SearchOption.AllDirectories);
            var code = "";
            //for (int i=0;i<fs.Length;i++)
            //{
            //   var str= System.IO.File.ReadAllText(fs[i]) + "\r\n";
            //    CSScriptLib.CSScript.Evaluator.LoadCode(str);
            //}
            //      var script = CSScriptLib.CSScript.Evaluator;
            //    CSScriptLib.CSScript.GlobalSettings.AddSearchDir(System.AppDomain.CurrentDomain.BaseDirectory + cs_path);
            //  ScriptRuntime pyRuntime = Python.CreateRuntime();
            // dynamic py = pyRuntime.UseFile(fs[0]);
            NLua.Lua lua = new NLua.Lua();
            lua.DoString("print('ffff')");
            //  script.SayHello("Hello World!");
            //    script.LoadCode("Console.WriteLine(new Class11().name);");//.GetType("Submission#0+EntryPoint").GetMethod("Start").Invoke(obj, null);

        }
    }
}
