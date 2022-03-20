using System;
using System.Runtime.InteropServices;


namespace ZopfliSharp.Internal
{
    /// <summary>
    /// <para><see cref="SafeBuffer"/> for malloced memory.</para>
    /// <para>Free memory using <see cref="Marshal.FreeCoTaskMem(IntPtr)"/>.</para>
    /// </summary>
    internal sealed class MallocedMemoryHandle : SafeBuffer
    {
        /// <summary>
        /// Initialize with null pointer (<see cref="IntPtr.Zero"/>).
        /// </summary>
        internal MallocedMemoryHandle()
            : base(true)
        {
        }

        /// <summary>
        /// Allocate memory and initialize handle with it.
        /// </summary>
        /// <param name="size">Size of memory.</param>
        internal MallocedMemoryHandle(int size)
            : base(true)
        {
            handle = Marshal.AllocCoTaskMem(size);
            Initialize((ulong)size);
        }


        /// <summary>
        /// True if the memory is not allocated (null pointer), otherwise false.
        /// </summary>
        public override bool IsInvalid => handle == IntPtr.Zero;


        /// <summary>
        /// Reallocate memory.
        /// </summary>
        /// <param name="size">Size of memory.</param>
        public void ReAlloc(int size)
        {
            handle = Marshal.ReAllocCoTaskMem(handle, size);
            Initialize((ulong)size);
        }


        /// <summary>
        /// Free malloced memory using <see cref="Marshal.FreeCoTaskMem(IntPtr)"/>.
        /// </summary>
        /// <returns>True if freeing is successful, otherwise false</returns>
        protected override bool ReleaseHandle()
        {
            Marshal.FreeCoTaskMem(handle);
            return true;
        }
    }
}
