using Mud.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mud.Commands
{
    public interface IShellCommand
    {
        bool IsCommand(StringScanner s);

        void Execute(StringScanner s, Shell.IEnv env, out bool status, out object obj);
    }
}
