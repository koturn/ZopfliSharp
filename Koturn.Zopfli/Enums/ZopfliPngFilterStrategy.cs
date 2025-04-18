namespace Koturn.Zopfli.Enums
{
    /// <summary>
    /// Enum forzopflipng strategy.
    /// </summary>
    public enum ZopfliPngFilterStrategy
    {
        /// <summary>
        /// Give all scanlines PNG filter type 0.
        /// </summary>
        Zero = 0,
        /// <summary>
        /// Give all scanlines PNG filter type 1.
        /// </summary>
        One = 1,
        /// <summary>
        /// Give all scanlines PNG filter type 2.
        /// </summary>
        Two = 2,
        /// <summary>
        /// Give all scanlines PNG filter type 3.
        /// </summary>
        Three = 3,
        /// <summary>
        /// Give all scanlines PNG filter type 4.
        /// </summary>
        Four = 4,
        /// <summary>
        /// Minimum sum.
        /// </summary>
        MinSum = 5,
        /// <summary>
        /// Entropy.
        /// </summary>
        Entropy = 6,
        /// <summary>
        /// Predefined (keep from input, this likely overlaps another strategy).
        /// </summary>
        Predefined = 7,
        /// <summary>
        /// Brute force (experimental).
        /// </summary>
        BruteForce = 8
    }
}
