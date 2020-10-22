using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace isocd_builder {
    /// <summary>
    /// This class provides a mechanism for parsing command-line arguments.
    /// </summary>
    public class InputArguments {
        public const string DEFAULT_LEADING_PATTERN = "-";

        protected Dictionary<string, string> _parsedArguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);      
        protected readonly string _leadingPattern;

        public string LeadingPattern {
            get {
                return _leadingPattern;
            }
        }

        public InputArguments(string[] args, string leadingPattern) {
            _leadingPattern = !string.IsNullOrEmpty(leadingPattern) ? leadingPattern : DEFAULT_LEADING_PATTERN;

            if(args != null && args.Length > 0) {
                Parse(args);
            }
        }

        public InputArguments(string[] args) : this(args, null) {
        }

        bool IsArg(string str) {
            return str.StartsWith(_leadingPattern);
        }

        void Parse(string[] args) {
            for(int i = 0; i < args.Length; i++) {
                if(string.IsNullOrWhiteSpace(args[i])) continue;

                string arg = null;
                string val = null;

                if(IsArg(args[i])) {
                    arg = args[i].Substring(1);

                    if(i + 1 < args.Length && !IsArg(args[i + 1])) {
                        val = args[i + 1];
                        i++;
                    }
                }
                else {
                    val = args[i];
                }

                // adjustment
                if(arg == null) {
                    arg = val;
                    val = null;
                }

                _parsedArguments[arg] = val;
            }
        }

        /// <summary>
        /// This method creates an instance of the class <T> and sets all the properties as per the _parsedArguments dictionary.
        /// </summary>
        public T ToObject<T>()
            where T : class, new() {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            try {
                var attributeDictionary = someObjectType
                .GetProperties()
				.Where(p => p.GetCustomAttribute<CmdLineOptionAttribute>() != null)
                .Select(
                    p =>
                    new KeyValuePair<string, string>(
                        p.GetCustomAttribute<CmdLineOptionAttribute>().ShortName.ToLower(),
                        p.Name
                    )
                )
                .ToDictionary(p => p.Key, p => p.Value);

                foreach (var item in _parsedArguments) {
                    var property = someObjectType.GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (property == null && attributeDictionary.ContainsKey(item.Key.ToLower())) {
                        property = someObjectType.GetProperty(attributeDictionary[item.Key.ToLower()], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    }

                    if (property == null) {
                        // If we have any unrecognised arguments, just return null to the caller so that it can show help to the user
                        return null;
                    }

                    if(property.PropertyType == typeof(string)) {
                        // Only set the property if a value was actually provided
                        // i.e. the user could incorrectly pass a string arg on the command line without it being following by a value 
                        if(!string.IsNullOrWhiteSpace(item.Value)) {
                            property.SetValue(someObject, item.Value, null);
                        }
                    }
                    else if (property.PropertyType == typeof(int) && int.TryParse(item.Value, out int intValue)) {
                        property.SetValue(someObject, intValue, null);
                    }
                    else if (property.PropertyType == typeof(bool)) {
                        // The user may have provided a value (i.e. true/false or 0/1), so use if present
                        if(item.Value != null) {
                            property.SetValue(someObject, ToBoolean(item.Value), null);
                        }
                        // User didn't provide a value, so just assume true
                        else {
                            property.SetValue(someObject, true, null);
                        }
                    }
                    else if (property.PropertyType == typeof(PadSize) && Enum.TryParse(item.Value, true, out PadSize padSizeValue)) {
                        property.SetValue(someObject, padSizeValue, null);
                    }
                }

                return someObject;
            }
            catch(Exception) {
                return null;
            }
        }

        /// <summary>
        /// This method takes a string and returns the boolean equivalent.
        /// </summary>
        bool ToBoolean(string value) {
            string[] representationsForFalse = { "false", "0", "off", "no" };

            if(string.IsNullOrEmpty(value) || representationsForFalse.Contains(value.ToLower())) {
                return false;
            }

            return true;
        }
    }
}
