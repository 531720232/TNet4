using System;
using System.Collections.Generic;
using System.Text;
using Mud.Shell;
using Mud.Utils;

namespace Mud.Commands
{
    [Help(
      Category = "Shell",
      Name = "New",
      Syntax = "New <角色名>",
      Description = "设置一个新的角色名",
      Examples = new string[] {
            "New 龙神无毒"
      }
  )]
    public class NewName : IShellCommand
    {
        public void Execute(StringScanner s, IEnv env, out bool status, out object obj)
        {
            status = false;
             obj = null;
          
            var RoleName = s.Scan(@".+").Trim();
            Env.Globals["RN"] = RoleName;
            env.Display.WriteLine(string.Format("角色已设定名称为{0}", RoleName));

        }

        public bool IsCommand(StringScanner s)
        {
            return s.Scan(@"New\s+").Length > 0;
        }
    }
}
