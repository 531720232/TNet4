using System.Configuration;

namespace TNet.Configuration
{
    /// <summary>
    /// 渠道节点属性
    /// </summary>
    public class RetailElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get { return this["id"] as string; }
            set { this["id"] = value; }
        }
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
        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        [ConfigurationProperty("args", IsRequired = false)]
        public string Args
        {
            get { return this["args"] as string; }
            set { this["args"] = value; }
        }
    }
}