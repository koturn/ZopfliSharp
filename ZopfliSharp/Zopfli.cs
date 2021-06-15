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
            /// <param name="options">Options for Zopfli algorithm.</param>
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

            /// <summary>
            /// Compresses data with Zopfli algorithm.
            /// </summary>
            /// <param name="options">Options for Zopfli algorithm.</param>
            /// <param name="btype"></param>
            /// <param name="final">A flag whether final or not,</param>
            /// <param name="inData">Input byte array.</param>
            /// <param name="inDataSize">Size of input byte array.</param>
            /// <param name="bitPointer">Bit pointer</param>
            /// <param name="outData">Pointer to the dynamic output array to which the result is appended. Must be freed after use.</param>
            /// <param name="outDatasize">Pointer to the dynamic output array size</param>
            [DllImport("zopfli.dll", ExactSpelling = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern unsafe void ZopfliDeflate(
                in ZopfliOptions options,
                int btype,
                bool final,
                [In] IntPtr inData,
                [In] UIntPtr inDataSize,
                ref byte bitPointer,
                out MallocedMemoryHandle outData,
                ref UIntPtr outDatasize);
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
        /// <param name="options">Options for Zopfli algorithm.</param>
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
        /// <param name="options">Options for Zopfli algorithm.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, int offset, int count, in ZopfliOptions options)
        {
            return Compress(data, offset, count, options, ZopfliFormat.GZip);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for Zopfli algorithm.</param>
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
        /// <param name="options">Options for Zopfli algorithm.</param>
        /// <param name="format">Output format.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, int offset, int count, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip)
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

            using (compressedDataHandle)
            {
                var compressedData = new byte[(ulong)compressedDataSize];
                Marshal.Copy(compressedDataHandle.DangerousGetHandle(), compressedData, 0, compressedData.Length);
                return compressedData;
            }
        }

        /// <summary>
        /// <para>Compress data with Zopfli algorithm.</para>
        /// <para>Just compress only, do not add header or footer.</para>
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="final">A flag whether final or not,</param>
        /// <param name="options">Options for Zopfli algorithm.</param>
        /// <param name="bitPointer">Bit pointer</param>
        /// <param name="outData">Pointer to the dynamic output array to which the result is appended. Must be freed after use.</param>
        /// <param name="outDatasize">Pointer to the dynamic output array size.</param>
        internal static unsafe void ZopfliDeflate(byte[] data, int offset, int count, bool final, in ZopfliOptions options, ref byte bitPointer, out MallocedMemoryHandle outdata, ref UIntPtr outDatasize)
        {
            fixed (byte* pData = &data[offset])
            {
                UnsafeNativeMethods.ZopfliDeflate(
                    in options,
                    2,
                    final,
                    (IntPtr)pData,
                    (UIntPtr)count,
                    ref bitPointer,
                    out outdata,
                    ref outDatasize);
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
