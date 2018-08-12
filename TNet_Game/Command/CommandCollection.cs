using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TNet.Command
{
    /// <summary>
    /// Command collection.
    /// </summary>
    public class CommandCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates the new element.
        /// </summary>
        /// <returns>The new element.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CommandElement();
        }
        /// <summary>
        /// Gets the element key.
        /// </summary>
        /// <returns>The element key.</returns>
        /// <param name="element">Element.</param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CommandElement)element).Cmd;
        }
        /// <summary>
        /// Gets the <see cref="TNet.Command.CommandCollection"/> at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        public CommandElement this[int index]
        {
            get
            {
                return (CommandElement)base.BaseGet(index);
            }
        }
        /// <summary>
        /// Gets the <see cref="TNet.Command.CommandCollection"/> with the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public new CommandElement this[string key]
        {
            get
            {
                return (CommandElement)base.BaseGet(key);
            }
        }
    }
}
