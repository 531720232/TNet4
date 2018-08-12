using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mud.Shell
{
  public  interface IInputCommand
    {
         Queue<string> Queue { get; set; }
         List<string> History { get; set; }
         Stopwatch Timer { get; set; }
         bool Running { get; set; }
         bool AutoExit { get; set; }

         Action<string> OnWrite { get; set; }

      

        string ReadCommand();
       

        /// <summary>
        /// Read a line from queue or user
        /// </summary>
        string ReadLine();
    

        void Write(string text);
       
    }
}
