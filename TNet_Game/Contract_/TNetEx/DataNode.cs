using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MessagePack;
namespace TNet
{
    /// <summary>
    /// Implementing the IDataNodeSerializable interface in your class will make it possible to serialize
    /// that class into the Data Node format more efficiently.
    /// </summary>



    /// <summary>
    /// Data Node is a hierarchical data type containing a name and a value, as well as a variable number of children.
    /// Data Nodes can be serialized to and from IO data streams.
    /// Think of it as an alternative to having to include a huge 1 MB+ XML parsing library in your project.
    /// 
    /// Basic Usage:
    /// To create a new node: new DataNode (name, value).
    /// To add a new child node: dataNode.AddChild("Scale", Vector3.one).
    /// To retrieve a Vector3 value: dataNode.GetChild<Vector3>("Scale").
    /// </summary>
    [MessagePackObject]
  public  class DataNode
    {
        public DataNode this[string path]
        {
            get { return GetHierarchy(path); }
            set { SetHierarchy(path, value); }
        }


        // Actual saved value
        object mValue = null;

    // Temporary flag that gets set to 'true' after text-based deserialization
    [NonSerialized] bool mResolved = true;

    /// <summary>
    /// Data node's name.
    /// </summary>
    [Key(0)]
    public string name;

    /// <summary>
    /// Data node's value.
    /// </summary>
    [Key(1)]
    public object value
    {
        set
        {
            mValue = value;
            mResolved = true;
        }
        get
        {
            // ResolveValue returns 'false' when children were used by the custom data type and should now be ignored.
            if (!mResolved)
                children.Clear();
            return mValue;
        }
    }
    public void Write(object b)
    {
        value = (b);
        type = b.GetType();

    }
    public T Read<T>()
    {
        return (T)value;

    }
    public object Read()
    {
        return Convert.ChangeType(value, (type));

    }
    /// <summary>
    /// Whether this node is serializable or not.
    /// A node must have a value or children for it to be serialized. Otherwise there isn't much point in doing so.
    /// </summary>
    [IgnoreMember]
    public bool isSerializable { get { return value != null || children.Count > 0; } }

    /// <summary>
    /// List of child nodes.
    /// </summary>

    public System.Collections.Generic.List<DataNode> children = new System.Collections.Generic.List<DataNode>();

    /// <summary>
    /// Type the value is currently in.
    /// </summary>
    [Key(2)]
    public Type type { get; set; }// { get { return (value != null) ? mValue.GetType().FullName : typeof(void).FullName; } }

    public DataNode() { }
    public DataNode(string name) { this.name = name; }
    public DataNode(string name, string value) { this.name = name; this.value = value; }

    /// <summary>
    /// Clear the value and the list of children.
    /// </summary>

    public void Clear()
    {
        value = null;
        children.Clear();
        // Activator.CreateInstance(type,)

    }

    /// <summary>
    /// Get the node's value cast into the specified type.
    /// </summary>

    public object Get(Type type) { return Convert.ChangeType(mValue,type); }

    /// <summary>
    /// Retrieve the value cast into the appropriate type.
    /// </summary>

    public T Get<T>()
    {

        return (T)Convert.ChangeType(mValue,typeof(T));
    }

    /// <summary>
    /// Retrieve the value cast into the appropriate type.
    /// </summary>

    public T Get<T>(T defaultVal)
    {

        var json = (T)Convert.ChangeType(mValue, typeof(T));
        return json != null ? json : defaultVal;
    }

    /// <summary>
    /// Convenience function to add a new child node.
    /// </summary>

    public DataNode AddChild()
    {
        DataNode tn = new DataNode();
        children.Add(tn);
        return tn;
    }

    /// <summary>
    /// Add a new child node without checking to see if another child with the same name already exists.
    /// </summary>

    public DataNode AddChild(string name)
    {
        DataNode node = AddChild();
        node.name = name;
        return node;
    }

    /// <summary>
    /// Add a new child node without checking to see if another child with the same name already exists.
    /// </summary>

    public DataNode AddChild(string name, string value)
    {
        DataNode node = AddChild();
        node.name = name;
        node.value = value;
        return node;
    }

    /// <summary>
    /// Add a new child node after checking to see if it already exists. If it does, the existing value is returned.
    /// </summary>

    public DataNode AddMissingChild(string name, string value)
    {
        DataNode node = GetChild(name);
        if (node != null) return node;
        node = AddChild();
        node.name = name;
        node.value = value;
        return node;
    }

    /// <summary>
    /// Set the specified child, replacing an existing one if one already exists with the same name.
    /// </summary>

    public DataNode ReplaceChild(DataNode child)
    {
        if (child == null) return null;

        for (int i = 0; i < children.Count; ++i)
        {
            if (children[i].name == child.name)
            {
                if (child.value == null && child.children.Count == 0)
                {
                    children.RemoveAt(i);
                    return child;
                }

                children[i] = child;
                return children[i];
            }
        }

        children.Add(child);
        return child;
    }

    /// <summary>
    /// Set a child value. Will add a new child if a child with the same name is not already present.
    /// </summary>

    public DataNode SetChild(string name, object value)
    {
        DataNode node = GetChild(name);
        if (node == null) node = AddChild();
        node.name = name;

        node.Write(value);
        return node;
    }

    /// <summary>
    /// Retrieve a child by its path.
    /// </summary>

    public DataNode GetHierarchy(string path)
    {
        path = path.Replace("\\", "/");
        string[] split = path.Split('/');
        DataNode node = this;
        int index = 0;

        while (node != null && index < split.Length)
        {
            bool found = false;

            for (int i = 0; i < node.children.Count; ++i)
            {
                if (node.children[i].name == split[index])
                {
                    node = node.children[i];
                    ++index;
                    found = true;
                    break;
                }
            }

            if (!found) return null;
        }
        return node;
    }

    /// <summary>
    /// Retrieve a child by its path.
    /// </summary>

    public T GetHierarchy<T>(string path)
    {
        DataNode node = GetHierarchy(path);
        if (node == null) return default(T);


        return (T)Convert.ChangeType(node.value, typeof(T));
    }

    /// <summary>
    /// Retrieve a child by its path.
    /// </summary>

    public T GetHierarchy<T>(string path, T defaultValue)
    {
        DataNode node = GetHierarchy(path);
        if (node == null) return defaultValue;
        object value = node.value;

        var json = (T)Convert.ChangeType(value, typeof(T));
        return json != null ? json : defaultValue;
    }
        /// <summary>
        /// Retrieve a child by its path.
        /// </summary>

        public bool TryGetHierarchy<T>(string path, out T out_value)
        {
            out_value = default(T);
            DataNode node = GetHierarchy(path);
            if (node == null) return false ;
            object value = node.value;
            if(value==null)
            {
                return false;

            }

            try
            {
                out_value = (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {

            }
                return out_value != null ? true:false;
        }
        /// <summary>
        /// Set a node's value given its hierarchical path.
        /// </summary>

        public DataNode SetHierarchy(string path, object obj)
    {
        DataNode node = this;

        if (!string.IsNullOrEmpty(path))
        {
            if (path.IndexOf('\\') == -1 && path.IndexOf('/') == -1)
            {
                if (obj == null)
                {
                    RemoveChild(path);
                    return null;
                }

                node = GetChild(path, true);
            }
            else
            {
                path = path.Replace("\\", "/");
                string[] names = path.Split('/');
                DataNode parent = null;
                int index = 0;

                while (node != null && index < names.Length)
                {
                    bool found = false;

                    for (int i = 0; i < node.children.Count; ++i)
                    {
                        if (node.children[i].name == names[index])
                        {
                            parent = node;
                            node = node.children[i];
                            ++index;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        if (obj == null) break;
                        parent = node;
                        node = node.AddChild(names[index]);
                        ++index;
                    }
                }

                if (obj == null)
                {
                    if (parent != null) parent.RemoveChild(names[index - 1]);
                    return parent;
                }
            }
        }

        node.Write(obj);
        return node;
    }

    /// <summary>
    /// Remove the specified child from the list. Returns the parent node of the removed node if successful.
    /// </summary>

    public DataNode RemoveHierarchy(string path) { return SetHierarchy(path, null); }

    /// <summary>
    /// Retrieve a child by name, optionally creating a new one if the child doesn't already exist.
    /// </summary>

    public DataNode GetChild(string name, bool createIfMissing = false)
    {
        for (int i = 0; i < children.Count; ++i)
            if (children[i].name == name)
                return children[i];

        if (createIfMissing) return AddChild(name);
        return null;
    }

    /// <summary>
    /// Get the value of the existing child.
    /// </summary>

    public T GetChild<T>(string name)
    {
        DataNode node = GetChild(name);
        if (node == null) return default(T);
        return node.Get<T>();
    }

    /// <summary>
    /// Get the value of the existing child or the default value if the child is not present.
    /// </summary>

    public T GetChild<T>(string name, T defaultValue)
    {
        DataNode node = GetChild(name);
        if (node == null) return defaultValue;
        return node.Get<T>();
    }

    /// <summary>
    /// Remove the specified child from the list.
    /// </summary>

    public bool RemoveChild(string name)
    {
        for (int i = 0; i < children.Count; ++i)
        {
            if (children[i].name == name)
            {
                children.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Clone the DataNode, creating a copy.
    /// </summary>

    public DataNode Clone()
    {
        DataNode copy = new DataNode(name);
        copy.mValue = mValue;
        copy.mResolved = mResolved;
        for (int i = 0; i < children.Count; ++i)
            copy.children.Add(children[i].Clone());
        return copy;
    }

    #region Serialization








    /// <summary>
    /// Read the node hierarchy from the specified buffer.
    /// </summary>



    /// <summary>
    /// Just here for consistency.
    /// </summary>








    /// <summary>
    /// Merge the current data with the specified. Returns whether some node's value was replaced.
    /// </summary>

    public bool Merge(DataNode other, bool replaceExisting = true)
    {
        bool replaced = false;

        if (other != null)
        {
            if (replaceExisting || value == null)
            {
                if (value != null && other.value != null) replaced = true;
                value = other.value;
            }

            for (int i = 0; i < other.children.Count; ++i)
            {
                DataNode child = other.children[i];
                replaced |= GetChild(child.name, true).Merge(child, replaceExisting);
            }
        }
        return replaced;
    }

    /// <summary>
    /// Convenience function for easy debugging -- convert the entire data into the string representation form.
    /// </summary>

    public override string ToString()
    {
        if (!isSerializable) return "";
        //   MemoryStream stream = new MemoryStream();

        string text = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        //    stream.Close();
        return text;
    }

    /// <summary>
    /// Convert the DataNode into a binary array of specified type.
    /// </summary>

    public byte[] ToArray()
    {
        MemoryStream stream = new MemoryStream();
        string text = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        var byt = System.Text.Encoding.UTF8.GetBytes(text);


        byte[] data = byt;

        return data;
    }
    #endregion
    #region Private Functions

    /// <summary>
    /// Process the string values, converting them to proper objects.
    /// Returns whether child nodes should be processed in turn.
    /// </summary>


    #endregion
    #region Static Helper Functions

    /// <summary>
    /// Get the next line from the stream reader.
    /// </summary>

    static string GetNextLine(TextReader reader)
    {
        string line = reader.ReadLine();

        while (line != null && line.Trim().StartsWith("//"))
        {
            line = reader.ReadLine();
            if (line == null) return null;
        }
        return line;
    }

    /// <summary>
    /// Calculate the number of tabs at the beginning of the line.
    /// </summary>

    static int CalculateTabs(string line)
    {
        if (line != null)
        {
            for (int i = 0; i < line.Length; ++i)
            {
                if (line[i] == '\t') continue;
                return i;
            }
        }
        return 0;
    }
    #endregion
}
}
