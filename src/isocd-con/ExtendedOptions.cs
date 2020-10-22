using isocd_builder;

namespace isocd_con {
    public class ExtendedOptions : Options {
        [CmdLineOption(201, "wp", "The full path of the WinUAE executable to test the built image", "<path>")]
        public string WinUAEPath { get; set; }

        public void RestoreDefaults() {
            // Call the base class SetDefaults() method
            SetDefaults();
        }
    }
}
