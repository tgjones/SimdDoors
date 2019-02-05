using System;
using SimdDoors.Implementations;
using Xunit;

namespace SimdDoors.Tests
{
    public class Tests : IDisposable
    {
        private World _world;
        public Tests()
        {
            _world = new World(numCharacters: 32, numDoors: 96);
        }

        [Fact]
        public void Oop()
        {
            OopImplementation.Run(_world);
            Assert.Equal(15, _world.GetNumOpenDoors());
        }

        [Fact]
        public void Simd()
        {
            SimdImplementation.Run(_world);
            Assert.Equal(15, _world.GetNumOpenDoors());
        }
        public void Dispose()
        {
            _world.Dispose();
        }
    }
}
