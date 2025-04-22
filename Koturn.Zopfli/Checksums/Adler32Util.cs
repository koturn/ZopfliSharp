using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif  // NETCOREAPP3_0_OR_GREATER


namespace Koturn.Zopfli.Checksums
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
            return Update(buf.AsSpan(), adler);
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
            return Update(buf.AsSpan(offset, count), adler);
        }

        /// <summary>
        /// <para>Update intermidiate Adler32 value.</para>
        /// <para>Use default value of <paramref name="adler"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <param name="adler">Intermidiate Adler32 value.</param>
        /// <returns>Updated intermidiate Adler32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static uint Update(ReadOnlySpan<byte> buf, uint adler = InitialValue)
        {
#if NETCOREAPP3_0_OR_GREATER
            // This method call will be repplaced by calling UpdateSsse3() or UpdateNaive() at JIT compiling time.
            return Ssse3.IsSupported ? UpdateSsse3(buf, adler) : UpdateNaive(buf, adler);
#else
            return UpdateNaive(buf, adler);
#endif  // NETCOREAPP3_0_OR_GREATER
        }

#if NETCOREAPP3_0_OR_GREATER
        /// <summary>
        /// <para>Update intermidiate Adler32 value.</para>
        /// <para>Use default value of <paramref name="adler"/> at first time.</para>
        /// <para>This method is implemented with SSSE3.</para>
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <param name="adler">Intermidiate Adler32 value.</param>
        /// <returns>Updated intermidiate Adler32 value.</returns>
        [Pure]
        private static uint UpdateSsse3(ReadOnlySpan<byte> buf, uint adler = InitialValue)
        {
            const uint BlockSize = 1 << 5;
            const uint BlockCount = NMax / BlockSize;

            var s1 = adler & 0x0000ffffU;
            var s2 = adler >> 16;

            var nBlocks = (uint)buf.Length / BlockSize;
            var count = (uint)buf.Length - nBlocks * BlockSize;

            unsafe
            {
                fixed (byte *pBuf = buf)
                {
                    var p = pBuf;

                    var tap1 = Vector128.Create(32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17);
                    var tap2 = Vector128.Create(16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1);
                    var zero = Vector128<byte>.Zero;
                    var ones = Vector128.Create(1, 1, 1, 1, 1, 1, 1, 1);

                    while (nBlocks > 0)
                    {
                        var n = Math.Min(BlockCount, nBlocks);
                        nBlocks -= n;

                        //
                        // Process n nBlocks of data. At most NMAX data bytes can be
                        // processed before s2 must be reduced modulo BASE.
                        //
                        var vps = Vector128.Create(s1 * n, 0, 0, 0);
                        var vs2 = Vector128.Create(s2, 0, 0, 0);
                        var vs1 = Vector128<uint>.Zero;

                        do
                        {
                            //
                            // Load 32 input bytes.
                            //
                            var bytes1 = Sse2.LoadVector128(p);
                            var bytes2 = Sse2.LoadVector128(p + 16);
                            //
                            // Add previous block byte sum to vps.
                            //
                            vps = Sse2.Add(vps, vs1);
                            //
                            // Horizontally add the bytes for s1, multiply-adds the
                            // bytes by [ 32, 31, 30, ... ] for s2.
                            //
                            vs1 = Sse2.Add(vs1, Sse2.SumAbsoluteDifferences(bytes1, zero).AsUInt32());
                            var mad1 = Ssse3.MultiplyAddAdjacent(bytes1, tap1).AsInt16();
                            vs2 = Sse2.Add(vs2, Sse2.MultiplyAddAdjacent(mad1, ones).AsUInt32());

                            vs1 = Sse2.Add(vs1, Sse2.SumAbsoluteDifferences(bytes2, zero).AsUInt32());
                            var mad2 = Ssse3.MultiplyAddAdjacent(bytes2, tap2).AsInt16();
                            vs2 = Sse2.Add(vs2, Sse2.MultiplyAddAdjacent(mad2, ones).AsUInt32());

                            p += BlockSize;
                        } while (--n > 0);

                        vs2 = Sse2.Add(vs2, Sse2.ShiftLeftLogical(vps, 5));

                        //
                        // Sum epi32 ints vs1(s2) and accumulate in s1(s2).
                        //
                        vs1 = Sse2.Add(vs1, Sse2.Shuffle(vs1, 0b10110001));
                        vs1 = Sse2.Add(vs1, Sse2.Shuffle(vs1, 0b01001110));
                        s1 += Sse2.ConvertToUInt32(vs1);

                        vs2 = Sse2.Add(vs2, Sse2.Shuffle(vs2, 0b10110001));
                        vs2 = Sse2.Add(vs2, Sse2.Shuffle(vs2, 0b01001110));
                        s2 = Sse2.ConvertToUInt32(vs2);

                        // Reduce.
                        s1 %= Base;
                        s2 %= Base;
                    }

                    //
                    // Handle leftover data.
                    //
                    if (count > 0) {
                        if (count >= 16) {
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            s2 += (s1 += *p++);
                            count -= 16;
                        }
                        for (; count > 0; count--)
                        {
                            s2 += (s1 += *p++);
                        }
                        if (s1 >= Base)
                        {
                            s1 -= Base;
                        }
                        s2 %= Base;
                    }
                }
            }

            //
            // Return the recombined sums.
            //
            return (s2 << 16) | s1;
        }
#endif  // NETCOREAPP3_0_OR_GREATER

        /// <summary>
        /// <para>Update intermidiate Adler32 value.</para>
        /// <para>Use default value of <paramref name="adler"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <param name="adler">Intermidiate Adler32 value.</param>
        /// <returns>Updated intermidiate Adler32 value.</returns>
        [Pure]
        private static uint UpdateNaive(ReadOnlySpan<byte> buf, uint adler = InitialValue)
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
