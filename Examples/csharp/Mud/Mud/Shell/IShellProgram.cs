using Mud.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mud.Shell
{
 public   abstract class IShellProgram
    {
  public   abstract    void Start(IInputCommand input, IDisplay display);
        public abstract  void RegisterCommands(List<IShellCommand> commands);
    }
}
