using System;
using System.Runtime.InteropServices;

namespace SimdDoors
{
    public unsafe class AlignedMemory<T> : IDisposable
        where T : unmanaged
    {
        private readonly IntPtr _rawPtr;

        public readonly T* AlignedPointer;

        public AlignedMemory(int length)
        {
            const int alignment = 16;

            // Over-allocate by enough bytes to ensure we can align the pointer.
            var sizeInBytes = length * Marshal.SizeOf<T>();
            _rawPtr = Marshal.AllocHGlobal(sizeInBytes + alignment);

            var misalignment = alignment - (_rawPtr.ToInt64() % alignment);
            AlignedPointer = (T*) (_rawPtr.ToInt64() + misalignment);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(_rawPtr);
        }
    }
}
