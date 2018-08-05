using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Script
{
    /// <summary>
    /// CSharp文件信息
    /// </summary>
    [Serializable]
    public class CSharpFileInfo : ScriptFileInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileCode"></param>
        /// <param name="fileName"></param>
        public CSharpFileInfo(string fileCode, string fileName)
            : base(fileCode, fileName)
        {
        }

    }
    /// <summary>
    /// DLL文件信息
    /// </summary>
    [Serializable]
    public class DLLFileInfo : ScriptFileInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileCode"></param>
        /// <param name="fileName"></param>
        public DLLFileInfo(string fileCode, string fileName)
            : base(fileCode, fileName)
        {
        }

    }
}
