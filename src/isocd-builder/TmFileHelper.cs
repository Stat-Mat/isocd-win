using System;
using System.IO;
using System.Text;

// No Newtonsoft.Json so as to avoid dependency on an external library
using System.Runtime.Serialization.Json;

using isocd_builder.Properties;

namespace isocd_builder {
    /// <summary>
    /// This class provides functionality to work with trademark files and the sources JSON file.
    /// </summary>
    public class TmFileHelper {
        public static string ISOCDWIN_TMSOURCES_FILE_PATH = Path.Combine(isocd_builder_constants.ISOCDWIN_PUBLIC_DOCUMENTS_PATH, isocd_builder_constants.TRADEMARK_FILE_SOURCES_NAME);

        public static void InstallTmSourcesFile() {
            // Write the TmFileSources.json file embedded in the aplication resources, overwriting if necessary
            File.WriteAllBytes(ISOCDWIN_TMSOURCES_FILE_PATH, Resources.TmFileSources);

            // Now download the latest trademark sources file from the server and replace our local copy if it's newer
            DownloadTmFileSources();
        }

        public static bool CheckTmFiles() {
            var cd32TmFilePath = Path.Combine(isocd_builder_constants.ISOCDWIN_PUBLIC_DOCUMENTS_PATH, isocd_builder_constants.CD32_TRADEMARK_FILE);
            var cdtvTmFilePath = Path.Combine(isocd_builder_constants.ISOCDWIN_PUBLIC_DOCUMENTS_PATH, isocd_builder_constants.CDTV_TRADEMARK_FILE);

            if(!File.Exists(cd32TmFilePath) ||
                !File.Exists(cdtvTmFilePath)) {
                return false;
            }

            return
                HashHelper.GetSHA1HashFromFile(cd32TmFilePath) == isocd_builder_constants.CD32_TRADEMARK_FILE_SHA1_HASH |
                HashHelper.GetSHA1HashFromFile(cdtvTmFilePath) == isocd_builder_constants.CDTV_TRADEMARK_FILE_SHA1_HASH;
        }

        static TmFileSources DeserializeTmFileSources(string tmFileSourcesFilename) {
            try {
                var json = File.ReadAllText(tmFileSourcesFilename);

                using(var ms = new MemoryStream(Encoding.Unicode.GetBytes(json))) {
                    var deserializer = new DataContractJsonSerializer(typeof(TmFileSources));
                    return (TmFileSources)deserializer.ReadObject(ms);
                }
            }
            catch(Exception) {
                return null;
            }
        }

        public static bool DownloadTmFiles() {
            var tmFileSources = DownloadTmFileSources();

            foreach(var cd32Source in tmFileSources.Cd32Sources) {
                if(DownloadHelper.Download(
                    cd32Source.Url,
                    cd32Source.Offset,
                    isocd_builder_constants.CD32_TRADEMARK_FILE_SIZE,
                    isocd_builder_constants.CD32_TRADEMARK_FILE
                )) {
                    break;
                }
            }

            foreach(var cdtvSource in tmFileSources.CdtvSources) {
                if(DownloadHelper.Download(
                    cdtvSource.Url,
                    cdtvSource.Offset,
                    isocd_builder_constants.CDTV_TRADEMARK_FILE_SIZE,
                    isocd_builder_constants.CDTV_TRADEMARK_FILE
                )) {
                    break;
                }
            }

            return CheckTmFiles();
        }

        /// <summary>
        /// This method downloads the latest TmFileSources.json file from the isocd-win repo on Github
        /// </summary>
        public static TmFileSources DownloadTmFileSources() {
            var tmFileSources = DeserializeTmFileSources(ISOCDWIN_TMSOURCES_FILE_PATH);
            var tmFileSourcesFromServerFilename = Path.Combine(isocd_builder_constants.ISOCDWIN_PUBLIC_DOCUMENTS_PATH, $"{isocd_builder_constants.TRADEMARK_FILE_SOURCES_NAME}.server");

            var success = DownloadHelper.Download(
                isocd_builder_constants.TRADEMARK_FILE_SOURCES_URL,
                0,
                0,
                tmFileSourcesFromServerFilename
            );

            if(success) {
                var tmFileSourcesFromServer = DeserializeTmFileSources(tmFileSourcesFromServerFilename);

                // Check if the server version of the sources file is newer
                if(tmFileSources == null || tmFileSourcesFromServer.Version > tmFileSources.Version) {
                    File.Delete(ISOCDWIN_TMSOURCES_FILE_PATH);
                    File.Move(tmFileSourcesFromServerFilename, ISOCDWIN_TMSOURCES_FILE_PATH);
                    tmFileSources = tmFileSourcesFromServer;
                }
            }

            if(tmFileSources == null) {
                throw new FileLoadException("Could not load the trademark sources file!");
            }

            return tmFileSources;
        }
    }
}
