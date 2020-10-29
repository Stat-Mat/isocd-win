using NUnit.Framework;
using System;

namespace isocd_builder.Tests {
    /// <summary>
    /// The InputArguments testing class.
    /// Contains all unit tests for the InputArguments class.
    /// </summary>
    [TestFixture]
    public class InputArgumentsTests {
        [Test]
        public void Validate_LongArgsWithLeadingHyphenParsedCorrectly() {
            var args = DataHelpers.GetFullLongArgs("-");
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            var fullArguments = new InputArguments(args.ToArray());
            var parsedFullOptions = fullArguments.ToObject<Options>();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }

        [Test]
        public void Validate_LongArgsWithLeadingDoubleHyphenParsedCorrectly() {
            var args = DataHelpers.GetFullLongArgs("--");
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            var fullArguments = new InputArguments(args.ToArray());
            var parsedFullOptions = fullArguments.ToObject<Options>();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }

        [Test]
        public void Validate_LongArgsWithLeadingForwardSlashParsedCorrectly() {
            var args = DataHelpers.GetFullLongArgs("/");
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            var fullArguments = new InputArguments(args.ToArray());
            var parsedFullOptions = fullArguments.ToObject<Options>();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }

        [Test]
        public void Validate_ShortArgsWithLeadingHyphenParsedCorrectly() {
            var args = DataHelpers.GetFullShortArgs("-");
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            var fullArguments = new InputArguments(args.ToArray());
            var parsedFullOptions = fullArguments.ToObject<Options>();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }

        [Test]
        public void Validate_ShortArgsWithLeadingDoubleHyphenParsedCorrectly() {
            var args = DataHelpers.GetFullShortArgs("--");
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            var fullArguments = new InputArguments(args.ToArray());
            var parsedFullOptions = fullArguments.ToObject<Options>();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }

        [Test]
        public void Validate_ShortArgsWithLeadingForwardSlashParsedCorrectly() {
            var args = DataHelpers.GetFullShortArgs("/");
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            var fullArguments = new InputArguments(args.ToArray());
            var parsedFullOptions = fullArguments.ToObject<Options>();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }

        [Test]
        public void Validate_LongUpperCaseArgsParsedCorrectly() {
            var args = DataHelpers.GetFullLongArgs("-", true);
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            var fullArguments = new InputArguments(args.ToArray());
            var parsedFullOptions = fullArguments.ToObject<Options>();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }

        [Test]
        public void Validate_ShortUpperCaseArgsParsedCorrectly() {
            var args = DataHelpers.GetFullShortArgs("-", true);
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            var fullArguments = new InputArguments(args.ToArray());
            var parsedFullOptions = fullArguments.ToObject<Options>();

            Assert.AreEqual(parsedFullOptions.GetObjectAsJson(), instantiatedFullOptions.GetObjectAsJson());
        }

        [Test]
        public void Validate_ShortArgsWithUnreconisedArgThrowsArgumentException() {
            var args = DataHelpers.GetFullShortArgs("-");

            // Add an unrecognised argument
            args.Add("-ua");
            args.Add("value");

            var fullArguments = new InputArguments(args.ToArray());

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_UNRECOGNISED_ARGUMENT));
                Assert.AreEqual("ua", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_ShortUpperCaseArgsWithUnreconisedArgThrowsArgumentException() {
            var args = DataHelpers.GetFullShortArgs("-", true);

            // Add an unrecognised argument
            args.Add("-UA");
            args.Add("value");

            var fullArguments = new InputArguments(args.ToArray());

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_UNRECOGNISED_ARGUMENT));
                Assert.AreEqual("UA", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_LongArgsWithUnreconisedArgThrowsArgumentException() {
            var args = DataHelpers.GetFullLongArgs("-");

            // Add an unrecognised argument
            args.Add("-UnrecognisedArg");
            args.Add("value");

            var fullArguments = new InputArguments(args.ToArray());

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_UNRECOGNISED_ARGUMENT));
                Assert.AreEqual("UnrecognisedArg", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_LongUpperCaseArgsWithUnreconisedArgThrowsArgumentException() {
            var args = DataHelpers.GetFullLongArgs("-", true);

            // Add an unrecognised argument
            args.Add("-UNRECOGNISEDARG");
            args.Add("value");

            var fullArguments = new InputArguments(args.ToArray());

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_UNRECOGNISED_ARGUMENT));
                Assert.AreEqual("UNRECOGNISEDARG", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_ValueNotProvidedForStringArgThrowsArgumentException() {
            // No value given for input folder
            var args = new string[] {
                "-InputFolder"
            };

            var fullArguments = new InputArguments(args);

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_ARGUMENT_MUST_SPECIFY_STRING_VALUE));
                Assert.AreEqual("InputFolder", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_NullValueForStringArgThrowsArgumentException() {
            // Null input folder
            var args = new string[] {
                "-InputFolder", null
            };

            var fullArguments = new InputArguments(args);

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_ARGUMENT_MUST_SPECIFY_STRING_VALUE));
                Assert.AreEqual("InputFolder", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_ValueNotProvidedForIntegerArgThrowsArgumentException() {
            // No value given for input folder
            var args = new string[] {
                "-DataCache"
            };

            var fullArguments = new InputArguments(args);

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_ARGUMENT_MUST_SPECIFY_INTEGER_VALUE));
                Assert.AreEqual("DataCache", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_NullValueForIntegerArgThrowsArgumentException() {
            // Null input folder
            var args = new string[] {
                "-DataCache", null
            };

            var fullArguments = new InputArguments(args);

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_ARGUMENT_MUST_SPECIFY_INTEGER_VALUE));
                Assert.AreEqual("DataCache", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_NonNumericValueForIntegerArgThrowsArgumentException() {
            // Value is the number word instead of numeric
            var args = new string[] {
                "-DataCache", "one"
            };

            var fullArguments = new InputArguments(args);

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_VALUE_PROVIDED_NOT_VALID_INTEGER));
                Assert.AreEqual("DataCache", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_ValueForIntegerArgTooLargeThrowsArgumentException() {
            // Value is one larger than can be expressed with a 32-bit signed integer
            var args = new string[] {
                "-DataCache", "2147483648"
            };

            var fullArguments = new InputArguments(args);

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_VALUE_PROVIDED_NOT_VALID_INTEGER));
                Assert.AreEqual("DataCache", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_ValueNotProvidedForEnumArgThrowsArgumentException() {
            // No value given for pad size argument
            var args = new string[] {
                "-PadSize"
            };

            var fullArguments = new InputArguments(args);

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_ARGUMENT_MUST_SPECIFY_VALID_VALUE));
                Assert.AreEqual("PadSize", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_NullValueForEnumArgThrowsArgumentException() {
            // Null value given for pad size argument
            var args = new string[] {
                "-PadSize", null
            };

            var fullArguments = new InputArguments(args);

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_ARGUMENT_MUST_SPECIFY_VALID_VALUE));
                Assert.AreEqual("PadSize", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void Validate_ValueForEnumArgNotRecognisedThrowsArgumentException() {
            // Value is not valid for pad size argument
            var args = new string[] {
                "-PadSize", "CDR1000"
            };

            var fullArguments = new InputArguments(args);

            try {
                var parsedFullOptions = fullArguments.ToObject<Options>();
            }
            catch(ArgumentException ex) {
                Assert.IsTrue(ex.Message.StartsWith(isocd_builder_constants.ARG_EXCEPTION_VALUE_PROVIDED_NOT_RECOGNISED));
                Assert.AreEqual("PadSize", ex.ParamName);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }
    }
}
