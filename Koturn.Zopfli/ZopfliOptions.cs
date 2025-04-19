#if NET7_0_OR_GREATER
#    define RUNTIME_MARSHALLING_DISABLED
#endif

using System;
using System.Runtime.InteropServices;
using Koturn.Zopfli.Enums;


namespace Koturn.Zopfli
{
    /// <summary>
    /// <para>Option value for zopflipng.</para>
    /// <para>This structure is used to interact with zopfli.dll.</para>
    /// </summary>
    /// <seealso cref="Zopfli.SafeNativeMethods.ZopfliInitOptions(out ZopfliOptions)"/>
    /// <seealso cref="Zopfli.SafeNativeMethods.ZopfliCompress(in ZopfliOptions, ZopfliFormat, IntPtr, UIntPtr, out Internal.MallocedMemoryHandle, out UIntPtr)"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct ZopfliOptions
    {
        /// <summary>
        /// Whether to print output
        /// </summary>
#if RUNTIME_MARSHALLING_DISABLED
        public bool Verbose
        {
            readonly get => _verbose != 0;
            set => _verbose = value ? 1 : 0;
        }
        /// <summary>
        /// Actual value of <see cref="Verbose"/>.
        /// </summary>
        private int _verbose;
#else
        [field: MarshalAs(UnmanagedType.Bool)]
        public bool Verbose { get; set; }
#endif  // RUNTIME_MARSHALLING_DISABLED
        /// <summary>
        /// Whether to print more detailed output
        /// </summary>
#if RUNTIME_MARSHALLING_DISABLED
        public bool VerboseMore
        {
            readonly get => _verboseMore != 0;
            set => _verboseMore = value ? 1 : 0;
        }
        /// <summary>
        /// Actual value of <see cref="VerboseMore"/>.
        /// </summary>
        private int _verboseMore;
#else
        [field: MarshalAs(UnmanagedType.Bool)]
        public bool VerboseMore { get; set; }
#endif  // RUNTIME_MARSHALLING_DISABLED
        /// <summary>
        /// <para>Maximum amount of times to rerun forward and backward pass to optimize LZ77 compression cost.</para>
        /// <para>Good values: 10, 15 for small files, 5 for files over several MB in size or it will be too slow.</para>
        /// </summary>
        public int NumIterations { get; set; }
        /// <summary>
        /// <para>If true, splits the data in multiple deflate blocks with optimal choice for the block boundaries.</para>
        /// <para>Block splitting gives better compression.</para>
        /// <para>Default: true (1).</para>
        /// </summary>
#if RUNTIME_MARSHALLING_DISABLED
        public bool BlockSplitting
        {
            readonly get => _blockSplitting != 0;
            set => _blockSplitting = value ? 1 : 0;
        }
        /// <summary>
        /// Actual value of <see cref="BlockSplitting"/>.
        /// </summary>
        private int _blockSplitting;
#else
        [field: MarshalAs(UnmanagedType.Bool)]
        public bool BlockSplitting { get; set; }
#endif  // RUNTIME_MARSHALLING_DISABLED
        /// <summary>
        /// No longer used, left for compatibility.
        /// </summary>
#if RUNTIME_MARSHALLING_DISABLED
        private readonly int _blockSplittingLast;
#else
        [field: MarshalAs(UnmanagedType.Bool)]
        private readonly bool _blockSplittingLast;
#endif  // RUNTIME_MARSHALLING_DISABLED
        /// <summary>
        /// <para>Maximum amount of blocks to split into (0 for unlimited, but this can give extreme results that hurt compression on some files).</para>
        /// <para>Default value: 15.</para>
        /// </summary>
        public int BlockSplittingMax { get; set; }


        /// <summary>
        /// Get default option value.
        /// </summary>
        /// <returns>Default option value.</returns>
        public static ZopfliOptions GetDefault()
        {
            Zopfli.SafeNativeMethods.ZopfliInitOptions(out var options);
            return options;
        }
    }
}
