using System;
using System.Collections.Generic;
using System.Text;


public class Action11 : TNet.Service.BaseStruct
{
  
   public override void BuildPacket()
        {
            this.PushIntoStack(RoleName);

        PushIntoStack(RoleIndex != -1 ? RoleIndex : 0);
        PushIntoStack(TNet.Extend.RandomUtils.GetRandom(1, 100));
        PushIntoStack(140);
        PushIntoStack(140);
        PushIntoStack(100);
        PushIntoStack(100);
        PushIntoStack(0);
      
        
        //this.PushIntoStack("六道轮回之心我认为台湾他");

    }
    public string RoleName;
    public string TK;
    public int RoleIndex;
    public override bool GetUrlElement()
    {
        if (httpGet.GetString("RN", ref RoleName) && httpGet.GetString("TK", ref TK) && httpGet.GetInt("RR", ref RoleIndex))
           
          
        {
          
        }
        else
        {
            return false;
        }
     
        return true;
    }

    public Action11(TNet.Service.ActionGetter get) : base(11, get)
    { 

    }
    public override bool TakeAction()
    {
  
        Console.WriteLine("tianshen xin ");
        return true;
    }
}

