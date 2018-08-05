using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Configuration
{
 public   class ConfigNode
    {
        public ConfigNode()
        {

        }
        public ConfigNode(string key,string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }

    }
}
