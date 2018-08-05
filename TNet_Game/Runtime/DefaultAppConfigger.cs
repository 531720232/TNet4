using System.Configuration;
using System.IO;
using TNet.Configuration;
using TNet.Extend;
using TNet.RPC.IO;

namespace TNet.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultAppConfigger : DefaultDataConfigger
    {
        /// <summary>
        /// init
        /// </summary>
        public DefaultAppConfigger()
        {
            ConfigFile = Path.Combine(MathUtils.RuntimePath, "App_Server.config");
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void LoadConfigData()
        {
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection("connectionStrings");
            var er = ConfigurationManager.ConnectionStrings.GetEnumerator();
            while (er.MoveNext())
            {
                var connSetting = er.Current as ConnectionStringSettings;
                if (connSetting == null) continue;
                AddNodeData(new ConnectionSection(connSetting.Name, connSetting.ProviderName, connSetting.ConnectionString));
            }
            var setting = GameZone.Setting;
            setting.Reset();
            MessageStructure.Enable7zip = setting.ActionEnable7Zip;
            MessageStructure.Enable7zipMinByte = setting.Action7ZipOutLength;
            base.LoadConfigData();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AppServerConfigger : DataConfigger
    {
        /// <summary>
        /// init
        /// </summary>
        public AppServerConfigger()
        {
            ConfigFile = Path.Combine(MathUtils.RuntimePath, "AppServer.config");
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void LoadConfigData()
        {

        }
    }
}