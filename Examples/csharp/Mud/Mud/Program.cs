using MessagePack;
using MessagePack.Resolvers;
using Mud.Shell;
using System;

namespace Mud
{
    class Program
    {
        static Shell.IShellProgram shell;
        static void Main(string[] args)
        {
           var a= new Game.BuildingList();
         var item=   Game.DataBase.buildings[0];
            a.Build(item);

//            var pack = TNet.Extend.PackageHelper.Create_A_Pack(531720232, 10,error:"星空凛");
//            pack.Reset();
//        var head=    pack.ReadHead();

////            // set extensions to default resolver.
////            // set extensions to default resolver.
////            MessagePack.Resolvers.CompositeResolver.RegisterAndSetAsDefault(
////                // enable extension packages first
////                ImmutableCollectionResolver.Instance,
////                ReactivePropertyResolver.Instance,
////                MessagePack.Unity.Extension.UnityBlitResolver.Instance,
////                MessagePack.Unity.UnityResolver.Instance,

////                // finaly use standard(default) resolver
////                StandardResolver.Instance);
////);
     


//            var bs = new TNet.IO.EndianBinaryWriter(TNet.IO.EndianBitConverter.Little, new System.IO.MemoryStream());
//            bs.WriteObj(new TNet.RPC.IO.MessageHead(1,2,"st",0,""));
          
//                var reader = bs.ToReader();
           
//            var rwr = reader.ReadObj<TNet.RPC.IO.MessageHead> ();
         //   var tws = MessagePackSerializer.Typeless.Serialize(pack) ;
            // var t = pack.Pos7zipBuffer();
            // var b = pack.CheckEnable7zip(t);
            shell = new Shell.ShellProgram();
         
            var input = new InputCommand();
            var display = new Display();
            input.Queue.Enqueue("New longshenwudu");
            input.Queue.Enqueue("ver");
            input.Queue.Enqueue("Connect 127.0.0.1:9001");
            shell.Start(input, display);
            


            Console.WriteLine("Hello World!");
        }
    }
}
