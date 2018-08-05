using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Script
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    /// <summary>
    /// 自动注册句柄
    /// </summary>
    public class ActionSet:Attribute
    {
        public string Handler { get; set; }
        public ActionSet(string handler)
        {
            Handler = handler;
        }
    }
    /// <summary>
    /// 自动运行的程序
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class AutoRun : Attribute
    {


    }

}
