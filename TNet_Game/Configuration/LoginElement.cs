using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TNet.Configuration
{
    /// <summary>
    /// 登录节点元素
    /// </summary>
    public class LoginElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the retail list.
        /// </summary>
        /// <value>The retail list.</value>
        [ConfigurationProperty("retailList", IsRequired = true)]
        public RetailCollection RetailList
        {
            get { return this["retailList"] as RetailCollection; }
            set { this["retail"] = value; }
        }
        /// <summary>
        /// Gets or sets the default name of the type.
        /// </summary>
        /// <value>The default name of the type.</value>
        [ConfigurationProperty("defaultType")]
        public string DefaultTypeName
        {
            get { return this["defaultType"] as string; }
            set { this["defaultType"] = value; }
        }
    }
}
