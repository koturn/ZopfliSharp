﻿using System;
using System.IO;
using ZopfliSharp.Checksums;
using ZopfliSharp.Internal;


namespace ZopfliSharp
{
    /// <summary>
    /// Provides methods and properties used to compressing data.
    /// </summary>
    public class ZopfliStream : ZopfliBaseStream
    {
        /// <summary>
        /// Default cache size (equivalent to master block size).
        /// </summary>
        public const int DefaultCacheSize = 1000000;


        /// <summary>
        /// Options for Zopfli compression.
        /// </summary>
        /// <seealso cref="Zopfli.Compress(byte[], in ZopfliOptions)"/>
        /// <seealso cref="Zopfli.Compress(byte[], in ZopfliOptions, ZopfliFormat)"/>
        /// <seealso cref="Zopfli.Compress(byte[], int, int, in ZopfliOptions)"/>
        /// <seealso cref="Zopfli.Compress(byte[], int, int, in ZopfliOptions, ZopfliFormat)"/>
        public ZopfliOptions Options
        {
            get => _options;
            set => _options = value;
        }
        /// <summary>
        /// Output binary format.
        /// </summary>
        public ZopfliFormat Format { get; set; }
        /// <summary>
        /// <para>A flag that must be set to true when writing the last data if the cache is not used.</para>
        /// <para>If false even at the last data write, an extra few bytes are written in the process at the time of the <see cref="Dispose"/> (<see cref="Stream.Close"/>) call.</para>
        /// </summary>
        public bool IsFinal { get; set; }

        /// <summary>
        /// Options for Zopfli compression.
        /// </summary>
        /// <seealso cref="Options"/>
        private ZopfliOptions _options;
        /// <summary>
        /// Malloced memory handle of compressed data.
        /// </summary>
        private MallocedMemoryHandle _compressedData;
        /// <summary>
        /// Bit pointer of compressed data.
        /// </summary>
        private byte _bitPointer = 0;
        /// <summary>
        /// Check sum value of CRC-32 or adler32.
        /// </summary>
        private uint _checksum;
        /// <summary>
        /// <para>Total amount of input data size.</para>
        /// <para>This value is only used for GZip footer.</para>
        /// </summary>
        private uint _inflatedSize = 0;
        /// <summary>
        /// <para>Cache of input data.</para>
        /// <para>Length of this byte array is equivalent to master block size.</para>
        /// </summary>
        private byte[]? _buffer;
        /// <summary>
        /// Current write position of <see cref="_buffer"/>.
        /// </summary>
        private int _position;
        /// <summary>
        /// A flag whether <see cref="Write(byte[], int, int)"/> was called first or not.
        /// </summary>
        private bool _isHeaderWritten;
        /// <summary>
        /// A flag whether final compression was done or not.
        /// </summary>
        private bool _isFinalCompressDone;


        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliStream"/> class
        /// by using the specified stream.
        /// </summary>
        /// <param name="stream">Destination stream</param>
        /// <param name="leaveOpen">true to leave the stream object open after disposing
        /// the <see cref="ZopfliStream"/> object; otherwise, false.</param>
        public ZopfliStream(Stream stream, bool leaveOpen = true)
            : this(stream, ZopfliOptions.GetDefault(), ZopfliFormat.GZip, DefaultCacheSize, leaveOpen)
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
            : this(stream, options, ZopfliFormat.GZip, DefaultCacheSize, leaveOpen)
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
            : this(stream, options, format, DefaultCacheSize, leaveOpen)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliStream"/> class
        /// by using the specified stream, options and binary format.
        /// </summary>
        /// <param name="stream">Destination stream</param>
        /// <param name="options">Options for Zopfli compression.</param>
        /// <param name="format">Output binary format.</param>
        /// <param name="cacheSize">The size of the data to be compressed at one time. The write data is cached until it reaches this size.</param>
        /// <param name="leaveOpen">true to leave the stream object open after disposing
        /// the <see cref="ZopfliStream"/> object; otherwise, false.</param>
        public ZopfliStream(Stream stream, in ZopfliOptions options, ZopfliFormat format, int cacheSize, bool leaveOpen = true)
            : base(stream, leaveOpen)
        {
            Format = format;
            IsFinal = false;
            _options = options;
            _compressedData = new MallocedMemoryHandle();
            _bitPointer = 0;
            _checksum = 0;
            _inflatedSize = 0;
            _buffer = cacheSize > 0 ? new byte[cacheSize] : null;
            _position = 0;
            _isHeaderWritten = false;
            _isFinalCompressDone = false;
        }


        /// <summary>
        /// writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin compressing bytes.</param>
        /// <param name="count">The number of bytes to be compressed.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            ThrowIfCannotWrite();

            var format = Format;

            // Write header if needed.
            if (!_isHeaderWritten)
            {
                _isHeaderWritten = true;
                if (format == ZopfliFormat.GZip)
                {
                    Zopfli.WriteGZipHeader(BaseStream);
                    _checksum = Crc32Util.InitialValue;
                }
                else if (format == ZopfliFormat.ZLib)
                {
                    Zopfli.WriteZLibHeader(BaseStream);
                    _checksum = Adler32Util.InitialValue;
                }
            }

            // Update checksum.
            if (format == ZopfliFormat.GZip)
            {
                _checksum = Crc32Util.Update(buffer, offset, count, _checksum);
            }
            else if (format == ZopfliFormat.ZLib)
            {
                _checksum = Adler32Util.Update(buffer, offset, count, _checksum);
            }

            var sourceBuffer = _buffer;
            if (sourceBuffer is null)
            {
                // Immediate compress.
                Compress(buffer, 0, count, IsFinal);
            }
            else
            {
                while (count > 0)
                {
                    if (_position == sourceBuffer.Length)
                    {
                        FlushCache(false);
                    }
                    var nWrite = Math.Min(count, sourceBuffer.Length - _position);
                    Buffer.BlockCopy(buffer, offset, sourceBuffer, _position, nWrite);
                    _position += nWrite;
                    offset += nWrite;
                    count -= nWrite;
                }
            }
        }

        /// <summary>
        /// Compress cached data immediately.
        /// </summary>
        public override void Flush()
        {
            if (!(_buffer is null) && _position > 0)
            {
                FlushCache(false);
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the System.IO.Stream and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            WriteFinal();
            base.Dispose(disposing);
        }


        /// <summary>
        /// Compress cached data.
        /// </summary>
        /// <param name="isFinal">A flag that indicates final data or not.</param>
        private void FlushCache(bool isFinal)
        {
            Compress(_buffer!, 0, _position, isFinal);
            _position = 0;
        }

        /// <summary>
        /// Compress a part of buffer.
        /// </summary>
        /// <param name="buffer">Target buffer to compress.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin compressing bytes.</param>
        /// <param name="count">The number of bytes to be compressed.</param>
        /// <param name="isFinal">A flag that indicates final data or not.</param>
        private void Compress(byte[] buffer, int offset, int count, bool isFinal)
        {
            Zopfli.DeflatePart(
                buffer,
                offset,
                count,
                _options,
                BlockType.Dynamic,
                isFinal,
                ref _bitPointer,
                ref _compressedData);

            _inflatedSize += (uint)count;
            if (isFinal)
            {
                _isFinalCompressDone = true;
            }
        }

        /// <summary>
        /// Compress and write final data and footer (if required).
        /// </summary>
        private void WriteFinal()
        {
            // Compress the last part of data.
            if (!(_buffer is null))
            {
                FlushCache(true);
            }
            else if (!_isFinalCompressDone)
            {
                Compress(new byte[1], 0, 0, true);
            }
            // Purge input buffer.
            _buffer = null!;
            // _position = 0;

            var s = BaseStream;

            // Write compressed data.
            WriteData(s, _compressedData);
            _compressedData.Dispose();
            _compressedData = null!;

            // Write footer.
            if (Format == ZopfliFormat.GZip)
            {
                Zopfli.WriteGZipFooter(s, Crc32Util.Finalize(_checksum), _inflatedSize);
            }
            else if (Format == ZopfliFormat.ZLib)
            {
                Zopfli.WriteZLibFooter(s, _checksum);
            }
            s.Flush();

            SetCanWrite(false);
        }

        /// <summary>
        /// Throw <see cref="IOException"/> if <see cref="ZopfliBaseStream.CanWrite"/> is false.
        /// </summary>
        private void ThrowIfCannotWrite()
        {
            if (!CanWrite)
            {
                ThrowIOException("Final data is already wrote out.");
            }
        }

        /// <summary>
        /// Write deflated data to <paramref name="s"/>.
        /// </summary>
        /// <param name="s">Destination stream.</param>
        /// <param name="handle">Memory handle returned from <see cref="Zopfli.DeflatePart(byte[], int, int, in ZopfliOptions, BlockType, bool, ref byte, ref MallocedMemoryHandle)"/>.</param>
        private static void WriteData(Stream s, MallocedMemoryHandle handle)
        {
            WriteData(s, handle, 0, (int)handle.ByteLength);
        }

        /// <summary>
        /// Write deflated data to <paramref name="s"/>.
        /// </summary>
        /// <param name="s">Destination stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="handle"/> at which to begin copying bytes.</param>
        /// <param name="count">The number of bytes to be copied.</param>
        /// <param name="handle">Memory handle returned from <see cref="Zopfli.DeflatePart(byte[], int, int, in ZopfliOptions, BlockType, bool, ref byte, ref MallocedMemoryHandle)"/>.</param>
        private static unsafe void WriteData(Stream s, MallocedMemoryHandle handle, int offset, int count)
        {
            using (var ums = new UnmanagedMemoryStream(handle, offset, count))
            {
                ums.CopyTo(s);
            }
        }
    }
}
