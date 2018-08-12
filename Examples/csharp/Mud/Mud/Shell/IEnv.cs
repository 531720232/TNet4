using System;
using System.Collections.Generic;
using System.Text;

namespace Mud.Shell
{
   public interface IEnv:IDisposable
    {
       IDisplay Display { get; set; }
        IInputCommand Input { get; set; }


    }
}
