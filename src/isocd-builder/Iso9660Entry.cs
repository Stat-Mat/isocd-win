using System;

namespace isocd_builder {
    class Iso9660Entry {
        public int Index;
        public int ParentDirectoryIndex;
        public ushort ParentDirectoryNumber;
        public ushort DirectoryNumber;
        public string Path;

        public string Identifier {
            get {
                if(Type == EntryType.File) {
                    // Add the file version number trailer to the identifier name (ISOCD always uses ;1)
                    return $"{Name};1";
                }
                else {
                    return Name;
                }
            }
        }

        public string Name { get; set; }

        public int DirectoryRecordSize;
        public int PathTableRecordSize;
        public EntryType Type;
        public long Size;
        public long SectorAlignedSize;
        public int NumberOfSectors;
        public int StartingSector;
        DateTime dateStamp;

        public DateTime DateStamp {
            get {
                return dateStamp;
            }
            set {
                dateStamp = value;
                BinaryDate = new byte[] {
                    (byte)(dateStamp.Year - 1900),
                    (byte)(dateStamp.Month),
                    (byte)(dateStamp.Day),
                    (byte)(dateStamp.Hour),
                    (byte)(dateStamp.Minute),
                    (byte)(dateStamp.Second),
                    // ISOCD always puts timezone offset as zero
                    0
                };
            }
        }

        public byte[] BinaryDate { get; protected set; }
    }
}
