using System;
using System.Runtime.InteropServices;
using System.Security;
using ZopfliSharp.Internal;


namespace ZopfliSharp
{
    /// <summary>
    /// P/Invoke methods for zopfli.dll.
    /// </summary>
    public static class Zopfli
    {
        /// <summary>
        /// Native methods.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        internal static class UnsafeNativeMethods
        {
            /// <summary>
            /// Get default parameter of <see cref="ZopfliOptions"/>.
            /// </summary>
            /// <param name="options">Destination struct of default values.</param>
            [DllImport("zopfli.dll", ExactSpelling = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void ZopfliInitOptions(out ZopfliOptions options);


            /// <summary>
            /// Compresses according to the given output format and appends the result to the output.
            /// </summary>
            /// <param name="options">Global program options.</param>
            /// <param name="outputType">The output format to use.</param>
            /// <param name="inData">Input byte array.</param>
            /// <param name="inDataSize">Size of input byte array.</param>
            /// <param name="outData">Pointer to the dynamic output array to which the result is appended. Must be freed after use.</param>
            /// <param name="outDatasize">Pointer to the dynamic output array size</param>
            [DllImport("zopfli.dll", ExactSpelling = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern unsafe void ZopfliCompress(
                in ZopfliOptions options,
                [In] ZopfliFormat outputType,
                [In] IntPtr inData,
                [In] UIntPtr inDataSize,
                out MallocedMemoryHandle outData,
                out UIntPtr outDatasize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="format">Output format.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, ZopfliFormat format = ZopfliFormat.GZip)
        {
            return Compress(data, 0, data.Length, format);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="format">Output format.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, int offset, int count, ZopfliFormat format = ZopfliFormat.GZip)
        {
            return Compress(data, offset, count, ZopfliOptions.GetDefault(), format);
        }



        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, in ZopfliOptions options)
        {
            return Compress(data, 0, data.Length, options);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, int offset, int count, in ZopfliOptions options)
        {
            return Compress(data, offset, count, options, ZopfliFormat.GZip);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <param name="format">Output format.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip)
        {
            return Compress(data, 0, data.Length, options, format);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <param name="format">Output format.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, int offset, int count, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip)
        {
            var compressedDataHandle = CompressUnmanaged(data, offset, count, options, format);
            using (compressedDataHandle)
            {
                var compressedData = new byte[compressedDataHandle.ByteLength];
                Marshal.Copy(compressedDataHandle.DangerousGetHandle(), compressedData, 0, compressedData.Length);
                return compressedData;
            }
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="format">Output format.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanaged(byte[] data, ZopfliFormat format = ZopfliFormat.GZip)
        {
            return CompressUnmanaged(data, 0, data.Length, format);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="format">Output format.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanaged(byte[] data, int offset, int count, ZopfliFormat format = ZopfliFormat.GZip)
        {
            return CompressUnmanaged(data, offset, count, ZopfliOptions.GetDefault(), format);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanaged(byte[] data, in ZopfliOptions options)
        {
            return CompressUnmanaged(data, 0, data.Length, options);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanaged(byte[] data, int offset, int count, in ZopfliOptions options)
        {
            return CompressUnmanaged(data, offset, count, options, ZopfliFormat.GZip);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <param name="format">Output format.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanaged(byte[] data, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip)
        {
            return CompressUnmanaged(data, 0, data.Length, options, format);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <param name="format">Output format.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanaged(byte[] data, int offset, int count, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip)
        {
            MallocedMemoryHandle compressedDataHandle;
            UIntPtr compressedDataSize;
            unsafe
            {
                fixed (byte* pData = &data[offset])
                {
                    UnsafeNativeMethods.ZopfliCompress(
                        options,
                        format,
                        (IntPtr)pData,
                        (UIntPtr)count,
                        out compressedDataHandle,
                        out compressedDataSize);
                }
            }

            compressedDataHandle.Initialize((ulong)compressedDataSize);
            return compressedDataHandle;
        }
    }


    /// <summary>
    /// Output binary format types.
    /// </summary>
    public enum ZopfliFormat
    {
        /// <summary>
        /// Output to gzip format.
        /// </summary>
        GZip,
        /// <summary>
        /// Output to zlib format.
        /// </summary>
        ZLib,
        /// <summary>
        /// Output to deflate format
        /// </summary>
        Deflate
    }
}
