using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.GameScreenReader {
    public class LootScreenReader : GameScreenReaderBase {
        public override Rectangle GetGameWindowBounds() {
            return new Rectangle(22, 44, 765, 490);
        }
    }
}
