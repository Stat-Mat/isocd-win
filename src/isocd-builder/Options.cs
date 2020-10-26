using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace isocd_builder {
    /// <summary>
    /// This class defines the core options for the ISO builder. It can be serialised to XML to save application settings. It also utilises CmdLineOptionAttribute to allow
    /// for perscriptive command-line argument parsing. It also implements the IValidatable interface to allow for instances of the class to validate the options provided.
    /// </summary>
    [Serializable]
    public class Options : IValidatable {
        public Options() {
            SetDefaults();
        }

        [CmdLineOption(101, "i", "Input folder", "<path>", IsRequired = true)]
        public string InputFolder { get; set; }

        [CmdLineOption(102, "o", "Output file name (ISO)", "<file>", IsRequired = true)]
        public string OutputFile { get; set; }

        [CmdLineOption(103, "t", "Trademark file (CDTV or CD32)", "<file>")]
        public string TrademarkFile { get; set; }

        [CmdLineOption(104, "v", "Volume identifier (32 bytes)", "<text>", MinLength = 1, MaxLength = 32, DefaultValue = isocd_builder_constants.VOLUME_IDENTIFIER)]
        public string VolumeId { get; set; }

        [CmdLineOption(105, "p", "Publisher identifier (128 bytes)", "<text>", MinLength = 1, MaxLength = 128, DefaultValue = "")]
        public string PublisherId { get; set; }

        [CmdLineOption(106, "a", "Application identifier (128 bytes)", "<text>", MinLength = 1, MaxLength = 128, DefaultValue = "")]
        public string ApplicationId { get; set; }

        [CmdLineOption(107, "vs", "Volume set identifier (128 bytes)", "<text>", MinLength = 1, MaxLength = 128, DefaultValue = "")]
        public string VolumeSetId { get; set; }

        [CmdLineOption(108, "dp", "Data preparer identifier (128 bytes)", "<text>", MinLength = 1, MaxLength = 128, DefaultValue = "")]
        public string DataPreparerId { get; set; }

        [CmdLineOption(109, "da", "CDFS data cache size (1 - 127)", "<value>", MinValue = 1, MaxValue = 127, DefaultValue = isocd_builder_constants.DEFAULT_DATA_CACHE)]
        public int DataCache { get; set; }

        [CmdLineOption(110, "dr", "CDFS directory cache size (2 - 127)", "<value>", MinValue = 1, MaxValue = 127, DefaultValue = isocd_builder_constants.DEFAULT_DIR_CACHE)]
        public int DirCache { get; set; }

        [CmdLineOption(111, "fl", "File lock cache size (1 - 9999)", "<value>", MinValue = 1, MaxValue = 9999, DefaultValue = isocd_builder_constants.DEFAULT_FILE_LOCK)]
        public int FileLock { get; set; }

        [CmdLineOption(112, "fh", "File handle cache size (1 - 9999)", "<value>", MinValue = 1, MaxValue = 9999, DefaultValue = isocd_builder_constants.DEFAULT_FILE_HANDLE)]
        public int FileHandle { get; set; }

        [CmdLineOption(113, "r", "Number of read retries (0 - 9999)", "<value>", MinValue = 0, MaxValue = 127, DefaultValue = isocd_builder_constants.DEFAULT_RETRIES)]
        public int Retries { get; set; }

        [CmdLineOption(114, "d", "Use direct read optimisation (CDTV only)", DefaultValue = false)]
        public bool DirectRead { get; set; }

        [CmdLineOption(115, "tm", "Use trademark file", DefaultValue = true)]
        public bool Trademark { get; set; }

        [CmdLineOption(116, "f", "Use fast search optimisation", DefaultValue = true)]
        public bool FastSearch { get; set; }

        [CmdLineOption(117, "s", "Use speed independent (allow reading at higher speeds with newer drives)", DefaultValue = false)]
        public bool SpeedIndependent { get; set; }

        [CmdLineOption(118, "ps", "Place data in the outside tracks of the disc to increase reading speed", "<size>", DefaultValue = PadSize.None)]
        public PadSize PadSize { get; set; }

        [CmdLineOption(119, "ts", "CD32, CDTV or Amiga", "<system>", DefaultValue = TargetSystemType.CD32)]
        public TargetSystemType TargetSystem { get; set; }

        /// <summary>
        /// This sets each property to the default value specified in its CmdLineOptionAttribute if present.
        /// </summary>
        public void SetDefaults() {
            foreach(var property in typeof(Options).GetProperties()
                .Where(p => p.GetCustomAttribute<CmdLineOptionAttribute>() != null && p.GetCustomAttribute<CmdLineOptionAttribute>().DefaultValue != null)) {
                property.SetValue(this, property.GetCustomAttribute<CmdLineOptionAttribute>().DefaultValue, null);
            }
        }

        /// <summary>
        /// This performs validation on the options provided.
        /// </summary>
        public ValidationResult Validate() {
            var errors = new List<string>();
            var type = GetType();

            // Get a dictionary of all the CmdLineOptionAttributes with the property name as key
            var attributeDictionary = type
            .GetProperties()
            .Where(p => p.GetCustomAttribute<CmdLineOptionAttribute>() != null)
            .Select(
                p =>
                new KeyValuePair<string, CmdLineOptionAttribute>(
                    p.Name,
                    p.GetCustomAttribute<CmdLineOptionAttribute>()
                )
            )
            .OrderBy(p => p.Value.Id)
            .ToDictionary(p => p.Key, p => p.Value);

            foreach(var item in attributeDictionary) {
                var property = type.GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // Check string properties
                if(property.PropertyType == typeof(string)) {
                    if(item.Value.IsRequired && string.IsNullOrWhiteSpace((string)property.GetValue(this))) {
                        errors.Add($"{item.Key} - the string must be provided.");
                    }

                    else if(!string.IsNullOrWhiteSpace((string)property.GetValue(this)) &&
                        // If both min and max are zero, we don't check the length
                        (item.Value.MinLength | item.Value.MaxLength) > 0 &&
                    (
                        ((string)property.GetValue(this)).Length < item.Value.MinLength ||
                        ((string)property.GetValue(this)).Length > item.Value.MaxLength
                    )) {
                        errors.Add($"{item.Key} - the string length must be between {item.Value.MinLength} and {item.Value.MaxLength}.");
                    }
                }

                // Check int properties
                else if(property.PropertyType == typeof(int) &&
                    (
                        ((int)property.GetValue(this)) < item.Value.MinValue ||
                        ((int)property.GetValue(this)) > item.Value.MaxValue
                    )) {
                    errors.Add($"{item.Key} - The value must be between {item.Value.MinValue} and {item.Value.MaxValue}.");
                }
            }

            // ToValidationResult is an extension method
            var validatonResult = errors.ToValidationResult();
            return validatonResult;
        }
    }
}
