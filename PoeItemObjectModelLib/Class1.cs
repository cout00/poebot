using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PoeItemObjectModelLib
{
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

    //class PoeParser {
    //    IItem Parse(string str) {
            
    //    }
    //}


    public interface IItemParser<T> where T:IItem {
        void Parse(string rawdata, T item);
    }

    public class ItemParser:PoeItemParser, IItemParser<IItem> {
        public void Parse(string rawdata, IItem item) {
            item.ItemStatus=ItemStatus.Corrupted;            
        }

        public void DoParse(string rawData) {

            var sections = Regex.Split(rawData, "--------");
            if (sections.Any()) {
                PoeItem poeItem = new PoeItem();
                Parse(rawData, poeItem);
                GetNextParser<IItemBaseHeader>().Parse();
                
            }            
        }

        protected override IItemParser<IItemBaseHeader> GetNextParser<IItemBaseHeader>() {
            return new ItemBaseHeaderParser() as IItemParser<IItemBaseHeader>;
        }
    }

    public abstract class PoeItemParser {
        protected abstract IItemParser<T> GetNextParser<T>() where T : IItem;




        //public IItem Parse(string rawData) {

        //}
    }

    public class ItemBaseHeaderParser:PoeItemParser,IItemParser<IItemBaseHeader> {
        public void Parse(string rawdata, IItemBaseHeader item) {
            throw new NotImplementedException();
        }

        protected override IItemParser<T> GetNextParser<T>() {
            throw new NotImplementedException();
        }
    }


    public interface IItem {
        ItemStatus ItemStatus { get; set; }
    }

    public interface IItemBaseHeader :IItem {
        string BaseName { get; set; }
        ItemRarity Rarity { get; set; }
        string Class { get; set; }
    }

    public interface IItemHeader :IItemBaseHeader {
        string Name { get; set; }
    }

    public interface ICurrencyGlobalParams :IItem {
        int Count { get; set; }
    }

    interface IMapGlobalParams :IItem {
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



    interface IMapSuffixAndPrefixBody :IItem {
        List<string> PreffixAndSuffixes { get; set; }
    }

    class PoeItem :IItem {
        public ItemStatus ItemStatus { get; set; }
    }

    class ItemBase :PoeItem {
        public string BaseName { get; set; }
        public string Class { get; set; }
        public ItemStatus ItemStatus { get; set; }
        public ItemRarity Rarity { get; set; }                
    }


    class Map :ItemBase, IMapGlobalParams {
        public int Tier { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemRarity { get; set; }
        public int PackSize { get; set; }
    }

    class RarityMap :Map, IMapSuffixAndPrefixBody, IItemHeader {
        public List<string> PreffixAndSuffixes { get; set; }
        public string Name { get; set; }
    }
}
