using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TNet.Command
{
    /// <summary>
    /// Gm section.
    /// </summary>
    public class GmSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        [ConfigurationProperty("command", IsRequired = true)]
        public CommandCollection Command
        {
            get { return this["command"] as CommandCollection; }
            set { this["command"] = value; }
        }

    }
}
