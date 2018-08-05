using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNet.Coms{
  public  class TNBehaviour:EcsRx.Components.IComponent
    {
        public TNBehaviour()
        {
            starts.Add(Start);
            updates.Add(Update);
            Init();

        }

        public static System.Collections.Concurrent.ConcurrentBag<Action> starts = new System.Collections.Concurrent.ConcurrentBag<Action>();

        public static System.Collections.Concurrent.ConcurrentBag<Action> updates = new System.Collections.Concurrent.ConcurrentBag<Action>();
        public virtual void Start()
        {
         
        }
        public virtual void Update()
        {
         
        }
         static object obj = new object();
        private static bool IsInit;
        public static void Init()
        {
            if (IsInit) return;
            System.Threading.Tasks.Task.Run(async () => { while (true) { Execute(); await System.Threading.Tasks.Task.Delay(1); } });        }
        public static void Execute()
        {
            IsInit = true;
            var st = starts.TryTake(out var action);
            if (st)
            {
                action.Invoke();
            }


            lock (obj)
            {
                foreach (var u in updates)
                {
                    u.Invoke();
                }
            }
        }
    
    }
}
