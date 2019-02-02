using PoeItemObjectModelLib.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PoeItemObjectModelLib {

    public enum ItemStatus {
        Debug,
        Corrupted,
        Unidentified,
        Identified
    }

    public enum ItemRarity {
        Debug,
        Normal,
        Magic,
        Rare,
        Unique,
        Currency,
        Gem,
        Divination_Card
    }

    public enum Destination {
        Keep,
        Sell,
    }

    interface IEvualator {
        bool Evualate(IItem item);
    }

    public class PickitFilter :IEvualator {        
        public int Order { get; set; }
        public string Title { get; set; }
        public Destination Destination { get; set; }

        public RarityEvualator RarityEvualator { get; }
        public ItemClassEvualator ItemClassEvualator { get; }
        public SpecificUniqueItemEvualator SpecificUniqueItemEvualator { get; }

        ICollection<IEvualator> Evualators = new List<IEvualator>();

        public PickitFilter() {
            RarityEvualator = new RarityEvualator();
            ItemClassEvualator = new ItemClassEvualator();
            SpecificUniqueItemEvualator = new SpecificUniqueItemEvualator();

            Evualators.Add(RarityEvualator);
            Evualators.Add(ItemClassEvualator);
            Evualators.Add(SpecificUniqueItemEvualator);
        }

        bool IEvualator.Evualate(IItem item) {
            var result = true;
            foreach (IEvualator evualator in Evualators)
                result = result && evualator.Evualate(item);
            return result;
        }
    }

    public abstract class Filter<T> :IEvualator where T : class, IElement {
        public bool IsActive { get; set; }
        public ICollection<T> Elements { get; }

        public Filter() {
            Elements = new List<T>();
        }

        protected abstract bool Evualate(T item);

        bool IEvualator.Evualate(IItem item) {
            if (!IsActive || !Elements.Any())
                return true;
            if (item is T) {
                return Evualate(item as T);
            }
            return false;
        }
    }

    public class QuantityEvualator :Filter<IQualityItem> {
        protected override bool Evualate(IQualityItem item) {
            return Elements.Any(a => a.Quality == item.Quality);
        }
    }

    public class SocketEvualator :Filter<ISocketedItem> {
        protected override bool Evualate(ISocketedItem item) {
            return Elements.Any(a => a.SocketsCount == item.SocketsCount);
        }
    }

    public class RarityEvualator :Filter<IItemBaseHeader> {
        protected override bool Evualate(IItemBaseHeader item) {
            return Elements.Any(a => a.Rarity == item.Rarity);
        }
    }

    public class ItemClassEvualator :Filter<IItemBaseHeader> {
        protected override bool Evualate(IItemBaseHeader item) {
            return Elements.Any(a => a.Class == item.Class);
        }
    }

    public class SpecificUniqueItemEvualator :Filter<IItemHeader> {
        protected override bool Evualate(IItemHeader item) {
            return Elements.Any(a => a.Rarity == ItemRarity.Unique && a.Name == item.Name);
        }
    }


    public class Pickit {

        public ICollection<PickitFilter> PickitFilters { get; }

        public Pickit() {
            PickitFilters = new List<PickitFilter>();
            PickitFilter filter = new PickitFilter();
            filter.RarityEvualator.IsActive = true;
            ItemHeader itemHeader = new ItemHeader();
            itemHeader.Class = ItemClass.Divination_Card;
            filter.ItemClassEvualator.Elements.Add(itemHeader);
            filter.ItemClassEvualator.IsActive = true;
            PickitFilters.Add(filter);
        }

        public bool IsValid(IItem item) {
            var result = false;
            foreach (IEvualator filter in PickitFilters) {
                result = result || filter.Evualate(item);
            }
            return result;
        }
    }

    public class ItemFactory {

        public IItem GetModel() {
            try {
                if (Clipboard.ContainsText()) {
                    var text = Clipboard.GetText();
                    var elements = Regex.Split(text, "--------").RemoveEmpty().ToList();
                    ItemHeader itemHeader = new ItemHeader();
                    itemHeader.Parse(elements);
                    if (itemHeader.Rarity.IsExtendedItem()) {
                        ExtendedItemHeader extendedItemHeader = new ExtendedItemHeader(itemHeader);
                        extendedItemHeader.Parse(elements);
                        return extendedItemHeader.GetItem();
                    }
                    return itemHeader.GetItem();
                } else {
                    return null;
                }
            }
            catch (Exception) {
                return null;
            }
        }
    }

    public class ElementContainer {

        List<object> Elements = new List<object>();

        public void AddElement<T>(IElementParser<T> element) {
            Elements.Add(element);
        }

        public IElementParser<T> GetElement<T>() {
            return Elements.Where(a => {
                var res = a as IElementParser<T>;
                return res != null;
            }).Select(a => a as IElementParser<T>).FirstOrDefault();
        }
    }


    public class ItemHeader :IItemBaseHeader, IParser {
        public BaseNames BaseName { get; set; }
        public ItemClass Class { get; set; }
        public ItemStatus Status { get; set; }
        public ItemRarity Rarity { get; set; }

        protected ElementContainer ElementContainer { get; }

        public virtual IItem GetItem() {
            if (Class == ItemClass.Currency) {
                Currency currency = new Currency(this);
                currency.Parse(rawData);
                return currency;
            }
            if (Class.IsGem()) {
                Gem gem = new Gem(this);
                gem.Parse(rawData);
                return gem;
            }
            if (Class == ItemClass.Divination_Card) {
                DivCard divCard = new DivCard(this);
                divCard.Parse(rawData);
                return divCard;
            }
            if (Class.IsArmor()) {

            }
            return null;
        }

        public ItemHeader(IItemBaseHeader itemHeader) : this() {
            BaseName = itemHeader.BaseName;
            Class = itemHeader.Class;
            Status = itemHeader.Status;
            Rarity = itemHeader.Rarity;
        }

        public ItemHeader() {
            ElementContainer = new ElementContainer();
            ElementContainer.AddElement(new QualtityElement());
            ElementContainer.AddElement(new SocketedItemElement());
        }

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

        protected IEnumerable<string> rawData;

        public virtual void Parse(IEnumerable<string> rawData) {
            this.rawData = rawData;
            ParseItemStatus(rawData.Last());
            var headerElements = Regex.Split(rawData.First(), Environment.NewLine).RemoveEmpty();
            Rarity = Regex.Replace(headerElements.First(), "Rarity: ", "").ToRarity();
            if (Rarity == ItemRarity.Divination_Card) {
                BaseName = BaseNames.Divination_Card;
                Class = ItemClass.Divination_Card;
                return;
            }
            BaseName = headerElements.Last().ToBaseName();
            var outItem = ItemBasePreloader.GetItem(BaseName);
            Class = outItem == null ? ItemClass.Debug : outItem.Class;
        }
    }

    public class QualityItem :ItemHeader, IQualityItem {

        public QualityItem(IItemBaseHeader itemHeader) : base(itemHeader) {

        }

        public override void Parse(IEnumerable<string> rawData) {
            var element = ElementContainer.GetElement<IQualityItem>();
            Quality = element.ParseElement(rawData.ToList()[1]).Quality;
        }

        public int Quality { get; set; }
    }

    public class ExtendedQualityItem :ExtendedItemHeader, IQualityItem {
        public ExtendedQualityItem(IItemHeader itemHeader) : base(itemHeader) {

        }

        public override void Parse(IEnumerable<string> rawData) {
            Quality = ElementContainer.GetElement<IQualityItem>().ParseElement(rawData.ToList()[1]).Quality;
        }

        public int Quality { get; set; }
    }

    public class ExtendedItemHeader :ItemHeader, IItemHeader {

        public ExtendedItemHeader(IItemBaseHeader itemHeader) : base(itemHeader) {

        }

        public ExtendedItemHeader(IItemHeader itemHeader) : base(itemHeader) {
            Name = itemHeader.Name;
        }

        public string Name { get; set; }

        public override void Parse(IEnumerable<string> rawdata) {
            base.rawData = rawData;
            var headerElements = Regex.Split(rawdata.First(), Environment.NewLine).RemoveEmpty().ToList();
            Name = headerElements[1];
        }
    }


    class Currency :ItemHeader, ICurrency {

        public int MaxStackSize { get; set; }
        public int StackSize { get; set; }

        public Currency(IItemBaseHeader itemHeader) : base(itemHeader) {

        }

        Currency() {

        }

        public override void Parse(IEnumerable<string> rawData) {
            var block = rawData.ToList()[1];
            var targetData = Regex.Replace(block, "Stack Size: ", string.Empty);
            var splitedData = Regex.Split(targetData, "/");
            StackSize = int.Parse(splitedData[0]);
            MaxStackSize = int.Parse(splitedData[1]);
        }
    }

    class Gem :QualityItem {
        public Gem(IItemBaseHeader itemHeader) : base(itemHeader) {

        }

    }

    class DivCard :ItemHeader {
        public DivCard(IItemBaseHeader itemHeader) : base(itemHeader) {

        }

        public override void Parse(IEnumerable<string> rawData) {

        }
    }


    public interface ICurrency :IItemBaseHeader {
        int MaxStackSize { get; set; }
        int StackSize { get; set; }
    }


    public interface IElement {

    }

    public interface IParser {
        IItem GetItem();
    }

    public interface IItem :IElement {
        ItemStatus Status { get; set; }
        void Parse(IEnumerable<string> rawData);
    }

    public interface IItemBaseHeader :IItem {
        BaseNames BaseName { get; set; }
        ItemRarity Rarity { get; set; }
        ItemClass Class { get; set; }
    }

    public interface IItemHeader :IItemBaseHeader {
        string Name { get; set; }
    }

    public interface ISocketedItem :IElement {
        int SocketsCount { get; set; }
    }

    public interface IQualityItem :IElement {
        int Quality { get; set; }
    }

    interface IBaseArmor :IItemBaseHeader, ISocketedItem, IQualityItem {

    }

    interface IArmor :IItemHeader, ISocketedItem, IQualityItem {

    }

    interface IMap :IBaseMap, IItemHeader {
        int ItemQuantity { get; set; }
        int ItemRarity { get; set; }
        int PackSize { get; set; }
    }

    interface IBaseMap :IItemBaseHeader {
        int Tier { get; set; }
    }

    public interface IElementParser<T> {
        T ParseElement(string data);
    }

    class QualtityElement :IQualityItem, IElementParser<IQualityItem> {
        public int Quality { get; set; }

        public IQualityItem ParseElement(string data) {
            var split = Regex.Split(data, Environment.NewLine);
            var targetLine = split.Where(a => a.Contains("Quality")).FirstOrDefault();
            var targetValue = targetLine.Replace("% (augmented)", string.Empty).Replace("Quality: +",
                string.Empty);
            Quality = int.Parse(targetValue);
            return this;
        }
    }

    class SocketedItemElement :ISocketedItem, IElementParser<ISocketedItem> {
        public int SocketsCount { get; set; }

        public ISocketedItem ParseElement(string data) {
            var line = data.Replace("Sockets: ", string.Empty);
            var res = line.Where(a => a.IsOneOf('R', 'G', 'B')).Count();
            SocketsCount = res;
            return this;
        }
    }

    ////    Rarity: Rare
    ////Loath Spiral
    ////Coral Ring
    ////--------
    ////Requirements:
    ////Level: 47
    ////--------
    ////Item Level: 62
    ////--------
    ////+21 to maximum Life
    ////--------
    ////+30 to Strength
    ////Adds 11 to 24 Fire Damage to Attacks
    ////Adds 1 to 27 Lightning Damage to Attacks
    ////+133 to Accuracy Rating
    ////+35 to maximum Energy Shield
    ////+12 Life gained on Kill
    ////--------
    ////Corrupted
    ////--------
    ////Note: ~price 2 chaos

}
