﻿using System;
using System.Runtime.InteropServices;


namespace ZopfliSharp.Internal
{
    /// <summary>
    /// <para><see cref="SafeHandle"/> for malloced memory.</para>
    /// <para>Free memory using <see cref="Marshal.FreeCoTaskMem(IntPtr)"/>.</para>
    /// </summary>
    internal sealed class MallocedMemoryHandle : SafeHandle
    {
        /// <summary>
        /// Initialize with null pointer (<see cref="IntPtr.Zero"/>).
        /// </summary>
        private MallocedMemoryHandle()
            : base(IntPtr.Zero, true)
        {
        }


        /// <summary>
        /// True if the memory is not allocated (null pointer), otherwise false.
        /// </summary>
        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
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
