using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace ZopfliSharp.Internal
{
    /// <summary>
    /// <para>Option value for zopflipng.</para>
    /// <para>This structure is used to interact with zopflipng.dll.</para>
    /// </summary>
    /// <seealso cref="ZopfliPng.UnsafeNativeMethods.CZopfliPNGSetDefaults(out CZopfliPNGOptions)"/>
    /// <seealso cref="ZopfliPng.UnsafeNativeMethods.CZopfliPNGOptimize(IntPtr, UIntPtr, in CZopfliPNGOptions, bool, out MallocedMemoryHandle, out UIntPtr)"/>
    [StructLayout(LayoutKind.Sequential)]
    internal struct CZopfliPNGOptions : IDisposable
    {
        /// <summary>
        /// Default value for <see cref="LossyTransparent"/>.
        /// </summary>
        public const bool DefaultLossyTransparent = false;
        /// <summary>
        /// Default value for <see cref="Lossy8bit"/>.
        /// </summary>
        public const bool DefaultLossy8bit = false;
        /// <summary>
        /// Default value for <see cref="AutoFilterStrategy"/>.
        /// </summary>
        public const bool DefaultAutoFilterStrategy = true;
        /// <summary>
        /// Default value for <see cref="KeepColorType"/>.
        /// </summary>
        public const bool DefaultKeepColorType = false;
        /// <summary>
        /// Default value for <see cref="UseZopfli"/>.
        /// </summary>
        public const bool DefaultUseZopfli = true;
        /// <summary>
        /// Default value for <see cref="NumIterations"/>.
        /// </summary>
        public const int DefaultNumIterations = 15;
        /// <summary>
        /// Default value for <see cref="NumIterationsLarge"/>.
        /// </summary>
        public const int DefaultNumIterationsLarge = 5;


        /// <summary>
        /// Allow altering hidden colors of fully transparent pixels.
        /// </summary>
        public bool LossyTransparent { get; set; }
        /// <summary>
        /// Convert 16-bit per channel images to 8-bit per channel.
        /// </summary>
        public bool Lossy8bit { get; set; }
        /// <summary>
        /// Filter strategies to try.
        /// </summary>
        public IntPtr FilterStrategiesPointer { get; private set; }
        /// <summary>
        /// How many strategies to try.
        /// </summary>
        public int NumFilterStrategies { get; private set; }
        /// <summary>
        /// Automatically choose filter strategy using less good compression.
        /// </summary>
        public bool AutoFilterStrategy { get; set; }
        /// <summary>
        /// Keep original color type (RGB, RGBA, gray, gray+alpha or palette) and bit depth of the PNG.
        /// </summary>
        public bool KeepColorType { get; set; }
        /// <summary>
        /// <para>PNG chunks to keep</para>
        /// <para>chunks to literally copy over from the original PNG to the resulting one.</para>
        /// </summary>
        public IntPtr KeepChunksPointer { get; private set; }
        /// <summary>
        /// How many entries in keepchunks.
        /// </summary>
        public int NumKeepChunks { get; private set; }
        /// <summary>
        /// Use Zopfli deflate compression.
        /// </summary>
        public bool UseZopfli { get; set; }
        /// <summary>
        /// Zopfli number of iterations.
        /// </summary>
        public int NumIterations { get; set; }
        /// <summary>
        /// Zopfli number of iterations on large images.
        /// </summary>
        public int NumIterationsLarge { get; set; }
        /// <summary>
        /// Unused, left for backwards compatiblity.
        /// </summary>
        private readonly int _blockSplitStrategy;


        /// <summary>
        /// Create option instance for zopflipng with default parameters.
        /// </summary>
        /// <param name="lossyTransparent">Allow altering hidden colors of fully transparent pixels.</param>
        /// <param name="lossy8bit">Convert 16-bit per channel images to 8-bit per channel.</param>
        /// <param name="autoFilterStrategy">Automatically choose filter strategy using less good compression.</param>
        /// <param name="keepColorType">Keep original color type (RGB, RGBA, gray, gray+alpha or palette) and bit depth of the PNG.</param>
        /// <param name="useZopfli">Use Zopfli deflate compression.</param>
        /// <param name="numIterations">Zopfli number of iterations.</param>
        /// <param name="numIterationsLarge">Zopfli number of iterations on large images.</param>
        public CZopfliPNGOptions(
            bool lossyTransparent = DefaultLossyTransparent,
            bool lossy8bit = DefaultLossy8bit,
            bool autoFilterStrategy = DefaultAutoFilterStrategy,
            bool keepColorType = DefaultKeepColorType,
            bool useZopfli = DefaultUseZopfli,
            int numIterations = DefaultNumIterations,
            int numIterationsLarge = DefaultNumIterationsLarge)
        {
            LossyTransparent = lossyTransparent;
            Lossy8bit = lossy8bit;
            FilterStrategiesPointer = IntPtr.Zero;
            NumFilterStrategies = 0;
            AutoFilterStrategy = autoFilterStrategy;
            KeepColorType = keepColorType;
            KeepChunksPointer = IntPtr.Zero;
            NumKeepChunks = 0;
            UseZopfli = useZopfli;
            NumIterations = numIterations;
            NumIterationsLarge = numIterationsLarge;
            _blockSplitStrategy = 0;
        }


        /// <summary>
        /// Create option instance from <see cref="ZopfliPNGOptions"/>.
        /// </summary>
        /// <param name="pngOptions">Instance of <see cref="ZopfliPNGOptions"/>.</param>
        public CZopfliPNGOptions(ZopfliPNGOptions pngOptions)
            : this(
                pngOptions.LossyTransparent,
                pngOptions.Lossy8bit,
                pngOptions.AutoFilterStrategy,
                pngOptions.KeepColorType,
                pngOptions.UseZopfli,
                pngOptions.NumIterations,
                pngOptions.NumIterationsLarge)
        {
            SetFilterStrategies(pngOptions.FilterStrategies);
            SetKeepChunks(pngOptions.KeepChunks);
        }


        /// <summary>
        /// Set <see cref="FilterStrategiesPointer"/> and <see cref="NumFilterStrategies"/> from a list.
        /// </summary>
        /// <param name="filterStrategies">List of filter strategies.</param>
        public void SetFilterStrategies(List<ZopfliPNGFilterStrategy> filterStrategies)
        {
            DisposeFilterStrategies();
            (FilterStrategiesPointer, NumFilterStrategies) = CreateFilterStrategies(filterStrategies);
        }


        /// <summary>
        /// Set <see cref="KeepChunksPointer"/> and <see cref="NumKeepChunks"/> from a list.
        /// </summary>
        /// <param name="keepChunks">List of chunk names.</param>
        public void SetKeepChunks(List<string> keepChunks)
        {
            DisposeKeepChunks();
            (KeepChunksPointer, NumKeepChunks) = CreateKeepChunks(keepChunks);
        }


        /// <summary>
        /// <para>Free the allocated memory for <see cref="FilterStrategiesPointer"/> and set to <see cref="IntPtr.Zero"/>.</para>
        /// <para>Similarly, set <see cref="NumFilterStrategies"/> to 0.</para>
        /// </summary>
        public void DisposeFilterStrategies()
        {
            if (FilterStrategiesPointer != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(FilterStrategiesPointer);
                FilterStrategiesPointer = IntPtr.Zero;
            }
            NumFilterStrategies = 0;
        }


        /// <summary>
        /// <para>Free the allocated memory for <see cref="KeepChunksPointer"/> and set to <see cref="IntPtr.Zero"/>.</para>
        /// <para>Similarly, set <see cref="NumKeepChunks"/> to 0.</para>
        /// </summary>
        public void DisposeKeepChunks()
        {
            if (KeepChunksPointer != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(KeepChunksPointer);
                KeepChunksPointer = IntPtr.Zero;
            }
            NumKeepChunks = 0;
        }


        /// <summary>
        /// Dispose resource of <see cref="FilterStrategiesPointer"/>.
        /// </summary>
        public void Dispose()
        {
            DisposeFilterStrategies();
            DisposeKeepChunks();
        }


        /// <summary>
        /// Get default option value.
        /// </summary>
        /// <returns>Default option value.</returns>
        public static CZopfliPNGOptions GetDefault()
        {
            ZopfliPng.UnsafeNativeMethods.CZopfliPNGSetDefaults(out var cPngOptions);
            return cPngOptions;
        }


        /// <summary>
        /// Allocate and write filter strategies data.
        /// </summary>
        /// <param name="filterStrategies">List of filter strategies.</param>
        /// <returns>Tuple of pointer to the filter strategies and the number of them.</returns>
        private static (IntPtr FilterStrategiesPointer, int NumFilterStrategies) CreateFilterStrategies(List<ZopfliPNGFilterStrategy> filterStrategies)
        {
            if (filterStrategies == null || filterStrategies.Count == 0)
            {
                return (IntPtr.Zero, 0);
            }

            var filterStrategiesCount = filterStrategies.Count;
            var filterStrategiesPointer = Marshal.AllocCoTaskMem(sizeof(ZopfliPNGFilterStrategy) * filterStrategiesCount);
            var numFilterStrategies = filterStrategies.Count;

            unsafe
            {
                var p = (ZopfliPNGFilterStrategy*)filterStrategiesPointer;
                for (int i = 0; i < filterStrategiesCount; i++)
                {
                    p[i] = filterStrategies[i];
                }
            }

            return (filterStrategiesPointer, numFilterStrategies);
        }


        /// <summary>
        /// Allocate and write chunk names to keep.
        /// </summary>
        /// <param name="keepChunks">List of chunk names.</param>
        /// <returns>Tuple of pointer to the chunk names and the number of them.</returns>
        private static (IntPtr KeepChunksPointer, int NumKeepChunks) CreateKeepChunks(List<string> keepChunks)
        {
            if (keepChunks == null || keepChunks.Count == 0)
            {
                return (IntPtr.Zero, 0);
            }

            unsafe
            {
                var memorySize = keepChunks.Aggregate(0, (acc, keepChunk) => acc + (keepChunk.Length + 1) * sizeof(char));

                // Memory:
                //   p_i: Pointer to c_i.
                //   c_i: Null-terminate string.
                //   [p_0][p_1][p_2]...[p_n][c_0][c_1][c_2]...[c_n]
                var keepChunksCount = keepChunks.Count;
                var keepChunksPointer = Marshal.AllocCoTaskMem(keepChunksCount * sizeof(byte*) + memorySize);

                var p = (byte**)keepChunksPointer;
                var q = (byte*)(&p[keepChunksCount]);
                for (int i = 0; i < keepChunksCount; i++)
                {
                    p[i] = q;
                    foreach (var c in Encoding.ASCII.GetBytes(keepChunks[i]))
                    {
                        *q++ = c;
                    }
                    *q++ = 0;  // Null-terminate
                }

                return (keepChunksPointer, keepChunksCount);
            }
        }
    }
}
