using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PoeItemObjectModelLib.PickitEngine.PickitStatistic;

namespace PoeItemObjectModelLib.PickitEngine {
    [Obsolete("do not use it")]
    public static class TempPickit {
        public static Pickit Pickit=new Pickit();
        public static ItemFactory Factory = new ItemFactory();
        public static StatisticProcessor StatisticProcessor = new StatisticProcessor();

        public static bool Validate() {
            var item = Factory.GetModel();
            if(item!=null) {
                StatisticProcessor.Add(item, Destination.Keep);
                return Pickit.IsValid(item);
            }
            return false;
        }

    }
}
