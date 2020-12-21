using System;
using System.Runtime.Serialization;


namespace ZopfliSharp.Exceptions
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
        protected ZopfliPngException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Create error message from error code of LodePNG.
        /// </summary>
        /// <param name="errorCode">An error code of LodePNG.</param>
        /// <returns>Error message.</returns>
        private static string CreateErrorMessageFromLodePngErrorCode(int errorCode)
        {
            switch (errorCode)
            {
                case 0: return "no error, everything went ok";
                case 1: return "nothing done yet";  /* the Encoder/Decoder has done nothing yet, error checking makes no sense yet */
                case 10: return "end of input memory reached without huffman end code";  /* while huffman decoding */
                case 11: return "error in code tree made it jump outside of huffman tree";  /* while huffman decoding */
                case 13: return "problem while processing dynamic deflate block";
                case 14: return "problem while processing dynamic deflate block";
                case 15: return "problem while processing dynamic deflate block";
                /* this error could happen if there are only 0 or 1 symbols present in the huffman code: */
                case 16: return "invalid code while processing dynamic deflate block";
                case 17: return "end of out buffer memory reached while inflating";
                case 18: return "invalid distance code while inflating";
                case 19: return "end of out buffer memory reached while inflating";
                case 20: return "invalid deflate block BTYPE encountered while decoding";
                case 21: return "NLEN is not ones complement of LEN in a deflate block";

                /*
                 * end of out buffer memory reached while inflating:
                 * This can happen if the inflated deflate data is longer than the amount of bytes required to fill up
                 * all the pixels of the image, given the color depth and image dimensions. Something that doesn't
                 * happen in a normal, well encoded, PNG image.
                 */
                case 22: return "end of out buffer memory reached while inflating";
                case 23: return "end of in buffer memory reached while inflating";
                case 24: return "invalid FCHECK in zlib header";
                case 25: return "invalid compression method in zlib header";
                case 26: return "FDICT encountered in zlib header while it's not used for PNG";
                case 27: return "PNG file is smaller than a PNG header";
                /* Checks the magic file header, the first 8 bytes of the PNG file */
                case 28: return "incorrect PNG signature, it's no PNG or corrupted";
                case 29: return "first chunk is not the header chunk";
                case 30: return "chunk length too large, chunk broken off at end of file";
                case 31: return "illegal PNG color type or bpp";
                case 32: return "illegal PNG compression method";
                case 33: return "illegal PNG filter method";
                case 34: return "illegal PNG interlace method";
                case 35: return "chunk length of a chunk is too large or the chunk too small";
                case 36: return "illegal PNG filter type encountered";
                case 37: return "illegal bit depth for this color type given";
                case 38: return "the palette is too small or too big";  /* 0, or more than 256 colors */
                case 39: return "tRNS chunk before PLTE or has more entries than palette size";
                case 40: return "tRNS chunk has wrong size for grayscale image";
                case 41: return "tRNS chunk has wrong size for RGB image";
                case 42: return "tRNS chunk appeared while it was not allowed for this color type";
                case 43: return "bKGD chunk has wrong size for palette image";
                case 44: return "bKGD chunk has wrong size for grayscale image";
                case 45: return "bKGD chunk has wrong size for RGB image";
                case 48: return "empty input buffer given to decoder. Maybe caused by non-existing file?";
                case 49: return "jumped past memory while generating dynamic huffman tree";
                case 50: return "jumped past memory while generating dynamic huffman tree";
                case 51: return "jumped past memory while inflating huffman block";
                case 52: return "jumped past memory while inflating";
                case 53: return "size of zlib data too small";
                case 54: return "repeat symbol in tree while there was no value symbol yet";
                /*
                 * jumped past tree while generating huffman tree, this could be when the
                 * tree will have more leaves than symbols after generating it out of the
                 * given lengths. They call this an oversubscribed dynamic bit lengths tree in zlib.
                 */
                case 55: return "jumped past tree while generating huffman tree";
                case 56: return "given output image colortype or bitdepth not supported for color conversion";
                case 57: return "invalid CRC encountered (checking CRC can be disabled)";
                case 58: return "invalid ADLER32 encountered (checking ADLER32 can be disabled)";
                case 59: return "requested color conversion not supported";
                case 60: return "invalid window size given in the settings of the encoder (must be 0-32768)";
                case 61: return "invalid BTYPE given in the settings of the encoder (only 0, 1 and 2 are allowed)";
                /* LodePNG leaves the choice of RGB to grayscale conversion formula to the user. */
                case 62: return "conversion from color to grayscale not supported";
                /* (2^31-1) */
                case 63: return "length of a chunk too long, max allowed for PNG is 2147483647 bytes per chunk";
                /* this would result in the inability of a deflated block to ever contain an end code. It must be at least 1. */
                case 64: return "the length of the END symbol 256 in the Huffman tree is 0";
                case 66: return "the length of a text chunk keyword given to the encoder is longer than the maximum of 79 bytes";
                case 67: return "the length of a text chunk keyword given to the encoder is smaller than the minimum of 1 byte";
                case 68: return "tried to encode a PLTE chunk with a palette that has less than 1 or more than 256 colors";
                case 69: return "unknown chunk type with 'critical' flag encountered by the decoder";
                case 71: return "invalid interlace mode given to encoder (must be 0 or 1)";
                case 72: return "while decoding, invalid compression method encountering in zTXt or iTXt chunk (it must be 0)";
                case 73: return "invalid tIME chunk size";
                case 74: return "invalid pHYs chunk size";
                /* length could be wrong, or data chopped off */
                case 75: return "no null termination char found while decoding text chunk";
                case 76: return "iTXt chunk too short to contain required bytes";
                case 77: return "integer overflow in buffer size";
                case 78: return "failed to open file for reading";  /* file doesn't exist or couldn't be opened for reading */
                case 79: return "failed to open file for writing";
                case 80: return "tried creating a tree of 0 symbols";
                case 81: return "lazy matching at pos 0 is impossible";
                case 82: return "color conversion to palette requested while a color isn't in palette, or index out of bounds";
                case 83: return "memory allocation failed";
                case 84: return "given image too small to contain all pixels to be encoded";
                case 86: return "impossible offset in lz77 encoding (internal bug)";
                case 87: return "must provide custom zlib function pointer if LODEPNG_COMPILE_ZLIB is not defined";
                case 88: return "invalid filter strategy given for LodePNGEncoderSettings.filter_strategy";
                case 89: return "text chunk keyword too short or long: must have size 1-79";
                /* the windowsize in the LodePNGCompressSettings. Requiring POT(==> & instead of %) makes encoding 12% faster. */
                case 90: return "windowsize must be a power of two";
                case 91: return "invalid decompressed idat size";
                case 92: return "integer overflow due to too many pixels";
                case 93: return "zero width or height is invalid";
                case 94: return "header chunk must have a size of 13 bytes";
                case 95: return "integer overflow with combined idat chunk size";
                case 96: return "invalid gAMA chunk size";
                case 97: return "invalid cHRM chunk size";
                case 98: return "invalid sRGB chunk size";
                case 99: return "invalid sRGB rendering intent";
                case 100: return "invalid ICC profile color type, the PNG specification only allows RGB or GRAY";
                case 101: return "PNG specification does not allow RGB ICC profile on gray color types and vice versa";
                case 102: return "not allowed to set grayscale ICC profile with colored pixels by PNG specification";
                case 103: return "invalid palette index in bKGD chunk. Maybe it came before PLTE chunk?";
                case 104: return "invalid bKGD color while encoding (e.g. palette index out of range)";
                case 105: return "integer overflow of bitsize";
                case 106: return "PNG file must have PLTE chunk if color type is palette";
                case 107: return "color convert from palette mode requested without setting the palette data in it";
                case 108: return "tried to add more than 256 values to a palette";
                /* this limit can be configured in LodePNGDecompressSettings */
                case 109: return "tried to decompress zlib or deflate data larger than desired max_output_size";
                case 110: return "custom zlib or inflate decompression failed";
                case 111: return "custom zlib or deflate compression failed";
                /*
                 * max text size limit can be configured in LodePNGDecoderSettings. This error prevents
                 * unreasonable memory consumption when decoding due to impossibly large text sizes.
                 */
                case 112: return "compressed text unreasonably large";
                /*
                 * max ICC size limit can be configured in LodePNGDecoderSettings. This error prevents
                 * unreasonable memory consumption when decoding due to impossibly large ICC profile
                 */
                case 113: return "ICC profile unreasonably large";
            }
            return "unknown error code";
        }
    }
}
