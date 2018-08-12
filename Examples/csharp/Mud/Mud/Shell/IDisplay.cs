using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mud.Shell
{
  public  interface IDisplay
    {
        List<TextWriter> TextWriters { get; set; }
        bool Pretty { get; set; }
        void WriteWelcome();
        void WritePrompt(string text);
        void WriteInfo(string text);
        void WriteError(Exception ex);
        void WriteResult(IEnumerable<object> results);

        #region Print public methods

        void Write(string text);


        void WriteLine(string text);


        void WriteLine(ConsoleColor color, string text);


        void  Write(ConsoleColor color, string text);
        void WriteLine(ConsoleColor darkCyan, object p);


        #endregion
    }

}
