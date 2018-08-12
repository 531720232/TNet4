using System.Configuration;

namespace TNet.Command
{
    /// <summary>
    /// Command element.
    /// </summary>
    public class CommandElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the cmd.
        /// </summary>
        /// <value>The cmd.</value>
        [ConfigurationProperty("cmd", IsRequired = true)]
        public string Cmd
        {
            get { return this["cmd"] as string; }
            set { this["cmd"] = value; }
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

    }
}