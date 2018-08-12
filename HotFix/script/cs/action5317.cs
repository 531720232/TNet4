using System;
using System.Collections.Generic;
using System.Text;


public class Action5317 : TNet.Service.BaseStruct
{
  
   public override void BuildPacket()
        {
            this.PushIntoStack(5317);
    
        this.PushIntoStack("六道轮回之心我认为台湾他");

        }
    public string fay;
    public override bool GetUrlElement()
    {
        if (httpGet.GetString("a", ref fay))
           
          
        {
            Console.WriteLine($"->{fay}");
        }
        else
        {
            return false;
        }
        return true;
    }

    public Action5317(TNet.Service.ActionGetter get) : base(5317, get)
    { 

    }
    public override bool TakeAction()
    {
  
        Console.WriteLine("tianshen xin ");
        return true;
    }
}

