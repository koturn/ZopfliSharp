using System.IO;
using Koturn.Zopfli.Checksums;
using Koturn.Zopfli.Tests.Internals;


namespace Koturn.Zopfli.Tests
{
    /// <summary>
    /// Test class for <see cref="Adler32Util"/> and <see cref="Crc32Util"/>.
    /// </summary>
    public class ChecksumTest
    {
        /// <summary>
        /// Test for <see cref="Adler32Util.Compute(byte[])"/>.
        /// </summary>
        [Fact]
        public void Adler32Test()
        {
            foreach (var filePath in Directory.GetFiles("."))
            {
                var data = File.ReadAllBytes(filePath);

                var adler32 = new Adler32Calculator();
                adler32.Update(data);

                Assert.Equal(adler32.HashValue, Adler32Util.Compute(data));
            }
        }

        /// <summary>
        /// Test for <see cref="Crc32Util.Compute(byte[])"/>.
        /// </summary>
        [Fact]
        public void Crc32Test()
        {
            foreach (var filePath in Directory.GetFiles("."))
            {
                var data = File.ReadAllBytes(filePath);

                var crc32 = new Crc32Calculator();
                crc32.Update(data);

                Assert.Equal(crc32.HashValue, Crc32Util.Compute(data));
            }
        }
    }
}
