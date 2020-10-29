using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Text;

namespace isocd_builder.Tests {
    public static class DataHelpers {
        /// <summary>
        /// This method generates a basic mock file system of a typical Amiga folder structure for use in the tests.
        /// </summary>
        public static MockFileSystem GetMockFileSystem() {
            // We use ISO-8859-1 for the encoding just like AmigaDOS
            var iso_8859_1 = Encoding.GetEncoding("ISO-8859-1");

            var basicFileSystem = new Dictionary<string, MockFileData>() {
                { @"c:\amiga\ReadmeCD³²", new MockFileData("Nothing to see here!", iso_8859_1) },
                { @"c:\amiga\C\cls", new MockFileData(ClsCommand.cls) },
                { @"c:\amiga\S\Startup-sequence", new MockFileData("C:cls\necho \"Hello World!\"\n", iso_8859_1) }
            };

            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>(basicFileSystem));
            mockFileSystem.AddDirectory(@"c:\build");

            // Set the LastWriteTimeUtc for all files and directories to be an arbitrary date and time.
            // This allows us to perform the SHA-1 hash checks against the built ISO images in some of the tests.
            foreach(var path in mockFileSystem.AllPaths) {
                mockFileSystem.File.SetLastWriteTimeUtc(path, new DateTime(2000, 01, 01, 00, 00, 00));
            }

            return mockFileSystem;
        }

        /// <summary>
        /// This method returns a set of full default options.
        /// </summary>
        public static Options GetFullOptions() {
            return new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\cd32.iso",
                TrademarkFile = "CD32.TM",
                VolumeId = "STATMAT_GAMES",
                PublisherId = "STATMAT",
                ApplicationId = "APP_ID",
                VolumeSetId = "VOL_SET_ID",
                DataPreparerId = "STATMAT",
                DataCache = 8,
                DirCache = 16,
                FileLock = 40,
                FileHandle = 16,
                Retries = 32,
                DirectRead = true,
                Trademark = true,
                FastSearch = true,
                SpeedIndependent = true,
                PadSize = PadSizeType.Cdr80,
                TargetSystem = TargetSystemType.CD32
            };
        }

        /// <summary>
        /// This method returns a set of full default short arguments with the defined leading pattern and optionally sets all args to uppercase.
        /// </summary>
        public static List<string> GetFullShortArgs(string leadingPattern, bool makeArgsUpperCase = false) {
            var args = new List<string> {
                $"{leadingPattern}i", @"c:\amiga",
                $"{leadingPattern}o", @"c:\build\cd32.iso",
                $"{leadingPattern}t", "CD32.TM",
                $"{leadingPattern}v", "STATMAT_GAMES",
                $"{leadingPattern}p", "STATMAT",
                $"{leadingPattern}a", "APP_ID",
                $"{leadingPattern}vs", "VOL_SET_ID",
                $"{leadingPattern}dp", "STATMAT",
                $"{leadingPattern}da", "8",
                $"{leadingPattern}dr", "16",
                $"{leadingPattern}fl", "40",
                $"{leadingPattern}fh", "16",
                $"{leadingPattern}r", "32",
                $"{leadingPattern}d",
                $"{leadingPattern}tm",
                $"{leadingPattern}f",
                $"{leadingPattern}s",
                $"{leadingPattern}ps", "cdr80",
                $"{leadingPattern}ts", "cd32",
            };

            if(makeArgsUpperCase) {
                // Only changes the args and not the values themselves
                for(var i = 0; i < args.Count; i += 2) {
                    args[i] = args[i].ToUpper();
                }
            }

            return args;
        }

        /// <summary>
        /// This method returns a set of full default long arguments with the defined leading pattern and optionally sets all args to uppercase.
        /// </summary>
        public static List<string> GetFullLongArgs(string leadingPattern, bool makeArgsUpperCase = false) {
            var args = new List<string> {
                $"{leadingPattern}InputFolder", @"c:\amiga",
                $"{leadingPattern}OutputFile", @"c:\build\cd32.iso",
                $"{leadingPattern}TrademarkFile", "CD32.TM",
                $"{leadingPattern}VolumeId", "STATMAT_GAMES",
                $"{leadingPattern}PublisherId", "STATMAT",
                $"{leadingPattern}ApplicationId", "APP_ID",
                $"{leadingPattern}VolumeSetId", "VOL_SET_ID",
                $"{leadingPattern}DataPreparerId", "STATMAT",
                $"{leadingPattern}DataCache", "8",
                $"{leadingPattern}DirCache", "16",
                $"{leadingPattern}FileLock", "40",
                $"{leadingPattern}FileHandle", "16",
                $"{leadingPattern}Retries", "32",
                $"{leadingPattern}DirectRead",
                $"{leadingPattern}Trademark",
                $"{leadingPattern}FastSearch",
                $"{leadingPattern}SpeedIndependent",
                $"{leadingPattern}PadSize", "CDR80",
                $"{leadingPattern}TargetSystem", "CD32",
            };

            if(makeArgsUpperCase) {
                // Only changes the args and not the values themselves
                for(var i = 0; i < args.Count; i += 2) {
                    args[i] = args[i].ToUpper();
                }
            }

            return args;
        }
    }
}
