using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Coms
{
  public  class TNBehaviourTest:TNBehaviour
    {
        public TNBehaviourTest():base()
        {
        
        }
        public override void Start()
        {
            base.Start();
      
          
        }
        public override void Update()
        {
            base.Update();
           if(Console.ReadKey().Key==ConsoleKey.A)
            {
           
            }
         
        }
    }
}
