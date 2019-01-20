using PoeItemObjectModelLib.Bases;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PoeItemObjectModelLib {

    #region TempShit
    //var classes = items.Select(a => a.BaseName).Distinct().ToList();
    //StringBuilder stringBuilder = new StringBuilder();
    //foreach (var item in classes) {
    //    var newItem = item.Replace(" ", "_").Replace("\'", "__").Replace("-", "___");
    //    stringBuilder.AppendLine($"{newItem},");
    //}
    //StreamWriter textWriter = File.CreateText("F:\\result.txt");
    //textWriter.WriteLine(stringBuilder);
    //textWriter.Close();
    //foreach (var baseItem in items) {
    //    ItemBaseStorage.Add(baseItem.BaseName, baseItem);
    //}

    //var items = Preload();
    //foreach (var item in poeMaps) {
    //    items.Add(new PoePreloadedItem() { BaseName = item.Name, Class = "Map", Height = 1, Width = 1 });
    //}
    //public class PoeMap {
    //    public string Name { get; set; }
    //    public int Tier { get; set; }
    //    public int Level { get; set; }
    //}




    //TODO Exception
    //XmlDocument document = new XmlDocument();
    ////document.Load(@"F:\convertjson.xml");
    ////var root = document.DocumentElement;
    //List<PoePreloadedItem> rawItems = new List<PoePreloadedItem>();
    //        //foreach (XmlNode items in root) {
    //        //    PoePreloadedItem poeItem = new PoePreloadedItem();
    //        //    var addtoList = true;
    //        //    foreach (XmlNode item in items.ChildNodes) {
    //        //        if (item.Name == "release_state" && item.InnerText == "unreleased")
    //        //            addtoList = false;

    //        //        if (item.Name == "item_class") {
    //        //            if (item.InnerText == "StackableCurrency") {
    //        //                poeItem.Class = "Currency";
    //        //            }
    //        //            else
    //        //                poeItem.Class = item.InnerText;
    //        //        }

    //        //        if (item.Name == "name")
    //        //            poeItem.BaseName = item.InnerText;
    //        //        if (item.Name == "inventory_height")
    //        //            poeItem.Height = int.Parse(item.InnerText);

    //        //        if (item.Name == "inventory_width")
    //        //            poeItem.Width = int.Parse(item.InnerText);

    //        //    }
    //        //    if (addtoList) {
    //        //        rawItems.Add(poeItem);
    //        //    }
    //        //}

    //        return rawItems;


    //    TextReader textReader = File.OpenText(@"F:\\maps.txt");
    //    string line;
    //    int lineNumber = 0;
    //    List<PoeMap> poeMaps = new List<PoeMap>();
    //    PoeMap poeMap = new PoeMap();
    //            while ((line = textReader.ReadLine()) != null) {
    //                if (lineNumber % 2 == 1) {
    //                    var res = line.Split(' ');
    //    poeMap.Tier = int.Parse(res[1]);
    //    poeMap.Level = int.Parse(res[2]);
    //    poeMaps.Add(poeMap);
    //                    poeMap = new PoeMap();
    //}
    //                if (lineNumber % 2 == 0) {
    //                    poeMap.Name = line;
    //                }
    //                lineNumber++;
    //            }




    #endregion

    public static class ItemBasePreloader {

        static Dictionary<BaseNames, PoePreloadedItem> ItemBaseStorage = new Dictionary<BaseNames, PoePreloadedItem>();

        static ItemBasePreloader() {            
            XmlSerializer ser = new XmlSerializer(typeof(List<PoePreloadedItem>));
            FileStream fs = new FileStream("ItemData.xml", FileMode.Open);
            var result = ((List<PoePreloadedItem>)ser.Deserialize(fs));
            fs.Close();
            foreach (var item in result) {
                if (ItemBaseStorage.ContainsKey(item.BaseName))
                    continue;                
                ItemBaseStorage.Add(item.BaseName, item);
            }
        }

        public static PoePreloadedItem GetItem(BaseNames baseName) {
            PoePreloadedItem outPut = null;
            ItemBaseStorage.TryGetValue(baseName, out outPut);
            return outPut;
        }        
    }
}
