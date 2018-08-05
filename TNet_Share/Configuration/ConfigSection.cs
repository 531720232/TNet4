using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TNet.Configuration
{
    public class ConfigSection
    {
        private List<ConfigNode> configNodes;

        public ConfigSection()
        {

        }
        public ConfigSection(string node_String)
        {

        }
        public ConfigSection(IEnumerable<ConfigNode> nodes)
        {

        }
        private List<ConfigNode> Nodes => configNodes ?? (configNodes = new List<ConfigNode>());
        public int NodeCount => configNodes == null ? 0 : configNodes.Count;
        public void Load(string node_String)
        {
            string[] nodes = node_String.Split(';');
            foreach(var str in nodes)
            {
                string[] item= str.Split('=');
                if(item.Length==2)
                {
                    Nodes.Add(new ConfigNode(item[0], item[1]));

                }
            }
        }
        public void Load(IEnumerable<ConfigNode> nodes)
        {
            Nodes.AddRange(nodes);
        }
        public IList<ConfigNode> GetNodes()=>Nodes.ToList();

        public string[] GetKeys() => Nodes.Select(t => t.Key).ToArray();

        public int GetValue(string key,int defaultValue=0)=> Convert.ToInt32(GetValue(key, defaultValue.ToString()));
        public decimal GetValue(string key, decimal defaultValue = 0) => Convert.ToDecimal(GetValue(key, defaultValue.ToString()));





        public string GetValue(string key, string defaultValue)
        {
            var node = Nodes.Find(t => t.Key == key);
            if (node != null)
            {
                return node.Value;
            }
       
            return defaultValue;
        }

        /// 
        /// </summary>
        public string[] GetValues()
        {
            return Nodes.Select(t => t.Value).ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        public ConfigNode GetNode(string key)
        {
            return Nodes.Find(t => t.Key == key);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddNode(ConfigNode node)
        {
            Nodes.Add(node);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, int value)
        {
            return SetValue(key, value.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, decimal value)
        {
            return SetValue(key, value.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        public bool SetValue(string key, string value)
        {
            var settingNode = Nodes.Find(t => t.Key == key);
            if (settingNode != null)
            {
                settingNode.Value = value;
                return true;
            }
            AddNode(new ConfigNode(key, value));
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool SetNode(ConfigNode node)
        {
            return SetValue(node.Key, node.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool RemoveKey(string key)
        {
            var settingNode = Nodes.Find(t => t.Key == key);
            if (settingNode != null)
            {
                return RemoveNode(settingNode);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool RemoveNode(ConfigNode node)
        {
            return Nodes.Remove(node);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            Nodes.Clear();
        }
    }
}
