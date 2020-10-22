using isocd_builder;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace isocd_win {
    public class ConfigManager {
        readonly string m_sConfigFileName = Path.Combine(isocd_builder_constants.ISOCDWIN_PUBLIC_DOCUMENTS_PATH, $"{Path.GetFileNameWithoutExtension(Application.ExecutablePath)}.xml");

        public ExtendedOptions Options { get; set; }

        public ConfigManager() {
            Options = new ExtendedOptions();
        }

        public bool LoadConfig() {
            if(File.Exists(m_sConfigFileName)) {
                using(var srReader = File.OpenText(m_sConfigFileName)) {
                    var tType = Options.GetType();
                    var xsSerializer = new XmlSerializer(tType);
                    Options = (ExtendedOptions) xsSerializer.Deserialize(srReader);
                    srReader.Close();
                    return true;
                }
            }

            return false;
        }

        public void SaveConfig() {
            using(var swWriter = File.CreateText(m_sConfigFileName)) {
                var tType = Options.GetType();
                if(tType.IsSerializable) {
                    var xsSerializer = new XmlSerializer(tType);
                    xsSerializer.Serialize(swWriter, Options);
                    swWriter.Close();
                }
            }
        }
    }
}
