using System;
using PriceCollector.Core.Data.Enums;

namespace PriceCollector.Core.Data.Settings {
    [Serializable]
    public class ParseItem {
        public string ProductName { get; set; }
        public string SellerName { get; set; }
        public string Url { get; set; }
        public SelectorType SelectorType { get; set; }
   }
}