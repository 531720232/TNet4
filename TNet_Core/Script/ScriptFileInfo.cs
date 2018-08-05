using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Script
{
    /// <summary>
    /// 脚本文件信息
    /// </summary>
    [Serializable]
    public abstract class ScriptFileInfo
    {
        private readonly string _fileCode;
        private readonly string _fileName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileCode">文件编号</param>
        /// <param name="fileName">完整路径和文件名</param>
        protected ScriptFileInfo(string fileCode, string fileName)
        {
            _fileCode = fileCode;
            _fileName = fileName;
        }

        /// <summary>
        /// 文件编号
        /// </summary>
        public string FileCode
        {
            get { return _fileCode; }
        }

        /// <summary>
        /// 源文件
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 完整路径和文件名
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        /// <summary>
        /// MD5哈希Code
        /// </summary>
        public string HashCode { get; set; }
    }
}
