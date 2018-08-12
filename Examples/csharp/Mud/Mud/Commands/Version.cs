using Mud.Shell;
using Mud.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mud.Commands
{
    [Help(
         Category = "Shell",
         Name = "版本",
         Syntax = "ver",
         Description = "列出当前游戏版本"
     )]
    internal class Version : IShellCommand
    {
        public bool IsCommand(StringScanner s)
        {
            return s.Scan(@"ver(sion)?$").Length > 0;
        }

        public void Execute(StringScanner s, IEnv env, out bool b, out object obj)
        {
            b = false;
            obj = null;
          

            env.Display.WriteLine(new System.Version(1,4).ToString());
        }
    }
}
