namespace isocd_win {
    partial class optionsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(optionsForm));
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.volumeIdLabel = new System.Windows.Forms.Label();
            this.volumeIdTextBox = new System.Windows.Forms.TextBox();
            this.dataCacheLabel = new System.Windows.Forms.Label();
            this.dataCacheSectorsLabel = new System.Windows.Forms.Label();
            this.dirCacheSectorsLabel = new System.Windows.Forms.Label();
            this.dirCacheLabel = new System.Windows.Forms.Label();
            this.fileLockNodesLabel = new System.Windows.Forms.Label();
            this.fileLockLlabel = new System.Windows.Forms.Label();
            this.fileHandleNodesLabel = new System.Windows.Forms.Label();
            this.fileHandleLabel = new System.Windows.Forms.Label();
            this.retriesLabel = new System.Windows.Forms.Label();
            this.directReadCheckBox = new System.Windows.Forms.CheckBox();
            this.fastSearchCheckBox = new System.Windows.Forms.CheckBox();
            this.speedIndCheckBox = new System.Windows.Forms.CheckBox();
            this.restoreDefaultsButton = new System.Windows.Forms.Button();
            this.playSoundsCheckBox = new System.Windows.Forms.CheckBox();
            this.volSetIdLabel = new System.Windows.Forms.Label();
            this.volumeSetIdTextBox = new System.Windows.Forms.TextBox();
            this.publisherIdLabel = new System.Windows.Forms.Label();
            this.publisherIdTextBox = new System.Windows.Forms.TextBox();
            this.dataPreparerIdLabel = new System.Windows.Forms.Label();
            this.dataPreparerIdTextBox = new System.Windows.Forms.TextBox();
            this.applicationIdLabel = new System.Windows.Forms.Label();
            this.applicationIdTextBox = new System.Windows.Forms.TextBox();
            this.imagePaddingComboBox = new System.Windows.Forms.ComboBox();
            this.imagePaddingLabel = new System.Windows.Forms.Label();
            this.WinUAETestCheckBox = new System.Windows.Forms.CheckBox();
            this.cdfsGroupBox = new System.Windows.Forms.GroupBox();
            this.isoGroupBox = new System.Windows.Forms.GroupBox();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.winUAEBrowseButton = new System.Windows.Forms.Button();
            this.winUAEPathLabel = new System.Windows.Forms.Label();
            this.winUAEPathTextBox = new System.Windows.Forms.TextBox();
            this.dirCacheTextBox = new isocd_win.ValueBox();
            this.dataCacheTextBox = new isocd_win.ValueBox();
            this.fileLockTextBox = new isocd_win.ValueBox();
            this.fileHandleTextBox = new isocd_win.ValueBox();
            this.retriesTextBox = new isocd_win.ValueBox();
            this.cdfsGroupBox.SuspendLayout();
            this.isoGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(599, 295);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 39;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(685, 295);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 40;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // volumeIdLabel
            // 
            this.volumeIdLabel.AutoSize = true;
            this.volumeIdLabel.Location = new System.Drawing.Point(42, 27);
            this.volumeIdLabel.Name = "volumeIdLabel";
            this.volumeIdLabel.Size = new System.Drawing.Size(56, 13);
            this.volumeIdLabel.TabIndex = 20;
            this.volumeIdLabel.Text = "Volume ID";
            // 
            // volumeIdTextBox
            // 
            this.volumeIdTextBox.Location = new System.Drawing.Point(99, 24);
            this.volumeIdTextBox.MaxLength = 32;
            this.volumeIdTextBox.Name = "volumeIdTextBox";
            this.volumeIdTextBox.Size = new System.Drawing.Size(260, 20);
            this.volumeIdTextBox.TabIndex = 21;
            // 
            // dataCacheLabel
            // 
            this.dataCacheLabel.AutoSize = true;
            this.dataCacheLabel.Location = new System.Drawing.Point(25, 27);
            this.dataCacheLabel.Name = "dataCacheLabel";
            this.dataCacheLabel.Size = new System.Drawing.Size(64, 13);
            this.dataCacheLabel.TabIndex = 2;
            this.dataCacheLabel.Text = "Data Cache";
            // 
            // dataCacheSectorsLabel
            // 
            this.dataCacheSectorsLabel.AutoSize = true;
            this.dataCacheSectorsLabel.Location = new System.Drawing.Point(198, 27);
            this.dataCacheSectorsLabel.Name = "dataCacheSectorsLabel";
            this.dataCacheSectorsLabel.Size = new System.Drawing.Size(43, 13);
            this.dataCacheSectorsLabel.TabIndex = 4;
            this.dataCacheSectorsLabel.Text = "Sectors";
            // 
            // dirCacheSectorsLabel
            // 
            this.dirCacheSectorsLabel.AutoSize = true;
            this.dirCacheSectorsLabel.Location = new System.Drawing.Point(198, 64);
            this.dirCacheSectorsLabel.Name = "dirCacheSectorsLabel";
            this.dirCacheSectorsLabel.Size = new System.Drawing.Size(43, 13);
            this.dirCacheSectorsLabel.TabIndex = 7;
            this.dirCacheSectorsLabel.Text = "Sectors";
            // 
            // dirCacheLabel
            // 
            this.dirCacheLabel.AutoSize = true;
            this.dirCacheLabel.Location = new System.Drawing.Point(35, 64);
            this.dirCacheLabel.Name = "dirCacheLabel";
            this.dirCacheLabel.Size = new System.Drawing.Size(54, 13);
            this.dirCacheLabel.TabIndex = 5;
            this.dirCacheLabel.Text = "Dir Cache";
            // 
            // fileLockNodesLabel
            // 
            this.fileLockNodesLabel.AutoSize = true;
            this.fileLockNodesLabel.Location = new System.Drawing.Point(198, 101);
            this.fileLockNodesLabel.Name = "fileLockNodesLabel";
            this.fileLockNodesLabel.Size = new System.Drawing.Size(38, 13);
            this.fileLockNodesLabel.TabIndex = 10;
            this.fileLockNodesLabel.Text = "Nodes";
            // 
            // fileLockLlabel
            // 
            this.fileLockLlabel.AutoSize = true;
            this.fileLockLlabel.Location = new System.Drawing.Point(39, 101);
            this.fileLockLlabel.Name = "fileLockLlabel";
            this.fileLockLlabel.Size = new System.Drawing.Size(50, 13);
            this.fileLockLlabel.TabIndex = 8;
            this.fileLockLlabel.Text = "File Lock";
            // 
            // fileHandleNodesLabel
            // 
            this.fileHandleNodesLabel.AutoSize = true;
            this.fileHandleNodesLabel.Location = new System.Drawing.Point(198, 138);
            this.fileHandleNodesLabel.Name = "fileHandleNodesLabel";
            this.fileHandleNodesLabel.Size = new System.Drawing.Size(38, 13);
            this.fileHandleNodesLabel.TabIndex = 13;
            this.fileHandleNodesLabel.Text = "Nodes";
            // 
            // fileHandleLabel
            // 
            this.fileHandleLabel.AutoSize = true;
            this.fileHandleLabel.Location = new System.Drawing.Point(29, 138);
            this.fileHandleLabel.Name = "fileHandleLabel";
            this.fileHandleLabel.Size = new System.Drawing.Size(60, 13);
            this.fileHandleLabel.TabIndex = 11;
            this.fileHandleLabel.Text = "File Handle";
            // 
            // retriesLabel
            // 
            this.retriesLabel.AutoSize = true;
            this.retriesLabel.Location = new System.Drawing.Point(48, 175);
            this.retriesLabel.Name = "retriesLabel";
            this.retriesLabel.Size = new System.Drawing.Size(40, 13);
            this.retriesLabel.TabIndex = 14;
            this.retriesLabel.Text = "Retries";
            // 
            // directReadCheckBox
            // 
            this.directReadCheckBox.AutoSize = true;
            this.directReadCheckBox.Location = new System.Drawing.Point(278, 63);
            this.directReadCheckBox.Name = "directReadCheckBox";
            this.directReadCheckBox.Size = new System.Drawing.Size(83, 17);
            this.directReadCheckBox.TabIndex = 16;
            this.directReadCheckBox.Text = "Direct Read";
            this.directReadCheckBox.UseVisualStyleBackColor = true;
            // 
            // fastSearchCheckBox
            // 
            this.fastSearchCheckBox.AutoSize = true;
            this.fastSearchCheckBox.Location = new System.Drawing.Point(278, 100);
            this.fastSearchCheckBox.Name = "fastSearchCheckBox";
            this.fastSearchCheckBox.Size = new System.Drawing.Size(83, 17);
            this.fastSearchCheckBox.TabIndex = 17;
            this.fastSearchCheckBox.Text = "Fast Search";
            this.fastSearchCheckBox.UseVisualStyleBackColor = true;
            // 
            // speedIndCheckBox
            // 
            this.speedIndCheckBox.AutoSize = true;
            this.speedIndCheckBox.Location = new System.Drawing.Point(278, 137);
            this.speedIndCheckBox.Name = "speedIndCheckBox";
            this.speedIndCheckBox.Size = new System.Drawing.Size(78, 17);
            this.speedIndCheckBox.TabIndex = 18;
            this.speedIndCheckBox.Text = "Speed Ind.";
            this.speedIndCheckBox.UseVisualStyleBackColor = true;
            // 
            // restoreDefaultsButton
            // 
            this.restoreDefaultsButton.Location = new System.Drawing.Point(468, 295);
            this.restoreDefaultsButton.Name = "restoreDefaultsButton";
            this.restoreDefaultsButton.Size = new System.Drawing.Size(106, 23);
            this.restoreDefaultsButton.TabIndex = 38;
            this.restoreDefaultsButton.Text = "Restore Defaults";
            this.restoreDefaultsButton.UseVisualStyleBackColor = true;
            this.restoreDefaultsButton.Click += new System.EventHandler(this.RestoreDefaultsButton_Click);
            // 
            // playSoundsCheckBox
            // 
            this.playSoundsCheckBox.AutoSize = true;
            this.playSoundsCheckBox.Location = new System.Drawing.Point(260, 24);
            this.playSoundsCheckBox.Name = "playSoundsCheckBox";
            this.playSoundsCheckBox.Size = new System.Drawing.Size(85, 17);
            this.playSoundsCheckBox.TabIndex = 34;
            this.playSoundsCheckBox.Text = "Play Sounds";
            this.playSoundsCheckBox.UseVisualStyleBackColor = true;
            // 
            // volSetIdLabel
            // 
            this.volSetIdLabel.AutoSize = true;
            this.volSetIdLabel.Location = new System.Drawing.Point(23, 64);
            this.volSetIdLabel.Name = "volSetIdLabel";
            this.volSetIdLabel.Size = new System.Drawing.Size(75, 13);
            this.volSetIdLabel.TabIndex = 22;
            this.volSetIdLabel.Text = "Volume Set ID";
            // 
            // volumeSetIdTextBox
            // 
            this.volumeSetIdTextBox.Location = new System.Drawing.Point(98, 61);
            this.volumeSetIdTextBox.MaxLength = 128;
            this.volumeSetIdTextBox.Name = "volumeSetIdTextBox";
            this.volumeSetIdTextBox.Size = new System.Drawing.Size(260, 20);
            this.volumeSetIdTextBox.TabIndex = 23;
            // 
            // publisherIdLabel
            // 
            this.publisherIdLabel.AutoSize = true;
            this.publisherIdLabel.Location = new System.Drawing.Point(34, 101);
            this.publisherIdLabel.Name = "publisherIdLabel";
            this.publisherIdLabel.Size = new System.Drawing.Size(64, 13);
            this.publisherIdLabel.TabIndex = 24;
            this.publisherIdLabel.Text = "Publisher ID";
            // 
            // publisherIdTextBox
            // 
            this.publisherIdTextBox.Location = new System.Drawing.Point(98, 98);
            this.publisherIdTextBox.MaxLength = 128;
            this.publisherIdTextBox.Name = "publisherIdTextBox";
            this.publisherIdTextBox.Size = new System.Drawing.Size(260, 20);
            this.publisherIdTextBox.TabIndex = 25;
            // 
            // dataPreparerIdLabel
            // 
            this.dataPreparerIdLabel.AutoSize = true;
            this.dataPreparerIdLabel.Location = new System.Drawing.Point(11, 138);
            this.dataPreparerIdLabel.Name = "dataPreparerIdLabel";
            this.dataPreparerIdLabel.Size = new System.Drawing.Size(87, 13);
            this.dataPreparerIdLabel.TabIndex = 26;
            this.dataPreparerIdLabel.Text = "Data Preparer ID";
            // 
            // dataPreparerIdTextBox
            // 
            this.dataPreparerIdTextBox.Location = new System.Drawing.Point(98, 135);
            this.dataPreparerIdTextBox.MaxLength = 128;
            this.dataPreparerIdTextBox.Name = "dataPreparerIdTextBox";
            this.dataPreparerIdTextBox.Size = new System.Drawing.Size(260, 20);
            this.dataPreparerIdTextBox.TabIndex = 27;
            // 
            // applicationIdLabel
            // 
            this.applicationIdLabel.AutoSize = true;
            this.applicationIdLabel.Location = new System.Drawing.Point(25, 175);
            this.applicationIdLabel.Name = "applicationIdLabel";
            this.applicationIdLabel.Size = new System.Drawing.Size(73, 13);
            this.applicationIdLabel.TabIndex = 28;
            this.applicationIdLabel.Text = "Application ID";
            // 
            // applicationIdTextBox
            // 
            this.applicationIdTextBox.Location = new System.Drawing.Point(98, 172);
            this.applicationIdTextBox.MaxLength = 128;
            this.applicationIdTextBox.Name = "applicationIdTextBox";
            this.applicationIdTextBox.Size = new System.Drawing.Size(260, 20);
            this.applicationIdTextBox.TabIndex = 29;
            // 
            // imagePaddingComboBox
            // 
            this.imagePaddingComboBox.FormattingEnabled = true;
            this.imagePaddingComboBox.ItemHeight = 13;
            this.imagePaddingComboBox.Location = new System.Drawing.Point(98, 209);
            this.imagePaddingComboBox.Name = "imagePaddingComboBox";
            this.imagePaddingComboBox.Size = new System.Drawing.Size(82, 21);
            this.imagePaddingComboBox.TabIndex = 31;
            // 
            // imagePaddingLabel
            // 
            this.imagePaddingLabel.AutoSize = true;
            this.imagePaddingLabel.Location = new System.Drawing.Point(20, 212);
            this.imagePaddingLabel.Name = "imagePaddingLabel";
            this.imagePaddingLabel.Size = new System.Drawing.Size(78, 13);
            this.imagePaddingLabel.TabIndex = 30;
            this.imagePaddingLabel.Text = "Image Padding";
            // 
            // WinUAETestCheckBox
            // 
            this.WinUAETestCheckBox.AutoSize = true;
            this.WinUAETestCheckBox.Location = new System.Drawing.Point(16, 24);
            this.WinUAETestCheckBox.Name = "WinUAETestCheckBox";
            this.WinUAETestCheckBox.Size = new System.Drawing.Size(113, 17);
            this.WinUAETestCheckBox.TabIndex = 33;
            this.WinUAETestCheckBox.Text = "Test with WinUAE";
            this.WinUAETestCheckBox.UseVisualStyleBackColor = true;
            // 
            // cdfsGroupBox
            // 
            this.cdfsGroupBox.Controls.Add(this.dirCacheTextBox);
            this.cdfsGroupBox.Controls.Add(this.dataCacheTextBox);
            this.cdfsGroupBox.Controls.Add(this.dataCacheLabel);
            this.cdfsGroupBox.Controls.Add(this.dataCacheSectorsLabel);
            this.cdfsGroupBox.Controls.Add(this.dirCacheLabel);
            this.cdfsGroupBox.Controls.Add(this.dirCacheSectorsLabel);
            this.cdfsGroupBox.Controls.Add(this.fileLockTextBox);
            this.cdfsGroupBox.Controls.Add(this.fileLockLlabel);
            this.cdfsGroupBox.Controls.Add(this.fileLockNodesLabel);
            this.cdfsGroupBox.Controls.Add(this.fileHandleTextBox);
            this.cdfsGroupBox.Controls.Add(this.fileHandleLabel);
            this.cdfsGroupBox.Controls.Add(this.fileHandleNodesLabel);
            this.cdfsGroupBox.Controls.Add(this.retriesTextBox);
            this.cdfsGroupBox.Controls.Add(this.retriesLabel);
            this.cdfsGroupBox.Controls.Add(this.speedIndCheckBox);
            this.cdfsGroupBox.Controls.Add(this.directReadCheckBox);
            this.cdfsGroupBox.Controls.Add(this.fastSearchCheckBox);
            this.cdfsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.cdfsGroupBox.Name = "cdfsGroupBox";
            this.cdfsGroupBox.Size = new System.Drawing.Size(369, 212);
            this.cdfsGroupBox.TabIndex = 1;
            this.cdfsGroupBox.TabStop = false;
            this.cdfsGroupBox.Text = "CDFS";
            // 
            // isoGroupBox
            // 
            this.isoGroupBox.Controls.Add(this.volumeSetIdTextBox);
            this.isoGroupBox.Controls.Add(this.volumeIdTextBox);
            this.isoGroupBox.Controls.Add(this.volumeIdLabel);
            this.isoGroupBox.Controls.Add(this.imagePaddingLabel);
            this.isoGroupBox.Controls.Add(this.imagePaddingComboBox);
            this.isoGroupBox.Controls.Add(this.volSetIdLabel);
            this.isoGroupBox.Controls.Add(this.publisherIdTextBox);
            this.isoGroupBox.Controls.Add(this.applicationIdLabel);
            this.isoGroupBox.Controls.Add(this.publisherIdLabel);
            this.isoGroupBox.Controls.Add(this.applicationIdTextBox);
            this.isoGroupBox.Controls.Add(this.dataPreparerIdTextBox);
            this.isoGroupBox.Controls.Add(this.dataPreparerIdLabel);
            this.isoGroupBox.Location = new System.Drawing.Point(401, 11);
            this.isoGroupBox.Name = "isoGroupBox";
            this.isoGroupBox.Size = new System.Drawing.Size(373, 244);
            this.isoGroupBox.TabIndex = 19;
            this.isoGroupBox.TabStop = false;
            this.isoGroupBox.Text = "ISO";
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.winUAEBrowseButton);
            this.generalGroupBox.Controls.Add(this.winUAEPathLabel);
            this.generalGroupBox.Controls.Add(this.winUAEPathTextBox);
            this.generalGroupBox.Controls.Add(this.WinUAETestCheckBox);
            this.generalGroupBox.Controls.Add(this.playSoundsCheckBox);
            this.generalGroupBox.Location = new System.Drawing.Point(12, 239);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(369, 92);
            this.generalGroupBox.TabIndex = 32;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // winUAEBrowseButton
            // 
            this.winUAEBrowseButton.Location = new System.Drawing.Point(284, 56);
            this.winUAEBrowseButton.Name = "winUAEBrowseButton";
            this.winUAEBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.winUAEBrowseButton.TabIndex = 37;
            this.winUAEBrowseButton.Text = "Browse";
            this.winUAEBrowseButton.UseVisualStyleBackColor = true;
            this.winUAEBrowseButton.Click += new System.EventHandler(this.WinUAEBrowseButton_Click);
            // 
            // winUAEPathLabel
            // 
            this.winUAEPathLabel.AutoSize = true;
            this.winUAEPathLabel.Location = new System.Drawing.Point(13, 61);
            this.winUAEPathLabel.Name = "winUAEPathLabel";
            this.winUAEPathLabel.Size = new System.Drawing.Size(73, 13);
            this.winUAEPathLabel.TabIndex = 35;
            this.winUAEPathLabel.Text = "WinUAE Path";
            // 
            // winUAEPathTextBox
            // 
            this.winUAEPathTextBox.Location = new System.Drawing.Point(86, 58);
            this.winUAEPathTextBox.Name = "winUAEPathTextBox";
            this.winUAEPathTextBox.Size = new System.Drawing.Size(187, 20);
            this.winUAEPathTextBox.TabIndex = 36;
            // 
            // dirCacheTextBox
            // 
            this.dirCacheTextBox.Location = new System.Drawing.Point(90, 61);
            this.dirCacheTextBox.MaxLength = 32;
            this.dirCacheTextBox.MaxValue = 127;
            this.dirCacheTextBox.MinValue = 2;
            this.dirCacheTextBox.Name = "dirCacheTextBox";
            this.dirCacheTextBox.Size = new System.Drawing.Size(106, 20);
            this.dirCacheTextBox.TabIndex = 6;
            // 
            // dataCacheTextBox
            // 
            this.dataCacheTextBox.Location = new System.Drawing.Point(90, 24);
            this.dataCacheTextBox.MaxLength = 32;
            this.dataCacheTextBox.MaxValue = 127;
            this.dataCacheTextBox.MinValue = 1;
            this.dataCacheTextBox.Name = "dataCacheTextBox";
            this.dataCacheTextBox.Size = new System.Drawing.Size(106, 20);
            this.dataCacheTextBox.TabIndex = 3;
            // 
            // fileLockTextBox
            // 
            this.fileLockTextBox.Location = new System.Drawing.Point(90, 98);
            this.fileLockTextBox.MaxLength = 32;
            this.fileLockTextBox.MaxValue = 9999;
            this.fileLockTextBox.MinValue = 1;
            this.fileLockTextBox.Name = "fileLockTextBox";
            this.fileLockTextBox.Size = new System.Drawing.Size(106, 20);
            this.fileLockTextBox.TabIndex = 9;
            // 
            // fileHandleTextBox
            // 
            this.fileHandleTextBox.Location = new System.Drawing.Point(90, 135);
            this.fileHandleTextBox.MaxLength = 32;
            this.fileHandleTextBox.MaxValue = 9999;
            this.fileHandleTextBox.MinValue = 1;
            this.fileHandleTextBox.Name = "fileHandleTextBox";
            this.fileHandleTextBox.Size = new System.Drawing.Size(106, 20);
            this.fileHandleTextBox.TabIndex = 12;
            // 
            // retriesTextBox
            // 
            this.retriesTextBox.Location = new System.Drawing.Point(90, 172);
            this.retriesTextBox.MaxLength = 32;
            this.retriesTextBox.MaxValue = 9999;
            this.retriesTextBox.MinValue = 0;
            this.retriesTextBox.Name = "retriesTextBox";
            this.retriesTextBox.Size = new System.Drawing.Size(106, 20);
            this.retriesTextBox.TabIndex = 15;
            // 
            // optionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 347);
            this.Controls.Add(this.generalGroupBox);
            this.Controls.Add(this.isoGroupBox);
            this.Controls.Add(this.cdfsGroupBox);
            this.Controls.Add(this.restoreDefaultsButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "optionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.cdfsGroupBox.ResumeLayout(false);
            this.cdfsGroupBox.PerformLayout();
            this.isoGroupBox.ResumeLayout(false);
            this.isoGroupBox.PerformLayout();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        System.Windows.Forms.Button okButton;
        System.Windows.Forms.Button cancelButton;
        System.Windows.Forms.Label volumeIdLabel;
        System.Windows.Forms.TextBox volumeIdTextBox;
        System.Windows.Forms.Label dataCacheLabel;
        System.Windows.Forms.Label dataCacheSectorsLabel;
        System.Windows.Forms.Label dirCacheSectorsLabel;
        System.Windows.Forms.Label dirCacheLabel;
        System.Windows.Forms.Label fileLockNodesLabel;
        System.Windows.Forms.Label fileLockLlabel;
        System.Windows.Forms.Label fileHandleNodesLabel;
        System.Windows.Forms.Label fileHandleLabel;
        System.Windows.Forms.Label retriesLabel;
        System.Windows.Forms.CheckBox directReadCheckBox;
        System.Windows.Forms.CheckBox fastSearchCheckBox;
        System.Windows.Forms.CheckBox speedIndCheckBox;
        System.Windows.Forms.Button restoreDefaultsButton;
        ValueBox retriesTextBox;
        ValueBox fileHandleTextBox;
        ValueBox fileLockTextBox;
        ValueBox dirCacheTextBox;
        ValueBox dataCacheTextBox;
        System.Windows.Forms.CheckBox playSoundsCheckBox;
        System.Windows.Forms.Label volSetIdLabel;
        System.Windows.Forms.TextBox volumeSetIdTextBox;
        System.Windows.Forms.Label publisherIdLabel;
        System.Windows.Forms.TextBox publisherIdTextBox;
        System.Windows.Forms.Label dataPreparerIdLabel;
        System.Windows.Forms.TextBox dataPreparerIdTextBox;
        System.Windows.Forms.Label applicationIdLabel;
        System.Windows.Forms.TextBox applicationIdTextBox;
        System.Windows.Forms.ComboBox imagePaddingComboBox;
        System.Windows.Forms.Label imagePaddingLabel;
        System.Windows.Forms.CheckBox WinUAETestCheckBox;
        System.Windows.Forms.GroupBox cdfsGroupBox;
        System.Windows.Forms.GroupBox isoGroupBox;
        System.Windows.Forms.GroupBox generalGroupBox;
        System.Windows.Forms.Label winUAEPathLabel;
        System.Windows.Forms.TextBox winUAEPathTextBox;
        System.Windows.Forms.Button winUAEBrowseButton;
    }
}