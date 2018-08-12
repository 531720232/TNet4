using System;
using TNet.Script;

namespace Game.Script
{

  [AutoRun]
    public class Class1
    {
public static void Main(string[] args)
        {
            Console.WriteLine("f25");
        }
    }

    
    public class Action2017 : TNet.Service.BaseStruct
    {
        public Action2017(TNet.Service.ActionGetter get) :base(11,get)
        {
          
        }
        public override bool TakeAction()
        {
            Console.WriteLine("f25");
            return true;
        }
    }
}

