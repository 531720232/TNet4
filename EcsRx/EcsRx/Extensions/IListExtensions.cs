﻿using System.Collections.Generic;
using System.Linq;
using EcsRx.Entities;
using EcsRx.Executor;

namespace EcsRx.Extensions
{
    public static class IListExtensions
    {
        public static void RemoveAllFrom<T>(this IList<T> list, IEnumerable<T> elementsToRemove)
        {
            foreach (var element in elementsToRemove)
            { list.Remove(element); }
        }
    }
}