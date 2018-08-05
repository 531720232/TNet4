using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Config
{
    public class RavenSection:TNet.Configuration.ConfigSection
    {
        public string Host;
        public string Main_db;
        public RavenSection()
        {
            Host = Configuration.ConfigUtils.GetSetting("Raven.Host", "localhost");
            Main_db = Configuration.ConfigUtils.GetSetting("Raven.Db", "gamedata");
        }
    }
}
