using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using PoeItemObjectModelLib.Bases;

namespace PoeItemObjectModelLib.Elements {
    public class ItemHeaderElement :IItemBaseHeader, IElementParser<IItemBaseHeader> {
        public string BaseName { get; set; }

        public string Class { get; set; }

        public ItemRarity Rarity { get; set; }
        
        public IItemBaseHeader ParseElement(string data) {
            var headerElements = Regex.Split(data, Environment.NewLine).RemoveEmpty();
            Rarity = Regex.Replace(headerElements.First(), "Rarity: ", "").ToRarity();
            BaseName = Rarity == ItemRarity.Unique ? headerElements.ToList()[1] : headerElements.Last();
            var outItem = ItemBasePreloader.GetItem(BaseName);
            Class = outItem == null ? ItemClass.None : outItem.Class_ID;
            return this;
        }
    }
}
