using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNet.Com
{
  public  interface IServer:EcsRx.Components.IComponent
    {
        EcsRx.Entities.IEntity Modules { get; set; }
        bool InitializeModules();
        void SetHandler(string id,TNet.Service.BaseStruct baseStruct);

    }
}
