using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

using PoeItemObjectModelLib.Bases;

namespace PoeItemObjectModelLib.PickitEngine.PickitStatistic {
    public class ItemStatisticInfo {
        public Destination ItemDestination { get; }
        public DateTime Date { get; }
        public string ItemClass { get; }
        public string ItemName { get; }

        public ItemStatisticInfo(Destination itemDestination, DateTime date, string itemClass, string itemName) {
            ItemDestination = itemDestination;
            Date = date;
            ItemClass = itemClass;
            ItemName = itemName;
        }

        public override string ToString() {
            return $"{Date.ToString("F")} - {ItemDestination} {ItemName}({ItemClass}) ";
        }
    }

    public class StatisticTotalInfo<T> where T:IItem {
        public int Count { get; set; }
        public string ItemName { get; set; }
        public string ItemClass { get; set; }
        public T Item { get; set; }

        public override string ToString() {
            return $"{ItemName}({ItemClass}) - {Count}";
        }
    }

    
    public abstract class StatisticBuilder<T> where T : IItem {
        protected List<StatisticTotalInfo<T>> Infos = new List<StatisticTotalInfo<T>>();
        public Dictionary<T, int> Looted = new Dictionary<T, int>();

        public void Add(T item) {
            InternalAdd(item);
            Looted.Clear();
            SetLoot();
        }

        protected abstract void InternalAdd(T item);
        protected abstract void SetLoot();
        protected abstract StringBuilder GetStringLoot();

        public StringBuilder MorphString() {
            if (!Infos.Any()) {
                return new StringBuilder();
            }
            return GetStringLoot();
        }
    }




    public class UniqueItemsStatisticBuilder :StatisticBuilder<ItemModel> {
        protected override void InternalAdd(ItemModel item) {
            Infos.Add(new StatisticTotalInfo<ItemModel>() { Item = item, ItemClass = item.Class, Count = 1, ItemName = item.BaseName });
        }

        protected override StringBuilder GetStringLoot() {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Looted) {
                stringBuilder.AppendLine($"{item.Key.Rarity} : {item.Value}");
            }
            return stringBuilder;
        }

        protected override void SetLoot() {
            var groups = Infos.GroupBy(a => a.Item.Rarity).Select(a => {                
                return new { Item = a.FirstOrDefault().Item, Count = a.Count() };
            });
            foreach (var item in groups) {
                Looted[item.Item] = item.Count;
            }
        }
    }

    public class CurrencyStatisticBuilder : StackItemsStatisticBuilder<Currency> {
        
    }

    public class DivCardStatisticBuilder :StackItemsStatisticBuilder<DivinationCard> {

    }

    public class StackItemsStatisticBuilder<StackItem> :StatisticBuilder<StackItem> where StackItem:ItemModel, IStackedItem {

        protected override void InternalAdd(StackItem item) {
            Infos.Add(new StatisticTotalInfo<StackItem>() { Item = item, ItemClass = item.Class, Count = item.StackSize, ItemName = item.BaseName });            
        }

        protected override StringBuilder GetStringLoot() {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Looted) {
                stringBuilder.AppendLine($"{item.Key.BaseName} : {item.Value}");
            }
            return stringBuilder;
        }

        protected override void SetLoot() {
            var groups = Infos.GroupBy(a => a.ItemName).Select(a => {
                var value = a.FirstOrDefault().Item;
                return new { Item = value, Count = a.Select(info => info.Item).Cast<IStackedItem>().Sum(stackedItem => stackedItem.StackSize) };
            });
            foreach (var item in groups) {
                Looted[item.Item] = item.Count;
            }
        }
    }

    public class StatisticProcessor {


        public event EventHandler<StatisticProcessor> OnData; 

        public List<ItemStatisticInfo> AllItems = new List<ItemStatisticInfo>();

        public UniqueItemsStatisticBuilder UniqueItemsStatistic=new UniqueItemsStatisticBuilder();
        public DivCardStatisticBuilder DivCardStatistic=new DivCardStatisticBuilder();
        public CurrencyStatisticBuilder CurrencyStatistic = new CurrencyStatisticBuilder();
        
        public void Add(ItemModel item, Destination destination) {
            AllItems.Add(new ItemStatisticInfo(destination, DateTime.Now, item.Class, item.BaseName));
            if(destination==Destination.Sell)
                return;                           
            if (item is Currency) 
                CurrencyStatistic.Add(item as Currency);            
            if(item.Rarity==ItemRarity.Unique)
                UniqueItemsStatistic.Add(item);            
            if (item.Class==ItemClass.DivinationCard) 
                DivCardStatistic.Add(item as DivinationCard);
            OnData?.Invoke(this, this);            
        }

    }

}
