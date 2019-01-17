using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PoeItemObjectModelLib {

    public enum ItemStatus {
        Corrupted,
        Unidentified,
        Identified
    }

    public enum ItemRarity {
        Common,
        Magic,
        Rare,
        Unique
    }

    public static class PoeItemsExtensions {
        public static bool IsIdentified(this IItem self) {
            return self.ItemStatus == ItemStatus.Corrupted || self.ItemStatus == ItemStatus.Identified;
        }


    }

    //class PoeParser {
    //    IItem Parse(string str) {

    //    }
    //}


    public interface IItemParser<T> where T : IItem {
        T Parse(string rawdata);
    }

    public class ItemParser {

        public void DoParse(string rawData) {

            var sections = Regex.Split(rawData, "--------");
            if (sections.Any()) {

                PoeItemParser poeItemParser = new PoeItemParser();
                var item = poeItemParser.Parse(sections.Last());
                if (item.IsIdentified()) {

                }
                ItemBaseHeaderParser headerParser = new ItemBaseHeaderParser();
                headerParser.Parse(sections.First());
            }
        }

        //protected override IItemParser<IItemBaseHeader> GetNextParser<IItemBaseHeader>() {
        //    return new ItemBaseHeaderParser() as IItemParser<IItemBaseHeader>;
        //}
    }


    class PoeItemParser : IItemParser<IItem> {
        public IItem Parse(string rawdata) {
            throw new NotImplementedException();
        }
    }

    public class ItemBaseHeaderParser : IItemParser<IItemBaseHeader> {
        public IItemBaseHeader Parse(string rawdata) {
            throw new NotImplementedException();
        }
    }


    public interface IItem {
        ItemStatus ItemStatus { get; set; }
    }

    public interface IItemBaseHeader : IItem {
        string BaseName { get; set; }
        ItemRarity Rarity { get; set; }
        string Class { get; set; }
    }

    public interface IItemHeader : IItemBaseHeader {
        string Name { get; set; }
    }

    public interface ICurrency: IItemBaseHeader {

    }

    public interface IGem : IItemBaseHeader {

    }

    public interface IMap : IItemBaseHeader {

    }

    public interface IOneHand : IItemBaseHeader {

    }

    public interface IOneTwoHand : IItemBaseHeader {

    }

    public interface IBodyArmor : IItemBaseHeader {

    }

    public interface IHelmet : IItemBaseHeader {

    }

    public interface IGloves : IItemBaseHeader {

    }

    public interface IBoots : IItemBaseHeader {

    }

    public interface IShield : IItemBaseHeader {

    }

    public interface IAccesorie : IItemBaseHeader {

    }


    public interface ICurrencyGlobalParams : IItem {
        int Count { get; set; }
    }

    interface IMapGlobalParams : IItem {
        int Tier { get; set; }
        int ItemQuantity { get; set; }
        int ItemRarity { get; set; }
        int PackSize { get; set; }
    }

    //    Rarity: Rare
    //Loath Spiral
    //Coral Ring
    //--------
    //Requirements:
    //Level: 47
    //--------
    //Item Level: 62
    //--------
    //+21 to maximum Life
    //--------
    //+30 to Strength
    //Adds 11 to 24 Fire Damage to Attacks
    //Adds 1 to 27 Lightning Damage to Attacks
    //+133 to Accuracy Rating
    //+35 to maximum Energy Shield
    //+12 Life gained on Kill
    //--------
    //Corrupted
    //--------
    //Note: ~price 2 chaos

    interface IMapSuffixAndPrefixBody : IItem {
        List<string> PreffixAndSuffixes { get; set; }
    }

    class PoeItem : IItem {
        public ItemStatus ItemStatus { get; set; }
    }

    class ItemBase : PoeItem {
        public string BaseName { get; set; }
        public string Class { get; set; }
        public ItemRarity Rarity { get; set; }
    }


    class Map : ItemBase, IMapGlobalParams {
        public int Tier { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemRarity { get; set; }
        public int PackSize { get; set; }
    }

    class RarityMap : Map, IMapSuffixAndPrefixBody, IItemHeader {
        public List<string> PreffixAndSuffixes { get; set; }
        public string Name { get; set; }
    }
}
