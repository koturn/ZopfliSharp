using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;


namespace ZopfliSharp.Checksums
{
    /// <summary>
    /// CRC-32 calculation class.
    /// </summary>
    public static class Crc32Util
    {
        /// <summary>
        /// Initial value of CRC-32.
        /// </summary>
        public const uint InitialValue = 0xffffffff;
        /// <summary>
        /// Generator Polynomial of CRC-32.
        /// </summary>
        private const uint Polynomial = 0xedb88320U;

        /// <summary>
        /// Cache of CRC-32 table.
        /// </summary>
        private static uint[]? _table;

        /// <summary>
        /// Compute CRC-32 value.
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <returns>CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Compute(byte[] buf)
        {
            return Compute(buf, 0, buf.Length);
        }

        /// <summary>
        /// Compute CRC-32 value.
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <param name="offset">Offset of <paramref name="buf"/>.</param>
        /// <param name="count">Data count of <paramref name="buf"/>.</param>
        /// <returns>CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Compute(byte[] buf, int offset, int count)
        {
            return Finalize(Update(buf, offset, count));
        }

        /// <summary>
        /// Compute CRC-32 value.
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <returns>CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Compute(ReadOnlySpan<byte> buf)
        {
            return Finalize(Update(buf));
        }

        /// <summary>
        /// <para>Update intermidiate CRC-32 value.</para>
        /// <para>Use default value of <paramref name="crc"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <param name="crc">Intermidiate CRC-32 value.</param>
        /// <returns>Updated intermidiate CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Update(byte[] buf, uint crc = InitialValue)
        {
            return Update(buf, 0, buf.Length, crc);
        }

        /// <summary>
        /// <para>Update intermidiate CRC-32 value.</para>
        /// <para>Use default value of <paramref name="crc"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="byte"/> data array.</param>
        /// <param name="offset">Offset of <paramref name="buf"/>.</param>
        /// <param name="count">Data count of <paramref name="buf"/>.</param>
        /// <param name="crc">Intermidiate CRC-32 value.</param>
        /// <returns>Updated intermidiate CRC-32 value.</returns>
        public static uint Update(byte[] buf, int offset, int count, uint crc = InitialValue)
        {
            var crcTable = GetTable();

            var im = offset + count;
            for (int i = offset; i < im; i++)
            {
                crc = crcTable[(byte)crc ^ buf[i]] ^ (crc >> 8);
            }

            return crc;
        }

        /// <summary>
        /// <para>Update intermidiate CRC-32 value.</para>
        /// <para>Use default value of <paramref name="crc"/> at first time.</para>
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        /// <param name="crc">Intermidiate CRC-32 value.</param>
        /// <returns>Updated intermidiate CRC-32 value.</returns>
        public static uint Update(ReadOnlySpan<byte> buf, uint crc = InitialValue)
        {
            var crcTable = GetTable();

            foreach (var x in buf)
            {
                crc = crcTable[(byte)crc ^ x] ^ (crc >> 8);
            }

            return crc;
        }

        /// <summary>
        /// <para>Update intermidiate CRC-32 value.</para>
        /// <para>Use default value of <paramref name="crc"/> at first time.</para>
        /// </summary>
        /// <param name="x">A value of <see cref="byte"/>.</param>
        /// <param name="crc">Intermidiate CRC-32 value.</param>
        /// <returns>Updated intermidiate CRC-32 value.</returns>
        public static uint Update(byte x, uint crc = InitialValue)
        {
            return GetTable()[(byte)crc ^ x] ^ (crc >> 8);
        }

        /// <summary>
        /// Calculate CRC-32 value from intermidiate CRC-32 value.
        /// </summary>
        /// <param name="crc">Intermidiate CRC-32 value</param>
        /// <returns>CRC-32 value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static uint Finalize(uint crc)
        {
            return ~crc;
        }


        /// <summary>
        /// <para>Get CRC-32 table cache.</para>
        /// <para>If the cache is not generated, generate and return it.</para>
        /// </summary>
        /// <returns>CRC-32 table</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint[] GetTable()
        {
            return _table ??= GenerateTable();
        }

        /// <summary>
        /// Generate CRC-32 value.
        /// This method only used in <see cref="GenerateTable"/>.
        /// </summary>
        /// <returns>CRC-32 table.</returns>
        private static uint[] GenerateTable()
        {
            var crcTable = new uint[256];

            for (int n = 0; n < crcTable.Length; n++)
            {
                var c = (uint)n;
                for (int k = 0; k < 8; k++)
                {
                    c = (c >> 1) ^ ((uint)-(int)(c & 1) & Polynomial);
                }
                crcTable[n] = c;
            }

            return crcTable;
        }
    }
}
