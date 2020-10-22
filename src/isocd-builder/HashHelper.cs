using System;
using System.Security.Cryptography;

namespace isocd_builder {
    public class HashHelper {
        public static string GetSHA1HashFromFile(string fileName) {
            using (var stream = System.IO.File.OpenRead(fileName)) {
                using(var sha1 = new SHA1Managed()) {
                    return BitConverter.ToString(sha1.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
                }
            }
        }

        public static string GetSHA1HashFromBuffer(byte[] buffer) {
            using(var sha1 = new SHA1Managed()) {
                return BitConverter.ToString(sha1.ComputeHash(buffer)).Replace("-", string.Empty).ToLower();
            }
        }
    }
}
