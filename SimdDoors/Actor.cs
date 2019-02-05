using System;
using System.Collections.Generic;
using System.Numerics;
using SimdDoors.Components;

namespace SimdDoors
{
    public class Actor
    {
        private readonly Dictionary<Type, Component> _components = new Dictionary<Type, Component>();

        public readonly Vector3 Position;

        public Actor(in Vector3 position, params Component[] components)
        {
            Position = position;

            foreach (var component in components)
            {
                _components.Add(component.GetType(), component);
                component.Actor = this;
            }
        }

        public T FindComponent<T>()
            where T : Component
        {
            _components.TryGetValue(typeof(T), out var result);
            return (T) result;
        }
    }
}
