using System.Numerics;

namespace SimdDoors.Components
{
    public class DoorComponent : Component
    {
        public float OpenDistanceSquared = 5f;
        public uint Allegiance;

        public bool IsOpen;

        public void Update(World world)
        {
            IsOpen = false;

            foreach (var actor in world.AllCharacters)
            {
                var c = actor.FindComponent<AllegianceComponent>();
                if (c != null && c.Allegiance == Allegiance)
                {
                    if (Vector3.DistanceSquared(actor.Position, Position) < OpenDistanceSquared)
                    {
                        IsOpen = true;
                        break;
                    }
                }
            }
        }
    }
}
