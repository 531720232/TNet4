using System;
using System.Collections.Generic;
using System.Text;
using Mud.Shell;
using Mud.Utils;
using TNet.RPC.IO;

namespace Mud.Commands
{
    [Help(
      Category = "Shell",
      Name = "Connect",
      Syntax = "Connect <服务器地址>",
      Description = "链接一个Mud服务器",
      Examples = new string[] {
            "Connect 127.0.0.1:9001"
      }
  )]
    public class Connect : IShellCommand
    {
        public static SocketRemoteClient socket;
        public void Execute(StringScanner s, IEnv env, out bool status, out object obj)
        {
            status = false;
            obj = null;
            var ip = s.Scan(@".+").Trim();
            env.Display.WriteLine(ip);
            var sd = ip.Split(':');
             socket = new SocketRemoteClient(sd[0],TNet.Extend.ObjectExtend.ToInt(sd[1]),1000);
            socket.Connect();
            var pa = new RequestParam();
            pa["ssid"] = Guid.NewGuid();
            pa["actionid"] = 5317;
            pa["msgid"] = 20182222;
            pa["uid"] = 531720232;
            pa["sid"] = 01;
            pa["proxyId"] = 02;
            pa["ptc1"] = 0;

            pa["fyindex"] = 531720232;
            socket.Send(pa.ToPostString());
        }

        public bool IsCommand(StringScanner s)
        {
            return s.Scan(@"Connect\s+").Length > 0;
        }
    }
}
