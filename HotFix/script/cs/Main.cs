using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNet.Script;
using TNet.Runtime;
/*▄█▀█●热更TNet.Contract.GameSocketHost*/
public class MainClass : TNet.Contract.GameSocketHost,TNet.Script.IMainScript
{
    public void Start(string[] args)
    {

        Console.WriteLine("动态热更in cs_sharp 2017");
        // base.Start(args);
        var name = TNServer_AppInConsole.Instance.Scene.AddComponent<TNet.Coms.NameShowed>();

        name.Name = "名将三国的游戏场景数据实现";
        //添加socketlistner
      //  TNServer_AppInConsole.Instance.Scene.AddComponent<TNet.Coms.TNServer_Socket>();

    }

    public byte[] ProcessRequest(object package, object param)
    {
        return null;

    }
    public void Stop()
    {

        Console.WriteLine("这是一个美丽的误会3");
    }
    public void ReStart()
    {

        Start(null);
    }


     protected override void OnStartAffer()
     {
     }

    protected override void OnServiceStop()
    {
    GameZone.Stop();
     }
    public static void Show()
    {
        Console.WriteLine("这不是一个美丽的误会");
    }
}



