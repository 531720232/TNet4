using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Configuration
{
 public   class ConnectionSection: ConfigSection
    {
        public ConnectionSection(string name,string providerName,string connection_string)
        {
            Load(name, providerName, connection_string);
        }

        public void Load(string name, string providerName, string connectionString)
        {
            Name = name;
            ProviderName = providerName;
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
