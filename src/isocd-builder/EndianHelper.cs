using System;

namespace isocd_builder {
    /// <summary>
    /// This class provides various methods for working with big and little-endian values.
    /// </summary>
    static class EndianHelper {
        /// <summary>
        /// Transforms a 4 byte unsigned int into a both endian 8 byte unsigned int.
        /// </summary>
        /// <param name="value">A 4 byte unsigned int.</param>
        /// <returns>A 8 byte both endian unsigned int.</returns>
        public static UInt64 BothEndian(UInt32 value) {
            UInt64 mask0 = 0xFF000000;
            UInt64 mask1 = 0x00FF0000;
            UInt64 mask2 = 0x0000FF00;
            UInt64 mask3 = 0x000000FF;

            return (UInt64)value |
                   (UInt64)((value & mask0) << 8) |
                   (UInt64)((value & mask1) << 24) |
                   (UInt64)((value & mask2) << 40) |
                   (UInt64)((value & mask3) << 56);
        }

        /// <summary>
        /// Transforms a 2 byte unsigned int into a both endian 4 byte unsigned int.
        /// </summary>
        /// <param name="value">A 2 byte unsigned int.</param>
        /// <returns>A 4 byte both endian unsigned int.</returns>
        public static UInt32 BothEndian(UInt16 value) {
            UInt32 mask0 = 0xFF00;
            UInt32 mask1 = 0x00FF;

            return (UInt32)value |
                   (UInt32)((value & mask0) << 8) |
                   (UInt32)((value & mask1) << 24);
        }

        /// <summary>
        /// Changes an integer's byte order (big endian->little endian || little endian->big endian).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt32 ChangeEndian(UInt32 value) {
            UInt32 mask0 = 0xFF000000;
            UInt32 mask1 = 0x00FF0000;
            UInt32 mask2 = 0x0000FF00;
            UInt32 mask3 = 0x000000FF;

            return ((value & mask0) >> 24) |
                   ((value & mask1) >> 8) |
                   ((value & mask2) << 8) |
                   ((value & mask3) << 24);
        }

        /// <summary>
        /// Changes a word's byte order (big endian->little endian || little endian->big endian).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt16 ChangeEndian(UInt16 value) {
            return (UInt16)((value >> 8) | (UInt16)((value & 0x00FF) << 8));
        }
    }
}