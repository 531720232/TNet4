﻿using System;
using System.Collections.Generic;
using EcsRx.Components;

namespace EcsRx.Entities
{
    public interface IEntity : IDisposable
    {
        Guid Id { get; }
        IEnumerable<IComponent> Components { get; }

        IComponent AddComponent(IComponent component);
        T AddComponent<T>() where T : class, IComponent, new(); 
        void RemoveComponent(IComponent component);
        void RemoveComponent<T>() where T : class, IComponent;
        void RemoveComponents(params IComponent[] components);
        void RemoveAllComponents();
        T GetComponent<T>() where T : class, IComponent;
        IComponent GetComponent(Type componentType);

        bool HasComponent<T>() where T : class, IComponent;
        bool HasComponents(params Type[] component);
        void AddComponents(params IComponent[] components);


        Entities.IEntity Parent { get; set; }
        IEnumerable<IEntity> Children { get; set; }
        void SetParent(IEntity parent);
        void AddChildren(params IEntity[] parent);
        void RemoveChildren(params IEntity[] parent);
    }
}
