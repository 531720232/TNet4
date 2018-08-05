using System;
using System.Collections.Generic;
using System.Text;
using TNet.Service;

namespace TNet.Context
{
  public static   class TNHandler
    {
        static Dictionary<string, BaseStruct> handlers = new Dictionary<string, BaseStruct>();
       // public static Dictionary<string, string> Handlers = new Dictionary<string, string>();
       public static void Reg(string id, BaseStruct action)
        {
            handlers.TryAdd(id, action);

        }
        public static void Remove(string id)
        {
            try
            {
                handlers.Remove(id, out var action);


            }
            catch
            {
            }
        }
        public static BaseStruct Get(string id)
        {
            BaseStruct action;
            handlers.TryGetValue(id, out action);
            return action;

        }

    }
}
