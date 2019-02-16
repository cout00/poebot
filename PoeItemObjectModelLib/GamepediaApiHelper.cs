using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

using PoeItemObjectModelLib.Bases;
using System.Reflection;

namespace PoeItemObjectModelLib {
    public static class GamepediaApiHelper {
        

        public static StringBuilder GetRequestData(string itemClass) {
            string requeststr =
                $@"https://pathofexile.gamepedia.com/api.php?action=cargoquery&=&tables=items&fields=name%2Cclass_id%2Crarity%2Csize_x%2Csize_y&where=items.class_id=%22{itemClass.Coerce()}%22&limit=499&group_by=name&format=xml";
            WebRequest request = WebRequest.Create(requeststr);
            request.ContentType = "application/xml; charset=utf-8";
            WebResponse response = request.GetResponse();
            StringBuilder stringBuilder = new StringBuilder();
            using (Stream stream = response.GetResponseStream()) {
                using (StreamReader reader = new StreamReader(stream)) {
                    string line = "";
                    while ((line = reader.ReadLine()) != null) {
                        stringBuilder.AppendLine(line);
                    }
                }
            }
            response.Close();
            return stringBuilder;
        }
        
        public static void CreateXmlDocument() {
            List<PoePreloadedItem> poePreloadedItems = new List<PoePreloadedItem>();
            foreach (FieldInfo field in typeof(GamepediaApiHelper).
                GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).
                Where(fi => fi.IsLiteral && !fi.IsInitOnly)) {
                poePreloadedItems.AddRange(GetItems(GetRequestData(field.GetRawConstantValue().ToString())));
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PoePreloadedItem>));
            using (Stream stream = File.Create("XmlResource.xml")) {
                xmlSerializer.Serialize(stream, poePreloadedItems);
            }
        }

        
        public static IEnumerable<PoePreloadedItem> GetItems(StringBuilder xml) {
            using (MemoryStream memoryStream = new MemoryStream()) {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream)) {
                    streamWriter.Write(xml);
                    streamWriter.Flush();
                    memoryStream.Position = 0;
                    List<PoePreloadedItem> result = new List<PoePreloadedItem>();
                    XDocument document = XDocument.Load(memoryStream);
                    foreach (XElement xElement in document.Root.Elements()) {
                        foreach (XElement element in xElement.Elements()) {
                            PoePreloadedItem poePreloadedItem = new PoePreloadedItem();
                            var atts = element.Element("title").Attributes();
                            poePreloadedItem.Rarity = atts.Where(a => a.Name == "rarity").FirstOrDefault().Value;
                            poePreloadedItem.Class_ID = atts.Where(a => a.Name == "_class.20.id").FirstOrDefault().Value.Coerce();
                            poePreloadedItem.Name = atts.Where(a => a.Name == "name").FirstOrDefault().Value;
                            poePreloadedItem.Size_X = atts.Where(a => a.Name == "_size.20.x").FirstOrDefault().Value.ToInt();
                            poePreloadedItem.Size_Y = atts.Where(a => a.Name == "_size.20.y").FirstOrDefault().Value.ToInt();
                            result.Add(poePreloadedItem);
                        }
                    }
                    return result;
                }
            }
        }

    }
}
