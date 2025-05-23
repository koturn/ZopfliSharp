using System;
using System.IO;


namespace Koturn.Zopfli
{
    /// <summary>
    /// Provides methods and properties used to optimizing PNG data.
    /// </summary>
    /// <remarks>
    /// Primary ctor: Initializes a new instance of the <see cref="ZopfliPngStream"/> class
    /// by using the specified stream and options.
    /// </remarks>
    /// <param name="stream">Destination stream.</param>
    /// <param name="options">Options for Zopfli PNG optimization.</param>
    /// <param name="leaveOpen">true to leave the stream object open after disposing
    /// the <see cref="ZopfliPngStream"/> object; otherwise, false.</param>
    public class ZopfliPngStream(Stream stream, ZopfliPngOptions options, bool leaveOpen)
        : ZopfliBaseStream(stream, leaveOpen)
    {
        /// <summary>
        /// <para>Initial buffer size.</para>
        /// <para>The value of the largest power of two less than 85K.</para>
        /// </summary>
        private const int InitialBufferSize = 65536;

        /// <summary>
        /// Options for PNG optimization.
        /// </summary>
        /// <remarks>
        /// Used for <seealso cref="ZopfliPng.OptimizePng(byte[], int, int, ZopfliPngOptions, bool)"/>
        /// </remarks>
        public ZopfliPngOptions Options { get; set; } = options;

        /// <summary>
        /// Buffer for reading <see cref="ZopfliBaseStream.BaseStream"/>.
        /// </summary>
        private byte[] _buffer = [];
        /// <summary>
        /// Write postion of <see cref="_buffer"/>.
        /// </summary>
        private int _position = 0;


        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliPngStream"/> class
        /// by using the specified stream.
        /// </summary>
        /// <param name="stream">Destination stream.</param>
        public ZopfliPngStream(Stream stream)
            : this(stream, ZopfliPngOptions.GetDefault(), false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliPngStream"/> class
        /// by using the specified stream.
        /// </summary>
        /// <param name="stream">Destination stream.</param>
        /// <param name="leaveOpen">true to leave the stream object open after disposing
        /// the <see cref="ZopfliPngStream"/> object; otherwise, false.</param>
        public ZopfliPngStream(Stream stream, bool leaveOpen)
            : this(stream, ZopfliPngOptions.GetDefault(), leaveOpen)
        {
        }


        /// <summary>
        /// writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            ThrowIfCannotWrite();
            EnsureCapacity(_position + count);
            Buffer.BlockCopy(buffer, offset, _buffer, _position, count);
            _position += count;
        }

        /// <summary>
        /// <para>Do compress the data and write compressed data to output stream.</para>
        /// <para>This method takes a long time.</para>
        /// <para>After calling this method, you will not be able to write any data.</para>
        /// </summary>
        public override void Flush()
        {
            ThrowIfCannotWrite();
            SetCanWrite(false);

            // Take a long time
            var compressedData = ZopfliPng.OptimizePng(_buffer, 0, _position, Options);

            BaseStream.Write(compressedData, 0, compressedData.Length);
        }


        /// <summary>
        /// Make the size of <see cref="_buffer"/> greater than or equal to the <paramref name="requiredCapacity"/>.
        /// </summary>
        /// <param name="requiredCapacity">Required buffer size.</param>
        /// <returns>true if size of <see cref="_buffer"/> is changed, otherwise false.</returns>
        private bool EnsureCapacity(int requiredCapacity)
        {
            if (requiredCapacity < 0)
            {
                ThrowIOException("Required capacity is too long");
                return false;
            }
            if (requiredCapacity > _buffer.Length)
            {
                _buffer = ChangeBufferSize(_buffer, requiredCapacity < InitialBufferSize ? InitialBufferSize
                    : requiredCapacity < (_buffer.Length * 2) ? (_buffer.Length * 2)
                    : requiredCapacity);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Throw <see cref="IOException"/> if <see cref="ZopfliBaseStream.CanWrite"/> is false.
        /// </summary>
        /// <exception cref="IOException"></exception>
        private void ThrowIfCannotWrite()
        {
            if (!CanWrite)
            {
                ThrowIOException("Buffer is aleady flushed");
            }
        }
    }
}
