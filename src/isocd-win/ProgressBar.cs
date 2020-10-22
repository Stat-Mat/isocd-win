using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace isocd_win {
    partial class ProgressBar : PictureBox {
        int _minimum = 0;
        int _maximum = 100;
        int _value = 0;

        bool _drawing = false;

        public ProgressBar() {
            // If we're in design mode, then draw a mock half way percentage value
            if(LicenseManager.UsageMode == LicenseUsageMode.Designtime) {
                _drawing = true;
                _value = 50;
            }
        }

        protected override void OnPaint(PaintEventArgs pe) {
            // Clear the background
            using(var hb = new HatchBrush(HatchStyle.Percent25, Color.LightGray, BackColor)) {
                pe.Graphics.FillRectangle(
                    hb,
                    0, 0,
                    ClientSize.Width, ClientSize.Height);
            }

            if(_drawing) {
                // Draw the progress bar
                var fraction =
                    (float)(_value - _minimum) /
                    (_maximum - _minimum);

                var width = Math.Round(fraction * ClientSize.Width);

                using(var sb = new SolidBrush(ForeColor)) {
                    pe.Graphics.FillRectangle(
                        sb,
                        0, 0,
                        (int)width, ClientSize.Height);
                }

                // Draw the percentage text
                pe.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                using(var sf = new StringFormat()) {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    var percent = Math.Round(fraction * 100);

                    pe.Graphics.DrawString(
                        percent.ToString() + "%",
                        Font, Brushes.Black,
                        ClientRectangle, sf);
                }
            }
        }

        public void SetDrawing(bool drawing) {
            _drawing = drawing;
        }

        /// <summary>
        /// Gets or sets the progress minimum value.
        /// </summary>
        /// <value>The minimum value this progress bar can start at.</value>
        [Category("Appearance")]
        [DefaultValue(0)]
        public virtual int Minimum {
            get { return _minimum; }
            set {
                _minimum = value;
            }
        }

        /// <summary>
        /// Gets or sets the progress maximum value.
        /// </summary>
        /// <value>The maximum value this progress bar can reach.</value>
        [Category("Appearance")]
        [DefaultValue(100)]
        public virtual int Maximum {
            get { return _maximum; }
            set {
                _maximum = value;
            }
        }

        /// <summary>
        /// Gets or sets the progress current value.
        /// </summary>
        /// <value>The current value of the progress bar.</value>
        [Category("Appearance")]
        [DefaultValue(0)]
        public virtual int Value {
            get { return _value; }
            set {
                if(value == 0 || _value != value) {
                    _value = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Unhide the Font property in the designer.
        /// </summary>
        /// <value>The font of the control, used for displaying the progress percentage.</value>
        [Category("Appearance")]
        [Browsable(true)]
        public override Font Font {
            get {
                return base.Font;
            }
            set {
                base.Font = value;
            }
        }

        /// <summary>
        /// Unhide the ForeColor property in the designer.
        /// </summary>
        /// <value>The forecolor of the control, used for displaying the progress bar.</value>
        [Category("Appearance")]
        [Browsable(true)]
        public override Color ForeColor {
            get {
                return base.ForeColor;
            }
            set {
                base.ForeColor = value;
            }
        }
    }
}
