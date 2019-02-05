using System;
using System.Collections.Generic;
using SimdDoors.Components;

namespace SimdDoors
{
    public class DoorData : IDisposable
    {
        public readonly int Count;

        // Input data
        public readonly AlignedMemory<float> X;
        public readonly AlignedMemory<float> Y;
        public readonly AlignedMemory<float> Z;
        public readonly AlignedMemory<float> RadiusSquared;
        public readonly AlignedMemory<uint> Allegiance;

        // Output data
        public readonly AlignedMemory<uint> ShouldBeOpen;

        public DoorData(int doorCount)
        {
            Count = doorCount;

            X = new AlignedMemory<float>(doorCount);
            Y = new AlignedMemory<float>(doorCount);
            Z = new AlignedMemory<float>(doorCount);
            RadiusSquared = new AlignedMemory<float>(doorCount);
            Allegiance = new AlignedMemory<uint>(doorCount);

            ShouldBeOpen = new AlignedMemory<uint>(doorCount);
        }

        public unsafe void Update(IReadOnlyList<Actor> doorActors)
        {
            for (var i = 0; i < doorActors.Count; i++)
            {
                var doorActor = doorActors[i];
                var doorComponent = doorActor.FindComponent<DoorComponent>();

                *(X.AlignedPointer + i) = doorActor.Position.X;
                *(Y.AlignedPointer + i) = doorActor.Position.Y;
                *(Z.AlignedPointer + i) = doorActor.Position.Z;
                *(RadiusSquared.AlignedPointer + i) = doorComponent.OpenDistanceSquared;
                *(Allegiance.AlignedPointer + i) = doorComponent.Allegiance;
            }
        }

        public void Dispose()
        {
            X.Dispose();
            Y.Dispose();
            Z.Dispose();
            RadiusSquared.Dispose();
            Allegiance.Dispose();

            ShouldBeOpen.Dispose();
        }
    }
}
