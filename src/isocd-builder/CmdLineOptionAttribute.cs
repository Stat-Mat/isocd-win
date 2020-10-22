using System;

namespace isocd_builder {
    /// <summary>
    /// This class provides an attribute based command-line arguments specification.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CmdLineOptionAttribute : Attribute {
        public int Id { get; }
        public string ShortName { get; }
        public string Description { get; }
        public string ParamName { get; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public bool IsRequired { get; set; }
        public object DefaultValue { get; set;}

        public CmdLineOptionAttribute(int id, string shortName, string description) : this(id, shortName, description, null) {
        }

        public CmdLineOptionAttribute(int id, string shortName, string description, string paramName) {
            Id = id;
            Description = description;
            ShortName = shortName;
            ParamName = paramName;
        }
    }
}
