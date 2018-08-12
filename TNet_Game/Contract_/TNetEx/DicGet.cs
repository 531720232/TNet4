using System;
using System.Collections.Generic;
using System.Text;
using TNet.Service;

namespace TNet.Contract
{
   public class DicGet: ActionGetter
    {


        public DicGet(RequestPackage package, GameSession session) : base(package, session)
        {
        data=    package.Aode;

        }
        public DataNode data;
        // public Dictionary<string, object> ps = new Dictionary<string, object>();
        public override bool CheckSign()
        {
            return base.CheckSign();
        }
      
        public override bool Get<T>(string aName, ref T rValue)
        {
            return data.GetHierarchy<T>(aName)!=null;
        }

    }
}
