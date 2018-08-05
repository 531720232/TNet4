using System;
using System.Collections.Generic;
using System.Text;
using TNet.Model;
using TNet.Model;

namespace TNet.Net
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="column"></param>
    /// <param name="fieldValue"></param>
    public delegate void EntityPropertySetFunc<T>(T entity, SchemaColumn column, object fieldValue) where T : new();

    /// <summary>
    /// 数据接收处理接口
    /// </summary>
    public interface IDataReceiver : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool TryReceive<T>(out List<T> dataList) where T : ISqlEntity, new();
    }

}
