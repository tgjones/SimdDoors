using System.Numerics;

namespace SimdDoors.Components
{
    public class Component
    {
        internal Actor Actor;

        public Vector3 Position => Actor.Position;
    }
}
