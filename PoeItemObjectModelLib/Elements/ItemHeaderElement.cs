using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using PoeItemObjectModelLib.Bases;

namespace PoeItemObjectModelLib.Elements {
    public class ItemHeaderElement :IItemBaseHeader, IElementParser<IItemBaseHeader> {
        readonly ItemStatus itemStatus;
        const string REGEX = "of.+";
        const string REGEX2 = @"^[\w-]+ ";                
        public string BaseName { get; set; }

        public string Class { get; set; }

        public ItemRarity Rarity { get; set; }

        public ItemHeaderElement() {

        }

        public ItemHeaderElement(ItemStatus itemStatus) {
            this.itemStatus = itemStatus;
        }

        public IItemBaseHeader ParseElement(string data) {
            var headerElements = Regex.Split(data, Environment.NewLine).RemoveEmpty();
            Rarity = Regex.Replace(headerElements.First().Trim(), "Rarity: ", "").ToRarity();
            BaseName = Rarity == ItemRarity.Unique ? headerElements.ToList()[1] : headerElements.Last();
            if (Rarity==ItemRarity.Magic && itemStatus==ItemStatus.Identified) {
                BaseName = Regex.Replace(BaseName, REGEX, string.Empty).Trim();
                //BaseName = Regex.Replace(BaseName, REGEX2, string.Empty).Trim();
            }
            var outItem = ItemBasePreloader.GetItem(BaseName);
            Class = outItem == null ? ItemClass.None : outItem.Class_ID;
            return this;
        }
    }
}
