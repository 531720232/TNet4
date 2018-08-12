using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mud.Shell
{
    public class Display:IDisplay
    {
        public List<TextWriter> TextWriters { get; set; }
        public bool Pretty { get; set; }

        public Display()
        {
            this.TextWriters = new List<TextWriter>();
            this.Pretty = false;
            version = new Version(1, 2, 3, 4);
        }

        Version version { get; set; }
        public void WriteWelcome()
        {
            this.WriteInfo("你即将进入末世，希望能再次见到你！");
           
            this.WriteInfo("当前版本"+version);
            this.WriteInfo("输入 `help`, `help full` or `help <command>`获得全命令");
         
            this.WriteInfo("");
        }

        public void WritePrompt(string text)
        {
            this.Write(ConsoleColor.White, text);
        }

        public void WriteInfo(string text)
        {
            this.WriteLine(ConsoleColor.Gray, text);
        }

        public void WriteError(Exception ex)
        {
            this.WriteLine(ConsoleColor.Red, ex.Message);

          
        }

        public void WriteResult(IEnumerable<object> results)
        {
            var index = 0;

            foreach (var result in results)
            {
                this.Write(ConsoleColor.Cyan, string.Format("[{0}]: ", ++index));

                this.Write(ConsoleColor.Cyan, string.Format("[{0}]: ", result));
            }
        }

        #region Print public methods

        public void Write(string text)
        {
            this.Write(Console.ForegroundColor, text);
        }

        public void WriteLine(string text)
        {
            this.WriteLine(Console.ForegroundColor, text);
        }

        public void WriteLine(ConsoleColor color, string text)
        {
            this.Write(color, text + Environment.NewLine);
        }

        public void Write(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;

            foreach (var writer in this.TextWriters)
            {
                writer.Write((text));
            }
        }

        public void WriteLine(ConsoleColor darkCyan, object p)
        {
            WriteLine(p.ToString());
        }

        #endregion
    }
}
