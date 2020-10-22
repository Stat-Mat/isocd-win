using System.Collections.Generic;

// No Newtonsoft.Json so as to avoid dependency on an external library
using System.Runtime.Serialization;

namespace isocd_builder {
    [DataContract]
    public class TmFileSources {
        [DataMember (Name="version")]
        public decimal Version { get; protected set; }

        [DataMember(Name = "cd32sources")]
        public List<Source> Cd32Sources { get; protected set; }

        [DataMember (Name = "cdtvsources")]
        public List<Source> CdtvSources { get; protected set; }
    }

    [DataContract]
    public class Source {
        [DataMember (Name = "url")]
        public string Url { get; protected set; }

        [DataMember (Name = "offset")]
        public int Offset { get; protected set; }
    }
}
