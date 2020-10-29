using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace isocd_builder {
    /// <summary>
    /// This class defines the core options for the ISO builder. It can be serialised to XML to save application settings. It also utilises CmdLineOptionAttribute to allow
    /// for prescriptive command-line argument parsing. It also implements the IValidatable interface to allow for instances of the class to validate the options provided.
    /// </summary>
    [Serializable]
    public class Options : IValidatable {
        public Options() {
            SetDefaults();
        }

        [CmdLineOption(101, "i", "Input folder", "<path>", MinLength = 1, IsRequired = true)]
        public string InputFolder { get; set; }

        [CmdLineOption(102, "o", "Output file name (ISO)", "<file>", MinLength = 1, IsRequired = true)]
        public string OutputFile { get; set; }

        [CmdLineOption(103, "t", "Trademark file (CDTV or CD32)", "<file>")]
        public string TrademarkFile { get; set; }

        [CmdLineOption(104, "v", "Volume identifier (32 bytes)", "<text>", MaxLength = 32, DefaultValue = isocd_builder_constants.VOLUME_IDENTIFIER)]
        public string VolumeId { get; set; }

        [CmdLineOption(105, "p", "Publisher identifier (128 bytes)", "<text>", MaxLength = 128, DefaultValue = "")]
        public string PublisherId { get; set; }

        [CmdLineOption(106, "a", "Application identifier (128 bytes)", "<text>", MaxLength = 128, DefaultValue = "")]
        public string ApplicationId { get; set; }

        [CmdLineOption(107, "vs", "Volume set identifier (128 bytes)", "<text>", MaxLength = 128, DefaultValue = "")]
        public string VolumeSetId { get; set; }

        [CmdLineOption(108, "dp", "Data preparer identifier (128 bytes)", "<text>", MaxLength = 128, DefaultValue = "")]
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

        [CmdLineOption(118, "ps", "Place data in the outside tracks of the disc to increase reading speed", "<size>", DefaultValue = PadSizeType.None)]
        public PadSizeType PadSize { get; set; }

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
            var attributeDictionary = GetAttributeDictionary();

            foreach(var item in attributeDictionary) {
                var property = type.GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // Check string properties
                if(property.PropertyType == typeof(string)) {
                    var stringValue = (string)property.GetValue(this);

                    if((item.Value.IsRequired || item.Value.MinLength > 0) && string.IsNullOrWhiteSpace(stringValue)) {
                        errors.Add($"{item.Key} - a string value must be provided.");
                    }
                    else if((item.Value.MinLength > item.Value.MaxLength && stringValue.Length < item.Value.MinLength) ||
                            (item.Value.MaxLength > item.Value.MinLength && item.Value.MinLength == 0 && stringValue.Length > item.Value.MaxLength) ||
                            (item.Value.MaxLength > item.Value.MinLength && item.Value.MinLength > 0 && (stringValue.Length < item.Value.MinLength || stringValue.Length > item.Value.MaxLength))
                        ) {
                            errors.Add($"{item.Key} - the string value length must be between {item.Value.MinLength} and {item.Value.MaxLength}.");
                    }
                }

                // Check int properties
                else if(property.PropertyType == typeof(int)) {
                    var intValue = (int)property.GetValue(this);

                    if(intValue < item.Value.MinValue || intValue > item.Value.MaxValue) {
                        errors.Add($"{item.Key} - the value must be between {item.Value.MinValue} and {item.Value.MaxValue}.");
                    }
                }
            }

            // ToValidationResult is an extension method
            var validatonResult = errors.ToValidationResult();
            return validatonResult;
        }

        /// <summary>
        /// Gets a dictionary of all the CmdLineOptionAttributes in this class with the property names as the key.
        /// </summary>
        Dictionary<string, CmdLineOptionAttribute> GetAttributeDictionary() {
            return GetType()
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
        }

        /// <summary>
        /// Overidden ToString() method which outputs all available options with a CmdLineOptionAttribute - can be used to show help.
        /// </summary>
        public override string ToString() {
            var attributeDictionary = GetAttributeDictionary();

            var builder = new StringBuilder();
            var lenLongestName = 0;
            var lenLongestShortName = 0;

            foreach(var item in attributeDictionary) {
                if(item.Key.Length + item.Value.ParamName?.Length > lenLongestName) {
                    lenLongestName = (item.Key.Length + item.Value.ParamName?.Length).Value;
                }

                if(item.Value.ShortName.Length + item.Value.ParamName?.Length > lenLongestShortName) {
                    lenLongestShortName = (item.Value.ShortName.Length + item.Value.ParamName?.Length).Value;
                }
            }

            foreach(var item in attributeDictionary.Where(a => a.Value.IsRequired == true)) {
                builder.AppendLine($" -{item.Key.ToLower()} {item.Value.ParamName}");
            }

            builder.AppendLine(" [MORE OPTIONS]");
            builder.AppendLine($"Full options list:");

            foreach(var item in attributeDictionary) {
                var param = !string.IsNullOrWhiteSpace(item.Value.ParamName) ? $" {item.Value.ParamName}" : "";
                var longPad = (lenLongestName - (item.Key.Length + param.Length - 1));
                var shortPad = (lenLongestShortName - (item.Value.ShortName.Length + param.Length - 1));

                builder.AppendLine($"  -{item.Value.ShortName}{param},{string.Empty.PadLeft(shortPad, ' ')} -{item.Key.ToLower()}{param} {string.Empty.PadLeft(longPad, ' ')}{item.Value.Description}");
            }

            return builder.ToString();
        }
    }
}
