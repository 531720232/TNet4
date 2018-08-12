using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Client
{
    public abstract class GameAction
    {
        public IO.EndianBinaryReader Reader { get;  set; }
        public int Id { get; set; }
        public  GameAction(int id,IO.EndianBinaryReader reader)
        {
            Id = id;
            Reader = reader;
        }

    }
  public  class ActionFactory
    {
        public static string ActionNamespace = "TNet.Client.{0}";
        public static GameAction Create(int id,IO.EndianBinaryReader reader)
        {
            var action = string.Format(ActionNamespace, id.ToString());
            var type=System.Type.GetType(action);
            if(type!=null)
            {

             return   (GameAction)TNet.Reflect.FastActivator.Create(type,reader);
            }
            
            return null;
        }
    }
}
