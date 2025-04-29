using System;


namespace Koturn.Zopfli.Tests.Internals
{
    /// <summary>
    /// Hash calculator interface.
    /// </summary>
    /// <typeparam name="T">Hash value type.</typeparam>
    interface IHash<T>
    {
        /// <summary>
        /// Get hash value.
        /// </summary>
        T HashValue { get; }

        /// <summary>
        /// Update hash value.
        /// </summary>
        /// <param name="buf"><see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> data.</param>
        void Update(ReadOnlySpan<byte> buf);
    }
}
