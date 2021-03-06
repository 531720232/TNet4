﻿using System;
using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Groups;

namespace EcsRx.Extensions
{
    public static class IDictionaryExtensions
    {
        public static void AddEntityToGroups(this IDictionary<IGroup, IList<IEntity>> groupAccessors, IEntity entity)
        {
            foreach (var group in groupAccessors.Keys)
            {
                if(entity.MatchesGroup(group))
                { groupAccessors[group].Add(entity); }
            }
        }
        
        public static void RemoveAndDispose<T>(this IDictionary<T, IDisposable> disposables, T key)
        {
            disposables[key].Dispose();
            disposables.Remove(key);
        }
    }
}