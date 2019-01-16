using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.Parsers.ItemParser.ItemBuilder {



    public class PoeItemParser : IItemParser<IItem> {
        public IItem Parse(string rawData, IItem itemToParse) {
            var sections = Regex.Split(rawData, "--------");
            if (sections.Any()) {
                PoeItem poeItem = new PoeItem();
                var headerparser = new ItemBaseHeaderParser();
                return headerparser.Parse(rawData, )
            }
        }
    }

    public class ItemBaseHeaderParser : IItemParser<IItemBaseHeader> {
        public IItemBaseHeader Parse(string rawData, IItemBaseHeader itemToParse) {
            throw new NotImplementedException();
        }
    }



    interface IItemParser<TItem> where TItem : IItem {
        TItem Parse(string rawData, TItem itemToParse);
    }

    public interface IItem {
        bool IsCorrupted { get; set; }
    }

    public interface IItemBaseHeader : IItem {
        string BaseName { get; set; }
        ItemRarity Rarity { get; set; }
        bool Itendified { get; set; }
    }

    public interface IItemHeader : IItemBaseHeader {
        string Name { get; set; }        
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

    class ItemBase : IItemBaseHeader {
        public string BaseName { get ; set ; }
        public bool IsCorrupted { get ; set ; }
        public ItemRarity Rarity { get ; set ; }
        public bool Itendified { get; set ; }
    }


    class Map : ItemBase, IMapGlobalParams {
        public int Tier { get ; set ; }
        public int ItemQuantity { get ; set ; }
        public int ItemRarity { get ; set ; }
        public int PackSize { get ; set ; }
    }

    class RarityMap : Map, IMapSuffixAndPrefixBody, IItemHeader {
        public List<string> PreffixAndSuffixes { get ; set ; }
        public string Name { get ; set ; }
    }
}
