using Mud.Commands;
using Mud.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mud.Shell
{
    internal class ShellProgram:IShellProgram
    {
        public override void Start(IInputCommand input, IDisplay display)
        {
            var commands = new List<IShellCommand>();
            var env = new Env { Input = input, Display = display };

            // register commands
            RegisterCommands(commands);

            display.TextWriters.Add(Console.Out);

            // show welcome message
            display.WriteWelcome();

            while (input.Running)
            {
                // read next command from user or queue
                var cmd = input.ReadCommand();

                if (string.IsNullOrEmpty(cmd)) continue;

                try
                {
                    var s = new StringScanner(cmd);

                    var found = false;

                    // first test all shell app commands
                    foreach (var command in commands)
                    {
                        if (!command.IsCommand(s)) continue;

                        command.Execute(s, env, out var a, out var b);
                      
                        found = true;
                        break;
                    }

                    // if not found, try database command
                    if (!found)
                    {

                    //    display.WriteResult(env.Engine.Run(cmd));
                    }
                }
                catch (Exception ex)
                {
                    display.WriteError(ex);
                }
            }
        }

        #region Register all commands

        public override void RegisterCommands(List<IShellCommand> commands)
        {
            var type = typeof(IShellCommand);
            var types = typeof(ShellProgram).Assembly
                .GetTypes()
                .Where(p => type.IsAssignableFrom(p) && p.IsClass);

            foreach (var cmd in types)
            {
                commands.Add(Activator.CreateInstance(cmd) as IShellCommand);
            }
        }

        #endregion
    }
}
