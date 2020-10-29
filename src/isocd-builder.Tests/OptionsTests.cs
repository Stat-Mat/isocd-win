using NUnit.Framework;

namespace isocd_builder.Tests {
    /// <summary>
    /// The Options testing class.
    /// Contains all unit tests for the Options class.
    /// </summary>
    [TestFixture]
    public class OptionsTests {
        [Test]
        public void Validate_DefaultOptionsValidateSuccessfully() {
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            var validationResult = instantiatedFullOptions.ValidationResult();

            Assert.IsTrue(validationResult.Success);
            Assert.IsTrue(validationResult.NumErrors == 0);
            Assert.AreEqual(validationResult.Message, isocd_builder_constants.VALIDATION_SUCCESSFUL_MESSAGE);
        }

        [Test]
        public void Validate_RequiredStringOptionCannotBeNull() {
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            // Set input folder to null
            instantiatedFullOptions.InputFolder = null;
            var validationResult = instantiatedFullOptions.ValidationResult();

            Assert.IsTrue(validationResult.Success == false);
            Assert.IsTrue(validationResult.NumErrors == 1);
            Assert.IsTrue(validationResult.Message != null);
        }

        [Test]
        public void Validate_RequiredStringOptionCannotBeWhitespace() {
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            // Set input folder to whitespace
            instantiatedFullOptions.InputFolder = " \t\r\n";
            var validationResult = instantiatedFullOptions.ValidationResult();

            Assert.IsTrue(validationResult.Success == false);
            Assert.IsTrue(validationResult.NumErrors == 1);
            Assert.IsTrue(validationResult.Message != null);
        }

        [Test]
        public void Validate_NonRequiredStringOptionCanBeNull() {
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            // Set trademark file to null
            instantiatedFullOptions.TrademarkFile = null;
            var validationResult = instantiatedFullOptions.ValidationResult();

            Assert.IsTrue(validationResult.Success);
            Assert.IsTrue(validationResult.NumErrors == 0);
            Assert.AreEqual(validationResult.Message, isocd_builder_constants.VALIDATION_SUCCESSFUL_MESSAGE);
        }

        [Test]
        public void Validate_MinStringLengthIsEnforced() {
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            // Set input folder to a blank string (i.e. make it's length zero)
            instantiatedFullOptions.InputFolder = "";
            var validationResult = instantiatedFullOptions.ValidationResult();

            Assert.IsTrue(validationResult.Success == false);
            Assert.IsTrue(validationResult.NumErrors == 1);
            Assert.IsTrue(validationResult.Message != null);
        }

        [Test]
        public void Validate_MaxStringLengthIsEnforced() {
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            // Make volume id string length one higher than is allowed
            instantiatedFullOptions.VolumeId = string.Empty.PadLeft(33, ' ');
            var validationResult = instantiatedFullOptions.ValidationResult();

            Assert.IsTrue(validationResult.Success == false);
            Assert.IsTrue(validationResult.NumErrors == 1);
            Assert.IsTrue(validationResult.Message != null);
        }

        [Test]
        public void Validate_MinIntegerValueIsEnforced() {
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            // Set data cache to one lower than is allowed
            instantiatedFullOptions.DataCache = 0;
            var validationResult = instantiatedFullOptions.ValidationResult();

            Assert.IsTrue(validationResult.Success == false);
            Assert.IsTrue(validationResult.NumErrors == 1);
            Assert.IsTrue(validationResult.Message != null);
        }

        [Test]
        public void Validate_MaxIntegerValueIsEnforced() {
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            // Set data cache to one higher than is allowed
            instantiatedFullOptions.DataCache = 128;
            var validationResult = instantiatedFullOptions.ValidationResult();

            Assert.IsTrue(validationResult.Success == false);
            Assert.IsTrue(validationResult.NumErrors == 1);
            Assert.IsTrue(validationResult.Message != null);
        }

        [Test]
        public void Validate_MultipleErrorsDetected() {
            var instantiatedFullOptions = DataHelpers.GetFullOptions();

            // Set input folder to null
            instantiatedFullOptions.InputFolder = null;

            // Set data cache to one higher than is allowed
            instantiatedFullOptions.DataCache = 128;
            var validationResult = instantiatedFullOptions.ValidationResult();

            Assert.IsTrue(validationResult.Success == false);
            Assert.IsTrue(validationResult.NumErrors == 2);
            Assert.IsTrue(validationResult.Message != null);
        }
    }
}
