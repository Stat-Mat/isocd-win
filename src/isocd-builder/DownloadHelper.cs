using System;
using System.IO;
using System.Net;

namespace isocd_builder {
    public static class DownloadHelper {
        public static bool Download(string url, int startPos, int len, string file) {
            try {
                var httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                httpWebRequest.Method = "GET";

                if(len > 0) {
                    httpWebRequest.AddRange(startPos, startPos + (len - 1));
                }

                var localFile = Path.Combine(isocd_builder_constants.ISOCDWIN_PUBLIC_DOCUMENTS_PATH, file);

                using(HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                using(var fileStream = new FileStream(localFile, FileMode.Create, FileAccess.Write, FileShare.Write)) {
                    httpWebResponse.GetResponseStream().CopyTo(fileStream);
                }

                return true;
            }
            catch(Exception) {
                return false;
            }
        }
    }
}
