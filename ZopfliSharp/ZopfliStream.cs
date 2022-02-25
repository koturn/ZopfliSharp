using System.IO;


namespace ZopfliSharp
{
    /// <summary>
    /// Provides methods and properties used to compressing data.
    /// </summary>
    public class ZopfliStream : ZopfliBaseStream
    {
        /// <summary>
        /// Options for Zopfli compression.
        /// </summary>
        /// <seealso cref="Zopfli.Compress(byte[], in ZopfliOptions)"/>
        /// <seealso cref="Zopfli.Compress(byte[], in ZopfliOptions, ZopfliFormat)"/>
        /// <seealso cref="Zopfli.Compress(byte[], int, int, in ZopfliOptions)"/>
        /// <seealso cref="Zopfli.Compress(byte[], int, int, in ZopfliOptions, ZopfliFormat)"/>
        public ZopfliOptions Options { get; set; }
        /// <summary>
        /// Output binary format.
        /// </summary>
        public ZopfliFormat Format { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliStream"/> class
        /// by using the specified stream.
        /// </summary>
        /// <param name="stream">Destination stream</param>
        /// <param name="leaveOpen">true to leave the stream object open after disposing
        /// the <see cref="ZopfliStream"/> object; otherwise, false.</param>
        public ZopfliStream(Stream stream, bool leaveOpen = true)
            : this(stream, ZopfliOptions.GetDefault(), leaveOpen)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliStream"/> class
        /// by using the specified stream and options.
        /// </summary>
        /// <param name="stream">Destination stream</param>
        /// <param name="options">Options for Zopfli compression.</param>
        /// <param name="leaveOpen">true to leave the stream object open after disposing
        /// the <see cref="ZopfliStream"/> object; otherwise, false.</param>
        public ZopfliStream(Stream stream, in ZopfliOptions options, bool leaveOpen = true)
            : this(stream, options, ZopfliFormat.GZip, leaveOpen)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliStream"/> class
        /// by using the specified stream, options and binary format.
        /// </summary>
        /// <param name="stream">Destination stream</param>
        /// <param name="options">Options for Zopfli compression.</param>
        /// <param name="format">Output binary format.</param>
        /// <param name="leaveOpen">true to leave the stream object open after disposing
        /// the <see cref="ZopfliStream"/> object; otherwise, false.</param>
        public ZopfliStream(Stream stream, in ZopfliOptions options, ZopfliFormat format, bool leaveOpen = true)
            : base(stream, leaveOpen)
        {
            Options = options;
            Format = format;
        }


        /// <summary>
        /// <para>Compress data with zopfli algorithm.</para>
        /// <para>This method will take a long time.</para>
        /// </summary>
        /// <param name="data">Target data.</param>
        /// <param name="offset">Offset of <paramref name="data"/>.</param>
        /// <param name="count">Data length of <paramref name="data"/>.</param>
        /// <returns>Compressed data.</returns>
        protected override byte[] CompressData(byte[] data, int offset, int count)
        {
            return Zopfli.Compress(data, offset, count, Options, Format);
        }
    }
}
