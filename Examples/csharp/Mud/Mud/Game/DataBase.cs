using System;
using System.Collections.Generic;
using System.Text;

namespace Mud.Game
{
  public  static class DataBase
    {
      public static List<BuildingList.BuildingItem> buildings { get; set; }
        static DataBase()
        {
            buildings = new List<BuildingList.BuildingItem>();
            buildings.Add(new BuildingList.BuildingItem { Name = "1级仓库", needss = 5,OnFinish=()=> { Console.WriteLine("仓库建造完毕"); },OnUpdate=()=> {  } });

        }
    }
}
