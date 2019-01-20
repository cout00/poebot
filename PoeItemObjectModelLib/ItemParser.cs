using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoeItemObjectModelLib {

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





    public class PoeItem :PoeBaseItem {
        public int SocketsCount { get; set; }
        public ItemRarity Rarity { get; set; }
        public string Name { get; set; }
        PoeItem(string baseName) : base(ItemBasePreloader.GetItem(baseName)) {

        }

        public static IItem CreateItem() {
            try {
                if (Clipboard.ContainsText()) {
                    
                    ///////
                    return null;
                } else {
                    return null;
                }
            }
            catch (Exception) {
                return null;
            }


        }

    }






}
