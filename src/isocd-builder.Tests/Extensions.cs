using System.Web.Script.Serialization;

namespace isocd_builder.Tests {
    public static class Extensions {
        public static string GetObjectAsJson(this object obj) {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            return oSerializer.Serialize(obj);
        }
    }
}
