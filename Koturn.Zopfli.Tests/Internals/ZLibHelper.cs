using System.IO;
using System.Runtime.InteropServices;

#if NET6_0_OR_GREATER
using ZLibStream = System.IO.Compression.ZLibStream;
using CompressionMode = System.IO.Compression.CompressionMode;
#else
using ZLibStream = Ionic.Zlib.ZlibStream;
using CompressionMode = Ionic.Zlib.CompressionMode;
#endif  // !NET6_0_OR_GREATER


namespace Koturn.Zopfli.Tests.Internals
{
    /// <summary>
    /// Provides some ZLib-decompression methods.
    /// </summary>
    internal static class ZLibHelper
    {
        /// <summary>
        /// Decompress data compressed in ZLIB format.
        /// </summary>
        /// <param name="compressedData">Data compressed in ZLIB format.</param>
        /// <returns>Decompressed data.</returns>
        public static byte[] Decompress(byte[] compressedData)
        {
            using (var ims = new MemoryStream(compressedData))
            using (var gzs = new ZLibStream(ims, CompressionMode.Decompress))
            using (var oms = new MemoryStream())
            {
                gzs.CopyTo(oms);
                return oms.ToArray();
            }
        }

        /// <summary>
        /// Decompress data compressed in ZLIB format.
        /// </summary>
        /// <param name="compressedDataBuffer">Data compressed in ZLIB format.</param>
        /// <returns>Decompressed data.</returns>
        public static byte[] Decompress(SafeBuffer compressedDataBuffer)
        {
            using (var ums = new UnmanagedMemoryStream(compressedDataBuffer, 0, (long)compressedDataBuffer.ByteLength))
            using (var gzs = new ZLibStream(ums, CompressionMode.Decompress))
            using (var ms = new MemoryStream())
            {
                gzs.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
