using System.Linq;

namespace isocd_builder {
    public static class MapperExtensions {
        public static void CopyMatchingProperties<T, TU>(this T source, TU dest) {
            var sourceProps = typeof(T).GetProperties()
                .Where(x => x.CanRead)
                .ToList();

            var destProps = typeof(TU).GetProperties()
                .Where(x => x.CanWrite)
                .ToList();

            foreach(var sourceProp in sourceProps) {
                var p = destProps.FirstOrDefault(x => x.Name == sourceProp.Name);

                if(p != null && p.PropertyType == sourceProp.PropertyType) {
                    p.SetValue(dest, sourceProp.GetValue(source, null), null);
                }
            }
        }
    }
}
