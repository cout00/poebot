using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApplication2.Parsers.ItemParser {
    public class PoeBaseItem {
        public string Class { get; set; }
        public string BaseName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public Point Position { get; set; }
        public List<string> Tags { get; set; }
        public PoeBaseItem() {
            Tags = new List<string>();
        }

        public PoeBaseItem(PoeBaseItem item) : this() {
            Class = item.Class;
            BaseName = item.BaseName;
            Height = item.Height;
            Width = item.Width;
            Position = item.Position;
            Tags = item.Tags;
        }

    }
}
