using PoeItemObjectModelLib.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using PoeItemObjectModelLib.Elements;

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

    public class SpecificUniqueItemEvualator :Filter<IItemBaseHeader> {
        protected override bool Evualate(IItemBaseHeader item) {
            return Elements.Any(a => a.Rarity == ItemRarity.Unique && a.BaseName == item.BaseName);
        }
    }


    public class Pickit {

        public ICollection<PickitFilter> PickitFilters { get; }

        public Pickit() {
            PickitFilters = new List<PickitFilter>();

            PickitFilter divCardFilter = new PickitFilter();
            divCardFilter.Destination=Destination.Keep;
            divCardFilter.RarityEvualator.IsActive = false;
            ItemHeaderElement divCardHeader = new ItemHeaderElement();
            divCardHeader.Class = ItemClass.DivinationCard;
            divCardFilter.ItemClassEvualator.Elements.Add(divCardHeader);
            divCardFilter.ItemClassEvualator.IsActive = true;


            PickitFilter mapPickitFilter = new PickitFilter();
            mapPickitFilter.Destination = Destination.Keep;
            mapPickitFilter.RarityEvualator.IsActive = false;
            ItemHeaderElement mapHeader = new ItemHeaderElement();
            mapHeader.Class = ItemClass.DivinationCard;
            mapPickitFilter.ItemClassEvualator.Elements.Add(mapHeader);
            mapPickitFilter.ItemClassEvualator.IsActive = true;

            PickitFilter uniqueItemFilter=new PickitFilter();
            uniqueItemFilter.Destination=Destination.Keep;
            uniqueItemFilter.RarityEvualator.IsActive = true;
            ItemHeaderElement uniqueItemElement=new ItemHeaderElement();
            uniqueItemElement.Rarity=ItemRarity.Unique;
            uniqueItemFilter.RarityEvualator.Elements.Add(uniqueItemElement);

            PickitFilter currencyItemFilter=new PickitFilter();
            currencyItemFilter.Destination=Destination.Keep;
            currencyItemFilter.RarityEvualator.IsActive = true;
            ItemHeaderElement currencyElement = new ItemHeaderElement();
            currencyElement.Rarity = ItemRarity.Currency;
            currencyItemFilter.RarityEvualator.Elements.Add(currencyElement);

            PickitFilters.Add(divCardFilter);
            PickitFilters.Add(uniqueItemFilter);
            PickitFilters.Add(currencyItemFilter);
            PickitFilters.Add(mapPickitFilter);
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
                    var itemHeader = new ItemHeaderElement().ParseElement(elements.FirstOrDefault());
                    var itemSize =new ItemSizeElement().ParseElement(itemHeader.BaseName);
                    var itemStatus=new ItemElementParser().ParseElement(elements.Last());
                    if (itemHeader.Class == ItemClass.Currency) {
                        var stackedItemElement = new StackedItemElement().ParseElement(elements[1]);
                        return new Currency()
                            .Assign(stackedItemElement)
                            .Assign(itemHeader)
                            .Assign(itemSize)
                            .Assign(itemStatus);
                    }
                    if (itemHeader.Class == ItemClass.DivinationCard) {
                        var stackedItemElement = new StackedItemElement().ParseElement(elements[1]);
                        return new DivinationCard()
                            .Assign(stackedItemElement)
                            .Assign(itemHeader)
                            .Assign(itemSize)
                            .Assign(itemStatus);
                    }
                    if (itemHeader.Class.IsGem()) {
                        var qualityElement = new QualtityElement().ParseElement(elements[1]);
                        return new Gem()
                            .Assign(qualityElement)
                            .Assign(itemHeader)
                            .Assign(itemSize)
                            .Assign(itemStatus);
                    }
                    if (itemHeader.Class.IsArmor()) {
                        var qualityElement = new QualtityElement().ParseElement(elements[1]);
                        var socketedElement = new SocketedItemElement().ParseElement(elements[3]);
                        return new Armor()
                            .Assign(qualityElement)
                            .Assign(socketedElement)
                            .Assign(itemHeader)
                            .Assign(itemSize)
                            .Assign(itemStatus);
                    }

                    if (itemHeader.Class.IsWeapon()) {
                        var qualityElement = new QualtityElement().ParseElement(elements[1]);
                        var socketedElement = new SocketedItemElement().ParseElement(elements[3]);
                        return new Weapons()
                            .Assign(qualityElement)
                            .Assign(socketedElement)
                            .Assign(itemHeader)
                            .Assign(itemSize)
                            .Assign(itemStatus);
                    }
                    if (itemHeader.Class==ItemClass.Map) {

                    }
                    return new PoeAnyItem().
                        Assign(itemHeader)
                        .Assign(itemSize)
                        .Assign(itemStatus);

                } else {
                    return null;
                }
            }
            catch {
                return null;
            }
        }
    }


    public abstract class ItemModel :IItemBaseHeader, IItemSize, IItem {
        public string BaseName { get; set; }

        public string Class { get; set; }

        public ItemRarity Rarity { get; set; }

        public int SizeX { get; set; }

        public int SizeY { get; set; }

        public ItemStatus Status { get; set; }

        public ItemModel Assign<T>(T instance) where T : IElement {
            var interfaces = GetType().GetInterfaces().FirstOrDefault(a => a.FullName == typeof(T).FullName);
            if (interfaces == null)
                throw new ArgumentException();
            var targetMap = GetType().GetInterfaceMap(typeof(T));
            var sourceMap = instance.GetType().GetInterfaceMap(typeof(T));
            foreach (MethodInfo sourceMapTargetMethod in sourceMap.TargetMethods) {
                if (sourceMapTargetMethod.Name.Contains("get_")) {
                    var setterName = "set_" + sourceMapTargetMethod.Name.Replace("get_", string.Empty);
                    var result = sourceMapTargetMethod.Invoke(instance, null);
                    var setter = targetMap.TargetMethods.FirstOrDefault(a => a.Name == setterName);
                    setter.Invoke(this, new[] { result });
                }
            }
            return this;
        }
    }

    public class PoeAnyItem : ItemModel {
        
    }

    
    class DivinationCard :ItemModel, IDivinationCard {
        public int MaxStackSize { get; set; }
        public int StackSize { get; set; }
    }

    class Gem :ItemModel, IGem {
        public int Quality { get; set; }
    }

    class Armor :ItemModel, IArmor {
        public int Quality { get; set; }

        public int SocketsCount { get; set; }
    }

    class Weapons :ItemModel, IWeapon {
        public int Quality { get; set; }

        public int SocketsCount { get; set; }
    }

    public interface IStackedItem :IElement {
        int StackSize { get; set; }
        int MaxStackSize { get; set; }
    }

    public interface ICurrency :IStackedItem {

    }

    public interface IDivinationCard :IStackedItem {

    }

    public interface IElement {

    }

    public interface IItemSize :IElement {
        int SizeX { get; set; }
        int SizeY { get; set; }
    }

    public interface IItem :IElement {
        ItemStatus Status { get; set; }
    }

    public interface IItemBaseHeader :IElement {
        string BaseName { get; set; }
        ItemRarity Rarity { get; set; }
        string Class { get; set; }
    }

    public interface ISocketedItem :IElement {
        int SocketsCount { get; set; }
    }

    public interface IQualityItem :IElement {
        int Quality { get; set; }
    }

    interface IWeapon : ISocketedItem, IQualityItem {

    }

    public interface IGem : IQualityItem {
        
    }

    interface IArmor :ISocketedItem, IQualityItem {

    }
    
    interface IBaseMap :IElement {
        int Tier { get; set; }
    }



}
