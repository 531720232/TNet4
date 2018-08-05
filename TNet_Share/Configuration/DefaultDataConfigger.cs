using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultDataConfigger : DataConfigger
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void LoadConfigData()
        {
        }

        internal void Add(ConfigSection nodeData)
        {
            AddNodeData(nodeData);
        }
    }
}
