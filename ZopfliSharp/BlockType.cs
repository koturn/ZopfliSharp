namespace ZopfliSharp
{
    /// <summary>
    /// Block type of compressed data.
    /// </summary>
    public enum BlockType : int
    {
        /// <summary>
        /// Don't compress data.
        /// </summary>
        NoCompress = 0,
        /// <summary>
        /// Split into fixed size block.
        /// </summary>
        Fixed = 1,
        /// <summary>
        /// Try all block type.
        /// </summary>
        Dynamic = 2
    }
}
