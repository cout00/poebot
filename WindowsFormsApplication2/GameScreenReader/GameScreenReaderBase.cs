using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.GameScreenReader {
    public abstract class GameScreenReaderBase {
        public Rectangle Bounds {
            get {
                var clientBounds = NativeApiWrapper.GetGameWindowRectange();
                return new Rectangle(clientBounds.X + GetGameWindowBounds().Left, clientBounds.Y + GetGameWindowBounds().Top, GetGameWindowBounds().Width, GetGameWindowBounds().Height);
            }
        }
        public abstract Rectangle GetGameWindowBounds();

        public Bitmap ReadScreen() {
            Bitmap bitmap = new Bitmap(Bounds.Width, Bounds.Height);
            using (Graphics gr = Graphics.FromImage(bitmap)) {
                gr.CopyFromScreen(Bounds.Left, Bounds.Top, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
            }
            return bitmap;
        }

        public Point ClientPointToWindowPoint(Point point) {
            var localgamerect = GetGameWindowBounds();
            return new Point(point.X + localgamerect.Left, point.Y + localgamerect.Top + 25);
        }

        public Point PlayerCord {
            get {
                var gamerect = GetGameWindowBounds();
                var playerLocalCord = NativeApiWrapper.PlayerLocalCord;
                return new Point(playerLocalCord.X - gamerect.Left, playerLocalCord.Y - gamerect.Top);
            }
        }

    }
}
