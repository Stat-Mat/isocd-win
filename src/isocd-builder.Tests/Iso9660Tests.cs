using NUnit.Framework;
using System;
using System.IO.Abstractions.TestingHelpers;

namespace isocd_builder.Tests {
    /// <summary>
    /// The Iso9660 testing class.
    /// Contains all unit tests for the Iso9660 class.
    /// </summary>
    [TestFixture]
    public class Iso9660Tests {
        const int CORRECT_ISO_SIZE_WITHOUT_TRADEMARK_FILE = 120832;
        const int CORRECT_ISO_SIZE_WITH_CD32_TRADEMARK_FILE = 122880;
        const int CORRECT_ISO_SIZE_WITH_CDTV_TRADEMARK_FILE = 143360;

        // These were taken from real files which were validated as correct
        const string CORRECT_CD32_ISO_SHA1_HASH = "d096a0cf1696b5070da3b3b21f5b7238158bd991";
        const string CORRECT_CDTV_ISO_SHA1_HASH = "986ca4efa12bd121a182e45174267fdbe1ca6bf3";
        const string CORRECT_AMIGA_ISO_SHA1_HASH = "704b3e53ba7ac28a7834088d99eb235ee78eb9df";

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
                Assert.AreEqual(isocd_builder_constants.ERROR_MESSAGE_INPUT_FOLDER_MUST_EXIST, ex.Message);
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
                Assert.AreEqual(isocd_builder_constants.ERROR_MESSAGE_OUTPUT_FOLDER_MUST_EXIST, ex.Message);
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
                Assert.AreEqual(isocd_builder_constants.ERROR_MESSAGE_TRADEMARK_FILE_MUST_EXIST, ex.Message);
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
                Assert.AreEqual(isocd_builder_constants.ERROR_MESSAGE_INPUT_FOLDER_IS_EMPTY, ex.Message);
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

            Assert.IsTrue(fileSystem.FileExists(basicOptions.OutputFile));
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

            Assert.AreEqual(fileSystem.FileInfo.FromFileName(basicOptions.OutputFile).Length, CORRECT_ISO_SIZE_WITH_CD32_TRADEMARK_FILE);
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

            Assert.AreEqual(fileSystem.FileInfo.FromFileName(basicOptions.OutputFile).Length, CORRECT_ISO_SIZE_WITH_CDTV_TRADEMARK_FILE);
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

            Assert.AreEqual(fileSystem.FileInfo.FromFileName(basicOptions.OutputFile).Length, CORRECT_ISO_SIZE_WITHOUT_TRADEMARK_FILE);
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
            Assert.AreEqual(sha1Hash, CORRECT_CD32_ISO_SHA1_HASH);
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
            Assert.AreEqual(sha1Hash, CORRECT_CDTV_ISO_SHA1_HASH);
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
            Assert.AreEqual(sha1Hash, CORRECT_AMIGA_ISO_SHA1_HASH);
        }
    }
}
