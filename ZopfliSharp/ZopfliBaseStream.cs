﻿using System;
using System.IO;


namespace ZopfliSharp
{
    /// <summary>
    /// A super class of <see cref="ZopfliStream"/> and <see cref="ZopfliPngStream"/>.
    /// </summary>
    public abstract class ZopfliBaseStream : Stream
    {
        /// <summary>
        /// <para>Gets a value indicating whether the current stream supports reading.</para>
        /// <para>Always false.</para>
        /// </summary>
        public override bool CanRead => false;
        /// <summary>
        /// <para>Gets a value indicating whether the current stream supports seeking.</para>
        /// <para>Always false.</para>
        /// </summary>
        public override bool CanSeek => false;
        /// <summary>
        /// <para>Gets a value indicating whether the current stream supports writing.</para>
        /// <para>True before <see cref="Flush()"/> or <see cref="Stream.Dispose()"/> is called, otherwise false.</para>
        /// </summary>
        public override bool CanWrite => _canWrite;
        /// <summary>
        /// <para>Gets the length in bytes of the stream.</para>
        /// <para>This property is not supported, always throws <see cref="NotSupportedException"/>.</para>
        /// </summary>
        public override long Length => throw new NotSupportedException($"{nameof(Length)} is not supported");
        /// <summary>
        /// <para>gets or sets the position within the current stream.</para>
        /// <para>This property is not supported, always throws <see cref="NotSupportedException"/>.</para>
        /// </summary>
        public override long Position
        {
            get => throw new NotSupportedException($"{nameof(Position)} is not supported");
            set => throw new NotSupportedException($"{nameof(Position)} is not supported");
        }

        /// <summary>
        /// Destination stream.
        /// </summary>
        public Stream BaseStream { get; }
        /// <summary>
        /// true if the application would like the base stream to remain open
        /// after <see cref="Stream.Close()"/> or <see cref="Stream.Dispose()"/> this stream.
        /// </summary>
        public bool LeaveOpen { get; }

        /// <summary>
        /// Buffer for reading <see cref="BaseStream"/>.
        /// </summary>
        private byte[] _buffer;
        /// <summary>
        /// A value indicating whether the current stream supports writing.
        /// </summary>
        /// <seealso cref="CanWrite"/>
        private bool _canWrite;
        /// <summary>
        /// Write postion of <see cref="_buffer"/>.
        /// </summary>
        private int _position;


        /// <summary>
        /// Initialize all members.
        /// </summary>
        protected ZopfliBaseStream(Stream stream, bool leaveOpen = true)
        {
            BaseStream = stream;
            LeaveOpen = leaveOpen;
            _buffer = new byte[0];
            _canWrite = true;
            _position = 0;
        }


        /// <summary>
        /// <para>Do compress the data and write compressed data to output stream.</para>
        /// <para>This method takes a long time.</para>
        /// <para>After calling this method, you will not be able to write any data.</para>
        /// </summary>
        public override void Flush()
        {
            ThrowIfCannotWrite();
            _canWrite = false;

            // Take a long time
            var compressedData = CompressData(_buffer, 0, _position);

            BaseStream.Write(compressedData, 0, compressedData.Length);
        }


        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream
        /// by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified
        /// byte array with the values between offset and (offset + count - 1) replaced by
        /// the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read
        /// from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer.
        /// This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0)
        /// if the end of the stream has been reached.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException($"{nameof(Read)} is not supported");
        }

        /// <summary>
        /// This method is not supported, always throws <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type System.IO.SeekOrigin indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException($"{nameof(Seek)} is not supported");
        }

        /// <summary>
        /// This method is not supported, always throws <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException($"{nameof(SetLength)} is not supported");
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
        /// Releases the unmanaged resources used by the System.IO.Stream and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            Flush();
            if (!LeaveOpen)
            {
                BaseStream.Close();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Compress specified data.
        /// </summary>
        /// <param name="data">Target data.</param>
        /// <param name="offset">Offset of <paramref name="data"/>.</param>
        /// <param name="count">Data length of <paramref name="data"/>.</param>
        /// <returns>Compressed data.</returns>
        protected abstract byte[] CompressData(byte[] data, int offset, int count);


        /// <summary>
        /// Make the size of <see cref="_buffer"/> greater than or equal to the <paramref name="requiredCapacity"/>.
        /// </summary>
        /// <param name="requiredCapacity">Required buffer size.</param>
        /// <returns>true if size of <see cref="_buffer"/> is changed, otherwise false.</returns>
        private bool EnsureCapacity(int requiredCapacity)
        {
            if (requiredCapacity < 0)
            {
                throw new IOException("Required capacity is too long");
            }
            if (requiredCapacity > _buffer.Length)
            {
                ChangeBufferSize(requiredCapacity < 256 ? 256
                    : requiredCapacity < (_buffer.Length * 2) ? (_buffer.Length * 2)
                    : requiredCapacity);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Change size of <see cref="_buffer"/>.
        /// </summary>
        /// <param name="size">New size of <see cref="_buffer"/>.</param>
        private void ChangeBufferSize(int size)
        {
            if (size == _buffer.Length)
            {
                return;
            }
            var newBuffer = new byte[size];
            Array.Copy(_buffer, 0, newBuffer, 0, Math.Min(_buffer.Length, newBuffer.Length));
            _buffer = newBuffer;
        }

        /// <summary>
        /// Throw <see cref="IOException"/> if <see cref="_canWrite"/> is false.
        /// </summary>
        private void ThrowIfCannotWrite()
        {
            if (!_canWrite)
            {
                throw new IOException("Buffer is aleady flushed");
            }
        }
    }
}
