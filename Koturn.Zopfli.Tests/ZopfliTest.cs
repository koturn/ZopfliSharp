#if NET7_0_OR_GREATER
#    define SUPPORT_LIBRARY_IMPORT
#endif  // NET7_0_OR_GREATER

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Xunit.Extensions.AssemblyFixture;
using Koturn.Zopfli.Enums;
using Koturn.Zopfli.Tests.Internals;


namespace Koturn.Zopfli.Tests
{
    /// <summary>
    /// Test class for <see cref="Zopfli"/>.
    /// </summary>
    public class ZopfliTest : IAssemblyFixture<UnitTestAssemblyFixture>
    {
        /// <summary>
        /// Check <c>sizeof(ZopfliOptions)</c> and <c>Marshal.SizeOf&lt;ZopfliOptions&gt;()</c>.
        /// </summary>
        [Fact]
        public void SizeTest()
        {
            unsafe
            {
#if NET7_0_OR_GREATER
                Assert.Equal(Marshal.SizeOf<ZopfliOptions>(), sizeof(ZopfliOptions));
#else
                Assert.Equal(24, Marshal.SizeOf<ZopfliOptions>());
                Assert.Equal(16, sizeof(ZopfliOptions));
#endif  // NET7_0_OR_GREATER
            }
        }

        /// <summary>
        /// Test for <see cref="Zopfli.Compress(byte[], ZopfliFormat)"/> with <see cref="ZopfliFormat.GZip"/>.
        /// </summary>
        [Fact]
        public void CompressGZipTest01()
        {
            const string filePath = "Koturn.Zopfli.dll";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            var compressedData = Zopfli.Compress(data, ZopfliFormat.GZip);

            Console.WriteLine($"Compress {filePath}: Original=[{data.Length}]Bytes Compressed=[{compressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");

            var decompressedData = GZipHelper.Decompress(compressedData);
            Assert.Equal(data, decompressedData);
        }

        /// <summary>
        /// Test for <see cref="Zopfli.CompressUnmanaged(byte[], ZopfliFormat)"/> with <see cref="ZopfliFormat.GZip"/>.
        /// </summary>
        [Fact]
        public void CompressUnmanagedGZipTest01()
        {
            const string filePath = "Koturn.Zopfli.dll";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            using (var compressedData = Zopfli.CompressUnmanaged(data, ZopfliFormat.GZip))
            {
                Console.WriteLine($"Compress {filePath}: Original=[{data.Length}]Bytes Compressed=[{compressedData.ByteLength}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");
                var decompressedData = GZipHelper.Decompress(compressedData);
                Assert.Equal(data, decompressedData);
            }
        }

        /// <summary>
        /// Test for <see cref="ZopfliStream"/> with <see cref="ZopfliFormat.GZip"/>.
        /// </summary>
        [Fact]
        public void CompressGZipWithStreamTest01()
        {
            const string filePath = "Koturn.Zopfli.dll";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            using (var oms = new MemoryStream())
            {
                using (var zs = new ZopfliStream(oms, ZopfliOptions.GetDefault(), ZopfliFormat.GZip))
                using (var ims = new MemoryStream(data))
                {
                    ims.CopyTo(zs);
                }
                var compressedData = oms.ToArray();

                Console.WriteLine($"Compress {filePath}: Original=[{data.Length}]Bytes Compressed=[{compressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");

                var decompressedData = GZipHelper.Decompress(compressedData);
                Assert.Equal(data, decompressedData);
            }
        }

        /// <summary>
        /// Test for <see cref="Zopfli.Compress(byte[], ZopfliFormat)"/> with <see cref="ZopfliFormat.ZLib"/>.
        /// </summary>
        [Fact]
        public void CompressZLibTest01()
        {
            const string filePath = "Koturn.Zopfli.dll";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            var compressedData = Zopfli.Compress(data, ZopfliFormat.ZLib);

            Console.WriteLine($"Compress {filePath}: Original=[{data.Length}]Bytes Compressed=[{compressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");

            var decompressedData = ZLibHelper.Decompress(compressedData);
            Assert.Equal(data, decompressedData);
        }

        /// <summary>
        /// Test for <see cref="Zopfli.CompressUnmanaged(byte[], ZopfliFormat)"/> with <see cref="ZopfliFormat.ZLib"/>.
        /// </summary>
        [Fact]
        public void CompressUnmanagedZLibTest01()
        {
            const string filePath = "Koturn.Zopfli.dll";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            using (var compressedData = Zopfli.CompressUnmanaged(data, ZopfliFormat.ZLib))
            {
                Console.WriteLine($"Compress {filePath}: Original=[{data.Length}] Compressed=[{compressedData.ByteLength}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");
                var decompressedData = ZLibHelper.Decompress(compressedData);
                Assert.Equal(data, decompressedData);
            }
        }

        /// <summary>
        /// Test for <see cref="ZopfliStream"/> with <see cref="ZopfliFormat.ZLib"/>.
        /// </summary>
        [Fact]
        public void CompressZLibWithStreamTest01()
        {
            const string filePath = "Koturn.Zopfli.dll";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            using (var oms = new MemoryStream())
            {
                using (var zs = new ZopfliStream(oms, ZopfliOptions.GetDefault(), ZopfliFormat.ZLib))
                using (var ims = new MemoryStream(data))
                {
                    ims.CopyTo(zs);
                }
                var compressedData = oms.ToArray();

                Console.WriteLine($"Compress {filePath}: Original=[{data.Length}]Bytes Compressed=[{compressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");

                var decompressedData = ZLibHelper.Decompress(compressedData);
                Assert.Equal(data, decompressedData);
            }
        }

        /// <summary>
        /// Test for <see cref="Zopfli.Compress(byte[], ZopfliFormat)"/> with <see cref="ZopfliFormat.Deflate"/>.
        /// </summary>
        [Fact]
        public void CompressDeflateTest01()
        {
            const string filePath = "Koturn.Zopfli.dll";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            var compressedData = Zopfli.Compress(data, ZopfliFormat.Deflate);

            Console.WriteLine($"Compress {filePath}: Original=[{data.Length}]Bytes Compressed=[{compressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");

            var decompressedData = DeflateHelper.Decompress(compressedData);
            Assert.Equal(data, decompressedData);
        }

        /// <summary>
        /// Test for <see cref="Zopfli.CompressUnmanaged(byte[], ZopfliFormat)"/> with <see cref="ZopfliFormat.Deflate"/>.
        /// </summary>
        [Fact]
        public void CompressUnmanagedDeflateTest01()
        {
            const string filePath = "Koturn.Zopfli.dll";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            using (var compressedData = Zopfli.CompressUnmanaged(data, ZopfliFormat.Deflate))
            {
                Console.WriteLine($"Compress {filePath}: Original=[{data.Length}]Bytes Compressed=[{compressedData.ByteLength}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");
                var decompressedData = DeflateHelper.Decompress(compressedData);
                Assert.Equal(data, decompressedData);
            }
        }

        /// <summary>
        /// Test for <see cref="ZopfliStream"/> with <see cref="ZopfliFormat.Deflate"/>.
        /// </summary>
        [Fact]
        public void CompressDeflateWithStreamTest01()
        {
            const string filePath = "Koturn.Zopfli.dll";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            using (var oms = new MemoryStream())
            {
                using (var zs = new ZopfliStream(oms, ZopfliOptions.GetDefault(), ZopfliFormat.Deflate))
                using (var ims = new MemoryStream(data))
                {
                    ims.CopyTo(zs);
                }
                var compressedData = oms.ToArray();

                Console.WriteLine($"Compress {filePath}: Original=[{data.Length}]Bytes Compressed=[{compressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");

                var decompressedData = DeflateHelper.Decompress(compressedData);
                Assert.Equal(data, decompressedData);
            }
        }
    }
}
