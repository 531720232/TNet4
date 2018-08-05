using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Script
{

    /// <summary>
    /// 脚本执行顺序:DLL->cs -> py -> lua
    /// </summary>
    [Serializable]
    public class ScriptRuntimeScope : DLLRuntimeScope
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settupInfo"></param>
        public ScriptRuntimeScope(ScriptSettupInfo settupInfo)
            : base(settupInfo)
        {
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
            base.Init();
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
