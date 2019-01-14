using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.Parsers.ItemParser.ItemBuilder {






    interface IItemHeader {
        ItemRarity Rarity { get; set; }
        string BaseName { get; set; }
        string Name { get; set; }
    }

    interface ICurrencyGlobalParams {
        int Count { get; set; }
    }

    interface IMapGlobalParams {
        int Tier { get; set; }
        int ItemQuantity { get; set; }
        int ItemRarity { get; set; }
        int PackSize { get; set; }
    }

    interface IMapSuffixAndPrefixBody {
        List<string> PreffixAndSuffixes { get; set; }
    }

}
