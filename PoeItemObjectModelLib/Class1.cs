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
        Gem
    }


    public class ItemFactory {

        public IItem GetModel() {
            try {
                if (Clipboard.ContainsText()) {
                    var text = Clipboard.GetText();
                    ItemHeader itemHeader = new ItemHeader();
                    itemHeader.Parse(text);
                    if (itemHeader.Rarity != ItemRarity.Normal) {
                        ExtendedItemHeader extendedItemHeader = new ExtendedItemHeader(itemHeader);
                        extendedItemHeader.Parse(text);
                        return extendedItemHeader.GetItem();
                    }
                    return itemHeader.GetItem();
                }
                else {
                    return null;
                }
            }
            catch (Exception) {
                return null;
            }
        }

    }

    class ItemHeader : IItemBaseHeader, IParser, IChainElement {
        public BaseNames BaseName { get; set; }
        public ItemClass Class { get; set; }
        public ItemStatus Status { get; set; }
        public ItemRarity Rarity { get; set; }
        public IEnumerable<ItemClass> TriggerOn { get; set; }
        public virtual IChainElement NextElement {
            get {
                var element = new Currency(this);
                element.Data = Data;
                return element;
            }
        }

        public virtual IEnumerable<string> Data { get; set ; }

        public virtual IItem GetItem() {
            

        }

        public ItemHeader(IItemBaseHeader itemHeader) {

        }

        public ItemHeader() {

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

        public virtual void Parse(string rawData) {
            var elements = Regex.Split(rawData, "--------").RemoveEmpty().ToList();
            ParseItemStatus(elements.Last());
            var headerElements = Regex.Split(elements.First(), Environment.NewLine).RemoveEmpty();
            Rarity = Regex.Replace(headerElements.First(), "Rarity: ", "").ToRarity();
            BaseName = headerElements.Last().ToBaseName();
            var outItem = ItemBasePreloader.GetItem(BaseName);
            Class = outItem == null ? ItemClass.Debug : outItem.Class;
            elements.Remove(elements.First());
            elements.Remove(elements.Last());
            Data = elements;

        }
    }

    class ExtendedItemHeader : ItemHeader, IItemHeader {

        public ExtendedItemHeader(IItemBaseHeader itemHeader):base(itemHeader) {
            
        }

        public ExtendedItemHeader(IItemHeader itemHeader):base(itemHeader) {
            Name = itemHeader.Name;
        }

        public string Name { get; set; }

        public override void Parse(string rawdata) {
            var elements = Regex.Split(rawdata, "--------").RemoveEmpty();
            var headerElements = Regex.Split(elements.First(), Environment.NewLine).RemoveEmpty().ToList();
            Name = headerElements[1];
        }
    }


    class Currency : ItemHeader, ICurrency {
        public override IChainElement NextElement => base.NextElement;

        public int MaxStackSize { get ; set ; }
        public int StackSize { get ; set; }

        public Currency(IItemBaseHeader itemHeader) : base(itemHeader) {

        }

        public override void Parse(string rawData) {
            base.Parse(rawData);
        }

    }



    public interface ICurrency : IItemBaseHeader, IChainElement {
        int MaxStackSize { get; set; }
        int StackSize { get; set; }

    }


    public interface IParser {
        IItem GetItem();
    }

    public interface IChainElement {
        IEnumerable<ItemClass> TriggerOn { get; set; }
        IChainElement NextElement { get; }
        IEnumerable<string> Data { get; set; }
    }

    public interface IItem {
        ItemStatus Status { get; set; }
        void Parse(string rawData);
    }

    public interface IItemBaseHeader : IItem {
        BaseNames BaseName { get; set; }
        ItemRarity Rarity { get; set; }
        ItemClass Class { get; set; }
    }

    public interface IItemHeader : IItemBaseHeader {
        string Name { get; set; }
    }

    interface ISocketedItem {
        int SocketsCount { get; set; }
    }




    interface IBaseArmor : IItemBaseHeader, ISocketedItem, IChainElement {

    }

    interface IArmor : IItemHeader, ISocketedItem, IChainElement {

    }

    interface IMap : IBaseMap, IItemHeader {
        int ItemQuantity { get; set; }
        int ItemRarity { get; set; }
        int PackSize { get; set; }
    }

    interface IBaseMap : IItemBaseHeader, IChainElement {
        int Tier { get; set; }
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
