using System;
using System.Runtime.InteropServices;
using System.Security;
using ZopfliSharp.Internal;


namespace ZopfliSharp
{
    /// <summary>
    /// P/Invoke methods for zopflipng.dll.
    /// </summary>
    public static class ZopfliPng
    {
        /// <summary>
        /// Native methods.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        internal static class UnsafeNativeMethods
        {
            /// <summary>
            /// Get default parameter of <see cref="ZopfliPNGOptions"/>.
            /// </summary>
            /// <param name="pngOptions">Options struct for ZopfliPNG.</param>
            [DllImport("zopflipng.dll", ExactSpelling = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void CZopfliPNGSetDefaults(out CZopfliPNGOptions pngOptions);


            /// <summary>
            /// Re-compress deflated data in PNG with zopfli algorithm.
            /// </summary>
            /// <param name="origPng">Source PNG binary.</param>
            /// <param name="origpngSize">Size of PNG binary.</param>
            /// <param name="pngOptions">Options for ZopfliPNG.</param>
            /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
            /// <param name="resultPng">Result PNG binary. This data is allocated with malloc in zopflipng.dll and caller of this method have to free the memory.</param>
            /// <param name="resultpngSize">Result PNG binary size.</param>
            /// <returns>Status code. 0 means success, otherwise it means failure.</returns>
            [DllImport("zopflipng.dll", ExactSpelling = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern unsafe int CZopfliPNGOptimize(
                [In] byte[] origPng,
                [In] UIntPtr origpngSize,
                in CZopfliPNGOptions pngOptions,
                [In] bool verbose,
                out MallocedMemoryHandle resultPng,
                out UIntPtr resultpngSize);
        }


        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns>Result PNG binary.</returns>
        public static byte[] OptimizePng(byte[] pngData, bool verbose = false)
        {
            return OptimizePng(pngData, pngData.LongLength, verbose);
        }


        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="pngDataLength">Byte length of <paramref name="pngData"/>.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns>Result PNG binary.</returns>
        public static byte[] OptimizePng(byte[] pngData, long pngDataLength, bool verbose = false)
        {
            return OptimizePng(pngData, pngDataLength, ZopfliPNGOptions.GetDefault(), verbose);
        }


        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="pngOptions">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns>Result PNG binary.</returns>
        public static byte[] OptimizePng(byte[] pngData, ZopfliPNGOptions pngOptions, bool verbose = false)
        {
            return OptimizePng(pngData, pngData.LongLength, pngOptions, verbose);
        }


        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="pngDataLength">Byte length of <paramref name="pngData"/>.</param>
        /// <param name="pngOptions">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns>Result PNG binary.</returns>
        public static byte[] OptimizePng(byte[] pngData, long pngDataLength, ZopfliPNGOptions pngOptions, bool verbose = false)
        {
            using (var cPngOptions = new CZopfliPNGOptions(pngOptions))
            {
                var error = UnsafeNativeMethods.CZopfliPNGOptimize(
                    pngData,
                    (UIntPtr)pngDataLength,
                    in cPngOptions,
                    verbose,
                    out var pResultPngHandle,
                    out var resultPngSize);

                if (pResultPngHandle.IsInvalid)
                {
                    return null;
                }

                using (pResultPngHandle)
                {
                    if (error != 0)
                    {
                        return null;
                    }
                    var resultPng = new byte[(ulong)resultPngSize];
                    Marshal.Copy(pResultPngHandle.DangerousGetHandle(), resultPng, 0, resultPng.Length);
                    return resultPng;
                }
            }
        }
    }
}
