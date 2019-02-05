using System;
using System.Collections.Generic;
using SimdDoors.Components;

namespace SimdDoors
{
    public class World : IDisposable
    {
        public readonly List<Actor> AllCharacters = new List<Actor>();
        public readonly List<Actor> AllDoors = new List<Actor>();

        public DoorData DoorData;

        public World(int numCharacters, int numDoors)
        {
            var random = new Random(600);

            const int range = 30;
            const int numTeams = 3;

            // Create characters.
            for (var i = 0; i < numCharacters; i++)
            {
                AllCharacters.Add(new Actor(
                    Vector3Utility.CreateRandomPosition(random, range),
                    new AllegianceComponent
                    {
                        Allegiance = (uint) random.Next(numTeams)
                    }));
            }

            // Create doors.
            for (var i = 0; i < numDoors; i++)
            {
                AllDoors.Add(new Actor(
                    Vector3Utility.CreateRandomPosition(random, range),
                    new DoorComponent
                    {
                        Allegiance = (uint) random.Next(numTeams)
                    }));
            }

            DoorData = new DoorData(numDoors);
            DoorData.Update(AllDoors);
        }

        public int GetNumOpenDoors()
        {
            var openDoors = 0;
            foreach (var door in AllDoors)
            {
                var doorComponent = door.FindComponent<DoorComponent>();
                if (doorComponent.IsOpen)
                {
                    openDoors++;
                }
            }

            return openDoors;
        }

        public void Dispose()
        {
            DoorData.Dispose();
        }
    }
}
