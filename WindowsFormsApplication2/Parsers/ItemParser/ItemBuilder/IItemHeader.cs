using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.Parsers.ItemParser.ItemBuilder {



    public class ItemParser : IItemParser<IItem> {
        public IItem Parse(string rawData) {
            throw new NotImplementedException();
        }
    }

    public class ItemBaseHeaderParser : IItemParser<IItemBaseHeader> {
        public IItemBaseHeader Parse(string rawData) {
            throw new NotImplementedException();
        }
    }



    interface IItemParser<TItem> where TItem:IItem {
        TItem Parse(string rawData);
    }

    public interface IItem {
        bool Itendified { get; set; }
    }

    public interface IItemBaseHeader : IItem {
        ItemRarity Rarity { get; set; }
        string BaseName { get; set; }        
    }

    public interface IItemHeader: IItemBaseHeader {
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

    interface IMapSuffixAndPrefixBody : IItem {
        List<string> PreffixAndSuffixes { get; set; }
    }

}
