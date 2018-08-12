using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TNet.Configuration
{
 public   class TNGameBaseBllSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>The login.</value>
        [ConfigurationProperty("login", IsRequired = true)]
        public LoginElement Login
        {
            get { return this["login"] as LoginElement; }
            set { this["login"] = value; }
        }
        /// <summary>
        /// Gets or sets the combat.
        /// </summary>
        /// <value>The combat.</value>
        [ConfigurationProperty("combat", IsRequired = false)]
        public CombatElement Combat
        {
            get { return this["combat"] as CombatElement; }
            set { this["combat"] = value; }
        }
    }
}
