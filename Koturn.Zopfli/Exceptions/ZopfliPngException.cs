using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;


namespace Koturn.Zopfli.Exceptions
{
    /// <summary>
    /// Represents errors that occur during zopfli compression.
    /// </summary>
    [Serializable]
    public class ZopfliPngException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliPngException"/> class.
        /// </summary>
        public ZopfliPngException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliPngException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ZopfliPngException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliPngException"/> class with
        /// a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference,
        /// the current exception is raised in a catch block that handles the inner exception.</param>
        public ZopfliPngException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliPngException"/> class with a specified error code.
        /// </summary>
        /// <param name="errorCode">The error code from zopflipng.dll.</param>
        public ZopfliPngException(int errorCode)
            : base(CreateErrorMessageFromLodePngErrorCode(errorCode))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliPngException"/> class with
        /// a specified error code and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        /// <param name="errorCode">The error code from zopflipng.dll.</param>
        /// <param name="inner">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference,
        /// the current exception is raised in a catch block that handles the inner exception.</param>
        public ZopfliPngException(int errorCode, Exception inner)
            : base(CreateErrorMessageFromLodePngErrorCode(errorCode), inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZopfliPngException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
        [Obsolete("This ctor is only for .NET Framework", DiagnosticId = "SYSLIB0051")]
#endif  // NET8_0_OR_GREATER
        protected ZopfliPngException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Throws <see cref="ZopfliPngException"/>.
        /// </summary>
        /// <exception cref="ZopfliPngException">Always thrown.</exception>
        [DoesNotReturn]
        public static void Throw()
        {
            throw new ZopfliPngException();
        }

        /// <summary>
        /// Throws <see cref="ZopfliPngException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <exception cref="ZopfliPngException">Always thrown.</exception>
        [DoesNotReturn]
        public static void Throw(string message)
        {
            throw new ZopfliPngException(message);
        }

        /// <summary>
        /// Throws <see cref="ZopfliPngException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference,
        /// the current exception is raised in a catch block that handles the inner exception.</param>
        /// <exception cref="ZopfliPngException">Always thrown.</exception>
        [DoesNotReturn]
        public static void Throw(string message, Exception inner)
        {
            throw new ZopfliPngException(message, inner);
        }

        /// <summary>
        /// Throws <see cref="ZopfliPngException"/>.
        /// </summary>
        /// <param name="errorCode">The error code from zopflipng.dll.</param>
        /// <exception cref="ZopfliPngException">Always thrown.</exception>
        [DoesNotReturn]
        public static void Throw(int errorCode)
        {
            throw new ZopfliPngException(errorCode);
        }

        /// <summary>
        /// Throws <see cref="ZopfliPngException"/>.
        /// </summary>
        /// <param name="errorCode">The error code from zopflipng.dll.</param>
        /// <param name="inner">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference,
        /// the current exception is raised in a catch block that handles the inner exception.</param>
        /// <exception cref="ZopfliPngException">Always thrown.</exception>
        [DoesNotReturn]
        public static void Throw(int errorCode, Exception inner)
        {
            throw new ZopfliPngException(errorCode, inner);
        }

        /// <summary>
        /// Throws <see cref="ZopfliPngException"/> if <paramref name="errorCode"/> is not zero.
        /// </summary>
        /// <param name="errorCode">The error code from zopflipng.dll.</param>
        /// <exception cref="ZopfliPngException">Always thrown.</exception>
        public static void ThrowIfError(int errorCode)
        {
            if (errorCode != 0)
            {
                Throw(errorCode);
            }
        }


        /// <summary>
        /// Create error message from error code of LodePNG.
        /// </summary>
        /// <param name="errorCode">An error code of LodePNG.</param>
        /// <returns>Error message.</returns>
        private static string CreateErrorMessageFromLodePngErrorCode(int errorCode)
        {
            return errorCode switch
            {
                0 => "no error, everything went ok",
                /* the Encoder/Decoder has done nothing yet, error checking makes no sense yet */
                1 => "nothing done yet",
                /* while huffman decoding */
                10 => "end of input memory reached without huffman end code",
                /* while huffman decoding */
                11 => "error in code tree made it jump outside of huffman tree",
                13 => "problem while processing dynamic deflate block",
                14 => "problem while processing dynamic deflate block",
                15 => "problem while processing dynamic deflate block",
                /* this error could happen if there are only 0 or 1 symbols present in the huffman code: */
                16 => "invalid code while processing dynamic deflate block",
                17 => "end of out buffer memory reached while inflating",
                18 => "invalid distance code while inflating",
                19 => "end of out buffer memory reached while inflating",
                20 => "invalid deflate block BTYPE encountered while decoding",
                21 => "NLEN is not ones complement of LEN in a deflate block",
                /*
                 * end of out buffer memory reached while inflating:
                 * This can happen if the inflated deflate data is longer than the amount of bytes required to fill up
                 * all the pixels of the image, given the color depth and image dimensions. Something that doesn't
                 * happen in a normal, well encoded, PNG image.
                 */
                22 => "end of out buffer memory reached while inflating",
                23 => "end of in buffer memory reached while inflating",
                24 => "invalid FCHECK in zlib header",
                25 => "invalid compression method in zlib header",
                26 => "FDICT encountered in zlib header while it's not used for PNG",
                27 => "PNG file is smaller than a PNG header",
                /* Checks the magic file header, the first 8 bytes of the PNG file */
                28 => "incorrect PNG signature, it's no PNG or corrupted",
                29 => "first chunk is not the header chunk",
                30 => "chunk length too large, chunk broken off at end of file",
                31 => "illegal PNG color type or bpp",
                32 => "illegal PNG compression method",
                33 => "illegal PNG filter method",
                34 => "illegal PNG interlace method",
                35 => "chunk length of a chunk is too large or the chunk too small",
                36 => "illegal PNG filter type encountered",
                37 => "illegal bit depth for this color type given",
                38 => "the palette is too small or too big",/* 0, or more than 256 colors */
                39 => "tRNS chunk before PLTE or has more entries than palette size",
                40 => "tRNS chunk has wrong size for grayscale image",
                41 => "tRNS chunk has wrong size for RGB image",
                42 => "tRNS chunk appeared while it was not allowed for this color type",
                43 => "bKGD chunk has wrong size for palette image",
                44 => "bKGD chunk has wrong size for grayscale image",
                45 => "bKGD chunk has wrong size for RGB image",
                48 => "empty input buffer given to decoder. Maybe caused by non-existing file?",
                49 => "jumped past memory while generating dynamic huffman tree",
                50 => "jumped past memory while generating dynamic huffman tree",
                51 => "jumped past memory while inflating huffman block",
                52 => "jumped past memory while inflating",
                53 => "size of zlib data too small",
                54 => "repeat symbol in tree while there was no value symbol yet",
                /*
                 * jumped past tree while generating huffman tree, this could be when the
                 * tree will have more leaves than symbols after generating it out of the
                 * given lengths. They call this an oversubscribed dynamic bit lengths tree in zlib.
                 */
                55 => "jumped past tree while generating huffman tree",
                56 => "given output image colortype or bitdepth not supported for color conversion",
                57 => "invalid CRC encountered (checking CRC can be disabled)",
                58 => "invalid ADLER32 encountered (checking ADLER32 can be disabled)",
                59 => "requested color conversion not supported",
                60 => "invalid window size given in the settings of the encoder (must be 0-32768)",
                61 => "invalid BTYPE given in the settings of the encoder (only 0, 1 and 2 are allowed)",
                /* LodePNG leaves the choice of RGB to grayscale conversion formula to the user. */
                62 => "conversion from color to grayscale not supported",
                /* (2^31-1) */
                63 => "length of a chunk too long, max allowed for PNG is 2147483647 bytes per chunk",
                /* this would result in the inability of a deflated block to ever contain an end code. It must be at least 1. */
                64 => "the length of the END symbol 256 in the Huffman tree is 0",
                66 => "the length of a text chunk keyword given to the encoder is longer than the maximum of 79 bytes",
                67 => "the length of a text chunk keyword given to the encoder is smaller than the minimum of 1 byte",
                68 => "tried to encode a PLTE chunk with a palette that has less than 1 or more than 256 colors",
                69 => "unknown chunk type with 'critical' flag encountered by the decoder",
                71 => "invalid interlace mode given to encoder (must be 0 or 1)",
                72 => "while decoding, invalid compression method encountering in zTXt or iTXt chunk (it must be 0)",
                73 => "invalid tIME chunk size",
                74 => "invalid pHYs chunk size",
                /* length could be wrong, or data chopped off */
                75 => "no null termination char found while decoding text chunk",
                76 => "iTXt chunk too short to contain required bytes",
                77 => "integer overflow in buffer size",
                /* file doesn't exist or couldn't be opened for reading */
                78 => "failed to open file for reading",
                79 => "failed to open file for writing",
                80 => "tried creating a tree of 0 symbols",
                81 => "lazy matching at pos 0 is impossible",
                82 => "color conversion to palette requested while a color isn't in palette, or index out of bounds",
                83 => "memory allocation failed",
                84 => "given image too small to contain all pixels to be encoded",
                86 => "impossible offset in lz77 encoding (internal bug)",
                87 => "must provide custom zlib function pointer if LODEPNG_COMPILE_ZLIB is not defined",
                88 => "invalid filter strategy given for LodePNGEncoderSettings.filter_strategy",
                89 => "text chunk keyword too short or long: must have size 1-79",
                /* the windowsize in the LodePNGCompressSettings. Requiring POT(==> & instead of %) makes encoding 12% faster. */
                90 => "windowsize must be a power of two",
                91 => "invalid decompressed idat size",
                92 => "integer overflow due to too many pixels",
                93 => "zero width or height is invalid",
                94 => "header chunk must have a size of 13 bytes",
                95 => "integer overflow with combined idat chunk size",
                96 => "invalid gAMA chunk size",
                97 => "invalid cHRM chunk size",
                98 => "invalid sRGB chunk size",
                99 => "invalid sRGB rendering intent",
                100 => "invalid ICC profile color type, the PNG specification only allows RGB or GRAY",
                101 => "PNG specification does not allow RGB ICC profile on gray color types and vice versa",
                102 => "not allowed to set grayscale ICC profile with colored pixels by PNG specification",
                103 => "invalid palette index in bKGD chunk. Maybe it came before PLTE chunk?",
                104 => "invalid bKGD color while encoding (e.g. palette index out of range)",
                105 => "integer overflow of bitsize",
                106 => "PNG file must have PLTE chunk if color type is palette",
                107 => "color convert from palette mode requested without setting the palette data in it",
                108 => "tried to add more than 256 values to a palette",
                /* this limit can be configured in LodePNGDecompressSettings */
                109 => "tried to decompress zlib or deflate data larger than desired max_output_size",
                110 => "custom zlib or inflate decompression failed",
                111 => "custom zlib or deflate compression failed",
                /*
                 * max text size limit can be configured in LodePNGDecoderSettings. This error prevents
                 * unreasonable memory consumption when decoding due to impossibly large text sizes.
                 */
                112 => "compressed text unreasonably large",
                /*
                 * max ICC size limit can be configured in LodePNGDecoderSettings. This error prevents
                 * unreasonable memory consumption when decoding due to impossibly large ICC profile
                 */
                113 => "ICC profile unreasonably large",
                _ => "unknown error code",
            };
        }
    }
}
