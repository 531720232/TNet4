using System;
using System.Collections.Generic;
using System.Text;

namespace Mud.Game
{
  public  class BuildingList
    {
        public class BuildingItem
        {
            public string Name;
            public Dictionary<string,int> need_cl;
            public double needss;
            public Action OnFinish;
            public Action OnUpdate;
        }
        public class Building
        {
            public BuildingItem item;
            public bool finish = false;
            public Guid guid;
            public System.Threading.Timer timer;
            public void Destroy()
            {
             
                timer.Dispose();
            }
        }
     
        public List<Building> items { get; set; } = new List<Building>();
        public int Max { get; set; } = 10;
        public  void Build(BuildingItem item)
        {
            if(items.Count>=Max)
            {
                return;
            }

            System.Threading.Tasks.Task.Run(async () => {
            var build = new Building();
            build.guid = Guid.NewGuid();
            build.item = item;
            items.Add(build);
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(item.needss));
            Console.WriteLine(build.guid);
            build.finish = true;
                build.item.OnFinish?.Invoke();
               build.timer = new System.Threading.Timer(Update, build, 0, 333);   
               
            });
          
        }
        private void Update(object ob)
        {
            if(ob is Building building)
            {
                building.item.OnUpdate?.Invoke();
            }
           
        }
    }

}
