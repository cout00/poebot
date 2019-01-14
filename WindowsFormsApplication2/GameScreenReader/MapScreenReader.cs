using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.GameScreenReader {
    public class MapScreenReader : GameScreenReaderBase {

        public override Rectangle GetGameWindowBounds() {
            return new Rectangle(100, 60, 500, 370);
        }
    }
}
