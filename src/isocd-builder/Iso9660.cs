using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Abstractions;

namespace isocd_builder {
    /// <summary>
    /// This class provides the core functionality to produce an ISO 9660 file system ISO image compatible with AmigaDOS.
    /// </summary>
    public class Iso9660 {
        readonly IFileSystem _fileSystem;
        readonly bool _usingRealFileSystem;

        int indexCounter = 0;
        ushort directoryNumber = 1;

        readonly List<Iso9660Entry> fullEntries = new List<Iso9660Entry>();
        readonly Queue<DirectoryQueueEntry> dirQueue = new Queue<DirectoryQueueEntry>();
        readonly WorkerUpdateStatus reportProgressUserState = new WorkerUpdateStatus();

        readonly Options options;

        // This is our standard constructor in production which uses the the normal System.IO namespace
        public Iso9660(Options options) : this(new FileSystem(), options) {
            _usingRealFileSystem = true;
        }

        // This is our testing constructor which uses the System.IO.Abstractions namespace to allow us to use a mock file system for unit testing
        public Iso9660(IFileSystem fileSystem, Options options) {
            if(fileSystem == null) {
                throw new NullReferenceException("The fileSystem object cannot be null.");
            }

            if(options == null) {
                throw new NullReferenceException("The options object cannot be null.");
            }

            _fileSystem = fileSystem;
            this.options = options;
        }

        /// <summary>
        /// Recursively scans a folder structure to find all files and directories present and generate appropriate records for the ISO 9660 filesystem.
        /// </summary>
        void TreeScan(DirectoryQueueEntry parentDir, ushort parentDirectoryNumber, BuildIsoWorker worker) {
            while(parentDir != null) {
                worker.Token.ThrowIfCancellationRequested();

                var entries = new List<Iso9660Entry>();
                var dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(parentDir.Path);

                entries.AddRange(
                    dirInfo.EnumerateFileSystemInfos()
                    .Select(
                        f => {
                            var entry = new Iso9660Entry {
                                ParentDirectoryIndex = parentDir.Index,
                                ParentDirectoryNumber = parentDirectoryNumber,
                                Path = f.FullName,
                                Name = f.Name,
                                DateStamp = f.LastWriteTimeUtc
                            };

                            if (f is FileInfoBase) {
                                entry.Type = EntryType.File;
                                entry.Size = ((FileInfoBase)f).Length;
                            }
                            else {
                                entry.Type = EntryType.Directory;
                                entry.DirectoryNumber = ++directoryNumber;

                                // Calculate the path table record size for directories
                                entry.PathTableRecordSize = isocd_builder_constants.MINIMUM_PATH_TABLE_RECORD_SIZE + (entry.Identifier.Length - 1);

                                // Padding is only required if the entry identifier length is odd
                                if ((entry.Identifier.Length & 1) == 1) {
                                    entry.PathTableRecordSize++;
                                }
                            }

                            // Calculate the ISO9660 directory record size for each entry
                            entry.DirectoryRecordSize = isocd_builder_constants.MINIMUM_DIR_RECORD_SIZE + (entry.Identifier.Length - 1);

                            // Padding is only required if the entry identifier length is even
                            if ((entry.Identifier.Length & 1) == 0) {
                                entry.DirectoryRecordSize++;
                            }

                            return entry;
                        }
                    )
                );

                // Remove any WinUAE attribute files
                entries.RemoveAll(e => e.Identifier == isocd_builder_constants.WINUAE_ATTRIBUTES_FILE);

                // ISOCD uses a case insensitive sort for the entries based on path
                entries.Sort((x, y) => string.Compare(x.Path, y.Path, StringComparison.OrdinalIgnoreCase));

                foreach (var entry in entries) {
                    // Now that the entries are sorted, set their correct indexes
                    entry.Index = indexCounter++;
                }

                fullEntries.AddRange(entries);

                foreach (var d in entries.Where(e => e.Type == EntryType.Directory)) {
                    dirQueue.Enqueue(new DirectoryQueueEntry {
                        Path = d.Path,
                        Index = d.Index
                    });
                }

                parentDir = null;

                if (dirQueue.Count > 0) {
                    parentDirectoryNumber++;
                    parentDir = dirQueue.Dequeue();
                }
            }
        }

        /// <summary>
        /// Gets the info for a file or directory from the source file system.
        /// </summary>
        void GetEntryInfo(Iso9660Entry entry) {
            var fileInfo = _fileSystem.FileInfo.FromFileName(entry.Path);

            // Store date
            entry.DateStamp = fileInfo.LastWriteTimeUtc;

            // And size if a file
            if(entry.Type == EntryType.File) {
                entry.Size = fileInfo.Length;
            }
            else {
                // Calculate the path table record size for directories
                entry.PathTableRecordSize = isocd_builder_constants.MINIMUM_PATH_TABLE_RECORD_SIZE + (entry.Identifier.Length - 1);

                // Padding is only required if the entry identifier length is odd
                if((entry.Identifier.Length & 1) == 1) {
                    entry.PathTableRecordSize++;
                }
            }

            // Calculate the ISO9660 directory record size for each entry
            entry.DirectoryRecordSize = isocd_builder_constants.MINIMUM_DIR_RECORD_SIZE + (entry.Identifier.Length - 1);

            // Padding is only required if the entry identifier length is even
            if((entry.Identifier.Length & 1) == 0) {
                entry.DirectoryRecordSize++;
            }
        }

        /// <summary>
        /// Writes the CDFS (Compact Disc File System) options to the provided binary stream. Also includes the custom trademark file if provided
        /// to allow booting of the CD on the CD32 or CDTV.
        /// </summary>
        int WriteCDFSOptions(BinaryWriter binWriter, int tmSize, int tmStartSector) {
            var bytesWritten = 0;

            binWriter.Write((byte)0x00);
            bytesWritten++;

            bytesWritten += WriteNumericCDFSOption(binWriter, options.DataCache, isocd_builder_constants.DEFAULT_DATA_CACHE, isocd_builder_constants.CACHE_DATA_NAME);
            bytesWritten += WriteNumericCDFSOption(binWriter, options.DirCache, isocd_builder_constants.DEFAULT_DIR_CACHE, isocd_builder_constants.CACHE_DIR_NAME);
            bytesWritten += WriteNumericCDFSOption(binWriter, options.FileLock, isocd_builder_constants.DEFAULT_FILE_LOCK, isocd_builder_constants.FILE_LOCK_NAME);
            bytesWritten += WriteNumericCDFSOption(binWriter, options.FileHandle, isocd_builder_constants.DEFAULT_FILE_HANDLE, isocd_builder_constants.FILE_HANDLE_NAME);
            bytesWritten += WriteNumericCDFSOption(binWriter, options.Retries, isocd_builder_constants.DEFAULT_RETRIES, isocd_builder_constants.RETRIES_NAME);

            bytesWritten += WriteBooleanCDFSOption(binWriter, options.DirectRead, isocd_builder_constants.DIRECT_READ_NAME);
            bytesWritten += WriteBooleanCDFSOption(binWriter, options.FastSearch, isocd_builder_constants.FAST_SEARCH_NAME);
            bytesWritten += WriteBooleanCDFSOption(binWriter, options.SpeedIndependent, isocd_builder_constants.SPEED_INDEPENDENT_NAME);

            // Include the trademark file if provided
            if(tmSize > 0) {
                binWriter.Write(isocd_builder_constants.TRADEMARK_NAME.ToCharArray());

                // All of these need to be written in big-endian for the Amiga
                binWriter.Write(EndianHelper.ChangeEndian(0x14));
                binWriter.Write(EndianHelper.ChangeEndian((uint)tmSize));
                binWriter.Write(EndianHelper.ChangeEndian((uint)tmStartSector));
                bytesWritten += 12;
            }

            return bytesWritten;
        }

        /// <summary>
        /// Writes a numeric CDFS option to the binary stream.
        /// </summary>
        int WriteNumericCDFSOption(BinaryWriter binWriter, int value, int defaultValue, string name) {
            // Only write the value to the stream if it differs from the system default
            if(value != defaultValue) {
                binWriter.Write(name.ToCharArray());
                binWriter.Write(EndianHelper.ChangeEndian((ushort)0x02));
                binWriter.Write(EndianHelper.ChangeEndian((ushort)value));
                return 6;
            }
            return 0;
        }

        /// <summary>
        /// Writes a boolean CDFS option to the binary stream.
        /// </summary>
        int WriteBooleanCDFSOption(BinaryWriter binWriter, bool value, string name) {
            // Only write to the stream if the option is true
            if(value) {
                binWriter.Write(name.ToCharArray());
                binWriter.Write(EndianHelper.ChangeEndian((ushort)0x00));
                return 4;
            }
            return 0;
        }

        /// <summary>
        /// Writes three volume descriptors specific to ISO9660:
        /// 1. Primary volume descriptor
        /// 2. Supplementary volume descriptor
        /// 3. Volume descriptor set terminator
        /// </summary>
        void WriteVolumeDescriptors(BinaryWriter binWriter,
                                            uint totalSectors,
                                            uint pathTableSize,
                                            uint bigEndianPathTableStartSector,
                                            uint littleEndianPathTableStartSector,
                                            int trademarkSize,
                                            int trademarkStartSector) {
            var volumeDescriptor = new MemoryStream(2048);

            using(var volumeDescriptorWriter = new BinaryWriter(volumeDescriptor)) {
                // Type Code
                volumeDescriptorWriter.Write((byte)0x01);

                // Standard Identifier 
                volumeDescriptorWriter.Write(isocd_builder_constants.STANDARD_IDENTIFIER.ToCharArray());

                // Version
                volumeDescriptorWriter.Write((byte)0x01);

                // Unused 
                volumeDescriptorWriter.Write((byte)0x00);

                // System Identifier
                volumeDescriptorWriter.Write(isocd_builder_constants.SYSTEM_IDENTIFIER.ToCharArray());

                // Volume Identifier 
                volumeDescriptorWriter.Write(options.VolumeId.ToCharArray());
                volumeDescriptorWriter.Seek(32 - options.VolumeId.Length, SeekOrigin.Current);

                // Unused
                volumeDescriptorWriter.Seek(8, SeekOrigin.Current);

                // Volume Space Size
                volumeDescriptorWriter.Write(EndianHelper.BothEndian(totalSectors));

                // Unused
                volumeDescriptorWriter.Seek(32, SeekOrigin.Current);

                // Volume Set Size
                volumeDescriptorWriter.Write(EndianHelper.BothEndian((ushort)0x01));

                // Volume Sequence Number
                volumeDescriptorWriter.Write(EndianHelper.BothEndian((ushort)0x01));

                // Logical Block Size
                volumeDescriptorWriter.Write(EndianHelper.BothEndian((ushort)isocd_builder_constants.SECTOR_SIZE));

                // Path Table Size
                volumeDescriptorWriter.Write(EndianHelper.BothEndian(pathTableSize));

                // Location of Type-L (little endian) Path Table (LBA)
                volumeDescriptorWriter.Write(littleEndianPathTableStartSector);

                // Location of Optional Type-L (little endian) Path Table (LBA)
                // ISOCD uses the location of the primary path table again
                volumeDescriptorWriter.Write(littleEndianPathTableStartSector);

                // Location of Type-M (big endian) Path Table (LBA)
                volumeDescriptorWriter.Write(EndianHelper.ChangeEndian(bigEndianPathTableStartSector));

                // Location of Optional Type-M (big endian) Path Table (LBA)
                // ISOCD uses the location of the primary path table again
                volumeDescriptorWriter.Write(EndianHelper.ChangeEndian(bigEndianPathTableStartSector));

                // Directory entry for the root directory
                WriteDirectoryRecord(fullEntries[0], volumeDescriptorWriter, WriteDirectoryType.FirstDirectoryRecord);

                // Volume Set Identifier (128 bytes)
                volumeDescriptorWriter.Write(options.VolumeSetId.ToCharArray());
                volumeDescriptorWriter.Seek(128 - options.VolumeSetId.Length, SeekOrigin.Current);

                // Publisher Identifier (128 bytes)
                volumeDescriptorWriter.Write(options.PublisherId.ToCharArray());
                volumeDescriptorWriter.Seek(128 - options.PublisherId.Length, SeekOrigin.Current);

                // Data Preparer Identifier (128 bytes)
                // user defined part first, followed by our own
                volumeDescriptorWriter.Write(options.DataPreparerId.ToCharArray());
                volumeDescriptorWriter.Write(isocd_builder_constants.ISOCDWIN_DATA_PREPARER_IDENTIFIER.ToCharArray());
                volumeDescriptorWriter.Seek(128 - options.DataPreparerId.Length - isocd_builder_constants.ISOCDWIN_DATA_PREPARER_IDENTIFIER.Length, SeekOrigin.Current);

                // Application Identifier (128 bytes)
                volumeDescriptorWriter.Write(options.ApplicationId.ToCharArray());
                volumeDescriptorWriter.Seek(128 - options.ApplicationId.Length, SeekOrigin.Current);

                // All zeroed:
                // Copyright File Identifier (38 bytes)
                // Abstract File Identifier (36 bytes)
                // Bibliographic File Identifier (37 bytes)
                volumeDescriptorWriter.Seek(111, SeekOrigin.Current);

                var now = DateTime.Now;

                if(!_usingRealFileSystem) {
                    // As we know we're under test, just set an arbitrary date and time to allow the hash checks to pass
                    now = new DateTime(2000, 01, 01, 00, 00, 00);
                }

                // Volume Creation Date and Time
                volumeDescriptorWriter.Write(
                    string.Format(
                        "{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}{6:D2}{7}",
                        now.Year,
                        now.Month,
                        now.Day,
                        now.Hour,
                        now.Minute,
                        now.Second,
                        // ISOCD always stores hundredths of a second as zero
                        0,
                        // ISOCD ignores the GMT timezone offset and zeroes it
                        '\x00'
                    ).ToCharArray()
                );

                // All zeroed:
                // Volume Modification Date and Time (17 bytes)
                // Volume Expiration Date and Time (17 bytes)
                // Volume Effective Date and Time (17 bytes)
                volumeDescriptorWriter.Seek(51, SeekOrigin.Current);

                // File Structure Version
                volumeDescriptorWriter.Write((byte)0x01);

                // Unused 
                volumeDescriptorWriter.Write((byte)0x00);

                // Application Used
                // ISOCD specific CDFS options
                var bytesWritten = WriteCDFSOptions(volumeDescriptorWriter, trademarkSize, trademarkStartSector);
                volumeDescriptorWriter.Seek(512 - bytesWritten, SeekOrigin.Current);

                // Write the primary and supplementary volume descriptors
                // ISOCD uses the same descriptor twice
                var buf = volumeDescriptor.GetBuffer();
                binWriter.Write(buf);
                binWriter.Write(buf);

                // Write Volume Descriptor Set Terminator
                binWriter.Write((byte)0xFF);
                binWriter.Write(isocd_builder_constants.STANDARD_IDENTIFIER.ToCharArray());
                binWriter.Write((byte)0x01);
                binWriter.Seek(isocd_builder_constants.SECTOR_SIZE - 7, SeekOrigin.Current);
            }
        }

        /// <summary>
        /// Calculates the space required for the ISO9660 path table. The ISO will contain two copies of this, one in big-endian format for the Amiga and
        /// another in little-endian for other systems, both of which are aligned to a sector size of 2048 bytes as per the ISO9660 spec.
        /// </summary>
        int CalcPathTableSize() {
            return fullEntries.Where(e => e.Type == EntryType.Directory).Sum(e => e.PathTableRecordSize);
        }

        /// <summary>
        /// Generates the path table in either big or little-endian format.
        /// </summary>
        byte[] GeneratePathTable(bool littleEndian) {
            var pathTable = new MemoryStream(2048);

            using(var pathTableWriter = new BinaryWriter(pathTable)) {

                foreach(var entry in fullEntries.Where(f => f.Type == EntryType.Directory)) {
                    var paddingByteRequired = entry.Identifier.Length % 2 > 0;

                    // ISOCD stores the directory names in the path table in uppercase (actual names are left intact)
                    var dirId = entry.Identifier.ToUpper();

                    // Length of Directory Identifier
                    pathTableWriter.Write((byte)dirId.Length);

                    // Extended Attribute Record Length
                    pathTableWriter.Write((byte)0x00);

                    // Location of Extent (LBA)
                    if(littleEndian) {
                        pathTableWriter.Write((uint)entry.StartingSector);
                    }
                    else {
                        pathTableWriter.Write(EndianHelper.ChangeEndian((uint)entry.StartingSector));
                    }

                    // Directory number of parent directory
                    if(littleEndian) {
                        pathTableWriter.Write(entry.ParentDirectoryNumber);
                    }
                    else {
                        pathTableWriter.Write(EndianHelper.ChangeEndian(entry.ParentDirectoryNumber));
                    }

                    // Directory Identifier (name)
                    pathTableWriter.Write(dirId.ToCharArray());

                    if(paddingByteRequired) {
                        pathTableWriter.Write((byte)0x00);
                    }
                }

                // Any unused part of the last sector is filled with zeroes,
                // so we must align the buffer to the sector size
                var size = AlignToSectorBoundary((int)pathTable.Length);
                var buf = new byte[size];
                pathTable.Seek(0, SeekOrigin.Begin);
                pathTable.Read(buf, 0, (int)pathTable.Length);

                return buf;
            }
        }

        /// <summary>
        /// Determines the starting sector for each file or directory in the file system. This information is used when generating the path tables.
        /// </summary>
        void GetEntryPositions(int startingSector) {
            long sectorPos;
            var currentSector = startingSector;

            foreach(var entry in fullEntries) {
                if(entry.Type == EntryType.File) {
                    entry.SectorAlignedSize = AlignToSectorBoundary((int)entry.Size);
                    entry.NumberOfSectors = (int)(entry.SectorAlignedSize / isocd_builder_constants.SECTOR_SIZE);
                }
                else if(entry.Type == EntryType.Directory) {
                    // Allow for the first and second directories ("." and "..")     
                    var totalSize = 2 * isocd_builder_constants.MINIMUM_DIR_RECORD_SIZE;
                    sectorPos = totalSize;

                    var children = GetChildEntriesForDirectory(entry.DirectoryNumber);

                    // Calculate the size of the child files and directories
                    foreach(var child in children) {
                        // Directory entries must not cross a sector boundary, so pad the current sector
                        // with zeroes so that the directory entry starts on the next consecutive sector
                        if(sectorPos + child.DirectoryRecordSize > isocd_builder_constants.SECTOR_SIZE) {
                            totalSize += (int)(isocd_builder_constants.SECTOR_SIZE - sectorPos);
                            sectorPos = 0;
                        }

                        sectorPos += child.DirectoryRecordSize;
                        totalSize += child.DirectoryRecordSize;
                    }

                    entry.Size = totalSize;
                    entry.SectorAlignedSize = AlignToSectorBoundary((int)entry.Size);
                    entry.NumberOfSectors = (int)(entry.SectorAlignedSize / isocd_builder_constants.SECTOR_SIZE);
                }

                sectorPos = 0;

                entry.StartingSector = currentSector;
                currentSector += entry.NumberOfSectors;
            }
        }

        /// <summary>
        /// Writes the files and directories to the binary stream.
        /// </summary>
        void WriteFilesAndDirectories(Stream isostream, BinaryWriter binWriter, BuildIsoWorker worker) {
            long sectorPos = 0;

            reportProgressUserState.TotalEntries = fullEntries.Count;
            reportProgressUserState.CurrentEntry = 0;

            foreach(var entry in fullEntries) {
                worker.Token.ThrowIfCancellationRequested();

                reportProgressUserState.CurrentEntry++;

                if(entry.Type == EntryType.File) {
                    // Empty files still occupy a single sector
                    if(entry.Size == 0) {
                        isostream.Seek(isocd_builder_constants.SECTOR_SIZE, SeekOrigin.Current);
                    }
                    else {
                        using(var entrystream = _fileSystem.File.OpenRead(entry.Path)) {
                            entrystream.CopyToWithCancel(isostream, worker.Token);
                        }

                        sectorPos = entry.Size % isocd_builder_constants.SECTOR_SIZE;
                    }
                }
                else {
                    // Add the first and second directories ("." and "..")                
                    WriteDirectoryRecord(fullEntries[entry.Index], binWriter, WriteDirectoryType.FirstDirectoryRecord);
                    WriteDirectoryRecord(fullEntries[entry.ParentDirectoryIndex], binWriter, WriteDirectoryType.SecondDirectoryRecord);
                    sectorPos = 2 * isocd_builder_constants.MINIMUM_DIR_RECORD_SIZE;

                    var children = GetChildEntriesForDirectory(entry.DirectoryNumber);

                    foreach(var child in children) {
                        worker.Token.ThrowIfCancellationRequested();

                        // Directory entries must not cross a sector boundary, so pad the current sector
                        // with zeroes so that the directory entry starts on the next consecutive sector
                        if(sectorPos + child.DirectoryRecordSize > isocd_builder_constants.SECTOR_SIZE) {
                            binWriter.Seek((int)(isocd_builder_constants.SECTOR_SIZE - sectorPos), SeekOrigin.Current);
                            sectorPos = 0;
                        }

                        sectorPos += child.DirectoryRecordSize;

                        WriteDirectoryRecord(child, binWriter);
                    }
                }

                if(sectorPos > 0) {
                    // Pad to the sector boundary with zeroes if necessary
                    binWriter.Seek((int)(isocd_builder_constants.SECTOR_SIZE - sectorPos), SeekOrigin.Current);
                }

                sectorPos = 0;
                worker.ReportProgress(reportProgressUserState);
            }
        }

        /// <summary>
        /// Writes a directory record to the binary stream.
        /// </summary>
        void WriteDirectoryRecord(Iso9660Entry entry, BinaryWriter binWriter, WriteDirectoryType writeDirectoryType = WriteDirectoryType.Normal) {
            // Length of Directory Record
            if(writeDirectoryType == WriteDirectoryType.Normal) {
                binWriter.Write((byte)entry.DirectoryRecordSize);
            }
            else {
                binWriter.Write((byte)isocd_builder_constants.MINIMUM_DIR_RECORD_SIZE);
            }

            // Extended Attribute Record Length
            binWriter.Write((byte)0x00);

            // Location of Extent (LBA)
            binWriter.Write(EndianHelper.BothEndian((uint)entry.StartingSector));

            // Data Length
            if(entry.Type == EntryType.Directory) {
                binWriter.Write(EndianHelper.BothEndian((uint)entry.SectorAlignedSize));
            }
            else {
                binWriter.Write(EndianHelper.BothEndian((uint)entry.Size));
            }

            // Recording Date and Time
            binWriter.Write(entry.BinaryDate);

            // File Flags
            if(entry.Type == EntryType.Directory) {
                binWriter.Write((byte)isocd_builder_constants.DIR_FLAG);
            }
            else {
                binWriter.Write((byte)0x00);
            }

            // All zeroed:
            // File Unit Size (1 byte)
            // Interleave Gap Size (1 byte)
            // Volume Sequence Number (4 bytes)
            binWriter.Seek(6, SeekOrigin.Current);

            // Length of File Identifier
            if(writeDirectoryType == WriteDirectoryType.Normal) {
                binWriter.Write((byte)entry.Identifier.Length);
            }
            else {
                binWriter.Write((byte)0x01);
            }

            // File Identifier
            switch(writeDirectoryType) {
                case WriteDirectoryType.FirstDirectoryRecord:
                    binWriter.Write((byte)0x00);
                    break;
                case WriteDirectoryType.SecondDirectoryRecord:
                    binWriter.Write((byte)0x01);
                    break;
                case WriteDirectoryType.Normal:
                default:
                    binWriter.Write(entry.Identifier.ToCharArray());

                    // Pad record if the identifier length is even
                    if((entry.Identifier.Length & 1) == 0) {
                        binWriter.Write((byte)0x00);
                    }
                    break;
            }
        }

        /// <summary>
        /// Returns all child entries associated with a given directory number.
        /// </summary>
        IEnumerable<Iso9660Entry> GetChildEntriesForDirectory(int directoryNumber) {
            // Excludes root if necessary with Index 0 (which has both ParentDirectoryNumber and DirectoryNumber set to 1)
            return fullEntries.Where(
                e => e.ParentDirectoryNumber == directoryNumber &&
                e.Index != 0
            );
        }

        /// <summary>
        /// Calculates the required sector aligned size to store data of the provided size.
        /// The default sector size for ISO9660 is 2048 bytes.
        /// </summary>
        int AlignToSectorBoundary(int size) {
            // Empty files still occupy a single sector
            if(size == 0) {
                return isocd_builder_constants.SECTOR_SIZE;
            }

            return ((size + (isocd_builder_constants.SECTOR_SIZE - 1)) / isocd_builder_constants.SECTOR_SIZE) * isocd_builder_constants.SECTOR_SIZE;
        }

        /// <summary>
        /// Builds the ISO image in accordance with the options provided when the class was instantiated.
        /// </summary>
        public void BuildIso(BuildIsoWorker worker) {
            // Check provided input folder exists
            if(!_fileSystem.Directory.Exists(options.InputFolder)) {
                throw new InvalidOperationException(isocd_builder_constants.ERROR_MESSAGE_INPUT_FOLDER_MUST_EXIST);
            }

            // Check provided output folder exists
            if(!_fileSystem.Directory.Exists(_fileSystem.Path.GetDirectoryName(options.OutputFile))) {
                throw new InvalidOperationException(isocd_builder_constants.ERROR_MESSAGE_OUTPUT_FOLDER_MUST_EXIST);
            }

            // Check provided trademark file exists
            if(!_fileSystem.File.Exists(options.TrademarkFile) && options.Trademark) {
                throw new InvalidOperationException(isocd_builder_constants.ERROR_MESSAGE_TRADEMARK_FILE_MUST_EXIST);
            }

            fullEntries.Clear();
            indexCounter = 0;
            directoryNumber = 1;

            var useTmFile = options.Trademark & !string.IsNullOrEmpty(options.TrademarkFile);
            byte[] tmBytes = null;

            // Add root record before we begin scanning
            var rootEntry = new Iso9660Entry {
                Index = indexCounter++,
                Type = EntryType.Directory,
                ParentDirectoryNumber = 1,
                DirectoryNumber = 1,
                Path = options.InputFolder,
                Name = "\x01"
            };

            GetEntryInfo(rootEntry);
            fullEntries.Add(rootEntry);

            TreeScan(new DirectoryQueueEntry {
                Path = rootEntry.Path,
                Index = rootEntry.Index
            }, 1, worker);

            // Check provided input folder isn't empty
            if(fullEntries.Count() == 1) {
                throw new InvalidOperationException(isocd_builder_constants.ERROR_MESSAGE_INPUT_FOLDER_IS_EMPTY);
            }

            var pathTableSize = CalcPathTableSize();

            if(useTmFile) {
                using(var tmStream = _fileSystem.File.OpenRead(options.TrademarkFile)) {
                    tmBytes = new byte[tmStream.Length];
                    tmStream.Read(tmBytes, 0, (int)tmStream.Length);
                }
            }
            else {
                tmBytes = new byte[0];
            }

            var trademarkStartingSector =
                // System Area
                16 +
                // 2 * PVDs
                2 +
                // TVD
                1 +
                // Path Tables (big and little-endian)
                2 * (AlignToSectorBoundary(pathTableSize) / isocd_builder_constants.SECTOR_SIZE);

            var directoriesStartingSector = trademarkStartingSector +
                // CDTV.TM / CD32.TM file
                (useTmFile ? AlignToSectorBoundary(tmBytes.Length) : 0) / isocd_builder_constants.SECTOR_SIZE;

            GetEntryPositions(directoriesStartingSector);

            var totalSectors =
                directoriesStartingSector +
                // Total sectors for all directories and files
                fullEntries.Sum(f => f.NumberOfSectors) +
                // 32 sectors of padding (64kb) at the end of the image
                32;

            var maxSectors = 0;

            switch(options.PadSize) {
                case PadSizeType.Cdr74:
                    maxSectors = isocd_builder_constants.MAX_SECTORS_CDR74;
                    break;
                default:
                    maxSectors = isocd_builder_constants.MAX_SECTORS_CDR80;
                    break;
            }

            // Check data will not exceed max sectors
            if(totalSectors > maxSectors) {
                throw new InvalidOperationException(isocd_builder_constants.ERROR_MESSAGE_ISO_IMAGE_TOO_BIG);
            }

            var paddingSectors = 0;

            // Pad image so as to fill a CD-R 74 or CD-R 80 disc if requested
            // This is done to maximize the performance of double speed reading on the CD32 drive
            if(options.PadSize != PadSizeType.None) {
                paddingSectors = maxSectors - totalSectors;
                totalSectors = maxSectors;

                foreach(var entry in fullEntries) {
                    entry.StartingSector += paddingSectors;
                }
            }

            var pathTableLittleEndian = GeneratePathTable(true);
            var pathTableBigEndian = GeneratePathTable(false);

            using (var isoStream = _fileSystem.File.Open(options.OutputFile, FileMode.Create))
            // Use the same character encoding as AmigaDOS
            using (var binWriter = new BinaryWriter(isoStream, Encoding.GetEncoding("ISO-8859-1"))) {
                // Write out the System Area blank sectors at the start of the image (32kb)
                isoStream.Seek(16 * isocd_builder_constants.SECTOR_SIZE, SeekOrigin.Begin);

                WriteVolumeDescriptors(
                    binWriter,
                    (uint)totalSectors,
                    (uint)pathTableSize,
                    // ISOCD always writes the big endian path table first, so the offset is known in advance (0x9800)
                    isocd_builder_constants.BIG_ENDIAN_PATH_TABLE_SECTOR,
                    (uint)(isocd_builder_constants.BIG_ENDIAN_PATH_TABLE_SECTOR + (pathTableBigEndian.Length / isocd_builder_constants.SECTOR_SIZE)),
                    tmBytes.Length,
                    trademarkStartingSector
                );

                // Write big endian path table
                binWriter.Write(pathTableBigEndian);

                // Write little endian path table
                binWriter.Write(pathTableLittleEndian);

                if(useTmFile) {
                    // Write the CDTV.TM / CD32.TM file and align to sector boundary
                    binWriter.Write(tmBytes);
                    binWriter.Seek(AlignToSectorBoundary(tmBytes.Length) - tmBytes.Length, SeekOrigin.Current);
                }

                if(paddingSectors > 0) {
                    worker.ReportProgress(new WorkerUpdateStatus { StatusMessage = "Adding padding to start of image..." });

                    // Add empty space at the beginning of the image to speed up CD reading with doublespeed
                    binWriter.Seek(paddingSectors * isocd_builder_constants.SECTOR_SIZE, SeekOrigin.Current);
                }

                WriteFilesAndDirectories(isoStream, binWriter, worker);

                // Pad out file with 64kb of zeroes
                isoStream.Seek(0xFFFF, SeekOrigin.Current);
                binWriter.Write((byte)0x00);
            }
        }
    }
}
