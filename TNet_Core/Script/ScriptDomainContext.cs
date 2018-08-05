using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TNet.Extend;

namespace TNet.Script
{
    /// <summary>
    /// Script domain context.
    /// </summary>
    public class ScriptDomainContext : MarshalByRefObject
    {
        private ConcurrentDictionary<string, Assembly> _assemblyList;
        /// <summary>
        /// 
        /// </summary>
        public ScriptDomainContext()
        {
            _assemblyList = new ConcurrentDictionary<string, Assembly>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyKey"></param>
        /// <param name="path"></param>
        public void LoadAssembly(string assemblyKey, string path)
        {
            if (!_assemblyList.ContainsKey(assemblyKey))
            {
                _assemblyList.TryAdd(assemblyKey, Assembly.LoadFrom(path));
            }
        }
        public void AddAssembly(string assemblyKey,Assembly assembly)
        {
            if (!_assemblyList.ContainsKey(assemblyKey))
            {
                _assemblyList.TryAdd(assemblyKey,assembly);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyKey"></param>
        /// <returns></returns>
        public Assembly GetAssembly(string assemblyKey)
        {
            return _assemblyList.ContainsKey(assemblyKey) ? _assemblyList[assemblyKey] : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyKey"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type GetTypeFrom(string assemblyKey, string typeName)
        {
            return _assemblyList.ContainsKey(assemblyKey) ? _assemblyList[assemblyKey].GetType(typeName) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyKey"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns>返回的类型需要加Serializable标识属性</returns>
        public object GetInstance(string assemblyKey, string typeName, params Object[] args)
        {
            var type = GetTypeFrom(assemblyKey, typeName);
            return type != null ? type.CreateInstance(args) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyKey"></param>
        /// <param name="typeName"></param>
        /// <param name="typeArgs"></param>
        /// <param name="method"></param>
        /// <param name="methodArgs"></param>
        /// <returns>返回的类型需要加Serializable标识属性</returns>
        public object Invoke(string assemblyKey, string typeName, Object[] typeArgs, string method, params Object[] methodArgs)
        {
            var type = GetTypeFrom(assemblyKey, typeName);
            if (type == null)
                return null;

            MethodInfo methodInfo = type.GetMethod(method);
            if (methodInfo == null)
                return null;

            Object obj = typeArgs == null ? type.CreateInstance() : type.CreateInstance(typeArgs);
            Object result = methodInfo.Invoke(obj, methodArgs);
            return result;
        }

    }
}
