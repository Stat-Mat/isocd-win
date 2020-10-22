using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Text;

namespace isocd_builder.Tests {
    public static class DataHelpers {
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
                PadSize = PadSize.Cdr80,
                TargetSystem = TargetSystemType.CD32
            };
        }

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
    }
}
