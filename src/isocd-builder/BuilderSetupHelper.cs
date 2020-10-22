using System;
using System.IO;

namespace isocd_builder {
    /// <summary>
    /// This class provides setup functionality to ensure the library can be used.
    /// </summary>
    public static class BuilderSetupHelper {
        public static BuilderSetupResult Setup() {
            var result = new BuilderSetupResult();

            try {
                // Check that the ISOCD-Win public documents folder exists
                if(!Directory.Exists(isocd_builder_constants.ISOCDWIN_PUBLIC_DOCUMENTS_PATH)) {
                    Directory.CreateDirectory(isocd_builder_constants.ISOCDWIN_PUBLIC_DOCUMENTS_PATH);
                }

                result.HaveTmFiles = TmFileHelper.CheckTmFiles();

                // Only install/download TmFileSources.json file if we don't yet have the trademark files
                if(!result.HaveTmFiles) {
                    // Check we have the latest TmFileSources.json file installed
                    TmFileHelper.InstallTmSourcesFile();
                }
            }
            catch(Exception ex) {
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
