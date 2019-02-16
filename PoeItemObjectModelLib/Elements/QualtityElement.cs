using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PoeItemObjectModelLib {
    class QualtityElement :IQualityItem, IElementParser<IQualityItem> {
        public int Quality { get; set; }

        public IQualityItem ParseElement(string data) {
            try {
                var split = Regex.Split(data, Environment.NewLine);
                var targetLine = split.Where(a => a.Contains("Quality")).FirstOrDefault();
                var targetValue = targetLine.Replace("% (augmented)", string.Empty).Replace("Quality: +",
                    string.Empty);
                Quality = int.Parse(targetValue);
            }
            catch (Exception) {
            }
            return this;
        }
    }
}