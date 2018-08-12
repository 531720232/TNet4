using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TNet.Configuration
{
    /// <summary>
    /// 战斗配置
    /// </summary>
    public class CombatElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeName
        {
            get { return this["type"] as string; }
            set { this["type"] = value; }
        }

    }
}
