using System.ComponentModel;
using System.Windows.Forms;

namespace isocd_win {
    public class ValueBox : TextBox {
        int _minValue;
        int _maxValue;

        protected override void OnKeyPress(KeyPressEventArgs e) {
            // Restrict input to numeric values
            if(!char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && ModifierKeys != Keys.Control) {
                e.Handled = true;
            }

            base.OnKeyPress(e);
        }

        [Category("Appearance")]
        public virtual int MinValue {
            get { return _minValue; }
            set {
                _minValue = value;
            }
        }

        [Category("Appearance")]
        public virtual int MaxValue {
            get { return _maxValue; }
            set {
                _maxValue = value;
            }
        }
    }
}
