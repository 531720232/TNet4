using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using TNet.Config;
using TNet.Configuration;
using TNet.Contract;
using TNet.Extend;
using TNet.Log;
using TNet.Script;
using TNet.Serialization;

namespace TNet.Runtime
{

    /// <summary>
    /// The Zone configuration information.
    /// </summary>
    [Serializable]
    public class ZoneSetting
    {

        static ZoneSetting()
        {
            bool result;
            try
            {
                result = ConfigManager.Intialize("appServerConfigger");
            }
            catch (Exception)
            {
                result = false;
            }
            if (!result)
            {
                try
                {
                    ConfigManager.GetConfigger<DefaultAppConfigger>();
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Configger init error:{0}", ex);
                }
            }
            LoadDecodeFunc();
        }

        private static string GetLocalIp()
        {
            string localIp = "";
            IPAddress[] addressList = Dns.GetHostEntry(Environment.MachineName).AddressList;
            foreach (var ipAddress in addressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ipAddress.ToString();
                    break;
                }
            }
            return localIp;
        }
        /// <summary>
        /// Object Initialization.
        /// </summary>
        public ZoneSetting()
        {
            var appServer = GetServerSection();
            var protocol = GetProtocolSection();
            var cacheSection = GetCacheSection();
            var scriptSection = GetScriptSection();

            CacheGlobalPeriod = cacheSection.ShareExpirePeriod;
            CacheUserPeriod = cacheSection.PersonalExpirePeriod;

            ScriptSysAsmReferences = scriptSection.SysAssemblyReferences;
            ScriptAsmReferences = scriptSection.AssemblyReferences;
            GamePort = protocol.GamePort;
            GameIpAddress = string.IsNullOrEmpty(protocol.GameIpAddress) ? GetLocalIp() : protocol.GameIpAddress;

            try
            {
                if (!string.IsNullOrEmpty(appServer.EntityAssemblyName))
                {
                    EntityAssembly = Assembly.LoadFrom(appServer.EntityAssemblyName);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Load entity assembly error:\"{0}\" {1}", appServer.EntityAssemblyName, ex);
            }
            ActionDispatcher = new ScutActionDispatcher();
            InitSerializer();
            Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            var appServer = GetServerSection();
            ProductCode = appServer.ProductCode;
            ProductName = appServer.ProductName;
            ProductServerId = appServer.ProductServerId;
            ClientVersion = appServer.ClientVersion;
            ClientDesDeKey = appServer.UserLoginDecodeKey;
            ActionTypeName = appServer.ActionTypeName;
            ScriptTypeName = appServer.ScriptTypeName;
            RemoteTypeName = appServer.RemoteTypeName;
            AccountServerUrl = appServer.AccountServerUrl;

            var protocol = GetProtocolSection();
            ProductSignKey = protocol.SignKey;
            ActionEnable7Zip = protocol.EnableAction7Zip;
            Action7ZipOutLength = protocol.Action7ZipOutLength;
        }

        private void InitSerializer()
        {
            string type = ConfigManager.Configger.GetFirstOrAddConfig<CacheSection>().SerializerType;
            if (string.Equals(type, "json", StringComparison.OrdinalIgnoreCase))
            {
                Serializer = new JsonCacheSerializer(Encoding.UTF8);
            }
            else
            {
                Serializer = new ProtobufCacheSerializer();
            }
        }

        private static AppServerSection GetServerSection()
        {
            return ConfigManager.Configger.GetFirstOrAddConfig<AppServerSection>();
        }
        private static ProtocolSection GetProtocolSection()
        {
            return ConfigManager.Configger.GetFirstOrAddConfig<ProtocolSection>();
        }
        private static CacheSection GetCacheSection()
        {
            return ConfigManager.Configger.GetFirstOrAddConfig<CacheSection>();
        }

        private static ScriptSection GetScriptSection()
        {
            return ConfigManager.Configger.GetFirstOrAddConfig<ScriptSection>();
        }

        /// <summary>
        /// 
        /// </summary>
        public string RedisHost
        {
            get
            {
                var section = ConfigManager.Configger.GetFirstOrAddConfig<RedisSection>();
                return section.Host;
            }
        }

        private static dynamic _scriptDecodeTarget;
        private static void LoadDecodeFunc()
        {
            string decodeFuncTypeName = "";
            try
            {
                var section = GetServerSection();
                decodeFuncTypeName = section.DecodeFuncTypeName;
                if (string.IsNullOrEmpty(decodeFuncTypeName)) return;
                var type = Type.GetType(decodeFuncTypeName, true, true);
                _scriptDecodeTarget = type.CreateInstance();
                ScriptEngines.SettupInfo.DecodeCallback += DecodeCallback;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Load DecodeFunc type error:\"{0}\" {1}", decodeFuncTypeName, ex);
            }
        }

        private static string DecodeCallback(string source, string ext)
        {
            if (_scriptDecodeTarget == null)
                return "";
            return _scriptDecodeTarget.DecodeCallback(source, ext);
        }


        /// <summary>
        /// Request signature key.
        /// </summary>
        public string ProductSignKey { get; set; }

        /// <summary>
        /// Des encryption key account password.
        /// </summary>
        [Obsolete("Sns no use")]
        public string ProductDesEnKey { get; set; }

        /// <summary>
        /// Des decryption for client password.
        /// </summary>
        public string ClientDesDeKey { get; set; }

        /// <summary>
        /// Global cache lifecycle.
        /// </summary>
        public int CacheGlobalPeriod { get; set; }

        /// <summary>
        /// Game players cache lifecycle.
        /// </summary>
        public int CacheUserPeriod { get; set; }

        /// <summary>
        /// Product code.
        /// </summary>
        public int ProductCode { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Product server id.
        /// </summary>
        public int ProductServerId { get; set; }

        /// <summary>
        /// Client ver
        /// </summary>
        public Version ClientVersion { get; set; }
        /// <summary>
        /// The entity assembly.
        /// </summary>
        public Assembly EntityAssembly { get; set; }

        /// <summary>
        /// Script use system assembly reference.
        /// </summary>
        public string[] ScriptSysAsmReferences { get; set; }

        /// <summary>
        /// Script use other assembly reference.
        /// </summary>
        public string[] ScriptAsmReferences { get; set; }

        /// <summary>
        /// enable gzip
        /// </summary>
        public bool ActionEnable7Zip { get; set; }

        /// <summary>
        /// stream out length use gzip.
        /// </summary>
        public int Action7ZipOutLength { get; set; }

        /// <summary>
        /// Action type name.
        /// </summary>
        public string ActionTypeName { get; set; }

        /// <summary>
        /// CSharp script type name.
        /// </summary>
        public string ScriptTypeName { get; set; }

        /// <summary>
        /// Remote type name.
        /// </summary>
        public string RemoteTypeName { get; set; }


        /// <summary>
        /// Account login server's url
        /// </summary>
        public string AccountServerUrl { get; set; }

        /// <summary>
        /// local ip
        /// </summary>
        public string GameIpAddress
        {
            get;
            private set;
        }

        /// <summary>
        /// socket port
        /// </summary>
        public int GamePort
        {
            get;
            private set;
        }

        /// <summary>
        /// Action repeater
        /// </summary>
        public IActionDispatcher ActionDispatcher { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICacheSerializer Serializer { get; set; }
        ///// <summary>
        ///// Before starting the script engine process.
        ///// </summary>
        //public event Action ScriptStartBeforeHandle;

        //internal void OnScriptStartBefore()
        //{
        //    if (ScriptStartBeforeHandle != null)
        //    {
        //        ScriptStartBeforeHandle();
        //    }
        //}

    }
}
