using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WindowsFormsApplication2.Parsers.ItemParser {

    public enum ItemRarity {
        Common,
        Magic,
        Rare,
        Unique
    }

    public class ItemBasePreloader {

        static Dictionary<string, PoeBaseItem> ItemBaseStorage = new Dictionary<string, PoeBaseItem>();

        public ItemBasePreloader() {
            var items = Preload();
            foreach (var baseItem in items) {
                ItemBaseStorage.Add(baseItem.BaseName, baseItem);
            }
        }

        public static PoeBaseItem GetItem(string baseName) {
            PoeBaseItem outPut = null;
            ItemBaseStorage.TryGetValue(baseName, out outPut);
            return outPut;
        }

        private static List<PoeBaseItem> Preload() {
            //TODO Exception
            XmlDocument document = new XmlDocument();
            document.Load(@"C:\Users\aizik\Downloads\convertjson.xml");
            var root = document.DocumentElement;
            List<PoeBaseItem> rawItems = new List<PoeBaseItem>();
            foreach (XmlNode items in root) {
                PoeBaseItem poeItem = new PoeBaseItem();
                var addtoList = true;
                foreach (XmlNode item in items.ChildNodes) {
                    if (item.Name == "release_state" && item.InnerText == "unreleased") 
                        addtoList = false;
                    
                    if (item.Name == "item_class") 
                        poeItem.Class = item.InnerText;
                    
                    if (item.Name == "name") 
                        poeItem.BaseName = item.InnerText;
                    
                    if (item.Name == "tags") 
                        poeItem.Tags.Add(item.InnerText);
                    
                    if (item.Name == "inventory_height") 
                        poeItem.Height = int.Parse(item.InnerText);
                    
                    if (item.Name == "inventory_width") 
                        poeItem.Width = int.Parse(item.InnerText);
                    
                }
                if (addtoList) {
                    rawItems.Add(poeItem);
                }
            }

            return rawItems;
        }
    }
}
