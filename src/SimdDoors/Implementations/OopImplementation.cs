using SimdDoors.Components;

namespace SimdDoors.Implementations
{
    public static class OopImplementation
    {
        public static void Run(World world)
        {
            foreach (var door in world.AllDoors)
            {
                var doorComponent = door.FindComponent<DoorComponent>();
                doorComponent.Update(world);
            }
        }
    }
}
