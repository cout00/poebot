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
                    var elements = Regex.Split(text, "--------").RemoveEmpty().ToList();
                    ItemHeader itemHeader = new ItemHeader();
                    itemHeader.Parse(elements);
                    if (itemHeader.Rarity != ItemRarity.Normal) {
                        ExtendedItemHeader extendedItemHeader = new ExtendedItemHeader(itemHeader);
                        extendedItemHeader.Parse(elements);
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

    class ItemHeader : IItemBaseHeader, IParser {
        public BaseNames BaseName { get; set; }
        public ItemClass Class { get; set; }
        public ItemStatus Status { get; set; }
        public ItemRarity Rarity { get; set; }                
        
        public virtual IItem GetItem() {
            if(Class==ItemClass.Currency) {
                Currency currency=new Currency(this);
                currency.Parse();
                return currency;
            }

            if(Class.IsArmor()) {
                
            }
        }

        public ItemHeader(IItemBaseHeader itemHeader) {
            BaseName = itemHeader.BaseName;
            Class = itemHeader.Class;
            Status = itemHeader.Status;
            Rarity = itemHeader.Rarity;
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

        public virtual void Parse(IEnumerable<string> rawData) {
            ParseItemStatus(rawData.Last());
            var headerElements = Regex.Split(rawData.First(), Environment.NewLine).RemoveEmpty();
            Rarity = Regex.Replace(headerElements.First(), "Rarity: ", "").ToRarity();
            BaseName = headerElements.Last().ToBaseName();
            var outItem = ItemBasePreloader.GetItem(BaseName);
            Class = outItem == null ? ItemClass.Debug : outItem.Class;                        
        }
    }

    class ExtendedItemHeader : ItemHeader, IItemHeader {

        public ExtendedItemHeader(IItemBaseHeader itemHeader):base(itemHeader) {
            
        }

        public ExtendedItemHeader(IItemHeader itemHeader):base(itemHeader) {
            Name = itemHeader.Name;
        }

        public string Name { get; set; }

        public override void Parse(IEnumerable<string> rawdata) {            
            var headerElements = Regex.Split(rawdata.First(), Environment.NewLine).RemoveEmpty().ToList();
            Name = headerElements[1];
        }
    }


    class Currency : ItemHeader, ICurrency {

        public int MaxStackSize { get ; set ; }
        public int StackSize { get ; set; }

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

    public interface ICurrency : IItemBaseHeader {
        int MaxStackSize { get; set; }
        int StackSize { get; set; }
    }

    public interface IParser {
        IItem GetItem();
    }
    
    public interface IItem {
        ItemStatus Status { get; set; }
        void Parse(IEnumerable<string> rawData);
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




    interface IBaseArmor : IItemBaseHeader, ISocketedItem {

    }

    interface IArmor : IItemHeader, ISocketedItem {

    }

    interface IMap : IBaseMap, IItemHeader {
        int ItemQuantity { get; set; }
        int ItemRarity { get; set; }
        int PackSize { get; set; }
    }

    interface IBaseMap : IItemBaseHeader {
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
