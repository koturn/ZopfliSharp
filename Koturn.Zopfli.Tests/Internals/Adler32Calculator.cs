using System;


namespace Koturn.Zopfli.Tests.Internals
{
    /// <summary>
    /// CRC-32 calculator.
    /// </summary>
    public class Adler32Calculator : IHash<uint>
    {
        /// <summary>
        /// Initial value of CRC-32.
        /// </summary>
        public const uint InitialValue = 0x00000001;
        /// <summary>
        /// Modulo of Adler32, which is the largest prime number that is less than 65536.
        /// </summary>
        private const int Base = 65521;
        /// <summary>
        /// NMax is the largest n such that <c>255 * n * (n + 1) / 2 + (n + 1) * (Base - 1) &lt;= 2 ** 32 - 1</c>
        /// </summary>
        private const int NMax = 5552;

        /// <summary>
        /// Hash value of Adler32.
        /// </summary>
        public uint HashValue => _value;

        /// <summary>
        /// Intermidiate hash value.
        /// </summary>
        private uint _value = InitialValue;


        /// <summary>
        /// Update intermidiate Adler32 value.
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        public void Update(ReadOnlySpan<byte> buf)
        {
            var adler = _value;
            var s1 = adler & 0x0000ffffU;
            var s2 = adler >> 16;

            var offset = 0;
            var count = buf.Length;
            while (count > 0)
            {
                var amount = Math.Min(NMax, count);
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

            _value = (s2 << 16) | s1;
        }
    }
}
