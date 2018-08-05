
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace TNet
{
    public class DocumentStoreHolder
    {
  
    }

    class Program
    {
        public string this[string c]
        {
            get { return null; }
        }
        static object STR=new object();
        static async System.Threading.Tasks.Task Tell(string str)
        {
          
                string a = "";
                foreach (var s in str.ToCharArray())
                {
                    a += s;
                    Console.Clear();
                    Console.WriteLine(a);
               await    System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.12f));
                }
                return;
            
        }
        static void Main(string[] args)
        {
  

         
        
       
            var app =new  TNet.Runtime.TNServer_AppInConsole();
            app.StartApplication();

          
     
            while (true)
            {
              

             
            }

           
                

        }

     
    
     

      
    }
    //public class Game_Start : EcsRxConsoleApplication
    //{
    //    protected override void ApplicationStarted()
    //    {

    //    }
    //    protected override void ApplicationStarting()
    //    {


    //    }

    //    }
}
