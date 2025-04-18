using System;
using System.Runtime.InteropServices;
using Koturn.Zopfli.Enums;


namespace Koturn.Zopfli
{
    /// <summary>
    /// <para>Option value for zopflipng.</para>
    /// <para>This structure is used to interact with zopfli.dll.</para>
    /// </summary>
    /// <seealso cref="Zopfli.UnsafeNativeMethods.ZopfliInitOptions(out ZopfliOptions)"/>
    /// <seealso cref="Zopfli.UnsafeNativeMethods.ZopfliCompress(in ZopfliOptions, ZopfliFormat, IntPtr, UIntPtr, out Internal.MallocedMemoryHandle, out UIntPtr)"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct ZopfliOptions
    {
        /// <summary>
        /// Whether to print output
        /// </summary>
        public bool Verbose { get; set; }
        /// <summary>
        /// Whether to print more detailed output
        /// </summary>
        public bool VerboseMore { get; set; }
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
        public bool BlockSplitting { get; set; }
        /// <summary>
        /// No longer used, left for compatibility.
        /// </summary>
        private readonly bool _blockSplittingLast;
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
            Zopfli.UnsafeNativeMethods.ZopfliInitOptions(out var options);
            return options;
        }
    }
}
