using System;
using System.Collections.Generic;
using System.Text;

namespace Mud.Shell
{
    public class Env : IEnv
    {
        public IDisplay Display { get; set; }
        public IInputCommand Input { get ; set ; }

        public static Dictionary<string, object> Globals { get; set; } = new Dictionary<string, object>();
        public void Dispose()
        {
        
        }

    }
}
