using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PoeItemObjectModelLib {
    public static class GamepediaApiHelper {
        public const string LifeFlask = "LifeFlask";
        public const string ManaFlask = "ManaFlask";
        public const string HybridFlask = "HybridFlask";
        public const string Amulet = "Amulet";
        public const string Ring = "Ring";
        public const string Claw = "Claw";
        public const string Dagger = "Dagger";
        public const string Wand = "Wand";
        public const string One_Hand_Sword = "One Hand Sword";
        public const string Thrusting_One_Hand_Sword = "Thrusting One Hand Sword";
        public const string One_Hand_Axe = "One Hand Axe";
        public const string One_Hand_Mace = "One Hand Mace";
        public const string Bow = "Bow";
        public const string Two_Hand_Sword = "Two Hand Sword";
        public const string Staff = "Staff";
        public const string Two_Hand_Axe = "Two Hand Axe";
        public const string Two_Hand_Mace = "Two Hand Axe";
        public const string Active_Skill_Gem = "Active Skill Gem";
        public const string Support_Skill_Gem = "Support Skill Gem";
        public const string Quiver = "Quiver";
        public const string Belt = "Belt";
        public const string Gloves = "Gloves";
        public const string Boots = "Boots";
        public const string Body_Armour = "Body Armour";
        public const string Helmet = "Helmet";
        public const string Shield = "Shield";
        public const string StackableCurrency = "StackableCurrency";
        public const string Sceptre = "Sceptre";
        public const string UtilityFlask = "UtilityFlask";
        public const string UtilityFlaskCritical = "UtilityFlaskCritical";
        public const string Map = "Map";
        public const string FishingRod = "FishingRod";
        public const string MapFragment = "MapFragment";
        public const string Jewel = "Jewel";
        public const string DivinationCard = "DivinationCard";
        public const string AbyssJewel = "AbyssJewel";
        public const string UniqueFragment = "UniqueFragment";

        public static StringBuilder GetRequestData(string itemClass) {
            string requeststr =
                $@"https://pathofexile.gamepedia.com/api.php?action=cargoquery&=&tables=items&fields=name%2Cclass_id%2Crarity%2Csize_x%2Csize_y&where=items.class_id=%22{itemClass}%22&limit=499&group_by=name&format=xml";
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
            var res = GamepediaApiHelper.GetRequestData(GamepediaApiHelper.Belt);
            var res2 = GamepediaApiHelper.GetItems(res);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PoePreloadedItem2>));
            using (Stream stream = File.Create("XmlResource")) {
                xmlSerializer.Serialize(stream, res2);
            }
        }

        public static IEnumerable<PoePreloadedItem2> GetItems(StringBuilder xml) {
            using (MemoryStream memoryStream = new MemoryStream()) {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream)) {
                    streamWriter.Write(xml);
                    streamWriter.Flush();
                    memoryStream.Position = 0;
                    List<PoePreloadedItem2> result = new List<PoePreloadedItem2>();
                    XDocument document = XDocument.Load(memoryStream);
                    foreach (XElement xElement in document.Root.Elements()) {
                        foreach (XElement element in xElement.Elements()) {
                            PoePreloadedItem2 poePreloadedItem2 = new PoePreloadedItem2();
                            var atts = element.Element("title").Attributes();
                            poePreloadedItem2.Rarity = atts.Where(a => a.Name == "rarity").FirstOrDefault().Value;
                            poePreloadedItem2.Class_ID = atts.Where(a => a.Name == "_class.20.id").FirstOrDefault().Value;
                            poePreloadedItem2.Name = atts.Where(a => a.Name == "name").FirstOrDefault().Value;
                            poePreloadedItem2.Size_X = atts.Where(a => a.Name == "_size.20.x").FirstOrDefault().Value;
                            poePreloadedItem2.Size_Y = atts.Where(a => a.Name == "_size.20.y").FirstOrDefault().Value;
                            result.Add(poePreloadedItem2);
                        }
                    }
                    return result;
                }
            }
        }

    }
}
