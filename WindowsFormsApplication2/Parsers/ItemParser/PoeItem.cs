using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2.Parsers.ItemParser {
    public class PoeItem : PoeBaseItem {
        public int SocketsCount { get; set; }
        public ItemRarity Rarity { get; set; }
        public string Name { get; set; }
        PoeItem(string baseName) : base(ItemBasePreloader.GetItem(baseName)) {

        }

        public static PoeItem CreateItem() {
            try {
                if (Clipboard.ContainsText()) {
                    var text = Clipboard.GetText();
                    ///////
                    return null;
                }
                else {
                    return null;
                }
            }
            catch (Exception) {
                return null;
            }


        }

    }
}
