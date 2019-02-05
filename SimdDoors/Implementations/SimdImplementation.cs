using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using SimdDoors.Components;

namespace SimdDoors.Implementations
{
    public static class SimdImplementation
    {
        public unsafe static void Run(World world)
        {
            if (!Sse.IsSupported || !Sse2.IsSupported)
            {
                throw new Exception("Your processor must support SSE and SSE2 to run this.");
            }

            var charCount = world.AllCharacters.Count;
            var chars = stackalloc CharData[charCount];

            for (var i = 0; i < charCount; i++)
            {
                var characterActor = world.AllCharacters[i];
                var allegianceComponent = characterActor.FindComponent<AllegianceComponent>();

                chars[i] = new CharData
                {
                    X = characterActor.Position.X,
                    Y = characterActor.Position.Y,
                    Z = characterActor.Position.Z,
                    Allegiance = allegianceComponent.Allegiance
                };
            }

            var doorData = world.DoorData;

            var doorCount = doorData.Count;
            for (var d = 0; d < doorCount; d += 4)
            {
                var doorX = Sse.LoadAlignedVector128(doorData.X.AlignedPointer + d);
                var doorY = Sse.LoadAlignedVector128(doorData.Y.AlignedPointer + d);
                var doorZ = Sse.LoadAlignedVector128(doorData.Z.AlignedPointer + d);
                var doorR2 = Sse.LoadAlignedVector128(doorData.RadiusSquared.AlignedPointer + d);
                var doorA = Sse2.LoadAlignedVector128(doorData.Allegiance.AlignedPointer + d);

                var state = Vector128<uint>.Zero;

                for (var cc = 0; cc < charCount; cc++)
                {
                    ref var c = ref chars[cc];

                    var charX = Vector128.Create(c.X);
                    var charY = Vector128.Create(c.Y);
                    var charZ = Vector128.Create(c.Z);
                    var charA = Vector128.Create(c.Allegiance);

                    var ddx = Sse.Subtract(doorX, charX);
                    var ddy = Sse.Subtract(doorY, charY);
                    var ddz = Sse.Subtract(doorZ, charZ);
                    var dtx = Sse.Multiply(ddx, ddx);
                    var dty = Sse.Multiply(ddy, ddy);
                    var dtz = Sse.Multiply(ddz, ddz);
                    var dst2 = Sse.Add(Sse.Add(dtx, dty), dtz);

                    var rmask = Sse.CompareLessThanOrEqual(dst2, doorR2);
                    var amask = Sse2.CompareEqual(charA, doorA);
                    var mask = Sse2.And(rmask.AsUInt32(), amask);

                    state = Sse2.Or(mask, state);
                }

                Sse2.StoreAligned(doorData.ShouldBeOpen.AlignedPointer + d, state);
            }

            for (var i = 0; i < doorCount; i++)
            {
                var isOpen = *(doorData.ShouldBeOpen.AlignedPointer + i) != 0;
                world.AllDoors[i].FindComponent<DoorComponent>().IsOpen = isOpen;
            }
        }
    }
}
