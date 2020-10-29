using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace isocd_builder {
    /// <summary>
    /// This class provides a mechanism for parsing command-line arguments.
    /// </summary>
    public class InputArguments {
        readonly string[] LEADING_PATTERNS = { "--", "-", "/" };

        protected Dictionary<string, string> _parsedArguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public InputArguments(string[] args) {
            if(args != null && args.Length > 0) {
                Parse(args);
            }
        }

        int GetLeadingPatternLength(string str) {
            if(string.IsNullOrWhiteSpace(str)) {
                return 0;
            }

            foreach(var pattern in LEADING_PATTERNS) {
                if(str.StartsWith(pattern)) {
                    return pattern.Length;
                }
            }

            return 0;
        }

        void Parse(string[] args) {
            for(int i = 0; i < args.Length; i++) {
                if(string.IsNullOrWhiteSpace(args[i])) {
                    continue;
                }

                string arg;
                string val = null;

                arg = args[i].Substring(GetLeadingPatternLength(args[i]));

                if(i + 1 < args.Length && GetLeadingPatternLength(args[i + 1]) == 0) {
                    val = args[i + 1];
                    i++;
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

            foreach(var item in _parsedArguments) {
                var property = someObjectType.GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if(property == null && attributeDictionary.ContainsKey(item.Key.ToLower())) {
                    property = someObjectType.GetProperty(attributeDictionary[item.Key.ToLower()], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                }

                if(property == null) {
                    throw new ArgumentException(isocd_builder_constants.ARG_EXCEPTION_UNRECOGNISED_ARGUMENT, $"{item.Key}");
                }

                if(property.PropertyType == typeof(string)) {
                    // Only set the property if a value was actually provided
                    // i.e. the user could incorrectly pass a string arg on the command line without it being following by a value 
                    if(!string.IsNullOrWhiteSpace(item.Value)) {
                        property.SetValue(someObject, item.Value, null);
                    }
                    else {
                        throw new ArgumentException(isocd_builder_constants.ARG_EXCEPTION_ARGUMENT_MUST_SPECIFY_STRING_VALUE, $"{item.Key}");
                    }
                }
                else if(property.PropertyType == typeof(int)) {
                    if(string.IsNullOrWhiteSpace(item.Value)) {
                        throw new ArgumentException(isocd_builder_constants.ARG_EXCEPTION_ARGUMENT_MUST_SPECIFY_INTEGER_VALUE, $"{item.Key}");
                    }
                    else if(int.TryParse(item.Value, out int intValue)) {
                        property.SetValue(someObject, intValue, null);
                    }
                    else {
                        throw new ArgumentException(isocd_builder_constants.ARG_EXCEPTION_VALUE_PROVIDED_NOT_VALID_INTEGER, $"{item.Key}");
                    }
                }
                else if(property.PropertyType == typeof(bool)) {
                    // The user may have provided a value (i.e. true/false or 0/1), so use if present
                    if(item.Value != null) {
                        property.SetValue(someObject, ToBoolean(item.Value), null);
                    }
                    // User didn't provide a value, so just assume true
                    else {
                        property.SetValue(someObject, true, null);
                    }
                }
                else if(property.PropertyType.BaseType != null && property.PropertyType.BaseType == typeof(Enum)) {
                    if(string.IsNullOrWhiteSpace(item.Value)) {
                        throw new ArgumentException(isocd_builder_constants.ARG_EXCEPTION_ARGUMENT_MUST_SPECIFY_VALID_VALUE, $"{item.Key}");
                    }
                    else {
                        object parsedEnum;

                        try {
                            parsedEnum = Convert.ChangeType(Enum.Parse(property.PropertyType, item.Value, true), property.PropertyType);
                        }
                        catch(ArgumentException ex) {
                            throw new ArgumentException(isocd_builder_constants.ARG_EXCEPTION_VALUE_PROVIDED_NOT_RECOGNISED, $"{item.Key}", ex);
                        }

                        property.SetValue(someObject, parsedEnum, null);
                    }
                }
            }

            return someObject;
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
