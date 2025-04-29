using System;


namespace Koturn.Zopfli.Tests.Internals
{
    /// <summary>
    /// CRC-32 calculator.
    /// </summary>
    public class Crc32Calculator : IHash<uint>
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
        /// Cache of CRC-32 lookup table.
        /// </summary>
        private static readonly uint[] _table = GenerateTable();

        /// <summary>
        /// Hash value of CRC-32.
        /// </summary>
        public uint HashValue => ~_value;

        /// <summary>
        /// Intermidiate hash value.
        /// </summary>
        private uint _value = InitialValue;


        /// <summary>
        /// Update intermidiate CRC-32 value.
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        public void Update(ReadOnlySpan<byte> buf)
        {
            var crc = _value;
            var crcTable = _table;

            foreach (var x in buf)
            {
                crc = crcTable[(byte)crc ^ x] ^ (crc >> 8);
            }

            _value = crc;
        }


        /// <summary>
        /// Generate CRC-32 value.
        /// This method only used in <see cref="GetTable"/>.
        /// </summary>
        /// <returns>CRC-32 table.</returns>
        /// <remarks>
        /// <see href="https://create.stephan-brumme.com/crc32/"/>
        /// </remarks>
        private static uint[] GenerateTable()
        {
            var crcTable = new uint[256];

            for (int n = 0; n < crcTable.Length; n++)
            {
                var c = (uint)n;
                for (var k = 0; k < 8; k++)
                {
                    c = (c >> 1) ^ ((uint)-(int)(c & 1) & Polynomial);
                }
                crcTable[n] = c;
            }

            return crcTable;
        }
    }
}
