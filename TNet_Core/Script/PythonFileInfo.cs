using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Script
{
    ///<summary>
    /// Python文件信息
    ///</summary>
    [Serializable]
    public class PythonFileInfo : ScriptFileInfo
    {
        ///<summary>
        ///</summary>
        ///<param name="fileCode"></param>
        ///<param name="fileName"></param>
        public PythonFileInfo(string fileCode, string fileName)
            : base(fileCode, fileName)
        {
        }

        /// <summary>
        /// The Compiled Code.
        /// </summary>
        public CompiledCode CompiledCode { get; set; }
    }
}
