using System;
using System.IO;
using System.Windows.Forms;

using isocd_builder;

namespace isocd_win {
    public partial class optionsForm : Form {
        ExtendedOptions _options;

        public optionsForm(ExtendedOptions settings) {
            InitializeComponent();

            _options = settings;

            // Set data preparer max length based on our appended software string
            dataPreparerIdTextBox.MaxLength = 128 - isocd_builder_constants.ISOCDWIN_DATA_PREPARER_IDENTIFIER.Length;

            imagePaddingComboBox.DataSource = Enum.GetValues(typeof(PadSize));
        }

        void SettingsForm_Shown(object sender, EventArgs e) {
            UpdateForm();
        }

        void UpdateForm() {
            volumeIdTextBox.Text = _options.VolumeId;
            volumeSetIdTextBox.Text = _options.VolumeSetId;
            publisherIdTextBox.Text = _options.PublisherId;
            dataPreparerIdTextBox.Text = _options.DataPreparerId;
            applicationIdTextBox.Text = _options.ApplicationId;
            imagePaddingComboBox.SelectedItem = _options.PadSize;

            dataCacheTextBox.Text = _options.DataCache.ToString();
            dirCacheTextBox.Text = _options.DirCache.ToString();
            fileLockTextBox.Text = _options.FileLock.ToString();
            fileHandleTextBox.Text = _options.FileHandle.ToString();
            retriesTextBox.Text = _options.Retries.ToString();
            directReadCheckBox.Checked = _options.DirectRead;
            fastSearchCheckBox.Checked = _options.FastSearch;
            speedIndCheckBox.Checked = _options.SpeedIndependent;

            playSoundsCheckBox.Checked = _options.PlaySounds;
            WinUAETestCheckBox.Checked = _options.WinUAETest;
            winUAEPathTextBox.Text = _options.WinUAEPath;
        }

        void OkButton_Click(object sender, EventArgs e) {
            // Create a new instance of the options object so that we can validate it before updating the original
            var newOptions = new ExtendedOptions {
                // These are not set here, so just set them the values in the provided ptions to allow the validation to work
                InputFolder = _options.InputFolder,
                OutputFile = _options.OutputFile,
                TrademarkFile = _options.TrademarkFile,
                Trademark = _options.Trademark,

                VolumeId = volumeIdTextBox.Text,
                VolumeSetId = volumeSetIdTextBox.Text,
                PublisherId = publisherIdTextBox.Text,
                DataPreparerId = dataPreparerIdTextBox.Text,
                ApplicationId = applicationIdTextBox.Text,
                PadSize = (PadSize)imagePaddingComboBox.SelectedItem,

                DataCache = int.Parse(dataCacheTextBox.Text),
                DirCache = int.Parse(dirCacheTextBox.Text),
                FileLock = int.Parse(fileLockTextBox.Text),
                FileHandle = int.Parse(fileHandleTextBox.Text),
                Retries = int.Parse(retriesTextBox.Text),
                DirectRead = directReadCheckBox.Checked,
                FastSearch = fastSearchCheckBox.Checked,
                SpeedIndependent = speedIndCheckBox.Checked,

                PlaySounds = playSoundsCheckBox.Checked,
                WinUAETest = WinUAETestCheckBox.Checked,
                WinUAEPath = winUAEPathTextBox.Text
            };

            if(!newOptions.IsValid()) {
                MessageBox.Show(newOptions.ValidationResult().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Validation was successful, so update the original settings object
            _options = newOptions;

            Close();
        }

        void CancelButton_Click(object sender, EventArgs e) {
            Close();
        }

        void RestoreDefaultsButton_Click(object sender, EventArgs e) {
            _options.RestoreDefaults();
            UpdateForm();
        }

        void WinUAEBrowseButton_Click(object sender, EventArgs e) {
            using(var fdlg = new OpenFileDialog {
                Title = "Browse for WinUAE executable",
                Filter = "WinUAE Executable (winuae*.exe)|winuae*.exe",
                FilterIndex = 1,
                RestoreDirectory = true,
                CheckFileExists = true,
                InitialDirectory = Directory.GetCurrentDirectory()
            }) {
                if(fdlg.ShowDialog() == DialogResult.OK) {
                    winUAEPathTextBox.Text = fdlg.FileName;
                }
            }
        }
    }
}
