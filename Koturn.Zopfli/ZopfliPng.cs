#if NET7_0_OR_GREATER
#    define SUPPORT_LIBRARY_IMPORT
#endif  // NET7_0_OR_GREATER
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using Koturn.Zopfli.Exceptions;
using Koturn.Zopfli.Internal;


namespace Koturn.Zopfli
{
    /// <summary>
    /// P/Invoke methods for zopflipng.dll.
    /// </summary>
#if SUPPORT_LIBRARY_IMPORT
    public static partial class ZopfliPng
#else
    public static class ZopfliPng
#endif  // SUPPORT_LIBRARY_IMPORT
    {
        /// <summary>
        /// Native methods.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
#if SUPPORT_LIBRARY_IMPORT
        internal static partial class SafeNativeMethods
#else
        internal static class SafeNativeMethods
#endif  // SUPPORT_LIBRARY_IMPORT
        {
            /// <summary>
            /// Get default parameter of <see cref="ZopfliPngOptions"/>.
            /// </summary>
            /// <param name="options">Options struct for ZopfliPNG.</param>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("zopflipng", EntryPoint = "CZopfliPNGSetDefaults", SetLastError = false)]
            public static partial void CZopfliPngSetDefaults(out CZopfliPngOptions options);
#else
            [DllImport("zopflipng", EntryPoint = "CZopfliPNGSetDefaults", ExactSpelling = true, SetLastError = false)]
            public static extern void CZopfliPngSetDefaults(out CZopfliPngOptions options);
#endif  // SUPPORT_LIBRARY_IMPORT

            /// <summary>
            /// Re-compress deflated data in PNG with zopfli algorithm.
            /// </summary>
            /// <param name="origPng">Source PNG binary.</param>
            /// <param name="origPngSize">Size of PNG binary.</param>
            /// <param name="options">Options for ZopfliPNG.</param>
            /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
            /// <param name="resultPng">Result PNG binary. This data is allocated with malloc in zopflipng.dll and caller of this method have to free the memory.</param>
            /// <param name="resultpngSize">Result PNG binary size.</param>
            /// <returns>Status code. 0 means success, otherwise it means failure.</returns>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("zopflipng", EntryPoint = "CZopfliPNGOptimize", SetLastError = false)]
            public static partial int CZopfliPngOptimize(
                IntPtr origPng,
                UIntPtr origPngSize,
                in CZopfliPngOptions options,
                [MarshalAs(UnmanagedType.U1)] bool verbose,
                out MallocedMemoryHandle resultPng,
                out UIntPtr resultpngSize);
#else
            [DllImport("zopflipng", EntryPoint = "CZopfliPNGOptimize", ExactSpelling = true, SetLastError = false)]
            public static extern int CZopfliPngOptimize(
                IntPtr origPng,
                UIntPtr origPngSize,
                in CZopfliPngOptions options,
                [MarshalAs(UnmanagedType.U1)] bool verbose,
                out MallocedMemoryHandle resultPng,
                out UIntPtr resultpngSize);
#endif  // SUPPORT_LIBRARY_IMPORT
        }


        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <returns>Result PNG binary.</returns>
        public static byte[] OptimizePng(byte[] pngData)
        {
            return OptimizePng(pngData, 0, pngData.Length, false);
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns>Result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static byte[] OptimizePng(byte[] pngData, bool verbose)
        {
            return OptimizePng(pngData, 0, pngData.Length, verbose);
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="offset">Byte offset of <paramref name="pngData"/>.</param>
        /// <param name="count">Byte length of <paramref name="pngData"/>.</param>
        /// <returns>Result PNG binary.</returns>
        public static byte[] OptimizePng(byte[] pngData, int offset, int count)
        {
            using (var cPngOptions = CZopfliPngOptions.GetDefault())
            {
                return OptimizePng(pngData, offset, count, cPngOptions, false);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="offset">Byte offset of <paramref name="pngData"/>.</param>
        /// <param name="count">Byte length of <paramref name="pngData"/>.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns>Result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static byte[] OptimizePng(byte[] pngData, int offset, int count, bool verbose)
        {
            using (var cPngOptions = CZopfliPngOptions.GetDefault())
            {
                return OptimizePng(pngData, offset, count, cPngOptions, verbose);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns>Result PNG binary.</returns>
        public static byte[] OptimizePng(byte[] pngData, ZopfliPngOptions options)
        {
            return OptimizePng(pngData, 0, pngData.Length, options, false);
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns>Result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static byte[] OptimizePng(byte[] pngData, ZopfliPngOptions options, bool verbose)
        {
            return OptimizePng(pngData, 0, pngData.Length, options, verbose);
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="offset">Byte offset of <paramref name="pngData"/>.</param>
        /// <param name="count">Byte length of <paramref name="pngData"/>.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns>Result PNG binary.</returns>
        public static byte[] OptimizePng(byte[] pngData, int offset, int count, ZopfliPngOptions options)
        {
            return OptimizePng(pngData, offset, count, options, false);
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="offset">Byte offset of <paramref name="pngData"/>.</param>
        /// <param name="count">Byte length of <paramref name="pngData"/>.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns>Result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static byte[] OptimizePng(byte[] pngData, int offset, int count, ZopfliPngOptions options, bool verbose)
        {
            using (var cPngOptions = new CZopfliPngOptions(options))
            {
                return OptimizePng(pngData, offset, count, cPngOptions, verbose);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <returns>Result PNG binary.</returns>
        public static byte[] OptimizePng(ReadOnlySpan<byte> pngData)
        {
            using (var cPngOptions = CZopfliPngOptions.GetDefault())
            {
                return OptimizePng(pngData, cPngOptions, false);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns>Result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static byte[] OptimizePng(ReadOnlySpan<byte> pngData, bool verbose)
        {
            using (var cPngOptions = CZopfliPngOptions.GetDefault())
            {
                return OptimizePng(pngData, cPngOptions, verbose);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns>Result PNG binary.</returns>
        public static byte[] OptimizePng(ReadOnlySpan<byte> pngData, ZopfliPngOptions options)
        {
            using (var cPngOptions = new CZopfliPngOptions(options))
            {
                return OptimizePng(pngData, cPngOptions, false);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns>Result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static byte[] OptimizePng(ReadOnlySpan<byte> pngData, ZopfliPngOptions options, bool verbose)
        {
            using (var cPngOptions = new CZopfliPngOptions(options))
            {
                return OptimizePng(pngData, cPngOptions, verbose);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        public static SafeBuffer OptimizePngUnmanaged(byte[] pngData)
        {
            return OptimizePngUnmanaged(pngData, 0, pngData.Length, false);
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static SafeBuffer OptimizePngUnmanaged(byte[] pngData, bool verbose)
        {
            return OptimizePngUnmanaged(pngData, 0, pngData.Length, verbose);
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="offset">Byte offset of <paramref name="pngData"/>.</param>
        /// <param name="count">Byte length of <paramref name="pngData"/>.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        public static SafeBuffer OptimizePngUnmanaged(byte[] pngData, int offset, int count)
        {
            using (var cPngOptions = CZopfliPngOptions.GetDefault())
            {
                return OptimizePngUnmanaged(pngData, offset, count, cPngOptions, false);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="offset">Byte offset of <paramref name="pngData"/>.</param>
        /// <param name="count">Byte length of <paramref name="pngData"/>.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static SafeBuffer OptimizePngUnmanaged(byte[] pngData, int offset, int count, bool verbose)
        {
            using (var cPngOptions = CZopfliPngOptions.GetDefault())
            {
                return OptimizePngUnmanaged(pngData, offset, count, cPngOptions, verbose);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        public static SafeBuffer OptimizePngUnmanaged(byte[] pngData, ZopfliPngOptions options)
        {
            return OptimizePngUnmanaged(pngData, 0, pngData.Length, options, false);
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static SafeBuffer OptimizePngUnmanaged(byte[] pngData, ZopfliPngOptions options, bool verbose)
        {
            return OptimizePngUnmanaged(pngData, 0, pngData.Length, options, verbose);
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="offset">Byte offset of <paramref name="pngData"/>.</param>
        /// <param name="count">Byte length of <paramref name="pngData"/>.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        public static SafeBuffer OptimizePngUnmanaged(byte[] pngData, int offset, int count, ZopfliPngOptions options)
        {
            using (var cPngOptions = new CZopfliPngOptions(options))
            {
                return OptimizePngUnmanaged(pngData, offset, count, cPngOptions, false);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="offset">Byte offset of <paramref name="pngData"/>.</param>
        /// <param name="count">Byte length of <paramref name="pngData"/>.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static SafeBuffer OptimizePngUnmanaged(byte[] pngData, int offset, int count, ZopfliPngOptions options, bool verbose)
        {
            using (var cPngOptions = new CZopfliPngOptions(options))
            {
                return OptimizePngUnmanaged(pngData, offset, count, cPngOptions, verbose);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        public static SafeBuffer OptimizePngUnmanaged(ReadOnlySpan<byte> pngData)
        {
            using (var cPngOptions = CZopfliPngOptions.GetDefault())
            {
                return OptimizePngUnmanaged(pngData, cPngOptions, false);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static SafeBuffer OptimizePngUnmanaged(ReadOnlySpan<byte> pngData, bool verbose)
        {
            using (var cPngOptions = CZopfliPngOptions.GetDefault())
            {
                return OptimizePngUnmanaged(pngData, cPngOptions, verbose);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        public static SafeBuffer OptimizePngUnmanaged(ReadOnlySpan<byte> pngData, ZopfliPngOptions options)
        {
            using (var cPngOptions = new CZopfliPngOptions(options))
            {
                return OptimizePngUnmanaged(pngData, cPngOptions, false);
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static SafeBuffer OptimizePngUnmanaged(ReadOnlySpan<byte> pngData, ZopfliPngOptions options, bool verbose)
        {
            using (var cPngOptions = new CZopfliPngOptions(options))
            {
                return OptimizePngUnmanaged(pngData, cPngOptions, verbose);
            }
        }


        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="offset">Byte offset of <paramref name="pngData"/>.</param>
        /// <param name="count">Byte length of <paramref name="pngData"/>.</param>
        /// <param name="cPngOptions">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        /// <exception cref="ZopfliPngException">Thrown when failed to optimize PNG data.</exception>
        internal static byte[] OptimizePng(byte[] pngData, int offset, int count, in CZopfliPngOptions cPngOptions, bool verbose = false)
        {
            using (var resultPngHandle = OptimizePngUnmanaged(pngData, offset, count, in cPngOptions, verbose))
            {
                var resultPng = new byte[resultPngHandle.ByteLength];
                Marshal.Copy(resultPngHandle.DangerousGetHandle(), resultPng, 0, resultPng.Length);
                return resultPng;
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="cPngOptions">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        /// <exception cref="ZopfliPngException">Thrown when failed to optimize PNG data.</exception>
        internal static byte[] OptimizePng(ReadOnlySpan<byte> pngData, in CZopfliPngOptions cPngOptions, bool verbose = false)
        {
            using (var resultPngHandle = OptimizePngUnmanaged(pngData, in cPngOptions, verbose))
            {
                var resultPng = new byte[resultPngHandle.ByteLength];
                Marshal.Copy(resultPngHandle.DangerousGetHandle(), resultPng, 0, resultPng.Length);
                return resultPng;
            }
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="offset">Byte offset of <paramref name="pngData"/>.</param>
        /// <param name="count">Byte length of <paramref name="pngData"/>.</param>
        /// <param name="cPngOptions">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        /// <exception cref="ZopfliPngException">Thrown when failed to optimize PNG data.</exception>
        internal static SafeBuffer OptimizePngUnmanaged(byte[] pngData, int offset, int count, in CZopfliPngOptions cPngOptions, bool verbose = false)
        {
            int error;
            MallocedMemoryHandle resultPngHandle;
            UIntPtr resultPngSize;

            unsafe
            {
                fixed (byte* pPngData = &pngData[offset])
                {
                    error = SafeNativeMethods.CZopfliPngOptimize(
                        (IntPtr)pPngData,
                        (UIntPtr)count,
                        cPngOptions,
                        verbose,
                        out resultPngHandle,
                        out resultPngSize);
                }
            }

            ZopfliPngException.ThrowIfError(error);

            resultPngHandle.Initialize((ulong)resultPngSize);

            return resultPngHandle;
        }

        /// <summary>
        /// Re-compress deflated data in PNG with Zopfli algorithm.
        /// </summary>
        /// <param name="pngData">Source PNG binary.</param>
        /// <param name="cPngOptions">Options for ZopfliPNG.</param>
        /// <param name="verbose">Output verbose message to stdout using printf() or not from zopflipng.dll.</param>
        /// <returns><see cref="SafeBuffer"/> of result PNG binary.</returns>
        /// <exception cref="ZopfliPngException">Thrown when failed to optimize PNG data.</exception>
        internal static SafeBuffer OptimizePngUnmanaged(ReadOnlySpan<byte> pngData, in CZopfliPngOptions cPngOptions, bool verbose = false)
        {
            int error;
            MallocedMemoryHandle resultPngHandle;
            UIntPtr resultPngSize;

            unsafe
            {
                fixed (byte* pPngData = pngData)
                {
                    error = SafeNativeMethods.CZopfliPngOptimize(
                        (IntPtr)pPngData,
                        (UIntPtr)pngData.Length,
                        cPngOptions,
                        verbose,
                        out resultPngHandle,
                        out resultPngSize);
                }
            }

            ZopfliPngException.ThrowIfError(error);

            resultPngHandle.Initialize((ulong)resultPngSize);

            return resultPngHandle;
        }
    }
}
