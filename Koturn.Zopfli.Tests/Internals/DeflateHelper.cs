using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;


namespace Koturn.Zopfli.Tests.Internals
{
    /// <summary>
    /// Provides some Deflate-decompression methods.
    /// </summary>
    internal static class DeflateHelper
    {
        /// <summary>
        /// Decompress data compressed in Deflate format.
        /// </summary>
        /// <param name="compressedData">Data compressed in Deflate format.</param>
        /// <returns>Decompressed data.</returns>
        public static byte[] Decompress(byte[] compressedData)
        {
            using (var ims = new MemoryStream(compressedData))
            using (var gzs = new DeflateStream(ims, CompressionMode.Decompress))
            using (var oms = new MemoryStream())
            {
                gzs.CopyTo(oms);
                return oms.ToArray();
            }
        }

        /// <summary>
        /// Decompress data compressed in Deflate format.
        /// </summary>
        /// <param name="compressedDataBuffer">Data compressed in Deflate format.</param>
        /// <returns>Decompressed data.</returns>
        public static byte[] Decompress(SafeBuffer compressedDataBuffer)
        {
            using (var ums = new UnmanagedMemoryStream(compressedDataBuffer, 0, (long)compressedDataBuffer.ByteLength))
            using (var gzs = new DeflateStream(ums, CompressionMode.Decompress))
            using (var ms = new MemoryStream())
            {
                gzs.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
