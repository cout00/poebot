using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2.Logger {
    public class DebugLogger : ILogger {
        private readonly TextBoxBase textBox;
        StringBuilder Builder = new StringBuilder();

        public DebugLogger(TextBoxBase textBox) {
            this.textBox = textBox;
        }

        public void WriteLog(string msg) {
            Builder.AppendLine(msg);
            textBox.Invoke((Action)(() => {
                textBox.Text = Builder.ToString();
                textBox.SelectionStart = textBox.Text.Length;
                textBox.ScrollToCaret();
            }));
        }
    }
}
