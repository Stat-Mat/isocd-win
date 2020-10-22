using System;
using isocd_builder;

namespace isocd_win {
    [Serializable]
    public class ExtendedOptions : Options {
        public bool PlaySounds { get; set; }

        public bool WinUAETest { get; set; }

        public string WinUAEPath { get; set; }

        public ExtendedOptions() {
            RestoreDefaults();
        }

        public void RestoreDefaults() {
            // Call the base class SetDefaults() method
            SetDefaults();

            PlaySounds = true;
            WinUAETest = false;
        }
    }
}
