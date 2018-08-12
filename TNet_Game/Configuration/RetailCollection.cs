using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TNet.Configuration
{
    /// <summary>
    /// 渠道集合节点属性
    /// </summary>
    public class RetailCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates the new element.
        /// </summary>
        /// <returns>The new element.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new RetailElement();
        }
        /// <summary>
        /// Gets the element key.
        /// </summary>
        /// <returns>The element key.</returns>
        /// <param name="element">Element.</param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RetailElement)element).Id;
        }
        /// <summary>
        /// Gets the <see cref="TNet.Configuration.RetailCollection"/> at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        public RetailElement this[int index]
        {
            get
            {
                return (RetailElement)base.BaseGet(index);
            }
        }
        /// <summary>
        /// Gets the <see cref="TNet.Configuration.RetailCollection"/> with the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public new RetailElement this[string key]
        {
            get
            {
                return (RetailElement)base.BaseGet(key);
            }
        }
    }
}
