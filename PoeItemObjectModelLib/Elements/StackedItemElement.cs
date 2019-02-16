using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PoeItemObjectModelLib.Elements {
    public class StackedItemElement :IElementParser<IStackedItem>, IStackedItem {
        public int MaxStackSize { get; set; }

        public int StackSize { get; set; }

        public IStackedItem ParseElement(string data) {            
            var targetData = Regex.Replace(data, "Stack Size: ", string.Empty);
            var splitedData = Regex.Split(targetData, "/");
            StackSize = int.Parse(splitedData[0]);
            MaxStackSize = int.Parse(splitedData[1]);
            return this;
        }
    }
}
