#if NET7_0_OR_GREATER
#    define SUPPORT_LIBRARY_IMPORT
#endif  // NET7_0_OR_GREATER
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using Koturn.Zopfli.Checksums;
using Koturn.Zopfli.Enums;
using Koturn.Zopfli.Internal;


namespace Koturn.Zopfli
{
    /// <summary>
    /// P/Invoke methods for zopfli.dll.
    /// </summary>
#if SUPPORT_LIBRARY_IMPORT
    public static partial class Zopfli
#else
    public static class Zopfli
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
            /// Get default parameter of <see cref="ZopfliOptions"/>.
            /// </summary>
            /// <param name="options">Destination struct of default values.</param>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("zopfli", EntryPoint = nameof(ZopfliInitOptions), SetLastError = false)]
            public static partial void ZopfliInitOptions(out ZopfliOptions options);
#else
            [DllImport("zopfli", EntryPoint = nameof(ZopfliInitOptions), ExactSpelling = true, SetLastError = false)]
            public static extern void ZopfliInitOptions(out ZopfliOptions options);
#endif  // SUPPORT_LIBRARY_IMPORT

            /// <summary>
            /// Compresses according to the given output format and appends the result to the output.
            /// </summary>
            /// <param name="options">Global program options.</param>
            /// <param name="outputType">The output format to use.</param>
            /// <param name="inData">Input byte array.</param>
            /// <param name="inDataSize">Size of input byte array.</param>
            /// <param name="outData">Pointer to the dynamic output array to which the result is appended. Must be freed after use.</param>
            /// <param name="outDatasize">Pointer to the dynamic output array size</param>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("zopfli", EntryPoint = nameof(ZopfliCompress), SetLastError = false)]
            public static partial void ZopfliCompress(
                in ZopfliOptions options,
                ZopfliFormat outputType,
                IntPtr inData,
                UIntPtr inDataSize,
                out MallocedMemoryHandle outData,
                out UIntPtr outDatasize);
#else
            [DllImport("zopfli", EntryPoint = nameof(ZopfliCompress), ExactSpelling = true, SetLastError = false)]
            public static extern void ZopfliCompress(
                in ZopfliOptions options,
                ZopfliFormat outputType,
                IntPtr inData,
                UIntPtr inDataSize,
                out MallocedMemoryHandle outData,
                out UIntPtr outDatasize);
#endif  // SUPPORT_LIBRARY_IMPORT

            /// <summary>
            /// Deflate given data block.
            /// </summary>
            /// <param name="options">Global program options.</param>
            /// <param name="blockType">Comression rule of each block.</param>
            /// <param name="isFinal">A flag which represents this block is final or not.</param>
            /// <param name="inData">Input byte array.</param>
            /// <param name="inStart">Offset of start of input byte array.</param>
            /// <param name="inEnd">Offset of end of input byte array.</param>
            /// <param name="bitPointer">Bit position of output data which has been written.</param>
            /// <param name="outData">Pointer to the dynamic output array to which the result is appended. Must be freed after use.</param>
            /// <param name="outDatasize">The dynamic output array size.</param>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("zopfli", EntryPoint = nameof(ZopfliDeflatePart), SetLastError = false)]
            public static unsafe partial void ZopfliDeflatePart(
                in ZopfliOptions options,
                BlockType blockType,
                [MarshalAs(UnmanagedType.U1)] bool isFinal,
                IntPtr inData,
                UIntPtr inStart,
                UIntPtr inEnd,
                ref byte bitPointer,
                ref MallocedMemoryHandle? outData,
                ref UIntPtr outDatasize);
#else
            [DllImport("zopfli", EntryPoint = nameof(ZopfliDeflatePart), ExactSpelling = true, SetLastError = false)]
            public static unsafe extern void ZopfliDeflatePart(
                in ZopfliOptions options,
                BlockType blockType,
                [MarshalAs(UnmanagedType.U1)] bool isFinal,
                IntPtr inData,
                UIntPtr inStart,
                UIntPtr inEnd,
                ref byte bitPointer,
                ref MallocedMemoryHandle? outData,
                ref UIntPtr outDatasize);
#endif  // SUPPORT_LIBRARY_IMPORT
        }


        /// <summary>
        /// Default master block size.
        /// </summary>
        public const int DefaultMasterBlockSize = 1024 * 1024;


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="format">Output format.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, ZopfliFormat format = ZopfliFormat.GZip)
        {
            return Compress(data, 0, data.Length, ZopfliOptions.GetDefault(), format);
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
        /// <param name="options">Options for Zopfli.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, in ZopfliOptions options)
        {
            return Compress(data, 0, data.Length, options, ZopfliFormat.GZip);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, int offset, int count, in ZopfliOptions options)
        {
            return Compress(data, offset, count, options, ZopfliFormat.GZip);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for Zopfli.</param>
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
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="format">Output format.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(byte[] data, int offset, int count, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip)
        {
            using (var compressedDataHandle = CompressUnmanaged(data, offset, count, options, format))
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
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(ReadOnlySpan<byte> data, ZopfliFormat format = ZopfliFormat.GZip)
        {
            return Compress(data, ZopfliOptions.GetDefault(), format);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="format">Output format.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] Compress(ReadOnlySpan<byte> data, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip)
        {
            using (var compressedDataHandle = CompressUnmanaged(data, options, format))
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
            return CompressUnmanaged(data, 0, data.Length, ZopfliOptions.GetDefault(), format);
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
        /// <param name="options">Options for Zopfli.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanaged(byte[] data, in ZopfliOptions options)
        {
            return CompressUnmanaged(data, 0, data.Length, options, ZopfliFormat.GZip);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanaged(byte[] data, int offset, int count, in ZopfliOptions options)
        {
            return CompressUnmanaged(data, offset, count, options, ZopfliFormat.GZip);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for Zopfli.</param>
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
        /// <param name="options">Options for Zopfli.</param>
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
                    SafeNativeMethods.ZopfliCompress(
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


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="format">Output format.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanaged(ReadOnlySpan<byte> data, ZopfliFormat format = ZopfliFormat.GZip)
        {
            return CompressUnmanaged(data, ZopfliOptions.GetDefault(), format);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="format">Output format.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanaged(ReadOnlySpan<byte> data, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip)
        {
            MallocedMemoryHandle compressedDataHandle;
            UIntPtr compressedDataSize;
            unsafe
            {
                fixed (byte* pData = data)
                {
                    SafeNativeMethods.ZopfliCompress(
                        options,
                        format,
                        (IntPtr)pData,
                        (UIntPtr)data.Length,
                        out compressedDataHandle,
                        out compressedDataSize);
                }
            }

            compressedDataHandle.Initialize((ulong)compressedDataSize);
            return compressedDataHandle;
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="format">Output format.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] CompressEx(byte[] data, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressEx(data, 0, data.Length, ZopfliOptions.GetDefault(), format, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="format">Output format.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] CompressEx(byte[] data, int offset, int count, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressEx(data, offset, count, ZopfliOptions.GetDefault(), format, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] CompressEx(byte[] data, in ZopfliOptions options, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressEx(data, 0, data.Length, options, ZopfliFormat.GZip, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] CompressEx(byte[] data, int offset, int count, in ZopfliOptions options, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressEx(data, offset, count, options, ZopfliFormat.GZip, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="format">Output format.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] CompressEx(byte[] data, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressEx(data, 0, data.Length, options, format, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="format">Output format.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] CompressEx(byte[] data, int offset, int count, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            using (var compressedDataHandle = CompressUnmanagedEx(data, offset, count, options, format, blockType, masterBlockSize))
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
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] CompressEx(ReadOnlySpan<byte> data, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressEx(data, ZopfliOptions.GetDefault(), format, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="format">Output format.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns>Compressed data of <paramref name="data"/>.</returns>
        public static byte[] CompressEx(ReadOnlySpan<byte> data, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            using (var compressedDataHandle = CompressUnmanagedEx(data, options, format, blockType, masterBlockSize))
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
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanagedEx(byte[] data, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressUnmanagedEx(data, 0, data.Length, ZopfliOptions.GetDefault(), format, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="format">Output format.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanagedEx(byte[] data, int offset, int count, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressUnmanagedEx(data, offset, count, ZopfliOptions.GetDefault(), format, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanagedEx(byte[] data, in ZopfliOptions options, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressUnmanagedEx(data, 0, data.Length, options, ZopfliFormat.GZip, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanagedEx(byte[] data, int offset, int count, in ZopfliOptions options, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressUnmanagedEx(data, offset, count, options, ZopfliFormat.GZip, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="format">Output format.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanagedEx(byte[] data, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressUnmanagedEx(data, 0, data.Length, options, format, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="format">Output format.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanagedEx(byte[] data, int offset, int count, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            var compressedDataHandle = format == ZopfliFormat.Deflate ? null : new MallocedMemoryHandle(16);
            if (compressedDataHandle != null)
            {
                WriteHeader(compressedDataHandle, 0, format);
            }

            unsafe
            {
                fixed (byte* pData = data)
                {
                    compressedDataHandle = CompressUnmanagedExCore(
                        compressedDataHandle,
                        (IntPtr)pData,
                        offset,
                        count,
                        options,
                        blockType,
                        masterBlockSize);
                }
            }

            if (format != ZopfliFormat.Deflate && compressedDataHandle != null)
            {
                var footerSize = WriteFooter(
                    compressedDataHandle,
                    format,
                    CalcChecksum(data, offset, count, format),
                    (uint)count);
            }

            return compressedDataHandle ?? new MallocedMemoryHandle();
        }

        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="format">Output format.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanagedEx(ReadOnlySpan<byte> data, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            return CompressUnmanagedEx(data, ZopfliOptions.GetDefault(), format, blockType, masterBlockSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="data">Source binary data.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="format">Output format.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns><see cref="SafeBuffer"/> of compressed data of <paramref name="data"/>.</returns>
        public static SafeBuffer CompressUnmanagedEx(ReadOnlySpan<byte> data, in ZopfliOptions options, ZopfliFormat format = ZopfliFormat.GZip, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            var compressedDataHandle = format == ZopfliFormat.Deflate ? null : new MallocedMemoryHandle(16);
            if (compressedDataHandle != null)
            {
                WriteHeader(compressedDataHandle, 0, format);
            }

            unsafe
            {
                fixed (byte* pData = data)
                {
                    compressedDataHandle = CompressUnmanagedExCore(
                        compressedDataHandle,
                        (IntPtr)pData,
                        0,
                        data.Length,
                        options,
                        blockType,
                        masterBlockSize);
                }
            }

            if (format != ZopfliFormat.Deflate && compressedDataHandle != null)
            {
                var footerSize = WriteFooter(
                    compressedDataHandle,
                    format,
                    CalcChecksum(data, format),
                    (uint)data.Length);
            }

            return compressedDataHandle ?? new MallocedMemoryHandle();
        }


        /// <summary>
        /// Deflate given data block.
        /// </summary>
        /// <param name="buffer">Source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="isFinal">A flag which represents this block is final or not.</param>
        /// <param name="bitPointer">Bit position of output data which has been written.</param>
        /// <param name="handle">Handle of unmanaged memory.</param>
        internal static unsafe void DeflatePart(byte[] buffer, int offset, int count, in ZopfliOptions options, BlockType blockType, bool isFinal, ref byte bitPointer, ref MallocedMemoryHandle handle)
        {
            var outDataSize = (nuint)handle.ByteLength;
            fixed (byte* pBuffer = buffer)
            {
                var p = (IntPtr)pBuffer;
                SafeNativeMethods.ZopfliDeflatePart(
                    options,
                    blockType,
                    isFinal,
                    p,
                    (UIntPtr)offset,
                    (UIntPtr)(offset + count),
                    ref bitPointer,
                    ref handle!,
                    ref outDataSize);
            }
            handle.Initialize((ulong)outDataSize);
        }


        /// <summary>
        /// Compress data with Zopfli algorithm.
        /// </summary>
        /// <param name="handle">Intermidiate compressed data.</param>
        /// <param name="pData">Pointer to source binary data.</param>
        /// <param name="offset">Source binary data offset.</param>
        /// <param name="count">Source binary data length.</param>
        /// <param name="options">Options for Zopfli.</param>
        /// <param name="blockType">Comression rule of each block.</param>
        /// <param name="masterBlockSize">Split size of source binary data. Set 0 or negative value to disable splitting.</param>
        /// <returns><see cref="MallocedMemoryHandle"/> of compressed data.</returns>
        private static MallocedMemoryHandle? CompressUnmanagedExCore(MallocedMemoryHandle? handle, IntPtr pData, int offset, int count, in ZopfliOptions options, BlockType blockType = BlockType.Dynamic, int masterBlockSize = DefaultMasterBlockSize)
        {
            if (masterBlockSize <= 0)
            {
                masterBlockSize = count;
            }

            bool isFinal;
            var bitPointer = (byte)0;
            var compressedDataSize = handle == null ? default : (nuint)handle.ByteLength;
            do
            {
                isFinal = count - masterBlockSize <= 0;
                var nWrite = isFinal ? count : masterBlockSize;

                SafeNativeMethods.ZopfliDeflatePart(
                    options,
                    blockType,
                    isFinal,
                    pData,
                    (UIntPtr)offset,
                    (UIntPtr)(offset + nWrite),
                    ref bitPointer,
                    ref handle,
                    ref compressedDataSize);
                offset += nWrite;
                count -= nWrite;
            } while (!isFinal);

            handle?.Initialize((ulong)compressedDataSize);

            return handle;
        }


        /// <summary>
        /// Get header size of specified format.
        /// </summary>
        /// <param name="format">Header format.</param>
        /// <returns>Size of header.</returns>
        private static int GetHeaderSize(ZopfliFormat format)
        {
            return format switch
            {
                ZopfliFormat.ZLib => 2,
                ZopfliFormat.GZip => 10,
                _ => 0
            };
        }


        /// <summary>
        /// Write header of specified format to the unmanaged memory.
        /// </summary>
        /// <param name="handle">Handle of unmanaged memory.</param>
        /// <param name="offset">Offset of unmanaged memory.</param>
        /// <param name="format">Header format.</param>
        /// <returns>Size of header.</returns>
        private static int WriteHeader(MallocedMemoryHandle handle, int offset, ZopfliFormat format)
        {
            if (format == ZopfliFormat.Deflate)
            {
                return 0;
            }
            using (var ums = new UnmanagedMemoryStream(handle, offset, GetHeaderSize(format), FileAccess.Write))
            {
                var headerSize = WriteHeader(ums, format);
                handle.Initialize((ulong)headerSize);
                return headerSize;
            }
        }


        /// <summary>
        /// Write header of specified format to the stream.
        /// </summary>
        /// <param name="s">Destination stream.</param>
        /// <param name="format">Header format.</param>
        /// <returns>Size of header.</returns>
        private static int WriteHeader(Stream s, ZopfliFormat format)
        {
            return format switch
            {
                ZopfliFormat.ZLib => WriteZLibHeader(s),
                ZopfliFormat.GZip => WriteGZipHeader(s),
                _ => 0
            };
        }


        /// <summary>
        /// Write header of zlib format.
        /// </summary>
        /// <param name="s">Destination stream.</param>
        /// <returns>Size of zlib header (always 2).</returns>
        internal static int WriteZLibHeader(Stream s)
        {
            // Compression Method and Flags.
            //   bits 0 to 3  CM     Compression method
            //   bits 4 to 7  CINFO  Compression info
            const uint cmf = 8 | (7 << 4);  /* CM 8, CINFO 7. See zlib spec.*/
            // Compression level.
            const uint flevel = 3;
            // Preset Dictionary.
            const uint fdict = 0;
            // FLG (FLaGs)
            // This flag byte is divided as follows:
            //   bits 0 to 4  FCHECK  (check bits for CMF and FLG)
            //   bit  5       FDICT   (preset dictionary)
            //   bits 6 to 7  FLEVEL  (compression level)
            const uint cmfflgPre = (cmf << 8) | (flevel << 6) | (fdict << 5);
            const uint fcheck = 31 - (cmfflgPre % 31);

            const uint cmfflg = cmfflgPre | fcheck;

            s.WriteByte((byte)(cmfflg >> 8));
            unchecked
            {
                s.WriteByte((byte)cmfflg);
            }

            return sizeof(byte) * 2;
        }


        /// <summary>
        /// Write header of gzip format.
        /// </summary>
        /// <param name="s">Destination stream.</param>
        /// <returns>Size of gzip header (always 10).</returns>
        internal static int WriteGZipHeader(Stream s)
        {
            var gzipHeader = new byte[] {
                31,  // ID1
                139,  // ID2
                8, // CM
                0,  // FLG
                0, 0, 0, 0,  // MTIME
                2,  // XFL, 2 indicates best compression.
                3  // OS follows Unix conventions.
            };
            s.Write(gzipHeader, 0, gzipHeader.Length);

            return gzipHeader.Length;
        }


        /// <summary>
        /// Get footer size of specified format.
        /// </summary>
        /// <param name="format">Footer format.</param>
        /// <returns>Size of footer.</returns>
        private static int GetFooterSize(ZopfliFormat format)
        {
            return format switch
            {
                ZopfliFormat.ZLib => sizeof(uint),
                ZopfliFormat.GZip => sizeof(uint) * 2,
                _ => 0
            };
        }


        /// <summary>
        /// Write footer of specified format to the unmanaged memory.
        /// </summary>
        /// <param name="handle">Handle of unmanaged memory.</param>
        /// <param name="format">Header format.</param>
        /// <param name="checksum">Value of checksum.</param>
        /// <param name="inflatedSize">Original data size.</param>
        /// <returns>Size of footer.</returns>
        private static int WriteFooter(MallocedMemoryHandle handle, ZopfliFormat format, uint checksum, uint inflatedSize)
        {
            if (format == ZopfliFormat.Deflate)
            {
                return 0;
            }
            var required = GetFooterSize(format);
            var length = (int)handle.ByteLength;
            var newLength = length + required;
            if (newLength > RoundUpToPowerOf2(length))
            {
                handle.ReAlloc(newLength);
            }
            else
            {
                handle.Initialize((ulong)newLength);
            }
            using (var ums = new UnmanagedMemoryStream(handle, length, required, FileAccess.Write))
            {
                return WriteFooter(ums, format, checksum, inflatedSize);
            }
        }


        /// <summary>
        /// Write footer of specified format to the stream.
        /// </summary>
        /// <param name="s">Destination stream.</param>
        /// <param name="format">Footer format.</param>
        /// <param name="checksum">Value of checksum.</param>
        /// <param name="inflatedSize">Original data size.</param>
        /// <returns>Size of footer.</returns>
        private static int WriteFooter(Stream s, ZopfliFormat format, uint checksum, uint inflatedSize)
        {
            return format switch
            {
                ZopfliFormat.ZLib => WriteZLibFooter(s, checksum),
                ZopfliFormat.GZip => WriteGZipFooter(s, checksum, inflatedSize),
                _ => 0
            };
        }


        /// <summary>
        /// Write footer of zlib format, just writing value of Adler-32.
        /// </summary>
        /// <param name="s">Destination stream.</param>
        /// <param name="adler">Checksum value of Adler32.</param>
        /// <returns>Size of zlib footer (always 4 bytes).</returns>
        internal static int WriteZLibFooter(Stream s, uint adler)
        {
            // Adler32 (Little Endian)
            s.WriteByte((byte)(adler >> 24));
            s.WriteByte((byte)(adler >> 16));
            s.WriteByte((byte)(adler >> 8));
            s.WriteByte((byte)adler);

            return sizeof(uint);
        }


        /// <summary>
        /// Write footer of gzip format, just writing value of CRC-32 and inflated data size.
        /// </summary>
        /// <param name="s">Destination stream.</param>
        /// <param name="inflatedSize">Original data size.</param>
        /// <param name="crc">Checksum value of CRC-32.</param>
        /// <returns>Size of gzip header (always 8 bytes).</returns>
        internal static int WriteGZipFooter(Stream s, uint crc, uint inflatedSize)
        {
            // CRC-32 (Big Endian)
            s.WriteByte((byte)crc);
            s.WriteByte((byte)(crc >> 8));
            s.WriteByte((byte)(crc >> 16));
            s.WriteByte((byte)(crc >> 24));

            // ISIZE (Big Endian)
            s.WriteByte((byte)inflatedSize);
            s.WriteByte((byte)(inflatedSize >> 8));
            s.WriteByte((byte)(inflatedSize >> 16));
            s.WriteByte((byte)(inflatedSize >> 24));

            return sizeof(uint) * 2;
        }


        /// <summary>
        /// Round up to the next highest power of 2.
        /// </summary>
        /// <param name="n">An <see cref="uint"/> value.</param>
        /// <returns>Rounded up value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint RoundUpToPowerOf2(int n)
        {
            return RoundUpToPowerOf2((uint)n);
        }


        /// <summary>
        /// Round up to the next highest power of 2.
        /// </summary>
        /// <param name="n">An <see cref="uint"/> value.</param>
        /// <returns>Rounded up value.</returns>
        /// <remarks><seealso href="https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2"/></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint RoundUpToPowerOf2(uint n)
        {
            n--;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            return n + 1;
        }


        /// <summary>
        /// Calculate checksum according to specified format.
        /// </summary>
        /// <param name="data"><see cref="byte"/> data array.</param>
        /// <param name="offset">Offset of <paramref name="data"/>.</param>
        /// <param name="count">Data count of <paramref name="data"/>.</param>
        /// <param name="format">Format of compressed data.</param>
        /// <returns>Checksum value.</returns>
        private static uint CalcChecksum(byte[] data, int offset, int count, ZopfliFormat format)
        {
            return format switch
            {
                ZopfliFormat.GZip => Crc32Util.Compute(data, offset, count),
                ZopfliFormat.ZLib => Adler32Util.Compute(data, offset, count),
                _ => 0U
            };
        }


        /// <summary>
        /// Calculate checksum according to specified format.
        /// </summary>
        /// <param name="data"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <param name="format">Format of compressed data.</param>
        /// <returns>Checksum value.</returns>
        private static uint CalcChecksum(ReadOnlySpan<byte> data, ZopfliFormat format)
        {
            return format switch
            {
                ZopfliFormat.GZip => Crc32Util.Compute(data),
                ZopfliFormat.ZLib => Adler32Util.Compute(data),
                _ => 0U
            };
        }
    }
}
