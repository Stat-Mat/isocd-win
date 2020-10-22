using NUnit.Framework;
using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;

namespace isocd_builder.Tests {
    /// <summary>
    /// The isocd-builder testing class.
    /// Contains all unit tests for the isocd-builder library.
    /// </summary>
    [TestFixture]
    public class isocd_builderTests {
        const int CORRECT_ISO_SIZE_WITHOUT_TRADEMARK_FILE = 120832;
        const int CORRECT_ISO_SIZE_WITH_CD32_TRADEMARK_FILE = 122880;
        const int CORRECT_ISO_SIZE_WITH_CDTV_TRADEMARK_FILE = 143360;

        // These were taken from real files which were tested with an emulator
        const string CORRECT_CD32_ISO_SHA1_HASH = "2b36a2c369906f7cf381b0f4587c96921fe7d25d";
        const string CORRECT_CDTV_ISO_SHA1_HASH = "25ed08e850a50e6c78d4b8105ecaef2c6ec004eb";
        const string CORRECT_AMIGA_ISO_SHA1_HASH = "3f3b7458290aa61f4f56d9fb3db5c625229bf620";

        [Test]
        public void Validate_ShouldThrowInvalidOperationExceptionIfInputFolderDoesNotExist() {
            var fileSystem = new MockFileSystem();

            // Only output folder exists
            fileSystem.AddDirectory(@"c:\build");

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\cd32.iso",
                Trademark = true,
                TrademarkFile = @"c:\build\CD32.TM"
            };

            var component = new Iso9660(fileSystem, basicOptions);

            try {
                component.BuildIso(new BuildIsoWorker());
            }
            catch(InvalidOperationException ex) {
                Assert.AreEqual("Provided input folder does not exist!", ex.Message);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_ShouldThrowInvalidOperationExceptionIfOutputFolderDoesNotExist() {
            var fileSystem = new MockFileSystem();

            // Only input folder exists
            fileSystem.AddDirectory(@"c:\amiga");

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\cd32.iso",
                Trademark = true,
                TrademarkFile = @"c:\build\CD32.TM"
            };

            var component = new Iso9660(fileSystem, basicOptions);

            try {
                component.BuildIso(new BuildIsoWorker());
            }
            catch(InvalidOperationException ex) {
                Assert.AreEqual("Provided output folder does not exist!", ex.Message);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_ShouldThrowInvalidOperationExceptionIfTrademarkFileDoesNotExist() {
            var fileSystem = DataHelpers.GetMockFileSystem();

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\cd32.iso",
                Trademark = true,
                TrademarkFile = @"c:\build\CD32.TM"
            };

            var component = new Iso9660(fileSystem, basicOptions);

            try {
                component.BuildIso(new BuildIsoWorker());
            }
            catch(InvalidOperationException ex) {
                Assert.AreEqual("Provided trademark file does not exist!", ex.Message);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_ShouldThrowInvalidOperationExceptionIfInputFolderIsEmpty() {
            var fileSystem = new MockFileSystem();

            // Real CD32.TM is 2048 bytes
            fileSystem.AddFile(@"c:\build\CD32.TM", new MockFileData(new byte[2048]));
            fileSystem.AddDirectory(@"c:\amiga");
            fileSystem.AddDirectory(@"c:\build");

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\cd32.iso",
                Trademark = true,
                TrademarkFile = @"c:\build\CD32.TM"
            };

            var component = new Iso9660(fileSystem, basicOptions);

            try {
                component.BuildIso(new BuildIsoWorker());
            }
            catch(InvalidOperationException ex) {
                Assert.AreEqual("Provided source folder is empty!", ex.Message);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_ShouldNotThrowInvalidOperationExceptionIfInputFolderOutputFolderAndTrademarkFileAllExist() {
            var fileSystem = DataHelpers.GetMockFileSystem();

            // Real CD32.TM is 2048 bytes
            fileSystem.AddFile(@"c:\build\CD32.TM", new MockFileData(new byte[2048]));

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\cd32.iso",
                Trademark = true,
                TrademarkFile = @"c:\build\CD32.TM"
            };

            var component = new Iso9660(fileSystem, basicOptions);

            try {
                component.BuildIso(new BuildIsoWorker());
            }
            catch(InvalidOperationException ex) {
                Assert.Fail($"An InvalidOperationException exception was thrown: {ex.Message}");
                return;
            }

            Assert.Pass("No InvalidOperationException was thrown.");
        }

        [Test]
        public void Validate_OutputFileCreated() {
            var fileSystem = DataHelpers.GetMockFileSystem();

            // Real CD32.TM is 2048 bytes
            fileSystem.AddFile(@"c:\build\CD32.TM", new MockFileData(new byte[2048]));

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\cd32.iso",
                Trademark = true,
                TrademarkFile = @"c:\build\CD32.TM"
            };

            var component = new Iso9660(fileSystem, basicOptions);
            component.BuildIso(new BuildIsoWorker());

            if(fileSystem.FileExists(basicOptions.OutputFile)) {
                Assert.Pass("ISO file created successfully.");
                return;
            }

            Assert.Fail("ISO file was not created.");
        }

        [Test]
        public void Validate_CD32OutputFileIsCorrectSize() {
            var fileSystem = DataHelpers.GetMockFileSystem();

            // Real CD32.TM is 2048 bytes
            fileSystem.AddFile(@"c:\build\CD32.TM", new MockFileData(new byte[2048]));

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\cd32.iso",
                Trademark = true,
                TrademarkFile = @"c:\build\CD32.TM"
            };

            var component = new Iso9660(fileSystem, basicOptions);
            component.BuildIso(new BuildIsoWorker());

            if(fileSystem.FileInfo.FromFileName(basicOptions.OutputFile).Length == CORRECT_ISO_SIZE_WITH_CD32_TRADEMARK_FILE) {
                Assert.Pass("CD32 ISO file was the correct size.");
                return;
            }

            Assert.Fail("CD32 ISO file was not the correct size.");
        }

        [Test]
        public void Validate_CDTVOutputFileIsCorrectSize() {
            var fileSystem = DataHelpers.GetMockFileSystem();

            // Real CDTV.TM is 22152 bytes
            fileSystem.AddFile(@"c:\build\CDTV.TM", new MockFileData(new byte[22152]));

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\cdtv.iso",
                Trademark = true,
                TrademarkFile = @"c:\build\CDTV.TM"
            };

            var component = new Iso9660(fileSystem, basicOptions);
            component.BuildIso(new BuildIsoWorker());

            if(fileSystem.FileInfo.FromFileName(basicOptions.OutputFile).Length == CORRECT_ISO_SIZE_WITH_CDTV_TRADEMARK_FILE) {
                Assert.Pass("CDTV ISO file was the correct size.");
                return;
            }

            Assert.Fail("CDTV ISO file was not the correct size.");
        }

        [Test]
        public void Validate_AmigaOutputFileIsCorrectSize() {
            var fileSystem = DataHelpers.GetMockFileSystem();

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\amiga.iso",
                Trademark = false
            };

            var component = new Iso9660(fileSystem, basicOptions);
            component.BuildIso(new BuildIsoWorker());

            if(fileSystem.FileInfo.FromFileName(basicOptions.OutputFile).Length == CORRECT_ISO_SIZE_WITHOUT_TRADEMARK_FILE) {
                Assert.Pass("Amiga ISO file was the correct size.");
                return;
            }

            Assert.Fail("Amiga ISO file was not the correct size.");
        }

        [Test]
        public void Validate_CD32OutputFileHasCorrectHash() {
            var fileSystem = DataHelpers.GetMockFileSystem();

            // Real CD32.TM is 2048 bytes
            fileSystem.AddFile(@"c:\build\CD32.TM", new MockFileData(new byte[2048]));

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\cd32.iso",
                Trademark = true,
                TrademarkFile = @"c:\build\CD32.TM"
            };

            var component = new Iso9660(fileSystem, basicOptions);
            component.BuildIso(new BuildIsoWorker());

            var sha1Hash = HashHelper.GetSHA1HashFromBuffer(fileSystem.GetFile(basicOptions.OutputFile).Contents);

            if(sha1Hash == CORRECT_CD32_ISO_SHA1_HASH) {
                Assert.Pass("CD32 ISO file has the correct SHA-1 hash.");
                return;
            }

            Assert.Fail($"CD32 ISO file does not have the correct SHA-1 hash: {sha1Hash}");
        }

        [Test]
        public void Validate_CDTVOutputFileHasCorrectHash() {
            var fileSystem = DataHelpers.GetMockFileSystem();

            // Real CDTV.TM is 22152 bytes
            fileSystem.AddFile(@"c:\build\CDTV.TM", new MockFileData(new byte[22152]));

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\cdtv.iso",
                Trademark = true,
                TrademarkFile = @"c:\build\CDTV.TM"
            };

            var component = new Iso9660(fileSystem, basicOptions);
            component.BuildIso(new BuildIsoWorker());

            var sha1Hash = HashHelper.GetSHA1HashFromBuffer(fileSystem.GetFile(basicOptions.OutputFile).Contents);

            if(sha1Hash == CORRECT_CDTV_ISO_SHA1_HASH) {
                Assert.Pass("CDTV ISO file has the correct SHA-1 hash.");
                return;
            }

            Assert.Fail($"CDTV ISO file does not have the correct SHA-1 hash: {sha1Hash}");
        }

        [Test]
        public void Validate_AmigaOutputFileHasCorrectHash() {
            var fileSystem = DataHelpers.GetMockFileSystem();

            var basicOptions = new Options {
                InputFolder = @"c:\amiga",
                OutputFile = @"c:\build\amiga.iso",
                Trademark = false
            };

            var component = new Iso9660(fileSystem, basicOptions);
            component.BuildIso(new BuildIsoWorker());

            var sha1Hash = HashHelper.GetSHA1HashFromBuffer(fileSystem.GetFile(basicOptions.OutputFile).Contents);

            if(sha1Hash == CORRECT_AMIGA_ISO_SHA1_HASH) {
                Assert.Pass("Amiga ISO file has the correct SHA-1 hash.");
                return;
            }

            Assert.Fail($"Amiga ISO file does not have the correct SHA-1 hash: {sha1Hash}");
        }

        [Test]
        public void Validate_ShortArgsParsedCorrectly() {
            string[] args = {
                "-i", @"c:\amiga",
                "-o", @"c:\build\cd32.iso",
                "-t", "CD32.TM",
                "-v", "STATMAT_GAMES",
                "-p", "STATMAT",
                "-a", "APP_ID",
                "-vs", "VOL_SET_ID",
                "-dp", "STATMAT",
                "-da", "8",
                "-dr", "16",
                "-fl", "40",
                "-fh", "16",
                "-r", "32",
                "-d",
                "-tm",
                "-f",
                "-s",
                "-ps", "cdr80",
                "-ts", "cd32"
            };

            var fullArguments = new InputArguments(args);
            var parsedFullOptions = fullArguments.ToObject<Options>();
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }

        [Test]
        public void Validate_ShortUpperCaseArgsParsedCorrectly() {
            string[] args = {
                "-I", @"c:\amiga",
                "-O", @"c:\build\cd32.iso",
                "-T", "CD32.TM",
                "-V", "STATMAT_GAMES",
                "-P", "STATMAT",
                "-A", "APP_ID",
                "-VS", "VOL_SET_ID",
                "-DP", "STATMAT",
                "-DA", "8",
                "-DR", "16",
                "-FL", "40",
                "-FH", "16",
                "-R", "32",
                "-D",
                "-TM",
                "-F",
                "-S",
                "-PS", "cdr80",
                "-TS", "CD32"
            };

            var fullArguments = new InputArguments(args);
            var parsedFullOptions = fullArguments.ToObject<Options>();
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }

        [Test]
        public void Validate_LongArgsParsedCorrectly() {
            string[] args = {
                "-InputFolder", @"c:\amiga",
                "-OutputFile", @"c:\build\cd32.iso",
                "-TrademarkFile", "CD32.TM",
                "-VolumeId", "STATMAT_GAMES",
                "-PublisherId", "STATMAT",
                "-ApplicationId", "APP_ID",
                "-VolumeSetId", "VOL_SET_ID",
                "-DataPreparerId", "STATMAT",
                "-DataCache", "8",
                "-DirCache", "16",
                "-FileLock", "40",
                "-FileHandle", "16",
                "-Retries", "32",
                "-DirectRead",
                "-Trademark",
                "-FastSearch",
                "-SpeedIndependent",
                "-PadSize", "CDR80",
                "-TargetSystem", "CD32"
            };

            var fullArguments = new InputArguments(args);
            var parsedFullOptions = fullArguments.ToObject<Options>();
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }

        [Test]
        public void Validate_LongLowerCaseArgsParsedCorrectly() {
            string[] args = {
                "-inputfolder", @"c:\amiga",
                "-outputfile", @"c:\build\cd32.iso",
                "-trademarkfile", "CD32.TM",
                "-volumeid", "STATMAT_GAMES",
                "-publisherid", "STATMAT",
                "-applicationid", "APP_ID",
                "-volumesetid", "VOL_SET_ID",
                "-datapreparerid", "STATMAT",
                "-datacache", "8",
                "-dircache", "16",
                "-filelock", "40",
                "-filehandle", "16",
                "-retries", "32",
                "-directread",
                "-trademark",
                "-fastsearch",
                "-speedindependent",
                "-padsize", "cdr80",
                "-targetsystem", "cd32"
            };

            var fullArguments = new InputArguments(args);
            var parsedFullOptions = fullArguments.ToObject<Options>();
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }
    }
}
