﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Entities;

namespace TNet.Coms
{
  public  class NameShowed:EcsRx.Components.IComponent
    {
       public string Name { get; set; }
        public IEntity Entity { get; set; }
    }
}
