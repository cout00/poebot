using System;

using PoeItemObjectModelLib.Bases;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PoeItemObjectModelLib {
    
    public static class ItemBasePreloader {

        static Dictionary<string, PoePreloadedItem> ItemBaseStorage = new Dictionary<string, PoePreloadedItem>();
        static bool loaded = false;

        public static void Load() {
            loaded = true;
            XmlSerializer ser = XmlSerializer.FromTypes(new[] { typeof(List<PoePreloadedItem>) })[0];
            FileStream fs = new FileStream("XmlResource.xml", FileMode.Open);
            var result = ((List<PoePreloadedItem>)ser.Deserialize(fs));
            fs.Close();
            foreach (var item in result) {
                if (ItemBaseStorage.ContainsKey(item.Name))
                    continue;
                ItemBaseStorage.Add(item.Name, item);
            }
        }        
        public static PoePreloadedItem GetItem(string baseName) {
            if(!loaded)
                throw new Exception("ItemsNotLoaded do it");            
            PoePreloadedItem outPut = null;
            ItemBaseStorage.TryGetValue(baseName, out outPut);
            return outPut;
        }        
    }
}
