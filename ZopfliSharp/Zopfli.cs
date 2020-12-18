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
                [In] byte[] inData,
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
            return Compress(data, data.LongLength, format);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="dataLength">Source binary data length.</param>
        /// <param name="format">Output format.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, long dataLength, ZopfliFormat format = ZopfliFormat.GZip)
        {
            return Compress(data, dataLength, ZopfliOptions.GetDefault(), format);
        }



        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, in ZopfliOptions options)
        {
            return Compress(data, data.LongLength, options);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="dataLength">Source binary data length.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, long dataLength, in ZopfliOptions options)
        {
            return Compress(data, dataLength, options, ZopfliFormat.GZip);
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
            return Compress(data, data.LongLength, options, format);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="dataLength">Source binary data length.</param>
        /// <param name="options">Options for ZopfliPNG.</param>
        /// <param name="format">Output format.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, long dataLength, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip)
        {
            UnsafeNativeMethods.ZopfliCompress(
                options,
                format,
                data,
                (UIntPtr)dataLength,
                out var compressedDataHandle,
                out var compressedDataSize);

            using (compressedDataHandle)
            {
                var compressedData = new byte[(ulong)compressedDataSize];
                Marshal.Copy(compressedDataHandle.DangerousGetHandle(), compressedData, 0, compressedData.Length);
                return compressedData;
            }
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
