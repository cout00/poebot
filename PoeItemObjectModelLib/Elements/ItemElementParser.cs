using System.Text.RegularExpressions;

namespace PoeItemObjectModelLib.Elements {
    class ItemElementParser :IItem, IElementParser<IItem> {
        public ItemStatus Status { get; set; }

        void ParseItemStatus(string rawData) {
            if (Regex.Match(rawData, ItemStatus.Corrupted.ToString()).Value != string.Empty) {
                Status = ItemStatus.Corrupted;
                return;
            }
            if (Regex.Match(rawData, ItemStatus.Unidentified.ToString()).Value != string.Empty) {
                Status = ItemStatus.Unidentified;
                return;
            }
            Status = ItemStatus.Identified;
            return;
        }

        public IItem ParseElement(string data) {
            ParseItemStatus(data);
            return this;
        }
    }
}