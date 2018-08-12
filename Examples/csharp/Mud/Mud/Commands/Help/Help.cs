using Mud.Shell;
using Mud.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mud.Commands.Help
{
    internal class Help : IShellCommand
    {
        public bool IsCommand(StringScanner s)
        {
            return s.Scan(@"help\s*").Length > 0;
        }

        public void Execute(StringScanner s, Shell.IEnv env, out bool b, out object obj)
        {
            b = false;
            obj = null;
            var param = s.Scan(".*");
            var d = env.Display;
            var maxlen = param.Length == 0 ? 100 : 0;

            // getting all HelpAttributes inside assemblies
            var helps = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Select(x => x.GetCustomAttributes(typeof(HelpAttribute), true).FirstOrDefault())
                .Where(x => x != null)
                .Select(x => x as HelpAttribute)
                .ToArray()
                .OrderBy(x => x.Category)
                .ThenBy(x => x.Syntax)
                .ToArray();

            // filter with has param
            if (param.Length > 0 && param != "full")
            {
                var subset = helps.Where(x => x.Name.Contains(param)).ToArray();

                // if no help found, try use description
                if (subset.Length == 0)
                {
                    subset = helps.Where(x => x.Syntax.Contains(param)).ToArray();
                }

                if (subset.Length == 0)
                {
                    d.WriteLine(ConsoleColor.Red, "No help found to: '" + param + "'");
                    return;
                }

                helps = subset;
            }

            d.WriteLine(ConsoleColor.White, "# 游戏全命令");
            var category = "";

            foreach (var help in helps)
            {
                // write category name
                if (category != help.Category)
                {
                    category = help.Category;
                    d.WriteLine("");
                    d.WriteLine(ConsoleColor.DarkYellow, "= " + category);
                    d.WriteLine(ConsoleColor.DarkYellow, "- " + "".PadRight(category.Length, '-'));
                }

                d.WriteLine("");
                d.WriteLine(ConsoleColor.Cyan, "> " + help.Syntax);
                d.WriteLine(ConsoleColor.DarkCyan, "  " + help.Description.MaxLength(maxlen));

                // show examples only when named help command
                if (param.Length > 0)
                {
                    foreach (var example in help.Examples)
                    {
                        d.WriteLine(ConsoleColor.Gray, "  > " + example);
                    }
                }
            }
        }
    }
}
