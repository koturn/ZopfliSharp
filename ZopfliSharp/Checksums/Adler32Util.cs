using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;


namespace ZopfliSharp.Checksums
{
    /// <summary>
    /// Adler32 calculation class.
    /// </summary>
    public static class Adler32Util
    {
        /// <summary>
        /// Initial value of Adler32.
        /// </summary>
        public const uint InitialValue = 0x00000001U;

        /// <summary>
        /// Modulo of Adler32, which is the largest prime number that is less than 65536.
        /// </summary>
        private const int Base = 65521;
        /// <summary>
        /// NMax is the largest n such that <c>255 * n * (n + 1) / 2 + (n + 1) * (Base - 1) &lt;= 2 ** 32 - 1</c>
        /// </summary>
        private const int NMax = 5552;


        /// <summary>
        /// Compute Adler32 value.
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <returns>Adler32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static uint Compute(byte[] buf)
        {
            return Compute(buf, 0, buf.Length);
        }

        /// <summary>
        /// Compute Adler32 value.
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <param name="offset">Offset of <paramref name="buf"/>.</param>
        /// <param name="count">Data count of <paramref name="buf"/>.</param>
        /// <returns>Adler32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static uint Compute(byte[] buf, int offset, int count)
        {
            return Update(buf, offset, count);
        }

        /// <summary>
        /// Compute Adler value.
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <returns>Adler32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static uint Compute(ReadOnlySpan<byte> buf)
        {
            return Update(buf);
        }

        /// <summary>
        /// <para>Update intermidiate Adler32 value.</para>
        /// <para>Use default value of <paramref name="adler"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <param name="adler">Intermidiate Adler32 value.</param>
        /// <returns>Updated intermidiate Adler32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static uint Update(byte[] buf, uint adler = InitialValue)
        {
            return Update(buf, 0, buf.Length, adler);
        }

        /// <summary>
        /// <para>Update intermidiate Adler32 value.</para>
        /// <para>Use default value of <paramref name="adler"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <param name="offset">Offset of <paramref name="buf"/>.</param>
        /// <param name="count">Data count of <paramref name="buf"/>.</param>
        /// <param name="adler">Intermidiate Adler32 value.</param>
        /// <returns>Updated intermidiate Adler32 value.</returns>
        [Pure]
        public static uint Update(byte[] buf, int offset, int count, uint adler = InitialValue)
        {
            var s1 = adler & 0x0000ffffU;
            var s2 = adler >> 16;

            while (count > 0)
            {
                var amount = count > NMax ? NMax : count;
                count -= amount;
                for (; amount > 0; amount--)
                {
                    s1 += buf[offset];
                    s2 += s1;
                    offset++;
                }
                s1 %= Base;
                s2 %= Base;
            }

            return (s2 << 16) | s1;
        }

        /// <summary>
        /// <para>Update intermidiate Adler32 value.</para>
        /// <para>Use default value of <paramref name="adler"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <param name="adler">Intermidiate Adler32 value.</param>
        /// <returns>Updated intermidiate Adler32 value.</returns>
        [Pure]
        public static uint Update(ReadOnlySpan<byte> buf, uint adler = InitialValue)
        {
            var s1 = adler & 0x0000ffffU;
            var s2 = adler >> 16;

            var offset = 0;
            var count = buf.Length;
            while (count > 0)
            {
                var amount = count > NMax ? NMax : count;
                count -= amount;
                for (; amount > 0; amount--)
                {
                    s1 += buf[offset];
                    s2 += s1;
                    offset++;
                }
                s1 %= Base;
                s2 %= Base;
            }

            return (s2 << 16) | s1;
        }

        /// <summary>
        /// <para>Update intermidiate Adler32 value.</para>
        /// <para>Use default value of <paramref name="adler"/> at first time.</para>
        /// </summary>
        /// <param name="x">A value of <see cref="byte"/>.</param>
        /// <param name="adler">Intermidiate Adler32 value.</param>
        /// <returns>Updated intermidiate Adler32 value.</returns>
        [Pure]
        public static uint Update(byte x, uint adler = InitialValue)
        {
            var s1 = (adler & 0x0000ffffU) + x;
            if (s1 >= Base)
            {
                s1 -= Base;
            }
            var s2 = (adler >> 16) + s1;
            if (s2 >= Base)
            {
                s2 -= Base;
            }

            return (s2 << 16) | s1;
        }
    }
}
