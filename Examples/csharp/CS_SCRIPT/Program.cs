using System;

public class Ls
{
    public Ls(string ab,string cd)
    {

    }

}

namespace CS_SCRIPT
{

    class Program
    {
        static void Main(string[] args)
        {
            var code = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/lib/Os.cs");
            var cs = CSScriptLib.CSScript.Evaluator.LoadCode(code,"531720232","2017");

            Console.WriteLine("Hello World!");
        }
    }
}
