using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using TNet.Extend;

namespace TNet.RPC.IO
{
    /// <summary>
    /// 参数获取者
    /// </summary>
    public class ParamGeter : IDisposable
    {
        private const string SignName = "sign";
        private readonly string _signKey;
        private string _requestParam;
        private Dictionary<string, string> _paramSet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="signKey"></param>
        public ParamGeter(string param, string signKey = "")
        {
            _signKey = signKey;
            _paramSet = new Dictionary<string, string>();
            InitData(param);
        }

        private void InitData(string param)
        {
            param = HttpUtility.UrlDecode(param, Encoding.UTF8) ?? "";
            int index = param.LastIndexOf(string.Format("&{0}=", SignName), StringComparison.OrdinalIgnoreCase);
            if (index != -1)
            {
                _requestParam = param.Substring(0, index);
            }
            string[] splitresult = param.Split(new char[] { '&' });
            foreach (string result in splitresult)
            {
                string[] equalsplitresult = result.Split(new char[] { '=' });
                if (equalsplitresult.Length == 2)
                {
                    string key = equalsplitresult[0].ToLower();
                    string value = HttpUtility.UrlDecode(equalsplitresult[1], Encoding.UTF8);
                    if (_paramSet.ContainsKey(key))
                    {
                        _paramSet[key] = value;
                    }
                    else
                    {
                        _paramSet.Add(key, value);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckSign()
        {
            if (_paramSet.ContainsKey(SignName))
            {
                if (_requestParam != null)
                {
                    string key = SignUtils.EncodeSign(_requestParam, _signKey);
                    if (!string.IsNullOrEmpty(key) && key == _paramSet[SignName])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            name = name.ToLower();
            return _paramSet.ContainsKey(name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(string name)
        {
            name = name.ToLower();
            if (_paramSet.ContainsKey(name))
            {
                return _paramSet[name];
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetInt(string name)
        {
            return GetString(name).ToInt();
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            DoDispose(true);
        }
        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void DoDispose(bool disposing)
        {
            if (disposing)
            {
                //清理托管对象
                _paramSet = null;
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
        }
    }
}
