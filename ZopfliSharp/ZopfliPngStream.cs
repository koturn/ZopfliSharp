using System.IO;


namespace ZopfliSharp
{
    /// <summary>
    /// Provides methods and properties used to optimizing PNG data.
    /// </summary>
    public class ZopfliPngStream : ZopfliBaseStream
    {
        /// <summary>
        /// Options for PNG optimization.
        /// </summary>
        /// <seealso cref="ZopfliPng.OptimizePng(byte[], int, int, ZopfliPNGOptions, bool)"/>
        public ZopfliPNGOptions PNGOptions { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliPngStream"/> class
        /// by using the specified stream.
        /// </summary>
        /// <param name="stream">Destination stream.</param>
        /// <param name="leaveOpen">true to leave the stream object open after disposing
        /// the <see cref="ZopfliPngStream"/> object; otherwise, false.</param>
        public ZopfliPngStream(Stream stream, bool leaveOpen = true)
            : this(stream, ZopfliPNGOptions.GetDefault(), leaveOpen)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliPngStream"/> class
        /// by using the specified stream and options.
        /// </summary>
        /// <param name="stream">Destination stream.</param>
        /// <param name="pngOptions">Options for Zopfli PNG optimization.</param>
        /// <param name="leaveOpen">true to leave the stream object open after disposing
        /// the <see cref="ZopfliPngStream"/> object; otherwise, false.</param>
        public ZopfliPngStream(Stream stream, ZopfliPNGOptions pngOptions, bool leaveOpen = true)
            : base(stream, leaveOpen)
        {
            PNGOptions = pngOptions;
        }


        /// <summary>
        /// <para>Compress PNG data with zopfli algorithm.</para>
        /// <para>This method will take a long time.</para>
        /// </summary>
        /// <param name="data">Target PNG data.</param>
        /// <param name="offset">Offset of <paramref name="data"/>.</param>
        /// <param name="count">Data length of <paramref name="data"/>.</param>
        /// <returns>Compressed PNG data.</returns>
        protected override byte[] CompressData(byte[] data, int offset, int count)
        {
            return ZopfliPng.OptimizePng(data, 0, count, PNGOptions);
        }
    }
}
