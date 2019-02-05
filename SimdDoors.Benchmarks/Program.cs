using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SimdDoors.Implementations;

namespace SimdDoors.Benchmarks
{
    public class Program
    {
        private World _world;

        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Program>();
        }

        [Params(32, 64, 96)]
        public int NumCharacters;

        [Params(64, 128, 256)]
        public int NumDoors;

        [GlobalSetup]
        public void Setup() => _world = new World(NumCharacters, NumDoors);

        [Benchmark]
        public void Oop() => OopImplementation.Run(_world);

        [Benchmark]
        public void Simd() => SimdImplementation.Run(_world);

        [GlobalCleanup]
        public void Cleanup() => _world.Dispose();
    }
}
