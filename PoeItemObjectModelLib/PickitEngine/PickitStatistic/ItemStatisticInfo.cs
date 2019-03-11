using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeItemObjectModelLib.PickitEngine.PickitStatistic {
    public class ItemStatisticInfo {
        public Destination ItemDestination { get; set; }
        public DateTime Date { get; set; }
        public string ItemClass { get; set; }
        public string ItemName { get; set; }

        public override string ToString() {
            return $"{Date.ToString("F")} - {ItemDestination} {ItemName}({ItemClass}) ";
        }
    }

    public class StatisticTotalInfo:ItemStatisticInfo {
        public int Count { get; set; }

        public override string ToString() {
            return $"{ItemName}({ItemClass}) - {Count}";
        }
    }

    public class StatisticProcessor {
        public List<ItemStatisticInfo> AllItems=new List<ItemStatisticInfo>();
        public List<StatisticTotalInfo> TotalItem=new List<StatisticTotalInfo>();


        public void Add(IItem item) {
            
        }

    }

}
