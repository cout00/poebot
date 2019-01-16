using System;
using System.Windows.Forms;
using WindowsFormsApplication2.Parsers.ItemParser.ItemBuilder;

namespace WindowsFormsApplication2.Parsers.ItemParser {
    public class PoeItem : PoeBaseItem {
        public int SocketsCount { get; set; }
        public ItemRarity Rarity { get; set; }
        public string Name { get; set; }
        PoeItem(string baseName) : base(ItemBasePreloader.GetItem(baseName)) {

        }

        public static IItem CreateItem() {
            try {
                if (Clipboard.ContainsText()) {
                    PoeItemParser poeItemParser = new PoeItemParser();
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
